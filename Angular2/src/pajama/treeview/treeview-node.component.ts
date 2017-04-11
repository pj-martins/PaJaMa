import { Component, Input } from '@angular/core';
import { TreeNode } from './treeview';

@Component({
	moduleId: module.id,
	selector: 'treeview-node',
	styleUrls: ['../styles.css', 'treeview.css'],
	template: `
<div *ngFor='let n of nodes'>
	<div class='treenode'>
		<div class='treenode-button'>
			<button class='button square-button' *ngIf='n.setChildNodes || (n.childNodes && n.childNodes.length > 0)' (click)='expandCollapseNode(n)'>
				<div class='icon-text'>{{n.isExpanded ? '-' : '+'}}</div>
			</button>
		</div>
		<div class='treenode-content'>
			<div *ngIf="n.template">
				<div treeviewNodeTemplate [node]="n"></div>
			</div>
			<div *ngIf="!n.template" class="{{n.class}}">
				{{n.text}}
			</div>
			<div *ngIf='n.isExpanded' class='treenode-child'>
				<treeview-node [nodes]='n.childNodes'></treeview-node>
			</div>
		</div>
	</div>
</div>
`
})
export class TreeViewNodeComponent {
	@Input()
	nodes: Array<TreeNode>;

	protected expandCollapseNode(node: TreeNode) {
		node.isExpanded = !node.isExpanded;
		if (node.isExpanded)
			this.expandNode(node);
	}

	private expandNode(node: TreeNode, andChildren = false) {
		if (node.getChildNodes && !node.childNodes) {
			node.getChildNodes(node.dataItem).subscribe(ns => {
				node.childNodes = ns;
				if (andChildren) {
					for (let childNode of node.childNodes) {
						childNode.isExpanded = node.isExpanded;
						this.expandNode(childNode, andChildren);
					}
				}
			});
		}
		else if (node.childNodes && andChildren) {
			for (let childNode of node.childNodes) {
				childNode.isExpanded = node.isExpanded;
				this.expandNode(childNode, andChildren);
			}
		}
	}

	expandAll() {
		for (let node of this.nodes) {
			node.isExpanded = true;
			this.expandNode(node, true);
		}
	}

	private recursivelyCollapseAll(nodes: Array<TreeNode>) {
		for (let node of nodes) {
			node.isExpanded = false;
			if (node.childNodes)
				this.recursivelyCollapseAll(node.childNodes);
		}
	}

	collapseAll() {
		this.recursivelyCollapseAll(this.nodes);
	}
}