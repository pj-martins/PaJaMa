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
var core_1 = require('@angular/core');
var gridview_1 = require('./gridview');
var GridViewHeaderCellComponent = (function () {
    function GridViewHeaderCellComponent(elementRef, zone) {
        this.elementRef = elementRef;
        this.zone = zone;
        this.sortChanged = new core_1.EventEmitter();
        this.widthChanged = new core_1.EventEmitter();
        this.columnOrderChanged = new core_1.EventEmitter();
        this.sortDirection = gridview_1.SortDirection;
        this._lockedColumns = [];
        this._resized = false;
        this.COLUMN_ID = "column_id";
    }
    GridViewHeaderCellComponent.prototype.setSort = function (column, event) {
        if (!column.sortable)
            return;
        var maxIndex = -1;
        for (var _i = 0, _a = this.parentGridView.getDataColumns(); _i < _a.length; _i++) {
            var col = _a[_i];
            if (col == column)
                continue;
            if (col.sortable) {
                if (!event.ctrlKey) {
                    col.sortDirection = gridview_1.SortDirection.None;
                    col.sortIndex = 0;
                }
                else if (col.sortIndex > maxIndex)
                    maxIndex = col.sortIndex;
            }
        }
        if (event.ctrlKey)
            column.sortIndex = maxIndex + 1;
        if (column.sortDirection === undefined) {
            column.sortDirection = gridview_1.SortDirection.Asc;
        }
        else {
            switch (column.sortDirection) {
                case gridview_1.SortDirection.None:
                case gridview_1.SortDirection.Desc:
                    column.sortDirection = gridview_1.SortDirection.Asc;
                    break;
                case 1:
                    column.sortDirection = gridview_1.SortDirection.Desc;
                    break;
            }
        }
        if (this.sortChanged)
            this.sortChanged.emit(column);
    };
    // we could set the column widths directly but that will cause grid to redraw which would
    // be expensive, so we'll wait until after
    GridViewHeaderCellComponent.prototype.startResize = function (evt) {
        var _this = this;
        if (this.elementRef.nativeElement.parentElement.nextElementSibling == null)
            return;
        this.elementRef.nativeElement.draggable = false;
        this._currEvt = evt;
        this._origMove = window.onmousemove;
        this._origUp = window.onmouseup;
        this._lockedColumns = [];
        this._parentTH = this.elementRef.nativeElement.parentElement;
        var next = this._parentTH.nextElementSibling;
        while (next != null && next.nextElementSibling != null) {
            this._lockedColumns.push(new LockedColumn(next, next.offsetWidth));
            next = next.nextElementSibling;
        }
        var prev = this._parentTH.previousElementSibling;
        while (prev != null) {
            this._lockedColumns.push(new LockedColumn(prev, prev.offsetWidth));
            prev = prev.previousElementSibling;
        }
        window.onmousemove = function () { return _this.resize(event); };
        window.onmouseup = function () { return _this.endResize(); };
    };
    // TODO: test test test
    GridViewHeaderCellComponent.prototype.endResize = function () {
        window.onmousemove = this._origMove;
        window.onmouseup = this._origUp;
        this._currEvt = null;
        this._origMove = null;
        this._origUp = null;
        if (this._resized) {
            this._resized = false;
            for (var _i = 0, _a = this.parentGridView.columns; _i < _a.length; _i++) {
                var col = _a[_i];
                if (col.getIdentifier() == this.elementRef.nativeElement.firstElementChild.id)
                    col.width = this._parentTH.offsetWidth.toString() + 'px';
                else {
                    for (var _b = 0, _c = this._lockedColumns; _b < _c.length; _b++) {
                        var l = _c[_b];
                        if (col.getIdentifier() == l.parentTH.children[0].children[0].id) {
                            col.width = l.originalWidth.toString() + 'px';
                        }
                    }
                }
            }
            if (this.parentGridView.saveGridStateToStorage)
                this.parentGridView.saveGridState();
        }
    };
    GridViewHeaderCellComponent.prototype.resize = function (event) {
        if (this._currEvt) {
            var currX = event.clientX;
            var delta = event.clientX - this._currEvt.clientX;
            this._parentTH.style.width = (this._parentTH.offsetWidth + delta).toString() + 'px';
            for (var _i = 0, _a = this._lockedColumns; _i < _a.length; _i++) {
                var locked = _a[_i];
                locked.parentTH.width = locked.originalWidth;
            }
            this._currEvt = event;
            this._resized = true;
        }
        else {
            this.endResize();
        }
    };
    GridViewHeaderCellComponent.prototype.dragOver = function (event) {
        if (!this.parentGridView.allowColumnOrdering)
            return;
        event.preventDefault();
    };
    GridViewHeaderCellComponent.prototype.dragStart = function (event) {
        if (!this.parentGridView.allowColumnOrdering)
            return;
        event.dataTransfer.setData(this.COLUMN_ID, event.currentTarget.id);
    };
    GridViewHeaderCellComponent.prototype.drop = function (event) {
        if (!this.parentGridView.allowColumnOrdering)
            return;
        var id = event.dataTransfer.getData(this.COLUMN_ID);
        this.columnOrderChanged.emit(new ColumnOrder(id, event.currentTarget.id));
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.DataColumn)
    ], GridViewHeaderCellComponent.prototype, "column", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.GridView)
    ], GridViewHeaderCellComponent.prototype, "parentGridView", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], GridViewHeaderCellComponent.prototype, "columnIndex", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], GridViewHeaderCellComponent.prototype, "sortChanged", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], GridViewHeaderCellComponent.prototype, "widthChanged", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], GridViewHeaderCellComponent.prototype, "columnOrderChanged", void 0);
    GridViewHeaderCellComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'gridview-headercell',
            styleUrls: ['gridview.css'],
            template: "\n<div class='sort-header' (click)='setSort(column, $event)' [id]='column.getIdentifier()' draggable=\"true\" (dragover)=\"dragOver($event)\" (dragstart)=\"dragStart($event)\" (drop)=\"drop($event)\">\n\t<div class='header-caption' [style.width]=\"(column.fieldName || column.sortField) && column.sortable ? '' : '100%'\">\n\t\t<div [innerHTML]=\"column.getCaption()\"></div>\n\t</div>\n\t<div [ngClass]=\"{ 'header-caption sort-arrows' : (column.fieldName || column.sortField) && column.sortable }\" *ngIf='(column.fieldName || column.sortField) && column.sortable'>\n\t\t<div [ngClass]=\"'sort-arrow top-empty' + (column.sortDirection == sortDirection.None ? ' glyphicon glyphicon-menu-up' : '')\"></div>\n\t\t<div [ngClass]=\"'sort-arrow bottom-empty' + (column.sortDirection == sortDirection.None ? ' glyphicon glyphicon-menu-down' : '')\"></div>\n\t\t<div [ngClass]=\"'sort-arrow' + (column.sortDirection == sortDirection.Desc ? ' glyphicon glyphicon-menu-up' : '')\"></div>\n\t\t<div [ngClass]=\"'sort-arrow' + (column.sortDirection == sortDirection.Asc ? ' glyphicon glyphicon-menu-down' : '')\"></div>\n\t</div>\n</div>\n<div class='resize-div' *ngIf='column.allowSizing' (mousedown)='startResize($event)'>|</div>\n"
        }), 
        __metadata('design:paramtypes', [core_1.ElementRef, core_1.NgZone])
    ], GridViewHeaderCellComponent);
    return GridViewHeaderCellComponent;
}());
exports.GridViewHeaderCellComponent = GridViewHeaderCellComponent;
var LockedColumn = (function () {
    function LockedColumn(parentTH, originalWidth) {
        this.parentTH = parentTH;
        this.originalWidth = originalWidth;
    }
    return LockedColumn;
}());
var ColumnOrder = (function () {
    function ColumnOrder(sourceIdentifier, targetIdentifier) {
        this.sourceIdentifier = sourceIdentifier;
        this.targetIdentifier = targetIdentifier;
    }
    return ColumnOrder;
}());
exports.ColumnOrder = ColumnOrder;
//# sourceMappingURL=gridview-headercell.component.js.map