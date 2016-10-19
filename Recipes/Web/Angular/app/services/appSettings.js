(function () {
	var appSettings = function () {
		this.apiUrl = 'http://localhost:50554';
	};

	angular.module('recipeSearch').service('appSettings', appSettings);
}());