import { Component, Input, Output, EventEmitter } from '@angular/core';
import { GridView } from './gridview';

@Component({
	moduleId: module.id,
	selector: 'gridview-settings',
	styleUrls: ['gridview-settings.css'],
	template: `
<div class='gridview-settings'>
	<div *ngIf='parentGridView.saveGridStateToStorage || parentGridView.allowColumnCustomization' class='dropup'>
		<button (click)='menuDown = !menuDown' class='btn btn-default'><span class='glyphicon glyphicon-cog'></span> Settings</button>
		<ul class='dropdown-menu' [style.display]="menuDown ? 'block' : 'none'">
			<li class='dropdown-item' (click)='customizeColumns()'><span class='glyphicon glyphicon-wrench'></span> Customize Columns</li>
			<li class='dropdown-item' *ngIf='parentGridView.saveGridStateToStorage' (click)='resetGridState()'><span class='glyphicon glyphicon-repeat'></span> Reset Grid</li>
		</ul>
	</div>
</div>
<div *ngIf='parentGridView' [hidden]='!customizingColumns' class='column-menu'>
	<div class='customize-header'>
		<div class='column-close-button' (click)='customizingColumns = false'><span class='glyphicon glyphicon-remove'></span></div>
	</div>
	<ul [style.display]="customizingColumns ? 'block' : 'none'">
		<li *ngFor='let col of parentGridView.columns' class='column-menu-item'>
			&nbsp;&nbsp;&nbsp;<input type='checkbox' [(ngModel)]='col.visible' (ngModelChange)='parentGridView.saveGridState()' />
			<span (click)='col.visible = !col.visible'>&nbsp;&nbsp;{{col.getCaption()}}</span>
		</li>
	</ul>
</div>
`
})
export class GridViewSettingsComponent {
	@Input() parentGridView: GridView;

	protected menuDown = false;
	protected customizingColumns = false;

	protected resetGridState() {
		this.parentGridView.resetGridState();
		this.menuDown = false;
	}

	protected customizeColumns() {
		this.customizingColumns = true;
		this.menuDown = false;
	}
}
