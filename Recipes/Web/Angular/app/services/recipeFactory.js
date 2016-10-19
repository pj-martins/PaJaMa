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

					ri.friendlyQuantity = function () {
						return quantityDisplayService.getFriendlyQuantity(this.quantity);
					};
					ri.getActiveIngredient = function () {
						return this.activeAlternate == null ? this.ingredientMeasurement : this.activeAlternate.toIngredientMeasurement;
					};
				}

				currRecipe.updateNutritionInfo = function () {
					this.totalCalories = 0;
					this.totalCarbohydrates = 0;
					this.totalFat = 0;
					this.totalSaturatedFat = 0;
					this.totalProtein = 0;
					this.totalSugars = 0;
					for (i = 0; i < this.recipeIngredientMeasurements.length; i++) {
						var ri = this.recipeIngredientMeasurements[i];
						if (!ri.exclude) {
							var activeIngr = ri.activeAlternate == null ? ri.ingredientMeasurement : ri.activeAlternate.toIngredientMeasurement;
							this.totalCalories += ri.quantity * activeIngr.caloriesPer;
							this.totalCarbohydrates += ri.quantity * activeIngr.carbohydratesPer;
							this.totalFat += ri.quantity * activeIngr.fatPer;
							this.totalSaturatedFat += ri.quantity * activeIngr.saturatedFatPer;
							this.totalProtein += ri.quantity * activeIngr.proteinPer;
							this.totalSugars += ri.quantity * activeIngr.sugarsPer;
						}
					}
					if (this.numberOfServings != 0) {
						this.totalCalories /= this.numberOfServings;
						this.totalCarbohydrates /= this.numberOfServings;
						this.totalFat /= this.numberOfServings;
						this.totalSaturatedFat /= this.numberOfServings;
						this.totalProtein /= this.numberOfServings;
						this.totalSugars /= this.numberOfServings;
					}
					this.totalCalories = Math.round(this.totalCalories);
					this.totalCarbohydrates = Math.round(this.totalCarbohydrates);
					this.totalFat = Math.round(this.totalFat);
					this.totalSaturatedFat = Math.round(this.totalSaturatedFat);
					this.totalProtein = Math.round(this.totalProtein);
					this.totalSugars = Math.round(this.totalSugars);
				};

				currRecipe.updateRecipeIngredientQuantities = function () {
					var fraction = this.numberOfServings / this.originalNumberOfServings;
					for (i = 0; i < this.recipeIngredientMeasurements.length; i++) {
						var ri = this.recipeIngredientMeasurements[i];
						if (ri.originalQuantity === undefined)
							return;
						ri.quantity = (ri.activeAlternate == null ? 1 : ri.activeAlternate.multiplier) * ri.originalQuantity * fraction;
					}
				};

				currRecipe.bookmark = function () {
					if (!this.editingRecipe)
						this.postRecipe('Bookmark changed.');
				};

				currRecipe.setRating = function (rating) {
					if (this.UserRating == rating)
						rating = 0;
					this.UserRating = rating;
					if (!this.editingRecipe)
						this.postRecipe('Rating changed.');
				};

				currRecipe.saveRecipe = function () {
					this.IsDirty = true;
					this.postRecipe('Recipe saved.', function () {
						this.IsDirty = false;
						this.editingRecipe = false;
						this.editingNotes = false;
					});
				};

				currRecipe.postRecipe = function (successMessage, postSave) {
					$('#loadingDiv').show();
					$http.post(appSettings.apiUrl + '/api/recipe', this)
						.success(function (data, status, headers, config) {
							$('#loadingDiv').hide();
							alert(successMessage);
							if (data != null) this.id = data.id;
							if (postSave != null) postSave();
						})
						.error(function (data, status, headers, config) {
							$('#loadingDiv').hide();
							alert('error: \n' + JSON.stringify(data) + '\n' + JSON.stringify(status) + '\n' + JSON.stringify(headers) + '\n' + JSON.stringify(config));
						});
				};

				currRecipe.setImage = function () {
					if (!this.editingRecipe) return;
				};

				currRecipe.recipeUrl = function () {
					return '#/recipe/' + this.id;
				};

				currRecipe.permalink = function () {
					window.open(this.recipeUrl());
				};


				currRecipe.originalNumberOfServings = currRecipe.numberOfServings;
				currRecipe.updateNutritionInfo();
				currRecipe.images = [];

				currRecipe.getActiveImageURL = function () {
					for (i = 0; i < this.recipeImages.length; i++) {
						if (this.recipeImages[i].active) {
							return this.recipeImages[i].imageURL;
							break;
						}
					}

					return this.recipeImages[0].imageURL;
				};
			}

			if (recipeId <= 0) {
				currRecipe = {
					recipeName: 'New Recipe',
					numberOfServings: 4,
					rating: 0,
					userRating: 0,
					recipeImages: [{ imageURL: appSettings.apiUrl + '/api/image/food.png' }]
				};
				initRecipe();
				currRecipe.editingRecipe = true;
				currRecipe.editingNotes = true;
				deferred.resolve(currRecipe);
			}
			else {
				if (!modal)
					$('#loadingDiv').show();

			    entityFactory.getEntity('Recipe', recipeId, { baseUrl: appSettings.apiUrl, dynamic: true })
					.then(function (data) {
						currRecipe = data;
						if (modal) {
							//alert(angular.element('.recipeIngredientMeasurements').height());
							//$('.modal-body').height(angular.element('.recipeIngredientMeasurements').height());
						}
						else
							$('#loadingDiv').hide();
						initRecipe();
						deferred.resolve(currRecipe);
					},
					function (data) {
						alert('error: ' + JSON.stringify(data));
						deferred.reject(data);
					});
			}

			return deferred.promise;
		};
		return factory;
	};

	angular.module('recipeSearch').factory('recipeFactory', recipeFactory);
}());