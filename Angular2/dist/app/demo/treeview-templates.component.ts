import { Component, OnInit } from '@angular/core';
import { TreeViewNodeTemplateComponent } from 'pajama/treeview/treeview-node-template.component';

@Component({
	moduleId: module.id,
	selector: 'room-node-template',
	template: `
<strong>{{node.text}}</strong> - (ID: <a href='javascript:void(0)' (click)='processID()'>{{node.dataItem.id}}</a>)
`
})
export class RoomNodeTemplateComponent extends TreeViewNodeTemplateComponent {
	protected processID() {
		alert(this.node.dataItem.id);
	}
}