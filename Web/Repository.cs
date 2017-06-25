using Newtonsoft.Json.Linq;
using PaJaMa.Data;
using System;
using System.Linq;
using System.Web;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;

namespace PaJaMa.Web
{
	public class Repository<TDbContext, TEntity> : IRepository
		where TEntity : class, IEntity
		where TDbContext : DbContextBase
	{
		protected TDbContext context { get; private set; }

		public DbContextBase Context { get { return context; } }

		public Repository()
		{
			createContext();
		}

		private void createContext()
		{
			context = Activator.CreateInstance<TDbContext>();
			// when authentication is used
			// context.ModifiedBy = HttpContext.Current.Request.LogonUserIdentity.Name;
		}

		public virtual IQueryable<TEntity> GetEntities()
		{
			return context.GetEntities<TEntity>();
		}

		public virtual IQueryable<TEntity> GetEntitiesOData(System.Net.Http.HttpRequestMessage request)
		{
			var modelBuilder = new ODataConventionModelBuilder();
			modelBuilder.EntitySet<TEntity>(typeof(TEntity).Name);
			var castedOptions = new ODataQueryOptions<TEntity>(new ODataQueryContext(modelBuilder.GetEdmModel(), typeof(TEntity))
				, request);
			var entities = GetEntities() as IQueryable<TEntity>;

			if (entities == null) return null;

			var filtered = castedOptions.ApplyTo(entities) as IQueryable<TEntity>;
			return filtered;
		}

		public virtual TEntity GetEntity(int id)
		{
			return context.GetEntity<TEntity>(id);
		}
		
		public virtual OperationResult InsertEntity(TEntity entity)
		{
			try
			{
				entity = context.InsertEntity<TEntity>(entity);
			}
			catch (Exception ex)
			{
				return new OperationResult()
				{
					Exception = ex,
					Failed = true,
					Entity = entity
				};
			}

			return new OperationResult()
			{
				Entity = entity
			};
		}

		public virtual OperationResult UpdateEntity(TEntity entity)
		{
			try
			{
				// TODO: some concerns here that may require cleanup:
				// 1: The only real reason we need the original at this point is so that we may
				// delete any orphaned children, although this might change if we want to narrow update statements to only
				// modified properties but no harm in leaving that as is.
				// 2: We are retrieving the original entity here so that we may get our includes to determine which child
				// collections to update if any. Ideally this should be done in the DataContext itself in which case we'd
				// need to come up with an alternate way to determine which children to include
				var newRepo = Activator.CreateInstance(this.GetType()) as Repository<TDbContext, TEntity>;
				var origEntity = newRepo.GetEntity(entity.ID);
				entity = context.UpdateEntity<TEntity>(entity, origEntity);
			}
			catch (Exception ex)
			{
				return new OperationResult()
				{
					Exception = ex,
					Failed = true,
					Entity = entity
				};
			}

			return new OperationResult()
			{
				Entity = entity
			};
		}

		public virtual OperationResult DeleteEntity(int id)
		{
			try
			{
				context.DeleteEntity<TEntity>(id);
			}
			catch (Exception ex)
			{
				var currEx = ex;
				while (currEx != null)
				{
					if (currEx.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
					{
						return new OperationResult()
						{
							Exception = new Exception("Item is in use and cannot be deleted!"),
							Failed = true
						};
					}
					currEx = currEx.InnerException;
				}
				return new OperationResult()
				{
					Exception = ex,
					Failed = true
				};
			}

			return new OperationResult();
		}

		IQueryable<IEntity> IRepository.GetEntities()
		{
			return GetEntities();
		}


		IQueryable<IEntity> IRepository.GetEntitiesOData(System.Net.Http.HttpRequestMessage request)
		{
			return GetEntitiesOData(request);
		}


		IEntity IRepository.GetEntity(int id)
		{
			return GetEntity(id);
		}


		OperationResult IRepository.InsertEntity(JObject entity)
		{
			return InsertEntity(entity.ToObject<TEntity>());
		}

		OperationResult IRepository.UpdateEntity(JObject entity)
		{
			return UpdateEntity(entity.ToObject<TEntity>());
		}
	}

	public interface IRepository
	{
		DbContextBase Context { get; }
		IQueryable<IEntity> GetEntities();
		IQueryable<IEntity> GetEntitiesOData(System.Net.Http.HttpRequestMessage request);
		IEntity GetEntity(int id);
		OperationResult InsertEntity(JObject entity);
		OperationResult UpdateEntity(JObject entity);
		OperationResult DeleteEntity(int id);
	}
}