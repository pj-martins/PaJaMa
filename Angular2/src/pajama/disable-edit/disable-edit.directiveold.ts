//import { Directive, Input, ElementRef, ViewEncapsulation, AfterViewInit } from '@angular/core';

//@Directive({
//	selector: '[disableEdit]'
//})
//export class DisableEditDirective implements AfterViewInit {

//	private _disableEdit = false;
//	private _inited = false;

//	private elements: Array<DisableElement> = [];

//	@Input()
//	get disableEdit(): boolean {
//		return this._disableEdit;
//	}
//	set disableEdit(v: boolean) {
//		this._disableEdit = v;
//		this._lastItemDisabled = {};
//		this._lastContainerDisabled = {};
//		if (this._inited)
//			this.addRemoveStyles();
//	}

//	constructor(private el: ElementRef) {
//	}

//	private _originalStyles: { [name: string]: any } = {};
//	private addRemoveStyles() {
//		for (let de of this.elements) {
//			if (!this._originalStyles[de.element.name]) {
//				this._originalStyles[de.element.name] = {};
//				Object.assign(this._originalStyles[de.element.name], de.element.style);
//			}
//			if (!this.disableEdit) {
//				de.element.readOnly = false;
//				de.element.disabled = false;
//				let css = de.element.style;
//				Object.assign(css, this._originalStyles[de.element.name]);
//				if (de.childButton) {
//					de.childButton.style.display = "";
//				}
//			}
//			else {
//				de.element.readOnly = true;
//				de.element.disabled = true;
//				let css = de.element.style;
//				css.setProperty("border", "0px", "important");
//				css.fontWeight = "bold";
//				css.backgroundColor = "rgb(240,240,240)";
//				if (de.childButton) {
//					de.childButton.style.display = "none";
//				}
//			}

//			let el = de.element.nextElementSibling;
//			if (de.element.attributes["ng-reflect-multi-typeahead"]) {
//				this.addRemoveMultiTypeaheadStyles(de);
//				el.addEventListener("DOMSubtreeModified", () => {
//					window.setTimeout(() => {
//						this.addRemoveMultiTypeaheadStyles(de);
//					}, 100);
//				});
//			}
//		}
//	}

//	private _lock = false;
//	private _lastItemDisabled: { [nameIndex: string]: boolean } = {};
//	private _lastContainerDisabled: { [name: string]: boolean } = {};
//	private addRemoveMultiTypeaheadStyles(de: DisableElement) {
//		if (this._lock) return;
//		this._lock = true;
//		let el = de.element.nextElementSibling;
//		let items = el.getElementsByClassName("multi-textbox-item");
//		if (items && items.length > 0) {
//			if (!this._originalStyles[de.element.name + "_multi-textbox-item"]) {
//				this._originalStyles[de.element.name + "_multi-textbox-item"] = {};
//				Object.assign(this._originalStyles[de.element.name + "_multi-textbox-item"], items[0].style);
//			}
//			let resetBorder = false;
//			for (let i = 0; i < items.length; i++) {
//				if (this._lastItemDisabled[de.element.name + "_" + i.toString()]) continue;
//				this._lastItemDisabled[de.element.name + "_" + i.toString()] = true;
//				let css = items[i].style;
//				if (this.disableEdit) {
//					resetBorder = true;
//					css.fontWeight = "bold";
//					css.borderRadius = "0px";
//					css.setProperty("border-right", "1px solid black", "important");
//				}
//				else {
//					Object.assign(css, this._originalStyles[de.element.name + "_multi-textbox-item"]);
//				}
//			}
//			if (resetBorder) {
//				for (let i = 0; i < items.length; i++) {
//					let css = items[i].style;
//					if (i < items.length - 1) {
//						css.setProperty("border-right", "1px solid black", "important");
//					}
//					else {
//						css.setProperty("border-right", "0px", "important");
//					}
//				}
//			}

//			if (!this._lastContainerDisabled) {
//				el.getElementsByClassName("multi-textbox-item-container")[0].style.setProperty("padding-left", "4px", "!important");
//				this._lastContainerDisabled[de.element.name] = true;
//			}
//		}
//		this._lock = false;
//	}

//	ngAfterViewInit() {
//		var inputs = [];
//		if (this.el.nativeElement.tagName.toLowerCase() == "input" ||
//			this.el.nativeElement.tagName.toLowerCase() == "textarea") {
//			inputs.push(this.el.nativeElement);
//		}
//		else if (this.el.nativeElement.tagName.toLowerCase() == "checklist") {
//			inputs.push(this.el.nativeElement.getElementsByClassName("checklist-button")[0]);
//		}
//		else {
//			inputs = this.el.nativeElement.querySelectorAll("input,textarea");
//		}
//		for (let input of inputs) {
//			let de = new DisableElement();
//			de.element = input;
//			de.element.onfocus = () => {
//				if (this.disableEdit) {
//					de.element.style.outline = "none";
//				}
//				else {
//					de.element.style.outline = this._originalStyles[de.element.name].outline;
//				}
//				if (de.originalOnFocus)
//					de.originalOnFocus();
//			};

//			if (de.element.attributes["datetimepicker"] || de.element.attributes["typeahead"]) {
//				let btn = de.element.nextElementSibling.getElementsByClassName("input-button-container");
//				if (btn.length > 0) {
//					de.childButton = btn[0];
//				}
//			}
//			else if (de.element.className && de.element.className.indexOf("checklist-button") >= 0) {
//				de.childButton = de.element.getElementsByClassName("drop-down-image")[0];
//			}

//			this.elements.push(de);
//		}
//		this._inited = true;
//		this.addRemoveStyles();

//	}
//}

//class DisableElement {
//	element: any;
//	childButton: any;
//	originalOnFocus: any;
//}
