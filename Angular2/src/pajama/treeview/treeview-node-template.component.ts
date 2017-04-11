import { TreeNode, ITreeViewNodeTemplateComponent } from './treeview';

export abstract class TreeViewNodeTemplateComponent implements ITreeViewNodeTemplateComponent {
	node: TreeNode;
}
