import { Component, Input, Compiler, ViewContainerRef, ViewChild, Injectable } from '@angular/core';
import { TemplateBuilder, TemplateComponent } from './template.builder';
import { GridView, DataColumn, FieldType } from './gridview';
import { GridViewComponent } from './gridview.component';


export interface IGridViewRowComponent {
    row: any;
    rowIndex: number;
    parentGridView: GridView;
    parentGridViewComponent: GridViewComponent;
}

@Injectable()
export class GridViewRowTemplateBuilder extends TemplateBuilder {
    protected createNewComponent(tmpl: string, styleUrls: Array<string> = []) {
        @Component({
            selector: 'gridview-row-template-component',
            template: tmpl,
            styleUrls: styleUrls,
            moduleId: module.id
        })
        class GridViewRowDynamicComponent implements IGridViewRowComponent {
            row: any;
            rowIndex: number;
            parentGridView: GridView;
            parentGridViewComponent: GridViewComponent;
        };
        return GridViewRowDynamicComponent;
    }
}

@Component({
    selector: 'gridview-rowtemplate',
    template: `<div #templatePlaceHolder></div>`
})
export class GridViewRowTemplateComponent extends TemplateComponent<IGridViewRowComponent> {
    @Input() row: any;
    @Input() rowIndex: number;
    @Input() parentGridView: GridView;
    @Input() parentGridViewComponent: GridViewComponent;

    constructor(protected compiler: Compiler, protected viewContainerRef: ViewContainerRef,
        protected templateBuilder: GridViewRowTemplateBuilder) {
        super(compiler, viewContainerRef, templateBuilder);
    }

    @ViewChild('templatePlaceHolder', { read: ViewContainerRef })
    protected get templatePlaceHolder(): ViewContainerRef {
        return this.dynamicComponentTarget;
    }
    protected set templatePlaceHolder(v: ViewContainerRef) {
        this.dynamicComponentTarget = v;
    }

    protected setComponent(from: GridViewRowTemplateComponent, to: IGridViewRowComponent) {
        to.row = from.row;
        to.rowIndex = from.rowIndex;
        to.parentGridView = from.parentGridView;
        to.parentGridViewComponent = from.parentGridViewComponent;
    }

    protected getImports(): Array<any> {
        let imports = super.getImports();
        if (this.parentGridView.rowTemplate && this.parentGridView.rowTemplate.imports) {
            imports = imports.concat(this.parentGridView.rowTemplate.imports);
        }
        return imports;
    }

    protected getDeclarations(): Array<any> {
        let declaration = super.getDeclarations();
        if (this.parentGridView.rowTemplate && this.parentGridView.rowTemplate.declarations) {
            declaration = declaration.concat(this.parentGridView.rowTemplate.declarations);
        }
        return declaration;
    }

    protected getStyleUrls(): Array<string> {
        let styleUrls = super.getStyleUrls();
        if (this.parentGridView.rowTemplate && this.parentGridView.rowTemplate.styleUrls)
            styleUrls = styleUrls.concat(this.parentGridView.rowTemplate.styleUrls);
        return styleUrls;
    }


    protected getInnerHtml() : string {
        return this.parentGridView.rowTemplate.template;
    }
}


