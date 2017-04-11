import { Component, Input, ViewChild } from '@angular/core';
import { TreeNode } from './treeview';
import { TreeViewNodeComponent } from './treeview-node.component';

@Component({
	moduleId: module.id,
	selector: 'treeview',
	styleUrls: ['../styles.css', 'treeview.css'],
	template: `
<div class='treeview' *ngIf='nodes'>
	<treeview-node [nodes]='nodes'></treeview-node>
</div>
`
})
export class TreeViewComponent {
	@Input()
	nodes: Array<TreeNode>;

	@ViewChild(TreeViewNodeComponent)
	protected treeViewNodeComponent: TreeViewNodeComponent;

	expandAll() {
		this.treeViewNodeComponent.expandAll();
	}

	collapseAll() {
		this.treeViewNodeComponent.collapseAll();
	}
}