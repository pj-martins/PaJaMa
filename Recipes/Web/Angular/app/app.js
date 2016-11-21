(function () {
	angular.module('recipeSearch', ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'ui.bootstrap.tpls',
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