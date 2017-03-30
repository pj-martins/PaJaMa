import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CheckListComponent } from './checklist.component';

@NgModule({
    imports: [
		CommonModule,
        FormsModule
    ],
    declarations: [
		CheckListComponent
    ],
    exports: [
		CheckListComponent
    ]
})
export class CheckListModule { }