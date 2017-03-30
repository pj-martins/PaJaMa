import { Component, Input, Output, EventEmitter, ElementRef, OnInit } from '@angular/core'

export const PRE_INPUT = `<div class='multi-textbox-container'>
	<div class='multi-textbox-item-container'>
		<div *ngFor='let item of items || []' class='multi-textbox-item'>
			{{getObjectValue(item)}}
			<div class='glyphicon glyphicon-remove multi-textbox-remove' (click)='removeItem(item)'></div>
		</div>
	</div>
	<div class='multi-textbox-input-container'>
`;

export const POST_INPUT = `
	</div>
	<div class='multi-textbox-button-container' [style.display]="currText ? 'inline' : 'none'">
		<button (click)='addItem()' class="multi-textbox-button" tabindex="-1">
			<div class="glyphicon glyphicon-plus"></div>
		</button>
	</div>
</div>`;

@Component({
	moduleId: module.id,
	selector: 'multi-textbox',
	template: PRE_INPUT + `
		<input type='text' [style.padding-left]="paddingLeft + 'px'" class='multi-textbox-input' [(ngModel)]='currText' (keydown)="keyDown($event, false)" (blur)='blur()' (focus)="resize()" />
	` + POST_INPUT,
	styleUrls: ['multi-textbox.css']
})
export class MultiTextboxComponent implements OnInit {

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

	constructor(protected elementRef: ElementRef) {
		
	}

	ngOnInit() {
	}

	private _originalPaddingLeft: number = 0;
	private _paddingLeft: number = 0;
	@Input()
	protected get paddingLeft() {
		return this._paddingLeft;
	}
	protected set paddingLeft(p: number) {
		this._originalPaddingLeft = p;
		this._paddingLeft = p;
	}

	protected resize() {
		let items = this.elementRef.nativeElement.getElementsByClassName("multi-textbox-item");
		let totWidth = 0;
		for (let item of items) {
			totWidth += item.offsetWidth + 1;
		}
		this._paddingLeft = this._originalPaddingLeft + totWidth;
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

	protected itemsAreEqual(item1: any, item2: any) : boolean {
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

	protected keyDown(event: KeyboardEvent, ignoreEnter: boolean) {
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

	protected blur() {
		this.addItem();
		this.resize();
	}

	protected getObjectValue(item: any) {
		return item;
	}
}