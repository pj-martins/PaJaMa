select count(*) from Recipe

select RecipeSourceName, Cnt from
(
	select RecipeSourceName, Cnt = count(*)
	from Recipe r
	join RecipeSource rs on rs.RecipeSourceID = r.RecipeSourceID
	group by RecipeSourceName
) z
order by Cnt desc