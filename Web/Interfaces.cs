using Newtonsoft.Json.Linq;
using PaJaMa.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Web
{
	public interface IRepository
	{
		IQueryable<IEntityDto> GetEntities();
		IQueryable<IEntityDto> GetEntitiesOData(System.Net.Http.HttpRequestMessage request);
		IEntityDto GetEntity(int id);
		OperationResult InsertEntity(JObject dto);
		OperationResult UpdateEntity(JObject dto);
		OperationResult DeleteEntity(int id);
	}
}
