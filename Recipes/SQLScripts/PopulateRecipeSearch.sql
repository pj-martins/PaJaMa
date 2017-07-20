insert into RecipeSearch (RecipeID, RecipeName, IngredientString, Rating, CoverImageURL, RecipeSourceID)
select r2.RecipeID, RecipeName,
stuff((select ' __ ' + i.IngredientName
	from Recipe r
	join RecipeIngredientMeasurement rim on rim.RecipeID = r.RecipeID
	join IngredientMeasurement im on im.IngredientMeasurementID = rim.IngredientMeasurementID
	join Ingredient i on i.IngredientID = im.IngredientID
	where r.RecipeID = r2.RecipeID for xml path('')), 1, 2, ''),
	Rating, (select top 1 ImageURL from RecipeImage where RecipeID = r2.RecipeID), RecipeSourceID
from Recipe r2
where RecipeID not in
(select RecipeID from RecipeSearch)

-- truncate table RecipeSearch