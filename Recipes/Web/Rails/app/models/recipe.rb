class Recipe < ActiveRecord::Base


    self.table_name = 'Recipe'
    self.primary_key = :RecipeID

    has_many :userrecipes, :class_name => 'UserRecipe', :foreign_key => :RecipeID
    has_many :recipeimages, :class_name => 'RecipeImage', :foreign_key => :RecipeID
    has_many :recipeingredientmeasurements, :class_name => 'RecipeIngredientMeasurement', :foreign_key => :RecipeID
    belongs_to :recipesource, :class_name => 'RecipeSource', :foreign_key => :RecipeSourceID

  def Source
      return recipesource.RecipeSourceName
  end
end
