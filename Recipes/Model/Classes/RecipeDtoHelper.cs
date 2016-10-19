//using AutoMapper;
//using PaJaMa.Recipes.Model.Dto;
//using PaJaMa.Recipes.Model.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PaJaMa.Recipes.Model.Classes
//{
//	public class RecipeDtoHelper
//	{
//		public static void InitMappings(string imageRootURL)
//		{
//			//Mapper.CreateMap<Recipe, RecipeSearchResultDto>()
//			//	.ForMember(r => r.Source, c => c.MapFrom(r => r.RecipeSourceName))
//			//	.ForMember(r => r.ImageRootURL, c => c.MapFrom(r => imageRootURL));

//			//Mapper.CreateMap<Recipe, RecipeDto>()
//			//	.ForMember(r => r.RecipeIngredients, c => c.MapFrom(r => r.RecipeIngredientMeasurements))
//			//	.ForMember(r => r.Source, c => c.MapFrom(r => r.RecipeSourceName))
//			//	.ForMember(r => r.ImageURLs, c => c.MapFrom(r => r.RecipeImages.Any() ?
//			//			r.RecipeImages.OrderBy(ri => ri.Sequence).Select(ri => string.IsNullOrEmpty(ri.LocalImagePath) ? ri.ImageURL :
//			//				(imageRootURL + "/" + ri.RecipeImageID.ToString())).ToArray() : new string[] { imageRootURL + "/food.png" }));

//			//Mapper.CreateMap<Ingredient, IngredientDto>()
//			//	.ForMember(i => i.PossibleIngredientMeasurements, c => c.MapFrom(i => i.IngredientMeasurements.ToArray()));

//			//Mapper.CreateMap<IngredientMeasurement, IngredientMeasurementDto>()
//			//	.ForMember(im => im.IngredientName, c => c.MapFrom(im => im.Ingredient.IngredientName))
//			//	.ForMember(im => im.MeasurementName, c => c.MapFrom(im => im.Measurement == null ? string.Empty : im.Measurement.MeasurementName));

//			//Mapper.CreateMap<IngredientMeasurementAlternate, IngredientMeasurementAlternateDto>()
//			//	.ForMember(ima => ima.Ingredient, c => c.MapFrom(ima => ima.IngredientMeasurement1));

//			//Mapper.CreateMap<RecipeIngredientMeasurement, RecipeIngredientDto>()
//			//	.ForMember(ri => ri.Ingredient, c => c.MapFrom(ri => ri.IngredientMeasurement))
//			//	.ForMember(ri => ri.Alternates, c => c.MapFrom(ri => from ria in ri.IngredientMeasurement.IngredientMeasurementAlternates
//			//														 select ria));

//			//Mapper.CreateMap<RecipeIngredientMeasurement, RecipeIngredientSearchDto>()
//			//	.ForMember(ri => ri.Ingredient, c => c.MapFrom(ri => ri.IngredientMeasurement));
//		}

//		private static List<RecipeSearchDto> getRecipeSearchResultDtos(List<Recipe> recipes)
//		{
//			if (!recipes.Any()) return new List<RecipeSearchDto>();
//			List<RecipeSearchDto> recipeDtos = new List<RecipeSearchDto>();
//			var db = new RecipesContext();
//			var recIds = recipes.Select(r => r.RecipeID).ToList();
//			var allRims = db.RecipeIngredientMeasurements
//				.Include("IngredientMeasurement.Ingredient")
//				.Include("IngredientMeasurement.Measurement").Where(rim => recIds.Contains(rim.RecipeID))
//				.ToList();
//			var allImgs = db.RecipeImages.Where(rim =>
//				recIds.Contains(rim.RecipeID)).ToList();

//			foreach (var recipe in recipes)
//			{
//				var recDto = Mapper.Map<Recipe, RecipeSearchDto>(recipe);
//				recDto.Ingredients = (from ri in allRims
//									  where ri.RecipeID == recipe.RecipeID
//									  select Mapper.Map<RecipeIngredientMeasurement, RecipeIngredientSearchDto>(ri).Amount
//									  + " " + ri.IngredientMeasurement.Ingredient.IngredientName).ToArray();
//				var myimg = allImgs.Where(i => i.RecipeID == recipe.RecipeID).OrderBy(i => i.Sequence).FirstOrDefault();
//				if (myimg != null)
//					recDto.ImageURL = string.IsNullOrEmpty(myimg.LocalImagePath) ? myimg.ImageURL : (recDto.ImageRootURL + "/" + myimg.RecipeImageID.ToString());
//				else
//					recDto.ImageURL = recDto.ImageRootURL + "/food.png";
//				recipeDtos.Add(recDto);
//			}
//			return recipeDtos;
//		}

//		//public static RecipeSearchResultsDto GetEditableRecipes(RecipesContext db, int userId)
//		//{
//		//	var recs = from ur in db.UserRecipes
//		//			   where ur.UserID == userId
//		//				&& ur.AllowEdit
//		//			   select ur.Recipe;

