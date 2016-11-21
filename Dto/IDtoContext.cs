using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Dto
{
	public interface IDtoContext
	{
		string ModifiedBy { get; set; }
		IQueryable<TEntityDto> GetEntities<TEntityDto>() where TEntityDto : IEntityDto;
		TEntityDto GetEntity<TEntityDto>(int id) where TEntityDto : IEntityDto;
		OperationResult InsertEntity<TEntityDto>(TEntityDto dto) where TEntityDto : IEntityDto;
		OperationResult UpdateEntity<TEntityDto>(TEntityDto dto) where TEntityDto : IEntityDto;
		OperationResult DeleteEntity<TEntityDto>(int id) where TEntityDto : IEntityDto;
	}
}

