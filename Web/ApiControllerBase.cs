using Newtonsoft.Json.Linq;
using PaJaMa.Data;
using PaJaMa.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Query;

namespace PaJaMa.Web
{
	public class ApiControllerBase<TDtoMapper, TEntity, TEntityDto> : ApiControllerBase
		where TEntity : class, IEntity
		where TEntityDto : class, IEntityDto
		where TDtoMapper : DtoMapperBase
	{
		public ApiControllerBase()
			: base()
		{
			repository = getNewRepository();
		}

		protected virtual Repository<TDtoMapper, TEntity, TEntityDto> getNewRepository()
		{
			return new Repository<TDtoMapper, TEntity, TEntityDto>();
		}

		public int? GetUserId()
		{
			if (Request.Headers.Authorization == null)
				return null;

			int userId = -1;
			// int.TryParse(User.Identity.GetUserId(), out userId);

			return userId <= 0 ? (int?)null : userId;
		}
	}

	public abstract class ApiControllerBase : System.Web.Http.ApiController
	{
		protected IRepository repository { get; set; }

		[HttpGet]
		[EnableQuery]
		public virtual HttpResponseMessage Entities()
		{
			var entities = repository.GetEntities();
			var response = Request.CreateResponse(HttpStatusCode.OK, entities);
            return response;
		}

		// when we use odata filter, the count from entities will return the count of all entries prefiltered thus the following method
		// to give us a count after the data has been filtered
		[HttpGet]
		public virtual PageResult<IEntityDto> EntitiesOData()
		{
			var entities = repository.GetEntitiesOData(Request);
			return new PageResult<IEntityDto>(entities, Request.ODataProperties().NextLink,
				 Request.ODataProperties().TotalCount);
		}

        [HttpGet]
        public virtual HttpResponseMessage Entity(int id)
        {
            try
            {
                var entity = repository.GetEntity(id);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (UnauthorizedAccessException)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost]
        public virtual HttpResponseMessage PostEntity([FromBody]JObject dto)
        {
            try
            {
                var result = repository.InsertEntity(dto);
                if (result.Failed)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.GetExceptionText());

                var response = Request.CreateResponse(HttpStatusCode.Created, result.EntityDto);
                string uri = Url.Link("DefaultApi", new { ID = result.EntityDto.ID });
                if (uri != null)
                    response.Headers.Location = new Uri(uri);
                return response;
            }
            catch (UnauthorizedAccessException)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        [HttpPut]
        public virtual HttpResponseMessage PutEntity(int id, [FromBody]JObject dto)
        {
            try
            {
                var result = repository.UpdateEntity(dto);
                if (result.Failed)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.GetExceptionText());
                return Request.CreateResponse(HttpStatusCode.Accepted, result.EntityDto);
            }
            catch (UnauthorizedAccessException)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        [HttpDelete]
        public virtual HttpResponseMessage DeleteEntity(int id)
        {
            try
            {
                var result = repository.DeleteEntity(id);

                if (result.Failed)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.GetExceptionText());

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (UnauthorizedAccessException)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}
