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
var gridview_component_1 = require('./gridview.component');
var GridViewFilterCellComponent = (function () {
    function GridViewFilterCellComponent(elementRef) {
        this.elementRef = elementRef;
        this.checklistItems = [];
        this.filterMode = gridview_1.FilterMode;
        this.parentWidth = 0;
    }
    GridViewFilterCellComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.column.filterMode == gridview_1.FilterMode.DistinctList || this.column.filterMode == gridview_1.FilterMode.DynamicList || this.column.filterOptions) {
            this.checklistItems = this.column.filterOptions || [];
            var copy = [];
            if (this.checklistItems) {
                for (var _i = 0, _a = this.checklistItems; _i < _a.length; _i++) {
                    var ci = _a[_i];
                    copy.push(ci);
                }
            }
            this.column.filterValue = copy;
            this.column.filterOptionsChanged.subscribe(function () {
                _this.checklistItems = _this.column.filterOptions;
                var copy2 = [];
                if (_this.checklistItems) {
                    for (var _i = 0, _a = _this.checklistItems; _i < _a.length; _i++) {
                        var ci = _a[_i];
                        copy2.push(ci);
                    }
                }
                _this.column.filterValue = copy2;
            });
            if (!this.column.width)
                this.parentWidth = this.elementRef.nativeElement.parentElement.offsetWidth;
        }
    };
    GridViewFilterCellComponent.prototype.filterChanged = function () {
        var _this = this;
        if (this.column.filterDelayMilliseconds > 0) {
            this._lastChange = new Date();
            window.setTimeout(function () {
                var now = new Date();
                if (now.getTime() - _this._lastChange.getTime() >= _this.column.filterDelayMilliseconds - 1) {
                    _this.fireFilter();
                }
            }, this.column.filterDelayMilliseconds);
        }
        else {
            this.fireFilter();
        }
    };
    GridViewFilterCellComponent.prototype.fireFilter = function () {
        this.parentGridView.currentPage = 1;
        this.parentGridView.dataChanged.emit(this.parentGridView);
        this.parentGridViewComponent.filterChanged.emit(this.column);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.DataColumn)
    ], GridViewFilterCellComponent.prototype, "column", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.GridView)
    ], GridViewFilterCellComponent.prototype, "parentGridView", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_component_1.GridViewComponent)
    ], GridViewFilterCellComponent.prototype, "parentGridViewComponent", void 0);
    GridViewFilterCellComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'gridview-filtercell',
            styleUrls: ['gridview.css'],
            template: "\n<div *ngIf='column.filterTemplate'>\n\n</div>\n<div *ngIf='!column.filterTemplate' [ngSwitch]='column.filterMode == filterMode.DistinctList || column.filterMode == filterMode.DynamicList || column.filterOptions'>\n\t<div *ngSwitchCase='true'>\n\t\t<check-list name='filtcheck' [ngStyle]=\"{'maxWidth': column.width || (parentWidth + 'px')}\" [showFilterIcon]='true' [items]='checklistItems' [selectedItems]='column.filterValue' (selectionChanged)='filterChanged()'  class='filter-check-list'></check-list>\n\t</div>\n\t<div *ngSwitchDefault>\n\t\t<input type='text' [(ngModel)]='column.filterValue' (ngModelChange)='filterChanged()' class='form-control filter-control' />\n\t</div>\n</div>\n"
        }), 
        __metadata('design:paramtypes', [core_1.ElementRef])
    ], GridViewFilterCellComponent);
    return GridViewFilterCellComponent;
}());
exports.GridViewFilterCellComponent = GridViewFilterCellComponent;
//# sourceMappingURL=gridview-filtercell.component.js.map