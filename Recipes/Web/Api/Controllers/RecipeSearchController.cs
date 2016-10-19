﻿using PaJaMa.Recipes.Web.Api.Repository;
using PaJaMa.Recipes.Model;
using PaJaMa.Recipes.Model.Entities;
using PaJaMa.Recipes.Model.Dto;
using PaJaMa.Web;
using System;
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
    public class RecipeSearchController : ApiControllerBase<RecipesContext, Recipe, RecipeCoverDto>
    {
        protected override Repository<RecipesContext, Recipe, RecipeCoverDto> getNewRepository()
        {
            return new RecipeSearchRepository();
        }

        [HttpGet]
        [EnableQuery]
        public HttpResponseMessage Entities(string includes, string excludes = null, float? rating = null, bool bookmarked = false,
            int? recipeSourceID = null, bool picturesOnly = false, int page = 1, int pageSize = 20)
        {
            var recipeRepository = repository as RecipeSearchRepository;
            int recipeCount = 0;
            var recipes = recipeRepository.SearchRecipes(includes, excludes, rating, bookmarked,
                recipeSourceID, picturesOnly, page, pageSize, out recipeCount);
            var response = Request.CreateResponse(HttpStatusCode.OK, recipes);
            response.Headers.Add("X-InlineCount", recipeCount.ToString());
            return response;
        }

        [HttpGet]
        [EnableQuery]
        public HttpResponseMessage Entities(int random)
        {
            var recipeRepository = repository as RecipeSearchRepository;
            return Request.CreateResponse(HttpStatusCode.OK, recipeRepository.GetRandomRecipes(random));
        }
    }
}