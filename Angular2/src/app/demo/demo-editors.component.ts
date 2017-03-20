import { Component } from '@angular/core';

@Component({
	moduleId: module.id,
	selector: 'demo-editors',
	templateUrl: './demo-editors.component.html'
})
export class DemoEditorsComponent {
	protected selectedDateTime: Date;
	protected selectedText: string;
	protected multiTextboxItems: Array<string> = ['Item 1', 'Item 2'];
	protected multiTypeaheadItems: Array<string> = [];
	protected dataSource: Array<string> = ['Alpha', 'Bravo', 'Charlie', 'Delta', 'Echo', 'Foxtrot', 'Tango', 'Zulu'];


	constructor() {
	}
}