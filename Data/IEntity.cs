using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace PaJaMa.Data
{
	public interface IEntity
	{
		int ID { get; }
		IEntity CloneEntity(DbContextBase context);
        List<ValidationError> Validate();
    }
}
