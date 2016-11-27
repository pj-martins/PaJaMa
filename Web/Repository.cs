using AutoMapper.QueryableExtensions;
using Newtonsoft.Json.Linq;
using PaJaMa.Data;
using PaJaMa.Dto;
using System;
using System.Linq;
using System.Web;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;

namespace PaJaMa.Web
{
	public class Repository<TDtoMapper, TEntity, TEntityDto> : IRepository
		where TEntity : class, IEntity
		where TEntityDto : class, IEntityDto
		where TDtoMapper : DtoMapperBase
	{
		protected TDtoMapper mapper { get; private set; }

		public Repository()
		{
			mapper = Activator.CreateInstance<TDtoMapper>();
		}

		private DbContextBase createContext()
		{
			var context = mapper.GetDbContext();
			context.ModifiedBy = HttpContext.Current.Request.LogonUserIdentity.Name;
			return context;
		}

		protected virtual bool isUnauthorized(AuthorizationType authorizationType, object source)
		{
			// by default we'll only alow gets, override the repository if you want to do inserts, updates, deletes
			return (int)authorizationType > (int)AuthorizationType.GetEntity;
		}

		public virtual IQueryable<TEntityDto> GetEntities()
		{
			if (isUnauthorized(AuthorizationType.GetEntities, null))
				throw new UnauthorizedAccessException();

			var context = createContext();
			var dbSet = context.Set<TEntity>().AsQueryable();
			return dbSet.ProjectTo<TEntityDto>(mapper.MapperConfig);
		}

		public virtual IQueryable<TEntityDto> GetEntitiesOData(System.Net.Http.HttpRequestMessage request)
		{
			if (isUnauthorized(AuthorizationType.GetEntities, null))
				throw new UnauthorizedAccessException();

			var modelBuilder = new ODataConventionModelBuilder();
			modelBuilder.EntitySet<TEntityDto>(typeof(TEntityDto).Name);
			var castedOptions = new ODataQueryOptions<TEntityDto>(new ODataQueryContext(modelBuilder.GetEdmModel(), typeof(TEntityDto))
				, request);
			var entities = GetEntities() as IQueryable<TEntityDto>;

			if (entities == null) return null;

			var filtered = castedOptions.ApplyTo(entities) as IQueryable<TEntityDto>;
			return filtered;
		}

		protected virtual TEntity getEntity(int id)
		{
			var context = createContext();
			return context.GetEntity<TEntity>(id);
		}

		public virtual TEntityDto GetEntity(int id)
		{
			var entity = getEntity(id);
			var context = createContext();
			var mapperInstance = mapper.MapperConfig.CreateMapper();
			var dto = mapperInstance.Map<TEntity, TEntityDto>(entity);

			if (isUnauthorized(AuthorizationType.GetEntity, dto))
				throw new UnauthorizedAccessException();

			return dto;
		}

		public virtual OperationResult InsertEntity(TEntityDto dto)
		{
			if (isUnauthorized(AuthorizationType.GetEntity, dto))
				throw new UnauthorizedAccessException();

			TEntity entity = null;
			try
			{
				entity = Activator.CreateInstance<TEntity>();
				var context = createContext();
				dto.MapToEntity(context, entity);
				context.Set<TEntity>().Add(entity);
				context.SaveChanges();
				mapper.MapperConfig.CreateMapper().Map<TEntity, TEntityDto>(entity, dto);
			}
			catch (Exception ex)
			{
				return new OperationResult()
				{
					Exception = ex,
					Failed = true,
					EntityDto = dto,
					Entity = entity
				};
			}

			return new OperationResult()
			{
				EntityDto = dto,
				Entity = entity
			};
		}

		public virtual OperationResult UpdateEntity(TEntityDto dto)
		{
			if (isUnauthorized(AuthorizationType.GetEntity, dto))
				throw new UnauthorizedAccessException();

			TEntity entity = null;
			try
			{
				entity = getEntity(dto.ID);
				var context = createContext();
				dto.MapToEntity(context, entity);
				context.SaveChanges();
				mapper.MapperConfig.CreateMapper().Map<TEntity, TEntityDto>(entity, dto);
			}
			catch (Exception ex)
			{
				return new OperationResult()
				{
					Exception = ex,
					Failed = true,
					EntityDto = dto,
					Entity = entity
				};
			}

			return new OperationResult()
			{
				EntityDto = dto,
				Entity = entity
			};
		}

		public virtual OperationResult DeleteEntity(int id)
		{
			if (isUnauthorized(AuthorizationType.DeleteEntity, id))
				throw new UnauthorizedAccessException();

			try
			{
				var entity = getEntity(id);
				if (entity != null)
				{
					var context = createContext();
					context.Set<TEntity>().Remove(entity);
					context.SaveChanges();
				}
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

		IQueryable<IEntityDto> IRepository.GetEntities()
		{
			return GetEntities();
		}


		IQueryable<IEntityDto> IRepository.GetEntitiesOData(System.Net.Http.HttpRequestMessage request)
		{
			return GetEntitiesOData(request);
		}


		IEntityDto IRepository.GetEntity(int id)
		{
			return GetEntity(id);
		}


		OperationResult IRepository.InsertEntity(JObject dto)
		{
			return InsertEntity(dto.ToObject<TEntityDto>());
		}

		OperationResult IRepository.UpdateEntity(JObject dto)
		{
			return UpdateEntity(dto.ToObject<TEntityDto>());
		}
	}
}