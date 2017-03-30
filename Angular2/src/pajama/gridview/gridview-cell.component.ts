import { Component, Input } from '@angular/core';
import { GridView, DataColumn, FieldType } from './gridview';
import { GridViewComponent } from './gridview.component';
import { PipesModule } from '../pipes/pipes.module';
import { ParserService } from '../services/parser.service';

@Component({
	moduleId: module.id,
	selector: 'gridview-cell',
	styleUrls: ['gridview.css'],
	template: `
<div *ngIf="column.template">
	<gridview-cell-template [column]="column" [row]="row" [parentGridViewComponent]="parentGridViewComponent" [parentGridView]="parentGridView"></gridview-cell-template>
</div>
<div *ngIf="!column.template && column.fieldType == fieldType.Date">
	<div [innerHTML]="getObjectValue() == null ? '' : getObjectValue() | moment:(column.format ? column.format : 'MM/DD/YYYY')"></div>
</div>
<div *ngIf="!column.template && !column.format && column.fieldType == fieldType.Boolean">
	<div [ngClass]="{ 'glyphicon glyphicon-ok' : getObjectValue(false) == true }"></div>
</div>
<!-- TODO: should we allow links to above items? duplication here too -->
<div *ngIf="column.fieldType != fieldType.Date && column.fieldType != fieldType.Boolean && !column.template && !column.format">
	<div *ngIf="column.url">
		<!-- TODO: always open in new window? -->
		<a href='{{getLink(col, row)}}' class="{{column.class}}" target='{{getLinkTarget(col, row)}}'>
			<div [innerHTML]="getObjectValue('')"></div>
		</a>
	</div>
	<div *ngIf="column.click">
		<button class="{{column.class}}" (click)="column.click.emit(row)">{{getObjectValue('')}}</button>
	</div>
	<div *ngIf="column.editType" [ngSwitch]="column.editType">
		<div *ngSwitchCase="'textarea'">
			<textarea [style.width]="column.width" class="{{column.class}}" (ngModelChange)="column.ngModelChange.emit(row)" [(ngModel)]="row[column.fieldName]" [minlength]="column.min" [maxlength]="column.max"></textarea>
		</div>
		<div *ngSwitchCase="'checkbox'">
			<input type="checkbox" [style.width]="column.width" class="{{column.class}}" (ngModelChange)="column.ngModelChange.emit(row)" [(ngModel)]="row[column.fieldName]" />
		</div>
		<!-- for some reason below can't handle checkbox -->
		<div *ngSwitchDefault>
			<input type="{{column.editType}}" [style.width]="column.width" class="{{column.class}}" (ngModelChange)="column.ngModelChange.emit(row)" [(ngModel)]="row[column.fieldName]" [min]="column.min" [max]="column.max" [minlength]="column.min" [maxlength]="column.max" />
		</div>
	</div>
	<div *ngIf="column.checkList">
		<div [style.width]="column.width" style='word-break: break-word'><input type="text" [dataSource]='column.items' [checkList]='row[column.fieldName]' [displayMember]="column.displayMember" /></div>
	</div>
<!-- TODO:
	<div *ngIf="column.typeahead">
		<div *ngIf="column.multi">
			<div [style.width]="column.width"><multi-typeahead (ngModelChange)='column.ngModelChange.emit(row)' [items]='column.dataSource' [(ngModel)]='row[column.fieldName]'></multi-typeahead></div>
		</div>
	</div>
-->
	<div *ngIf="column.decimalPlaces !== undefined">
		<div style='text-align: right;padding-right:5px;' [innerHTML]="getObjectValue('') | number : '1.' + column.decimalPlaces + '-' + column.decimalPlaces"></div>
	</div>
	<div *ngIf="!column.url && !column.click && !column.editType && !column.checkList && !column.typeahead && column.decimalPlaces === undefined">
		<div [innerHTML]="getObjectValue('')"></div>
	</div>
</div>
`
})
export class GridViewCellComponent {
	@Input() column: DataColumn;
	@Input() row: any;

	@Input() parentGridViewComponent: GridViewComponent;
	@Input() parentGridView: GridView;

	protected fieldType = FieldType;

	constructor(protected parserService: ParserService) { }

	protected getObjectValue(def: any = null) {
		let val = this.parserService.getObjectValue(this.column.fieldName, this.row);
		if (val == null) return def;
		if (this.column.columnPipe) {
			console.log(val);
			val = this.column.columnPipe.pipe.transform(val, this.column.columnPipe.args);
		}
		return val;
	}
}