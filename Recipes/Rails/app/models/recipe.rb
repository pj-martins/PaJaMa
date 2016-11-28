# NOTE database naming convention doesn't match ruby standards, since we're reusing it though we'll stick to the MS naming convention
class Recipe < ActiveRecord::Base
	self.table_name = "Recipe"

	has_many :RecipeIngredientMeasurements, :foreign_key => :RecipeID
	has_many :RecipeImages, :foreign_key => :RecipeID
	has_one :RecipeSource, :foreign_key => :RecipeSourceID, :primary_key => :RecipeSourceID

	def self.get_random(random)
		# only find recipes with images
		return Recipe.where("Rating >= ?", 4).joins(:RecipeImages).limit(random).order("RANDOM()")
	end

	def self.search_recipes(includes = nil, excludes = nil, rating = nil, bookmarked = nil, recipe_source_id = nil, pictures_only = false, page = 1, page_size = 20)
		page ||= 1
		page_size ||= 20
        curr = Recipe;
		if (recipe_source_id)
			curr = curr.where(RecipeSourceID: recipe_source_id.to_f)
		end

		if (rating)
			curr = curr.where("Rating >= ?", rating.to_f)
		end

		if (includes)
			parts = includes.split(';')
			include_ids = nil;
			for ing in parts
				curr_ids = RecipeIngredientMeasurement.joins(:IngredientMeasurement => :Ingredient).where("IngredientName like '%#{ing}%'").pluck(:RecipeID)
				if include_ids === nil
					include_ids = curr_ids
				else
					logger.debug "include: #{include_ids}"
					logger.debug "curr: #{curr_ids}"
					include_ids &= curr_ids
				end
			end
			curr = curr.where(RecipeID: include_ids)
		end

		return curr.limit(page_size.to_f).offset((page.to_f - 1) * page_size.to_f)
    end

	def self.get_recipe(recipe_id)
		return Recipe.find(recipe_id)
	end
end