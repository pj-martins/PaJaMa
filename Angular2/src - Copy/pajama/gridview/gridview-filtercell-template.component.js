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
var template_builder_1 = require("./template.builder");
var gridview_filtercell_component_1 = require("./gridview-filtercell.component");
var gridview_1 = require("./gridview");
var GridViewFilterCellTemplateBuilder = (function (_super) {
    __extends(GridViewFilterCellTemplateBuilder, _super);
    function GridViewFilterCellTemplateBuilder() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    GridViewFilterCellTemplateBuilder.prototype.createNewComponent = function (tmpl, styleUrls) {
        if (styleUrls === void 0) { styleUrls = []; }
        var GridViewFilterCellDynamicComponent = (function () {
            function GridViewFilterCellDynamicComponent() {
            }
            return GridViewFilterCellDynamicComponent;
        }());
        GridViewFilterCellDynamicComponent = __decorate([
            core_1.Component({
                selector: 'gridview-filtercell-template-component',
                template: tmpl,
                styleUrls: styleUrls,
                moduleId: module.id
            })
        ], GridViewFilterCellDynamicComponent);
        ;
        return GridViewFilterCellDynamicComponent;
    };
    return GridViewFilterCellTemplateBuilder;
}(template_builder_1.TemplateBuilder));
GridViewFilterCellTemplateBuilder = __decorate([
    core_1.Injectable()
], GridViewFilterCellTemplateBuilder);
exports.GridViewFilterCellTemplateBuilder = GridViewFilterCellTemplateBuilder;
var GridViewFilterCellTemplateComponent = (function (_super) {
    __extends(GridViewFilterCellTemplateComponent, _super);
    function GridViewFilterCellTemplateComponent(compiler, viewContainerRef, templateBuilder) {
        var _this = _super.call(this, compiler, viewContainerRef, templateBuilder) || this;
        _this.compiler = compiler;
        _this.viewContainerRef = viewContainerRef;
        _this.templateBuilder = templateBuilder;
        return _this;
    }
    Object.defineProperty(GridViewFilterCellTemplateComponent.prototype, "templatePlaceHolder", {
        get: function () {
            return this.dynamicComponentTarget;
        },
        set: function (v) {
            this.dynamicComponentTarget = v;
        },
        enumerable: true,
        configurable: true
    });
    GridViewFilterCellTemplateComponent.prototype.setComponent = function (from, to) {
        to.parentGridViewFilterCellComponent = from.parentGridViewFilterCellComponent;
        to.column = from.column;
    };
    GridViewFilterCellTemplateComponent.prototype.getImports = function () {
        var imports = _super.prototype.getImports.call(this);
        if (this.column.filterTemplate && this.column.filterTemplate.imports) {
            imports = imports.concat(this.column.filterTemplate.imports);
        }
        return imports;
    };
    GridViewFilterCellTemplateComponent.prototype.getDeclarations = function () {
        var declaration = _super.prototype.getDeclarations.call(this);
        if (this.column.filterTemplate && this.column.filterTemplate.declarations) {
            declaration = declaration.concat(this.column.filterTemplate.declarations);
        }
        return declaration;
    };
    GridViewFilterCellTemplateComponent.prototype.getStyleUrls = function () {
        var styleUrls = _super.prototype.getStyleUrls.call(this);
        if (this.column.filterTemplate && this.column.filterTemplate.styleUrls)
            styleUrls = styleUrls.concat(this.column.filterTemplate.styleUrls);
        return styleUrls;
    };
    GridViewFilterCellTemplateComponent.prototype.getInnerHtml = function () {
        return this.column.filterTemplate.template;
    };
    return GridViewFilterCellTemplateComponent;
}(template_builder_1.TemplateComponent));
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_filtercell_component_1.GridViewFilterCellComponent)
], GridViewFilterCellTemplateComponent.prototype, "parentGridViewFilterCellComponent", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_1.DataColumn)
], GridViewFilterCellTemplateComponent.prototype, "column", void 0);
__decorate([
    core_1.ViewChild('templatePlaceHolder', { read: core_1.ViewContainerRef }),
    __metadata("design:type", core_1.ViewContainerRef),
    __metadata("design:paramtypes", [core_1.ViewContainerRef])
], GridViewFilterCellTemplateComponent.prototype, "templatePlaceHolder", null);
GridViewFilterCellTemplateComponent = __decorate([
    core_1.Component({
        selector: 'gridview-filtercell-template',
        template: "<div #templatePlaceHolder></div>"
    }),
    __metadata("design:paramtypes", [core_1.Compiler, core_1.ViewContainerRef,
        GridViewFilterCellTemplateBuilder])
], GridViewFilterCellTemplateComponent);
exports.GridViewFilterCellTemplateComponent = GridViewFilterCellTemplateComponent;
//# sourceMappingURL=gridview-filtercell-template.component.js.map