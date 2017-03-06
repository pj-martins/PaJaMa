import { Component, Input, Output, EventEmitter, OnInit, forwardRef, NgZone } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { ParserService } from '../services/parser.service';

const noop = () => {
};

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
	provide: NG_VALUE_ACCESSOR,
	useExisting: forwardRef(() => TypeaheadComponent),
	multi: true
};

@Component({
	moduleId: module.id,
	selector: 'typeahead',
	template: `
		<div class='typeahead-container' [hidden]='isForMulti'>
			<div class='typeahead-input-container'>
				<input type='text' [(ngModel)]='textValue' (keyup)='valueChanged($event)' (keydown)='keydown($event)' class='form-control text_id_{{uniqueId}}' />
			</div>
			<div class='typeahead-button-container' *ngIf='!dataSourceFunction && !hideButton'>
				<button class='btn btn-default glyphicon glyphicon-chevron-down typeahead-button button_id_{{uniqueId}}' tabindex='-1' (click)='openByButton()' (keydown)='keydown($event)'></button>
			</div>
		</div>
		<ul [hidden]='!dropdownVisible' class='typeahead-popup' tabindex="-1">
			<li *ngFor='let item of items; let i = index' [hidden]='itemHidden(item)'>
				<div class='typeahead-item' class="typeahead-item {{activeIndex == i ? 'typeahead-item-selected' : ''}}" (mouseover)='hovered(i)' (click)='selectItem(item)'>
					<div [innerHtml]='itemDisplay(item)'></div>
				</div>
			</li>
        </ul>
		<strong [hidden]='!typeaheadError' class='typeahead-error'>
			Invalid selection, please select item from list.
		</strong>
        `,
	providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR],
	styleUrls: ['typeahead.css']
})
export class TypeaheadComponent implements ControlValueAccessor, OnInit {
	private innerValue: any;
	private onTouchedCallback: () => void = noop;
	private onChangeCallback: (_: any) => void = noop;
	private valueWritten = false;

	private isOpenByButton = false;

	dataSourceFunction: (partial: string) => any;
	private _dataSource: any;



	@Input()
	get dataSource(): any {
		return this._dataSource;
	}
	set dataSource(val: any) {
		this._dataSource = val;
		if (!val) return;
		if (val instanceof Array) {
			this.items = val;
		}
		else if (typeof val == "function") {
			this.dataSourceFunction = val;
		}
		else
			throw typeof val;
		if (this.valueWritten)
			this.setText();
	}

	items: Array<any> = [];


	activeIndex = -1;

	@Input()
	displayMember: string;

	@Input()
	limitToList = true;

	@Input()
	valueMember: string;

	@Input()
	minLength = 1;

	@Input()
	hideButton = false;

	@Input()
	isForMulti = false;

	@Output()
	itemSelected = new EventEmitter<any>();

	typeaheadError = false;
	currentonclick: any;
	uniqueId = Math.floor((1 + Math.random()) * 0x10000).toString();

	constructor(private parserService: ParserService, private zone: NgZone) { }

	dropdownVisible = false;

	get value(): any {
		return this.innerValue;
	}

	set value(v: any) {
		if (v !== this.innerValue) {
			this.innerValue = v;
			this.onChangeCallback(this.innerValue);
		}
	}

	ngOnInit() {
		// TODO: hackish, need to find a better way to hide drop down when they click off of it, can't use blur
		// since blur will fire when the dropdown div is clicked in which case we don't want to hide the dropdown
		let self = this;
		this.currentonclick = document.onclick;
		document.onclick = (event: any) => {
			if (this.currentonclick) this.currentonclick(event);
			if (self.dropdownVisible && event.target && event.target.className.indexOf(`id_${this.uniqueId}`) < 0) {
				self.zone.run(() => {
					self.dropdownVisible = false;
					self.isOpenByButton = false;
					let matchIndices = self.getMatchIndices();
					if (matchIndices.length == 1) {
						self.selectItem(self.items[matchIndices[0]]);
					}
					else if (this.limitToList) {
						self.value = null;
						self.typeaheadError = true;
					}
					else {
						self.value = this.textValue;
						this.itemSelected.emit(this.innerValue);
					}
				});
			}
		};
	}

	textValue: string;
	selectItem(item: any) {
		this.typeaheadError = false;
		this.textValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
		this.value = this.valueMember ? this.parserService.getObjectValue(this.valueMember, item) : item;
		this.isOpenByButton = false;
		this.dropdownVisible = false;
		this.itemSelected.emit(this.innerValue);
	}

	activateItem(index: number) {
		this.activeIndex = index;
	}

	openByButton() {
		this.dropdownVisible = !this.dropdownVisible;
		this.isOpenByButton = this.dropdownVisible;
		if (this.isOpenByButton && this.activeIndex < 0 && this.textValue) {
			for (let i = 0; i < this.items.length; i++) {
				let item = this.items[i];
				let objValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
				if (objValue == this.textValue) {
					this.activeIndex = i;
					break;
				}
			}
		}
	}

	private getMatchIndices(): Array<number> {
		let indices: Array<number> = [];
		let matchCount = 0;
		for (let i = 0; i < this.items.length; i++) {
			let item = this.items[i];
			if (this.getTextMatchIndex(item) >= 0 || this.isOpenByButton)
				indices.push(i);
		}
		return indices;
	}

