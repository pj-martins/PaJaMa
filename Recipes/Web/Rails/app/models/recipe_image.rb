class RecipeImage < ActiveRecord::Base
    self.table_name = 'RecipeImage'
    self.primary_key = :RecipeImageID

    belongs_to :recipe, :class_name => 'Recipe', :foreign_key => :RecipeID
end
