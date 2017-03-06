"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var checklist_module_1 = require('../checklist/checklist.module');
//import { CheckList } from '../checklist/checklist';
var template_builder_1 = require('./template.builder');
var GridViewFilterCellModel = (function () {
    function GridViewFilterCellModel() {
    }
    GridViewFilterCellModel.prototype.filterChanged = function () {
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
    GridViewFilterCellModel.prototype.fireFilter = function () {
        this.parentGridView.currentPage = 1;
        this.parentGridView.dataChanged.emit(this.parentGridView);
        this.parentGridViewComponent.filterChanged.emit(this.column);
    };
    return GridViewFilterCellModel;
}());
var GridViewFilterCellTemplateBuilder = (function (_super) {
    __extends(GridViewFilterCellTemplateBuilder, _super);
    function GridViewFilterCellTemplateBuilder() {
        _super.apply(this, arguments);
    }
    GridViewFilterCellTemplateBuilder.prototype.createNewComponent = function (tmpl, styleUrls) {
        if (styleUrls === void 0) { styleUrls = []; }
        var GridViewCellDynamicComponent = (function () {
            function GridViewCellDynamicComponent() {
            }
            GridViewCellDynamicComponent = __decorate([
                core_1.Component({
                    selector: 'gridview-cell-template-component',
                    template: tmpl,
                    styleUrls: styleUrls,
                    moduleId: module.id
                }), 
                __metadata('design:paramtypes', [])
            ], GridViewCellDynamicComponent);
            return GridViewCellDynamicComponent;
        }());
        ;
        return GridViewCellDynamicComponent;
    };
    GridViewFilterCellTemplateBuilder = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], GridViewFilterCellTemplateBuilder);
    return GridViewFilterCellTemplateBuilder;
}(template_builder_1.TemplateBuilder));
exports.GridViewFilterCellTemplateBuilder = GridViewFilterCellTemplateBuilder;
var GridViewFilterCellComponent = (function (_super) {
    __extends(GridViewFilterCellComponent, _super);
    function GridViewFilterCellComponent(compiler, viewContainerRef, templateBuilder) {
        _super.call(this, compiler, viewContainerRef, templateBuilder);
        this.compiler = compiler;
        this.viewContainerRef = viewContainerRef;
        this.templateBuilder = templateBuilder;
    }
    Object.defineProperty(GridViewFilterCellComponent.prototype, "templatePlaceHolder", {
        get: function () {
            return this.dynamicComponentTarget;
        },
        set: function (v) {
            this.dynamicComponentTarget = v;
        },
        enumerable: true,
        configurable: true
    });
    GridViewFilterCellComponent.prototype.setComponent = function (from, to) {
        var _this = this;
        to.model = new GridViewFilterCellModel();
        to.model.column = from.column;
        to.model.parentGridView = from.parentGridView;
        to.model.parentGridViewComponent = from.parentGridViewComponent;
        //to.customProps = from.customProps;
        if (this.column.filterMode == gridview_1.FilterMode.DistinctList || this.column.filterMode == gridview_1.FilterMode.DynamicList || this.column.filterOptions) {
            to.model.customProps = {};
            to.model.customProps.checklistItems = this.column.filterOptions || [];
            var copy = [];
            if (to.model.customProps.checklistItems) {
                for (var _i = 0, _a = to.model.customProps.checklistItems; _i < _a.length; _i++) {
                    var ci = _a[_i];
                    copy.push(ci);
                }
            }
            to.model.column.filterValue = copy;
            this.column.filterOptionsChanged.subscribe(function () {
                to.model.customProps.checklistItems = _this.column.filterOptions;
                var copy2 = [];
                if (to.model.customProps.checklistItems) {
                    for (var _i = 0, _a = to.model.customProps.checklistItems; _i < _a.length; _i++) {
                        var ci = _a[_i];
                        copy2.push(ci);
                    }
                }
                to.model.column.filterValue = copy2;
            });
        }
    };
    GridViewFilterCellComponent.prototype.getImports = function () {
        var imports = _super.prototype.getImports.call(this);
        if (this.column.filterTemplate && this.column.filterTemplate.imports) {
            imports = imports.concat(this.column.filterTemplate.imports);
        }
        if (this.column.filterMode == gridview_1.FilterMode.DistinctList || this.column.filterMode == gridview_1.FilterMode.DynamicList || this.column.filterOptions)
            imports = imports.concat(checklist_module_1.CheckListModule);
        return imports;
    };
    GridViewFilterCellComponent.prototype.getDeclarations = function () {
        var declarations = _super.prototype.getDeclarations.call(this);
        if (this.column.filterTemplate && this.column.filterTemplate.declarations) {
            declarations = declarations.concat(this.column.filterTemplate.declarations);
        }
        return declarations;
    };
    GridViewFilterCellComponent.prototype.getStyleUrls = function () {
        var styleUrls = _super.prototype.getStyleUrls.call(this);
        if (this.column.filterTemplate && this.column.filterTemplate.styleUrls)
            styleUrls = styleUrls.concat(this.column.filterTemplate.styleUrls);
        // styleUrls.push('gridview.css');
        return styleUrls;
    };
    GridViewFilterCellComponent.prototype.getInnerHtml = function () {
        if (this.column.filterTemplate)
            return this.column.filterTemplate.template;
        var html = '';
        if (this.column.filterMode == gridview_1.FilterMode.DistinctList || this.column.filterMode == gridview_1.FilterMode.DynamicList || this.column.filterOptions)
            html = "<check-list name='filtcheck' [items]='model.customProps.checklistItems' [selectedItems]='model.column.filterValue' (selectionChanged)='model.filterChanged()'  class='form-control filter-control'></check-list>";
        else
            html = "<input type='text' [(ngModel)]='model.column.filterValue' (ngModelChange)='model.filterChanged()' class='form-control filter-control' />";
        return html;
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
    __decorate([
        core_1.ViewChild('templatePlaceHolder', { read: core_1.ViewContainerRef }), 
        __metadata('design:type', core_1.ViewContainerRef)
    ], GridViewFilterCellComponent.prototype, "templatePlaceHolder", null);
    GridViewFilterCellComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'gridview-filtercell',
            template: "<div #templatePlaceHolder></div>",
            styleUrls: ['gridview.css']
        }), 
        __metadata('design:paramtypes', [core_1.Compiler, core_1.ViewContainerRef, GridViewFilterCellTemplateBuilder])
    ], GridViewFilterCellComponent);
    return GridViewFilterCellComponent;
}(template_builder_1.TemplateComponent));
exports.GridViewFilterCellComponent = GridViewFilterCellComponent;
//# sourceMappingURL=gridview.filtercell.js.map