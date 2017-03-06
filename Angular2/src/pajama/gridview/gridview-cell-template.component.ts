import { Component, Input, Compiler, ViewContainerRef, ViewChild, Injectable } from '@angular/core';
import { TemplateBuilder, TemplateComponent } from './template.builder';
import { GridView, DataColumn, FieldType } from './gridview';
import { GridViewComponent } from './gridview.component';
import { PipesModule } from '../pipes/pipes.module';


export interface IGridViewCellTemplateComponent {
	column: DataColumn;
	row: any;
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
		class GridViewCellDynamicComponent implements IGridViewCellTemplateComponent {
			column: DataColumn;
			row: any;
			parentGridViewComponent: GridViewComponent;
			parentGridView: GridView;
		};
		return GridViewCellDynamicComponent;
	}
}

@Component({
	selector: 'gridview-cell-template',
	template: `<div #templatePlaceHolder></div>`
})
export class GridViewCellTemplateComponent extends TemplateComponent<IGridViewCellTemplateComponent> {
	@Input() column: DataColumn;
	@Input() row: any;

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

	protected setComponent(from: GridViewCellTemplateComponent, to: IGridViewCellTemplateComponent) {
		to.row = from.row;
		to.column = from.column;
		to.parentGridView = from.parentGridView;
		to.parentGridViewComponent = from.parentGridViewComponent;
	}

	protected getImports(): Array<any> {
		let imports = super.getImports();
		if (this.column.template && this.column.template.imports) {
			imports = imports.concat(this.column.template.imports);
		}

		if (this.column.fieldType == FieldType.Date) {
			imports = imports.concat(PipesModule);
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

	// TODO: reworked grid so only time this component *should* be used is when the column has a template associated
	protected getInnerHtml(): string {
		return this.column.template.template;
	}
}