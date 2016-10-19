class IngredientMeasurement < ActiveRecord::Base
    self.table_name = 'IngredientMeasurement'
    self.primary_key = :IngredientMeasurementID

    belongs_to :ingredient, :class_name => 'Ingredient', :foreign_key => :IngredientID
    has_many :ingredientmeasurementalternates, :class_name => 'IngredientMeasurementAlternate'
    has_many :ingredientmeasurementalternates, :class_name => 'IngredientMeasurementAlternate'
    has_many :recipeingredientmeasurements, :class_name => 'RecipeIngredientMeasurement', :foreign_key => :IngredientMeasurementID
    belongs_to :measurement, :class_name => 'Measurement', :foreign_key => :MeasurementID

end
