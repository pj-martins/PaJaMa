import { Component, Input } from '@angular/core';

@Component({
	moduleId: module.id,
	selector: 'test-component',
	template: `<input type='text' class="{{class}}" />`
})
export class TestComponent {
	@Input() class: string;
}