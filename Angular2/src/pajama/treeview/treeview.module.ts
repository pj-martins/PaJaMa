import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TreeViewComponent } from './treeview.component';
import { TreeViewNodeComponent } from './treeview-node.component';
import { TreeViewNodeTemplateDirective } from './treeview-node-template.directive';

@NgModule({
    imports: [
		CommonModule,
		FormsModule
    ],
    declarations: [
		TreeViewComponent,
		TreeViewNodeComponent,
		TreeViewNodeTemplateDirective
    ],
    exports: [
		TreeViewComponent,
		TreeViewNodeComponent,
		TreeViewNodeTemplateDirective
    ]
})
export class TreeViewModule { }