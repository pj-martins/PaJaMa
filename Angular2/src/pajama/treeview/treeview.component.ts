import { Component, Input, ViewChild } from '@angular/core';
import { TreeViewNode } from './treeview';
import { TreeViewNodeComponent } from './treeview-node.component';

@Component({
	moduleId: module.id,
	selector: 'treeview',
	styleUrls: ['../styles.css', 'treeview.css'],
	template: `
<div class='treeview component' *ngIf='nodes'>
	<treeview-node [nodes]='nodes'></treeview-node>
</div>
`
})
export class TreeViewComponent {
	@Input()
	nodes: Array<TreeViewNode>;

	@ViewChild(TreeViewNodeComponent)
	protected treeViewNodeComponent: TreeViewNodeComponent;

	expandAll() {
		this.treeViewNodeComponent.expandAll();
	}

	collapseAll() {
		this.treeViewNodeComponent.collapseAll();
	}
}