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
	<div gridviewCellTemplate [column]="column" [row]="row" [parentGridViewComponent]="parentGridViewComponent" [parentGridView]="parentGridView"></div>
</div>
<div *ngIf="!column.template && column.fieldType == fieldType.Date">
	<div [innerHTML]="getObjectValue() == null ? '' : getObjectValue() | moment:(column.format ? column.format : 'MM/DD/YYYY')"></div>
</div>
<div *ngIf="!column.template && !column.format && column.fieldType == fieldType.Boolean">
	<div [ngClass]="{ 'glyphicon glyphicon-ok' : getObjectValue(false) == true }"></div>
</div>
<!-- TODO: should we allow links to above items? duplication here too -->
<div *ngIf="column.fieldType != fieldType.Date && column.fieldType != fieldType.Boolean && !column.template && !column.format">
	<div [innerHTML]="getObjectValue('')"></div>
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