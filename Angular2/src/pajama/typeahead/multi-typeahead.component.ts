import { Component, Input, Output, EventEmitter, OnInit, forwardRef, ViewChild } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { ParserService } from '../services/parser.service';
import { TypeaheadComponent } from './typeahead.component';

const noop = () => {
};

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
	provide: NG_VALUE_ACCESSOR,
	useExisting: forwardRef(() => MultiTypeaheadComponent),
	multi: true
};

@Component({
	moduleId: module.id,
	selector: 'multi-typeahead',
	template: `<input type='text' class='form-control' (keydown)='keydown($event)' (keyup)='valueChanged($event)' [(ngModel)]='itemString' />
<typeahead #childTypeahead [(ngModel)]='lastItem' [limitToList]='false' [isForMulti]='true' class='multi-typeahead' [dataSource]='getItems' (itemSelected)='itemSelected($event)'></typeahead>`,
	providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
})
export class MultiTypeaheadComponent implements ControlValueAccessor, OnInit {
	private onTouchedCallback: () => void = noop;
	private onChangeCallback: (_: any) => void = noop;

	// todo: func
	@Input()
	dataSource: Array<any>;

	@ViewChild("childTypeahead")
	childTypeAhead: TypeaheadComponent;

	constructor(private parserService: ParserService) { }

	ngOnInit() {
	}

	private _itemString: string;
	get itemString(): string {
		return this._itemString;
	}
	set itemString(v: string) {
		this._itemString = v;
		this.childTypeAhead.textValue = this.lastItem;

		let val = [];
		if (this.itemString)
			val = this.itemString.split(',');
		this.onChangeCallback(val);
	}

	get lastItem(): string {
		if (!this.itemString) return '';
		let parts = this.itemString.split(',');
		return parts[parts.length - 1];
	}
	set lastItem(i: string) {
		// do nothing, typeahead is hidden
	}

	// TODO: valuemember, displaymember
	itemSelected(item) {
		for (let i = this.itemString.length - 1; i >= 0; i--) {
			if (i == 0) {
				this.itemString = item;
			}
			else if (this.itemString[i] == ',') {
				this.itemString = this.itemString.substring(0, i + 1) + item;
				break;
			}
		}
	}

	writeValue(value: Array<any>) {
		this.itemString = value ? value.join(',') : '';
	}

	registerOnChange(fn: any) {
		this.onChangeCallback = fn;
	}

	registerOnTouched(fn: any) {
		this.onTouchedCallback = fn;
	}

	getItems = (partial: string): Array<any> => {
		let parts = partial.split(',');
		if (parts.length < 1) return [];

		let lastItem = parts[parts.length - 1];

		let matches = new Array<any>();
		for (let ds of this.dataSource) {
			if (ds.toLowerCase().indexOf(lastItem.toLowerCase()) >= 0) {
				matches.push(ds);
			}
		}
		return matches;
	}

	keydown(evt: any) {
		let childDropdownVisible = this.childTypeAhead.dropdownVisible;
		let charCode = evt.which || evt.keyCode;
		let isComma = false;
		if (charCode == 188) {
			this.childTypeAhead.processSelection(9);
			isComma = true;
		}
		else {
			this.childTypeAhead.keydown(evt);
		}
		if (!isComma && childDropdownVisible && charCode == 9)
			evt.preventDefault();
	}

	valueChanged(evt: any) {
		this.childTypeAhead.valueChanged(evt);
	}
}