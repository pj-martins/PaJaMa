(function () {
	var RecipeViewController = function ($scope, $routeParams) {
		$scope.recipeId = ($routeParams.recipeId) ? parseInt($routeParams.recipeId) : 0;
	};

	angular.module('recipeSearch').controller('RecipeViewController', RecipeViewController);
}());