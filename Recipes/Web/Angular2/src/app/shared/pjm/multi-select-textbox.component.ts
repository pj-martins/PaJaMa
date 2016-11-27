import { Component, Input } from '@angular/core'
import { TypeaheadComponent } from './typeahead.component';

@Component({
	moduleId: module.id,
	selector: 'multi-select-textbox',
	template: `<div class='multi-select-textbox-container'>
	<div class='multi-select-textbox-item-container'>
		<div *ngFor='let item of items || []'>
			<div class='multi-select-textbox-item'>
				{{item}}
				<div class='glyphicon glyphicon-remove multi-select-textbox-remove' (click)='removeItem(item)'></div>
			</div>
		</div>
	</div>
	<div class='multi-select-textbox-input-container'>
		<input type='text' [(ngModel)]='currText' (keydown)="keyDown($event)" (keypress)="keyPress($event)" *ngIf='!dataSource' />
		<typeahead *ngIf='dataSource' (keydown)="keyDown($event)" (keypress)="keyPress($event)" [dataSource]='dataSource' [displayMember]='displayMember' [valueMember]='valueMember' [disableFormControl]='true' [minLength]='minLength' [(ngModel)]='currText' (itemSelected)='itemSelected($event)'></typeahead>
	</div>
	<div class='glyphicon glyphicon-plus multi-select-textbox-add' (click)='addItem()'></div>
</div>`,
	styleUrls: ['multi-select-textbox.css']
})
export class MultiSelectTextboxComponent {
	@Input()
	items: Array<string> = []

	// passthrough to typeahead if applicable
	@Input()
	dataSource: any;

	@Input()
	displayMember: string;

	@Input()
	valueMember: string;

	@Input()
	minLength = 1;

	protected currText: string;

	protected removeItem(item: string) {
		for (let i = this.items.length - 1; i >= 0; i--) {
			if (this.items[i] == item) {
				this.items.splice(i, 1);
				break;
			}
		}
	}

	protected addItem() {
		if (this.currText) {
			this.items.push(this.currText);
			this.currText = "";
		}
	}

	protected keyPress(event: KeyboardEvent) {
		if (event.which == 13) {
			this.addItem();
			event.preventDefault();
		}
	}

	protected keyDown(event: any) {
		let charCode = event.which || event.keyCode;
		if (charCode == 8) {
			if (!this.currText && this.items.length > 0) {
				this.items.splice(this.items.length - 1, 1);
			}
		}
	}

	protected itemSelected(item) {
		if (item) {
			this.items.push(item);
			this.currText = "";
		}
	}
}