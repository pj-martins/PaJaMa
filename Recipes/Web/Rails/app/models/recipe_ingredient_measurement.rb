class RecipeIngredientMeasurement < ActiveRecord::Base
    self.table_name = 'RecipeIngredientMeasurement'
    self.primary_key = :RecipeIngredientMeasurementID

    belongs_to :ingredientmeasurement, :class_name => 'IngredientMeasurement', :foreign_key => :IngredientMeasurementID
    belongs_to :recipe, :class_name => 'Recipe', :foreign_key => :RecipeID

  def description
      rtv = ""
      unless self.Quantity.nil?
        rtv << self.Quantity.to_s + " "
      end
      unless ingredientmeasurement.measurement.nil?
        rtv << ingredientmeasurement.measurement.MeasurementName + " "
      end
      rtv << ingredientmeasurement.ingredient.IngredientName
  end
end
