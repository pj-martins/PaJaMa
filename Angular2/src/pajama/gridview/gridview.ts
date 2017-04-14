import { EventEmitter, PipeTransform, Type } from '@angular/core';
import { ParserService } from '../services/parser.service';
import { OrderByPipe } from '../pipes/order-by.pipe';
import { Observable } from 'rxjs/Observable';
import { SortDirection, Utils } from '../shared';

export const TEMP_KEY_VALUE = "tmp_key_value";

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
	rowEdit = new EventEmitter<RowArguments>();
	rowSave = new EventEmitter<RowArguments>();
	rowCreate = new EventEmitter<RowArguments>();
	rowDelete = new EventEmitter<RowArguments>();
	rowTemplate: Type<IGridViewRowTemplateComponent>;
	customProps: { [name: string]: any; } = {};
	customEvents: any = {};
	filterVisible: boolean;
	disableFilterRow: boolean;
	loading: boolean;
	showNoResults = true;
	allowColumnOrdering = false;
	allowColumnCustomization = false;
	saveGridStateToStorage = false;
	gridStateVersion = 0;
	allowEdit = false;
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

	getVisibleColumns(hideRowTemplate): Array<ColumnBase> {
			
		let cols = new Array<ColumnBase>();
		for (let c of this.columns) {
			if (c.visible && (!hideRowTemplate || !this.rowTemplate))
				cols.push(c);
		}
		return cols;
	}

	getDistinctValues(column: DataColumn): any[] {
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

	private _stateLoaded = false;
	private _defaultState: GridState;
	// TODO: where should this be called from?
	loadGridState() {
		if (this._stateLoaded || !this.saveGridStateToStorage) return;
		if (!this._defaultState) {
			let orderedCols = new OrderByPipe().transform(this.columns, ['columnIndex']);
			for (let i = 0; i < orderedCols.length; i++) {
				orderedCols[i].columnIndex = i;
			}
			this._defaultState = this.getGridState();
		}

		this._stateLoaded = true;
		let stateString = localStorage.getItem(this.name + this.gridStateVersion.toString());
		if (stateString) {
			let state = <GridState>JSON.parse(stateString);
			this.setGridState(state);
			return true;
		}
		return false;
	}

	saveGridState() {
		if (!this.saveGridStateToStorage) return;

		if (!this.name)
			throw 'Grid name required to save to local storage';

		let state = this.getGridState();
		// TODO: is grid name too generic?
		localStorage.setItem(this.name + this.gridStateVersion.toString(), JSON.stringify(state));
	}

	resetGridState() {
		if (this._defaultState) {
			this.setGridState(this._defaultState);
			localStorage.removeItem(this.name + this.gridStateVersion.toString());

			// THIS SEEMS HACKISH! IN ORDER FOR THE COMPONENT TO REDRAW, IT NEEDS TO DETECT
			// A CHANGE TO THE COLUMNS VARIABLE ITSELF RATHER THAN WHAT'S IN THE COLLECTION
			let copies: Array<ColumnBase> = [];
			for (let c of this.columns) {
				copies.push(c);
			}

			this.columns = copies;
			this.refreshData();
		}
	}

	private getGridState(): GridState {
		let state = new GridState();
		state.currentPage = this.currentPage;
		state.pageSize = this.pageSize;
		state.filterVisible = this.filterVisible;

		for (let col of this.columns) {
			let colState = new GridColumnState();
			colState.identifier = col.getIdentifier();
			if (this.allowColumnOrdering)
				colState.columnIndex = col.columnIndex;
			colState.width = col.width;
			colState.visible = col.visible;
			if (col instanceof DataColumn) {
				let cd = <DataColumn>col;
				colState.sortDirection = cd.sortDirection;
				colState.sortIndex = cd.sortIndex;
				// all selected
				if (cd.filterValue instanceof Array && cd.filterOptions && cd.filterValue.length >= cd.filterOptions.length)
					colState.filterValue = null;
				else
					colState.filterValue = cd.filterValue;
			}
			state.gridColumnStates.push(colState);
		}

		return state;
	}

	private setGridState(state: GridState) {
		this.currentPage = state.currentPage;
		this.pageSize = state.pageSize;
		this.filterVisible = state.filterVisible;

		// make a clone as to not hinder original list
		let copy = new Array<ColumnBase>();
		for (let col of this.columns) {
			copy.push(col);
		}
		let orderedCols: Array<ColumnBase> = new OrderByPipe().transform(copy, ['-columnIndex']);

		let refilter = false;
		// lets set ordering, visibility, filtering first
		for (let col of orderedCols) {
			for (let colState of state.gridColumnStates) {
				if (col.getIdentifier() != colState.identifier) continue;
				if (this.allowColumnOrdering)
					col.columnIndex = colState.columnIndex;
				col.visible = colState.visible;
				if (col instanceof DataColumn) {
					let cd = <DataColumn>col;
					cd.sortDirection = colState.sortDirection;
					cd.sortIndex = colState.sortIndex;
					if (colState.filterValue instanceof Array) {
						if (colState.filterValue.length > 0) {
							cd.filterValue = colState.filterValue;
							refilter = true;
						}
					}
					else if (colState.filterValue) {
						cd.filterValue = colState.filterValue;
						refilter = true;
						if (col.filterMode == FilterMode.DateRange) {
							if (col.filterValue.fromDate)
								col.filterValue.fromDate = new Date(col.filterValue.fromDate);
							if (col.filterValue.toDate)
								col.filterValue.toDate = new Date(col.filterValue.toDate);
						}
					}
					else if (cd.filterValue) {
						cd.filterValue = null;
						refilter = true;
					}
				}
				break;
			}
		}

		// handle outside of control
		//if (refilter) {
		//	window.setTimeout(() => this.dataChanged.emit(this), 100);
		//}

		// recalculate indices in case we have duplicates
		orderedCols = new OrderByPipe().transform(orderedCols, ['columnIndex']);
		if (this.allowColumnOrdering) {
			for (let i = 0; i < orderedCols.length; i++) {
				orderedCols[i].columnIndex = i;
			}
		}

		// we need the last column to be a floater, so after we've set indices correctly we can now
		// determine the true last column
		orderedCols = new OrderByPipe().transform(orderedCols, ['-columnIndex']);
		for (let i = 0; i < orderedCols.length; i++) {
			let col = orderedCols[i];
			for (let colState of state.gridColumnStates) {
				if (col.getIdentifier() != colState.identifier) continue;
				// last column is floater
				col.width = i == 0 ? "" : colState.width;
				break;
			}
		}
	}
}
export enum FilterMode {
	None,
	BeginsWith,
	Contains,
	Equals,
	NotEqual,
	DistinctList,
	DynamicList,
	DateRange
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
	customProps: { [name: string]: any; } = {};

