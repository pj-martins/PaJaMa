import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ParserService } from '../pajama/services/parser.service';
import { AppComponent } from './app.component';
import { GridViewModule } from '../pajama/gridview/gridview.module';
import { CheckListModule } from '../pajama/checklist/checklist.module';
import { OverlayModule } from '../pajama/overlay/overlay.module';
import { TypeaheadModule } from '../pajama/typeahead/typeahead.module';
import { DateTimePickerModule } from '../pajama/datetime-picker/datetime-picker.module';
import { MultiTextboxModule } from '../pajama/multi-textbox/multi-textbox.module';
import { ExpandCollapseModule } from '../pajama/expand-collapse/expand-collapse.module';
import { PipesModule } from '../pajama/pipes/pipes.module';
import { routing } from './app.routing';
import { DemoGridComponent } from './demo/demo-grid.component';
import { DemoEditorsComponent } from './demo/demo-editors.component';

import { CoordinatorFilterCellTemplateComponent, CustomerCellTemplateComponent, EventTypeFilterCellTemplateComponent, RequestedByFilterCellTemplateComponent } from './demo/grid-cell-templates.component';
import { RoomComponent } from './demo/room.component';


import { SandboxComponent } from './sandbox/sandbox.component';
import { EvenValidatorComponent, EvenValidatorDirective } from './sandbox/even-validator.component';
import { TestComponent } from './sandbox/test.component';

@NgModule({
	imports: [
		BrowserModule,
		FormsModule,
		ReactiveFormsModule,
		HttpModule,
		GridViewModule,
		OverlayModule,
		TypeaheadModule,
		PipesModule,
		CheckListModule,
		DateTimePickerModule,
		MultiTextboxModule,
		ExpandCollapseModule,
		routing
	],
	providers: [ParserService],
	declarations: [
		AppComponent,
		DemoGridComponent,
		DemoEditorsComponent,

		SandboxComponent,
		EvenValidatorComponent,
		EvenValidatorDirective,

		CoordinatorFilterCellTemplateComponent,
		CustomerCellTemplateComponent,
		EventTypeFilterCellTemplateComponent,
		RequestedByFilterCellTemplateComponent,
		RoomComponent,

		TestComponent,
	],
	entryComponents: [
		CoordinatorFilterCellTemplateComponent,
		CustomerCellTemplateComponent,
		EventTypeFilterCellTemplateComponent,
		RequestedByFilterCellTemplateComponent,
		RoomComponent
	],
	bootstrap: [AppComponent]
})
export class AppModule { }