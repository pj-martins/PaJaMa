import { Component, Input, Output, EventEmitter } from '@angular/core';
import { GridView } from './gridview';

@Component({
	moduleId: module.id,
	selector: 'gridview-settings',
	styleUrls: ['gridview.css'],
	template: `
<div *ngIf='parentGridView.saveGridStateToStorage || parentGridView.allowColumnCustomization'>
	<div><span class='glyphicon glyphicon-gear'></span> Settings</div>
</div>
`
})
export class GridViewSettingsComponent {
	@Input() parentGridView: GridView;
}
