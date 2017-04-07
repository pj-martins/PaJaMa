import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
	moduleId: module.id,
	selector: 'sandbox',
	template: `
<test-component class='my_class'></test-component>
`
})
export class SandboxComponent implements OnInit {
	
	ngOnInit() {
	}
}