class RecipeController < ActionController::Base

  def search
		recipes = RecipeSearch.searchRecipes(params)
		response.headers["X-InlineCount"] = recipes[:numRecipes].to_s
		render json: recipes[:recipes]
	end

	def recipe
		rec = Recipe.find(params[:id])
		render json: RecipeDto.new(rec)
	end
=begin
		results = client.execute(sql)
		recipes = results.map{|r| RecipeSearchResultDto.new(r["RecipeID"], r["RecipeName"], r["Rating"], r["Source"])}

		idsToPull = recipes.map{|r| r.recipeID}
		sql = "select RecipeID, IngredientName, MeasurementName, Quantity from RecipeIngredientMeasurement rim with (nolock)
	join IngredientMeasurement im with (nolock) on im.IngredientMeasurementID = rim.IngredientMeasurementID
	left join Measurement m with (nolock) on m.MeasurementID = im.MeasurementID
	join Ingredient i with (nolock) on i.IngredientID = im.IngredientID
	where RecipeID in("

		sql << idsToPull.join(",") + ")"

		allIngrs = client.execute(sql)

		recipes.each do |rec|
			myIngrs = allIngrs.find_all{|i| i["RecipeID"] == rec.recipeID}
			myIngrs.each do |ing|
				rec.ingredients << "#{ing['Quantity']}" + " " +
					(ing["MeasurementName"].nil? ? "" : ing["MeasurementName"] + " ") +
					(ing["IngredientName"].nil? ? "" : ing["IngredientName"] + " ")
			end
		end


		sql = "select RecipeID, ImageURL from RecipeImage with (nolock)
	where RecipeID in("
		sql << idsToPull.join(",") + ")"

		allImgs = client.execute(sql)

		recipes.each do |rec|
			myImg = allImgs.find_all{|i| i["RecipeID"] == rec.recipeID}.first;
			 unless myImg.nil?
				 rec.imageURL = myImg["ImageURL"]
			 end
		end

		render json: RecipeSearchResultsDto.new(recipes.size, recipes)
=end
end
