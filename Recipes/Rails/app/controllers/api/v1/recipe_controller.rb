class Api::V1::RecipeController < Api::ApiController
  def index
    render json: self.jsonify(Recipe.get_recipe(params[:id]))
  end

  def jsonify(recipes)
	return recipes.as_json({
			:include => {
				:RecipeImages => { :only => [:ImageURL,:Sequence] }, 
				:RecipeSource => { :only => :RecipeSourceName },
				:RecipeIngredientMeasurements => {
					:include => { 
						:IngredientMeasurement => { :include => [:Ingredient,:Measurement] }
					}
				}
			}
		})
  end
end
