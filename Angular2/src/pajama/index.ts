﻿import { NgModule } from '@angular/core';
import { CheckListModule } from './checklist/checklist.module';
import { DateTimePickerModule } from './datetime-picker/datetime-picker.module';
import { ExpandCollapseModule } from './expand-collapse/expand-collapse.module';
import { GridViewModule } from './gridview/gridview.module';
import { MultiTextboxModule } from './multi-textbox/multi-textbox.module';
import { OverlayModule } from './overlay/overlay.module';
import { TypeaheadModule } from './typeahead/typeahead.module';
import { ModalDialogModule } from './modal-dialog/modal-dialog.module';
import { DisableEditDirective } from './disable-edit/disable-edit.directive';

export * from './checklist/checklist.module';
export * from './checklist/checklist.component';
export * from './datetime-picker/datetime-picker.module';
export * from './datetime-picker/datetime-picker.component';
export * from './expand-collapse/expand-collapse.module';
export * from './expand-collapse/expand-collapse.component';
export * from './gridview/gridview.module';
export * from './gridview/gridview.component';
export * from './gridview/gridview-templates.component';
export * from './gridview/gridview-cell-templates.component';
export * from './gridview/gridview';
export * from './treeview/treeview.module';
export * from './treeview/treeview.component';
export * from './treeview/treeview';
export * from './shared';
export * from './multi-textbox/multi-textbox.module';
export * from './multi-textbox/multi-textbox.component';
export * from './modal-dialog/modal-dialog.module';
export * from './modal-dialog/modal-dialog.component';
export * from './overlay/overlay.module';
export * from './overlay/overlay.component';
export * from './typeahead/typeahead.module';
export { TypeaheadComponent } from './typeahead/typeahead.component';
export * from './typeahead/multi-typeahead.component';
export * from './services/data.service';
export * from './services/parser.service';
export * from './pipes/pipes.module';
export * from './pipes/enum-to-list.pipe';
export * from './pipes/moment.pipe';
export * from './pipes/order-by.pipe';
export * from './pipes/to-camel-case.pipe';
export { DisableEditDirective } from './disable-edit/disable-edit.directive';

@NgModule({
	imports: [
		CheckListModule,
		DateTimePickerModule,
		ExpandCollapseModule,
		GridViewModule,
		MultiTextboxModule,
		OverlayModule,
		TypeaheadModule
	],
	declarations: [
		DisableEditDirective
	]
})
export class PaJaMaModule { }