import { Component, Input, Output, ElementRef } from '@angular/core'
import { Observable } from 'rxjs/Observable';

@Component({
	moduleId: module.id,
	selector: 'modal-dialog',
	template: `
<div class="modal-dialog-container component" *ngIf='shown' [style.left]="overrideLeft" [style.top]="overrideTop">
	<div class="modal-dialog-header" *ngIf="showFooter">
		<button class="icon-remove-black icon-small icon-button modal-close-button" *ngIf="!hideCloseButton"  (click)="hide()">
        </button>
        <div class="modal-title"><strong>{{headerText}}</strong></div>
	</div>
    <div class="modal-dialog-body">
        <ng-content></ng-content>
	</div>
	<div class="modal-dialog-footer" *ngIf="showFooter">
		<button type="button" class="btn" *ngIf="buttons == button.OK || buttons == button.OKCancel" (click)="ok()">OK</button>
		<button type="button" class="btn" *ngIf="buttons == button.OKCancel" (click)="cancel()">Cancel</button>
        <button type="button" class="btn" *ngIf="buttons == button.YesNo" (click)="yes()">Yes</button>
        <button type="button" class="btn" *ngIf="buttons == button.YesNo" (click)="no()">No</button>
	</div>
</div>
<div class="modal-background" *ngIf="shown && showBackdrop"></div>
`,
	styleUrls: ['../assets/css/styles.css', '../assets/css/icons.css', '../assets/css/buttons.css', 'modal-dialog.css']
})
export class ModalDialogComponent {
	private currObserver: any;

	protected overrideTop;
	protected overrideLeft;

	protected button = Button;
	protected buttons = Button.OK;
	protected shown = false;

	@Input()
	headerText: string;

	@Input()
	showBackdrop = false;

	@Input()
	hideCloseButton = false;

	@Input()
	showHeader = true;

	@Input()
	showFooter = true;

	protected ok() {
		this._hide(DialogResult.OK);
	}

	protected cancel() {
		this._hide(DialogResult.Cancel);
	}

	protected yes() {
		this._hide(DialogResult.Yes);
	}

	protected no() {
		this._hide(DialogResult.No);
	}

	get isShown(): boolean {
		return this.shown;
	}

	constructor(private elementRef: ElementRef) { }

	toggle() {
		if (!this.shown)
			this.show();
		else
			this.hide();
	}

	show(buttons = Button.OK, hideAfter = 0, x = -1, y = -1): Observable<DialogResult> {
		this.buttons = buttons;
		this.shown = true;
		if (x >= 0 && y >= 0) {
			this.overrideTop = y.toString() + "px";
			this.overrideLeft = x.toString() + "px";
		}
		if (hideAfter > 0) {
			// TODO: always ok?
			window.setTimeout(() => this._hide(DialogResult.OK),
				hideAfter + 200);
		}
		return Observable.create(o => this.currObserver = o);
	}

	hide() {
		this._hide();
	}

	private _hide(dialogResult: DialogResult = DialogResult.Cancel) {
		this.shown = false;
		this.overrideTop = null;
		this.overrideLeft = null;

		if (this.currObserver) {
			this.currObserver.next(dialogResult);
			this.currObserver.complete();
		}
	}
}

export enum Button {
	OK,
	OKCancel,
	YesNo,
	None
}

export enum DialogResult {
	OK,
	Cancel,
	Yes,
	No
}