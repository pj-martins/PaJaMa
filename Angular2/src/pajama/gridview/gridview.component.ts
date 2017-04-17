﻿import { Component, Input, Output, EventEmitter, OnInit, NgZone, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { GridView, DataColumn, FilterMode, PagingType, FieldType, SelectMode, ColumnBase, GridState, RowArguments, TEMP_KEY_VALUE } from './gridview';
import { SortDirection } from '../shared';
import { DetailGridViewComponent } from './detail-gridview.component';
import { GridViewPagerComponent } from './gridview-pager.component';
import { GridViewHeaderCellComponent } from './gridview-headercell.component';
import { ParserService } from '../services/parser.service';
import { Utils } from '../shared';

@Component({
	moduleId: module.id,
	selector: 'gridview',
	styleUrls: ['../assets/css/styles.css', '../assets/css/icons.css', '../assets/css/buttons.css', 'gridview.css'],
	template: `
<div *ngIf="grid" class='gridview component'>
    <div class='header-button' *ngIf='hasFilterRow()' (click)='toggleFilter()'><div class='icon-filter-black icon-small'></div><strong>&nbsp;&nbsp;Filter</strong></div>
    <div class='header-button' *ngIf='hasFilterRow()' style='padding-right:5px'><input type='checkbox' (click)='toggleFilter()' [checked]='grid.filterVisible' /></div>
    <div class='header-button' *ngIf='grid.detailGridView' (click)='collapseAll()' style='margin-bottom:2px'><div class='icon-minus-black icon-small'></div><strong>&nbsp;&nbsp;Collapse All</strong></div>
    <div class='header-button' *ngIf='grid.detailGridView' (click)='expandAll()'><div class='icon-plus-black icon-small'></div><strong>&nbsp;&nbsp;Expand All</strong></div>
    <table disable-animate [ngClass]="(grid.noBorder ? '' : 'grid-border ') + (grid.height ? 'scrollable-table ' : '') + 'table table-condensed'" cellspacing=0>
        <thead *ngIf='grid.showHeader'>
            <tr>
                <th *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton' style='width:20px'></th>
                <th *ngIf='grid.allowRowSelect' style='width:1%'></th>
                <th *ngFor="let col of grid.getVisibleColumns() | orderBy:['columnIndex'];let i = index;let last = last; let first = first" [style.width]="col.width" [ngClass]="!last ? 'resize-border' : ''">
                    <gridview-headercell (sortChanged)='handleSortChanged($event)' [first]='first' [last]='last' [columnIndex]='i' [column]='col' [parentGridView]="grid"></gridview-headercell>
                </th>
				<th *ngIf='grid.allowEdit' style='width:45px'>
					<button (click)='addRow()' class='icon-plus-white icon-small icon-button'></button>
				</th>
            </tr>
            <tr *ngIf='hasFilterRow() && grid.filterVisible'>
                <td class="filter-td" *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton' style='width:20px'></td>
                <td class="filter-td" *ngIf='grid.allowRowSelect'></td>
                <td class="filter-td" *ngFor="let col of grid.getVisibleColumns() | orderBy:['columnIndex']">
					<gridview-filtercell *ngIf="(col.filterMode && col.filterMode != 0) || col.filterTemplate" [parentGridView]="grid" [parentGridViewComponent]="self" [column]='col'>
					</gridview-filtercell>
				</td>
				<td class="filter-td" *ngIf='grid.allowEdit'></td>
            </tr>
        </thead>
        <tbody [style.height]="grid.height">
            <tr *ngIf='displayData == null && !grid.loading'>
                <td *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton'></td>
                <td [attr.colspan]="getVisibleColumnCount() + (grid.allowEdit ? 1 : 0)">No results found!</td>
            </tr>
            <tr *ngIf='grid.loading'>
                <td [attr.colspan]="getVisibleColumnCount() + 1 + (grid.allowEdit ? 1 : 0)">
                    <div class="template-loading">
                        <div class="template-inner">
                            <br />
                            <img src="data:image/gif;base64,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" />
                            &nbsp;&nbsp;<strong>Loading</strong>
                            <br /><br />
                        </div>
                    </div>
                </td>
            </tr>
            <tr *ngIf='(grid.showNoResults && grid.data && grid.data.length < 1) && !grid.loading'>
                <td [attr.colspan]="getVisibleColumnCount() + 1 + (grid.allowEdit ? 1 : 0)">
                    <div class="template-loading">
                        <div class="template-inner">
                            <strong>No results found!</strong><br />
                        </div>
                    </div>
                </td>
            </tr>
            <tr *ngIf='grid.loading' style="display:none"><td [attr.colspan]="getVisibleColumnCount() + 1 + (grid.allowEdit ? 1 : 0)"></td></tr>
            <ng-template ngFor let-row [ngForOf]="displayData" let-i="index">
                <tr *ngIf='!grid.loading && !grid.rowTemplate' [ngClass]="(grid.getRowClass ? grid.getRowClass(row) : '') + (i % 2 != 0 ? ' gridview-alternate-row' : '') + (grid.selectMode > 0 ? ' selectable-row' : '') + (selectedKeys[getTempKeyValue(row)] ? ' selected-row' : '')" (click)='rowClick(row)'>
                    <td *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton'>
						<!--<button class="glyphicon glyphicon-small {{detailGridViewComponents[getTempKeyValue(row)] && detailGridViewComponents[getTempKeyValue(row)].isExpanded() ? 'glyphicon-minus' : 'glyphicon-plus'}}" (click)='expandCollapse(getTempKeyValue(row))'></button>-->
						<!--<div class="expandcollapse-button-container">
						  <button class="expandcollapse-button" (click)='expandCollapse(getTempKeyValue(row))'>
							<div class="expandcollapse-horizontal"></div>
							<div class="expandcollapse-vertical" *ngIf='!detailGridViewComponents[getTempKeyValue(row)] || !detailGridViewComponents[getTempKeyValue(row)].isExpanded()'></div>
						  </button>
						</div>-->
						<button class="{{detailGridViewComponents[getTempKeyValue(row)] && detailGridViewComponents[getTempKeyValue(row)].isExpanded() ? 'icon-minus-black' : 'icon-plus-black'}} icon-small icon-button" (click)="expandCollapse(getTempKeyValue(row))"></button>
					</td>
                    <td *ngFor="let col of grid.getVisibleColumns(true) | orderBy:['columnIndex'];let last = last; let first = first" [ngClass]="col.getRowCellClass ? col.getRowCellClass(row) : (col.disableWrapping ? 'no-wrap' : '')">
						<gridview-cell [column]="col" [row]="row" [last]='last' [first]='first' [index]='i' [parentGridViewComponent]="self" [parentGridView]="grid"></gridview-cell>
					</td>
					<td *ngIf='grid.allowEdit'>
						<button *ngIf="!editing(row) && !promptConfirm[getTempKeyValue(row)]" class="icon-pencil-black icon-small icon-button" (click)="editRow(row)"></button>
						<button *ngIf="!editing(row) && !promptConfirm[getTempKeyValue(row)]" class="icon-remove-black icon-small icon-button" (click)="confirmDelete(row)"></button>
						<button *ngIf="editing(row)" class="icon-check-black icon-small icon-button" (click)="saveEdit(row)"></button>
						<button *ngIf="editing(row)" class="icon-cancel-black icon-small icon-button" (click)="cancelEdit(row)"></button>
					</td>
                </tr>
				<tr *ngIf='promptConfirm[getTempKeyValue(row)]'>
                    <td [attr.colspan]="getVisibleColumnCount() + 2" class="prompt-confirm-td">
						Are you sure?&nbsp;&nbsp;
						<button class="icon-button" (click)="deleteRow(row)"><span class="icon-check-black icon-small"></span> Yes</button>&nbsp;&nbsp;
						<button class="icon-button" (click)="cancelDelete(row)"><span class="icon-cancel-black icon-small"></span> No</button>&nbsp;&nbsp;
					</td>
                </tr>
                <tr *ngIf='!grid.loading && grid.rowTemplate'>
                    <td [attr.colspan]="getVisibleColumnCount() + (grid.allowEdit ? 1 : 0)"><div gridviewRowTemplate [parentGridView]="grid" [parentGridViewComponent]="self" [row]="row"></div></td>
                </tr>
                <tr [hidden]='grid.loading' *ngIf='grid.detailGridView' class="detail-gridview-row" [hidden]='!detailGridViewComponents[getTempKeyValue(row)] || !detailGridViewComponents[getTempKeyValue(row)].isExpanded()'>
                    <td *ngIf="!grid.detailGridView.hideExpandButton"></td>
                    <td [attr.colspan]="getVisibleColumnCount() + (grid.allowEdit ? 1 : 0)" class='detailgrid-container'><detail-gridview [parentGridViewComponent]="self" [detailGridView]="grid.detailGridView" [row]="row"></detail-gridview></td>
                </tr>
            </ng-template>
        </tbody>
        <tfoot *ngIf='grid.showFooter'>
            <tr>
                <td *ngIf='grid.detailGridView' style='width:20px'></td>
                <td *ngFor="let col of grid.getVisibleColumns() | orderBy:['columnIndex']" grid-view-footer-cell='col'></td>
            </tr>
        </tfoot>
    </table>
	<div class='row'>
		<div class='float-left'>
			<gridview-pager [parentGridView]='grid' [parentGridViewComponent]="self" (pageChanging)='handlePageChanging()' (pageChanged)='handlePageChanged($event)'></gridview-pager>
		</div>
		<div class='float-right gridview-settings'>
			<gridview-settings [parentGridView]='grid'></gridview-settings>
		</div>
	</div>
</div>`
})
export class GridViewComponent {
	private _grid: GridView;

	protected selectedKeys: { [keyFieldValue: string]: boolean } = {};

	@Input() get grid(): GridView {
		return this._grid;
	}
	set grid(value: GridView) {
		if (this._grid != null) {
			this._grid.dataChanged.unsubscribe();
		}
		this._grid = value;
		if (this._grid != null) {
			this._grid.dataChanged.subscribe(() => this.resetData());
			this.initPager();
		}
	}

	@Input() parentComponent: any;

	private _pager: GridViewPagerComponent;
	@ViewChild(GridViewPagerComponent)
	get pager(): GridViewPagerComponent {
		return this._pager;
	}
	set pager(v: GridViewPagerComponent) {
		this._pager = v;
		this.initPager();
	}

	private initPager() {
		if (!this.pager || !this._grid) return;

		let pageFound = false;
		for (let pageSize of this.pager.pageSizes) {
			if (pageSize.size == this._grid.pageSize) {
				pageFound = true;
				break;
			}
		}

		if (!pageFound) {
			this.pager.pageSizes.push({ size: this._grid.pageSize, label: this._grid.pageSize.toString() });
			this.pager.pageSizes.sort((a, b) => {
				if (a.size == 0) return 1;
				if (b.size == 0) return -1;
				if (a.size > b.size) return 1;
				if (a.size < b.size) return -1;
				return 0;
			});
		}
	}

	@ViewChildren(GridViewHeaderCellComponent)
	headerCells: QueryList<GridViewHeaderCellComponent>;

	@Output() sortChanged = new EventEmitter<DataColumn>();
	@Output() filterChanged = new EventEmitter<DataColumn>();
	@Output() pageChanged = new EventEmitter<any>();
	@Output() selectionChanged = new EventEmitter<any[]>();
	constructor(public parserService: ParserService, private zone: NgZone) { }

	editingRows: { [tempKeyValue: string]: any } = {};
	detailGridViewComponents: { [tempKeyValue: string]: DetailGridViewComponent } = {};
	showRequired: { [tempKeyValue: string]: boolean } = {};

	private newRows: { [tempKeyValue: string]: any } = {};
	protected promptConfirm: { [templateKeyValue: string]: boolean } = {};

	protected self: GridViewComponent = this;
	protected sortDirection = SortDirection;
	protected fieldType = FieldType;

	private _unpagedData: Array<any>;
	private _displayData: Array<any>;

	private resetData(resetPage: boolean = false) {

		let expandedKeys: Array<string> = [];
		if (this.detailGridViewComponents) {
			for (let k of Object.keys(this.detailGridViewComponents)) {
				if (this.detailGridViewComponents[k].isExpanded())
					expandedKeys.push(k);
			}
		}

		this._displayData = null;
		this._unpagedData = null;
		if (resetPage)
			this.grid.currentPage = 1;

		if (this.detailGridViewComponents) {
			this.collapseAll();

			if (expandedKeys && expandedKeys.length > 0) {
				for (let k of expandedKeys) {
					for (let d of this.displayData) {
						if (d[this.grid.keyFieldName] == k) {
							if (!this.detailGridViewComponents[k].isExpanded())
								this.detailGridViewComponents[k].expandCollapse();
							break;
						}
					}
				}
			}
		}
	}

	protected hasFilterRow() {
		if (this.grid.disableFilterRow) return false;
		for (let col of this.grid.getDataColumns()) {
			if (col.filterMode != FilterMode.None) {
				return true;
			}
		}
		return false;
	}

	protected editing(row: any) {
		return this.editingRows[this.getTempKeyValue(row)];
	}

	getTempKeyValue(row) {
		if (!row[TEMP_KEY_VALUE])
			row[TEMP_KEY_VALUE] = Utils.newGuid();
		return row[TEMP_KEY_VALUE];
	}

	private _indexWidthInited = false;
	protected getVisibleColumnCount(): number {
		if (this.grid.rowTemplate)
			return 1;

		let count = 0;
		for (let col of this.grid.columns) {
			if (!this._indexWidthInited && count != 0 && col.columnIndex == 0) {
				col.columnIndex = count;
			}
			if (col.visible) {
				count++;
			}
		}

		if (!this._indexWidthInited) {
			this._indexWidthInited = true;
		}
		return count;
	}

	protected toggleFilter() {
		this.grid.filterVisible = !this.grid.filterVisible;
		this.grid.currentPage = 1;
		this.filterChanged.emit(null);
		this.grid.saveGridState();
		this.refreshDataSource();
	}

	protected rowClick(row) {
		if (this.grid.selectMode > 0) {
			this.selectedKeys[this.getTempKeyValue(row)] = !this.selectedKeys[this.getTempKeyValue(row)];

			if (this.grid.selectMode == SelectMode.Single && this.selectedKeys[this.getTempKeyValue(row)]) {
				for (let d of this.grid.data) {
					if (d[this.grid.keyFieldName] != this.getTempKeyValue(row)) {
						this.selectedKeys[d[this.grid.keyFieldName]] = false;
					}
				}
			}

			let selectedRows = [];
			for (let d of this.grid.data) {
				if (this.selectedKeys[d[this.grid.keyFieldName]])
					selectedRows.push(d);
			}

			this.selectionChanged.emit(selectedRows);
		}
	}

	protected handleSortChanged(column: DataColumn) {
		if (this.sortChanged)
			this.sortChanged.emit(column);

		this.resetData();
		if (this.grid.saveGridStateToStorage)
			this.grid.saveGridState();
	}

	private getSortedData(data: Array<any>): Array<any> {
		if (!data) return [];
		if (this.grid.disableAutoSort) return data;

		let sorts = new Array<DataColumn>();
		if (this.grid.columns) {
			for (let col of this.grid.getDataColumns()) {
				if (col.fieldName && col.sortDirection !== undefined && col.sortDirection != SortDirection.None) {
					if (col.sortIndex === undefined)
						col.sortIndex = 0;
					sorts.push(col);
				}
			}
		}

		if (sorts.length <= 0) {
			return data;
		}

		sorts.sort((a, b) => {
			return a.sortIndex - b.sortIndex;
		});

		data.sort((a, b) => {
			for (let i = 0; i < sorts.length; i++) {

				let curr = sorts[i];
				let aval = this.parserService.getObjectValue(curr.fieldName, a);
				let bval = this.parserService.getObjectValue(curr.fieldName, b);

				if (curr.customSort) {
					var s = curr.customSort(aval, bval);
					if (s != 0)
						return s;
				}

				if (aval && typeof aval == "string") aval = aval.toLowerCase();
				if (bval && typeof bval == "string") bval = bval.toLowerCase();

				if (aval == bval)
					continue;

				if (curr.sortDirection == SortDirection.Desc)
					return aval > bval ? -1 : 1;

				return aval < bval ? -1 : 1;
			}

			return 0;
		});

		return data;
	}

	private getFilteredData(rawData: Array<any>): Array<any> {
		if (this.grid.disableAutoFilter) return rawData;
		if (!this.grid.filterVisible && !this.grid.disableFilterRow) return rawData;

		if (!rawData) return [];
		let filteredData: Array<any> = [];
		for (let row of rawData) {
			if (this.showRow(row))
				filteredData.push(row);
		}
		return filteredData;
	}

	get unpagedData(): Array<any> {
		if (!this._unpagedData || this._unpagedData.length < 1) {
			this._unpagedData = this.getFilteredData(this.getSortedData(this.grid.data));
		}
		return this._unpagedData;
	}

	expandAll() {
		for (let row of this.displayData) {
			if (!this.detailGridViewComponents[this.getTempKeyValue(row)].isExpanded())
				this.detailGridViewComponents[this.getTempKeyValue(row)].expandCollapse();
		}
	}

	collapseAll() {
		for (let row of this.displayData) {
			if (this.detailGridViewComponents[this.getTempKeyValue(row)] && this.detailGridViewComponents[this.getTempKeyValue(row)].isExpanded())
				this.detailGridViewComponents[this.getTempKeyValue(row)].expandCollapse();
		}
	}

	expandCollapse(keyFieldValue) {
		this.detailGridViewComponents[keyFieldValue].expandCollapse();
	}

	getSelectedKeys(): Array<any> {
		let selected = [];
		for (let k of Object.keys(this.selectedKeys)) {
			if (this.selectedKeys[k]) {
				selected.push(k);
			}
		}
		return selected;
	}

	private showRow(row: any): boolean {

		for (let col of this.grid.getDataColumns()) {
			if (!col.visible) continue;
			if (col.customFilter) {
				if (!col.customFilter(row))
					return false;
				continue;
			}
			if (col.filterMode != FilterMode.None && col.filterValue != null) {
				let itemVal = this.parserService.getObjectValue(col.fieldName, row);
				switch (col.filterMode) {
					case FilterMode.BeginsWith:
						if (!itemVal || itemVal.toLowerCase().indexOf(col.filterValue.toLowerCase()) != 0)
							return false;
						break;
					case FilterMode.Contains:
					case FilterMode.DistinctList:
					case FilterMode.DynamicList:
						if (col.filterValue instanceof Array) {
							if (col.filterValue.length > 0 && (!itemVal || col.filterValue.indexOf(itemVal) == -1)) {
								return false;
							}
						}
						else if (!itemVal || itemVal.toLowerCase().indexOf(col.filterValue.toLowerCase()) == -1)
							return false;
						break;
					case FilterMode.NotEqual:
						if (col.fieldType == FieldType.Date) {
							return new Date(itemVal).getTime() != new Date(col.filterValue).getTime();
						}
						if (!itemVal || itemVal == col.filterValue)
							return false;
						break;
					case FilterMode.Equals:
						if (col.fieldType == FieldType.Date) {
							return new Date(itemVal).getTime() == new Date(col.filterValue).getTime();
						}
						if (!itemVal || itemVal != col.filterValue)
							return false;
				}
			}
		}

		return true;
	}

	protected get displayData(): Array<any> {
		if (this._displayData == null && this.unpagedData != null) {
			var rawData = this.unpagedData;
			if (this.grid.pageSize == 0 || this.grid.pagingType != PagingType.Auto)
				this._displayData = rawData;
			else
				this._displayData = rawData.slice((this.grid.currentPage - 1) * this.grid.pageSize, this.grid.currentPage * this.grid.pageSize);
		}

		return this._displayData || [];
	}

	private removeRowFromGrid(row: any) {
		for (let i = 0; i < this._displayData.length; i++) {
			if (this.getTempKeyValue(this._displayData[i]) == this.getTempKeyValue(row)) {
				this._displayData.splice(i, 1);
				break;
			}
		}
		for (let i = 0; i < this.grid.data.length; i++) {
			if (this.getTempKeyValue(this.grid.data[i]) == this.getTempKeyValue(row)) {
				this.grid.data.splice(i, 1);
				break;
			}
		}
	}

	addRow() {
		let newRow = {};

		if (this.grid.detailGridView) {
			window.setTimeout(() => {
				let dgvc = this.detailGridViewComponents[this.getTempKeyValue(newRow)];
				dgvc.expandCollapse();
			}, 100)

		}

		let args = new RowArguments();
		args.row = newRow;
		this.grid.rowCreate.emit(args);
		if (!args.cancel) {
			this._displayData.splice(0, 0, newRow);
			this.grid.data.splice(0, 0, newRow);
			this.editingRows[this.getTempKeyValue(newRow)] = newRow;
			this.newRows[this.getTempKeyValue(newRow)] = newRow;
		}
	}

	editRow(row: any) {
		let args = new RowArguments();
		args.row = row;

		if (this.grid.detailGridView) {
			let dgvc = this.detailGridViewComponents[this.getTempKeyValue(row)];
			dgvc.expandCollapse();

		}

		this.grid.rowEdit.emit(args);
		if (!args.cancel) {
			this.editingRows[this.getTempKeyValue(row)] = {};
			Object.assign(this.editingRows[this.getTempKeyValue(row)], row);
		}
	}

	protected confirmDelete(row: any) {
		this.promptConfirm[this.getTempKeyValue(row)] = true;
	}

	protected cancelDelete(row: any) {
		delete this.promptConfirm[this.getTempKeyValue(row)];
	}

	deleteRow(row: any) {
		let args = new RowArguments();
		args.row = row;
		this.grid.rowDelete.emit(args);
		if (!args.cancel) {
			this.removeRowFromGrid(row);
			delete this.editingRows[this.getTempKeyValue(row)];
			delete this.newRows[this.getTempKeyValue(row)];
			delete this.promptConfirm[this.getTempKeyValue(row)];
		}
	}

	saveEdit(row: any): boolean {
		delete this.showRequired[this.getTempKeyValue((row))];
		for (let col of this.grid.getDataColumns()) {
			if (col.fieldName && col.required && !this.parserService.getObjectValue(col.fieldName, row)) {
				this.showRequired[this.getTempKeyValue((row))] = true;
				return false;
			}
		}

		if (this.grid.detailGridView) {
			let dgvc = this.detailGridViewComponents[this.getTempKeyValue(row)];
			if (dgvc) {
				for (let row of dgvc.detailGridViewInstance.data) {
					if (!dgvc.gridViewComponent.saveEdit(row))
						return false;
				}
			}
		}

		let args = new RowArguments();
		args.row = row;
		this.grid.rowSave.emit(args);
		if (!args.cancel) {
			delete this.editingRows[this.getTempKeyValue(row)];
			return true;
		}
		return false;
	}

	cancelEdit(row: any) {
		if (this.grid.detailGridView) {
			let dgvc = this.detailGridViewComponents[this.getTempKeyValue(row)];
			if (dgvc) {
				for (let row of dgvc.detailGridViewInstance.data) {
					dgvc.gridViewComponent.cancelEdit(row);
				}
			}
		}

		if (this.newRows[this.getTempKeyValue(row)]) {
			this.removeRowFromGrid(row);
			delete this.newRows[this.getTempKeyValue(row)];
		}
		else
			Object.assign(row, this.editingRows[this.getTempKeyValue(row)]);
		delete this.editingRows[this.getTempKeyValue(row)];
	}

	refreshDataSource() {
		this._displayData = null;
		this._unpagedData = null;
	}

	resetDisplayData() {
		this._displayData = null;
	}

	handlePageChanging() {
		if (this.detailGridViewComponents)
			this.collapseAll();
	}

	handlePageChanged(pageNumber: any) {
		if (this.pageChanged)
			this.pageChanged.emit(pageNumber);
		if (this.grid.saveGridStateToStorage)
			this.grid.saveGridState();
	}
}