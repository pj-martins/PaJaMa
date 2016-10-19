class RecipeSearchResultDto < RecipeBaseDto
  def initialize(recipe)
    super(recipe)
    @ingredients = recipe.recipeingredientmeasurements.map{|ri| ri.description}
    @imageURL = recipe.recipeimages[0].nil? ? "" : recipe.recipeimages[0].ImageURL
  end
end