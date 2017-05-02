import { Component, Input, ViewChild } from '@angular/core';
import { GridView, ColumnBase, DataColumn } from './gridview';
import { ModalDialogComponent } from '../modal-dialog/modal-dialog.component';

@Component({
	moduleId: module.id,
	selector: 'gridview-settings',
	styleUrls: ['../assets/css/styles.css', '../assets/css/icons.css', '../assets/css/buttons.css', 'gridview-settings.css'],
	template: `
<div class='gridview-settings'>
	<div *ngIf='parentGridView.saveGridStateToStorage || parentGridView.allowColumnCustomization' class='dropup'>
		<button (click)='openCloseSettings($event)' class='btn btn-default icon-button'><span class='icon-gear-black icon-small'></span> Settings</button>
		<button (click)='helpModal.toggle()' class='btn btn-default icon-button'><span class='icon-question-black icon-small'></span> Help</button>
	</div>
</div>
<modal-dialog #settingsModal [showFooter]="false" [showHeader]="false">
	<div class='column-menu'>
		<ul class='settings-dropdown-menu'>
			<li class='settings-dropdown-item icon-button' (click)='customizeColumns()'><span class='icon-wrench-black icon-small'></span> Customize Columns</li>
			<li class='settings-dropdown-item icon-button' *ngIf='parentGridView.saveGridStateToStorage' (click)='resetGridState()'><span class='icon-refresh-black icon-small'></span> Reset Grid</li>
		</ul>
	</div>
</modal-dialog>
<modal-dialog #customizeModal *ngIf='parentGridView'>
	<div class='column-menu'>
		<ul>
			<li *ngFor='let col of parentGridView.columns' class='column-menu-item'>
				&nbsp;&nbsp;&nbsp;<input type='checkbox' [(ngModel)]='col.visible' (ngModelChange)='visibilityChanged(col)' />
				<span (click)='showHideColumn(col)'>&nbsp;&nbsp;{{col.getCaption()}}</span>
			</li>
		</ul>
	</div>
</modal-dialog>
<modal-dialog #helpModal>
	<div class='settings-body'>
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
		<div *ngIf="parentGridView.allowColumnCustomization">
			<strong>Showing &amp; Hiding Columns</strong><br />
			To show or hide a column, open up the settings panel by clicking on the button labeled "Settings" and selecting "Customize Columns". A popup will appear where you may select or unselect
			columns to be shown or hidden.
			<br /><br />
		</div>
		<div *ngIf="columnsSizable">
			<strong>Resizing Columns</strong><br />
			Columns that are resizable will have a white bar to the right of the column's caption. To resize, click and hold the white bar to the right of the column you'd like to resize, and drag your cursor to the left or right. 
			<br /><br />
		</div>
		<div *ngIf="parentGridView.allowColumnOrdering">
			<strong>Re-ordering Columns</strong><br />
			To re-order columns, click and hold the caption of the column you'd like to be re-ordered and drag the column on to another column where you'd like the dragged column to be inserted. If you drag to the left, the column will appear
			before the column it is dropped on. If you drag to the right it will appear after the column it is dropped on.
			<br /><br />
		</div>
		<strong>Oh No!</strong><br />
		You may get yourself into a pickle where you've lost your way around and have gotten the grid into an undesired state, no worries! To reset the grid, click on the button labeled "Settings" and click on "Reset" to reset to the default state.
	</div>
</modal-dialog>
`
})
export class GridViewSettingsComponent {

	@Input() parentGridView: GridView;

	@ViewChild("customizeModal") cutomizeModal: ModalDialogComponent;
	@ViewChild("settingsModal") settingsModal: ModalDialogComponent;

	protected columnsSizable = false;

	protected resetGridState() {
		this.parentGridView.resetGridState();
		this.settingsModal.hide();
	}

	protected customizeColumns() {
		this.cutomizeModal.show();
		this.settingsModal.hide();
	}

	protected visibilityChanged(col: ColumnBase) {
		if (col instanceof DataColumn && (<DataColumn>col).filterValue)
			this.parentGridView.dataChanged.emit(this.parentGridView);

		this.parentGridView.saveGridState();
	}

	protected showHideColumn(col: ColumnBase) {
		col.visible = !col.visible;
		this.visibilityChanged(col);
	}

	protected openCloseSettings(evt: any) {
		if (this.settingsModal.isShown) {
			this.settingsModal.hide();
		}
		else {
			this.columnsSizable = false;
			for (let col of this.parentGridView.columns) {
				if (col.allowSizing) {
					this.columnsSizable = true;
					break;
				}
			}
			this.settingsModal.show(null, 0, evt.clientX || evt.X, evt.clientY || evt.Y);
		}
	}
}
