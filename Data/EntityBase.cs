﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace PaJaMa.Data
{
    public abstract class EntityBase : IEntity
    {
        private PropertyInfo getKeyProp()
        {
            var attr = (from p in this.GetType().GetProperties()
                        let a = p.GetCustomAttribute(typeof(KeyAttribute)) as KeyAttribute
                        where a != null
                        select new { Prop = p, Attr = a }).First();

            return attr.Prop;
        }

        // TODO:
        //public string ModifiedBy { get; set; }
        //public DateTime ModifiedDT { get; set; }

        public abstract int ID { get; set; }
        public virtual List<ValidationError> Validate() { return new List<ValidationError>(); }

        public virtual void OnEntitySaved(DbContextBase context) { }
        public virtual IEntity CloneEntity(DbContextBase context)
        {
            var newEntity = Activator.CreateInstance(this.GetType()) as IEntity;
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var storageMetadata = ((EntityConnection)objectContext.Connection).GetMetadataWorkspace().GetItems(DataSpace.SSpace);
            var entityProps = (from s in storageMetadata where s.BuiltInTypeKind == BuiltInTypeKind.EntityType select s as EntityType);
            var personRightStorageMetadata = (from m in entityProps where m.Name == ObjectContext.GetObjectType(this.GetType()).Name select m).Single();
            foreach (var item in personRightStorageMetadata.Properties)
            {
                var prop = this.GetType().GetProperty(item.Name);
                if (prop.Name == "ID") continue;
                if (prop.Name == getKeyProp().Name) continue;

                prop.SetValue(newEntity, prop.GetValue(this, null), null);
            }
            return newEntity;
        }
    }
}
