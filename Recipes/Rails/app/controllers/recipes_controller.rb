class RecipesController < ApplicationController
  def index
    @recipes = RecipeDecorator.decorate_collection(Recipe.get_random(20))
	@search_parameters = RecipeSearchParameter.new;
  end

  def search
	@search_parameters = params[:@search_parameters]
	@recipes = RecipeDecorator.decorate_collection(Recipe.search_recipes(@search_parameters[:includes], @search_parameters[:excludes],
		@search_parameters[:rating]))
	render :action => "index"
  end
end
