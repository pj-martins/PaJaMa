import { Component, Input } from '@angular/core';
import { TreeViewNode } from './treeview';

@Component({
	moduleId: module.id,
	selector: 'treeview-node',
	styleUrls: ['../styles.css', 'treeview.css'],
	template: `
<div *ngFor='let n of nodes'>
	<div class='treenode'>
		<div class="expandcollapse-button-container">
		  <button class="expandcollapse-button" *ngIf='n.setChildNodes || (n.childNodes && n.childNodes.length > 0)' (click)='expandCollapseNode(n)'>
			<div class="expandcollapse-horizontal"></div>
			<div class="expandcollapse-vertical" *ngIf='!n.isExpanded'></div>
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
	nodes: Array<TreeViewNode>;

	protected expandCollapseNode(node: TreeViewNode) {
		node.isExpanded = !node.isExpanded;
		if (node.isExpanded)
			this.expandNode(node);
	}

	private expandNode(node: TreeViewNode, andChildren = false) {
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

	private recursivelyCollapseAll(nodes: Array<TreeViewNode>) {
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