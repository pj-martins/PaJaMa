import { Component, Input, Output, EventEmitter, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { GridView, DataColumn, LinkColumn, ButtonColumn, FilterMode, SortDirection, PagingType, FieldType } from './gridview';
import { DetailGridViewComponent } from './detail-gridview.component';
import { ParserService } from './parser.service';

@Component({
	moduleId: module.id,
	selector: 'gridview',
	styleUrls: ['gridview.css'],
	template: `
<div *ngIf="grid">
    <div class='header-button' [hidden]='!(hasFilterRow())' (click)='grid.filterVisible = !grid.filterVisible'><div class='glyphicon glyphicon-filter'></div><strong>&nbsp;&nbsp;Filter</strong></div>
    <div class='header-button' [hidden]='!(hasFilterRow())' style='padding-right:5px'><input type='checkbox' [(ngModel)]='grid.filterVisible' /></div>
    <div class='header-button' *ngIf='grid.detailGridView' (click)='collapseAll()' style='margin-bottom:2px'><div class='glyphicon glyphicon-minus'></div><strong>&nbsp;&nbsp;Collapse All</strong></div>
    <div class='header-button' *ngIf='grid.detailGridView' (click)='expandAll()'><div class='glyphicon glyphicon-plus'></div><strong>&nbsp;&nbsp;Expand All</strong></div>
    <table disable-animate [ngClass]="'gridview ' + (grid.noBorder ? '' : 'grid-border ') + (grid.height > 0 ? 'scrollable-table ' : '') + (grid.rowClick ? 'table-hover ' : '') + 'table table-condensed'">
        <thead *ngIf='grid.showHeader'>
            <tr>
                <th *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton' style='width:39px'></th>
                <th *ngIf='grid.allowRowSelect' style='width:1%'></th>
                <th *ngFor='let col of grid.columns' [hidden]='!col.visible' [style.width]="col.width">
                    <div class='sort-header' (click)='setSort(col, $event)'>
                        <div class='header-caption' [style.width]="(col.fieldName || col.sortField) && col.sortable ? '' : '100%'"><div [innerHTML]="col | columnCaption"></div></div>
                        <div [ngClass]="{ 'header-caption sort-arrows' : (col.fieldName || col.sortField) && col.sortable }" *ngIf='(col.fieldName || col.sortField) && col.sortable'>
                            <div [ngClass]="'sort-arrow top-empty' + (col.sortDirection == sortDirection.None ? ' glyphicon glyphicon-menu-up' : '')"></div>
                            <div [ngClass]="'sort-arrow bottom-empty' + (col.sortDirection == sortDirection.None ? ' glyphicon glyphicon-menu-down' : '')"></div>
                            <div [ngClass]="'sort-arrow' + (col.sortDirection == sortDirection.Desc ? ' glyphicon glyphicon-menu-up' : '')"></div>
                            <div [ngClass]="'sort-arrow' + (col.sortDirection == sortDirection.Asc ? ' glyphicon glyphicon-menu-down' : '')"></div>
                        </div>
                    </div>
                </th>
            </tr>
            <tr [hidden]='!(hasFilterRow && grid.filterVisible)'>
                <td *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton' style='width:39px'></td>
                <td *ngIf='grid.allowRowSelect' style='width:1%'></td>
                <td *ngFor='let col of grid.columns' [hidden]='!(col.visible || col.visible === undefined)'><gridview-filtercell *ngIf="col.filterMode && col.filterMode != 0" [parentGridView]="grid" [parentGridViewComponent]="self" [column]='col'></gridview-filtercell></td>
            </tr>
        </thead>
        <tbody [style.height]="grid.height + 'px'">
            <tr [hidden]='displayData != null'>
                <td *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton'></td>
                <td [attr.colspan]="getVisibleColumnCount() + (grid.allowRowSelect ? 1 : 0) ">No results found!</td>
            </tr>
            <tr [hidden]='!(grid.loading)'>
                <td [attr.colspan]="getVisibleColumnCount() + 1 + (grid.allowRowSelect ? 1 : 0) ">
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
            <tr [hidden]='!(grid.showNoResults && grid.data && grid.data.length < 1)'>
                <td [attr.colspan]="getVisibleColumnCount() + 1 + (grid.allowRowSelect ? 1 : 0) ">
                    <div class="template-loading">
                        <div class="template-inner">
                            <strong>No results found!</strong><br />
                        </div>
                    </div>
                </td>
            </tr>
            <tr [hidden]='!(grid.loading)' style="display:none"><td [attr.colspan]="getVisibleColumnCount() + 1 + (grid.allowRowSelect ? 1 : 0) "></td></tr>
            <template ngFor let-row [ngForOf]="displayData" let-i="index">
                <tr *ngIf='!grid.rowTemplate' [ngClass]="(grid.getRowClass ? grid.getRowClass(row) : '') + (i % 2 != 0 ? ' gridview-alternate-row' : '')" (click)='grid.rowClick ? grid.rowClick(row) : null' style="cursor:{{ grid.rowClick ? 'pointer' : '' }}">
                    <td *ngIf='grid.detailGridView && !grid.detailGridView.hideExpandButton' style='width:39px'><button class="glyphicon glyphicon-small {{detailGridViewComponents[row[grid.keyFieldName]] && detailGridViewComponents[row[grid.keyFieldName]].isExpanded() ? 'glyphicon-minus' : 'glyphicon-plus'}}" (click)='expandCollapse(row[grid.keyFieldName])'></button></td>
                    <td *ngIf='grid.allowRowSelect' style='width:1%'><div style='text-align:center'><input type='checkbox' [(ngModel)]='selectedKeys[row[grid.keyFieldName]]' /></div></td>
                    <td *ngFor='let col of grid.columns' [hidden]='!(!grid.rowTemplate && (col.visible || col.visible === undefined))' [style.width]="col.width" [ngClass]="col.getRowCellClass ? col.getRowCellClass(row) : ''">
						<!-- TODO: try separating format into non grid view cell component -->
						<div *ngIf="!col.click && (col.template || (col.format && col.fieldType != fieldType.Date))">
							<gridview-cell [column]="col" [row]="row" [parentGridViewComponent]="self" [parentGridView]="grid"></gridview-cell>
						</div>
						<div *ngIf="!col.click && !col.template && !col.format && col.fieldType == fieldType.Html">
							<div [innerHTML]="parserService.getObjectValue(col.fieldName, row) == null ? '' : parserService.getObjectValue(col.fieldName, row)"></div>
						</div>
						<div *ngIf="!col.click && !col.template && col.fieldType == fieldType.Date">
							<div [innerHTML]="parserService.getObjectValue(col.fieldName, row) == null ? '' : parserService.getObjectValue(col.fieldName, row) | moment:(col.format ? col.format : 'MM/DD/YYYY')"></div>
						</div>
						<div *ngIf="!col.click && !col.template && !col.format && col.fieldType == fieldType.Boolean">
							<div [ngClass]="{ 'glyphicon glyphicon-ok' : parserService.getObjectValue(col.fieldName, row) == true }"></div>
						</div>
						<!-- TODO: should we allow links to above items? duplication here too -->
						<div *ngIf="!col.click && col.fieldType != fieldType.Html && col.fieldType != fieldType.Date && col.fieldType != fieldType.Boolean && !col.template && !col.format">
							<div *ngIf="col.url">
								<!-- TODO: always open in new window? -->
								<!-- <a href='{{col.linkSettings.linkToUrl}}{{row[col.linkSettings.linkToField ? col.linkSettings.linkToField : grid.keyFieldName]}}' target="_{{row[col.linkSettings.targetField ? col.linkSettings.targetField : grid.keyFieldName]}}"><div [innerText]="parserService.getObjectValue(col.fieldName, row) == null ? '' : parserService.getObjectValue(col.fieldName, row)"></div></a> -->
								<a href='{{getLink(col, row)}}' target='{{getLinkTarget(col, row)}}'>{{parserService.getObjectValue(col.fieldName, row)}}</a>
							</div>
							<div *ngIf="!col.url">
								<div [innerText]="parserService.getObjectValue(col.fieldName, row) == null ? '' : parserService.getObjectValue(col.fieldName, row)"></div>
							</div>
						</div>
						<div *ngIf='col.click'>
							<button [ngClass]='col.buttonClass' (click)='buttonColumnClick(col, row)'><div *ngIf='col.innerHtml' [innerHtml]='col.innerHtml'></div><div *ngIf='!col.innerHtml'>{{col.fieldName ? parserService.getObjectValue(col.fieldName, row) : ''}}</div></button>
						</div>
					</td>
                </tr>
                <tr *ngIf='grid.rowTemplate'>
                    <td [attr.colspan]="getVisibleColumnCount()"><gridview-rowtemplate [parentGridView]="grid" [parentGridViewComponent]="self" [row]="row"></gridview-rowtemplate></td>
                </tr>
                <tr *ngIf='grid.detailGridView' class="detail-gridview-row" [hidden]='!detailGridViewComponents[row[grid.keyFieldName]] || !detailGridViewComponents[row[grid.keyFieldName]].isExpanded()'>
                    <td *ngIf="!grid.allowRowSelect && !grid.detailGridView.hideExpandButton"></td>
                    <td [attr.colspan]="getVisibleColumnCount() + (grid.allowRowSelect ? 1 : 0) " class='detailgrid-container'><detail-gridview [parentGridViewComponent]="self" [detailGridView]="grid.detailGridView" [row]="row"></detail-gridview></td>
                </tr>
            </template>
        </tbody>
        <tfoot *ngIf='grid.showFooter'>
            <tr>
                <td *ngIf='grid.detailGridView' style='width:39px'></td>
                <td *ngIf='grid.allowRowSelect' style='width:1%'></td>
                <td *ngFor='let col of grid.columns' [hidden]='!(col.visible || col.visible === undefined)' [style.width]="col.width" grid-view-footer-cell='col'></td>
            </tr>
        </tfoot>
    </table>
    <div class='show-hide-animation grid-pagination' [hidden]='!(grid.pagingType < 2 && displayData && grid.data && grid.data.length > 0)'>
        <div [hidden]='grid.pageSize <= 0 || (grid.pagingType == 0 ? unpagedData.length : grid.totalRecords) <= grid.pageSize'>
            <ul class="pagination">
                <li (click)='setPage(1)'>First</li>
                <li (click)='setPage(grid.currentPage - 1)'>Previous</li>
				<li [style.display]="!moreToLeft?'none':'inline-block'" (click)="setPage(grid.currentPage - grid.pageSize)">...</li>
                <li *ngFor="let p of getPageArray()" [ngClass]="{'pagination-selected' : p == grid.currentPage}" (click)='setPage(p)'>{{p}}</li>
				<li [style.display]="!moreToRight?'none':'inline-block'" (click)="setPage(grid.currentPage + grid.pageSize)">...</li>
                <li (click)='setPage(grid.currentPage + 1)'>Next</li>
                <li (click)='gotoLast()'>Last</li>
            </ul>
            <br />
        </div>
		<div [hidden]="(grid.pagingType == 0 ? unpagedData.length : grid.totalRecords) <= 10">
			Show: <select [(ngModel)]='grid.pageSize' (ngModelChange)='setPage(1)'><option *ngFor='let ps of pageSizes' [value]="ps.size">{{ps.label}}</option></select>&nbsp;&nbsp;&nbsp;&nbsp;
			Showing {{ displayData.length }} of {{ grid.pagingType == 0 ? unpagedData.length : grid.totalRecords }} total records.
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

	@Input() parentComponent: any;

	set grid(value: GridView) {
		if (this._grid != null)
			this._grid.dataChanged.unsubscribe();
		this._grid = value;
		if (this._grid != null) {
			if (this._grid.detailGridView && !this._grid.keyFieldName) {
				throw "Grids with detail grids require a key field name";
			}
			if (this._grid.allowRowSelect && !this._grid.keyFieldName) {
				throw "Grids with row select enable require a key field name";
			}
			this._grid.dataChanged.subscribe(() => this.resetData());
			let pageFound = false;
			for (let pageSize of this.pageSizes) {
				if (pageSize.size == this._grid.pageSize) {
					pageFound = true;
					break;
				}
			}

			if (!pageFound) {
				this.pageSizes.push({ size: this._grid.pageSize, label: this._grid.pageSize.toString() });
				this.pageSizes.sort((a, b) => {
					if (a.size == 0) return 1;
					if (b.size == 0) return -1;
					if (a.size > b.size) return 1;
					if (a.size < b.size) return -1;
					return 0;
				});
			}
		}
	}

	@Output() sortChanged = new EventEmitter<DataColumn>();
	@Output() filterChanged = new EventEmitter<DataColumn>();
	@Output() pageChanged = new EventEmitter<any>();

	constructor(public parserService: ParserService) { }

	protected pageSizes: Array<any> = [{ size: 10, label: '10' }, { size: 25, label: '25' }, { size: 50, label: '50' }, { size: 100, label: '100' }, { size: 0, label: 'All' }];
	detailGridViewComponents: { [keyFieldValue: string]: DetailGridViewComponent } = {};
	protected self: GridViewComponent = this;
	protected sortDirection = SortDirection;
	protected fieldType = FieldType;

	private _unpagedData: Array<any>;
	private _displayData: Array<any>;

	protected moreToLeft = false;
	protected moreToRight = false;

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
		for (let col of this.grid.getDataColumns()) {
			if (col.filterMode != FilterMode.None) {
				return true;
			}
		}
		return false;
	}

	protected getLink(column: LinkColumn, row: any) {
		let url = column.url;
		for (let k of Object.keys(column.parameters)) {
			url += `;${k}=${this.parserService.getObjectValue(column.parameters[k], row)}`;
		}
		return url;
	}

	protected getLinkTarget(column: LinkColumn, row: any) {
		if (column.target) {
			return `_${this.parserService.getObjectValue(column.target, row)}`;
		}
		return '';
	}


	protected getVisibleColumnCount(): number {
		if (this.grid.rowTemplate)
			return 1;

		let count = 0;
		for (let col of this.grid.columns) {
			if (col.visible)
				count++;
		}
		return count;
	}

	protected buttonColumnClick(column: ButtonColumn, row: any) {
		column.click.emit(row);
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
		if (!this.grid.filterVisible) return rawData;

		if (!rawData) return [];
		let filteredData: Array<any> = [];
		for (let row of rawData) {
			if (this.showRow(row))
				filteredData.push(row);
		}
		return filteredData;
	}

	protected get unpagedData(): Array<any> {
		if (!this._unpagedData || this._unpagedData.length < 1) {
			this._unpagedData = this.getFilteredData(this.getSortedData(this.grid.data));
		}
		return this._unpagedData;
	}

	setPage(page: number) {
		if (this.detailGridViewComponents)
			this.collapseAll();
		if (page) {
			if (page < 1) page = 1;
			let totalPages = this.getTotalPages();
			if (page > totalPages)
				page = totalPages;

			this.grid.currentPage = page;
		}
		this._displayData = null;
		this.pageChanged.emit(page);
	}

	expandAll() {
		for (let row of this.displayData) {
			if (!this.detailGridViewComponents[row[this.grid.keyFieldName]].isExpanded())
				this.detailGridViewComponents[row[this.grid.keyFieldName]].expandCollapse();
		}
	}

	collapseAll() {
		for (let row of this.displayData) {
			if (this.detailGridViewComponents[row[this.grid.keyFieldName]] && this.detailGridViewComponents[row[this.grid.keyFieldName]].isExpanded())
				this.detailGridViewComponents[row[this.grid.keyFieldName]].expandCollapse();
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
			if (col.filterMode != FilterMode.None && col.filterValue != null) {
				let itemVal = this.parserService.getObjectValue(col.fieldName, row);
				switch (col.filterMode) {
					case FilterMode.BeginsWith:
						if (!itemVal || itemVal.toLowerCase().indexOf(col.filterValue.toLowerCase()) != 0)
							return false;
						break;
					case FilterMode.Contains:
					case FilterMode.DistinctList:
						if (col.filterValue instanceof Array) {
							if (!itemVal || col.filterValue.indexOf(itemVal) == -1) {
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

	get displayData(): Array<any> {
		if (this._displayData == null) {
			var rawData = this.unpagedData;
			if (this.grid.pageSize == 0 || this.grid.pagingType != PagingType.Auto)
				this._displayData = rawData;
			else
				this._displayData = rawData.slice((this.grid.currentPage - 1) * this.grid.pageSize, this.grid.currentPage * this.grid.pageSize);
		}

		return this._displayData;
	}

	setSort(column: DataColumn, event: any) {
		if (!column.sortable) return;

		let maxIndex = -1;
		for (let col of this.grid.getDataColumns()) {
			if (col == column) continue;
			if (col.sortable) {
				if (!event.ctrlKey) {
					col.sortDirection = SortDirection.None;
					col.sortIndex = 0;
				}
				else if (col.sortIndex > maxIndex)
					maxIndex = col.sortIndex;
			}
		}
		if (event.ctrlKey)
			column.sortIndex = maxIndex + 1;

		if (column.sortDirection === undefined) {
			column.sortDirection = SortDirection.Asc;
		}
		else {
			switch (column.sortDirection) {
				case SortDirection.None:
				case SortDirection.Desc:
					column.sortDirection = SortDirection.Asc;
					break;
				case 1:
					column.sortDirection = SortDirection.Desc;
					break;
			}
		}

		if (this.sortChanged)
			this.sortChanged.emit(column);

		this.resetData();
	}

	private getTotalPages(): number {
		let totalItems = (this.grid.pagingType == PagingType.Auto ? (this.unpagedData ? this.unpagedData.length : 0) : this.grid.totalRecords);
		let totalPages = Math.ceil(totalItems / this.grid.pageSize);
		return totalPages;
	}

	getPageArray() {
		let pageArray: number[] = [];
		let totalPages = this.getTotalPages();
		let start = 1;
		let end = totalPages > 10 ? 10 : totalPages;
		if (totalPages > 10 && this.grid.currentPage > 5) {
			end = this.grid.currentPage + 4;
			if (end > totalPages) {
				end = totalPages;
			}
			start = end - 9;

		}
		for (let i = start; i <= end; i++) {
			pageArray.push(i);
		}

		this.moreToRight = totalPages > end;
		this.moreToLeft = start > 1;

		return pageArray;
	}

	gotoLast() {
		this.setPage(this.getTotalPages());
	}
}