using System;
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
		public string ModifiedBy { get; set; }
		public DbContextBase(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{

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

		public override int SaveChanges()
		{
			List<ValidationError> results = new List<ValidationError>();
			var changedEntries = GetChangedEntries();
			foreach (ObjectStateEntry entry in changedEntries)
			{
				IEntity entity = entry.Entity as IEntity;
				results.AddRange(entity.Validate());
			}

			if (results.Any())
			{
				throw new Exception("Validation failed:\r\n" +
					string.Join("\r\n", results.Select(r => r.Entity.ToString() + " - " + r.Error).ToArray()));
			}

			int saved = 0;

			try
			{
				saved = base.SaveChanges();
			}
			catch (DbEntityValidationException ex)
			{
				string message = "DbEntityValidationException Error:  ";

				foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
				{
					foreach (DbValidationError subItem in item.ValidationErrors)
					{
						message = string.Format("{0} [{1}] occurred in {2} at {3}", message, subItem.ErrorMessage, item.Entry.Entity.GetType().Name, subItem.PropertyName);
					}
				}

				throw new Exception(string.Format("{0} {1}", message, ex.ToString()));
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}

			return saved;
		}

		public virtual TEntity GetEntity<TEntity>(int id)
			where TEntity : class, IEntity
		{
			// since we're using generics, we have to dynamically create our expression to filter by entity's key field name
			var keyFieldName = typeof(TEntity).GetProperties().First(p => p.GetCustomAttributes(typeof(KeyAttribute)).Any()).Name;

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
			var dbSet = Set<TEntity>().AsQueryable();
			return dbSet.SingleOrDefault(lambda);
		}
	}
}