//		//	RecipeSearchResultsDto searchResults = new RecipeSearchResultsDto();
//		//	searchResults.TotalResults = recs.Count();
//		//	searchResults.Recipes = getRecipeSearchResultDtos(recs.ToList()).ToArray();
//		//	return searchResults;
//		//}

//		public static IngredientDto[] SearchIngredients(RecipesContext db, string partial)
//		{
//			return db.SearchIngredients(partial).Select(i => Mapper.Map<Ingredient, IngredientDto>(i)).ToArray();
//		}

//		public static string GetRecipeImagePath(RecipesContext db, int recipeImageID)
//		{
//			RecipeImage rim = db.RecipeImages.First(ri => ri.RecipeImageID == recipeImageID);
//			return string.IsNullOrEmpty(rim.LocalImagePath) ? rim.ImageURL : rim.LocalImagePath;
//		}

//		public static void UpdateRecipe(RecipesContext db, RecipeDto dtoRecipe, int userId)
//		{
//			if (userId <= 0) return;

//			var trans = db.Database.BeginTransaction();
//			try
//			{
//				bool allowEdit = false;
//				if (dtoRecipe.IsDirty)
//				{
//					//allowEdit = true;
//					//var recSource = db.RecipeSources.FirstOrDefault(r => r.RecipeSourceName.ToLower() == dtoRecipe.Source.ToLower());
//					//if (recSource == null)
//					//{
//					//	recSource = new RecipeSource();
//					//	recSource.RecipeSourceName = dtoRecipe.Source;
//					//	db.RecipeSources.Add(recSource);
//					//	db.SaveChanges();
//					//}

//					//Recipe rec = null;
//					//if (dtoRecipe.RecipeID == 0)
//					//{
//					//	rec = new Recipe();
//					//	db.Recipes.Add(rec);
//					//}
//					//else
//					//	rec = db.Recipes.First(r => r.RecipeID == dtoRecipe.RecipeID);

//					//Mapper.Map<RecipeDto, Recipe>(dtoRecipe, rec);

//					//rec.RecipeSourceID = recSource.RecipeSourceID;


//					//for (int i = rec.RecipeRecipeIngredientMeasurements.Count - 1; i >= 0; i--)
//					//{
//					//	var rim = rec.RecipeRecipeIngredientMeasurements[i];
//					//	if (!dtoRecipe.RecipeIngredients.Any(ri => ri.Ingredient != null &&
//					//		ri.Ingredient.IngredientMeasurementID == rim.IngredientMeasurementID))
//					//	{
//					//		rim.Delete();
//					//		rec.RecipeRecipeIngredientMeasurements.RemoveAt(i);
//					//	}
//					//}

//					//foreach (var ri in dtoRecipe.RecipeIngredients)
//					//{
//					//	if (ri.Ingredient != null)
//					//	{
//					//		var rim = rec.RecipeRecipeIngredientMeasurements
//					//			.FirstOrDefault(x => x.IngredientMeasurementID == ri.Ingredient.IngredientMeasurementID);

//					//		if (rim == null)
//					//		{
//					//			rim = df.CreateDataItem<RecipeIngredientMeasurement>();
//					//			rim.RecipeID = rec.RecipeID;
//					//			rim.IngredientMeasurementID = ri.Ingredient.IngredientMeasurementID;
//					//		}

//					//		rim.Quantity = ri.Quantity;
//					//		rim.Save();
//					//	}
//					//}

//					//db.SaveChanges();
//					//dtoRecipe.RecipeID = rec.RecipeID;
//				}

//				var ur = db.UserRecipes.FirstOrDefault(x => x.RecipeID == dtoRecipe.RecipeID && x.UserID == userId);
//				if (ur == null)
//				{
//					ur = new UserRecipe();
//					ur.RecipeID = dtoRecipe.RecipeID;
//					ur.UserID = userId;
//					db.UserRecipes.Add(ur);
//				}

//				ur.Rating = dtoRecipe.UserRating;
//				ur.IsBookmarked = dtoRecipe.IsBookmarked;
//				ur.Notes = string.IsNullOrEmpty(dtoRecipe.Notes) ? null : dtoRecipe.Notes;
//				if (allowEdit) ur.AllowEdit = true;
//				db.SaveChanges();

//				trans.Commit();
//			}
//			catch
//			{
//				trans.Rollback();
//				throw;
//			}
//		}

//		//public static void DeleteRecipe(RecipesEntities db, int recipeId)
//		//{
//		//	Recipe rec = db.Recipes.First(r => r.RecipeID == recipeId);
//		//  db.Recipes.Remove(rec);
//		////	db.Entry(rec).State = System.Data.Entity.EntityState.Deleted;
//		//	db.SaveChanges();
//		//}
//	}
//}
