import { Component, ViewChild } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ModalDialogComponent, DialogResult, Button } from 'pajama/modal-dialog/modal-dialog.component';

@Component({
	moduleId: module.id,
	selector: 'demo-modal',
	template: `
<button (click)='basicModal.show()'>Basic Modal</button>
<modal-dialog #basicModal headerText='Basic' bodyContent='Here is a basic modal'>
</modal-dialog>

<button (click)='showYesNoModal()'>Yes No Modal</button>
<modal-dialog #yesNoModal [hideCloseButton]='true' [showBackdrop]='true' headerText='Yes No Modal'>
	<div style="padding:15px:">
		<strong>This is a yes no modal</strong>
	</div>
</modal-dialog>

{{statusText}}
`
})
export class DemoModalComponent {
	@ViewChild("yesNoModal")
	protected yesNoModal: ModalDialogComponent;

	protected statusText = "";

	protected showYesNoModal() {
		this.yesNoModal.show(Button.YesNo).subscribe(d => {
			this.statusText = d == DialogResult.Yes ? "Yes selected" : "No selected";
		});
	}
}