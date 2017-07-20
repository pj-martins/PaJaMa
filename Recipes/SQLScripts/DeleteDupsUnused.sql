select RecipeID 
into #tmpDups
from
(
select *, RowNum = row_number() over (partition by recipeurl order by recipeid) from Recipe r
where recipeurl is not null
) z
where z.RowNum > 1



delete from RecipeImage where RecipeID in (select RecipeID from #tmpDups)
delete from RecipeIngredientMeasurement where RecipeID in (select RecipeID from #tmpDups)
delete from Recipe where RecipeID in (select RecipeID from #tmpDups)

delete from IngredientMeasurement where IngredientMeasurementID not in (select IngredientMeasurementID from RecipeIngredientMeasurement)
delete from Ingredient where IngredientID not in (select IngredientID from IngredientMeasurement)
delete from Measurement where MeasurementID not in (select MeasurementID from IngredientMeasurement)

drop table #tmpDups