import { EventEmitter } from '@angular/core';
import { ParserService } from './parser.service';

export class GridView {
	private _data: Array<any>;

	pageSize: number = 10;
	currentPage: number = 1;
	totalRecords: number;
	columns: Array<ColumnBase> = [];
	showHeader: boolean = true;
	detailGridView: DetailGridView;
	keyFieldName: string;
	allowRowSelect: boolean;
	disableAutoSort: boolean;
	disableAutoFilter: boolean;
	pagingType: PagingType = PagingType.Auto;
	height: number = 0;
	dataChanged: EventEmitter<any> = new EventEmitter<any>();
	rowTemplate: GridViewTemplate;
	customProps: { [name: string]: any; } = {};
	customEvents: any = {};
	filterVisible: boolean;
	loading: boolean;
	showNoResults: boolean = true;

	getRowClass: (row: any) => string;

	getDataColumns(): Array<DataColumn> {
		let cols: Array<DataColumn> = [];
		for (let col of this.columns) {
			if (col instanceof DataColumn) {
				cols.push(<DataColumn>col);
			}
		}
		return cols;
	}

	private getDistinctValues(column: DataColumn): any[] {
		if (!column.fieldName) return null;
		if (!this._data) return null;

		let parserService = new ParserService();

		let vals: any[] = [];
		for (let i = 0; i < this._data.length; i++) {

			let val = parserService.getObjectValue(column.fieldName, this._data[i]);
			if (vals.indexOf(val) < 0)
				vals.push(val);
		}
		vals.sort();
		return vals;
	}

	setFilterOptions() {
		for (let col of this.getDataColumns()) {
			if (col.filterMode == FilterMode.DistinctList) {
				col.filterOptions = this.getDistinctValues(col);
				col.filterOptionsChanged.emit(col);
			}
		}
	}

	refreshData() {
		this.dataChanged.emit(this);
		this.setFilterOptions();
	}

	set data(data: Array<any>) {
		this._data = data;
		this.refreshData();
	}

	get data(): Array<any> {
		return this._data;
	}
}
export enum SortDirection {
	None,
	Asc,
	Desc
}
export enum FilterMode {
	None,
	BeginsWith,
	Contains,
	Equals,
	NotEqual,
	DistinctList
}
export enum PagingType {
	Auto,
	Manual,
	Disabled
}
abstract class ColumnBase {
	visible: boolean = true;
	width: string;
	getRowCellClass: (row: any) => string;
	dataChanged = new EventEmitter<any[]>();

	constructor(public caption?: string) { }
}
export class DataColumn extends ColumnBase {
	template: GridViewTemplate;
	fieldType: FieldType = FieldType.String;
	format: string;
	sortIndex: number = 0;
	filterValue: any;
	sortable: boolean;
	filterMode: FilterMode = FilterMode.None;
	filterTemplate: GridViewTemplate;
	sortDirection: SortDirection = SortDirection.None;
	customSort: (obj1: any, obj2: any) => number;

	private _filterOptions: any[];
	get filterOptions(): any[] {
		return this._filterOptions;
	}

	set filterOptions(v: any[]) {
		this._filterOptions = v;
		this.filterOptionsChanged.emit(v);
	}

	filterOptionsChanged: EventEmitter<any> = new EventEmitter<any>();

	constructor(public fieldName?: string, public caption?: string) {
		super(caption);
		this.dataChanged.subscribe((d: any[]) => {

		});
	}
}
export class LinkColumn extends DataColumn {
	url: string;
	parameters: { [parameterName: string]: string } = {};
	target: string;

	constructor(public fieldName?: string, public caption?: string) {
		super(fieldName, caption);
	}
}
export class ButtonColumn extends ColumnBase {
	buttonClass: string;
	innerHtml: string;
	click = new EventEmitter<any>();
}
export enum FieldType {
	String,
	Boolean,
	Date,
	Html
}
export class DetailGridViewDataEventArgs {
	constructor(public parentRow: any, public detailGridViewInstance: DetailGridView) { }
}
export class DetailGridView extends GridView {

	setChildData: EventEmitter<DetailGridViewDataEventArgs> = new EventEmitter<DetailGridViewDataEventArgs>();
	hideExpandButton: boolean;

	createInstance(): DetailGridView {
		let grid = new DetailGridView();
		Object.assign(grid, this);
		return grid;
	}
}
export class GridViewTemplate {
	constructor(public template: string, public imports?: Array<any>, public declarations?: Array<any>, public styleUrls?: Array<string>) { }
}