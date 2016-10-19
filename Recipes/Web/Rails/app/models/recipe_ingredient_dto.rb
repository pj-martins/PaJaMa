class RecipeIngredientDto
  def initialize(recIngMeasurement)
    @quantity = recIngMeasurement.Quantity
    @ingredientMeasurement = IngredientMeasurementDto.new(recIngMeasurement.ingredientmeasurement)
  end
end