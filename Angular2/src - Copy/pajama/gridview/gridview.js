"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var parser_service_1 = require("../services/parser.service");
var order_by_pipe_1 = require("../pipes/order-by.pipe");
var GridView = (function () {
    function GridView() {
        this.pageSize = 10;
        this.currentPage = 1;
        this.columns = [];
        this.showHeader = true;
        this.pagingType = PagingType.Auto;
        this.dataChanged = new core_1.EventEmitter();
        this.customProps = {};
        this.customEvents = {};
        this.showNoResults = true;
        this.allowColumnOrdering = false;
        this.allowColumnCustomization = false;
        this.saveGridStateToStorage = false;
        this._stateLoaded = false;
    }
    GridView.prototype.getDataColumns = function () {
        var cols = [];
        for (var _i = 0, _a = this.columns; _i < _a.length; _i++) {
            var col = _a[_i];
            if (col instanceof DataColumn) {
                cols.push(col);
            }
        }
        return cols;
    };
    GridView.prototype.getDistinctValues = function (column) {
        if (!column.fieldName)
            return null;
        if (!this._data)
            return null;
        var parserService = new parser_service_1.ParserService();
        var vals = [];
        for (var i = 0; i < this._data.length; i++) {
            var val = parserService.getObjectValue(column.fieldName, this._data[i]);
            if (vals.indexOf(val) < 0)
                vals.push(val);
        }
        vals.sort();
        return vals;
    };
    GridView.prototype.setFilterOptions = function () {
        for (var _i = 0, _a = this.getDataColumns(); _i < _a.length; _i++) {
            var col = _a[_i];
            if (col.filterMode == FilterMode.DistinctList) {
                col.filterOptions = this.getDistinctValues(col);
                col.filterOptionsChanged.emit(col);
            }
        }
    };
    GridView.prototype.refreshData = function () {
        this.dataChanged.emit(this);
        this.setFilterOptions();
    };
    Object.defineProperty(GridView.prototype, "data", {
        get: function () {
            return this._data;
        },
        set: function (data) {
            this._data = data;
            this.refreshData();
        },
        enumerable: true,
        configurable: true
    });
    // TODO: where should this be called from?
    GridView.prototype.loadGridState = function () {
        if (this._stateLoaded || !this.saveGridStateToStorage)
            return;
        if (!this._defaultState) {
            var orderedCols = new order_by_pipe_1.OrderByPipe().transform(this.columns, ['columnIndex']);
            for (var i = 0; i < orderedCols.length; i++) {
                orderedCols[i].columnIndex = i;
            }
            this._defaultState = this.getGridState();
        }
        this._stateLoaded = true;
        var stateString = localStorage.getItem(this.name);
        if (stateString) {
            var state = JSON.parse(stateString);
            this.setGridState(state);
        }
    };
    GridView.prototype.saveGridState = function () {
        if (!this.saveGridStateToStorage)
            return;
        if (!this.name)
            throw 'Grid name required to save to local storage';
        var state = this.getGridState();
        // TODO: is grid name too generic?
        localStorage.setItem(this.name, JSON.stringify(state));
    };
    GridView.prototype.resetGridState = function () {
        if (this._defaultState) {
            this.setGridState(this._defaultState);
            localStorage.removeItem(this.name);
            // THIS SEEMS HACKISH! IN ORDER FOR THE COMPONENT TO REDRAW, IT NEEDS TO DETECT
            // A CHANGE TO THE COLUMNS VARIABLE ITSELF RATHER THAN WHAT'S IN THE COLLECTION
            var copies = [];
            for (var _i = 0, _a = this.columns; _i < _a.length; _i++) {
                var c = _a[_i];
                copies.push(c);
            }
            this.columns = copies;
            this.refreshData();
        }
    };
    GridView.prototype.getGridState = function () {
        var state = new GridState();
        state.currentPage = this.currentPage;
        state.pageSize = this.pageSize;
        state.filterVisible = this.filterVisible;
        for (var _i = 0, _a = this.columns; _i < _a.length; _i++) {
            var col = _a[_i];
            var colState = new GridColumnState();
            colState.identifier = col.getIdentifier();
            colState.columnIndex = col.columnIndex;
            colState.width = col.width;
            colState.visible = col.visible;
            if (col instanceof DataColumn) {
                var cd = col;
                colState.sortDirection = cd.sortDirection;
                colState.sortIndex = cd.sortIndex;
                // all selected
                if (cd.filterValue instanceof Array && cd.filterOptions && cd.filterValue.length >= cd.filterOptions.length)
                    colState.filterValue = null;
                else
                    colState.filterValue = cd.filterValue;
            }
            state.gridColumnStates.push(colState);
        }
        return state;
    };
    GridView.prototype.setGridState = function (state) {
        this.currentPage = state.currentPage;
        this.pageSize = state.pageSize;
        this.filterVisible = state.filterVisible;
        for (var _i = 0, _a = this.columns; _i < _a.length; _i++) {
            var col = _a[_i];
            for (var _b = 0, _c = state.gridColumnStates; _b < _c.length; _b++) {
                var colState = _c[_b];
                if (col.getIdentifier() != colState.identifier)
                    continue;
                col.columnIndex = colState.columnIndex;
                col.width = colState.width;
                col.visible = colState.visible;
                if (col instanceof DataColumn) {
                    var cd = col;
                    cd.sortDirection = colState.sortDirection;
                    cd.sortIndex = colState.sortIndex;
                    if (colState.filterValue instanceof Array) {
                        if (colState.filterValue.length > 0)
                            cd.filterValue = colState.filterValue;
                    }
                    else if (colState.filterValue)
                        cd.filterValue = colState.filterValue;
                }
            }
        }
    };
    return GridView;
}());
exports.GridView = GridView;
var ColumnSortDirection;
(function (ColumnSortDirection) {
    ColumnSortDirection[ColumnSortDirection["None"] = 0] = "None";
    ColumnSortDirection[ColumnSortDirection["Asc"] = 1] = "Asc";
    ColumnSortDirection[ColumnSortDirection["Desc"] = 2] = "Desc";
})(ColumnSortDirection = exports.ColumnSortDirection || (exports.ColumnSortDirection = {}));
var FilterMode;
(function (FilterMode) {
    FilterMode[FilterMode["None"] = 0] = "None";
    FilterMode[FilterMode["BeginsWith"] = 1] = "BeginsWith";
    FilterMode[FilterMode["Contains"] = 2] = "Contains";
    FilterMode[FilterMode["Equals"] = 3] = "Equals";
    FilterMode[FilterMode["NotEqual"] = 4] = "NotEqual";
    FilterMode[FilterMode["DistinctList"] = 5] = "DistinctList";
    FilterMode[FilterMode["DynamicList"] = 6] = "DynamicList";
})(FilterMode = exports.FilterMode || (exports.FilterMode = {}));
var PagingType;
(function (PagingType) {
    PagingType[PagingType["Auto"] = 0] = "Auto";
    PagingType[PagingType["Manual"] = 1] = "Manual";
    PagingType[PagingType["Disabled"] = 2] = "Disabled";
})(PagingType = exports.PagingType || (exports.PagingType = {}));
var SelectMode;
(function (SelectMode) {
    SelectMode[SelectMode["None"] = 0] = "None";
    SelectMode[SelectMode["Single"] = 1] = "Single";
    SelectMode[SelectMode["Multi"] = 2] = "Multi";
})(SelectMode = exports.SelectMode || (exports.SelectMode = {}));
var ColumnBase = (function () {
    function ColumnBase(caption) {
        this.caption = caption;
        this.visible = true;
        this.columnIndex = 0;
        this.dataChanged = new core_1.EventEmitter();
        this.customProps = {};
    }
    ColumnBase.prototype.getIdentifier = function () {
        if (!this.name)
            this.name = Math.floor((1 + Math.random()) * 0x10000).toString();
        return this.name;
    };
    return ColumnBase;
}());
exports.ColumnBase = ColumnBase;
var DataColumn = (function (_super) {
    __extends(DataColumn, _super);
    function DataColumn(fieldName, caption) {
        var _this = _super.call(this, caption) || this;
        _this.fieldName = fieldName;
        _this.caption = caption;
        _this.fieldType = FieldType.String;
        _this.sortIndex = 0;
        _this.filterMode = FilterMode.None;
        _this.filterDelayMilliseconds = 0;
        _this.sortDirection = ColumnSortDirection.None;
        _this.filterOptionsChanged = new core_1.EventEmitter();
        _this.dataChanged.subscribe(function (d) {
        });
        return _this;
    }
    Object.defineProperty(DataColumn.prototype, "filterOptions", {
        get: function () {
            return this._filterOptions;
        },
        set: function (v) {
            this._filterOptions = v;
            this.filterOptionsChanged.emit(v);
        },
        enumerable: true,
        configurable: true
    });
    DataColumn.prototype.getODataField = function () {
        if (!this.fieldName)
            return '';
        var expression = '';
        var parts = this.fieldName.split('.');
        var firstIn = true;
        for (var _i = 0, parts_1 = parts; _i < parts_1.length; _i++) {
            var p = parts_1[_i];
            expression += (firstIn ? '' : '/');
            expression += p.substring(0, 1).toUpperCase() + p.substring(1);
            firstIn = false;
        }
        return expression;
    };
    DataColumn.prototype.getCaption = function () {
        if (this.caption)
            return this.caption;
        var parsedFieldName = this.fieldName;
        if (!parsedFieldName || parsedFieldName == '')
            return '';
        if (parsedFieldName.lastIndexOf('.') > 0) {
            parsedFieldName = parsedFieldName.substring(parsedFieldName.lastIndexOf('.') + 1, parsedFieldName.length);
        }
        return parsedFieldName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) {
            return str.toUpperCase();
        });
    };
    DataColumn.prototype.getIdentifier = function () {
        if (this.name)
            return this.name;
        if (this.fieldName)
            return this.fieldName;
        return this.caption;
    };
    return DataColumn;
}(ColumnBase));
exports.DataColumn = DataColumn;
var NumericColumn = (function (_super) {
    __extends(NumericColumn, _super);
    function NumericColumn() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.decimalPlaces = 0;
        return _this;
    }
    return NumericColumn;
}(DataColumn));
exports.NumericColumn = NumericColumn;
var LinkColumn = (function (_super) {
    __extends(LinkColumn, _super);
    function LinkColumn(fieldName, caption) {
        var _this = _super.call(this, fieldName, caption) || this;
        _this.fieldName = fieldName;
        _this.caption = caption;
        _this.parameters = {};
        return _this;
    }
    return LinkColumn;
}(DataColumn));
exports.LinkColumn = LinkColumn;
var ButtonColumn = (function (_super) {
    __extends(ButtonColumn, _super);
    function ButtonColumn(fieldName, caption) {
        var _this = _super.call(this, fieldName, caption) || this;
        _this.fieldName = fieldName;
        _this.caption = caption;
        _this.click = new core_1.EventEmitter();
        return _this;
    }
    return ButtonColumn;
}(DataColumn));
exports.ButtonColumn = ButtonColumn;
var EditColumn = (function (_super) {
    __extends(EditColumn, _super);
    function EditColumn() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.editType = EditColumn.TEXT;
        _this.ngModelChange = new core_1.EventEmitter();
        return _this;
    }
    return EditColumn;
}(DataColumn));
EditColumn.TEXT = "text";
EditColumn.TEXTAREA = "textarea";
EditColumn.NUMBER = "number";
EditColumn.CHECKBOX = "checkbox";
exports.EditColumn = EditColumn;
var CheckListColumn = (function (_super) {
    __extends(CheckListColumn, _super);
    function CheckListColumn() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Object.defineProperty(CheckListColumn.prototype, "checkList", {
        get: function () { return true; },
        enumerable: true,
        configurable: true
    });
    return CheckListColumn;
}(DataColumn));
exports.CheckListColumn = CheckListColumn;
var TypeaheadColumn = (function (_super) {
    __extends(TypeaheadColumn, _super);
    function TypeaheadColumn() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.ngModelChange = new core_1.EventEmitter();
        return _this;
    }
    Object.defineProperty(TypeaheadColumn.prototype, "typeahead", {
        // TODO: what else should be exposed?
        get: function () { return true; },
        enumerable: true,
        configurable: true
    });
    return TypeaheadColumn;
}(DataColumn));
exports.TypeaheadColumn = TypeaheadColumn;
var ColumnPipe = (function () {
    function ColumnPipe(pipe, args) {
        this.pipe = pipe;
        this.args = args;
    }
    return ColumnPipe;
}());
exports.ColumnPipe = ColumnPipe;
var FieldType;
(function (FieldType) {
    FieldType[FieldType["String"] = 0] = "String";
    FieldType[FieldType["Boolean"] = 1] = "Boolean";
    FieldType[FieldType["Date"] = 2] = "Date";
    FieldType[FieldType["Html"] = 3] = "Html";
})(FieldType = exports.FieldType || (exports.FieldType = {}));
var DetailGridViewDataEventArgs = (function () {
    function DetailGridViewDataEventArgs(parentRow, detailGridViewInstance) {
        this.parentRow = parentRow;
        this.detailGridViewInstance = detailGridViewInstance;
    }
    return DetailGridViewDataEventArgs;
}());
exports.DetailGridViewDataEventArgs = DetailGridViewDataEventArgs;
var DetailGridView = (function (_super) {
    __extends(DetailGridView, _super);
    function DetailGridView() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.setChildData = new core_1.EventEmitter();
        return _this;
    }
    DetailGridView.prototype.createInstance = function () {
        var grid = new DetailGridView();
        Object.assign(grid, this);
        return grid;
    };
    return DetailGridView;
}(GridView));
exports.DetailGridView = DetailGridView;
var GridViewTemplate = (function () {
    function GridViewTemplate(template, imports, declarations, styleUrls) {
        this.template = template;
        this.imports = imports;
        this.declarations = declarations;
        this.styleUrls = styleUrls;
    }
    return GridViewTemplate;
}());
exports.GridViewTemplate = GridViewTemplate;
// don't want to clutter storage with unneccessary info
var GridState = (function () {
    function GridState() {
        this.gridColumnStates = [];
    }
    return GridState;
}());
exports.GridState = GridState;
var GridColumnState = (function () {
    function GridColumnState() {
    }
    return GridColumnState;
}());
exports.GridColumnState = GridColumnState;
//# sourceMappingURL=gridview.js.map