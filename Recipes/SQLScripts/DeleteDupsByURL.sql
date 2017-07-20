delete from Recipe where RecipeID in
(
select RecipeID from
(
select RecipeID, RecipeName, RowNum = row_number() over (partition by RecipeName, RecipeURL order by RecipeID)
from Recipe
) z
where RowNum > 1
)

delete from RecipeSearch where RecipeID not in (select RecipeID from Recipe)