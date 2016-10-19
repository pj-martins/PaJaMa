class RecipeDto < RecipeBaseDto
  def initialize(recipe)
    super(recipe)
    @recipeURL = recipe.RecipeURL
    @directions = recipe.Directions
    @numberOfServings = recipe.NumberOfServings
    @recipeIngredients = recipe.recipeingredientmeasurements.map{ |ri| RecipeIngredientDto.new(ri) }
    @imageURLs = recipe.recipeimages.map{ |ri| ri.ImageURL }
  end
end