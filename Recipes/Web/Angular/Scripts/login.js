angular.module('login', [])
.config(function ($httpProvider) {
	$httpProvider.defaults.useXDomain = true;
	delete $httpProvider.defaults.headers.common['X-Requested-With'];

})
.controller('loginController', function ($scope, $rootScope, $http, localStorageService) {
	$scope.userName = "demo";
	$scope.password = "demo";
	$scope.loggingIn = false;
	$scope.myRecipes = [];

	$scope.init = function (apiURL) {
		$scope.apiURL = apiURL;
	};

	$scope.showLogin = function () {
		$scope.loggingIn = true;
	};

	$scope.login = function () {
		var data = "grant_type=password&username=" + $scope.userName + "&password=" + $scope.password;
		$http.post($scope.apiURL + '/Token', data, {
			headers:
			{ 'Content-Type': 'application/x-www-form-urlencoded' }
		})
		.success(function (data, status, headers, config) {
			$scope.loggingIn = false;
			localStorageService.add('loginData', data);
		})
		.error(function (data, status, headers, config) {
			alert('error: \n' + JSON.stringify(data) + '\n' + JSON.stringify(status) + '\n' + JSON.stringify(headers) + '\n' + JSON.stringify(config));
		});
	};

	$scope.logout = function () {
		localStorageService.remove('loginData');
		$http.defaults.headers.common.Authorization = null;
	};

	$scope.isLoggedIn = function () {
		return localStorageService.get('loginData') != null;
	};

	$scope.loggedInUser = function () {
	    return !$scope.isLoggedIn ? "" : localStorageService.get('loginData').userName;
	};

	$scope.baseURL = function () {
		return $rootScope.baseURL;
	}

	$scope.myRecipesToggle = function (open) {
	    if (open) {
	        $http.get($scope.apiURL + '/api/recipe/editable')
                .success(function (data, status, headers, config) {
                    $scope.myRecipes = data.Recipes;
                })
                .error(function (data, status, headers, config) {
                    alert('error: ' + JSON.stringify(data));
                });
	    };
	};
})
.directive('rsRecipeLogin', function (appSettings) {
	return {
		restrict: 'E',
		templateUrl: appSettings.baseUrl + 'Templates/LoginTemplate.html'
	};
});
