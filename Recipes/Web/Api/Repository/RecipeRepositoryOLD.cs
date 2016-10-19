//using AutoMapper;
//using PaJaMa.Recipes.Model.Dto;
//using PaJaMa.Recipes.Model.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace PaJaMa.Recipes.Web.Api.Repository
//{
//	public class RecipeRepository
//	{
//		private RecipesContext _context;

//		private string foodImagePath
//		{
//			get { return string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, "/Images/food.png"); }
//		}

//		public RecipeRepository()
//		{
//			_context = new RecipesContext();

//			Mapper.CreateMap<Recipe, RecipeDto>()
//				.ForMember(r => r.RecipeIngredients, c => c.MapFrom(r => r.RecipeIngredientMeasurements))
//				.ForMember(r => r.Source, c => c.MapFrom(r => r.RecipeSourceName))
//				.ForMember(r => r.ImageURLs, c => c.MapFrom(r => r.RecipeImages.Any() ?
//						r.RecipeImages.OrderBy(ri => ri.Sequence).Select(ri => string.IsNullOrEmpty(ri.LocalImagePath) ? ri.ImageURL :
//							("/" + ri.RecipeImageID.ToString())).ToArray() : new string[] { foodImagePath }));

//			Mapper.CreateMap<RecipeSource, RecipeSourceDto>();
//		}

//		public RecipeDto GetRecipe(int recipeID, int? userId)
//		{
//			var recipe = _context.Recipes.First(r => r.RecipeID == recipeID);
//			RecipeDto recDto = Mapper.Map<Recipe, RecipeDto>(recipe);
//			if (userId != null)
//			{
//				var recUser = recipe.UserRecipes.FirstOrDefault(ur => ur.UserID == userId.Value);
//				if (recUser != null)
//				{
//					recDto.IsBookmarked = recUser.IsBookmarked;
//					recDto.UserRating = recUser.Rating.GetValueOrDefault();
//					recDto.Notes = recUser.Notes;
//				}
//			}
//			return recDto;
//		}

//		public RecipeSourceDto[] GetSources()
//		{
//			var recSrces = _context.RecipeSources.ToList();
//			return recSrces.Select(rs => Mapper.Map<RecipeSource, RecipeSourceDto>(rs))
//				.OrderBy(r => r.RecipeSourceName).ToArray();
//		}
//	}
//}