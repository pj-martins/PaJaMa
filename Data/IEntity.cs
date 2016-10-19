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
		string ModifiedBy { get; set; }
		DateTime? ModifiedDT { get; set; }
		IEntity CloneEntity(DbContextBase context);
        void MapFromDto(DbContextBase context, IEntityDto dto);
    }

    // TODO: separate into a Core assembly
	public interface IEntityDto
	{
		int ID { get; set; }
        string ModifiedBy { get; set; }
        DateTime? ModifiedDT { get; set; }
    }
}
