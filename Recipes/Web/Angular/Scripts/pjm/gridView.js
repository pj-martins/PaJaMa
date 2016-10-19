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
		},
		summaryType: {
			NONE: 0,
			SUM: 1,
			AVG: 2,
			MIN: 3,
			MAX: 4
		}
	});

	angular.module('pjm')
		.directive('gridView', function ($compile, $filter, $sce, gridViewConstants) {
			return {
				scope: {
					gridView: '='
				},
				replace: true,
				template: "<div><table disable-animate ng-class=\"'gridView ' + (gridView.noBorder ? '' : 'gridViewBorder ') + (gridView.gridHeight > 0 ? 'scrollableTable ' : '') + (gridView.rowClick ? 'table-hover ' : '') + 'table table-striped table-condensed'\">" +
					"<thead ng-show='gridView.showHeader === undefined || gridView.showHeader === true'><tr class='repeat-animation'><th ng-show='gridView.detailGrid' style='width:5px'></th><th ng-show='gridView.allowRowSelect' style='width:5px'></th>" +
					"<th ng-repeat='col in gridView.columns' ng-show='col.visible' ng-attr-style='width:{{col.width}}'>" +
					"<div class='sortHeader' ng-click='_sort(col)'><div class='headerCaption'>{{col.caption}}</div>" +
					"<div class='headerCaption' ng-show='col.fieldName && col.sortable'>&nbsp;&nbsp;&nbsp;" +
					"<div class='sortArrow topEmpty glyphicon glyphicon-menu-up' ng-show='!col.sortDirection || col.sortDirection == 0'></div>" +
					"<div class='sortArrow bottomEmpty glyphicon glyphicon-menu-down' ng-show='!col.sortDirection || col.sortDirection == 0'></div>" +
					"<div class='sortArrow glyphicon glyphicon-menu-down' ng-show='col.sortDirection == 1'></div>" +
					"<div class='sortArrow glyphicon glyphicon-menu-up' ng-show='col.sortDirection == 2'></div>" +
					"</div></div></th></tr></thead>" +
					"<tbody ng-class=\"gridView.gridHeight > 0 ? 'scrollableTable scrollableTableBody' : ''\" style='height:{{ !gridView.gridHeight ? 0 : gridView.gridHeight }}px'>" +
					"<tr ng-show='hasFilterRow'><td ng-show='gridView.detailGrid' style='width:5px'></td><td ng-show='gridView.allowRowSelect' style='width:5px'></td><td ng-repeat='col in gridView.columns' ng-show='col.visible'  grid-view-filter-cell='col'></td></tr>" +
					"<tr ng-repeat-start='item in page = (filtered = (gridView.data | filter : _filter) | orderBy : (gridView.disableAutoSort || !gridView.sortedColumn ? 0 : gridView.sortedColumn.fieldName) : (gridView.disableAutoSort || !gridView.sortedColumn ? false : gridView.sortedColumn.sortDirection == 2) |" +
					" page: (gridView.autoPage ? gridView.pageSize : 0) : (gridView.autoPage ? gridView.currentPage : 1))' ng-class=\"'repeat-animation ' + (gridView.getRowClass ? gridView.getRowClass(item) : '')\" ng-click='gridView.rowClick ? gridView.rowClick(item) : null' style=\"cursor:{{gridView.rowClick ? 'pointer' : ''}}\">" +
					"<td ng-show='gridView.detailGrid' style='width:5px'><button class=\"glyphicon glyphicon-small {{item._expanded ? 'glyphicon-minus' : 'glyphicon-plus'}}\" ng-click='item._expandCollapse()'></button></td><td ng-show='gridView.allowRowSelect' style='width:5px'><div style='text-align:center'><input type='checkbox' ng-model='item.selected' ng-change='_selectionChanged(item)' /></div></td>" +
					"<td ng-repeat='col in gridView.columns' ng-show='col.visible' ng-attr-style='width:{{ col.width }}' grid-view-cell='col' ng-class=\"col.getRowCellClass ? col.getRowCellClass(item) : ''\"></td>" +
					"</tr><tr ng-repeat-end ng-if='gridView.detailGrid' ng-show='item._expanded'><td></td><td colspan='{{visibleColumnCount + (gridView.allowRowSelect ? 1 : 0) }}'><div detail-grid='gridView.detailGrid'></div></td></tr></tbody>" +
					"<tfoot ng-show='gridView.showFooter'><tr><td ng-show='gridView.detailGrid' style='width:5px'></td><td ng-show='gridView.allowRowSelect' style='width:5px'></td>" +
					"<td ng-repeat='col in gridView.columns' ng-show='col.visible' ng-attr-style='width:{{ col.width }}' grid-view-footer-cell='col'></td></tr></tfoot>" +
					"</table>" +
					"<div class='show-hide-animation' ng-show='gridView.paginate && gridView.data.length > 0'>" +
					"<pagination ng-change='_pageChanged()' total-items='(gridView.autoPage ? filtered.length : gridView.totalRecords)' ng-model='gridView.currentPage' boundary-links='true' " +
					"max-size='gridView.pageSize' items-per-page='gridView.pageSize' rotate='false' class='pagination-sm'></pagination>" +
					"<h5>Showing {{ page.length }} of {{ gridView.autoPage ? filtered.length : gridView.totalRecords }} total records</h5></div>" +
					"</div>"
					,
				link: function ($scope, element, attrs) {
					$scope.parentScope = $scope.$parent;
					$scope.hasFilterRow = false;
					$scope.visibleColumnCount = 0;

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

					$scope._sort = function (column) {
						if (!column.sortable) return;

						$scope.gridView.sortedColumn = column;

						if ($scope.gridView.sorting !== undefined)
							$scope.gridView.sorting(column);

						for (var i = 0; i < $scope.gridView.columns.length; i++) {
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
					}

					$scope._pageChanged = function () {
						if ($scope.gridView.pageChanged !== undefined)
							$scope.gridView.pageChanged();
					};

					$scope._filter = function (item) {

						for (var i = 0; i < $scope.gridView.columns.length; i++) {
							var col = $scope.gridView.columns[i];
							if (col.filterMode && col.filterMode != gridViewConstants.filterMode.NONE && col.filterValue != null) {
								var fieldName = col.fieldName;
								var currItem = item;

								while (fieldName.indexOf('.') > 0) {
									currItem = currItem[fieldName.substring(0, fieldName.indexOf('.'))];
									fieldName = fieldName.substring(fieldName.indexOf('.') + 1, fieldName.length);
								}

								var itemVal = currItem[fieldName];
								switch (col.filterMode) {
									case gridViewConstants.filterMode.BEGINS_WITH:
										if (!itemVal || itemVal.toLowerCase().indexOf(col.filterValue.toLowerCase()) != 0)
											return false;
										break;
									case gridViewConstants.filterMode.CONTAINS:
										if (angular.isArray(col.filterValue)) {
											if (!itemVal || col.filterValue.indexOf(itemVal) == -1) {
												return false;
											}
										}
										else if (!itemVal || itemVal.toLowerCase().indexOf(col.filterValue.toLowerCase()) == -1)
											return false;
										break;
									case gridViewConstants.filterMode.NOT_EQUAL:
										if (col.fieldType == 'date') {
											return new Date(itemVal).getTime() != new Date(col.filterValue).getTime();
										}
										if (!itemVal || itemVal == col.filterValue)
											return false;
										break;
									case gridViewConstants.filterMode.EQUALS:
										if (col.fieldType == 'date') {
											return new Date(itemVal).getTime() == new Date(col.filterValue).getTime();
										}
										if (!itemVal || itemVal != col.filterValue)
											return false;
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
							for (var i = 0; i < $scope.gridView.selectedItems.length; i++) {
								if (angular.equals($scope.gridView.selectedItems[i], item)) {
									$scope.gridView.selectedItems.splice(i, 1);
									break;
								}

							}
						}
						if ($scope.gridView.selectionChanged !== undefined)
							$scope.gridView.selectionChanged(item);
					}

					$scope.$watch('gridView.data', function (value) {
						if (!value) return;

						if ($scope.gridView.paginate && $scope.gridView.autoPage)
							$scope.gridView.totalRecords = value.length;

						if ($scope.gridView.allowRowSelect && value && $scope.gridView.selectedItems && $scope.gridView.selectedItems.length > 0) {
							for (var i = 0; i < $scope.gridView.selectedItems.length; i++) {
								for (j = 0; j < value.length; j++) {
									if (value[j].id == $scope.gridView.selectedItems[i].id) {
										value[j].selected = true;
										j = value.length;
									}
								}
							}
						}
					});

					var getCaptionFromFieldName = function (caption) {
						if (!caption || caption == '') return '';
						if (caption.lastIndexOf('.') > 0) {
							caption = caption.substring(caption.lastIndexOf('.') + 1, caption.length);
						}
						return caption.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) { return str.toUpperCase(); });
					};

					var getEventPercentage = function () {
						return ($scope.visibleColumnCount == 0 ? "100%" : (100 / $scope.visibleColumnCount) + "%");
					}

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

						$scope.gridView.getDBField = function (col) {
							var parts = col.fieldName.split('.');
							var dbFieldName = '';
							for (var i = 0; i < parts.length; i++) {
								dbFieldName += (i > 0 ? '/' : '') + parts[i].substring(0, 1).toUpperCase() + parts[i].substring(1, parts[i].length);
							}
							return dbFieldName;
						}

						$scope.gridView.getDBSortExpression = function () {
							if ($scope.gridView.sortedColumn) {
								return this.getDBField($scope.gridView.sortedColumn) + ($scope.gridView.sortedColumn.sortDirection == gridViewConstants.sortDirection.DESC ? ' desc' : '');
							}

							return null;
						}

						$scope.gridView.clearSelection = function () {
							if (this.selectedItems) {
								for (var i = 0; i < this.selectedItems.length; i++) {
									this.selectedItems[i].selected = false;
								}
								for (var i = 0; i < this.data.length; i++) {
									this.data[i].selected = false;
								}
								this.selectedItems = [];
							}
						}

						$scope.gridView.expandAll = function () {
							if (this.detailGrid) {
								for (var i = 0; i < this.data.length; i++) {
									if (this.data[i]._expandCollapse)
										this.data[i]._expandCollapse(true);
									else
										this.data[i]._expandOnInit = true;
								}

								if (this.detailGrids) {
									for (var i = 0; i < this.detailGrids.length; i++) {
										if (this.detailGrids[i].expandAll)
											this.detailGrids[i].expandAll();
									}
								}
							}
						}

						$scope.gridView.collapseAll = function () {
							if (this.detailGrid) {
								for (var i = 0; i < this.detailGrids.length; i++) {
									this.detailGrids[i].collapseAll();
								}

								for (var i = 0; i < this.data.length; i++) {
									this.data[i]._expandCollapse(false);
								}
							}
						}

						for (var i = 0; i < $scope.gridView.columns.length; i++) {
							var col = $scope.gridView.columns[i];
							if (col.visible === undefined)
								col.visible = true;

							if (col.visible)
								$scope.visibleColumnCount++;

						}

						for (var i = 0; i < $scope.gridView.columns.length; i++) {
							var col = $scope.gridView.columns[i];
							if (col.filterMode && col.filterMode != gridViewConstants.filterMode.NONE) {

								if (col.filterValue === undefined)
									col.filterValue = null;
								$scope.hasFilterRow = true;

							}

							if (!col.caption)
								col.caption = getCaptionFromFieldName(col.fieldName);

							if (!col.width)
								col.width = getEventPercentage();

							if (col.sortDirection && col.sortDirection != gridViewConstants.sortDirection.NONE)
								$scope.gridView.sortedColumn = col;
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
					$scope.gridView = $scope.$parent.gridView;
					$scope.parentScope = $scope.$parent.parentScope;
					var col = $scope.gridViewCell;
					var cellText = col.template;
					if (!cellText) {
						if (col.fieldType && col.fieldType == 'boolean') {
							cellText = "<div ng-class=\"item." + col.fieldName + (col.format ? " | " + col.format : "") + " ? 'glyphicon glyphicon-ok' : ''\"></div>";
						}
						else {
							// TODO: look at using regex
							var fld = '';
							for (var i = 0; i < col.fieldName.length; i++) {
								var charCode = col.fieldName.toLowerCase().charCodeAt(i);
								if ((charCode >= 97 && charCode <= 122) || charCode == 95) {
									var prepend = true;
									if (i > 0) {
										var prevCharCode = col.fieldName.toLowerCase().charCodeAt(i - 1);
										if ((prevCharCode >= 97 && prevCharCode <= 122) // a - z
											|| prevCharCode == 95 // _
											|| prevCharCode == 39 // '
											|| prevCharCode == 34 // "
											|| prevCharCode == 46 // .
											) {
											prepend = false;
										}
									}
									if (prepend)
										fld += 'item.';
								}
								fld += col.fieldName[i];

							}
							cellText = "{{" + fld + (col.format ? " | " + col.format : "") + "}}";
						}
					}

					var el = $compile("<div>" + cellText + "</div>")($scope);
					element.append(el);
				}
			}
		});
	angular.module('pjm')
		.directive('gridViewFilterCell', function ($compile, $filter, $parse, gridViewConstants) {
			return {
				scope: {
					gridViewFilterCell: '='
				},
				link: function ($scope, element, attrs) {
					var col = $scope.gridViewFilterCell;
					$scope.column = col;
					if (!col.filterMode || col.filterMode == gridViewConstants.filterMode.NONE)
						return;

					$scope.item = $scope.$parent.item;
					$scope.gridView = $scope.$parent.gridView;

					$scope._filterChanged = function (column) {
						if ($scope.gridView.filterChanged !== undefined)
							$scope.gridView.filterChanged(column);
					}

					var cellHTML = col.filterTemplate
					if (!cellHTML) {
						if (col.fieldType && col.fieldType == 'boolean') {
							cellHTML = "<div ng-class=\"item." + col.fieldName + (col.format ? " | " + col.format : "") + " ? 'glyphicon glyphicon-ok' : ''\"></div>";
						}
						else if (col.fieldType && col.fieldType == 'date') {
							cellHTML = "<input type='text' ng-model='column.filterValue' ng-change='_filterChanged(column)' class='form-control filter-control' bs-datepicker date-picker />";
						}
						else if (col.filterValues && col.fieldName) {
							col._checkList = { items: col.filterValues };
							cellHTML = "<div check-list='column._checkList' class='form-control filter-control' ng-model='column.filterValue' ng-change='_filterChanged(column)'></div>";
							$scope.$watch('column.filterValues', function (value) {
								col._checkList.items = value;
								col._checkList.items.sort();
							});
						}
						else if (col.distinctFilter && col.fieldName) {
							col._checkList = { items: [] };
							$scope.$watch('gridView.data', function (value) {
								if (value) {
									col._checkList.items = [];
									var getter = $parse(col.fieldName);
									for (var i = 0; i < value.length; i++) {
										var val = getter(value[i]);
										if (col._checkList.items.indexOf(val) < 0)
											col._checkList.items.push(val);
									}
									col._checkList.items.sort();
								}
							});
							cellHTML = "<div check-list='column._checkList' class='form-control filter-control' ng-model='column.filterValue' ng-change='_filterChanged(column)'></div>";
						}
						else {
							cellHTML = "<input type='text' ng-model='column.filterValue' ng-change='_filterChanged(column)' class='form-control filter-control' />";
						}
					}

					var el = $compile(cellHTML)($scope);
					element.append(el);
				}
			}
		});
	angular.module('pjm')
		.directive('gridViewFooterCell', function ($compile, gridViewConstants) {
			return {
				scope: {
					gridViewFooterCell: '='
				},
				link: function ($scope, element, attrs) {
					var col = $scope.gridViewFooterCell;
					if ((col.summaries && col.fieldName) || col.footerTemplate) {
						$scope.item = $scope.$parent.item;
						$scope.gridView = $scope.$parent.gridView;
						var cellText = col.footerTemplate;
						if (!cellText) {
							cellText = "";
							for (var i = 0; i < col.summaries.length; i++) {
								var prefix = '';
								var filter = '';
								switch (col.summaries[i]) {
									case gridViewConstants.summaryType.SUM:
										filter = "sum";
										break;
									case gridViewConstants.summaryType.AVG:
										filter = "avg";
										break;
									case gridViewConstants.summaryType.MIN:
										filter = "min";
										break;
									case gridViewConstants.summaryType.MAX:
										filter = "max";
										break;
								}
								cellText += "<div><strong>" + filter.toUpperCase() + " = {{gridView.data | " + filter + ":'" + col.fieldName + "'" + (col.format? " | " + col.format : "") + " }}</strong></div>";
							}
						}

						var el = $compile("<div>" + cellText + "</div>")($scope);
						element.append(el);
					}
				}
			}
		});
	angular.module('pjm')
		.directive('detailGrid', function ($compile) {
			return {
				scope: {
					detailGrid: '='
				},
				link: function ($scope, element, attrs) {
					if ($scope.detailGrid) {
						$scope.inited = false;
						var item = $scope.$parent.$parent.item;
						item._expandCollapse = function (expand) {
							item._expanded = (expand === undefined || expand == null ? !item._expanded : expand);
							if (!$scope.inited) {
								$scope.inited = true;

								$scope._currentGrid = angular.copy($scope.detailGrid);
								if ($scope.detailGrid.getData)
									$scope._currentGrid.data = $scope.detailGrid.getData(item);
								else if ($scope.detailGrid.dataMember)
								$scope._currentGrid.data = $scope.$parent.$parent.item[$scope.detailGrid.dataMember];
								if (!$scope.$parent.gridView.detailGrids) {
									$scope.$parent.gridView.detailGrids = [];
								}

								$scope.$parent.gridView.detailGrids.push($scope._currentGrid);
								var el = $compile("<div grid-view='_currentGrid'></div>")($scope);
								element.append(el);
							}
						}
						if (item._expandOnInit) {
							item._expandOnInit = false;
							item._expandCollapse(true);
						}
					}
				}
			}
		});
}());