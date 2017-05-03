import { Directive, Input, ElementRef, ViewEncapsulation, AfterViewInit } from '@angular/core';

// make sure shared styles.css are reference, for radios, use span to apply visibility/boldness
@Directive({
	selector: '[disableEdit]'
})
export class DisableEditDirective implements AfterViewInit {

	private _disableEdit = false;
	private _inited = false;

	private elements: Array<any> = [];

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
		window.setTimeout(() => {
			for (let el of this.elements) {
				let cls = "disable-edit ";
				if (!this.disableEdit && el.className.indexOf(cls) >= 0) {
					el.className = el.className.replace(cls, "");
				}
				else if (this.disableEdit && el.className.indexOf(cls) < 0) {
					el.className = cls + el.className;
				}
				if (el.tagName.toLowerCase() == 'checklist') {
					for (let btn of el.getElementsByTagName("button")) {
						btn.disabled = this.disableEdit;
					}
				}
				else {
					el.readOnly = this.disableEdit;
					el.disabled = this.disableEdit;
				}
			}
		}, 100);
	}

	ngAfterViewInit() {
		this.elements = [];
		if (this.el.nativeElement.tagName.toLowerCase() == "input" ||
			this.el.nativeElement.tagName.toLowerCase() == "textarea") {
			this.elements.push(this.el.nativeElement);
		}
		else if (this.el.nativeElement.tagName.toLowerCase() == "checklist") {
			this.elements.push(this.el.nativeElement.getElementsByClassName("checklist-button")[0]);
		}
		else {
			this.elements = this.el.nativeElement.querySelectorAll("input,textarea,checklist");
		}
		this._inited = true;
		this.addRemoveStyles();

	}
}