(function () {
	var AddIngredientController = function ($scope, $http, appSettings) {
		$scope.addedIngredient = null;
		$scope.addedIngredientMeasurement = null;
		$scope.addedQuantity = 0;
		$scope.loadingIngredient = false;

		$scope.getIngredients = function (val) {
			if (val == "" || val.length < 3) return [];
			$scope.loadingIngredient = true;
			return $http.get(appSettings.apiUrl + '/api/ingredient?partial=' + val)
				.then(function (response) {
					$scope.loadingIngredient = false;
					return response.data;
				});
		};

		$scope.getMeasurements = function (val) {
			if ($scope.addedIngredient == null) return [];
			return $scope.addedIngredient.PossibleIngredientMeasurements;
		};

		$scope.hideAddIngredient = function () {
			$scope.$parent.addingIngredient = false;
		};

		$scope.addIngredient = function () {
			if ($scope.addedIngredientMeasurement != null) {
				var rec = $scope.$parent.recipe;
				if (rec.RecipeIngredients == null)
					rec.RecipeIngredients = [];

				rec.RecipeIngredients.push(
					{
						Alternates: [],
						ActiveAlternate: null,
						Exclude: false,
						Quantity: $scope.addedQuantity,
						Ingredient: $scope.addedIngredientMeasurement
					});

				$scope.addedQuantity = 0;
				$scope.addedIngredient = null;
				$scope.addedIngredientMeasurement = null;

				$scope.$parent.updateNutritionInfo();
			}
			$scope.hideAddIngredient();
		};
	};

	angular.module('recipeSearch').controller('AddIngredientController', AddIngredientController);
}());