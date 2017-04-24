﻿import { Component, Input } from '@angular/core';
import { GridView, DataColumn, FieldType } from './gridview';
import { GridViewComponent } from './gridview.component';
import { PipesModule } from '../pipes/pipes.module';
import { ParserService } from '../services/parser.service';

@Component({
	moduleId: module.id,
	selector: 'gridview-cell',
	styleUrls: ['gridview.css'],
	template: `
<div *ngIf="!editing && column.template">
	<div [gridviewCellTemplate]="column.template" [column]="column" [row]="row" [parentGridViewComponent]="parentGridViewComponent" [parentGridView]="parentGridView"></div>
</div>
<div *ngIf="!column.template && (!editing || !column.editTemplate)">
	<div *ngIf="column.fieldType == fieldType.Date">
		<div *ngIf="!editing || column.readonly" 
				[innerHTML]="getObjectValue() == null ? '' : getObjectValue() | moment:(column.format ? column.format : 'MM/DD/YYYY')"></div>
		<!-- TODO: determine when to hide time/date -->
		<input type="text" dateTimePicker style="width:100%" *ngIf="(parentGridView.allowEdit || parentGridView.allowAdd) && !column.readonly &&  editing" [(ngModel)]="row[column.fieldName]" />
		<div class="error-label" *ngIf="!row[column.fieldName] && showRequired">{{column.caption}} is required!</div>
	</div>
	<div *ngIf="!column.format && column.fieldType == fieldType.Boolean">
		<div *ngIf="!editing || column.readonly" [ngClass]="{ 'glyphicon glyphicon-ok' : getObjectValue(false) == true }"></div>
		<input type="checkbox" *ngIf="(parentGridView.allowEdit || parentGridView.allowAdd) && !column.readonly && editing" [(ngModel)]="row[column.fieldName]" />
	</div>
	<div *ngIf="column.click">
		<button class="{{column.class}}" (click)="column.click.emit(row)">{{getObjectValue('')}}</button>
	</div>
	<!-- TODO: should we allow links to above items? duplication here too -->
	<div *ngIf="column.fieldType != fieldType.Date && column.fieldType != fieldType.Boolean && !column.format && !column.click">
		<div *ngIf="!editing || column.readonly" [innerHTML]="getObjectValue('')"></div>
		<input type="text" style="width:100%" *ngIf="(parentGridView.allowEdit || parentGridView.allowAdd) && !column.readonly && editing" [(ngModel)]="row[column.fieldName]" />
		<div class="error-label" *ngIf="!row[column.fieldName] && showRequired">{{column.caption}} is required!</div>
	</div>
</div>
<div *ngIf="editing && column.editTemplate">
	<div [gridviewCellTemplate]="column.editTemplate" [column]="column" [row]="row" [parentGridViewComponent]="parentGridViewComponent" [parentGridView]="parentGridView"></div>
</div>
`
})
export class GridViewCellComponent {
	@Input() column: DataColumn;
	@Input() row: any;

	@Input() parentGridViewComponent: GridViewComponent;
	@Input() parentGridView: GridView;
	@Input() first: boolean;
	@Input() last: boolean;

	@Input() index: number;

	protected fieldType = FieldType;

	protected get editing(): boolean {
		return this.parentGridViewComponent.editingRows[this.row[this.parentGridViewComponent.grid.keyFieldName]];
	}

	protected get showRequired(): boolean {
		return this.parentGridViewComponent.showRequired[this.row[this.parentGridViewComponent.grid.keyFieldName]];
	}

	constructor(protected parserService: ParserService) { }

	protected getObjectValue(def: any = null) {
		let val = this.parserService.getObjectValue(this.column.fieldName, this.row);
		if (val == null) return def;
		if (this.column.columnPipe) {
			val = this.column.columnPipe.pipe.transform(val, this.column.columnPipe.args);
		}
		return val;
	}
}