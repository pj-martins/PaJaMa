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
var GridViewRowTemplateBuilder = (function (_super) {
    __extends(GridViewRowTemplateBuilder, _super);
    function GridViewRowTemplateBuilder() {
        _super.apply(this, arguments);
    }
    GridViewRowTemplateBuilder.prototype.createNewComponent = function (tmpl, styleUrls) {
        if (styleUrls === void 0) { styleUrls = []; }
        var GridViewRowDynamicComponent = (function () {
            function GridViewRowDynamicComponent() {
            }
            GridViewRowDynamicComponent = __decorate([
                core_1.Component({
                    selector: 'gridview-row-template-component',
                    template: tmpl,
                    styleUrls: styleUrls,
                    moduleId: module.id
                }), 
                __metadata('design:paramtypes', [])
            ], GridViewRowDynamicComponent);
            return GridViewRowDynamicComponent;
        }());
        ;
        return GridViewRowDynamicComponent;
    };
    GridViewRowTemplateBuilder = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], GridViewRowTemplateBuilder);
    return GridViewRowTemplateBuilder;
}(template_builder_1.TemplateBuilder));
exports.GridViewRowTemplateBuilder = GridViewRowTemplateBuilder;
var GridViewRowTemplateComponent = (function (_super) {
    __extends(GridViewRowTemplateComponent, _super);
    function GridViewRowTemplateComponent(compiler, viewContainerRef, templateBuilder) {
        _super.call(this, compiler, viewContainerRef, templateBuilder);
        this.compiler = compiler;
        this.viewContainerRef = viewContainerRef;
        this.templateBuilder = templateBuilder;
    }
    Object.defineProperty(GridViewRowTemplateComponent.prototype, "templatePlaceHolder", {
        get: function () {
            return this.dynamicComponentTarget;
        },
        set: function (v) {
            this.dynamicComponentTarget = v;
        },
        enumerable: true,
        configurable: true
    });
    GridViewRowTemplateComponent.prototype.setComponent = function (from, to) {
        to.row = from.row;
        to.parentGridView = from.parentGridView;
        to.parentGridViewComponent = from.parentGridViewComponent;
    };
    GridViewRowTemplateComponent.prototype.getImports = function () {
        var imports = _super.prototype.getImports.call(this);
        if (this.parentGridView.rowTemplate && this.parentGridView.rowTemplate.imports) {
            imports = imports.concat(this.parentGridView.rowTemplate.imports);
        }
        return imports;
    };
    GridViewRowTemplateComponent.prototype.getDeclarations = function () {
        var declaration = _super.prototype.getDeclarations.call(this);
        if (this.parentGridView.rowTemplate && this.parentGridView.rowTemplate.declarations) {
            declaration = declaration.concat(this.parentGridView.rowTemplate.declarations);
        }
        return declaration;
    };
    GridViewRowTemplateComponent.prototype.getStyleUrls = function () {
        var styleUrls = _super.prototype.getStyleUrls.call(this);
        if (this.parentGridView.rowTemplate && this.parentGridView.rowTemplate.styleUrls)
            styleUrls = styleUrls.concat(this.parentGridView.rowTemplate.styleUrls);
        return styleUrls;
    };
    GridViewRowTemplateComponent.prototype.getInnerHtml = function () {
        return this.parentGridView.rowTemplate.template;
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], GridViewRowTemplateComponent.prototype, "row", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_1.GridView)
    ], GridViewRowTemplateComponent.prototype, "parentGridView", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', gridview_component_1.GridViewComponent)
    ], GridViewRowTemplateComponent.prototype, "parentGridViewComponent", void 0);
    __decorate([
        core_1.ViewChild('templatePlaceHolder', { read: core_1.ViewContainerRef }), 
        __metadata('design:type', core_1.ViewContainerRef)
    ], GridViewRowTemplateComponent.prototype, "templatePlaceHolder", null);
    GridViewRowTemplateComponent = __decorate([
        core_1.Component({
            selector: 'gridview-rowtemplate',
            template: "<div #templatePlaceHolder></div>"
        }), 
        __metadata('design:paramtypes', [core_1.Compiler, core_1.ViewContainerRef, GridViewRowTemplateBuilder])
    ], GridViewRowTemplateComponent);
    return GridViewRowTemplateComponent;
}(template_builder_1.TemplateComponent));
exports.GridViewRowTemplateComponent = GridViewRowTemplateComponent;
//# sourceMappingURL=gridview.rowtemplate.js.map