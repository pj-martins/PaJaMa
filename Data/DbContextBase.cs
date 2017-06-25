using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Data
{
	public abstract class DbContextBase : DbContext
	{
		public const string INACTIVE = "Inactive";
		public string ModifiedBy { get; set; }
		public DbContextBase(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			Configuration.AutoDetectChangesEnabled = false;
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

		public List<ObjectStateEntry> GetChangedEntries()
		{
			return ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntries(
				EntityState.Added | EntityState.Modified).ToList();
		}

		public virtual TEntity GetEntity<TEntity>(Int64 id)
			where TEntity : class, IEntity
		{
			// since we're using generics, we have to dynamically create our expression to filter by entity's key field name
			var attr = (from p in typeof(TEntity).GetProperties()
						let a = p.GetCustomAttribute(typeof(KeyAttribute)) as KeyAttribute
						where a != null
						select new { Prop = p, Attr = a }).First();

			var keyFieldName = attr.Prop.Name;

			// specify the entity's table name to be queries against (... from {0})
			var entityParam = Expression.Parameter(typeof(TEntity));

			var lambda = Expression.Lambda<Func<TEntity, bool>>(
				// specify the operator
				Expression.Equal(
				// specify field to query against, in this case the key field on the entity's attribute
				Expression.Property(entityParam, keyFieldName),
				// specify the value to query against
				Expression.Constant(id)),
				entityParam
				);

			// run the query
			return Set<TEntity>().First(lambda);
		}

		public virtual IQueryable<TEntity> GetEntities<TEntity>()
			where TEntity : class, IEntity
		{
			var dbSet = Set<TEntity>().AsQueryable();

			var inactiveProp = typeof(TEntity).GetProperty(INACTIVE, BindingFlags.Public | BindingFlags.Instance);
			if (inactiveProp != null)
			{
				// specify the entity's table name to be queries against (... from {0})
				var entityParam = Expression.Parameter(typeof(TEntity));

				var lambda = Expression.Lambda<Func<TEntity, bool>>(
					// specify the operator
				Expression.Equal(
					// specify field to query against, in this case the key field on the entity's attribute
				Expression.Property(entityParam, INACTIVE),
					// specify the value to query against
				Expression.Constant(false)),
				entityParam
				);

				dbSet = dbSet.Where(lambda);
			}

			return dbSet.AsQueryable();
		}

		public virtual TEntity InsertEntity<TEntity>(TEntity entity)
			where TEntity : class, IEntity
		{
			// we're calling prepParentsChildren after we add so that we may "un-add" existing parents
			// from the context
			Set<TEntity>().Add(entity);
			prepParentsChildren(entity, null);

			saveChanges();
			return entity;
		}

		public virtual TEntity UpdateEntity<TEntity>(TEntity entity, TEntity original)
			where TEntity : class, IEntity
		{
			// TODO: concurrency

			// we're calling prepParentsChildren before we set modified so that parent IDs may be set
			// correctly, otherwise EF will complain about ref constraints
			prepParentsChildren(entity, original);
			Entry(entity).State = EntityState.Modified;

			saveChanges();
			return entity;
		}

		private void prepParentsChildren(IEntity entity, IEntity original)
		{
			#region PARENTS
			var parentProps = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(p => p.PropertyType.GetInterface(typeof(IEntity).Name) != null);

			foreach (var parentProp in parentProps)
			{
				var parentVal = parentProp.GetValue(entity) as IEntity;
				if (parentVal != null)
				{
					// EntityFramework will mark a parent as new if the child is new so we need to detach the parent
					Entry(parentVal).State = EntityState.Detached;
				}
			}
			#endregion

			#region CHILDREN
			// check for props with ICollection<T> where T is IEntity
			var childProps = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(p => p.PropertyType.GetGenericArguments().Any(ga => ga.GetInterface(typeof(IEntity).Name) != null));

			foreach (var childProp in childProps)
			{
				// one thing to note, if the child is not in our Includes, both the current and original entity's collections
				// will be empty, in which case nothing will get updated/deleted
				var childVal = childProp.GetValue(entity) as IEnumerable;
				var currentKeys = new List<Int64>();
				IEnumerable originalChildVal = null;
				if (original != null)
					originalChildVal = childProp.GetValue(original) as IEnumerable;

				if (childVal != null)
				{
					foreach (var child in childVal.OfType<IEntity>())
					{
						if (child.ID != 0)
							currentKeys.Add(child.ID);

						// TODO: add support for multi foreign keys
						var primKey = entity.GetType().GetProperties()
							.Where(p => p.GetCustomAttribute<KeyAttribute>() != null)
							.First();

						var childKey = child.GetType().GetProperty(primKey.Name);
						childKey.SetValue(child, primKey.GetValue(entity));

						// we shouldn't need to set each child as modified since they might not have changed, but we'd have to compare
						// against the original so for now we'll have them all update
						this.Entry(child).State = child.ID == 0 ? EntityState.Added : EntityState.Modified;
					}
				}

				// now delete orphans
				if (originalChildVal != null)
				{
					foreach (var originalChild in originalChildVal.OfType<IEntity>())
					{
						if (!currentKeys.Contains(originalChild.ID))
						{
							// since the original child is attached to a different context and all we really care about
							// is the ID for a delete, we'll create a to delete only instance
							var childToDelete = Activator.CreateInstance(originalChild.GetType()) as IEntity;
							(childToDelete as EntityBase).ID = originalChild.ID;
							this.Entry(childToDelete).State = EntityState.Deleted;
						}
					}
				}
			}
			#endregion
		}

		private void deleteEntity(IEntity entity)
		{
			// delete children
			// check for props with ICollection<T> where T is IEntity
			var childProps = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(p => p.PropertyType.GetGenericArguments().Any(ga => ga.GetInterface(typeof(IEntity).Name) != null));

			foreach (var childProp in childProps)
			{
				var childVal = childProp.GetValue(entity) as IEnumerable;

				if (childVal == null)
					continue;

				// if our child property was not a part of our includes, this list will be empty
				// and no children will get deleted, although that might cause a referential constraint
				// so validation will need to be done first
				foreach (var childEntity in childVal.OfType<IEntity>().ToList())
				{
					deleteEntity(childEntity);
				}
			}

			var inactiveProp = entity.GetType().GetProperty(DbContextBase.INACTIVE, BindingFlags.Public | BindingFlags.Instance);
			if (inactiveProp != null)
			{
				inactiveProp.SetValue(entity, true);
				Entry(entity).State = EntityState.Modified;
			}
			else
			{
				Entry(entity).State = EntityState.Deleted;
			}
		}

		public virtual void DeleteEntity<TEntity>(Int64 id)
			where TEntity : class, IEntity
		{
			var entity = GetEntity<TEntity>(id);
			if (entity != null)
			{
				deleteEntity(entity);
				saveChanges();
			}
		}

		public List<ObjectStateEntry> GetChangedDeletedEntries()
		{
			return ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntries(
				EntityState.Added | EntityState.Modified | EntityState.Deleted).ToList();
		}

		private int saveChanges()
		{
			var changedEntries = GetChangedEntries();
			foreach (ObjectStateEntry entry in changedEntries)
			{
				var entity = entry.Entity as IEntity;

				string modifiedBy = ModifiedBy;
				if (string.IsNullOrEmpty(modifiedBy))
					modifiedBy = "NOUSER";

				// TODO:
				//entity.ModifiedBy = modifiedBy;
				//entity.ModifiedDT = DateTime.Now;
			}

			int saved = base.SaveChanges();

			return saved;
		}

		public override int SaveChanges()
		{
			return saveChanges();
		}
	}
}
