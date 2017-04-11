import { Directive, Input, ElementRef, ViewEncapsulation, AfterViewInit } from '@angular/core';

@Directive({
	selector: '[disableEdit]'
})
export class DisableEditDirective implements AfterViewInit {

	private _disableEdit = false;
	private _inited = false;

	private elements: Array<DisableElement> = [];

	@Input()
	get disableEdit(): boolean {
		return this._disableEdit;
	}
	set disableEdit(v: boolean) {
		this._disableEdit = v;
		if (this._inited)
			this.addRemoveStyles();
	}

	constructor(private el: ElementRef) {
	}

	private addRemoveStyles() {
		for (let de of this.elements) {
			if (!this.disableEdit) {
				de.element.readOnly = false;
				de.element.disabled = false;
				let css = de.element.style;
				css.border = de.originalBorder;
				css.fontWeight = de.originalFontWeight;
				css.backgroundColor = de.originalBackgroundColor;
				if (de.childButton) {
					de.childButton.style.display = de.originalButtonDisplay;
				}
			}
			else {
				de.element.readOnly = true;
				de.element.disabled = true;
				let css = de.element.style;
				css.setProperty("border", "0px", "important");
				css.fontWeight = "bold";
				css.backgroundColor = "rgb(240,240,240)";
				if (de.childButton) {
					de.childButton.style.display = "none";
				}
			}
		}
	}

	ngAfterViewInit() {
		var inputs = [];
		if (this.el.nativeElement.tagName.toLowerCase() == "input" ||
			this.el.nativeElement.tagName.toLowerCase() == "textarea") {
			inputs.push(this.el.nativeElement);
		}
		else if (this.el.nativeElement.tagName.toLowerCase() == "checklist") {
			inputs.push(this.el.nativeElement.getElementsByClassName("checklist-button")[0]);
		}
		else {
			inputs = this.el.nativeElement.querySelectorAll("input,textarea");
		}
		for (let input of inputs) {
			let de = new DisableElement();
			de.element = input;
			let css = input.style;
			de.originalBorder = css.border;
			de.originalFontWeight = css.fontWeight;
			de.originalBackgroundColor = css.backgroundColor;
			de.originalOutline = css.outline;
			de.originalOnFocus = de.element.onfocus;
			de.element.onfocus = () => {
				if (this.disableEdit) {
					de.element.style.outline = "none";
				}
				else {
					de.element.style.outline = de.originalOutline;
				}
				if (de.originalOnFocus)
					de.originalOnFocus();
			};

			if (de.element.attributes["datetimepicker"] || de.element.attributes["typeahead"]) {
				let btn = de.element.nextElementSibling.getElementsByClassName("input-button-container");
				if (btn.length > 0) {
					de.childButton = btn[0];
					de.originalButtonDisplay = de.childButton.style.display;
				}
			}
			else if (de.element.className && de.element.className.indexOf("checklist-button") >= 0) {
				de.childButton = de.element.getElementsByClassName("drop-down-image")[0];
				de.originalButtonDisplay = de.childButton.style.display;
			}

			this.elements.push(de);
		}
		this._inited = true;
		this.addRemoveStyles();

	}
}

class DisableElement {
	element: any;
	originalBorder: string;
	originalFontWeight: string;
	originalBackgroundColor: string;
	originalOutline: string;
	originalOnFocus: any;

	childButton: any;
	originalButtonDisplay: string;
}
