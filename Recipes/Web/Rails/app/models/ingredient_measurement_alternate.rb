class IngredientMeasurementAlternate < ActiveRecord::Base
    self.table_name = 'IngredientMeasurementAlternate'
    self.primary_key = :IngredientMeasurementAlternateID

    belongs_to :ingredientmeasurement, :class_name => 'IngredientMeasurement', :foreign_key => :FromIngredientMeasurementID
    belongs_to :ingredientmeasurement, :class_name => 'IngredientMeasurement', :foreign_key => :ToIngredientMeasurementID
end
