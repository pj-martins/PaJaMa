class RecipeSource < ActiveRecord::Base
    self.table_name = 'RecipeSource'
    self.primary_key = :RecipeSourceID

    has_many :recipes, :class_name => 'Recipe', :foreign_key => :RecipeSourceID
end
