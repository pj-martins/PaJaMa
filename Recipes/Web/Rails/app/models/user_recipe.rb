class UserRecipe < ActiveRecord::Base
    self.table_name = 'UserRecipe'
    self.primary_key = :UserRecipeID

    belongs_to :recipe, :class_name => 'Recipe', :foreign_key => :RecipeID
    belongs_to :user, :class_name => 'User', :foreign_key => :UserID
end
