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
var template_builder_1 = require('./template.builder');
var gridview_1 = require('./gridview');
var gridview_component_1 = require('./gridview.component');
var pipes_module_1 = require('../pipes/pipes.module');
var GridViewCellTemplateBuilder = (function (_super) {
    __extends(GridViewCellTemplateBuilder, _super);
    function GridViewCellTemplateBuilder() {
        _super.apply(this, arguments);
    }
    GridViewCellTemplateBuilder.prototype.createNewComponent = function (tmpl, styleUrls) {
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
    GridViewCellTemplateBuilder = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], GridViewCellTemplateBuilder);
    return GridViewCellTemplateBuilder;
}(template_builder_1.TemplateBuilder));
exports.GridViewCellTemplateBuilder = GridViewCellTemplateBuilder;
var GridViewCellTemplateComponent = (function (_super) {
    __extends(GridViewCellTemplateComponent, _super);
    function GridViewCellTemplateComponent(compiler, viewContainerRef, templateBuilder) {
        _super.call(this, compiler, viewContainerRef, templateBuilder);
        this.compiler = compiler;
        this.viewContainerRef = viewContainerRef;
        this.templateBuilder = templateBuilder;
    }
    Object.defineProperty(GridViewCellTemplateComponent.prototype, "templatePlaceHolder", {
        get: function () {
            return this.dynamicComponentTarget;
        },
        set: function (v) {
            this.dynamicComponentTarget = v;
        },
        enumerable: true,
        configurable: true
    });
    GridViewCellTemplateComponent.prototype.setComponent = function (from, to) {
        to.row = from.row;
        to.column = from.column;
        to.parentGridView = from.parentGridView;
        to.parentGridViewComponent = from.parentGridViewComponent;
    };
    GridViewCellTemplateComponent.prototype.getImports = function () {
        var imports = _super.prototype.getImports.call(this);
        if (this.column.template && this.column.template.imports) {
            imports = imports.concat(this.column.template.imports);
        }
        if (this.column.fieldType == gridview_1.FieldType.Date) {
            imports = imports.concat(pipes_module_1.PipesModule);
        }
        return imports;
    };
    GridViewCellTemplateComponent.prototype.getDeclarations = function () {
        var declaration = _super.prototype.getDeclarations.call(this);
        if (this.column.template && this.column.template.declarations) {
            declaration = declaration.concat(this.column.template.declarations);
        }
        return declaration;
    };
    GridViewCellTemplateComponent.prototype.getStyleUrls = function () {
        var styleUrls = _super.prototype.getStyleUrls.call(this);
        if (this.column.template && this.column.template.styleUrls)
            styleUrls = styleUrls.concat(this.column.template.styleUrls);
        return styleUrls;
    };
    // TODO: reworked grid so only time this component *should* be used is when the column has a template associated
    GridViewCellTemplateComponent.prototype.getInnerHtml = function () {
        return this.column.template.template;
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.DataColumn)
    ], GridViewCellTemplateComponent.prototype, "column", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], GridViewCellTemplateComponent.prototype, "row", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_component_1.GridViewComponent)
    ], GridViewCellTemplateComponent.prototype, "parentGridViewComponent", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.GridView)
    ], GridViewCellTemplateComponent.prototype, "parentGridView", void 0);
    __decorate([
        core_1.ViewChild('templatePlaceHolder', { read: core_1.ViewContainerRef }), 
        __metadata('design:type', core_1.ViewContainerRef)
    ], GridViewCellTemplateComponent.prototype, "templatePlaceHolder", null);
    GridViewCellTemplateComponent = __decorate([
        core_1.Component({
            selector: 'gridview-cell-template',
            template: "<div #templatePlaceHolder></div>"
        }), 
        __metadata('design:paramtypes', [core_1.Compiler, core_1.ViewContainerRef, GridViewCellTemplateBuilder])
    ], GridViewCellTemplateComponent);
    return GridViewCellTemplateComponent;
}(template_builder_1.TemplateComponent));
exports.GridViewCellTemplateComponent = GridViewCellTemplateComponent;
//# sourceMappingURL=gridview-cell-template.component.js.map