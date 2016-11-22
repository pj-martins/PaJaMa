using PaJaMa.Recipes.Dto;
using PaJaMa.Recipes.Dto.Entities;
using PaJaMa.Recipes.Model.Entities;
using PaJaMa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaJaMa.Recipes.Web.Api.Controllers
{
#if DEBUG
	[System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
#endif
	public class RecipeSourceController : ApiGetControllerBase<RecipesDtoMapper, RecipeSource, RecipeSourceDto> { }
}