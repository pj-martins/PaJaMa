select * from
(
select 
	Domain = substring(Directions, 1, charindex('/', Directions, 9)),
	Cnt = count(*)
from Recipe
where Directions like 'http%' and RecipeSourceID = 65
group by substring(Directions, 1, charindex('/', Directions, 9))
) z order by Cnt desc