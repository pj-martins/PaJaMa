import { Component } from '@angular/core';

@Component({
	moduleId: module.id,
	selector: 'demo-editors',
	templateUrl: './demo-editors.component.html'
})
export class DemoEditorsComponent {
	protected demoDateTime: Date;

	constructor() {
	}
}