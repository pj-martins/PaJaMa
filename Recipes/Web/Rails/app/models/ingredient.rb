class Ingredient < ActiveRecord::Base
    self.table_name = 'Ingredient'
    self.primary_key = :IngredientID

    has_many :ingredientmeasurements, :class_name => 'IngredientMeasurement', :foreign_key => :IngredientID
end
