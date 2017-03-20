import { Component, Input, ViewChild, ElementRef } from '@angular/core'
import { MultiTextboxComponent, PRE_INPUT, POST_INPUT } from '../multi-textbox/multi-textbox.component';
import { TypeaheadComponent } from './typeahead.component';

@Component({
	moduleId: module.id,
	selector: 'multi-typeahead',
	template: PRE_INPUT +
	 `<typeahead #childTypeahead (keydown)="keyDown($event, true)" [padLeft]="paddingLeft" [dataSource]='dataSource' (focus)='resize()' [displayMember]='displayMember' 
			[valueMember]='valueMember' [minLength]='minLength' [(ngModel)]='currText' (itemSelected)='itemSelected($event)'></typeahead>`
	 + POST_INPUT,
	styleUrls: ['../multi-textbox/multi-textbox.css', 'typeahead.css']
})
export class MultiTypeaheadComponent extends MultiTextboxComponent {
	@ViewChild("childTypeahead") childTypeahead: TypeaheadComponent;

	@Input()
	minLength = 1;

	constructor(protected elementRef: ElementRef) {
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
}