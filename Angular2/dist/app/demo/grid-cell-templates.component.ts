import { Component, OnInit } from '@angular/core';
import { GridView, DataColumn, FilterMode, FieldType } from 'pajama/gridview/gridview';
import { GridViewCellTemplateComponent, GridViewFilterCellTemplateComponent } from 'pajama/gridview/gridview-templates.component';

@Component({
	moduleId: module.id,
	selector: 'coordinator-filter-cell',
	template: `
<input typeahead [(ngModel)]='column.filterValue' [dataSource]='column.customProps.coordinators'
	(itemSelected)="parentFilterCellComponent.filterChanged()"
    (ngModelChange)="parentFilterCellComponent.filterChanged()" />
`
})
export class CoordinatorFilterCellTemplateComponent extends GridViewFilterCellTemplateComponent { }

@Component({
	moduleId: module.id,
	selector: 'event-type-filter-cell',
	template: `
<input type="text" [multiTextbox]="column.filterValue" style="width:40%" (itemsChanged)="parentFilterCellComponent.filterChanged()" />
`
})
export class EventTypeFilterCellTemplateComponent extends GridViewFilterCellTemplateComponent { }

@Component({
	moduleId: module.id,
	selector: 'requested-by-filter-cell',
	template: `
<input [multiTypeahead]='column.filterValue' (itemsChanged)="parentFilterCellComponent.filterChanged()" [dataSource]='column.filterOptions' />
`
})
export class RequestedByFilterCellTemplateComponent extends GridViewFilterCellTemplateComponent { }

@Component({
	moduleId: module.id,
	selector: 'customer-cell',
	template: `
<strong>{{row.customer.customerName}}</strong>
`
})
export class CustomerCellTemplateComponent extends GridViewCellTemplateComponent { }