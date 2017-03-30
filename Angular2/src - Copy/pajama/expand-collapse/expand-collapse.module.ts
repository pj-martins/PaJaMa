import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExpandCollapseComponent } from './expand-collapse.component';

@NgModule({
    imports: [
		CommonModule,
        FormsModule
    ],
    declarations: [
		ExpandCollapseComponent
    ],
    exports: [
		ExpandCollapseComponent
    ]
})
export class ExpandCollapseModule { }