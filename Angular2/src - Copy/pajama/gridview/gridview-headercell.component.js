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
var GridViewHeaderCellComponent = (function () {
    function GridViewHeaderCellComponent(elementRef, zone) {
        this.elementRef = elementRef;
        this.zone = zone;
        this.sortChanged = new core_1.EventEmitter();
        this.widthChanged = new core_1.EventEmitter();
        this.sortDirection = gridview_1.ColumnSortDirection;
        this._lockedColumns = [];
        this._resized = false;
        // IE has to be 'text' for some reason
        this.COLUMN_ID = "text";
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
                    col.sortDirection = gridview_1.ColumnSortDirection.None;
                    col.sortIndex = 0;
                }
                else if (col.sortIndex > maxIndex)
                    maxIndex = col.sortIndex;
            }
        }
        if (event.ctrlKey)
            column.sortIndex = maxIndex + 1;
        if (column.sortDirection === undefined) {
            column.sortDirection = gridview_1.ColumnSortDirection.Asc;
        }
        else {
            switch (column.sortDirection) {
                case gridview_1.ColumnSortDirection.None:
                case gridview_1.ColumnSortDirection.Desc:
                    column.sortDirection = gridview_1.ColumnSortDirection.Asc;
                    break;
                case 1:
                    column.sortDirection = gridview_1.ColumnSortDirection.Desc;
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
        while (next != null) {
            if (next.nextElementSibling == null) {
                // this is our floater
                if (next.style.width != "") {
                    // this is a scenario (should be the only scenario) where a column's order was previously changed
                    // then it was dragged to become the last, if this is the case we need to lock other columns and clear
                    // this one out so it can float
                    var tempPrev = next.previousElementSibling;
                    while (tempPrev != null) {
                        tempPrev.style.width = tempPrev.offsetWidth + 'px';
                        tempPrev = tempPrev.previousElementSibling;
                    }
                }
                next.style.width = "";
                break;
            }
            else {
                this._lockedColumns.push(new LockedColumn(next, next.offsetWidth));
                next = next.nextElementSibling;
            }
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
            this.widthChanged.emit(this.column);
        }
    };
    GridViewHeaderCellComponent.prototype.resize = function (event) {
        if (this._currEvt && event.buttons == 1) {
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
        this.changeColumnOrder(id, event.currentTarget.id);
    };
    GridViewHeaderCellComponent.prototype.changeColumnOrder = function (sourceIdentifier, targetIdentifier) {
        var sourceCol;
        var targetCol;
        for (var _i = 0, _a = this.parentGridView.columns; _i < _a.length; _i++) {
            var col = _a[_i];
            if (col.getIdentifier() == sourceIdentifier) {
                sourceCol = col;
                break;
            }
        }
        for (var _b = 0, _c = this.parentGridView.columns; _b < _c.length; _b++) {
            var col = _c[_b];
            if (col.getIdentifier() == targetIdentifier) {
                targetCol = col;
                break;
            }
        }
        if (!sourceCol)
            throw sourceIdentifier + " not found!";
        if (!targetCol)
            throw targetIdentifier + " not found!";
        var targetIndex = targetCol.columnIndex;
        if (sourceCol.columnIndex <= targetCol.columnIndex) {
            for (var _d = 0, _e = this.parentGridView.columns; _d < _e.length; _d++) {
                var col = _e[_d];
                if (col.getIdentifier() == sourceCol.getIdentifier())
                    continue;
                if (col.columnIndex > sourceCol.columnIndex && col.columnIndex <= targetCol.columnIndex)
                    col.columnIndex--;
            }
        }
        else {
            for (var _f = 0, _g = this.parentGridView.columns; _f < _g.length; _f++) {
                var col = _g[_f];
                if (col.getIdentifier() == sourceCol.getIdentifier())
                    continue;
                if (col.columnIndex < sourceCol.columnIndex && col.columnIndex >= targetCol.columnIndex)
                    col.columnIndex++;
            }
        }
        sourceCol.columnIndex = targetIndex;
        // THIS SEEMS HACKISH! IN ORDER FOR THE COMPONENT TO REDRAW, IT NEEDS TO DETECT
        // A CHANGE TO THE COLUMNS VARIABLE ITSELF RATHER THAN WHAT'S IN THE COLLECTION
        var sortedColumns = [];
        for (var _h = 0, _j = this.parentGridView.columns; _h < _j.length; _h++) {
            var c = _j[_h];
            sortedColumns.push(c);
        }
        this.parentGridView.columns = sortedColumns;
        if (this.parentGridView.saveGridStateToStorage)
            this.parentGridView.saveGridState();
    };
    return GridViewHeaderCellComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_1.DataColumn)
], GridViewHeaderCellComponent.prototype, "column", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_1.GridView)
], GridViewHeaderCellComponent.prototype, "parentGridView", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Number)
], GridViewHeaderCellComponent.prototype, "columnIndex", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], GridViewHeaderCellComponent.prototype, "sortChanged", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], GridViewHeaderCellComponent.prototype, "widthChanged", void 0);
GridViewHeaderCellComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'gridview-headercell',
        styleUrls: ['gridview-headercell.css'],
        template: "\n<div class='sort-header' (click)='setSort(column, $event)' [id]='column.getIdentifier()' draggable=\"true\" (dragover)=\"dragOver($event)\" (dragstart)=\"dragStart($event)\" (drop)=\"drop($event)\">\n\t<div class='header-caption' [style.width]=\"(column.fieldName || column.sortField) && column.sortable ? '' : '100%'\">\n\t\t<div [innerHTML]=\"column.getCaption()\"></div>\n\t</div>\n\t<div [ngClass]=\"{ 'header-caption sort-arrows' : (column.fieldName || column.sortField) && column.sortable }\" *ngIf='(column.fieldName || column.sortField) && column.sortable'>\n\t\t<div [ngClass]=\"'sort-arrow top-empty' + (column.sortDirection == sortDirection.None ? ' glyphicon glyphicon-menu-up' : '')\"></div>\n\t\t<div [ngClass]=\"'sort-arrow bottom-empty' + (column.sortDirection == sortDirection.None ? ' glyphicon glyphicon-menu-down' : '')\"></div>\n\t\t<div [ngClass]=\"'sort-arrow' + (column.sortDirection == sortDirection.Desc ? ' glyphicon glyphicon-menu-up' : '')\"></div>\n\t\t<div [ngClass]=\"'sort-arrow' + (column.sortDirection == sortDirection.Asc ? ' glyphicon glyphicon-menu-down' : '')\"></div>\n\t</div>\n</div>\n<div class='resize-div' *ngIf='column.allowSizing' (mousedown)='startResize($event)'>|</div>\n"
    }),
    __metadata("design:paramtypes", [core_1.ElementRef, core_1.NgZone])
], GridViewHeaderCellComponent);
exports.GridViewHeaderCellComponent = GridViewHeaderCellComponent;
var LockedColumn = (function () {
    function LockedColumn(parentTH, originalWidth) {
        this.parentTH = parentTH;
        this.originalWidth = originalWidth;
    }
    return LockedColumn;
}());
//# sourceMappingURL=gridview-headercell.component.js.map