	valueChanged(event: any) {
		let charCode = event.which || event.keyCode;
		// which other char codes?
		if ((charCode >= 48 && charCode <= 90) || (charCode >= 96 && charCode <= 111) || (charCode >= 186 && charCode <= 222) || charCode == 8) {
			this.isOpenByButton = false;
			this.value = null;
			let ddVisible = this.textValue && this.textValue.length >= this.minLength;
			if (ddVisible && this.dataSourceFunction != null) {
				var funcValue = this.dataSourceFunction(this.textValue);
				if (funcValue) {
					if (funcValue instanceof Array) {
						this.items = funcValue;
						this.setActiveIndex(ddVisible);
					}
					else if (funcValue.subscribe) {
						funcValue.subscribe((r: any) => {
							this.items = r;
							this.setActiveIndex(ddVisible);
						});
					}
					else throw funcValue;
				}
			}
			else {
				this.setActiveIndex(ddVisible);
			}
		}
	}

	private setActiveIndex(ddVisible: boolean) {
		if (ddVisible) {
			let indices = this.getMatchIndices();
			if (indices.length < 1)
				ddVisible = false;
			else if (indices.indexOf(this.activeIndex) < 0) {
				if (indices.length > 0)
					this.activeIndex = indices[0];
				else
					this.activeIndex = -1;
			}
		}
		this.dropdownVisible = ddVisible;
		if (!ddVisible) {
			this.activeIndex = -1;
		}
	}

	processSelection(charCode) {
		if (this.activeIndex >= 0) {
			this.selectItem(this.items[this.activeIndex]);
			if (charCode == 13)
				event.preventDefault();
		}
		else if (!this.limitToList) {
			this.value = this.textValue;
			this.itemSelected.emit(this.innerValue);
		}
		else if (!this.value && this.textValue) {
			this.typeaheadError = true;
		}
	}

	keydown(event: any) {
		this.typeaheadError = false;
		let charCode = event.which || event.keyCode;
		switch (charCode) {
			case 13:
			case 9:
				this.processSelection(charCode);
				break;
			case 40:
				if (!this.dropdownVisible && !this.isOpenByButton && !this.hideButton && !this.textValue) {
					this.isOpenByButton = true;
					this.dropdownVisible = true;
				}
				let indices = this.getMatchIndices();
				if (this.activeIndex < 0) {
					this.activeIndex = indices[0];
				}
				else {
					if (this.activeIndex == indices[indices.length - 1])
						this.activeIndex = indices[0];
					else {
						let indicesIndex = 0;
						for (let i = 0; i < indices.length; i++) {
							if (indices[i] == this.activeIndex) {
								this.activeIndex = indices[i + 1];
								break;
							}
						}
					}
				}
				event.preventDefault();
				break;
			case 38:
				if (!this.dropdownVisible && !this.isOpenByButton && !this.hideButton && !this.textValue) {
					this.isOpenByButton = true;
					this.dropdownVisible = true;
				}
				let indices2 = this.getMatchIndices();
				if (this.activeIndex < 0) {
					this.activeIndex = indices[indices.length - 1];
				}
				else {
					if (this.activeIndex == indices2[0])
						this.activeIndex = indices2[indices2.length - 1];
					else {
						let indicesIndex = 0;
						for (let i = 0; i < indices2.length; i++) {
							if (indices2[i] == this.activeIndex) {
								this.activeIndex = indices2[i - 1];
								break;
							}
						}
					}
				}
				event.preventDefault();
				break;
		}
	}

	itemHidden(item: any): boolean {
		if (this.isOpenByButton) return false;
		return this.getTextMatchIndex(item) < 0;
	}

	private getTextMatchIndex(item: any): number {
		let objValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
		if (this.textValue) {
			if (typeof objValue == "string" && typeof this.textValue == "string") {
				let matchIndex = objValue.toLowerCase().indexOf(this.textValue.toLowerCase());
				return matchIndex;
			}
		}
		return -1;
	}

	itemDisplay(item) {
		let objValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
		let matchIndex = this.getTextMatchIndex(item);
		if (matchIndex >= 0) {
			return objValue.substring(0, matchIndex) + '<strong>' + objValue.substring(matchIndex, matchIndex + this.textValue.length) + '</strong>'
				+ objValue.substring(matchIndex + this.textValue.length);
		}

		return objValue;
	}

	hovered(index) {
		this.activeIndex = index;
	}

	writeValue(value: any) {
		if (value === undefined || value == null) {
			this.innerValue = null;
			this.setText();
		}
		else if (value !== this.innerValue) {
			this.innerValue = value;
			this.valueWritten = true;
			this.setText();
		}
	}

	setText() {
		if (this.innerValue === undefined || this.innerValue === null || this.innerValue === '') {
			this.textValue = '';
		}
		else if (!this.textValue && this.displayMember && !this.valueMember) {
			this.textValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, this.innerValue) : this.innerValue;
		}
		else if (!this.textValue && this.displayMember && this.valueMember && this.items && this.items.length > 0) {
			for (let item of this.items) {
				let val = this.parserService.getObjectValue(this.valueMember, item);
				if (val == this.innerValue) {
					this.textValue = this.parserService.getObjectValue(this.displayMember, item);
					break;
				}
			}
		}

	}

	registerOnChange(fn: any) {
		this.onChangeCallback = fn;
	}

	registerOnTouched(fn: any) {
		this.onTouchedCallback = fn;
	}
}