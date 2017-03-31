import { Component, OnInit } from '@angular/core';
import { GridView, DataColumn, FilterMode, FieldType, ColumnSortDirection, IGridViewFilterCellTemplateComponent, IGridViewFilterCellComponent, IGridViewCellTemplateComponent, IGridViewComponent } from '../../pajama/gridview/gridview';

@Component({
	moduleId: module.id,
	selector: 'coordinator-filter-cell',
	template: `
<input typeahead [(ngModel)]='column.filterValue' [dataSource]='column.customProps.coordinators'
	(itemSelected)="parentFilterCellComponent.filterChanged()"
    (ngModelChange)="parentFilterCellComponent.filterChanged()" />
`
})
export class CoordinatorFilterCellTemplateComponent implements IGridViewFilterCellTemplateComponent {
	column: DataColumn;
	parentGridView: GridView;
	parentFilterCellComponent: IGridViewFilterCellComponent;
}

@Component({
	moduleId: module.id,
	selector: 'customer-cell',
	template: `
<strong>{{row.customer.customerName}}</strong>
`
})
export class CustomerCellTemplateComponent implements IGridViewCellTemplateComponent {
	column: DataColumn;
	parentGridView: GridView;
	parentGridViewComponent: IGridViewComponent;
	row: any;
}