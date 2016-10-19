class Measurement < ActiveRecord::Base
    self.table_name = 'Measurement'
    self.primary_key = :MeasurementID

    has_many :ingredientmeasurements, :class_name => 'IngredientMeasurement', :foreign_key => :MeasurementID
end
