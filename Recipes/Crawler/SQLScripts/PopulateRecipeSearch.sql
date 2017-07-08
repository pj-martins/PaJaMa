insert into RecipeSearch (RecipeID, RecipeName, IngredientString)
select r2.RecipeID, RecipeName,
stuff((select '__' + i.IngredientName
	from Recipe r
	join RecipeIngredientMeasurement rim on rim.RecipeID = r.RecipeID
	join IngredientMeasurement im on im.IngredientMeasurementID = rim.IngredientMeasurementID
	join Ingredient i on i.IngredientID = im.IngredientID
	where r.RecipeID = r2.RecipeID for xml path('')), 1, 2, '')
from Recipe r2

