(function () {
	var RecipeIngredientController = function ($scope, $http) {
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
			$scope.recipe.updateRecipeIngredientQuantities();
			$scope.recipe.updateNutritionInfo();
		};

		$scope.updateNutrition = function () {
			$scope.recipeIngredient.exclude = !$scope.recipeIngredient.include;
			$scope.recipe.updateNutritionInfo();
		};
	};

	angular.module('recipeSearch').controller('RecipeIngredientController', RecipeIngredientController);
}());