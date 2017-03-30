import { Component, Input, ViewChild, ElementRef } from '@angular/core'
import { MultiTextboxComponent, PRE_INPUT, POST_INPUT } from '../multi-textbox/multi-textbox.component';
import { TypeaheadComponent } from './typeahead.component';
import { ParserService } from '../services/parser.service';

@Component({
	moduleId: module.id,
	selector: 'multi-typeahead',
	template: PRE_INPUT +
	`<typeahead #childTypeahead (keydown)="keyDown($event, true)" [matchOn]="matchOn" [padLeft]="paddingLeft + 'px'" [dataSource]='dataSource' (focus)='resize()' [displayMember]='displayMember' 
			[valueMember]='valueMember' [minLength]='minLength' [(ngModel)]='currText' (itemSelected)='itemSelected($event)' [waitMs]='waitMs'></typeahead>`
	+ POST_INPUT,
	styleUrls: ['../multi-textbox/multi-textbox.css', 'typeahead.css']
})
export class MultiTypeaheadComponent extends MultiTextboxComponent {
	@ViewChild("childTypeahead") childTypeahead: TypeaheadComponent;

	@Input()
	matchOn: Array<string>;

	@Input()
	minLength = 1;

	@Input()
	displayMember: string;

	@Input()
	waitMs = 0;

	constructor(protected elementRef: ElementRef, private parserService: ParserService) {
		super(elementRef);
	}

	@Input()
	get dataSource(): any {
		return this.childTypeahead.dataSource;
	}
	set dataSource(val: any) {
		this.childTypeahead.dataSource = val;
	}

	protected itemSelected(item) {
		if (item) {
			this.items.push(item);
			window.setTimeout(() =>
				this.currText = "", 10);
			this.itemsChanged.emit(null);
			this.resize();
		}
	}

	protected itemsAreEqual(item1: any, item2: any): boolean {
		if (!this.displayMember)
			return item1 == item2;
		return this.parserService.getObjectValue(this.displayMember, item1) ==
			this.parserService.getObjectValue(this.displayMember, item2);
	}

	protected getObjectValue(item) {
		if (!this.displayMember)
			return item;
		return this.parserService.getObjectValue(this.displayMember, item);
	}
}