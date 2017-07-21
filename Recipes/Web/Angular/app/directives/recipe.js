(function () {
	var recipe = function (recipeFactory, localStorageService, appSettings, $http, pdfService, quantityDisplayService) {
		return {
			scope: { recipeId: '=', modal: '@' },
			templateUrl: 'app/views/recipe.html',
			link: function ($scope, element, attrs) {
				$scope.editingServings = false;
				$scope.editingRecipeName = false;
				$scope.editingNotes = false;
				$scope.editingRecipe = false;
				$scope.addingIngredient = false;
				$scope.activeImageIndex = 0;
				$scope.quantityDisplayService = quantityDisplayService;

				$scope.pdfOpen = false;
				$scope.pdfIncludePicture = true;
				$scope.pdfIncludeNutrition = true;
				$scope.pdfIncludeRating = true;

				$scope.emailOpen = false;
				$scope.emailTo = 'pj_martins@outlook.com';
				$scope.emailSubject = 'test';
				$scope.emailMessage = 'test';

				$scope.sources = null;
				$scope.lockServings = false;

				$scope.toggleServingsEdit = function (editing) {
					if (!editing && $scope.recipe.NumberOfServings == null) return;
					$scope.editingServings = editing;
				};

				$scope.toggleRecipeNameEdit = function (editing) {
					$scope.editingRecipeName = editing;
				};

				$scope.isLoggedIn = function () {
					return true;
					// return localStorageService.get('loginData') != null;
				};

				$scope.doneEditingNotes = function () {
					$scope.postRecipe('Notes updated.');
					$scope.editingNotes = false;
				}

				$scope.togglePdf = function () {
					$scope.pdfOpen = !$scope.pdfOpen;
				};

				$scope.downloadPDF = function () {
					$scope.pdfOpen = false;
					if (!$scope.pdfIncludePicture) {
						pdfService.downloadPDF($scope.recipe, null, null, $scope.pdfIncludeNutrition, $scope.pdfIncludeRating);
					}
					else {
						$http.get(appSettings.apiUrl + '/api/image/getImage?imageURL=' + $scope.recipe.recipeImages[$scope.activeImageIndex].imageURL)
						.success(function (data, status, headers, config) {
							pdfService.downloadPDF($scope.recipe, data, headers('Content-Type'), $scope.pdfIncludeNutrition, $scope.pdfIncludeRating);
						})
						.error(function (data, status, headers, config) {
							alert('error: \n' + JSON.stringify(data) + '\n' + JSON.stringify(status) + '\n' + JSON.stringify(headers) + '\n' + JSON.stringify(config));
						});
					}
				};

				$scope.sendEmail = function () {
					$scope.emailOpen = false;
					emailService.sendEmail($scope.recipe, { emailTo: $scope.emailTo, emailSubject: $scope.emailSubject, emailMessage: $scope.emailMessage });
				}

				$scope.removeIngredient = function (recipeIngredient) {
					var index = $scope.recipe.RecipeIngredients.indexOf(recipeIngredient);
					if (index != -1) {
						$scope.recipe.RecipeIngredients.splice(index, 1);
						$scope.updateNutritionInfo();
					}
				};

				$scope.getActiveIngredient = function (recipeIngredient) {
					return recipeIngredient.activeAlternate == null ? recipeIngredient.ingredientMeasurement : recipeIngredient.activeAlternate.toIngredientMeasurement;
				};

				$scope.permalink = function () {
					window.open('#/recipe/' + $scope.recipe.id);
				};

				$scope.bookmark = function () {
					$http.post(appSettings.apiUrl + '/api/recipe/bookmarkRecipe', $scope.recipe.id)
						.success(function (data, status, headers, config) {
							alert('recipe bookmarked');
						})
						.error(function (data, status, headers, config) {
							alert('error: \n' + JSON.stringify(data) + '\n' + JSON.stringify(status) + '\n' + JSON.stringify(headers) + '\n' + JSON.stringify(config));
						});
				}

				$scope.setRating = function (rating) {
					if ($scope.recipe.userRating == rating)
						rating = 0;
					$http.post(appSettings.apiUrl + '/api/recipe/SetRecipeRating', { recipeID: $scope.recipe.id, rating: rating })
						.success(function (data, status, headers, config) {
							$scope.recipe.userRating = rating;
							alert('recipe rated');
						})
						.error(function (data, status, headers, config) {
							alert('error: \n' + JSON.stringify(data) + '\n' + JSON.stringify(status) + '\n' + JSON.stringify(headers) + '\n' + JSON.stringify(config));
						});
				}

				recipeFactory.getRecipe($scope.recipeId, $scope.modal).then(function (recipe) {
					$scope.recipe = recipe;
				}, function (error) { alert(error.mesage); });
			}
		};
	};

	angular.module('recipeSearch').directive('recipe', recipe);
}());