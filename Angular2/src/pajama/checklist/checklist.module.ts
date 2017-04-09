import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CheckListComponent } from './checklist.component';
//import { CheckListDirective } from './checklist.directive';

@NgModule({
    imports: [
		CommonModule,
        FormsModule
    ],
    declarations: [
		CheckListComponent,
		//CheckListDirective
    ],
    exports: [
		CheckListComponent,
		//CheckListDirective
	],
	entryComponents: [
		CheckListComponent
	]
})
export class CheckListModule { }