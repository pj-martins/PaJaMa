import { Component, Input, Output, EventEmitter } from '@angular/core'

@Component({
	moduleId: module.id,
	selector: 'multi-textbox',
	template: `<div class='multi-textbox-container'>
	<div class='multi-textbox-item-container'>
		<div *ngFor='let item of items || []'>
			<div class='multi-textbox-item'>
				{{item}}
				<div class='glyphicon glyphicon-remove multi-textbox-remove' (click)='removeItem(item)'></div>
			</div>
		</div>
	</div>
	<div class='multi-textbox-textbox'>
		<input type='text' [(ngModel)]='currText' (keydown)="keyDown($event)" (keypress)="keyPress($event)" (blur)='blur()' />
	</div>
	<div class='glyphicon glyphicon-plus multi-textbox-add' [style.display]="currText ? 'inline-block' : 'none'" (click)='addItem()'></div>
</div>`,
	styleUrls: ['multi-textbox.css']
})
export class MultiTextboxComponent {

	private _items: Array<any> = [];
	@Input()
	get items(): Array<any> {
		return this._items;
	}
	set items(v: Array<any>) {
		this._items = v || [];
	}

	@Output() itemsChanged = new EventEmitter<any>();

	protected currText: string;

	protected removeItem(item: string) {
		for (let i = this._items.length - 1; i >= 0; i--) {
			if (this._items[i] == item) {
				this._items.splice(i, 1);
				this.itemsChanged.emit(null);
				break;
			}
		}
	}

	protected addItem() {
		if (this.currText) {
			this._items.push(this.currText);
			this.currText = "";
			this.itemsChanged.emit(null);
		}
	}

	protected keyPress(event: KeyboardEvent) {
		if (event.which == 13) {
			this.addItem();
			event.preventDefault();
		}
	}

	protected keyDown(event: any) {
		let charCode = event.which || event.keyCode;
		if (charCode == 8) {
			if (!this.currText && this._items.length > 0) {
				this._items.splice(this._items.length - 1, 1);
				this.itemsChanged.emit(null);
			}
		}
	}

	protected blur() {
		this.addItem();
	}
}