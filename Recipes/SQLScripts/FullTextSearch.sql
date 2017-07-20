CREATE FULLTEXT CATALOG recipes_catalog;

CREATE FULLTEXT INDEX 
  ON RecipeSearch(RecipeName LANGUAGE 1033, IngredientString LANGUAGE 1033)
  KEY INDEX PK_RecipeSearch ON recipes_catalog; 
GO

