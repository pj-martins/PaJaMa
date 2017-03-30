import { NgModule } from '@angular/core';
import { OverlayComponent } from './overlay.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [
		CommonModule,
        FormsModule
    ],
    declarations: [
        OverlayComponent
    ],
    exports: [
        OverlayComponent
    ]
})
export class OverlayModule { }
