(function () {
	angular.module('pjm')
		.directive('checkList', function ($document, $parse) {
			return {
				scope: {
					checkList: '='
				},
				replace: true,
				template: "<div class='check-list'><button ng-click='toggleDropdown($event)' class=\"check-list-button {{formcontrol ? 'check-list-button-formcontrol' : ''}}\">{{selectedText ? selectedText : '&nbsp;'}}<div class=\"pull-right drop-down-image glyphicon {{ allSelected ? 'glyphicon-triangle-bottom' : 'glyphicon-filter'}}\"></div></button>" +
					"<div class='check-list-dropdown' ng-show='dropdownVisible'>" +
					"<div ng-click='selectAll()' class='check-list-item check-list-item-all'><div class=\"check-list-check glyphicon {{ allSelected ? 'glyphicon-ok' : 'glyphicon-none'}}\"></div>(Select All)</div>" +
					"<div ng-repeat='item in items' ng-click='$parent.selectItem(item)' class='check-list'><div class='check-list-item'><div class=\"check-list-check glyphicon {{ item.selected ? 'glyphicon-ok' : 'glyphicon-none'}}\"></div>{{item.display}}</div>" +
					"</div></div></div>"
				,
				link: function ($scope, element, attrs) {
					$scope.formcontrol = false;

					if (element[0].className && element[0].className.indexOf('form-control') >= 0) {
						$scope.formcontrol = true;
					}

					$scope.selectedText = "";
					$scope.allSelected = true;

					$scope.$watch('checkList.items', function (value) {
						if (value) {
							$scope.items = [];
							for (var i = 0; i < value.length; i++) {
								var newItem = { item: value[i], display: $scope.checkList.displayMember ? value[i][$scope.checkList.displayMember] : value[i], selected: true };
								$scope.items.push(newItem);
							}
						}
					});

					var updateTextSelected = function () {
						$scope.selectedText = '';
						$scope.checkList.selectedItems = [];
						var firstIn = true;
						for (var i = 0; i < $scope.items.length; i++) {
							if ($scope.items[i].selected || $scope.allSelected) {
								$scope.checkList.selectedItems.push($scope.items[i].item);
								if (!$scope.allSelected && firstIn)
									$scope.selectedText = $scope.items[i].display;
								firstIn = false;
							}
						}
						
						if (!$scope.allSelected && $scope.checkList.selectedItems.length > 1)
							$scope.selectedText = "(...)";

						if (attrs.ngModel) {
							var model = element.controller('ngModel');
							model.$setViewValue($scope.checkList.selectedItems);
						}
					}

					$scope.selectItem = function (item) {
						item.selected = !item.selected;
						$scope.allSelected = false;
						updateTextSelected();
					}

					$scope.selectAll = function () {
						$scope.allSelected = !$scope.allSelected;
						for (var i = 0; i < $scope.items.length; i++) {
							$scope.items[i].selected = $scope.allSelected;
						}
						updateTextSelected();
					}

					$scope.toggleDropdown = function () {
						$scope.dropdownVisible = !$scope.dropdownVisible;
					}

					var onDocumentClick = function (event) {
						if (element.find(event.target).length < 1) {
							$scope.$apply(function () { $scope.dropdownVisible = false });
						}
					};

					$document.on('click', onDocumentClick);

					element.on('$destroy', function () {
						$document.off('click', onDocumentClick)
					});
				}
			}
		});
}());