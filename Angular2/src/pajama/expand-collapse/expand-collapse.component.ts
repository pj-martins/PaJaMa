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
	<input type="checkbox" [checked]="!hidden" class="hidden-checkbox">
	<div class="expand content" [style.max-height]="maxHeight">
		<ng-content></ng-content>
	</div>
</div>
`,
	styleUrls: ['expand-collapse.css']
})
export class ExpandCollapseComponent {
	@Input() hidden = false;
	@Input() headerText: string;
	protected maxHeight = '10000px';
}