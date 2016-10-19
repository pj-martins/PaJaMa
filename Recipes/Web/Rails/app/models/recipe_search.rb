class RecipeSearch < ActiveRecord::Base
    self.table_name = 'RecipeSearch'
    self.primary_key = :RecipeID

    def self.searchRecipes(params)
        if (params[:includes].nil? && params[:excludes].nil?)
          return self.searchRecipesNoKeywords(params)
        end

        sql = "select top 1000 r.* from Recipe r with (nolock)
	join RecipeSearch rs with (nolock) on rs.RecipeID = r.RecipeID
	join RecipeSource so with (nolock) on so.RecipeSourceID = r.RecipeSourceID
"

        sql << "where 1 = 1 "

        sqlpars = { }

        unless params[:rating].nil?
            sql << "and Rating >= :rating "
            sqlpars[:rating] = params[:rating]
        end

        includes = params[:includes]
        unless includes.nil?
            parts = includes.split('_')
            includes = "\"" + includes.sub("_", " ").sub(" ", "*\" AND \"").sub("'", "''") + "*\""
            sql << "and ((contains(rs.Ingredients, :includes) or contains(rs.RecipeName, :includes))"
            sqlpars[:includes] = includes
            if parts.size > 1
                rest = ""
                for index in 1 ... parts.size
                    rest << parts[index] + " "
                end
                rest.strip
                sql << " or (contains(rs.RecipeName, :recipeName) and contains(rs.Ingredients, :restIngredients))"
                sqlpars[:recipeName] = "\"" + parts[0].sub(" ", "*\" AND \"") +	"*\""
                sqlpars[:restIngredients] = "\"" + rest.sub(" ", "*\" AND \"") +	"*\""
            end
            sql << ")"
        end

        numRecipes = @connection.execute("select count(*) from (" + sql + ")")[0]

        #pageNum = params["$skip"]

        paginateSql = "select *, RowNum = row_number() over (partition by RecipeName order by RecipeName) from ("
        paginateSql << sql
        paginateSql << ") z where RowNum > "

        recipes = Recipe.find_by_sql([sql, sqlpars])
        ActiveRecord::Associations::Preloader.new(recipes, recipeingredientmeasurements:
                                                             {ingredientmeasurement: [:ingredient, :measurement]}).run
        ActiveRecord::Associations::Preloader.new(recipes, :recipeimages).run
        ActiveRecord::Associations::Preloader.new(recipes, :recipesource).run



        return recipes.map{|r| RecipeSearchResultDto.new(r)}
    end

    def self.searchRecipesNoKeywords(params)
        recipes = Recipe.includes([:recipeimages, :recipesource, recipeingredientmeasurements:
            {ingredientmeasurement: [:ingredient, :measurement]}])
        unless (params[:rating].nil?)
          recipes = recipes.where(["Rating >= ?", params[:rating]])
        end
        unless (params[:bookmarked].nil?)
          recipes = recipes.where("IsBookmarked = 1")
        end
        unless (params[:recipeSourceID].nil?)
          recipes = recipes.where(["RecipeSourceID = ?", params[:recipeSourceID]])
        end
        unless (params[:picturesOnly].nil?)
          recipes = recipes.includes(:recipeimages).where("RecipeImage.RecipeImageID IS NOT NULL")
        end

        numRecipes = recipes.count
        recipes = recipes.order(:RecipeName).page(params["$skip"].nil? ? 1 : params["$skip"].to_i).per_page(params["$top"].nil? ? 1000 : params["$top"].to_i)
        return {:recipes => recipes.map{ |r| RecipeSearchResultDto.new(r)}, :numRecipes => numRecipes }
    end

end
