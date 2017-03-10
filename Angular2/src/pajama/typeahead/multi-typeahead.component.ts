import { Component, Input, ViewChild } from '@angular/core'
import { MultiTextboxComponent } from '../multi-textbox/multi-textbox.component';
import { TypeaheadComponent } from './typeahead.component';

// RESEMGLES MULTI TEXTBOX, WATCH CHANGES IN BOTH
@Component({
	moduleId: module.id,
	selector: 'multi-typeahead',
	template: `<div class='multi-textbox-container'>
	<div class='multi-textbox-item-container'>
		<div *ngFor='let item of items || []'>
			<div class='multi-textbox-item'>
				{{item}}
				<div class='glyphicon glyphicon-remove multi-textbox-remove' (click)='removeItem(item)'></div>
			</div>
		</div>
	</div>
	<div class='multi-textbox-textbox'>
		<typeahead #childTypeahead (keydown)="keyDown($event)" (keypress)="keyPress($event)" [dataSource]='dataSource' [displayMember]='displayMember' [valueMember]='valueMember' [minLength]='minLength' [(ngModel)]='currText' (itemSelected)='itemSelected($event)'></typeahead>
	</div>
	<div class='glyphicon glyphicon-plus multi-textbox-add' [style.display]="currText ? 'inline-block' : 'none'" (click)='addItem()'></div>
</div>`,
	styleUrls: ['../multi-textbox/multi-textbox.css', 'typeahead.css']
})
export class MultiTypeaheadComponent extends MultiTextboxComponent {
	@ViewChild("childTypeahead") childTypeahead: TypeaheadComponent;

	@Input()
	minLength = 1;

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
		}
	}
}