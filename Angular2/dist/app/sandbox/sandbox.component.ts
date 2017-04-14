import { Component, Input, OnInit } from '@angular/core';

@Component({
	moduleId: module.id,
	selector: 'sandbox',
	template: `
<button class='btn btn-small'>Regular SM</button>&nbsp;
<button class='btn'>Regular</button>&nbsp;
<button class='btn btn-large'>Regular LRG</button>&nbsp;
<button class='btn btn-xlarge'>Regular XL</button>
<br />
<br />
<button class='btn btn-small btn-default'>White SM</button>&nbsp;
<button class='btn btn-default'>White</button>&nbsp;
<button class='btn btn-large btn-default'>White LRG</button>&nbsp;
<button class='btn btn-xlarge btn-default'>White XL</button>
<br />
<br />
<button class='btn btn-small btn-primary'>Blue SM</button>&nbsp;
<button class='btn btn-primary'>Blue</button>&nbsp;
<button class='btn btn-large btn-primary'>Blue LRG</button>&nbsp;
<button class='btn btn-xlarge btn-primary'>Blue XL</button>
<br />
<br />
<button class='btn btn-small btn-danger'>Red SM</button>&nbsp;
<button class='btn btn-danger'>Red</button>&nbsp;
<button class='btn btn-large btn-danger'>Red LRG</button>&nbsp;
<button class='btn btn-xlarge btn-danger'>Red XL</button>
`
})
export class SandboxComponent {
	
}