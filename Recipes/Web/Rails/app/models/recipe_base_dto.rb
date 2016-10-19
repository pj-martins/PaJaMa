class RecipeBaseDto
  def initialize(recipe)
    @recipeID = recipe.RecipeID
    @recipeName = recipe.RecipeName
    @rating = recipe.Rating
    @source = recipe.Source
  end
end