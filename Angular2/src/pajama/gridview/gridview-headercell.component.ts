import { Component, Input, Output, EventEmitter, ElementRef, NgZone } from '@angular/core';
import { GridView, DataColumn, FieldType, ColumnBase } from './gridview';
import { SortDirection } from '../shared';
import { GridViewComponent } from './gridview.component';
import { PipesModule } from '../pipes/pipes.module';
import { ParserService } from '../services/parser.service';

@Component({
	moduleId: module.id,
	selector: 'gridview-headercell',
	styleUrls: ['../styles.css', 'gridview-headercell.css'],
	template: `
<div class="sort-header" (click)='setSort(column, $event)' [id]='column.getIdentifier()' draggable="true" (dragover)="dragOver($event)" (dragstart)="dragStart($event)" (drop)="drop($event)">
	<div class='header-caption' [style.width]="(column.fieldName || column.sortField) && column.sortable ? '' : '100%'">
		<div [innerHTML]="column.getCaption()"></div>
	</div>
	<div [ngClass]="{ 'header-caption sort-arrows' : (column.fieldName || column.sortField) && column.sortable }" *ngIf='(column.fieldName || column.sortField) && column.sortable'>
		<div [ngClass]="'top-empty' + (column.sortDirection == sortDirection.None ? ' arrow-up' : '') + ' sort-arrow'"></div>
		<div [ngClass]="'bottom-empty' + (column.sortDirection == sortDirection.None ? ' arrow-down' : '') + ' sort-arrow'"></div>
		<div [ngClass]="'sort-arrow' + (column.sortDirection == sortDirection.Desc ? ' arrow-up' : '')"></div>
		<div [ngClass]="'sort-arrow' + (column.sortDirection == sortDirection.Asc ? ' arrow-down' : '')"></div>
	</div>
</div>
<div class='resize-div' *ngIf='column.allowSizing && !last' (mousedown)='startResize($event)'>|</div>
`
})
export class GridViewHeaderCellComponent {
	@Input() column: DataColumn;
	@Input() parentGridView: GridView;
	@Input() columnIndex: number;
	@Input() first: boolean;
	@Input() last: boolean;
	@Output() sortChanged = new EventEmitter<DataColumn>();
	@Output() widthChanged = new EventEmitter<DataColumn>();


	constructor(public elementRef: ElementRef, private zone: NgZone) { }

	sortDirection = SortDirection;

