insert into RecipeSearch (RecipeID, RecipeName, Ingredients)
(
	select RecipeID, RecipeName, IngredientString = substring(stuff (
		(select '_' + IngredientName
			from RecipeIngredientMeasurement rim
			join IngredientMeasurement im on im.IngredientMeasurementID = rim.IngredientMeasurementID
			join Ingredient i on i.IngredientID = im.IngredientID
			where rim.RecipeID = r.RecipeID 
			for xml path ('')), 1, 1, ''), 1, 5000)
	from Recipe r
	where r.RecipeID not in
	(
		select recipeid from recipesearch
	)
)
