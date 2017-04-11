//import { Component, Input, ElementRef, ViewEncapsulation } from '@angular/core';

//// this really could be a directive but we need the stylesheet
//@Component({
//	moduleId: module.id,
//	selector: '[disableEdit]',
//	template: '',
//	styleUrls: ['disable-edit.css'],
//	encapsulation: ViewEncapsulation.None
//})
//export class DisableEditComponent {

//	private _disableEdit = false;

//	@Input()
//	get disableEdit(): boolean {
//		return this._disableEdit;
//	}
//	set disableEdit(v: boolean) {
//		this._disableEdit = v;
//		this.addRemoveClass();
//	}

//	constructor(private el: ElementRef) {
//		this.addRemoveClass();
//	}

//	private addRemoveClass() {
//		var inputs = [];
//		if (this.el.nativeElement.tagName.toLowerCase() == "input" ||
//			this.el.nativeElement.tagName.toLowerCase() == "textarea") {
//			inputs.push(this.el.nativeElement);
//		}
//		else {
//			throw "NOT IMPLEMENTED";
//		}
//		for (let input of inputs) {
//			if (!this.disableEdit) {
//				input.className = input.className.replace(" disable-edit", "");
//				input.readOnly = false;
//			}
//			else if (input.className.indexOf(" disable-edit") < 0) {
//				input.readOnly = true;
//				input.className += " disable-edit";
//			}
//		}
//	}
//}