	setSort(column: DataColumn, event: any) {
		if (!column.sortable) return;

		let maxIndex = -1;
		for (let col of this.parentGridView.getDataColumns()) {
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
	}

	private _origMove: any;
	private _origUp: any;
	private _origX: number;
	private _lockedColumns: Array<LockedColumn> = [];
	private _parentTH: any;
	private _origWidth: number;

	// we could set the column widths directly but that will cause grid to redraw which would
	// be expensive, so we'll wait until after
	protected startResize(evt: any) {
		if (this.elementRef.nativeElement.parentElement.nextElementSibling == null)
			return;

		this.elementRef.nativeElement.draggable = false;
		this._origX = evt.clientX;
		this._origMove = window.onmousemove;
		this._origUp = window.onmouseup;
		this._lockedColumns = [];
		this._parentTH = this.elementRef.nativeElement.parentElement;
		this._origWidth = this._parentTH.offsetWidth;
		var next = this._parentTH.nextElementSibling;
		while (next != null) {
			if (next.nextElementSibling == null) {
				// this is our floater
				if (next.style.width != "") {
					// this is a scenario (should be the only scenario) where a column's order was previously changed
					// then it was dragged to become the last, if this is the case we need to lock other columns and clear
					// this one out so it can float
					let tempPrev = next.previousElementSibling;
					while (tempPrev != null) {
						tempPrev.style.width = tempPrev.offsetWidth + 'px';
						tempPrev = tempPrev.previousElementSibling;
					}
				}
				next.style.width = "";
				break;
			}
			else {
				this._lockedColumns.push(new LockedColumn(next, next.offsetWidth));
				next = next.nextElementSibling;
			}
		}

		let prev = this._parentTH.previousElementSibling;
		while (prev != null) {
			this._lockedColumns.push(new LockedColumn(prev, prev.offsetWidth));
			prev = prev.previousElementSibling;
		}

		window.onmousemove = () => this.resize(event);
		window.onmouseup = () => this.endResize();
	}

	// TODO: test test test
	protected endResize() {
		window.onmousemove = this._origMove;
		window.onmouseup = this._origUp;
		this._origX = 0;
		this._origMove = null;
		this._origUp = null;
		if (this._resized) {
			this._resized = false;
			for (let col of this.parentGridView.columns) {
				if (col.getIdentifier() == this.elementRef.nativeElement.firstElementChild.id)
					col.width = this._parentTH.offsetWidth.toString() + 'px';
				else {
					for (let l of this._lockedColumns) {
						if (col.getIdentifier() == l.parentTH.children[0].children[0].id) {
							col.width = l.originalWidth.toString() + 'px';
						}
					}
				}
			}

			if (this.parentGridView.saveGridStateToStorage)
				this.parentGridView.saveGridState();

			this.widthChanged.emit(this.column)
		}
	}

	private _resized = false;
	private resize(event: any) {
		if (this._origX && event.buttons == 1) {
			let delta = event.clientX - this._origX;
			this._parentTH.style.width = (this._origWidth + delta).toString() + 'px';
			for (let locked of this._lockedColumns) {
				locked.parentTH.width = locked.originalWidth;
			}
			this._resized = true;
		}
		else {
			this.endResize();
		}
	}

	// IE has to be 'text' for some reason
	private COLUMN_ID = "text";
	protected dragOver(event) {
		if (!this.parentGridView.allowColumnOrdering) return;
		event.preventDefault();
	}

	protected dragStart(event) {
		if (!this.parentGridView.allowColumnOrdering) return;
		event.dataTransfer.setData(this.COLUMN_ID, event.currentTarget.id);
	}

	protected drop(event) {
		if (!this.parentGridView.allowColumnOrdering) return;
		let id = event.dataTransfer.getData(this.COLUMN_ID);
		this.changeColumnOrder(id, event.currentTarget.id);
	}

	private changeColumnOrder(sourceIdentifier: string, targetIdentifier: string) {
		let sourceCol: ColumnBase;
		let targetCol: ColumnBase;

		for (let col of this.parentGridView.columns) {
			if (col.getIdentifier() == sourceIdentifier) {
				sourceCol = col;
				break;
			}
		}

		for (let col of this.parentGridView.columns) {
			if (col.getIdentifier() == targetIdentifier) {
				targetCol = col;
				break;
			}
		}

		if (!sourceCol) throw sourceIdentifier + " not found!";
		if (!targetCol) throw targetIdentifier + " not found!";

		let targetIndex = targetCol.columnIndex;
		if (sourceCol.columnIndex <= targetCol.columnIndex) {
			for (let col of this.parentGridView.columns) {
				if (col.getIdentifier() == sourceCol.getIdentifier()) continue;
				if (col.columnIndex > sourceCol.columnIndex && col.columnIndex <= targetCol.columnIndex)
					col.columnIndex--;
			}
		}
		else {
			for (let col of this.parentGridView.columns) {
				if (col.getIdentifier() == sourceCol.getIdentifier()) continue;
				if (col.columnIndex < sourceCol.columnIndex && col.columnIndex >= targetCol.columnIndex)
					col.columnIndex++;
			}
		}
		sourceCol.columnIndex = targetIndex;

		// THIS SEEMS HACKISH! IN ORDER FOR THE COMPONENT TO REDRAW, IT NEEDS TO DETECT
		// A CHANGE TO THE COLUMNS VARIABLE ITSELF RATHER THAN WHAT'S IN THE COLLECTION
		let sortedColumns: Array<ColumnBase> = [];
		for (let c of this.parentGridView.columns) {
			sortedColumns.push(c);
		}

		this.parentGridView.columns = sortedColumns;
		if (this.parentGridView.saveGridStateToStorage)
			this.parentGridView.saveGridState();
	}
}

class LockedColumn {
	constructor(public parentTH: any, public originalWidth: any) { }
}