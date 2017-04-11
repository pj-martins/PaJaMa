import { Component, OnInit, Type, ViewChild } from '@angular/core';
import { Event, Customer } from '../classes/classes';
import { RoomComponent } from './room.component';
import { TreeNode } from 'pajama/treeview/treeview';
import { TreeViewComponent } from 'pajama/treeview/treeview.component';
import { RoomNodeTemplateComponent } from './treeview-templates.component';
import { Observable } from 'rxjs/Observable';
import * as moment from 'moment';

declare var EVENTS: Array<Event>;

@Component({
	moduleId: module.id,
	selector: 'demo-tree',
	template: `
<treeview [nodes]='nodes'></treeview>
<br /><br />
<button (click)='treeViewComponent.expandAll()'>Expand All</button>
<button (click)='treeViewComponent.collapseAll()'>Collapse All</button>
`
})
export class DemoTreeComponent implements OnInit {
	protected nodes: Array<TreeNode>;

	@ViewChild(TreeViewComponent)
	treeViewComponent: TreeViewComponent;

	ngOnInit() {
		this.nodes = new Array<TreeNode>();
		for (let i = 0; i < EVENTS.length; i++) {
			let e = EVENTS[i];

			let node: TreeNode;
			for (let curr of this.nodes) {
				if (curr.text == e.customer.customerName) {
					node = curr;
					break;
				}
			}

			if (node == null) {
				node = new TreeNode();
				node.dataItem = e;
				node.text = e.customer.customerName;
				node.childNodes = new Array<TreeNode>();
				this.nodes.push(node);
			}

			let eventNode = new TreeNode();
			eventNode.dataItem = e;
			eventNode.class = "event-node";
			eventNode.text = moment(e.eventStartDT).format("MM/DD/YYYY") + " - " + moment(e.eventEndDT).format("MM/DD/YYYY") + " " + e.requestedBy;

			// synchronous
			if (i < 5) {
				eventNode.childNodes = new Array<TreeNode>();
				for (let r of e.hallRequestRooms) {
					let childNode = new TreeNode();
					childNode.dataItem = r;
					childNode.text = r.hallRoom.roomName;
					childNode.template = RoomNodeTemplateComponent;
					eventNode.childNodes.push(childNode);
				}
			}
			// asynchronous
			else if (i < 10) {
				eventNode.getChildNodes = (parent: any) => {
					let e = <Event>parent;
					let childNodes = new Array<TreeNode>();
					for (let r of e.hallRequestRooms) {
						let childNode = new TreeNode();
						childNode.dataItem = r;
						childNode.text = r.hallRoom.roomName;
						childNode.template = RoomNodeTemplateComponent;
						childNodes.push(childNode);
					}

					return Observable.create(o => o.next(childNodes));
				}
			}

			node.childNodes.push(eventNode);
		}
	}
}