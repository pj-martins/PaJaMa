import { NgModule } from '@angular/core';
import { CheckListModule } from './checklist/checklist.module';
import { GridViewModule } from './gridview/gridview.module';
import { OverlayModule } from './overlay/overlay.module';
import { TypeaheadModule } from './typeahead/typeahead.module';

export { CheckListModule } from './checklist/checklist.module';
export { CheckListComponent } from './checklist/checklist.component';
export { GridViewModule } from './gridview/gridview.module';
export { GridViewComponent } from './gridview/gridview.component';
export { ButtonColumn, CheckListColumn, ColumnBase, ColumnPipe, DataColumn, DetailGridView, DetailGridViewDataEventArgs, EditColumn, FilterMode, FieldType, GridView, GridViewTemplate, LinkColumn, PagingType, SelectMode, SortDirection as GridViewSortDirection, TypeaheadColumn } from './gridview/gridview';
export { MultiTextboxModule } from './multi-textbox/multi-textbox.module';
export { MultiTextboxComponent } from './multi-textbox/multi-textbox.component';
export { OverlayModule } from './overlay/overlay.module';
export { OverlayComponent } from './overlay/overlay.component';
export { TypeaheadModule } from './typeahead/typeahead.module';
export { TypeaheadComponent } from './typeahead/typeahead.component';
export { MultiTypeaheadComponent } from './typeahead/multi-typeahead.component';
export { BinaryFilter, DataService, FilterGroup, FilterOperator, FilterType, GetArguments, Items, ODataArguments, OrderBy, SortDirection as DataServiceSortDirection } from './services/data.service';
export { ParserService } from './services/parser.service';
export { PipesModule } from './pipes/pipes.module';
export { EnumToListPipe } from './pipes/enum-to-list.pipe';
export { MomentPipe } from './pipes/moment.pipe';
export { OrderByPipe } from './pipes/order-by.pipe';
export { ToCamelCasePipe } from './pipes/to-camel-case.pipe';

@NgModule({
	imports: [
		GridViewModule,
		OverlayModule,
		CheckListModule,
		TypeaheadModule
	]
})
export class PaJaMaModule { }