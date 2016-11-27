class Api::V1::LookupsController < Api::ApiController
  def recipe_sources
    render json: RecipeSource.all;
  end
end
