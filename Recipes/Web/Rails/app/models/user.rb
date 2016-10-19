class User < ActiveRecord::Base
    self.table_name = 'User'
    self.primary_key = :UserID

    has_many :userrecipes, :class_name => 'UserRecipe', :foreign_key => :UserID
end
