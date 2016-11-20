using PaJaMa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Dto
{
    public interface IEntityDto
    {
        int ID { get; set; }
		void MapToEntity(DbContextBase context, IEntity dto);
	}
}
