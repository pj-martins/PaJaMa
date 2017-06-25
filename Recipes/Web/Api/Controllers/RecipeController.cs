using PaJaMa.Recipes.Web.Api.Repository;
using PaJaMa.Recipes.Model;
using PaJaMa.Recipes.Model.Entities;
using PaJaMa.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;

namespace PaJaMa.Recipes.Web.Api.Controllers
{
#if DEBUG
    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
#endif
    public class RecipeController : ApiControllerBase<RecipesContext, Recipe>
    {
        protected override Repository<RecipesContext, Recipe> getNewRepository()
        {
            return new RecipeRepository();
        }
        //[Route("editable")]
        //public HttpResponseMessage Get()
        //{
        //	var db = new RecipesContext();
        //	return Request.CreateResponse(HttpStatusCode.OK, 
        //		RecipeDtoHelper.GetEditableRecipes(db, GetUserId().GetValueOrDefault()));
        //}

        //[Route("{id:int}")]
        //public HttpResponseMessage Get(int id)
        //{
        //	var repository = new RecipeRepository();
        //	// string imageUrl = ConfigurationManager.AppSettings["RecipeImageURL"];
        //	return Request.CreateResponse(HttpStatusCode.OK, repository.GetRecipe(id, GetUserId()));
        //}

        //// POST api/recipe
        //[Route]
        //[HttpPost]
        //public RecipeDto Post(RecipeDto recipe)
        //{
        //	var db = new RecipesContext();
        //	RecipeDtoHelper.UpdateRecipe(db, recipe, GetUserId().GetValueOrDefault());
        //	return recipe;
        //}

        // PUT api/recipe/5
        //[HttpPut]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/recipe/5
        //[HttpDelete]
        //public void Delete(int id)
        //{
        //	//var db = new RecipesEntities();
        //	//RecipeDtoHelper.DeleteRecipe(db, id);
        //}
    }
}
