select r.RecipeName, rim.Quantity, m.MeasurementName, i.IngredientName, r.RecipeURL, rse.Ingredients
from Recipe r
join RecipeIngredientMeasurement rim on rim.RecipeID = r.RecipeID
join IngredientMeasurement im on im.IngredientMeasurementID = rim.IngredientMeasurementID
join Ingredient i on i.IngredientID = im.IngredientID
left join Measurement m on m.MeasurementID = im.MeasurementID
left join RecipeSearch rse on rse.RecipeID = r.RecipeID
where r.RecipeID = 1851559