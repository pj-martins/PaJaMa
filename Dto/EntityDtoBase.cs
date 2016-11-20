using Newtonsoft.Json;
using PaJaMa.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Dto
{
    public abstract class EntityDtoBase : IEntityDto
    {
        public int ID { get; set; }

		// AutoMapper has its quirks, it works fine going from Entity to Dto, other way around not so much, hence we'll
		// do a bit of a condensed version
		public virtual void MapToEntity(DbContextBase context, IEntity entity)
		{
			var scalarProps = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(p => p.PropertyType.GetInterface(typeof(IEntityDto).Name) == null
						&& !p.PropertyType.GetGenericArguments().Any(ga => ga.GetInterface(typeof(IEntityDto).Name) != null)
						&& !p.PropertyType.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any()
					);

			// first loop through our non parent/children fields
			foreach (var scalarProp in scalarProps)
			{
				var entityProp = this.GetType().GetProperty(scalarProp.Name, BindingFlags.Public | BindingFlags.Instance);
				if (entityProp == null || !entityProp.CanWrite) continue;

				entityProp.SetValue(entity, scalarProp.GetValue(this));
			}

			#region PARENTS
			var parentProps = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(p => p.PropertyType.GetInterface(typeof(IEntityDto).Name) != null);

			foreach (var prop in parentProps)
			{
				// make sure we find a matching prop on the entity itself
				var entityProp = this.GetType().GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance);
				if (entityProp == null || entityProp.PropertyType.GetInterface(typeof(IEntity).Name) == null) continue;

				var entityVal = entityProp.GetValue(entity) as IEntity;

				var dtoVal = prop.GetValue(this, null) as IEntityDto;

				// if our dto value is blank then we need to blank out the entity value
				if (dtoVal == null)
				{
					if (entityVal != null)
						entityProp.SetValue(entity, null);
					continue;
				}

				// we only want to change the entity's value if it was blank initially or if the parent (and thus parent id) has changed
				if (entityVal == null || entityVal.ID != dtoVal.ID)
				{
					// this is an existing parent so get from database
					if (dtoVal.ID != 0)
					{
						// entityframework is not nice when it comes to retrieving an entity dynamically, so we'll do some
						// tricky tricky reflection to get the GetEntity method and to invoke it
						var method = context.GetType().GetMethod("GetEntity", BindingFlags.Public | BindingFlags.Instance);
						var generic = method.MakeGenericMethod(entityProp.PropertyType);
						entityVal = generic.Invoke(context, new object[] { dtoVal.ID }) as IEntity;
					}
					else
					{
						// the dto object is a new object so we need to make our entity value new too
						entityVal = Activator.CreateInstance(entityProp.PropertyType) as IEntity;
					}

					entityProp.SetValue(this, entityVal);
				}

				// we only want to save parents if they've explicitly been configured to be saved, otherwise we just
				// need the foreign keys and we can set them to unchanged
				if ((entity as EntityBase).SaveParentChild(entityProp.Name) || entityVal.ID == 0)
					(dtoVal as IEntityDto).MapToEntity(context, entity);
				else if (entityVal.ID != 0)
					context.Entry(entityVal).State = EntityState.Unchanged;

			}
			#endregion

			#region CHILDREN
			var childProps = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(p => p.PropertyType.GetGenericArguments().Any(ga => ga.GetInterface(typeof(IEntityDto).Name) != null));

			foreach (var prop in childProps)
			{
				// make sure we find a matching prop on the entity itself. Note the prop has to be a collection with a
				// generic argument with interface IEntity, we're assuming it will be a collection, might need to do some
				// safeguarding against that
				var entityProp = this.GetType().GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance);
				if (entityProp == null || !entityProp.PropertyType.GetGenericArguments().Any(ga => ga.GetInterface(typeof(IEntity).Name) != null))
					continue;

				var dtoVal = prop.GetValue(this, null);
				var entityVal = entityProp.GetValue(entity) as IEnumerable;

				// if our dto value is blank then we need to blank out the entity value
				if (dtoVal == null)
				{
					entityVal = null;
					continue;
				}

				var entityType = entityProp.PropertyType.GetGenericArguments().First();

				// this should never be true, EF will always initialize a child list as empty, but just in case
				if (entityVal == null)
				{
					var genericType = typeof(List<>).MakeGenericType(entityType);
					entityVal = Activator.CreateInstance(genericType) as IEnumerable;
					entityProp.SetValue(this, entityVal);
				}

				// only do this 
				if ((entity as EntityBase).SaveParentChild(entityProp.Name))
				{
					// entityframework collections are of type HashSet (which implements ICollection). The add method is only
					// on the generic definitions and thus cannot be dynamically accessed, so we need to use tricky tricky
					// reflection again to get to the add method
					var addMethod = entityVal.GetType().GetMethod("Add");

					// we'll keep track of current keys so we can remove orphans later
					var dtoKeys = new List<int>();

					foreach (IEntityDto childDto in dtoVal as IList)
					{
						IEntity childEntity = null;

						// if our child has an ID 0 it is a new item to be added to our collection, otherwise
						// we need to find the existing child, need to do some testing to see if we'd ever
						// have orphaned dto IDs without a matching entity ID, should only happen on stale objects
						// in which case we would've failed on an earlier check
						if (childDto.ID == 0)
						{
							childEntity = Activator.CreateInstance(entityType) as IEntity;
							addMethod.Invoke(entityVal, new object[] { childEntity });
						}
						else
						{
							childEntity = (entityVal as IEnumerable).OfType<IEntity>().FirstOrDefault(e => e.ID == childDto.ID);

							// can't think of another scenario where an entity will not be in the db anymore other than someone
							// else removing it at the same time
							if (childEntity == null)
								throw new Exception("Item no longer exists. Please refresh the page.");
							dtoKeys.Add(childDto.ID);
						}

						childDto.MapToEntity(context, childEntity);
					}

					// now lets delete our orphans
					var copiedList = entityVal.Cast<IEntity>().ToList();
					foreach (var copied in copiedList)
					{
						if (copied.ID == 0) continue;
						if (!dtoKeys.Contains(copied.ID))
							context.Entry(copied).State = EntityState.Deleted;
					}
				}
				else
				{
					// TODO can't think of a situation right now where child collections wouldn't want to be saved
				}
			}
			#endregion
		}
	}
}
