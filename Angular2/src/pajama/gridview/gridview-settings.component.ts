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
		<button (click)='helping = !helping' class='btn btn-default'><span class='glyphicon glyphicon-question-sign'></span> Help</button>
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
<div [hidden]='!helping' class='help-text'>
	<div class='help-text-header'>
		<div class='help-text-close-button' (click)='helping = false'><span class='glyphicon glyphicon-remove'></span></div>
	</div>
	<div class='help-text-body' [style.display]="helping ? 'block' : 'none'">
		<strong>Sorting Columns:</strong><br />
		To sort a column, click on the column's caption. If the column has not been sorted yet, it will sort ascending first, indicated by an arrow pointing down. 
		Clicking a second time will sort descending, indicated by an arrow pointing up.
		<br /><br />
		To sort multiple columns, after the first has been sorted, hold down the control key and select the second column you'd like to be sorted. 
		Keep the control key down when you want to toggle ascending to descending. To clear multiple sorts, release the control key and click on the desired single column to be sorted.
		<br /><br />
		<strong>Paging</strong><br />
		By default, the grid will show a maximum of 15 requests. If more than 15 are present, you may use the pager below the left of the grid. The page size may be changed, by selecting
		a new size in the drop down below the pager. Caution! If you select the "All" option and more than roughly 500 requests are shown, the application might load slower.
		<br /><br />
		<strong>Filtering</strong><br />
		If filtering is desired, you may check the "Filter" checkbox located above the grid to the right. This will toggle a filter row where you may apply desired filter. Only some columns allow filtering.
		<br /><br />
		<strong>Showing &amp; Hiding Columns</strong><br />
		To show or hide a column, open up the settings panel by clicking on the button labeled "Settings" and selecting "Customize Columns". A popup will appear where you may select or unselect
		columns to be shown or hidden.
		<br /><br />
		<strong>Resizing Columns</strong><br />
		Columns that are resizable will have a white bar to the right of the column's caption. To resize, click and hold the white bar to the right of the column you'd like to resize, and drag your cursor to the left or right. 
		<br /><br />
		<strong>Re-ordering Columns</strong><br />
		To re-order columns, click and hold the caption of the column you'd like to be re-ordered and drag the column on to another column where you'd like the dragged column to be inserted. If you drag to the left, the column will appear
		before the column it is dropped on. If you drag to the right it will appear after the column it is dropped on.
		<br /><br />
		<strong>Oh No!</strong><br />
		You may get yourself into a pickle where you've lost your way around and have gotten the grid into an undesired state, no worries! To reset the grid, click on the button labeled "Settings" and click on "Reset" grid to reset to the default state.
	</div>
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
