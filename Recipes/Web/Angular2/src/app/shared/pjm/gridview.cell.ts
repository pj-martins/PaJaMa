import { Component, Input, Compiler, ViewContainerRef, ViewChild, Injectable } from '@angular/core';
import { TemplateBuilder, TemplateComponent } from './template.builder';
import { GridView, DataColumn, FieldType } from './gridview';
import { GridViewComponent } from './gridview.component';


interface IGridViewCellComponent {
	column: DataColumn;
    row: any;
    rowIndex: number;
    parentGridViewComponent: GridViewComponent;
    parentGridView: GridView;
}

@Injectable()
export class GridViewCellTemplateBuilder extends TemplateBuilder {
    createNewComponent(tmpl: string, styleUrls: Array<string> = []) {
        @Component({
            selector: 'gridview-cell-template-component',
            template: tmpl,
            styleUrls: styleUrls,
            moduleId: module.id
        })
        class GridViewCellDynamicComponent implements IGridViewCellComponent {
			column: DataColumn;
            row: any;
            rowIndex: number;
            parentGridViewComponent: GridViewComponent;
            parentGridView: GridView;
        };
        return GridViewCellDynamicComponent;
    }
}

@Component({
    selector: 'gridview-cell',
    template: `<div #templatePlaceHolder></div>`
})
export class GridViewCellComponent extends TemplateComponent<IGridViewCellComponent> {
    @Input() column: DataColumn;
    @Input() row: any;
    @Input() rowIndex: number;
    @Input() parentGridViewComponent: GridViewComponent;
    @Input() parentGridView: GridView;
    
    constructor(protected compiler: Compiler, protected viewContainerRef: ViewContainerRef,
        protected templateBuilder: GridViewCellTemplateBuilder) {
        super(compiler, viewContainerRef, templateBuilder);
    }

    @ViewChild('templatePlaceHolder', { read: ViewContainerRef })
    protected get templatePlaceHolder(): ViewContainerRef {
        return this.dynamicComponentTarget;
    }
    protected set templatePlaceHolder(v: ViewContainerRef) {
        this.dynamicComponentTarget = v;
    }

    protected setComponent(from: GridViewCellComponent, to: IGridViewCellComponent) {
        to.row = from.row;
        to.column = from.column;
        to.rowIndex = from.rowIndex;
        to.parentGridView = from.parentGridView;
        to.parentGridViewComponent = from.parentGridViewComponent;
    }

    protected getImports(): Array<any> {
        let imports = super.getImports();
        if (this.column.template && this.column.template.imports) {
            imports = imports.concat(this.column.template.imports);
        }
        return imports;
    }

    protected getDeclarations(): Array<any> {
        let declaration = super.getDeclarations();
        if (this.column.template && this.column.template.declarations) {
            declaration = declaration.concat(this.column.template.declarations);
        }
        return declaration;
    }

    protected getStyleUrls(): Array<string> {
        let styleUrls = super.getStyleUrls();
        if (this.column.template && this.column.template.styleUrls)
            styleUrls = styleUrls.concat(this.column.template.styleUrls);
        return styleUrls;
    }

    protected getInnerHtml() : string {
        let html = '';
        if (this.column.fieldType == FieldType.Boolean) {
            html = `<div [ngClass]="{ 'glyphicon glyphicon-ok' : row.${this.column.fieldName} == true }"></div>`;
        }
        else {
            if (this.column.template)
                return this.column.template.template;

            let innerText = `row.${this.column.fieldName}`;

            if (this.column.fieldType == FieldType.Html) {
                return `<div [innerHTML]="${innerText}"></div>`;
            }


            if (this.column.format)
                innerText += " | " + this.column.format;
            else if (this.column.fieldType == FieldType.Date)
                innerText += " | date:'MM/dd/yyyy'";
            html = `<div [innerText]="${innerText}"></div>`;
        }

        return html;
    }
}