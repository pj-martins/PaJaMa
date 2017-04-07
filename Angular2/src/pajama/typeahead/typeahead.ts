import { EventEmitter, NgZone } from '@angular/core';
import { ParserService } from '../services/parser.service';

export const TYPEAHEAD_TEMPLATE = `
	<div class='typeahead'>
		<div class='input-button-container id_{{uniqueId}}' *ngIf='!typeahead.dataSourceFunction && !typeahead.hideButton'>
			<button class='input-button id_{{uniqueId}}' (click)='typeahead.openByButton()' (keydown)='typeahead.keydown($event)' tabindex="-1">
				<div class='typeahead-dropdown-image arrow-down id_{{uniqueId}}'></div>
			</button>
		</div>
		<div class='input-button-container' *ngIf='typeahead.dataSourceFunction && typeahead.loading'>
			<span class='glyphicon glyphicon-refresh refresh-icon'></span>
		</div>
		<div [hidden]='!typeahead.dropdownVisible' class='typeahead-popup component'>
			<div *ngFor='let item of typeahead.items; let i = index' [hidden]='typeahead.itemHidden(item)'>
				<div class='typeahead-item' class="typeahead-item {{typeahead.activeIndex == i ? 'typeahead-item-selected' : ''}}" (mouseover)='typeahead.hovered(i)' (click)='typeahead.selectItem(item)'>
					<div [innerHtml]='typeahead.itemDisplay(item)'></div>
				</div>
			</div>
        </div>
		<strong [hidden]='!typeahead.typeaheadError' class='typeahead-error'>
			Invalid selection, please select item from list.
		</strong>
	</div>
`;

export class Typeahead {

	valueChanged = new EventEmitter<any>();

	private isOpenByButton = false;

	constructor(private parserService: ParserService) { }

	protected loading = false;

	items: Array<any> = [];

	dataSourceFunction: (partial: string) => any;
	private _dataSource: any;

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
	}

	private _lock = false;
	private _innerValue: any;
	private get innerValue(): any {
		return this._innerValue;
	}
	private set innerValue(v: any) {
		this._innerValue = v;
		if (!this._lock)
			this.valueChanged.emit(v);
	}

	displayMember: string;
	matchOn: Array<string>;
	limitToList = true;
	valueMember: string;
	dropdownVisible = false;
	activeIndex = -1;
	textValue: string;
	minLength = 1;
	waitMs = 0;
	hideButton = false;
	itemSelected = new EventEmitter<any>();
	uniqueId = Math.floor((1 + Math.random()) * 0x10000).toString();


	typeaheadError = false;


	protected selectItem(item: any) {
		this.typeaheadError = false;
		this.textValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
		this.innerValue = this.valueMember ? this.parserService.getObjectValue(this.valueMember, item) : item;
		this.isOpenByButton = false;
		this.dropdownVisible = false;
		this.itemSelected.emit(this.innerValue);
	}


	protected activateItem(index: number) {
		this.activeIndex = index;
	}

	private isMatch(item: any): boolean {
		if (this.textValue) {
			let objValues = [];
			if (this.matchOn) {
				for (let m of this.matchOn) {
					let val = this.parserService.getObjectValue(m, item);
					if (val)
						objValues.push(val)
				}
			}
			else if (this.displayMember) {
				let val = this.parserService.getObjectValue(this.displayMember, item);
				if (val)
					objValues.push(val)
			}
			else {
				objValues.push(item);
			}
			for (let objValue of objValues) {
				if (typeof objValue == "string" && typeof this.textValue == "string") {
					let matchIndex = objValue.toLowerCase().indexOf(this.textValue.toLowerCase());
					if (matchIndex >= 0)
						return true;
				}
			}
		}
		return false;
	}

	private getMatchIndices(): Array<number> {
		let indices: Array<number> = [];
		let matchCount = 0;
		for (let i = 0; i < this.items.length; i++) {
			let item = this.items[i];
			if (this.isMatch(item) || this.isOpenByButton)
				indices.push(i);
		}
		return indices;
	}

	private _lastKeypress: Date;
	keyup(event: any) {
		let charCode = event.which || event.keyCode;
		// which other char codes?
		if ((charCode >= 48 && charCode <= 90) || (charCode >= 96 && charCode <= 111) || (charCode >= 186 && charCode <= 222) || charCode == 8) {
			this._lastKeypress = new Date();
			this.isOpenByButton = false;
			this.innerValue = null;
			let ddVisible = this.textValue && this.textValue.length >= this.minLength;
			if (ddVisible && this.dataSourceFunction != null) {
				var funcValue = this.dataSourceFunction(this.textValue);
				if (funcValue) {
					if (funcValue instanceof Array) {
						this.loading = true;
						this.items = funcValue;
						this.loading = false;
						this.setActiveIndex(ddVisible);
					}
					else if (funcValue.subscribe) {
						if (this.waitMs != 0) {
							window.setTimeout(() => {
								let now = new Date();
								if (now.getTime() - this._lastKeypress.getTime() >= this.waitMs)
									this.processObservableFunction(funcValue, ddVisible);
							}, this.waitMs + 10);
						}
						else {
							this.processObservableFunction(funcValue, ddVisible);
						}
					}
					else throw funcValue;
				}
			}
			else {
				this.setActiveIndex(ddVisible);
			}
		}
	}

	private processObservableFunction(funcValue: any, ddVisible: boolean) {
		this.loading = true;
		funcValue.subscribe((r: any) => {
			this.items = r;
			this.loading = false;
			this.setActiveIndex(ddVisible);
		});
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

	protected processSelection(charCode) {
		if (this.activeIndex >= 0) {
			this.selectItem(this.items[this.activeIndex]);
			if (charCode == 13)
				event.preventDefault();
		}
		else if (!this.limitToList) {
			this.innerValue = this.textValue;
			this.itemSelected.emit(this.innerValue);
		}
		else if (!this.innerValue && this.textValue) {
			this.typeaheadError = true;
		}
	}

	writeValue(value: any) {
		this._lock = true;
		if (value === undefined || value == null) {
			this.innerValue = null;
		}
		else if (value !== this.innerValue) {
			this.innerValue = value;
		}
		this._lock = false;
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

	protected itemHidden(item: any): boolean {
		if (this.isOpenByButton) return false;
		return !this.isMatch(item);
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

	protected itemDisplay(item) {
		let objValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
		let matchIndex = this.getTextMatchIndex(item);
		if (matchIndex >= 0) {
			return objValue.substring(0, matchIndex) + '<strong>' + objValue.substring(matchIndex, matchIndex + this.textValue.length) + '</strong>'
				+ objValue.substring(matchIndex + this.textValue.length);
		}

		return objValue;
	}

	protected hovered(index) {
		this.activeIndex = index;
	}

	currentonclick: any;
	initTypeahead(sender: any, document: any) {
		let self = sender;
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

	protected openByButton() {
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
}