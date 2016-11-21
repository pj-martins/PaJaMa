(function () {
	var RecipeImageController = function ($scope) {
		$scope.parentScope = $scope.$parent;
		$scope.myInterval = 5000;
		$scope.noWrapSlides = false;
		$scope.active = 0;
	};

	angular.module('recipeSearch').controller('RecipeImageController', RecipeImageController);
}());