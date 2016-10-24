(function () {
	angular.module('pjm')
	.constant('gridViewConstants', {
		sortDirection: {
			NONE: 0,
			ASC: 1,
			DESC: 2
		},
		filterMode: {
			NONE: 0,
			BEGINS_WITH: 1,
			CONTAINS: 2,
			EQUALS: 3,
			NOT_EQUAL: 4
		}
	});

	angular.module('pjm')
		.directive('gridView', function ($compile, $filter, $sce, $parse, gridViewConstants) {
			return {
				scope: {
					gridView: '='
				},
				replace: true,
				template: "<div><table disable-animate ng-class=\"'gridView ' + (gridView.noBorder ? '' : 'gridViewBorder ') + (gridView.gridHeight > 0 ? 'scrollableTable ' : '') + (gridView.rowClick ? 'table-hover ' : '') + 'table table-striped table-condensed'\">" +
					"<thead><tr class='repeat-animation'><th ng-show='hasGroupColumns' style='width:30px'></th><th ng-show='gridView.allowRowSelect' style='width:5%'></th>" +
					"<th ng-repeat='col in gridView.columns' ng-show='col.visible === undefined || col.visible === true' style='width:{{col.width ? col.width : getEventPercentage()}}'>" +
					"<div class='sortHeader' ng-click='_sort(col)'><div class='headerCaption'>" +
					"{{col.caption ? col.caption : getCaptionFromFieldName(col.fieldName)}}</div>" +
					"<div class='headerCaption' ng-show='col.fieldName && col.sortable'>&nbsp;&nbsp;&nbsp;" +
					"<div class='sortArrow topEmpty glyphicon glyphicon-menu-up' ng-show='!col.sortDirection || col.sortDirection == 0'></div>" +
					"<div class='sortArrow bottomEmpty glyphicon glyphicon-menu-down' ng-show='!col.sortDirection || col.sortDirection == 0'></div>" +
					"<div class='sortArrow glyphicon glyphicon-menu-down' ng-show='col.sortDirection == 1'></div>" +
					"<div class='sortArrow glyphicon glyphicon-menu-up' ng-show='col.sortDirection == 2'></div>" +
					"</div></div></th></tr></thead>" +
					"<tbody ng-class=\"gridView.gridHeight > 0 ? 'scrollableTable scrollableTableBody' : ''\" style='height:{{ !gridView.gridHeight ? 0 : gridView.gridHeight }}px'>" +
					"<tr ng-show='hasFilterRow()'><td ng-show='hasGroupColumns' style='width:30px'></td><td ng-show='gridView.allowRowSelect' style='width:5%'></td><td ng-repeat='col in gridView.columns'  grid-view-filter-cell='col'></td></tr>" +
					"<tr ng-repeat=\"item in page = (filtered = (data | filter : _filter) | orderBy : (hasGroupColumns ? '_groupSequence' : ((!gridView.disableAutoSort && gridView.getSortedColumn().sortDirection == 2 ? '-' : '') + (gridView.disableAutoSort ? 0 : gridView.getSortedColumn().fieldName))) |" +
					" page: (gridView.autoPage ? gridView.pageSize : 0) : (gridView.autoPage ? gridView.currentPage : 1))\" class=\"repeat-animation {{item._isGroupRow ? 'groupRow' : ''}}\" ng-click='gridView.rowClick ? gridView.rowClick(item) : null' style=\"cursor:{{gridView.rowClick ? 'pointer' : ''}}\" ng-show='!item._parentGroup || item._parentGroup._expanded'>" +
					"<td ng-show='hasGroupColumns' style='width:30px' class=\"{{item._isGroupRow ? (item._expanded ? 'glyphicon glyphicon-minus btn btn-xs' : 'glyphicon glyphicon-plus btn btn-xs') : ''}}\" ng-click='_expandCollapse(item)'></td><td ng-show='gridView.allowRowSelect' style='width:5%'><div style='text-align:center'><input type='checkbox' ng-model='item.selected' ng-change='_selectionChanged(item)' /></div></td>" +
					"<td ng-repeat='col in gridView.columns' ng-show='col.visible === undefined || col.visible' style='width:{{ col.width ? col.width : getEventPercentage()}}' grid-view-cell='col'></td>" +
					"</tr></tbody></table>" +
					"<div class='show-hide-animation' ng-show='gridView.paginate && data.length > 0'>" +
					"<pagination ng-change='_pageChanged()' total-items='(gridView.autoPage ? filtered.length : gridView.totalRecords)' ng-model='gridView.currentPage' boundary-links='true' " +
					"max-size='gridView.pageSize' items-per-page='gridView.pageSize' rotate='false' class='pagination-sm'></pagination>" +
					"<h5>Showing {{ page.length }} of {{ gridView.autoPage ? filtered.length : gridView.totalRecords }} total records</h5></div>" +
					"</div>"
					,
				link: function ($scope, element, attrs) {
					$scope.parentScope = $scope.$parent;
					$scope.data = [];

					var expandeds = [];

					$scope.getCaptionFromFieldName = function (caption) {
						if (!caption || caption == '') return '';
						if (caption.lastIndexOf('.') > 0) {
							caption = caption.substring(caption.lastIndexOf('.') + 1, caption.length);
						}
						return caption.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) { return str.toUpperCase(); });
					};

					$scope.getEventPercentage = function () {
						if (!$scope.gridView || !$scope.gridView.columns || $scope.gridView.columns.length < 1) return '100%';

						var visibleColCount = 0;
						for (i = 0; i < $scope.gridView.columns.length; i++) {
							if ($scope.gridView.columns[i].visible === undefined || $scope.gridView.columns[i].visible === true)
								visibleColCount++;
						}

						return (visibleColCount == 0 ? "100%" : (100 / visibleColCount) + "%");
					}

					$scope.hasFilterRow = function () {
						for (i = 0; i < $scope.gridView.columns.length; i++) {
							var col = $scope.gridView.columns[i];
							if ((col.visible === undefined || col.visible === true) && col.filterMode && col.filterMode != gridViewConstants.filterMode.NONE) {
								return true;
							}
						}

						return false;
					}

					$scope.getCellText = function (item, col) {
						if (col.template) return $scope.$eval(col.template, item);
						if (col.fieldName) {
							if (col.format) {
								// TODO: other types
								return $filter('date')(item[col.fieldName], col.format);
							}

							return item[col.fieldName];
						}
						return "";
					};

					//$scope.getGroupColumns = function () {
					//	var cols = [];
					//	for (i = 0; i < $scope.gridView.columns.length; i++) {
					//		if ($scope.gridView.columns[i].group)
					//			cols.push($scope.gridView.columns[i]);
					//	}
					//	return cols;
					//}

					$scope._sort = function (column) {
						if (!column.sortable) return;

						if ($scope.gridView.sorting !== undefined)
							$scope.gridView.sorting(column);

						for (i = 0; i < $scope.gridView.columns.length; i++) {
							if ($scope.gridView.columns[i] == column) continue;
							if ($scope.gridView.columns[i].sortable)
								$scope.gridView.columns[i].sortDirection = gridViewConstants.sortDirection.NONE;
						}

						if (column.sortDirection === undefined) {
							column.sortDirection = gridViewConstants.sortDirection.ASC;
						}
						else {
							switch (column.sortDirection) {
								case gridViewConstants.sortDirection.NONE:
								case gridViewConstants.sortDirection.DESC:
									column.sortDirection = gridViewConstants.sortDirection.ASC;
									break;
								case 1:
									column.sortDirection = gridViewConstants.sortDirection.DESC;
									break;
							}
						}

						if ($scope.gridView.sortChanged !== undefined)
							$scope.gridView.sortChanged(column);

						if ($scope.hasGroupColumns) {
							
							createGroups();
						}
					}

					$scope._pageChanged = function () {
						if ($scope.gridView.pageChanged !== undefined)
							$scope.gridView.pageChanged();
					};

					$scope._filter = function (item) {

						for (i = 0; i < $scope.gridView.columns.length; i++) {
							var col = $scope.gridView.columns[i];
							if (col.filterMode && col.filterMode != gridViewConstants.filterMode.NONE && col.filterValue != null) {
								var fieldName = col.fieldName;
								var currItem = item;

								while (fieldName.indexOf('.') > 0) {
									currItem = currItem[fieldName.substring(0, fieldName.indexOf('.'))];
									fieldName = fieldName.substring(fieldName.indexOf('.') + 1, fieldName.length);
								}

								switch (col.filterMode) {
									case gridViewConstants.filterMode.BEGINS_WITH:
										if (!currItem[fieldName] || currItem[fieldName].toLowerCase().indexOf(col.filterValue.toLowerCase()) != 0)
											return false;
										break;
									case gridViewConstants.filterMode.CONTAINS:
										if (!currItem[fieldName] || currItem[fieldName].toLowerCase().indexOf(col.filterValue.toLowerCase()) == -1)
											return false;
										break;
									case gridViewConstants.filterMode.NOT_EQUAL:
										if (!currItem[fieldName] || currItem[fieldName] == col.filterValue)
											return false;
										break;
									case gridViewConstants.filterMode.EQUALS:
										if (!currItem[fieldName] || currItem[fieldName] != col.filterValue)
											return false
								}
							}
						}

						return true;
					};

					$scope._selectionChanged = function (item) {
						if (item.selected) {
							$scope.gridView.selectedItems.push(item);
						}
						else {
							for (i = 0; i < $scope.gridView.selectedItems.length; i++) {
								if (angular.equals($scope.gridView.selectedItems[i], item)) {
									$scope.gridView.selectedItems.splice(i, 1);
									break;
								}

							}
						}
						if ($scope.gridView.selectionChanged !== undefined)
							$scope.gridView.selectionChanged(item);
					}

					$scope._expandCollapse = function (item, collapse) {
						if (item._isGroupRow) {
							item._expanded = (collapse ? false : !item._expanded);
							if (!item._expanded) {
								for (i = 0; i < item._childGroups.length; i++) {
									$scope._expandCollapse(item._childGroups[i], true);
								}
							}

							var col = item._groupColumn;
							var getter = $parse(col.fieldName);
							var itemVal = getter(item);

							if (item._expanded && (!expandeds[col.fieldName] || expandeds[col.fieldName].indexOf(itemVal) < 0)) {
								if (!expandeds[col.fieldName])
									expandeds[col.fieldName] = [];
								expandeds[col.fieldName].push(itemVal);
							}
							else if (item._expanded && expandeds[col.fieldName] && expandeds[col.fieldName].indexOf(itemVal) >= 0) {
								var ind = expandeds[col.fieldName].indexOf(itemVal);
								expandeds[col.fieldName].splice(ind, 1);
							}
						}
					}

					var createGroups = function () {
						if ($scope.data.length > 0) {

							var currSequence = 0;
							var sortedCol = $scope.gridView.getSortedColumn();
							if (sortedCol != null) {
								$scope.data.sort(function (a, b) {
									for (i = 0; i < $scope.gridView.columns.length; i++) {
										var col = $scope.gridView.columns[i];
										var getter = $parse(col.fieldName);
										if (col.group) {
											if (getter(a) != getter(b)) {
												if (col.sortDirection === gridViewConstants.sortDirection.ASC)
													return getter(a) > getter(b) ? 1 : -1;
												return getter(a) > getter(b) ? -1 : 1;
											}
										}

										if (angular.equals(col, sortedCol)) {
											if (col.sortDirection === gridViewConstants.sortDirection.ASC)
												return getter(a) > getter(b) ? -1 : 1;
											return getter(a) > getter(b) ? 1 : -1;
										}
									}
								});
							}

							for (i = 0; i < $scope.gridView.columns.length; i++) {
								var grpRows = [];
								var col = $scope.gridView.columns[i];
								if (col.group) {
									$scope.hasGroupColumns = true;
									for (j = 0; j < $scope.data.length; j++) {
										var item = $scope.data[j];

										if (item._isGroupRow) continue;

										var getter = $parse(col.fieldName);
										var itemVal = getter(item);

										var groupRow = null;
										for (k = 0; k < grpRows.length; k++) {
											if (getter(grpRows[k]) == itemVal) {
												groupRow = grpRows[k];
												break;
											}
										}

										if (groupRow == null) {
											var setter = getter.assign;
											var groupRow = {};
											if (item._parentGroup) {
												groupRow = angular.copy(item._parentGroup);
											}

											// assuming there will never be 1000 groups
											groupRow._isGroupRow = true
											groupRow._groupSequence = currSequence + (1000 * j);
											groupRow._childGroups = [];
											groupRow._groupColumn = col;


											if (expandeds[col.fieldName] && expandeds[col.fieldName].indexOf(itemVal) >= 0)
												groupRow._expanded = true;
											else
												groupRow._expanded = false;

											setter(groupRow, itemVal);
											if (item._parentGroup) {
												groupRow._parentGroup = item._parentGroup;
												groupRow._parentGroup._childGroups.push(groupRow);
											}
											$scope.data.push(groupRow);
											grpRows.push(groupRow);
										}

										item._groupSequence = groupRow._groupSequence + 1;
										item._parentGroup = groupRow;
									}

									currSequence++;
								}
							}
						}
					}

					$scope.$watch('gridView.data', function (value) {
						if (!value) return;

						$scope.hasGroupColumns = false;
						$scope.data = [];

						if ($scope.gridView.paginate && $scope.gridView.autoPage)
							$scope.gridView.totalRecords = value.length;

						for (j = 0; j < value.length; j++) {
							$scope.data.push(value);
							if ($scope.gridView.allowRowSelect && value && $scope.gridView.selectedItems && $scope.gridView.selectedItems.length > 0) {
								for (i = 0; i < $scope.gridView.selectedItems.length; i++) {
									if (value[j].id == $scope.gridView.selectedItems[i].id) {
										value[j].selected = true;
										j = value.length;
									}
								}
							}
						}

						createGroups();
					});

					// init
					var initGrid = function () {
						if ($scope.gridInited) return;
						if (!$scope.gridView) {
							$scope.$watch('gridView', function (value) {
								initGrid();
							})
							return;
						}
						$scope.gridInited = true;

						if (!$scope.gridView.selectedItems) {
							$scope.gridView.selectedItems = [];
						}
						$scope.gridView.getSortedColumn = function () {
							for (i = 0; i < $scope.gridView.columns.length; i++) {
								if ($scope.gridView.columns[i].sortable && $scope.gridView.columns[i].sortDirection && $scope.gridView.columns[i].sortDirection != gridViewConstants.sortDirection.NONE)
									return $scope.gridView.columns[i];
							}
							return null;
						};

						$scope.gridView.getDBField = function (col) {
							var parts = col.fieldName.split('.');
							var dbFieldName = '';
							for (i = 0; i < parts.length; i++) {
								dbFieldName += (i > 0 ? '/' : '') + parts[i].substring(0, 1).toUpperCase() + parts[i].substring(1, parts[i].length);
							}
							return dbFieldName;
						}

						$scope.gridView.getDBSortExpression = function () {
							var col = $scope.gridView.getSortedColumn();
							if (col) {
								return $scope.gridView.getDBField(col) + (col.sortDirection == gridViewConstants.sortDirection.DESC ? ' desc' : '');
							}

							return null;
						}

						$scope.gridView.clearSelection = function () {
							if ($scope.gridView.selectedItems) {
								for (i = 0; i < $scope.gridView.selectedItems.length; i++) {
									$scope.gridView.selectedItems[i].selected = false;
								}
								for (i = 0; i < $scope.data.length; i++) {
									$scope.data[i].selected = false;
								}
								$scope.gridView.selectedItems = [];
							}
						}
					}

					initGrid();
				}
			}
		});
	angular.module('pjm')
		.directive('gridViewCell', function ($compile) {
			return {
				scope: {
					gridViewCell: '='
				},
				link: function ($scope, element, attrs) {
					$scope.item = $scope.$parent.item;
					$scope.parentScope = $scope.$parent.parentScope;
					var col = $scope.gridViewCell;
					var cellText = col.template;
					if (!cellText) {
						if (col.fieldType && col.fieldType == 'boolean') {
							cellText = "<div ng-class=\"item." + col.fieldName + (col.format ? " | " + col.format : "") + " ? 'glyphicon glyphicon-ok' : ''\"></div>";
						}
						else {
							cellText = "{{item." + col.fieldName + (col.format ? " | " + col.format : "") + "}}";
						}
					}
					var el = $compile("<div>" + cellText + "</div>")($scope);
					element.append(el);
				}
			}
		});
	angular.module('pjm')
		.directive('gridViewFilterCell', function ($compile, gridViewConstants) {
			return {
				scope: {
					gridViewFilterCell: '='
				},
				link: function ($scope, element, attrs) {
					$scope.gridView = $scope.$parent.gridView;
					$scope.parentScope = $scope.$parent;

					var col = $scope.gridViewFilterCell;
					$scope.column = col;
					if (!col.filterMode || col.filterMode == gridViewConstants.filterMode.NONE)
						return;

					$scope.item = $scope.$parent.item;

					$scope._filterChanged = function (column) {
						if ($scope.gridView.filterChanged !== undefined)
							$scope.gridView.filterChanged(column);
					}

					var cellHTML = col.filterTemplate;
					if (!cellHTML) {
						if (col.fieldType && col.fieldType == 'boolean') {
							cellHTML = "<div ng-class=\"item." + col.fieldName + (col.format ? " | " + col.format : "") + " ? 'glyphicon glyphicon-ok' : ''\"></div>";
						}
						else if (col.distinctFilter && col.fieldName) {
							cellHTML = "<select ng-model='column.filterValue' style='width:100%;height:26px' ng-change='_filterChanged(column)' ng-options=\"row." + col.fieldName + " as row." + col.fieldName +
									" for row in parentScope.data | orderBy:'" + col.fieldName + "' | unique:'" + col.fieldName + "'\"><option value=''></option></select>"
						}
						else {
							cellHTML = "<input type='text' ng-model='column.filterValue' ng-change='_filterChanged(column)' style='width:100%;height:26px' />";
						}
					}

					var el = $compile(cellHTML)($scope);
					element.append(el);
				}
			}
		});
}());