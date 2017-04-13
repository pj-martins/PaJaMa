import { TreeViewNode, ITreeViewNodeTemplateComponent } from './treeview';

export abstract class TreeViewNodeTemplateComponent implements ITreeViewNodeTemplateComponent {
	node: TreeViewNode;
}
