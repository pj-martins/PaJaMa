using Newtonsoft.Json.Linq;
using PaJaMa.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData.Extensions;

namespace PaJaMa.Web
{
	public abstract class ApiControllerBase<TDbContext, TEntity> : ApiGetControllerBase<TDbContext, TEntity>
		where TEntity : class, IEntity
		where TDbContext : DbContextBase
	{
		public ApiControllerBase()
			: base()
		{
		}

		[HttpPost]
		public virtual HttpResponseMessage PostEntity([FromBody]JObject dto)
		{
			try
			{
				var result = repository.InsertEntity(dto);
				if (result.Failed)
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.GetExceptionText());

				var response = Request.CreateResponse(HttpStatusCode.Created, result.Entity);
				string uri = Url.Link("DefaultApi", new { ID = result.Entity.ID });
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
				return Request.CreateResponse(HttpStatusCode.Accepted, result.Entity);
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