select rs.RecipeID, rs.RecipeName, rs.Ingredients, so.RecipeSourceName, r.RecipeURL, so.RecipeSourceID from RecipeSearch rs
join Recipe r on r.RecipeID = rs.RecipeID
join RecipeSource so on so.RecipeSourceID = r.RecipeSourceID
where contains(rs.RecipeName, 'cheesecake')