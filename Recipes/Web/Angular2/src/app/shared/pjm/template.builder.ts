import { NgModule, AfterViewInit, Compiler, ComponentFactory, ComponentRef, Injectable, ViewContainerRef } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

export abstract class TemplateBuilder {

    // this object is singleton - so we can use this as a cache
    private _cacheOfTypes: { [templateKey: string]: any } = {};
    private _cacheOfModules: { [templateKey: string]: any } = {};

    public createComponentAndModule(template: string, imports: Array<any> = [], declarations: Array<any> = [], styleUrls: Array<string> = []): { type: any, module: any } {
        let module: any;
        let type = this._cacheOfTypes[template];

        if (type) {
            module = this._cacheOfModules[template];
            console.log("Module and Type are returned from cache")
            return { type: type, module: module };
        }

        // unknown template ... let's create a Type for it
        type = this.createNewComponent(template, styleUrls);
        module = this.createComponentModule(type, imports, declarations);

        // cache that type and module - because the only difference would be "template"
        this._cacheOfTypes[template] = type;
        this._cacheOfModules[template] = module;

        return { type: type, module: module };
    }

    protected abstract createNewComponent(tmpl: string, styleUrls: Array<string>): void;

    protected createComponentModule(componentType: any, imports: Array<any> = [], declarations: Array<any> = []) {
        imports.push(BrowserModule);
        imports.push(FormsModule);
        declarations.push(componentType);
        @NgModule({
            imports: imports,
            declarations: declarations,
        })
        class RuntimeComponentModule {
        }
        // a module for just this Type
        return RuntimeComponentModule;
    }
}

export abstract class TemplateComponent<TComponent> implements AfterViewInit {

    constructor(protected compiler: Compiler, protected viewContainerRef: ViewContainerRef,
        protected templateBuilder: TemplateBuilder) {

    }

    protected dynamicComponentTarget: ViewContainerRef;
    protected componentRef: ComponentRef<TComponent>;

    protected abstract getInnerHtml(): string;
    protected getImports(): Array<any> {
        return [];
    }

    protected getDeclarations(): Array<any> {
        return [];
    }

    protected getStyleUrls(): Array<string> {
        return [];
    }

    protected abstract setComponent(from: TemplateComponent<TComponent>, to: TComponent): void;

    ngAfterViewInit() {
        if (this.componentRef) {
            this.componentRef.destroy();
        }

        var template = this.getInnerHtml();
        var result = this.templateBuilder.createComponentAndModule(template, this.getImports(), this.getDeclarations(), this.getStyleUrls());

        var componentType = result.type;
        var runtimeModule = result.module

        // compile module
        this.compiler
            .compileModuleAndAllComponentsAsync(runtimeModule)
            .then((moduleWithFactories) => {

                let factory: ComponentFactory<any>;
                for (let fact of moduleWithFactories.componentFactories) {
                    if (fact.componentType.name == componentType.name) {
                        factory = fact;
                        break;
                    }
                }

                // Target will instantiate and inject component (we'll keep reference to it)
                this.componentRef = this
                    .dynamicComponentTarget
                    .createComponent(factory);

                let component = this.componentRef.instance;
                this.setComponent(this, component);
            });
    }
}
