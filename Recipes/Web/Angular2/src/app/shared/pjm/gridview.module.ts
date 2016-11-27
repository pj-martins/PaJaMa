import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GridViewComponent } from './gridview.component';
import { DetailGridViewComponent } from './detail-gridview.component';
import { GridViewRowTemplateBuilder, GridViewRowTemplateComponent } from './gridview.rowtemplate';
import { GridViewFilterCellComponent, GridViewFilterCellTemplateBuilder } from './gridview.filtercell';
import { ColumnCaptionPipe } from './column-caption.pipe';
import { GridViewCellTemplateBuilder, GridViewCellComponent } from './gridview.cell';
import { ParserService } from './parser.service';
import { MomentPipe } from './moment.pipe';

@NgModule({
	imports: [
		CommonModule,
		FormsModule
	],
	declarations: [
		GridViewComponent,
		GridViewCellComponent,
		GridViewFilterCellComponent,
		GridViewRowTemplateComponent,
		DetailGridViewComponent,
		ColumnCaptionPipe,
		MomentPipe
	],
	exports: [
		GridViewComponent,
		GridViewCellComponent,
		GridViewFilterCellComponent,
		GridViewRowTemplateComponent
	],
	providers: [GridViewCellTemplateBuilder, GridViewRowTemplateBuilder, GridViewFilterCellTemplateBuilder]
})
export class GridViewModule { }