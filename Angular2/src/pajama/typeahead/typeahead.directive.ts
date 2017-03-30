import { Directive, ViewContainerRef, ComponentFactoryResolver, TemplateRef, Input, Output, OnInit, ComponentRef, ElementRef, EventEmitter, OnChanges } from '@angular/core';
import { TypeaheadComponent } from './typeahead.component';
import { ParserService } from '../services/parser.service';
import * as moment from 'moment'

@Directive({
	selector: '[typeahead][ngModel]',
	host: { '(keydown)': 'keydown($event)', '(keyup)': 'keyup($event)' }
})
export class TypeaheadDirective implements OnInit, OnChanges {
	private _component: ComponentRef<TypeaheadComponent>;
	private valueWritten = false;

	@Input("ngModel") ngModel: any;
	@Output("ngModelChange") ngModelChange = new EventEmitter<any>();

	private _initialColor: string;

	@Input()
	get dataSource(): any {
		return this._component.instance.typeahead.dataSource;
	}
	set dataSource(v: any) {
		this._component.instance.typeahead.dataSource = v;
		if (this.valueWritten)
			this.setText(this.ngModel);
	}

	@Input()
	get displayMember(): string {
		return this._component.instance.typeahead.displayMember;
	}
	set displayMember(v: string) {
		this._component.instance.typeahead.displayMember = v;
	}

	@Input()
	get matchOn(): Array<string> {
		return this._component.instance.typeahead.matchOn;
	}
	set matchOn(v: Array<string>) {
		this._component.instance.typeahead.matchOn = v;
	}

	@Input()
	get limitToList(): boolean {
		return this._component.instance.typeahead.limitToList;
	}
	set limitToList(v: boolean) {
		this._component.instance.typeahead.limitToList = v;
	}

	@Input()
	get valueMember(): string {
		return this._component.instance.typeahead.valueMember;
	}
	set valueMember(v: string) {
		this._component.instance.typeahead.valueMember = v;
	}

	@Input()
	get minLength(): number {
		return this._component.instance.typeahead.minLength;
	}
	set minLength(v: number) {
		this._component.instance.typeahead.minLength = v;
	}

	@Input()
	get waitMs(): number {
		return this._component.instance.typeahead.waitMs;
	}
	set waitMs(v: number) {
		this._component.instance.typeahead.waitMs = v;
	}

	@Input()
	get hideButton(): boolean {
		return this._component.instance.typeahead.hideButton;
	}
	set hideButton(v: boolean) {
		this._component.instance.typeahead.hideButton = v;
	}

	@Output()
	get itemSelected(): EventEmitter<any> {
		return this._component.instance.typeahead.itemSelected;
	}
	set itemSelected(v: EventEmitter<any>) {
		this._component.instance.typeahead.itemSelected = v;
	}

	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef, private elementRef: ElementRef,
		private parserService: ParserService) {
		let factory = this.componentFactoryResolver.resolveComponentFactory(TypeaheadComponent);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.typeahead.itemSelected.subscribe(v => {
			this.elementRef.nativeElement.style.color = this.elementRef.nativeElement.style.backgroundColor || "white";
			this.ngModelChange.emit(v);
			//this.setText(v);
		});
		this._initialColor = this.elementRef.nativeElement.style.color;
	}

	ngOnInit() {
		// UGLY, this fires before the initial value
		this._component.instance.typeahead.writeValue(this.ngModel);
		//this.setText(this.ngModel);
		this.valueWritten = true;
	}

	protected keydown(event: any) {
		this._component.instance.typeahead.keydown(event);
	}

	protected keyup(event: any) {
		this._component.instance.typeahead.textValue = this.elementRef.nativeElement.value;
		this._component.instance.typeahead.keyup(event);
	}

	ngOnChanges(changes) {
		// user is typing
		if (this.elementRef.nativeElement == document.activeElement) return;
		this.elementRef.nativeElement.style.color = this.elementRef.nativeElement.style.backgroundColor || "white";
		this.setText(this.ngModel);
	}

	private setText(v: any) {
		let textValue = "";
		if (v === undefined || v === null || v === '') {
			textValue = '';
		}
		else if (!textValue && this.displayMember && !this.valueMember) {
			textValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, v) : v;
		}
		else if (!textValue && this._component.instance.typeahead.items && this._component.instance.typeahead.items.length > 0) {
			for (let item of this._component.instance.typeahead.items) {
				if (this.displayMember && this.valueMember) {
					let val = this.parserService.getObjectValue(this.valueMember, item);
					if (val == v) {
						textValue = this.parserService.getObjectValue(this.displayMember, item);
						break;
					}
				}
				else if (item == v) {
					textValue = item;
					break;
				}
			}
		}
		window.setTimeout(() => {
			this.elementRef.nativeElement.value = textValue;
			this.elementRef.nativeElement.style.color = this._initialColor;
		}, 50);
	}
}