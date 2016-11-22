# NOTE database naming convention doesn't match ruby standards, since we're reusing it though we'll stick to the MS naming convention
class RecipeImage < ActiveRecord::Base
	self.table_name = "RecipeImage"
end