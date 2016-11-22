﻿class Api::V1::RecipesController < Api::ApiController
  def index
    render json: self.jsonify(Recipe.get_random(20))
  end

  def search
	render json: self.jsonify(Recipe.search_recipes(params))
  end

  def jsonify(recipes)
	return recipes.as_json({
			:include => {
				:RecipeImages => { :only => :ImageURL }, 
				:RecipeSource => { :only => :RecipeSourceName },
				:RecipeIngredientMeasurements => {
					:include => { 
						:IngredientMeasurement => {
							:include => { :Ingredient => { :only => :IngredientName } }, 
							:only => :Ingredient
						}
					},
					:only => :IngredientMeasurement
				}
			},
			:only => [:RecipeName]})
  end
=begin
  def recipe
    return @recipe if defined?(@recipe)
    @recipe = Recipe.
              includes(instructions: { location: :ingredient }).
              find_by!(guid: params['id']).
              decorate
  end
=end
end
