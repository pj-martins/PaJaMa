using Newtonsoft.Json.Linq;
using PaJaMa.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace PaJaMa.Web
{
#if DEBUG
    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
#endif
	public class DynamicController : ApiControllerBase
	{
        private Type getContextType()
		{
			string contextType = ConfigurationManager.AppSettings["DbContextType"];
			if (string.IsNullOrEmpty(contextType)) throw new Exception("Must specify an appsetting DbContextType in order to use dynamic controller!");
			
			var type = Type.GetType(contextType);
			if (type == null) throw new Exception("Type " + contextType + " could not be found!");
			return type;
		}

        private IRepository getRepository(string entityType)
        {
            var allowedDynamicTypes = ConfigurationManager.AppSettings["AllowedDynamicTypes"].Split(';');
            if (!allowedDynamicTypes.Contains(entityType)) throw new UnauthorizedAccessException();

            var dtoAsm = System.Reflection.Assembly.Load(ConfigurationManager.AppSettings["DtoAssembly"]);

            var type = getContextType();
            var repoType = typeof(Repository<,,>).MakeGenericType(type, type.Assembly.GetTypes().First(t => t.Name.ToLower() == entityType.ToLower()),
                dtoAsm.GetTypes().First(t => t.Name.ToLower() == entityType.ToLower() + "dto"));
            return Activator.CreateInstance(repoType) as IRepository;
        }

        [HttpGet]
        [Route("api/Dynamic/{entityType}/entity/{id}")]
        public HttpResponseMessage Entity(string entityType, int id)
        {
            repository = getRepository(entityType);
            return base.Entity(id);
        }

        [EnableQuery]
        [HttpGet]
        [Route("api/Dynamic/{entityType}/entities")]
        public HttpResponseMessage Entities(string entityType)
        {
            repository = getRepository(entityType);
            return base.Entities();
        }

        [HttpGet]
        [Route("api/Dynamic/{entityType}/entitiesOData")]
        public PageResult<IEntityDto> EntitiesOData(string entityType)
        {
            repository = getRepository(entityType);
            return base.EntitiesOData();
        }

        public override HttpResponseMessage PostEntity([FromBody] JObject dto)
        {
            throw new UnauthorizedAccessException();
        }

        public override HttpResponseMessage PutEntity(int id, [FromBody] JObject dto)
        {
            throw new UnauthorizedAccessException();
        }

        public override HttpResponseMessage DeleteEntity(int id)
        {
            throw new UnauthorizedAccessException();
        }
    }
}