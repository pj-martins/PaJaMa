using AutoMapper;
using AutoMapper.QueryableExtensions;
using PaJaMa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Dto
{
    public abstract class DtoContextBase<TDbContext> : IDtoContext where TDbContext : DbContextBase
    {
		private MapperConfiguration _mapperConfig;
		protected List<EntityDtoMap> mappings { get; private set; }
		public string ModifiedBy { get; set; }

		public DtoContextBase()
        {
			mappings = new List<EntityDtoMap>();
			_mapperConfig = new MapperConfiguration(createMaps);
        }

		private TDbContext createContext()
		{
			var ctxt = Activator.CreateInstance<TDbContext>();
			ctxt.ModifiedBy = ModifiedBy;
			return ctxt;
		}

		protected abstract void createMaps(IMapperConfigurationExpression cfg);

		protected void createMap<TEntity, TEntityDto>(IMapperConfigurationExpression cfg, Expression<Func<TEntity, int>> idMap)
			where TEntity : IEntity
			where TEntityDto : IEntityDto
		{
			mappings.Add(new EntityDtoMap(typeof(TEntity), typeof(TEntityDto), cfg.CreateMap<TEntity, TEntityDto>()
				.ForMember(x => x.ID, y => y.MapFrom(idMap))));
		}

		public virtual IQueryable<TEntityDto> GetEntities<TEntityDto>() where TEntityDto : IEntityDto
		{
			var context = createContext();
			var entityType = mappings.First(m => m.DtoType == typeof(TEntityDto)).EntityType;
			var dbSet = context.Set(entityType).AsQueryable();
			return dbSet.ProjectTo<TEntityDto>(_mapperConfig);
		}

		public virtual TEntityDto GetEntity<TEntityDto>(int id)
			where TEntityDto : IEntityDto
		{
			var entityType = mappings.First(m => m.DtoType == typeof(TEntityDto)).EntityType;
			var entity = createContext().GetEntity(entityType, id);
			var mapperInstance = _mapperConfig.CreateMapper();
			var dto = (TEntityDto)mapperInstance.Map(entity, entityType, typeof(TEntityDto));
			return dto;
		}

		public virtual OperationResult InsertEntity<TEntityDto>(TEntityDto dto)
			where TEntityDto : IEntityDto
		{
			var context = createContext();
			IEntity entity = null;
			try
			{
				var entityType = mappings.First(m => m.DtoType == typeof(TEntityDto)).EntityType;
				entity = (IEntity)Activator.CreateInstance(entityType);
				dto.MapToEntity(context, entity);
				context.Set(entityType).Add(entity);
				context.SaveChanges();
				_mapperConfig.CreateMapper().Map(entity, dto, entityType, typeof(TEntityDto));
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

		public virtual OperationResult UpdateEntity<TEntityDto>(TEntityDto dto)
			where TEntityDto : IEntityDto
		{
			IEntity entity = null;
			try
			{
				var context = createContext();
				var entityType = mappings.First(m => m.DtoType == typeof(TEntityDto)).EntityType;
				entity = context.GetEntity(entityType, dto.ID);
				dto.MapToEntity(context, entity);
				context.SaveChanges();
				_mapperConfig.CreateMapper().Map(entity, dto, entityType, typeof(TEntityDto));
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

		public virtual OperationResult DeleteEntity<TEntityDto>(int id)
			where TEntityDto : IEntityDto
		{
			try
			{
				var context = createContext();
				var entityType = mappings.First(m => m.DtoType == typeof(TEntityDto)).EntityType;
				var entity = context.GetEntity(entityType, id);
				if (entity != null)
				{
					context.Set(entityType).Remove(entity);
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
	}
}
