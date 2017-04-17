import { Component, Input, OnInit, ElementRef } from '@angular/core';
import { DataColumn, FilterMode, GridView, IGridViewFilterCellComponent, FieldType } from './gridview';
import { GridViewComponent } from './gridview.component';
import { CheckListModule } from '../checklist/checklist.module';

@Component({
	moduleId: module.id,
	selector: 'gridview-filtercell',
	styleUrls: ['../assets/css/styles.css', '../assets/css/icons.css', '../assets/css/buttons.css', 'gridview-filtercell.css'],
	template: `
<div class='gridview-filtercell'>
	<div class='gridview-filtercell-content'>
		<div *ngIf='column.filterTemplate'>
			<div gridviewFilterCellTemplate [parentFilterCellComponent]="self" [column]="column"></div>
		</div>
		<div *ngIf='!column.filterTemplate'>
			<div *ngIf='column.filterMode == filterMode.DistinctList || column.filterMode == filterMode.DynamicList || column.filterOptions'>
				<checklist type='text' name='filtcheck' [showFilterIcon]='true' [dataSource]='checklistItems' [selectedItems]='column.filterValue' (selectionChanged)='filterChanged()'  class='filter-check-list filtercell-textbox'></checklist>
			</div>
			<div *ngIf='column.filterMode == filterMode.DateRange'>
				<datefilter [column]='column' [parentFilterCellComponent]="self" [parentGridView]="parentGridView"></datefilter>
			</div>
			<div *ngIf='column.filterMode != filterMode.DistinctList && column.filterMode != filterMode.DynamicList && !column.filterOptions && column.filterMode != filterMode.DateRange'>
				<input type='text' [(ngModel)]='column.filterValue' (ngModelChange)='filterChanged()' class="filtercell-textbox" />
			</div>
		</div>
	</div>
	<div class='gridview-filtercell-clear'>
		<div class='clickable icon-remove-black icon-x-small' (click)='clearFilter()'>
		</div>
	</div>
</div>
`
})
export class GridViewFilterCellComponent implements OnInit, IGridViewFilterCellComponent {
	@Input() column: DataColumn;
	@Input() parentGridView: GridView
	@Input() parentGridViewComponent: GridViewComponent;

	protected self = this;
	protected checklistItems = [];
	protected filterMode = FilterMode;

	constructor(protected elementRef: ElementRef) { }

	protected parentWidth = 0;

	ngOnInit() {
		if (this.column.filterMode == FilterMode.DistinctList || this.column.filterMode == FilterMode.DynamicList || this.column.filterOptions) {
			this.checklistItems = this.column.filterOptions || [];
			let copy = [];
			if (this.checklistItems) {
				for (let ci of this.checklistItems) {
					copy.push(ci);
				}
			}
			if (!this.column.filterValue || this.column.filterValue.length == 0)
				this.column.filterValue = copy;

			this.column.filterOptionsChanged.subscribe(() => {
				this.checklistItems = this.column.filterOptions;
				let copy2 = [];
				if (this.checklistItems) {
					for (let ci of this.checklistItems) {
						copy2.push(ci);
					}
				}
				if (!this.column.filterValue || this.column.filterValue.length == 0)
					this.column.filterValue = copy2;
			});

			if (!this.column.width)
				this.parentWidth = this.elementRef.nativeElement.parentElement.offsetWidth;
		}

	}

	private _lastChange: Date;
	filterChanged() {
		if (this.column.filterDelayMilliseconds > 0) {
			this._lastChange = new Date();
			window.setTimeout(() => {
				let now = new Date();
				if (now.getTime() - this._lastChange.getTime() >= this.column.filterDelayMilliseconds - 1) {
					this.fireFilter();
				}
			}, this.column.filterDelayMilliseconds);
		}
		else {
			this.fireFilter();
		}
		this.parentGridView.saveGridState();
	}

	private fireFilter() {
		this.parentGridView.currentPage = 1;
		this.parentGridView.dataChanged.emit(this.parentGridView);
		this.parentGridViewComponent.filterChanged.emit(this.column);
	}

	clearFilter() {
		this.column.filterValue = null;
		if (this.checklistItems && this.checklistItems.length > 0) {
			let copy = [];
			for (let ci of this.checklistItems) {
				copy.push(ci);
			}
			this.column.filterValue = copy;
		}


		this.filterChanged();
	}
}