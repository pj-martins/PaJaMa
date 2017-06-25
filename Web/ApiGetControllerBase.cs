using PaJaMa.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;

namespace PaJaMa.Web
{
	public abstract class ApiGetControllerBase<TDbContext, TEntity> : ApiController
		where TEntity : class, IEntity
		where TDbContext : DbContextBase
	{
		protected IRepository repository { get; set; }

		public ApiGetControllerBase()
			: base()
		{
			repository = getNewRepository();
		}

		protected virtual Repository<TDbContext, TEntity> getNewRepository()
		{
			return new Repository<TDbContext, TEntity>();
		}

		public int? GetUserId()
		{
			if (Request.Headers.Authorization == null)
				return null;

			int userId = -1;
			// int.TryParse(User.Identity.GetUserId(), out userId);

			return userId <= 0 ? (int?)null : userId;
		}
	
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
		public virtual PageResult<IEntity> EntitiesOData()
		{
			var entities = repository.GetEntitiesOData(Request);
			return new PageResult<IEntity>(entities, Request.ODataProperties().NextLink,
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
	}
}