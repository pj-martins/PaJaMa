"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var core_1 = require('@angular/core');
var parser_service_1 = require('../services/parser.service');
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
    return GridView;
}());
exports.GridView = GridView;
(function (SortDirection) {
    SortDirection[SortDirection["None"] = 0] = "None";
    SortDirection[SortDirection["Asc"] = 1] = "Asc";
    SortDirection[SortDirection["Desc"] = 2] = "Desc";
})(exports.SortDirection || (exports.SortDirection = {}));
var SortDirection = exports.SortDirection;
(function (FilterMode) {
    FilterMode[FilterMode["None"] = 0] = "None";
    FilterMode[FilterMode["BeginsWith"] = 1] = "BeginsWith";
    FilterMode[FilterMode["Contains"] = 2] = "Contains";
    FilterMode[FilterMode["Equals"] = 3] = "Equals";
    FilterMode[FilterMode["NotEqual"] = 4] = "NotEqual";
    FilterMode[FilterMode["DistinctList"] = 5] = "DistinctList";
    FilterMode[FilterMode["DynamicList"] = 6] = "DynamicList";
})(exports.FilterMode || (exports.FilterMode = {}));
var FilterMode = exports.FilterMode;
(function (PagingType) {
    PagingType[PagingType["Auto"] = 0] = "Auto";
    PagingType[PagingType["Manual"] = 1] = "Manual";
    PagingType[PagingType["Disabled"] = 2] = "Disabled";
})(exports.PagingType || (exports.PagingType = {}));
var PagingType = exports.PagingType;
(function (SelectMode) {
    SelectMode[SelectMode["None"] = 0] = "None";
    SelectMode[SelectMode["Single"] = 1] = "Single";
    SelectMode[SelectMode["Multi"] = 2] = "Multi";
})(exports.SelectMode || (exports.SelectMode = {}));
var SelectMode = exports.SelectMode;
var ColumnBase = (function () {
    function ColumnBase(caption) {
        this.caption = caption;
        this.visible = true;
        this.columnIndex = 0;
        this.dataChanged = new core_1.EventEmitter();
    }
    ColumnBase.prototype.getIdentifier = function () {
        return this.name;
    };
    return ColumnBase;
}());
exports.ColumnBase = ColumnBase;
var DataColumn = (function (_super) {
    __extends(DataColumn, _super);
    function DataColumn(fieldName, caption) {
        _super.call(this, caption);
        this.fieldName = fieldName;
        this.caption = caption;
        this.fieldType = FieldType.String;
        this.sortIndex = 0;
        this.filterMode = FilterMode.None;
        this.filterDelayMilliseconds = 0;
        this.sortDirection = SortDirection.None;
        this.filterOptionsChanged = new core_1.EventEmitter();
        this.dataChanged.subscribe(function (d) {
        });
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
var LinkColumn = (function (_super) {
    __extends(LinkColumn, _super);
    function LinkColumn(fieldName, caption) {
        _super.call(this, fieldName, caption);
        this.fieldName = fieldName;
        this.caption = caption;
        this.parameters = {};
    }
    return LinkColumn;
}(DataColumn));
exports.LinkColumn = LinkColumn;
var ButtonColumn = (function (_super) {
    __extends(ButtonColumn, _super);
    function ButtonColumn(fieldName, caption) {
        _super.call(this, fieldName, caption);
        this.fieldName = fieldName;
        this.caption = caption;
        this.click = new core_1.EventEmitter();
    }
    return ButtonColumn;
}(DataColumn));
exports.ButtonColumn = ButtonColumn;
var EditColumn = (function (_super) {
    __extends(EditColumn, _super);
    function EditColumn() {
        _super.apply(this, arguments);
        this.editType = EditColumn.TEXT;
        this.ngModelChange = new core_1.EventEmitter();
    }
    EditColumn.TEXT = "text";
    EditColumn.TEXTAREA = "textarea";
    EditColumn.NUMBER = "number";
    EditColumn.CHECKBOX = "checkbox";
    return EditColumn;
}(DataColumn));
exports.EditColumn = EditColumn;
var CheckListColumn = (function (_super) {
    __extends(CheckListColumn, _super);
    function CheckListColumn() {
        _super.apply(this, arguments);
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
        _super.apply(this, arguments);
        this.ngModelChange = new core_1.EventEmitter();
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
(function (FieldType) {
    FieldType[FieldType["String"] = 0] = "String";
    FieldType[FieldType["Boolean"] = 1] = "Boolean";
    FieldType[FieldType["Date"] = 2] = "Date";
    FieldType[FieldType["Html"] = 3] = "Html";
})(exports.FieldType || (exports.FieldType = {}));
var FieldType = exports.FieldType;
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
        _super.apply(this, arguments);
        this.setChildData = new core_1.EventEmitter();
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
//# sourceMappingURL=gridview.js.map