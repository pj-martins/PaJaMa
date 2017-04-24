import { Component, OnInit, Type } from '@angular/core';
import { GridView, DataColumn, FilterMode, FieldType, DetailGridView } from 'pajama/gridview/gridview';
import { SortDirection } from 'pajama/shared';
import { TypeaheadModule } from 'pajama/typeahead/typeahead.module';
import { MultiTextboxModule } from 'pajama/multi-textbox/multi-textbox.module';
import { Event } from '../classes/classes';
import {
	CustomerCellTemplateComponent, CoordinatorFilterCellTemplateComponent, EventTypeFilterCellTemplateComponent, RequestedByFilterCellTemplateComponent, CustomerCellEditTemplateComponent
} from './grid-cell-templates.component';
import { RoomComponent } from './room.component';
import { Observable } from 'rxjs/Observable';

declare var EVENTS: Array<Event>;

@Component({
	moduleId: module.id,
	selector: 'demo-grid',
	template: `
<gridview [grid]='gridDemo' (pageChanged)='pageChanged()'></gridview>
<br /><br /><br /><br />
<input type="checkbox" [(ngModel)]='gridDemo.allowEdit' />Allow Edit
<br />
Height: <input type="text" [(ngModel)]='gridDemo.height' />
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
		this.gridDemo.pageSize = 20;
		this.gridDemo.filterVisible = true;
		this.gridDemo.allowColumnOrdering = true;
		this.gridDemo.saveGridStateToStorage = true;
		this.gridDemo.allowColumnCustomization = true;
		this.gridDemo.name = "gridDemo";

		let custCol = new DataColumn("customer.customerName");
		custCol.filterMode = FilterMode.DistinctList;
		custCol.sortable = true;
		custCol.allowSizing = true;
		custCol.template = CustomerCellTemplateComponent;
		custCol.editTemplate = CustomerCellEditTemplateComponent;
		this.gridDemo.columns.push(custCol);

		let startCol = new DataColumn("eventStartDT", "Start");
		startCol.fieldType = FieldType.Date;
		startCol.sortable = true;
		startCol.sortDirection = SortDirection.Desc;
		startCol.width = "110px";
		startCol.filterMode = FilterMode.DateRange;
		this.gridDemo.columns.push(startCol);

		let endCol = new DataColumn("eventEndDT", "End");
		endCol.fieldType = FieldType.Date;
		endCol.width = "110px";
		endCol.filterMode = FilterMode.DateRange;
		this.gridDemo.columns.push(endCol);

		this._coordinatorColumn = new DataColumn("coordinator");

		this._coordinatorColumn.filterMode = FilterMode.Equals;
		this._coordinatorColumn.filterTemplate = CoordinatorFilterCellTemplateComponent;
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
		evtTypeCol.filterTemplate = EventTypeFilterCellTemplateComponent;
		evtTypeCol.allowSizing = true;
		this.gridDemo.columns.push(evtTypeCol);

		let requestedByCol = new DataColumn("requestedBy");
		requestedByCol.filterMode = FilterMode.DistinctList;
		requestedByCol.filterValue = [];
		//requestedByCol.filterTemplate = RequestedByFilterCellTemplateComponent;
		requestedByCol.sortable = true;
		this.gridDemo.columns.push(requestedByCol);

		let cancelledCol = new DataColumn("cancelled");
		cancelledCol.fieldType = FieldType.Boolean;
		this.gridDemo.columns.push(cancelledCol);

		this.gridDemo.keyFieldName = "id";

		let roomsDetailGridView = new DetailGridView();
		roomsDetailGridView.rowTemplate = RoomComponent;
		roomsDetailGridView.getChildData = (parentRow: any) => {
			let evt = <Event>parentRow;
			if (!evt.hallRequestRooms)
				evt.hallRequestRooms = [];
			return Observable.create(o => o.next(evt.hallRequestRooms));
		}
		this.gridDemo.detailGridView = roomsDetailGridView;

		this.gridDemo.loadGridState();
	}

	pageChanged() {
	}
}