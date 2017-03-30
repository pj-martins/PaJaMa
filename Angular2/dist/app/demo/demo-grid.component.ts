import { Component, OnInit } from '@angular/core';
import { GridView, DataColumn, FilterMode, FieldType, SortDirection, GridViewTemplate } from 'pajama/gridview/gridview';
import { TypeaheadModule } from 'pajama/typeahead/typeahead.module';
import { MultiTextboxModule } from 'pajama/multi-textbox/multi-textbox.module';
import { Event } from '../classes/classes';

declare var EVENTS: Array<Event>;

@Component({
	moduleId: module.id,
	selector: 'demo-grid',
	template: `
<gridview [grid]='gridDemo' (pageChanged)='pageChanged()'></gridview>
`
})
export class DemoGridComponent implements OnInit {
	protected gridDemo: GridView;
	private _coordinatorColumn: DataColumn;

	constructor() {
		this.initGrid();
	}

	ngOnInit() {
		this.gridDemo.data = EVENTS;
		this._coordinatorColumn.customProps["coordinators"] = this.gridDemo.getDistinctValues(this._coordinatorColumn);
	}

	private initGrid() {
		this.gridDemo = new GridView();
		this.gridDemo.filterVisible = true;
		this.gridDemo.allowColumnOrdering = true;
		this.gridDemo.saveGridStateToStorage = true;
		this.gridDemo.allowColumnCustomization = true;
		this.gridDemo.name = "gridDemo";

		let custCol = new DataColumn("customer.customerName");
		custCol.filterMode = FilterMode.DistinctList;
		custCol.sortable = true;
		custCol.allowSizing = true;
		this.gridDemo.columns.push(custCol);

		let startCol = new DataColumn("eventStartDT", "Start");
		startCol.fieldType = FieldType.Date;
		startCol.sortable = true;
		startCol.sortDirection = SortDirection.Desc;
		startCol.width = "110px";
		this.gridDemo.columns.push(startCol);

		let endCol = new DataColumn("eventEndDT", "End");
		endCol.fieldType = FieldType.Date;
		endCol.width = "110px";
		this.gridDemo.columns.push(endCol);

		this._coordinatorColumn = new DataColumn("coordinator");

		this._coordinatorColumn.filterMode = FilterMode.Equals;
		this._coordinatorColumn.filterTemplate = new GridViewTemplate(`
<typeahead [(ngModel)]='column.filterValue' [dataSource]='column.customProps.coordinators'
	 (itemSelected)="parentGridViewFilterCellComponent.filterChanged()"
	 (ngModelChange)="parentGridViewFilterCellComponent.filterChanged()">
</typeahead>`, [TypeaheadModule]);
		this._coordinatorColumn.sortable = true;
		this._coordinatorColumn.allowSizing = true;
		this.gridDemo.columns.push(this._coordinatorColumn);

		let phoneNumberCol = new DataColumn("phoneNumber");
		phoneNumberCol.width = "160px";
		phoneNumberCol.filterMode = FilterMode.Contains;
		this.gridDemo.columns.push(phoneNumberCol);

		let evtTypeCol = new DataColumn("hallEventType.eventTypeName", "Event Type");
		evtTypeCol.filterMode = FilterMode.DynamicList;
		evtTypeCol.filterValue = [];
		evtTypeCol.filterTemplate = new GridViewTemplate(`
<multi-textbox [items]='column.filterValue' (itemsChanged)="parentGridViewFilterCellComponent.filterChanged()"></multi-textbox>
`, [MultiTextboxModule]);
		evtTypeCol.allowSizing = true;
		this.gridDemo.columns.push(evtTypeCol);

		let requestedByCol = new DataColumn("requestedBy");
		requestedByCol.filterMode = FilterMode.DistinctList;
		requestedByCol.filterValue = [];
		requestedByCol.filterTemplate = new GridViewTemplate(`
<multi-typeahead [items]='column.filterValue' (itemsChanged)="parentGridViewFilterCellComponent.filterChanged()" [dataSource]='column.filterOptions'></multi-typeahead>
`, [TypeaheadModule]);
		requestedByCol.sortable = true;
		this.gridDemo.columns.push(requestedByCol);
		this.gridDemo.loadGridState();
	}

	pageChanged() {
	}
}