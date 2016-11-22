# NOTE database naming convention doesn't match ruby standards, since we're reusing it though we'll stick to the MS naming convention
class Recipe < ActiveRecord::Base
	self.table_name = "Recipe"

	has_many :RecipeIngredientMeasurements, :foreign_key => :RecipeID
	has_many :RecipeImages, :foreign_key => :RecipeID
	has_one :RecipeSource, :foreign_key => :RecipeSourceID, :primary_key => :RecipeSourceID

	def self.get_random(random)
		return Recipe.limit(random).order("RANDOM()")
	end

	def self.search_recipes(params) #includes, excludes, rating, bookmarked, recipe_source_id, pictures_only, page, page_size)
		params[:page] ||= 1
		params[:pageSize] ||= 20
        curr = Recipe;
		if (params[:recipeSourceID])
			curr = curr.where(RecipeSourceID: params[:recipeSourceID].to_f)
		end

		if (params[:rating])
			curr = curr.where("Rating >= ?", params[:rating].to_f)
		end

		if (params[:includes])

		end

		return curr.limit(params[:pageSize].to_f).offset((params[:page].to_f - 1) * params[:pageSize].to_f)
    end
end