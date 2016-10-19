(function () {
	var recipe = function (recipeFactory, localStorageService, appSettings) {
		return {
			scope: { recipeId: '=', modal: '@' },
			templateUrl: 'app/views/recipe.html',
			link: function ($scope, element, attrs) {
				$scope.editingServings = false;
				$scope.editingRecipeName = false;
				$scope.editingNotes = false;
				$scope.editingRecipe = false;
				$scope.addingIngredient = false;

				$scope.pdfOpen = false;
				$scope.pdfIncludePicture = true;
				$scope.pdfIncludeNutrition = true;
				$scope.pdfIncludeRating = true;

				$scope.emailOpen = false;
				$scope.emailTo = 'pj_martins@hotmail.com';
				$scope.emailSubject = 'test';
				$scope.emailMessage = 'test';

				$scope.sources = null;
				$scope.lockServings = false;

				recipeFactory.getRecipe($scope.recipeId, $scope.modal).then(function (recipe) {
					$scope.recipe = recipe;

					$scope.toggleServingsEdit = function (editing) {
						if (!editing && $scope.recipe.NumberOfServings == null) return;
						$scope.editingServings = editing;
					};

					$scope.toggleRecipeNameEdit = function (editing) {
						$scope.editingRecipeName = editing;
					};

					$scope.isLoggedIn = function () {
						return localStorageService.get('loginData') != null;
					};

					$scope.doneEditingNotes = function () {
						$scope.postRecipe('Notes updated.');
						$scope.editingNotes = false;
					}

					$scope.togglePdf = function () {
						$scope.pdfOpen = !$scope.pdfOpen;
						alert($scope.pdfOpen);
					};

					$scope.downloadPDF = function () {
						$scope.pdfOpen = false;
						if (!$scope.pdfIncludePicture)
							pdfService.downloadPDF($scope.recipe, null, $scope.pdfIncludeNutrition, $scope.pdfIncludeRating);
						else {
							$http.post(appSettings.apiUrl + '/api/image', { ImageURL: $scope.recipe.getActiveImageURL() })
							.success(function (data, status, headers, config) {
								pdfService.downloadPDF($scope.recipe, data, $scope.pdfIncludeNutrition, $scope.pdfIncludeRating);
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

					$scope.permalink = function () {
						$scope.recipe.permalink();
					};
				}, function (error) { alert(error.mesage); });
			}
		};
	};

	angular.module('recipeSearch').directive('recipe', recipe);
}());