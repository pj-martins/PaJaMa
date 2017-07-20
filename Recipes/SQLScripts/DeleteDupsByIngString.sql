declare @Dups table (RecipeID int not null)

insert into @Dups
select RecipeID from (
select *, RowNum = row_number() over (partition by IngredientString order by RecipeID)
from RecipeSearch
) z 
where RowNum > 1
order by RecipeName

delete from RecipeImage where RecipeID in (select RecipeID from @Dups)
delete from RecipeIngredientMeasurement where RecipeID in (select RecipeID from @Dups)
delete from Recipe where RecipeID in (select RecipeID from @Dups)
delete from RecipeSearch where RecipeID in (select RecipeID from @Dups)