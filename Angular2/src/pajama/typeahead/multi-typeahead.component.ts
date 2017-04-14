import { Component, Input, ViewChild, ElementRef } from '@angular/core'
import { MultiTextboxComponent, MULTITEXTBOX_TEMPLATE } from '../multi-textbox/multi-textbox.component';
import { Typeahead, TYPEAHEAD_TEMPLATE } from './typeahead';
import { ParserService } from '../services/parser.service';

@Component({
	moduleId: module.id,
	selector: 'multi-typeahead',
	template: TYPEAHEAD_TEMPLATE + MULTITEXTBOX_TEMPLATE,
	styleUrls: ['../assets/css/styles.css', '../assets/css/icons.css', '../assets/css/buttons.css', '../multi-textbox/multi-textbox.css', 'typeahead.css']
})
export class MultiTypeaheadComponent extends MultiTextboxComponent {

	typeahead: Typeahead;

	get currText(): string {
		return this.typeahead.textValue;;
	}
	set currText(v: string) {
		this.typeahead.textValue = v;
	}

	constructor(protected elementRef: ElementRef, private parserService: ParserService) {
		super(elementRef);
		this.typeahead = new Typeahead(this.parserService);
		this.typeahead.itemSelected.subscribe(i => this.itemSelected(i));
	}

	protected itemSelected(item) {
		if (item) {
			this.items.push(item);
			window.setTimeout(() => {
				this.currText = "";
				this.itemsChanged.emit(null);
				this.resize();
			}, 10);
		}
	}

	protected itemsAreEqual(item1: any, item2: any): boolean {
		if (!this.typeahead.displayMember)
			return item1 == item2;
		return this.parserService.getObjectValue(this.typeahead.displayMember, item1) ==
			this.parserService.getObjectValue(this.typeahead.displayMember, item2);
	}

	protected getObjectValue(item) {
		if (!this.typeahead.displayMember)
			return item;
		return this.parserService.getObjectValue(this.typeahead.displayMember, item);
	}
}