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
var common_1 = require('@angular/common');
var forms_1 = require('@angular/forms');
var TemplateBuilder = (function () {
    function TemplateBuilder() {
        // this object is singleton - so we can use this as a cache
        this._cacheOfTypes = {};
        this._cacheOfModules = {};
    }
    TemplateBuilder.prototype.createComponentAndModule = function (template, imports, declarations, styleUrls) {
        if (imports === void 0) { imports = []; }
        if (declarations === void 0) { declarations = []; }
        if (styleUrls === void 0) { styleUrls = []; }
        var module;
        var type = this._cacheOfTypes[template];
        if (type) {
            module = this._cacheOfModules[template];
            console.log("Module and Type are returned from cache");
            return { type: type, module: module };
        }
        // unknown template ... let's create a Type for it
        type = this.createNewComponent(template, styleUrls);
        module = this.createComponentModule(type, imports, declarations);
        // cache that type and module - because the only difference would be "template"
        this._cacheOfTypes[template] = type;
        this._cacheOfModules[template] = module;
        return { type: type, module: module };
    };
    TemplateBuilder.prototype.createComponentModule = function (componentType, imports, declarations) {
        if (imports === void 0) { imports = []; }
        if (declarations === void 0) { declarations = []; }
        imports.push(common_1.CommonModule);
        imports.push(forms_1.FormsModule);
        declarations.push(componentType);
        var RuntimeComponentModule = (function () {
            function RuntimeComponentModule() {
            }
            RuntimeComponentModule = __decorate([
                core_1.NgModule({
                    imports: imports,
                    declarations: declarations,
                }), 
                __metadata('design:paramtypes', [])
            ], RuntimeComponentModule);
            return RuntimeComponentModule;
        }());
        // a module for just this Type
        return RuntimeComponentModule;
    };
    return TemplateBuilder;
}());
exports.TemplateBuilder = TemplateBuilder;
var TemplateComponent = (function () {
    function TemplateComponent(compiler, viewContainerRef, templateBuilder) {
        this.compiler = compiler;
        this.viewContainerRef = viewContainerRef;
        this.templateBuilder = templateBuilder;
    }
    TemplateComponent.prototype.getImports = function () {
        return [];
    };
    TemplateComponent.prototype.getDeclarations = function () {
        return [];
    };
    TemplateComponent.prototype.getStyleUrls = function () {
        return [];
    };
    TemplateComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        if (this.componentRef) {
            this.componentRef.destroy();
        }
        var template = this.getInnerHtml();
        var result = this.templateBuilder.createComponentAndModule(template, this.getImports(), this.getDeclarations(), this.getStyleUrls());
        var componentType = result.type;
        var runtimeModule = result.module;
        // compile module
        this.compiler
            .compileModuleAndAllComponentsAsync(runtimeModule)
            .then(function (moduleWithFactories) {
            var factory;
            for (var _i = 0, _a = moduleWithFactories.componentFactories; _i < _a.length; _i++) {
                var fact = _a[_i];
                if (fact.componentType.name == componentType.name) {
                    factory = fact;
                    break;
                }
            }
            // Target will instantiate and inject component (we'll keep reference to it)
            _this.componentRef = _this
                .dynamicComponentTarget
                .createComponent(factory);
            var component = _this.componentRef.instance;
            _this.setComponent(_this, component);
        });
    };
    return TemplateComponent;
}());
exports.TemplateComponent = TemplateComponent;
//# sourceMappingURL=template.builder.js.map