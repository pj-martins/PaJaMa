(function () {
	angular.module('recipeSearch', ['ngRoute', 'mgcrea.ngStrap', 'ui.bootstrap.carousel', 'ui.bootstrap.tpls', 'ui.bootstrap.tooltip',
		'infinite-scroll', 'LocalStorageModule', 'pjm'])

		.config(function ($httpProvider, $routeProvider) {
			var viewBase = 'app/views/';
			$routeProvider
			.when('/recipe/:recipeId', {
				controller: 'RecipeViewController',
				templateUrl: viewBase + 'recipeView.html'
			})
			.otherwise({
				controller: 'RecipeSearchController',
				templateUrl: viewBase + 'recipeSearch.html'
			});
		});
}());