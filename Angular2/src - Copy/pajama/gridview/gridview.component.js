"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var gridview_1 = require("./gridview");
var gridview_pager_component_1 = require("./gridview-pager.component");
var parser_service_1 = require("../services/parser.service");
var GridViewComponent = (function () {
    function GridViewComponent(parserService, zone) {
        this.parserService = parserService;
        this.zone = zone;
        this.selectedKeys = {};
        this.sortChanged = new core_1.EventEmitter();
        this.filterChanged = new core_1.EventEmitter();
        this.pageChanged = new core_1.EventEmitter();
        this.selectionChanged = new core_1.EventEmitter();
        this.detailGridViewComponents = {};
        this.self = this;
        this.sortDirection = gridview_1.ColumnSortDirection;
        this.fieldType = gridview_1.FieldType;
        this._indexWidthInited = false;
    }
    Object.defineProperty(GridViewComponent.prototype, "grid", {
        get: function () {
            return this._grid;
        },
        set: function (value) {
            var _this = this;
            if (this._grid != null)
                this._grid.dataChanged.unsubscribe();
            this._grid = value;
            if (this._grid != null) {
                if (this._grid.detailGridView && !this._grid.keyFieldName) {
                    throw "Grids with detail grids require a key field name";
                }
                if (this._grid.selectMode > 0 && !this._grid.keyFieldName) {
                    throw "Grids with row select enable require a key field name";
                }
                this._grid.dataChanged.subscribe(function () { return _this.resetData(); });
                this.initPager();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GridViewComponent.prototype, "pager", {
        get: function () {
            return this._pager;
        },
        set: function (v) {
            this._pager = v;
            this.initPager();
        },
        enumerable: true,
        configurable: true
    });
    GridViewComponent.prototype.initPager = function () {
        if (!this.pager || !this._grid)
            return;
        var pageFound = false;
        for (var _i = 0, _a = this.pager.pageSizes; _i < _a.length; _i++) {
            var pageSize = _a[_i];
            if (pageSize.size == this._grid.pageSize) {
                pageFound = true;
                break;
            }
        }
        if (!pageFound) {
            this.pager.pageSizes.push({ size: this._grid.pageSize, label: this._grid.pageSize.toString() });
            this.pager.pageSizes.sort(function (a, b) {
                if (a.size == 0)
                    return 1;
                if (b.size == 0)
                    return -1;
                if (a.size > b.size)
                    return 1;
                if (a.size < b.size)
                    return -1;
                return 0;
            });
        }
    };
    GridViewComponent.prototype.resetData = function (resetPage) {
        if (resetPage === void 0) { resetPage = false; }
        var expandedKeys = [];
        if (this.detailGridViewComponents) {
            for (var _i = 0, _a = Object.keys(this.detailGridViewComponents); _i < _a.length; _i++) {
                var k = _a[_i];
                if (this.detailGridViewComponents[k].isExpanded())
                    expandedKeys.push(k);
            }
        }
        this._displayData = null;
        this._unpagedData = null;
        if (resetPage)
            this.grid.currentPage = 1;
        if (this.detailGridViewComponents) {
            this.collapseAll();
            if (expandedKeys && expandedKeys.length > 0) {
                for (var _b = 0, expandedKeys_1 = expandedKeys; _b < expandedKeys_1.length; _b++) {
                    var k = expandedKeys_1[_b];
                    for (var _c = 0, _d = this.displayData; _c < _d.length; _c++) {
                        var d = _d[_c];
                        if (d[this.grid.keyFieldName] == k) {
                            if (!this.detailGridViewComponents[k].isExpanded())
                                this.detailGridViewComponents[k].expandCollapse();
                            break;
                        }
                    }
                }
            }
        }
    };
    GridViewComponent.prototype.hasFilterRow = function () {
        if (this.grid.disableFilterRow)
            return false;
        for (var _i = 0, _a = this.grid.getDataColumns(); _i < _a.length; _i++) {
            var col = _a[_i];
            if (col.filterMode != gridview_1.FilterMode.None) {
                return true;
            }
        }
        return false;
    };
    GridViewComponent.prototype.getLink = function (column, row) {
        var url = column.url;
        for (var _i = 0, _a = Object.keys(column.parameters); _i < _a.length; _i++) {
            var k = _a[_i];
            url += ";" + k + "=" + this.parserService.getObjectValue(column.parameters[k], row);
        }
        return url;
    };
    GridViewComponent.prototype.getLinkTarget = function (column, row) {
        if (column.target) {
            return "_" + this.parserService.getObjectValue(column.target, row);
        }
        return '';
    };
    GridViewComponent.prototype.getVisibleColumnCount = function () {
        if (this.grid.rowTemplate)
            return 1;
        var count = 0;
        for (var _i = 0, _a = this.grid.columns; _i < _a.length; _i++) {
            var col = _a[_i];
            if (!this._indexWidthInited && count != 0 && col.columnIndex == 0) {
                col.columnIndex = count;
            }
            if (col.visible) {
                count++;
            }
        }
        if (!this._indexWidthInited) {
            this._indexWidthInited = true;
        }
        return count;
    };
    GridViewComponent.prototype.toggleFilter = function () {
        this.grid.filterVisible = !this.grid.filterVisible;
        this.filterChanged.emit(null);
        this.grid.saveGridState();
    };
    GridViewComponent.prototype.rowClick = function (row) {
        if (this.grid.selectMode > 0) {
            this.selectedKeys[row[this.grid.keyFieldName]] = !this.selectedKeys[row[this.grid.keyFieldName]];
            if (this.grid.selectMode == gridview_1.SelectMode.Single && this.selectedKeys[row[this.grid.keyFieldName]]) {
                for (var _i = 0, _a = this.grid.data; _i < _a.length; _i++) {
                    var d = _a[_i];
                    if (d[this.grid.keyFieldName] != row[this.grid.keyFieldName]) {
                        this.selectedKeys[d[this.grid.keyFieldName]] = false;
                    }
                }
            }
            var selectedRows = [];
            for (var _b = 0, _c = this.grid.data; _b < _c.length; _b++) {
                var d = _c[_b];
                if (this.selectedKeys[d[this.grid.keyFieldName]])
                    selectedRows.push(d);
            }
            this.selectionChanged.emit(selectedRows);
        }
    };
    GridViewComponent.prototype.handleSortChanged = function (column) {
        if (this.sortChanged)
            this.sortChanged.emit(column);
        this.resetData();
        if (this.grid.saveGridStateToStorage)
            this.grid.saveGridState();
    };
    GridViewComponent.prototype.getSortedData = function (data) {
        var _this = this;
        if (!data)
            return [];
        if (this.grid.disableAutoSort)
            return data;
        var sorts = new Array();
        if (this.grid.columns) {
            for (var _i = 0, _a = this.grid.getDataColumns(); _i < _a.length; _i++) {
                var col = _a[_i];
                if (col.fieldName && col.sortDirection !== undefined && col.sortDirection != gridview_1.ColumnSortDirection.None) {
                    if (col.sortIndex === undefined)
                        col.sortIndex = 0;
                    sorts.push(col);
                }
            }
        }
        if (sorts.length <= 0) {
            return data;
        }
        sorts.sort(function (a, b) {
            return a.sortIndex - b.sortIndex;
        });
        data.sort(function (a, b) {
            for (var i = 0; i < sorts.length; i++) {
                var curr = sorts[i];
                var aval = _this.parserService.getObjectValue(curr.fieldName, a);
                var bval = _this.parserService.getObjectValue(curr.fieldName, b);
                if (curr.customSort) {
                    var s = curr.customSort(aval, bval);
                    if (s != 0)
                        return s;
                }
                if (aval && typeof aval == "string")
                    aval = aval.toLowerCase();
                if (bval && typeof bval == "string")
                    bval = bval.toLowerCase();
                if (aval == bval)
                    continue;
                if (curr.sortDirection == gridview_1.ColumnSortDirection.Desc)
                    return aval > bval ? -1 : 1;
                return aval < bval ? -1 : 1;
            }
            return 0;
        });
        return data;
    };
    GridViewComponent.prototype.getFilteredData = function (rawData) {
        if (this.grid.disableAutoFilter)
            return rawData;
        if (!this.grid.filterVisible && !this.grid.disableFilterRow)
            return rawData;
        if (!rawData)
            return [];
        var filteredData = [];
        for (var _i = 0, rawData_1 = rawData; _i < rawData_1.length; _i++) {
            var row = rawData_1[_i];
            if (this.showRow(row))
                filteredData.push(row);
        }
        return filteredData;
    };
    Object.defineProperty(GridViewComponent.prototype, "unpagedData", {
        get: function () {
            if (!this._unpagedData || this._unpagedData.length < 1) {
                this._unpagedData = this.getFilteredData(this.getSortedData(this.grid.data));
            }
            return this._unpagedData;
        },
        enumerable: true,
        configurable: true
    });
    GridViewComponent.prototype.expandAll = function () {
        for (var _i = 0, _a = this.displayData; _i < _a.length; _i++) {
            var row = _a[_i];
            if (!this.detailGridViewComponents[row[this.grid.keyFieldName]].isExpanded())
                this.detailGridViewComponents[row[this.grid.keyFieldName]].expandCollapse();
        }
    };
    GridViewComponent.prototype.collapseAll = function () {
        for (var _i = 0, _a = this.displayData; _i < _a.length; _i++) {
            var row = _a[_i];
            if (this.detailGridViewComponents[row[this.grid.keyFieldName]] && this.detailGridViewComponents[row[this.grid.keyFieldName]].isExpanded())
                this.detailGridViewComponents[row[this.grid.keyFieldName]].expandCollapse();
        }
    };
    GridViewComponent.prototype.expandCollapse = function (keyFieldValue) {
        this.detailGridViewComponents[keyFieldValue].expandCollapse();
    };
    GridViewComponent.prototype.getSelectedKeys = function () {
        var selected = [];
        for (var _i = 0, _a = Object.keys(this.selectedKeys); _i < _a.length; _i++) {
            var k = _a[_i];
            if (this.selectedKeys[k]) {
                selected.push(k);
            }
        }
        return selected;
    };
    GridViewComponent.prototype.showRow = function (row) {
        for (var _i = 0, _a = this.grid.getDataColumns(); _i < _a.length; _i++) {
            var col = _a[_i];
            if (col.filterMode != gridview_1.FilterMode.None && col.filterValue != null) {
                var itemVal = this.parserService.getObjectValue(col.fieldName, row);
                switch (col.filterMode) {
                    case gridview_1.FilterMode.BeginsWith:
                        if (!itemVal || itemVal.toLowerCase().indexOf(col.filterValue.toLowerCase()) != 0)
                            return false;
                        break;
                    case gridview_1.FilterMode.Contains:
                    case gridview_1.FilterMode.DistinctList:
                    case gridview_1.FilterMode.DynamicList:
                        if (col.filterValue instanceof Array) {
                            if (col.filterValue.length > 0 && (!itemVal || col.filterValue.indexOf(itemVal) == -1)) {
                                return false;
                            }
                        }
                        else if (!itemVal || itemVal.toLowerCase().indexOf(col.filterValue.toLowerCase()) == -1)
                            return false;
                        break;
                    case gridview_1.FilterMode.NotEqual:
                        if (col.fieldType == gridview_1.FieldType.Date) {
                            return new Date(itemVal).getTime() != new Date(col.filterValue).getTime();
                        }
                        if (!itemVal || itemVal == col.filterValue)
                            return false;
                        break;
                    case gridview_1.FilterMode.Equals:
                        if (col.fieldType == gridview_1.FieldType.Date) {
                            return new Date(itemVal).getTime() == new Date(col.filterValue).getTime();
                        }
                        if (!itemVal || itemVal != col.filterValue)
                            return false;
                }
            }
        }
        return true;
    };
    Object.defineProperty(GridViewComponent.prototype, "displayData", {
        get: function () {
            if (this._displayData == null) {
                var rawData = this.unpagedData;
                if (this.grid.pageSize == 0 || this.grid.pagingType != gridview_1.PagingType.Auto)
                    this._displayData = rawData;
                else
                    this._displayData = rawData.slice((this.grid.currentPage - 1) * this.grid.pageSize, this.grid.currentPage * this.grid.pageSize);
            }
            return this._displayData;
        },
        enumerable: true,
        configurable: true
    });
    GridViewComponent.prototype.refreshDataSource = function () {
        this._displayData = null;
        this._unpagedData = null;
    };
    GridViewComponent.prototype.resetDisplayData = function () {
        this._displayData = null;
    };
    GridViewComponent.prototype.handlePageChanging = function () {
        if (this.detailGridViewComponents)
            this.collapseAll();
    };
    GridViewComponent.prototype.handlePageChanged = function (pageNumber) {
        if (this.pageChanged)
            this.pageChanged.emit(pageNumber);
        if (this.grid.saveGridStateToStorage)
            this.grid.saveGridState();
    };
    return GridViewComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_1.GridView),
    __metadata("design:paramtypes", [gridview_1.GridView])
], GridViewComponent.prototype, "grid", null);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], GridViewComponent.prototype, "parentComponent", void 0);
__decorate([
    core_1.ViewChild(gridview_pager_component_1.GridViewPagerComponent),
    __metadata("design:type", gridview_pager_component_1.GridViewPagerComponent),
    __metadata("design:paramtypes", [gridview_pager_component_1.GridViewPagerComponent])
], GridViewComponent.prototype, "pager", null);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], GridViewComponent.prototype, "sortChanged", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], GridViewComponent.prototype, "filterChanged", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], GridViewComponent.prototype, "pageChanged", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], GridViewComponent.prototype, "selectionChanged", void 0);
GridViewComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'gridview',
        styleUrls: ['gridview.css'],
        template: "\n<div *ngIf=\"grid\">\n    <div class='header-button' [hidden]='!(hasFilterRow())' (click)='toggleFilter()'><div class='glyphicon glyphicon-filter'></div><strong>&nbsp;&nbsp;Filter</strong></div>\n    <div class='header-button' [hidden]='!(hasFilterRow())' style='padding-right:5px'><input type='checkbox' (click)='toggleFilter()' [checked]='grid.filterVisible' /></div>\n    <div class='header-button' *ngIf='grid.detailGridView' (click)='collapseAll()' style='margin-bottom:2px'><div class='glyphicon glyphicon-minus'></div><strong>&nbsp;&nbsp;Collapse All</strong></div>\n    <div class='header-button' *ngIf='grid.detailGridView' (click)='expandAll()'><div class='glyphicon glyphicon-plus'></div><strong>&nbsp;&nbsp;Expand All</strong></div>\n    <table disable-animate [ngClass]=\"'gridview ' + (grid.noBorder ? '' : 'grid-border ') + (grid.height ? 'scrollable-table ' : '') + 'table table-condensed'\">\n        <thead [hidden]='!grid.showHeader'>\n            <tr>\n                <th *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton' style='width:39px'></th>\n                <th *ngIf='grid.allowRowSelect' style='width:1%'></th>\n                <th *ngFor=\"let col of grid.columns | orderBy:['columnIndex'];let i = index\" [hidden]='!col.visible' [style.width]=\"col.width\">\n                    <gridview-headercell (sortChanged)='handleSortChanged($event)' [columnIndex]='i' [column]='col' [parentGridView]=\"grid\"></gridview-headercell>\n                </th>\n            </tr>\n            <tr [hidden]='!(hasFilterRow() && grid.filterVisible)'>\n                <td *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton' style='width:39px'></td>\n                <td *ngFor=\"let col of grid.columns | orderBy:['columnIndex']\" [hidden]='!(col.visible || col.visible === undefined)'>\n\t\t\t\t\t<gridview-filtercell *ngIf=\"col.filterMode && col.filterMode != 0\" [parentGridView]=\"grid\" [parentGridViewComponent]=\"self\" [column]='col'>\n\t\t\t\t\t</gridview-filtercell>\n\t\t\t\t</td>\n            </tr>\n        </thead>\n        <tbody [style.height]=\"grid.height\">\n            <tr [hidden]='displayData != null || grid.loading'>\n                <td *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton'></td>\n                <td [attr.colspan]=\"getVisibleColumnCount()\">No results found!</td>\n            </tr>\n            <tr [hidden]='!(grid.loading)'>\n                <td [attr.colspan]=\"getVisibleColumnCount() + 1\">\n                    <div class=\"template-loading\">\n                        <div class=\"template-inner\">\n                            <br />\n                            <img src=\"data:image/gif;base64,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\" />\n                            &nbsp;&nbsp;<strong>Loading</strong>\n                            <br /><br />\n                        </div>\n                    </div>\n                </td>\n            </tr>\n            <tr [hidden]='!(grid.showNoResults && grid.data && grid.data.length < 1) || grid.loading'>\n                <td [attr.colspan]=\"getVisibleColumnCount() + 1\">\n                    <div class=\"template-loading\">\n                        <div class=\"template-inner\">\n                            <strong>No results found!</strong><br />\n                        </div>\n                    </div>\n                </td>\n            </tr>\n            <tr [hidden]='!(grid.loading)' style=\"display:none\"><td [attr.colspan]=\"getVisibleColumnCount() + 1\"></td></tr>\n            <template ngFor let-row [ngForOf]=\"displayData\" let-i=\"index\">\n                <tr [hidden]='grid.loading' *ngIf='!grid.rowTemplate' [ngClass]=\"(grid.getRowClass ? grid.getRowClass(row) : '') + (i % 2 != 0 ? ' gridview-alternate-row' : '') + (grid.selectMode > 0 ? ' selectable-row' : '') + (selectedKeys[row[grid.keyFieldName]] ? ' selected-row' : '')\" (click)='rowClick(row)'>\n                    <td *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton' style='width:39px'><button class=\"glyphicon glyphicon-small {{detailGridViewComponents[row[grid.keyFieldName]] && detailGridViewComponents[row[grid.keyFieldName]].isExpanded() ? 'glyphicon-minus' : 'glyphicon-plus'}}\" (click)='expandCollapse(row[grid.keyFieldName])'></button></td>\n                    <td *ngFor=\"let col of grid.columns | orderBy:['columnIndex']\" [hidden]='!(!grid.rowTemplate && (col.visible || col.visible === undefined))' [ngClass]=\"col.getRowCellClass ? col.getRowCellClass(row) : (col.disableWrapping ? 'no-wrap' : '')\">\n\t\t\t\t\t\t<gridview-cell [column]=\"col\" [row]=\"row\" [parentGridViewComponent]=\"self\" [parentGridView]=\"grid\"></gridview-cell>\n\t\t\t\t\t</td>\n                </tr>\n                <tr [hidden]='grid.loading' *ngIf='grid.rowTemplate'>\n                    <td [attr.colspan]=\"getVisibleColumnCount()\"><gridview-rowtemplate [parentGridView]=\"grid\" [parentGridViewComponent]=\"self\" [row]=\"row\"></gridview-rowtemplate></td>\n                </tr>\n                <tr [hidden]='grid.loading' *ngIf='grid.detailGridView' class=\"detail-gridview-row\" [hidden]='!detailGridViewComponents[row[grid.keyFieldName]] || !detailGridViewComponents[row[grid.keyFieldName]].isExpanded()'>\n                    <td *ngIf=\"!grid.detailGridView.hideExpandButton\"></td>\n                    <td [attr.colspan]=\"getVisibleColumnCount()\" class='detailgrid-container'><detail-gridview [parentGridViewComponent]=\"self\" [detailGridView]=\"grid.detailGridView\" [row]=\"row\"></detail-gridview></td>\n                </tr>\n            </template>\n        </tbody>\n        <tfoot *ngIf='grid.showFooter'>\n            <tr>\n                <td *ngIf='grid.detailGridView' style='width:39px'></td>\n                <td *ngFor=\"let col of grid.columns | orderBy:['columnIndex']\" [hidden]='!(col.visible || col.visible === undefined)' grid-view-footer-cell='col'></td>\n            </tr>\n        </tfoot>\n    </table>\n\t<div class='row'>\n\t\t<div class='pull-left'>\n\t\t\t<gridview-pager [parentGridView]='grid' [parentGridViewComponent]=\"self\" (pageChanging)='handlePageChanging()' (pageChanged)='handlePageChanged($event)'></gridview-pager>\n\t\t</div>\n\t\t<div class='pull-right gridview-settings'>\n\t\t\t<gridview-settings [parentGridView]='grid'></gridview-settings>\n\t\t</div>\n\t</div>\n</div>"
    }),
    __metadata("design:paramtypes", [parser_service_1.ParserService, core_1.NgZone])
], GridViewComponent);
exports.GridViewComponent = GridViewComponent;
//# sourceMappingURL=gridview.component.js.map