import { EventEmitter, Type } from '@angular/core';
import { Observable } from 'rxjs/Observable'

export class TreeViewNode {
	dataItem: any;
	text: string;
	class: string;
	isExpanded = false;

	childNodes: Array<TreeViewNode>;
	getChildNodes: (parent: any) => Observable<Array<TreeViewNode>>;

	template: Type<ITreeViewNodeTemplateComponent>;

}

export interface ITreeViewNodeTemplateComponent {
	node: TreeViewNode;
}