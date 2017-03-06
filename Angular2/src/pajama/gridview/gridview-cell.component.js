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
var parser_service_1 = require('../services/parser.service');
var GridViewCellComponent = (function () {
    function GridViewCellComponent(parserService) {
        this.parserService = parserService;
        this.fieldType = gridview_1.FieldType;
    }
    GridViewCellComponent.prototype.getObjectValue = function () {
        var val = this.parserService.getObjectValue(this.column.fieldName, this.row);
        if (this.column.columnPipe) {
            val = this.column.columnPipe.pipe.transform(val, this.column.columnPipe.args);
        }
        return val;
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.DataColumn)
    ], GridViewCellComponent.prototype, "column", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], GridViewCellComponent.prototype, "row", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_component_1.GridViewComponent)
    ], GridViewCellComponent.prototype, "parentGridViewComponent", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.GridView)
    ], GridViewCellComponent.prototype, "parentGridView", void 0);
    GridViewCellComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'gridview-cell',
            styleUrls: ['gridview.css'],
            template: "\n<div *ngIf=\"column.template\">\n\t<gridview-cell-template [column]=\"col\" [row]=\"row\" [parentGridViewComponent]=\"self\" [parentGridView]=\"grid\"></gridview-cell-template>\n</div>\n<div *ngIf=\"!column.template && column.fieldType == fieldType.Date\">\n\t<div [innerHTML]=\"parserService.getObjectValue(column.fieldName, row) == null ? '' : parserService.getObjectValue(column.fieldName, row) | moment:(column.format ? column.format : 'MM/DD/YYYY')\"></div>\n</div>\n<div *ngIf=\"!column.template && !column.format && column.fieldType == fieldType.Boolean\">\n\t<div [ngClass]=\"{ 'glyphicon glyphicon-ok' : parserService.getObjectValue(column.fieldName, row) == true }\"></div>\n</div>\n<!-- TODO: should we allow links to above items? duplication here too -->\n<div *ngIf=\"column.fieldType != fieldType.Date && column.fieldType != fieldType.Boolean && !column.template && !column.format\">\n\t<div *ngIf=\"column.url\">\n\t\t<!-- TODO: always open in new window? -->\n\t\t<a href='{{getLink(col, row)}}' class=\"{{column.class}}\" target='{{getLinkTarget(col, row)}}'>\n\t\t\t<div [innerHTML]=\"parserService.getObjectValue(column.fieldName, row) == null ? '' : parserService.getObjectValue(column.fieldName, row)\"></div>\n\t\t</a>\n\t</div>\n\t<div *ngIf=\"column.click\">\n\t\t<button class=\"{{column.class}}\" (click)=\"column.click.emit(row)\">{{parserService.getObjectValue(column.fieldName, row) == null ? '' : parserService.getObjectValue(column.fieldName, row)}}</button>\n\t</div>\n\t<div *ngIf=\"column.editType\" [ngSwitch]=\"column.editType\">\n\t\t<div *ngSwitchCase=\"'textarea'\">\n\t\t\t<textarea [style.width]=\"column.width\" class=\"{{column.class}}\" (ngModelChange)=\"column.ngModelChange.emit(row)\" [(ngModel)]=\"row[column.fieldName]\" [minlength]=\"column.min\" [maxlength]=\"column.max\"></textarea>\n\t\t</div>\n\t\t<div *ngSwitchCase=\"'checkbox'\">\n\t\t\t<input type=\"checkbox\" [style.width]=\"column.width\" class=\"{{column.class}}\" (ngModelChange)=\"column.ngModelChange.emit(row)\" [(ngModel)]=\"row[column.fieldName]\" />\n\t\t</div>\n\t\t<!-- for some reason below can't handle checkbox -->\n\t\t<div *ngSwitchDefault>\n\t\t\t<input type=\"{{column.editType}}\" [style.width]=\"column.width\" class=\"{{column.class}}\" (ngModelChange)=\"column.ngModelChange.emit(row)\" [(ngModel)]=\"row[column.fieldName]\" [min]=\"column.min\" [max]=\"column.max\" [minlength]=\"column.min\" [maxlength]=\"column.max\" />\n\t\t</div>\n\t</div>\n\t<div *ngIf=\"column.checkList\">\n\t\t<div [style.width]=\"column.width\" style='word-break: break-word'><check-list [items]='column.items' [selectedItems]='row[column.fieldName]' [displayMember]=\"column.displayMember\"></check-list></div>\n\t</div>\n\t<div *ngIf=\"column.typeahead\">\n\t\t<!-- TODO: single -->\n\t\t<div *ngIf=\"column.multi\">\n\t\t\t<div [style.width]=\"column.width\"><multi-typeahead (ngModelChange)='column.ngModelChange.emit(row)' [dataSource]='column.dataSource' [(ngModel)]='row[column.fieldName]'></multi-typeahead></div>\n\t\t</div>\n\t</div>\n\t<div *ngIf=\"!column.url && !column.click && !column.editType && !column.checkList && !column.typeahead\">\n\t\t<div [innerHTML]=\"parserService.getObjectValue(column.fieldName, row) == null ? '' : getObjectValue()\"></div>\n\t</div>\n</div>\n"
        }), 
        __metadata('design:paramtypes', [parser_service_1.ParserService])
    ], GridViewCellComponent);
    return GridViewCellComponent;
}());
exports.GridViewCellComponent = GridViewCellComponent;
//# sourceMappingURL=gridview-cell.component.js.map