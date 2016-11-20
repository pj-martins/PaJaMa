(function () {
	var RecipeIngredientController = function ($scope, $http, recipeFactory) {
		$scope.recipeIngredient.alternating = false;
		$scope.recipeIngredient.originalQuantity = $scope.recipeIngredient.quantity;
		$scope.recipeIngredient.include = !$scope.recipeIngredient.exclude;

		$scope.pickAlternate = function () {
			$scope.recipeIngredient.alternating = true;
		};

		$scope.hideAlternate = function () {
			$scope.recipeIngredient.alternating = false;
		};

		$scope.updateQuantitiesNutrition = function () {
		    recipeFactory.updateRecipeIngredientQuantities($scope.recipe);
		    recipeFactory.updateNutritionInfo($scope.recipe);
		};

		$scope.updateNutrition = function () {
			$scope.recipeIngredient.exclude = !$scope.recipeIngredient.include;
			recipeFactory.updateNutritionInfo($scope.recipe);
		};
	};

	angular.module('recipeSearch').controller('RecipeIngredientController', RecipeIngredientController);
}());