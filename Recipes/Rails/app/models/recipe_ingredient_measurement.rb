# NOTE database naming convention doesn't match ruby standards, since we're reusing it though we'll stick to the MS naming convention
class RecipeIngredientMeasurement < ActiveRecord::Base
	self.table_name = "RecipeIngredientMeasurement"
	belongs_to :IngredientMeasurement, :foreign_key => :IngredientMeasurementID
end