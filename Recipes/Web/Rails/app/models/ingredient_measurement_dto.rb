class IngredientMeasurementDto
  def initialize(ingMeasurement)
    @ingredient = ingMeasurement.ingredient.IngredientName
    @measurement = ingMeasurement.measurement.nil? ? "" : ingMeasurement.measurement.MeasurementName
    @caloriesPer = ingMeasurement.CaloriesPer
  end
end