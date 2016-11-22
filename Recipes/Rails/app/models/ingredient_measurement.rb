# NOTE database naming convention doesn't match ruby standards, since we're reusing it though we'll stick to the MS naming convention
class IngredientMeasurement < ActiveRecord::Base
	self.table_name = "IngredientMeasurement"
	belongs_to :Ingredient, :foreign_key => :IngredientID
	belongs_to :Measurement, :foreign_key => :MeasurementID
end