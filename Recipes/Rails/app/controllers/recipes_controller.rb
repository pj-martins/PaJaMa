class RecipesController < ApplicationController
  def index
    @recipes = RecipeDecorator.decorate_collection(Recipe.get_random(20))
  end
end
