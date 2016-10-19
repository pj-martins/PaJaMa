(function () {
	var templateFactory = function (appSettings) {
		return {
		    getRemoveTemplate: function () {
				return '<div class="imgIncludeExclude"><img width=25 src="img/delete.png" style="cursor:pointer" ng-click="parentScope.removeRow($parent.$parent.$index)" /></div>';
			},
			getIncludeTemplate: function () {
				return '<div class="imgIncludeExclude" ng-switch="item.include">' +
									'<img width=25 ng-switch-when="true" src="img/plus.png" />' +
									'<img width=25 ng-switch-when="false" src="img/minus.png" />' +
						'</div>';
			}
		};
	};

	angular.module('recipeSearch').factory('templateFactory', templateFactory);
}());