	constructor(public caption?: string) { }

	getIdentifier(): string {
		if (!this.name)
			this.name = Utils.newGuid();
		return this.name;
	}
}
export class DataColumn extends ColumnBase {
	fieldType: FieldType = FieldType.String;
	columnPipe: ColumnPipe;
	sortIndex: number = 0;
	filterValue: any;
	format: string;
	sortable: boolean;
	disableWrapping: boolean;
	filterMode: FilterMode = FilterMode.None;
	template: Type<IGridViewCellTemplateComponent>;
	editTemplate: Type<IGridViewCellTemplateComponent>;
	filterTemplate: Type<IGridViewFilterCellTemplateComponent>;
	filterDelayMilliseconds = 0;
	sortDirection: SortDirection = SortDirection.None;
	customSort: (obj1: any, obj2: any) => number;
	customFilter: (obj: any) => boolean;
	required = false;

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
export class NumericColumn extends DataColumn {
	decimalPlaces = 0;
}
export class ButtonColumn extends DataColumn {
	click = new EventEmitter<any>();
	class: string;
	constructor(public fieldName?: string, public caption?: string) {
		super(fieldName, caption);
	}
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
export class DetailGridView extends GridView {

	getChildData: (parent: any) => Observable<Array<any>>;
	hideExpandButton: boolean;

	createInstance(): DetailGridView {
		let grid = new DetailGridView();
		Object.assign(grid, this);
		return grid;
	}
}

// don't want to clutter storage with unneccessary info
export class GridState {
	gridColumnStates: Array<GridColumnState> = [];
	currentPage: number;
	pageSize: number;
	filterVisible: boolean;
}
export class GridColumnState {
	identifier: string;
	width: string;
	sortDirection: SortDirection;
	sortIndex: number;
	columnIndex: number;
	filterValue: any;
	visible: boolean;
}
export class RowArguments {
	row: any;
	cancel: boolean;
}

export interface IGridViewFilterCellTemplateComponent {
	column: DataColumn;
	parentGridView: GridView;
	parentFilterCellComponent: IGridViewFilterCellComponent;
}

export interface IGridViewCellTemplateComponent {
	row: any;
	column: DataColumn;
	parentGridView: GridView;
	parentGridViewComponent: IGridViewComponent;
}

export interface IGridViewRowTemplateComponent {
	parentGridView: GridView;
	row: any;
	parentGridViewComponent: IGridViewComponent;
}

export interface IGridViewComponent {

}

export interface IGridViewFilterCellComponent {
	filterChanged(): void;
}