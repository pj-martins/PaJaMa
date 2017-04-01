import { GridView, DataColumn, IGridViewFilterCellTemplateComponent, IGridViewComponent, IGridViewCellTemplateComponent, IGridViewRowTemplateComponent } from './gridview';

export abstract class GridViewFilterCellTemplateComponent implements IGridViewFilterCellTemplateComponent {
	column: DataColumn;
	parentGridView: GridView;
	parentFilterCellComponent: IGridViewFilterCellTemplateComponent;
}

export abstract class GridViewCellTemplateComponent implements IGridViewCellTemplateComponent {
	column: DataColumn;
	parentGridView: GridView;
	parentGridViewComponent: IGridViewComponent;
	row: any;
}

export abstract class GridViewRowTemplateComponent implements IGridViewRowTemplateComponent {
	parentGridView: GridView;
	row: any;
	parentGridViewComponent: IGridViewComponent;
}
