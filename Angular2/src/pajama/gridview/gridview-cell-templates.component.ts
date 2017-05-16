import { Component, Input } from '@angular/core';
import { IGridViewCellTemplateComponent, DataColumn, GridView } from './gridview';
import { GridViewComponent } from './gridview.component';

@Component({
	moduleId: module.id,
	selector: 'gridview-textarea-cell',
	styleUrls: ['gridview.css'],
	template: `
<textarea style="width:100%" rows="5" [(ngModel)]="row[column.fieldName]" (ngModelChange)="parentGridView.cellValueChanged.emit(self)"></textarea>
`
})
export class GridViewTextAreaCellComponent implements IGridViewCellTemplateComponent {
	@Input() column: DataColumn;
	@Input() row: any;
	@Input() parentGridViewComponent: GridViewComponent;
	@Input() parentGridView: GridView;

	protected self = this;
}