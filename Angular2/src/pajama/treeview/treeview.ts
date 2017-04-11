import { EventEmitter, Type } from '@angular/core';
import { Observable } from 'rxjs/Observable'

export class TreeNode {
	dataItem: any;
	text: string;
	isExpanded = false;

	childNodes: Array<TreeNode>;
	getChildNodes: (parent: any) => Observable<Array<TreeNode>>;

	template: Type<ITreeViewNodeTemplateComponent>;

}

export interface ITreeViewNodeTemplateComponent {
	node: TreeNode;
}