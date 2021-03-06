﻿import { Component, Input, OnInit, ElementRef } from '@angular/core';
import { DataColumn, FilterMode, GridView } from './gridview';
import { GridViewComponent } from './gridview.component';
import { CheckListModule } from '../checklist/checklist.module';

@Component({
	moduleId: module.id,
	selector: 'gridview-filtercell',
	styleUrls: ['gridview.css'],
	template: `
<div *ngIf='column.filterTemplate'>

</div>
<div *ngIf='!column.filterTemplate' [ngSwitch]='column.filterMode == filterMode.DistinctList || column.filterMode == filterMode.DynamicList || column.filterOptions'>
	<div *ngSwitchCase='true'>
		<check-list name='filtcheck' [ngStyle]="{'maxWidth': column.width || (parentWidth + 'px')}" [showFilterIcon]='true' [items]='checklistItems' [selectedItems]='column.filterValue' (selectionChanged)='filterChanged()'  class='filter-check-list'></check-list>
	</div>
	<div *ngSwitchDefault>
		<input type='text' [(ngModel)]='column.filterValue' (ngModelChange)='filterChanged()' class='form-control filter-control' />
	</div>
</div>
`
})
export class GridViewFilterCellComponent implements OnInit {
	@Input() column: DataColumn;
	@Input() parentGridView: GridView
	@Input() parentGridViewComponent: GridViewComponent;

	protected checklistItems = [];
	protected filterMode = FilterMode;

	constructor(private elementRef: ElementRef) { }

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
			this.column.filterValue = copy;
			this.column.filterOptionsChanged.subscribe(() => {
				this.checklistItems = this.column.filterOptions;
				let copy2 = [];
				if (this.checklistItems) {
					for (let ci of this.checklistItems) {
						copy2.push(ci);
					}
				}
				this.column.filterValue = copy2;
			});

			if (!this.column.width)
				this.parentWidth = this.elementRef.nativeElement.parentElement.offsetWidth;
		}

	}

	private _lastChange: Date;
	protected filterChanged() {
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
}