import { Component, Input } from '@angular/core';

@Component({
	moduleId: module.id,
	selector: 'expand-collapse',
	template: `
<div class='expand-collapse'>
    <div class='header-panel'>
		<strong>{{headerText}} </strong>
		<button (click)="collapsed = !collapsed"><span class="icon-small icon-button {{collapsed ? 'icon-plus-black' : 'icon-minus-black'}}"></span></button>
	</div>
	<div class="content {{collapsed ? 'content-collapsed' : 'content-expanded'}}">
		<ng-content></ng-content>
		<br /><br />
	</div>
</div>
`,
	styleUrls: ['expand-collapse.css']
})
export class ExpandCollapseComponent {
	@Input() collapsed = false;
	@Input() headerText: string;
}