import { Component, Input, Output, EventEmitter, OnInit, NgZone } from '@angular/core';

export class CheckListItem {
	selected: boolean;
	constructor(public item: any) {
	}
}

@Component({
	moduleId: module.id,
	selector: 'check-list',
	template: `
    <div class='check-list check-list-container form-control id_{{uniqueId}}'>
		<div class='check-list-input-container'>
			<button (click)='dropdownVisible = !dropdownVisible' class="check-list-input" [style.text-align]="showMultiplesEllipses ? 'center' : ''">
				{{selectedText ? selectedText : '&nbsp;'}}
			</button>
		</div>
		<div class='check-list-button-container'>
			<button (click)='dropdownVisible = !dropdownVisible' class="check-list-button">
				<div class="drop-down-image glyphicon {{ allSelected || !showFilterIcon ? 'glyphicon-chevron-down' : 'glyphicon-filter'}}"></div>
			</button>
		</div>
        <div class='check-list-dropdown' [hidden]='!dropdownVisible'>
            <div *ngIf='!disableAll' (click)='selectAll()' class='check-list-item check-list-item-all id_{{uniqueId}}'>
                <div class="check-list-check glyphicon {{ allSelected ? 'glyphicon-ok' : 'glyphicon-none'}}"></div>(Select All)</div>
                <div *ngFor='let item of displayItems' (click)='selectItem(item)' class='check-list id_{{uniqueId}}'>
                <div class='check-list-item id_{{uniqueId}}'>
                    <div class="check-list-check glyphicon {{ item.selected ? 'glyphicon-ok' : 'glyphicon-none'}}"></div>{{displayMember ? item.item[displayMember] : item.item}}
                </div>
            </div>
        </div>
    </div>
`,
	styleUrls: ['checklist.css']
})
export class CheckListComponent implements OnInit {
	@Input() displayMember: string;
	@Input() showFilterIcon = false;
	@Input() disableAll = false;
	@Output() selectionChanged = new EventEmitter<any>();
	@Input() showMultiplesEllipses = false;

	protected displayItems: Array<CheckListItem> = [];
	protected allSelected = false;

	constructor(private zone: NgZone) { }

	private _value: Array<any> = [];
	@Input()
	get selectedItems(): any {
		return this._value;
	}
	set selectedItems(v: any) {
		this._value = v;
		this.updateSelection(true);
	}


	private _items: Array<any> = [];
	@Input()
	get items(): Array<any> {
		return this._items;
	}
	set items(v: Array<any>) {
		this._items = v || [];
		this.displayItems = [];
		for (let i of this._items) {
			this.displayItems.push(new CheckListItem(i));
		}
		this.updateSelection(true);
	}

	protected selectedText: string;
	protected dropdownVisible: boolean;
	private currentonclick: any;
	protected uniqueId = Math.floor((1 + Math.random()) * 0x10000).toString();

	private updateSelection(init = false) {
		if (init) {
			let allSelected = true;
			for (let di of this.displayItems) {
				di.selected = false;
				if (this._value) {
					for (let i of this._value) {
						if (this.equals(di.item, i)) {
							di.selected = true;
							break;
						}
					}
					if (!di.selected)
						allSelected = false;
				}
				else {
					di.selected = true;
				}
			}
			this.allSelected = allSelected;
		}

		this.selectedText = "";

		// TODO: validate all is in items
		if (this.selectedItems) {
			if (this.items) {
				if (this.selectedItems.length >= this.items.length) {
					return;
				}
			}

			if (this.showMultiplesEllipses && this.selectedItems.length > 1)
				this.selectedText = "(...)";
			else {
				for (let i = 0; i < this.selectedItems.length; i++) {
					this.selectedText += (i == 0 ? "" : ", ") + (this.displayMember ? this.selectedItems[i][this.displayMember] : this.selectedItems[i]);
				}
			}
		}
	}

	// TODO: share
	private equals(x, y) {
		if (x === y)
			return true;
		// if both x and y are null or undefined and exactly the same
		if (!(x instanceof Object) || !(y instanceof Object))
			return false;
		// if they are not strictly equal, they both need to be Objects
		if (x.constructor !== y.constructor)
			return false;
		// they must have the exact same prototype chain, the closest we can do is
		// test there constructor.

		let p;
		for (p in x) {
			if (!x.hasOwnProperty(p))
				continue;
			// other properties were tested using x.constructor === y.constructor
			if (!y.hasOwnProperty(p))
				return false;
			// allows to compare x[ p ] and y[ p ] when set to undefined
			if (x[p] === y[p])
				continue;
			// if they have the same strict value or identity then they are equal
			if (typeof (x[p]) !== "object")
				return false;
			// Numbers, Strings, Functions, Booleans must be strictly equal
			if (!this.equals(x[p], y[p]))
				return false;
		}
		for (p in y) {
			if (y.hasOwnProperty(p) && !x.hasOwnProperty(p))
				return false;
		}
		return true;
	}

	private updateSelectedCollection(items: CheckListItem[]) {
		for (let item of items) {
			let existingIndex = -1;
			for (let i = 0; i < this.selectedItems.length; i++) {
				if (this.equals(this.selectedItems[i], item.item)) {
					existingIndex = i;
					break;
				}
			}
			if (item.selected && existingIndex < 0) {
				this.selectedItems.push(item.item);
			}
			else if (!item.selected && existingIndex >= 0) {
				this.selectedItems.splice(existingIndex, 1);
			}
		}
	}

	selectItem(item: CheckListItem) {
		item.selected = !item.selected;
		if (!item.selected)
			this.allSelected = false;
		this.updateSelectedCollection([item]);
		this.updateSelection();
		this.selectionChanged.emit(null);
	}

	selectAll() {
		this.allSelected = !this.allSelected;
		for (let i of this.displayItems) {
			i.selected = this.allSelected;
		}
		this.updateSelectedCollection(this.displayItems);
		this.updateSelection();
		this.selectionChanged.emit(null);
	}

	ngOnInit() {
		// TODO: hackish, need to find a better way to hide drop down when they click off of it, can't use blur
		// since blur will fire when the dropdown div is clicked in which case we don't want to hide the dropdown
		let self = this;
		this.currentonclick = document.onclick;
		document.onclick = (event: any) => {
			if (this.currentonclick) this.currentonclick(event);
			if (self.dropdownVisible && event.target && event.target.className.indexOf(`id_${this.uniqueId}`) < 0 && (!event.target.offsetParent || event.target.offsetParent.className.indexOf(`id_${this.uniqueId}`) < 0)) {
				self.zone.run(() => self.dropdownVisible = false);
			}
		};
	}
}