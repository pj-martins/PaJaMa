import { EventEmitter } from '@angular/core';
import { ParserService } from '../services/parser.service';
import { PipeTransform } from '@angular/core';

export class GridView {
	private _data: Array<any>;

	pageSize: number = 10;
	currentPage: number = 1;
	totalRecords: number;
	columns: Array<ColumnBase> = [];
	showHeader: boolean = true;
	detailGridView: DetailGridView;
	keyFieldName: string;
	selectMode: SelectMode;
	disableAutoSort: boolean;
	disableAutoFilter: boolean;
	pagingType: PagingType = PagingType.Auto;
	height: string;
	dataChanged: EventEmitter<any> = new EventEmitter<any>();
	rowTemplate: GridViewTemplate;
	customProps: { [name: string]: any; } = {};
	customEvents: any = {};
	filterVisible: boolean;
	disableFilterRow: boolean;
	loading: boolean;
	showNoResults: boolean = true;
	name: string;

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
	DistinctList,
	DynamicList
}
export enum PagingType {
	Auto,
	Manual,
	Disabled
}
export enum SelectMode {
	None,
	Single,
	Multi
}
export class ColumnBase {
	visible: boolean = true;
	width: string;
	name: string;
	columnIndex: number = 0;
	allowSizing: boolean;
	getRowCellClass: (row: any) => string;
	dataChanged = new EventEmitter<any[]>();

	constructor(public caption?: string) { }

	getIdentifier(): string {
		return this.name;
	}
}
export class DataColumn extends ColumnBase {
	template: GridViewTemplate;
	fieldType: FieldType = FieldType.String;
	columnPipe: ColumnPipe;
	sortIndex: number = 0;
	filterValue: any;
	sortable: boolean;
	disableWrapping: boolean;
	filterMode: FilterMode = FilterMode.None;
	filterTemplate: GridViewTemplate;
	filterDelayMilliseconds = 0;
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

	getODataField(): string {
		if (!this.fieldName) return '';
		let expression = '';
		let parts = this.fieldName.split('.');
		let firstIn = true;
		for (let p of parts) {
			expression += (firstIn ? '' : '/');
			expression += p.substring(0, 1).toUpperCase() + p.substring(1);
			firstIn = false;
		}
		return expression;
	}

	getCaption(): string {
		if (this.caption) return this.caption;
		let parsedFieldName = this.fieldName;
		if (!parsedFieldName || parsedFieldName == '') return '';
		if (parsedFieldName.lastIndexOf('.') > 0) {
			parsedFieldName = parsedFieldName.substring(parsedFieldName.lastIndexOf('.') + 1, parsedFieldName.length);
		}
		return parsedFieldName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) {
			return str.toUpperCase();
		});
	}

	getIdentifier(): string {
		if (this.name) return this.name;
		if (this.fieldName) return this.fieldName;
		return this.caption;
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
export class ButtonColumn extends DataColumn {
	click = new EventEmitter<any>();
	class: string;
	constructor(public fieldName?: string, public caption?: string) {
		super(fieldName, caption);
	}
}
export class EditColumn extends DataColumn {
	editType = EditColumn.TEXT;
	class: string;
	ngModelChange = new EventEmitter<any>();
	static readonly TEXT: string = "text";
	static readonly TEXTAREA: string = "textarea";
	static readonly NUMBER: string = "number";
	static readonly CHECKBOX: string = "checkbox";
}
export class CheckListColumn extends DataColumn {
	get checkList() { return true; }
	commaSeparated: boolean;
	items: Array<any>;
	displayMember: string;
}
export class TypeaheadColumn extends DataColumn {
	// TODO: what else should be exposed?
	get typeahead() { return true; }
	ngModelChange = new EventEmitter<any>();
	dataSource: any;
	multi: true;
}
export class ColumnPipe {
	constructor(public pipe: PipeTransform, public args?: any) { }
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