import { Component, Input, Compiler, ViewContainerRef, ViewChild, Injectable } from '@angular/core';
import { TemplateBuilder, TemplateComponent } from './template.builder';
import { GridViewFilterCellComponent } from './gridview-filtercell.component';
import { DataColumn } from './gridview';
import { PipesModule } from '../pipes/pipes.module';


export interface IGridViewFilterCellTemplateComponent {
	parentGridViewFilterCellComponent: GridViewFilterCellComponent;
	column: DataColumn;
}

@Injectable()
export class GridViewFilterCellTemplateBuilder extends TemplateBuilder {
	createNewComponent(tmpl: string, styleUrls: Array<string> = []) {
		@Component({
			selector: 'gridview-filtercell-template-component',
			template: tmpl,
			styleUrls: styleUrls,
			moduleId: module.id
		})
		class GridViewFilterCellDynamicComponent implements IGridViewFilterCellTemplateComponent {
			parentGridViewFilterCellComponent: GridViewFilterCellComponent;
			column: DataColumn;
		};
		return GridViewFilterCellDynamicComponent;
	}
}

@Component({
	selector: 'gridview-filtercell-template',
	template: `<div #templatePlaceHolder></div>`
})
export class GridViewFilterCellTemplateComponent extends TemplateComponent<IGridViewFilterCellTemplateComponent> {
	@Input() parentGridViewFilterCellComponent: GridViewFilterCellComponent;
	@Input() column: DataColumn;

	constructor(protected compiler: Compiler, protected viewContainerRef: ViewContainerRef,
		protected templateBuilder: GridViewFilterCellTemplateBuilder) {
		super(compiler, viewContainerRef, templateBuilder);
	}

	@ViewChild('templatePlaceHolder', { read: ViewContainerRef })
	protected get templatePlaceHolder(): ViewContainerRef {
		return this.dynamicComponentTarget;
	}
	protected set templatePlaceHolder(v: ViewContainerRef) {
		this.dynamicComponentTarget = v;
	}

	protected setComponent(from: GridViewFilterCellTemplateComponent, to: IGridViewFilterCellTemplateComponent) {
		to.parentGridViewFilterCellComponent = from.parentGridViewFilterCellComponent;
		to.column = from.column;
	}

	protected getImports(): Array<any> {
		let imports = super.getImports();
		if (this.column.filterTemplate && this.column.filterTemplate.imports) {
			imports = imports.concat(this.column.filterTemplate.imports);
		}
		
		return imports;
	}

	protected getDeclarations(): Array<any> {
		let declaration = super.getDeclarations();
		if (this.column.filterTemplate && this.column.filterTemplate.declarations) {
			declaration = declaration.concat(this.column.filterTemplate.declarations);
		}

		return declaration;
	}

	protected getStyleUrls(): Array<string> {
		let styleUrls = super.getStyleUrls();
		if (this.column.filterTemplate && this.column.filterTemplate.styleUrls)
			styleUrls = styleUrls.concat(this.column.filterTemplate.styleUrls);
		return styleUrls;
	}

	protected getInnerHtml(): string {
		return this.column.filterTemplate.template;
	}
}