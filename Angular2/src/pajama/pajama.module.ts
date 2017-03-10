import { NgModule } from '@angular/core';
import { CheckListModule } from './checklist/checklist.module';
import { GridViewModule } from './gridview/gridview.module';
import { OverlayModule } from './overlay/overlay.module';
import { TypeaheadModule } from './typeahead/typeahead.module';

@NgModule({
	imports: [
		GridViewModule,
		OverlayModule,
		CheckListModule,
		TypeaheadModule
	]
})
export class PaJaMaModule { }