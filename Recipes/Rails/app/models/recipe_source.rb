# NOTE database naming convention doesn't match ruby standards, since we're reusing it though we'll stick to the MS naming convention
class RecipeSource < ActiveRecord::Base
	self.table_name = "RecipeSource"
	has_many :Recipe, :foreign_key => :RecipeSourceID
end