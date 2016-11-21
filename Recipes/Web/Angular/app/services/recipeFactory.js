(function () {
	var recipeFactory = function ($http, $q, quantityDisplayService, appSettings, entityFactory) {
		var factory = {};
		factory.getRecipe = function (recipeId, modal) {
			var deferred = $q.defer();

			var currRecipe = null;
			var initRecipe = function () {
				if (currRecipe.numberOfServings == null)
					currRecipe.numberOfServings = 0;
				for (i = 0; i < currRecipe.recipeIngredientMeasurements.length; i++) {
					var ri = currRecipe.recipeIngredientMeasurements[i];
					if (ri.alternates.length > 0) {
						var orig = { toIngredientMeasurement: ri.ingredientMeasurement, multiplier: 1 };
						ri.alternates.splice(0, 0, orig);
						ri.activeAlternate = orig;
					}
				}


				currRecipe.originalNumberOfServings = currRecipe.numberOfServings;
				factory.updateNutritionInfo(currRecipe);
				currRecipe.images = [];
			}

			entityFactory.getEntity('Recipe', recipeId, { baseUrl: appSettings.apiUrl })
				.then(function (data) {
					currRecipe = data;
					initRecipe();
					deferred.resolve(currRecipe);
				},
				function (data) {
					deferred.reject(data);
				});

			return deferred.promise;
		};

		factory.updateNutritionInfo = function (recipe) {
			recipe.totalCalories = 0;
			recipe.totalCarbohydrates = 0;
			recipe.totalFat = 0;
			recipe.totalSaturatedFat = 0;
			recipe.totalProtein = 0;
			recipe.totalSugars = 0;
			for (var i = 0; i < recipe.recipeIngredientMeasurements.length; i++) {
				var ri = recipe.recipeIngredientMeasurements[i];
				if (!ri.exclude) {
					var activeIngr = ri.activeAlternate == null ? ri.ingredientMeasurement : ri.activeAlternate.toIngredientMeasurement;
					recipe.totalCalories += ri.quantity * activeIngr.caloriesPer;
					recipe.totalCarbohydrates += ri.quantity * activeIngr.carbohydratesPer;
					recipe.totalFat += ri.quantity * activeIngr.fatPer;
					recipe.totalSaturatedFat += ri.quantity * activeIngr.saturatedFatPer;
					recipe.totalProtein += ri.quantity * activeIngr.proteinPer;
					recipe.totalSugars += ri.quantity * activeIngr.sugarsPer;
				}
			}
			if (recipe.numberOfServings != 0) {
				recipe.totalCalories /= recipe.numberOfServings;
				recipe.totalCarbohydrates /= recipe.numberOfServings;
				recipe.totalFat /= recipe.numberOfServings;
				recipe.totalSaturatedFat /= recipe.numberOfServings;
				recipe.totalProtein /= recipe.numberOfServings;
				recipe.totalSugars /= recipe.numberOfServings;
			}
			recipe.totalCalories = Math.round(recipe.totalCalories);
			recipe.totalCarbohydrates = Math.round(recipe.totalCarbohydrates);
			recipe.totalFat = Math.round(recipe.totalFat);
			recipe.totalSaturatedFat = Math.round(recipe.totalSaturatedFat);
			recipe.totalProtein = Math.round(recipe.totalProtein);
			recipe.totalSugars = Math.round(recipe.totalSugars);
		};

		factory.updateRecipeIngredientQuantities = function (recipe) {
			var fraction = recipe.numberOfServings / recipe.originalNumberOfServings;
			for (i = 0; i < recipe.recipeIngredientMeasurements.length; i++) {
				var ri = recipe.recipeIngredientMeasurements[i];
				if (ri.originalQuantity === undefined)
					return;
				ri.quantity = (ri.activeAlternate == null ? 1 : ri.activeAlternate.multiplier) * ri.originalQuantity * fraction;
			}
		};

		factory.setRating = function (recipe, rating) {
			if (recipe.userRating == rating)
				rating = 0;
			recipe.userRating = rating;
			return factory.saveRecipe(recipe);
		};

		factory.saveRecipe = function (recipe) {
			return $http.post(appSettings.apiUrl + '/api/recipe', recipe);
		};

		return factory;
	};

	angular.module('recipeSearch').factory('recipeFactory', recipeFactory);
}());