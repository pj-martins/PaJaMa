import { Component, OnInit } from '@angular/core';
import { GridView, DataColumn, FilterMode, FieldType, SortDirection } from '../../pajama/gridview/gridview';
import { Event } from '../classes/classes';

declare var EVENTS: Array<Event>;

@Component({
	moduleId: module.id,
	selector: 'gridview-basic',
	template: `
<gridview [grid]='gridDemo' (pageChanged)='pageChanged()'></gridview>
`
})
export class GridViewBasicDemoComponent implements OnInit {
	protected gridDemo: GridView;

	constructor() {
		this.initGrid();
	}

	ngOnInit() {
		this.gridDemo.data = EVENTS;
	}

	private initGrid() {
		this.gridDemo = new GridView();
		this.gridDemo.filterVisible = true;
		this.gridDemo.allowColumnOrdering = true;
		this.gridDemo.saveGridStateToStorage = true;
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

		let coordinatorCol = new DataColumn("coordinator");
		coordinatorCol.filterMode = FilterMode.DistinctList;
		coordinatorCol.sortable = true;
		this.gridDemo.columns.push(coordinatorCol);

		let phoneNumberCol = new DataColumn("phoneNumber");
		phoneNumberCol.width = "180px";
		this.gridDemo.columns.push(phoneNumberCol);

		let evtTypeCol = new DataColumn("hallEventType.eventTypeName", "Event Type");
		evtTypeCol.filterMode = FilterMode.Contains;
		evtTypeCol.allowSizing = true;
		this.gridDemo.columns.push(evtTypeCol);

		let requestedByCol = new DataColumn("requestedBy");
		requestedByCol.filterMode = FilterMode.DistinctList;
		requestedByCol.sortable = true;
		this.gridDemo.columns.push(requestedByCol);
	}

	pageChanged() {
	}
}