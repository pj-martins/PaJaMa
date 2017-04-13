import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GridViewComponent } from './gridview.component';
import { DetailGridViewComponent } from './detail-gridview.component';
import { GridViewFilterCellComponent } from './gridview-filtercell.component';
import { GridViewFilterCellTemplateDirective } from './gridview-filtercell-template.directive';
import { GridViewCellComponent } from './gridview-cell.component';
import { GridViewCellTemplateDirective } from './gridview-cell-template.directive';
import { GridViewRowTemplateDirective } from './gridview-row-template.directive';
import { GridViewHeaderCellComponent } from './gridview-headercell.component';
import { GridViewPagerComponent } from './gridview-pager.component';
import { DateFilterComponent } from './gridview-datefilter.component';
import { GridViewSettingsComponent } from './gridview-settings.component';
import { ParserService } from '../services/parser.service';
import { PipesModule } from '../pipes/pipes.module';
import { TypeaheadModule } from '../typeahead/typeahead.module';
import { CheckListModule } from '../checklist/checklist.module';
import { DateTimePickerModule } from '../datetime-picker/datetime-picker.module';
import { ModalDialogModule } from '../modal-dialog/modal-dialog.module';

@NgModule({
    imports: [
		CommonModule,
		FormsModule,
		PipesModule,
		CheckListModule,
		TypeaheadModule,
		DateTimePickerModule,
		ModalDialogModule
    ],
    declarations: [
        GridViewComponent,
        GridViewCellComponent,
		GridViewFilterCellComponent,
		GridViewPagerComponent,
		GridViewSettingsComponent,
		GridViewHeaderCellComponent,
		GridViewRowTemplateDirective,
		GridViewCellTemplateDirective,
		GridViewFilterCellTemplateDirective,
		DetailGridViewComponent,
		DateFilterComponent
    ],
    exports: [
        GridViewComponent,
        GridViewCellComponent,
        GridViewFilterCellComponent,
		GridViewRowTemplateDirective,
		GridViewCellTemplateDirective,
		GridViewFilterCellTemplateDirective
    ]
})
export class GridViewModule { }