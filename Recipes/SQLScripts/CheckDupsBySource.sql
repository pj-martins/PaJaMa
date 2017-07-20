-- cleanup dups by source
select * from
(
select RecipeName, RecipeURL, RecipeSourceName ,
	RowNum = row_number() over (partition by RecipeName, RecipeSourceName order by RecipeID)
from Recipe r
join RecipeSource rs on rs.RecipeSourceID = r.RecipeSourceID
) z
where RowNum > 1
