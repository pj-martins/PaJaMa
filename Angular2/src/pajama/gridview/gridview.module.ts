import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GridViewComponent } from './gridview.component';
import { DetailGridViewComponent } from './detail-gridview.component';
import { GridViewRowTemplateBuilder, GridViewRowTemplateComponent } from './gridview.rowtemplate';
import { GridViewFilterCellComponent } from './gridview-filtercell.component';
import { GridViewFilterCellTemplateComponent } from './gridview-filtercell-template.component';
import { GridViewFilterCellTemplateBuilder } from './gridview-filtercell-template.component';
import { GridViewCellComponent } from './gridview-cell.component';
import { GridViewCellTemplateBuilder } from './gridview-cell-template.component';
import { GridViewCellTemplateComponent } from './gridview-cell-template.component';
import { GridViewHeaderCellComponent } from './gridview-headercell.component';
import { GridViewPagerComponent } from './gridview-pager.component';
import { GridViewSettingsComponent } from './gridview-settings.component';
import { ParserService } from '../services/parser.service';
import { PipesModule } from '../pipes/pipes.module';
import { TypeaheadModule } from '../typeahead/typeahead.module';
import { CheckListModule } from '../checklist/checklist.module';

@NgModule({
    imports: [
		CommonModule,
		FormsModule,
		PipesModule,
		CheckListModule,
		TypeaheadModule
    ],
    declarations: [
        GridViewComponent,
        GridViewCellComponent,
		GridViewFilterCellComponent,
		GridViewPagerComponent,
		GridViewSettingsComponent,
		GridViewHeaderCellComponent,
		GridViewRowTemplateComponent,
		GridViewCellTemplateComponent,
		GridViewFilterCellTemplateComponent,
        DetailGridViewComponent
    ],
    exports: [
        GridViewComponent,
        GridViewCellComponent,
        GridViewFilterCellComponent,
		GridViewRowTemplateComponent,
		GridViewCellTemplateComponent,
		GridViewFilterCellTemplateComponent
    ],
	providers: [GridViewRowTemplateBuilder, GridViewCellTemplateBuilder, GridViewFilterCellTemplateBuilder]
})
export class GridViewModule { }