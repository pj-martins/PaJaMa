import { NgModule, Injectable, ViewChild, Component, ComponentFactory, Input, Compiler, ViewContainerRef, ComponentRef, Injector, OnInit, Type } from '@angular/core';
import { DataColumn, FilterMode, GridView } from './gridview';
import { GridViewComponent } from './gridview.component';
import { CheckListModule } from '../checklist/checklist.module';
//import { CheckList } from '../checklist/checklist';
import { TemplateBuilder, TemplateComponent } from './template.builder';

class GridViewFilterCellModel {
	column: DataColumn;
	parentGridView: GridView
	parentGridViewComponent: GridViewComponent;
	customProps: any;

	private _lastChange: Date;

	filterChanged() {
		if (this.column.filterDelayMilliseconds > 0) {
			this._lastChange = new Date();
			window.setTimeout(() => {
				let now = new Date();
				if (now.getTime() - this._lastChange.getTime() >= this.column.filterDelayMilliseconds - 1) {
					this.fireFilter();
				}
			}, this.column.filterDelayMilliseconds);
		}
		else {
			this.fireFilter();
		}
	}

	private fireFilter() {
		this.parentGridView.currentPage = 1;
		this.parentGridView.dataChanged.emit(this.parentGridView);
		this.parentGridViewComponent.filterChanged.emit(this.column);
	}
}
interface IGridViewFilterCellComponent {
	model: GridViewFilterCellModel;
}

@Injectable()
export class GridViewFilterCellTemplateBuilder extends TemplateBuilder {
	createNewComponent(tmpl: string, styleUrls: Array<string> = []) {
		@Component({
			selector: 'gridview-cell-template-component',
			template: tmpl,
			styleUrls: styleUrls,
			moduleId: module.id
		})
		class GridViewCellDynamicComponent implements IGridViewFilterCellComponent {
			model: GridViewFilterCellModel;
		};
		return GridViewCellDynamicComponent;
	}
}


@Component({
	moduleId: module.id,
	selector: 'gridview-filtercell',
	template: `<div #templatePlaceHolder></div>`,
	styleUrls: ['gridview.css']
})
export class GridViewFilterCellComponent extends TemplateComponent<IGridViewFilterCellComponent> {
	@Input() column: DataColumn;
	@Input() parentGridView: GridView
	@Input() parentGridViewComponent: GridViewComponent;

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

	protected setComponent(from: GridViewFilterCellComponent, to: IGridViewFilterCellComponent) {
		to.model = new GridViewFilterCellModel();
		to.model.column = from.column;
		to.model.parentGridView = from.parentGridView;
		to.model.parentGridViewComponent = from.parentGridViewComponent;


		//to.customProps = from.customProps;
		if (this.column.filterMode == FilterMode.DistinctList || this.column.filterMode == FilterMode.DynamicList || this.column.filterOptions) {
			to.model.customProps = {};
			to.model.customProps.checklistItems = this.column.filterOptions || [];
			let copy = [];
			if (to.model.customProps.checklistItems) {
				for (let ci of to.model.customProps.checklistItems) {
					copy.push(ci);
				}
			}
			to.model.column.filterValue = copy;
			this.column.filterOptionsChanged.subscribe(() => {
				to.model.customProps.checklistItems = this.column.filterOptions;
				let copy2 = [];
				if (to.model.customProps.checklistItems) {
					for (let ci of to.model.customProps.checklistItems) {
						copy2.push(ci);
					}
				}
				to.model.column.filterValue = copy2;
			});
			// let self = this;
			//to.model.customProps.selectionChanged = function() {
			//    to.model.column.filterValue = to.model.customProps.checklist.selectedItems;
			//    to.model.filterChanged();
			//}
		}
	}

	protected getImports(): Array<any> {
		let imports = super.getImports();
		if (this.column.filterTemplate && this.column.filterTemplate.imports) {
			imports = imports.concat(this.column.filterTemplate.imports);
		}

		if (this.column.filterMode == FilterMode.DistinctList || this.column.filterMode == FilterMode.DynamicList || this.column.filterOptions)
			imports = imports.concat(CheckListModule);

		return imports;
	}

	protected getDeclarations(): Array<any> {
		let declarations = super.getDeclarations();

		if (this.column.filterTemplate && this.column.filterTemplate.declarations) {
			declarations = declarations.concat(this.column.filterTemplate.declarations);
		}
		return declarations;
	}

	protected getStyleUrls(): Array<string> {
		let styleUrls = super.getStyleUrls();
		if (this.column.filterTemplate && this.column.filterTemplate.styleUrls)
			styleUrls = styleUrls.concat(this.column.filterTemplate.styleUrls);
		// styleUrls.push('gridview.css');
		return styleUrls;
	}

	protected getInnerHtml(): string {
		if (this.column.filterTemplate)
			return this.column.filterTemplate.template;

		let html = '';
		if (this.column.filterMode == FilterMode.DistinctList || this.column.filterMode == FilterMode.DynamicList || this.column.filterOptions)
			html = "<check-list name='filtcheck' [items]='model.customProps.checklistItems' [selectedItems]='model.column.filterValue' (selectionChanged)='model.filterChanged()'  class='form-control filter-control'></check-list>";
		else
			html = "<input type='text' [(ngModel)]='model.column.filterValue' (ngModelChange)='model.filterChanged()' class='form-control filter-control' />";
		return html;
	}
}