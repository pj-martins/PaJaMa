import { Component, Input } from '@angular/core';

@Component({
	moduleId: module.id,
	selector: 'expand-collapse',
	template: `
<div class='expand-collapse'>
    <div class='header-panel'>
		<strong>{{headerText}} </strong>
		<button (click)="hidden = !hidden"><span class="glyphicon {{hidden ? 'glyphicon-plus' : 'glyphicon-minus'}}"></span></button>
	</div>
	<div class="content {{hidden ? 'content-collapsed' : 'content-expanded'}}">
		<ng-content></ng-content>
		<br /><br />
	</div>
</div>
`,
	styleUrls: ['expand-collapse.css']
})
export class ExpandCollapseComponent {
	@Input() hidden = false;
	@Input() headerText: string;
}