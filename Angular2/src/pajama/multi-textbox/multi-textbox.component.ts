import { Component, Input, Output, EventEmitter, ElementRef, OnInit } from '@angular/core'

export const MULTITEXTBOX_TEMPLATE = `
	<div class='input-button-container component' [style.display]="currText ? 'inline' : 'none'">
		<button (click)='addItem()' class="input-button" tabindex="-1">
			<div class="glyphicon glyphicon-plus"></div>
		</button>
	</div>
	<br />
	<div class='multi-textbox-item-container component' [style.padding-left]="(originalPaddingLeft <= 0 ? 0 : originalPaddingLeft - 1) + 'px'">
		<div *ngFor='let item of items || []' class='multi-textbox-item'>
			{{getObjectValue(item)}}
			<div class='multi-textbox-remove close-outer' (click)='removeItem(item)'>
				<div class='close-inner'>
					<div class='close-icon'></div>
				</div>
			</div>
		</div>
	</div>
`;

@Component({
	moduleId: module.id,
	selector: 'multi-textbox',
	template: MULTITEXTBOX_TEMPLATE,
	styleUrls: ['../styles.css', 'multi-textbox.css']
})
export class MultiTextboxComponent implements OnInit {

	private _items: Array<any> = [];
	get items(): Array<any> {
		return this._items;
	}
	set items(v: Array<any>) {
		this._items = v || [];
	}

	@Output() itemsChanged = new EventEmitter<any>();
	@Output() paddingChanged = new EventEmitter<any>();

	currText: string;

	constructor(protected elementRef: ElementRef) {
	}

	ngOnInit() {
	}

	originalPaddingLeft = -999;
	private _paddingLeft: number = 0;
	get paddingLeft() {
		return this._paddingLeft + (this.originalPaddingLeft < 0 ? 0 : this.originalPaddingLeft);
	}

	resize() {
		let items = this.elementRef.nativeElement.getElementsByClassName("multi-textbox-item");
		let totWidth = 0;
		for (let item of items) {
			totWidth += item.offsetWidth + 1;
		}
		this._paddingLeft = totWidth;
		this.paddingChanged.emit(this._paddingLeft);
	}

	protected removeItem(item: any) {
		for (let i = this._items.length - 1; i >= 0; i--) {
			if (this.itemsAreEqual(this._items[i], item)) {
				this._items.splice(i, 1);
				this.itemsChanged.emit(null);
				break;
			}
		}
		this.resize();
	}

	protected itemsAreEqual(item1: any, item2: any): boolean {
		return item1 == item2;
	}

	protected addItem() {
		if (this.currText) {
			this._items.push(this.currText);
			this.currText = "";
			this.itemsChanged.emit(null);
		}
		this.resize();
	}

	keydown(event: KeyboardEvent, ignoreEnter: boolean) {
		let charCode = event.which || event.keyCode;
		if (charCode == 8) {
			if (!this.currText && this._items.length > 0) {
				this._items.splice(this._items.length - 1, 1);
				this.itemsChanged.emit(null);
			}
		}
		else if (charCode == 13 && this.currText && !ignoreEnter) {
			this.addItem();
			event.preventDefault();
		}
		window.setTimeout(() => this.resize(), 100);
	}

	blur() {
		this.addItem();
		this.resize();
	}

	protected getObjectValue(item: any) {
		return item;
	}
}