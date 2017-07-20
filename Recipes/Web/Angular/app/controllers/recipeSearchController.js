(function () {
	var RecipeSearchController = function ($scope, $http, $uibModal, $sce, localStorageService, appSettings, templateFactory, entityFactory) {
		$scope.keyword = "";
		$scope.rating = 0;
		$scope.recipeName = "";
		$scope.bookmarked = false;
		$scope.picturesOnly = false;
		$scope.searchResults = null;
		$scope.totalRecords = 0;
		$scope.page = 1;
		$scope.pageSize = 100;
		$scope.disableScrolling = true;
		$scope.resultsText = "Featured Recipes:";
		$scope.editingRecipe = null;
		$scope.source = "";
		$scope.recipeSources = [];

		$scope.gridKeywords = {
			columns: [
                { fieldName: 'include', width: 30, caption: '', template: templateFactory.getIncludeTemplate() },
                { fieldName: 'keyword' },
                { fieldName: 'remove', caption: '', template: templateFactory.getRemoveTemplate(), width: 30 }
			],
			data: []
		};

		$scope.init = function () {
			var loginData = localStorageService.get('loginData');
			if (loginData != null)
				$http.defaults.headers.common.Authorization = 'Bearer ' + loginData.access_token;

			$('#loadingDiv').show();
			$http.get(appSettings.apiUrl + '/api/RecipeSearch/GetRandomRecipes?random=20').then(function (response) {
				$scope.searchResults = response.data;
				$('#loadingDiv').hide();
			}, function (data, status, headers, config) {
				alert('error: ' + JSON.stringify(data));
				$('#loadingDiv').hide();
			});

			entityFactory.getEntities('recipeSource', { baseUrl: appSettings.apiUrl }).then(function (data) {
				$scope.recipeSources = data.results;
			}, function (data, status, headers, config) {
				alert('error: ' + JSON.stringify(data));
			});
		};

		$scope.includeKeyword = function () {
			if ($scope.keyword == '') return;
			$scope.gridKeywords.data.push({ keyword: $scope.keyword, include: true });
			$scope.keyword = '';
			$scope.autoSearch();
		};

		$scope.excludeKeyword = function () {
			if ($scope.keyword == '') return;
			$scope.gridKeywords.data.push({ keyword: $scope.keyword, include: false });
			$scope.keyword = '';
			$scope.autoSearch();
		};

		$scope.keywordEnter = function () {
			$scope.includeKeyword();
		};

		$scope.removeRow = function (index) {
			$scope.gridKeywords.data.splice(index, 1);
			$scope.autoSearch();
		};

		$scope.setFilterRating = function (rating) {
			if ($scope.rating == rating)
				$scope.rating = 0;
			else
				$scope.rating = rating;
			$scope.autoSearch();
		};

		$scope.setBookmarked = function () {
			$scope.autoSearch();
		};

		$scope.setPicturesOnly = function () {
			$scope.autoSearch();
		};

		$scope.selectSource = function () {
			$scope.autoSearch();
		};

		$scope.autoSearch = function () {
			// we could fire off a search each time a search parameter changes, for testing purposes we'll disable for now
			// $scope.startSearch($scope.page)
		}

		var ingredientLists = { };
		$scope.getIngredientList = function (recipe) {
			if (!ingredientLists[recipe.id])
				ingredientLists[recipe.id] = recipe.ingredientString ? $sce.trustAsHtml(recipe.ingredientString.replace(/ __ /g, '<br />')) : '';
			return ingredientLists[recipe.id];
		}

		$scope.clearScreen = function () {
			$scope.gridKeywords.data = [];
			$scope.keyword = "";
			$scope.rating = 0;
			$scope.bookmarked = false;
			$scope.picturesOnly = false;
			$scope.searchResults = null;
			$scope.source = null;
			$scope.recipeName = null;
			$scope.disableScrolling = true;
			$scope.resultsText = "No recipes found.";
		};

		$scope.scrollNext = function () {
			if ($scope.disableScrolling) return;
			$scope.disableScrolling = true;
			$scope.startSearch($scope.page + 1);
		};

		$scope.startSearch = function (page) {
			if (page < 1) page = 1;
			$scope.page = page;

			//if ($scope.gridKeywords.data.length <= 0 && !$scope.picturesOnly) {
			//	var filt = '1 eq 1';

			//	if ($scope.rating != 0) filt += ' and Rating ge ' + $scope.rating;
			//	if ($scope.bookmarked) filt += ' and IsBookmarked eq true';
			//	if ($scope.source) filt += ' and RecipeSourceID eq ' + $scope.source.id;
				
			//	if (filt == '1 eq 1') return;

			//	$('#loadingDiv').show();
			//	entityFactory.getEntities('RecipeSearch', { baseUrl: appSettings.apiUrl, filter: filt, pageSize: $scope.pageSize, pageNumber: $scope.page, includeCount: true, orderBy: 'RecipeName' }).then(function (data) {
			//		if ($scope.searchResults == null || page < 2) {
			//			window.scrollTo(0, 0);
			//			$scope.searchResults = data.results;
			//			$scope.totalRecords = data.totalRecords;
			//		}
			//		else
			//			$scope.searchResults = $scope.searchResults.concat(data.results);

			//		if ($scope.totalRecords == 0)
			//			$scope.resultsText = "No recipes found matching your criteria.";
			//		else
			//			$scope.resultsText = $scope.totalRecords + " recipes found.";

			//		$scope.disableScrolling = data.results.length < $scope.pageSize;
			//		$('#loadingDiv').hide();
			//	}, function (data) {
			//		alert('error: ' + JSON.stringify(data));
			//		$('#loadingDiv').hide();
			//	});
			//	return;
			//}


			var params = '?pageSize=' + $scope.pageSize + '&page=' + $scope.page;

			var firstIn = true;
			var includeExcludes = '';
			for (i = 0; i < $scope.gridKeywords.data.length; i++) {
				if ($scope.gridKeywords.data[i].include) {
					includeExcludes += (firstIn ? '' : ';') + $scope.gridKeywords.data[i].keyword;
					firstIn = false;
				}
			}

			if (includeExcludes != '') params += '&includes=' + includeExcludes;

			firstIn = true;
			includeExcludes = '';
			for (i = 0; i < $scope.gridKeywords.data.length; i++) {
				if (!$scope.gridKeywords.data[i].include) {
					includeExcludes += (firstIn ? '' : ';') + $scope.gridKeywords.data[i].keyword;
					firstIn = false;
				}
			}
			if (includeExcludes != '') params += '&excludes=' + includeExcludes;

			if ($scope.recipeName) params += "&recipeName=" + $scope.recipeName;
			if ($scope.rating != 0) params += '&rating=' + $scope.rating;
			if ($scope.bookmarked) params += '&bookmarked=' + true;
			if ($scope.source != null && $scope.source != '') params += '&recipeSourceID=' + $scope.source.id;
			if ($scope.picturesOnly) params += '&picturesOnly=' + true;

			$('#loadingDiv').show();


			$http.get(appSettings.apiUrl + '/api/RecipeSearch/SearchRecipes' + params).then(function (response) {
				if ($scope.searchResults == null || page < 2) {
					window.scrollTo(0, 0);
					$scope.searchResults = response.data;
					$scope.totalRecords = response.headers('X-InlineCount');
				}
				else
					$scope.searchResults = $scope.searchResults.concat(response.data);

				if ($scope.totalRecords == 0)
					$scope.resultsText = "No recipes found matching your criteria.";
				else
					$scope.resultsText = $scope.totalRecords + " recipes found.";

				$scope.disableScrolling = response.data.length < $scope.pageSize;
				$('#loadingDiv').hide();
			},
			function (error) {
				$('#loadingDiv').hide();
				alert(error.message);
			});
		};

		$scope.openRecipe = function (recipe) {
            var modal = $uibModal.open({
				templateUrl: 'app/views/modalRecipe.html',
				controller: function ($scope) {
				    $scope.recipe = recipe;
				    $scope.modal = modal;
				}
			});
		};

		$scope.canBookmark = function () {
			return true;
			// return localStorageService.get('loginData') != null;
		};

		$scope.baseUrl = function () {
			return appSettings.baseUrl;
		};
	};

	angular.module('recipeSearch').controller('RecipeSearchController', RecipeSearchController);
}());