import { NgModule } from '@angular/core';
import { CheckListModule } from './checklist/checklist.module';
import { GridViewModule } from './gridview/gridview.module';
import { OverlayModule } from './overlay/overlay.module';
import { TypeAheadModule } from './typeahead/typeahead.module';

@NgModule({
	imports: [
		GridViewModule,
		OverlayModule,
		CheckListModule,
		TypeAheadModule
	]
})
export class PaJaMaModule { }