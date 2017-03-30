import { Directive, ViewContainerRef, ComponentFactoryResolver, TemplateRef, Input, Output, OnInit, ComponentRef, ElementRef, EventEmitter } from '@angular/core';
import { TypeaheadComponent } from './typeahead.component';
import { ParserService } from '../services/parser.service';
import * as moment from 'moment'

@Directive({
	selector: '[typeahead][ngModel]',
	host: { '(keydown)': 'keydown($event)', '(keyup)': 'keyup($event)' }
})
export class TypeaheadDirective implements OnInit {
	private _component: ComponentRef<TypeaheadComponent>;
	private valueWritten = false;

	@Input("ngModel") ngModel: any;
	@Output("ngModelChange") ngModelChange = new EventEmitter<any>();

	private _initialColor: string;

	@Input()
	get dataSource(): any {
		return this._component.instance.dataSource;
	}
	set dataSource(v: any) {
		this._component.instance.dataSource = v;
		if (this.valueWritten)
			this.setText(this.ngModel);
	}

	@Input()
	get displayMember(): string {
		return this._component.instance.displayMember;
	}
	set displayMember(v: string) {
		this._component.instance.displayMember = v;
	}

	@Input()
	get matchOn(): Array<string> {
		return this._component.instance.matchOn;
	}
	set matchOn(v: Array<string>) {
		this._component.instance.matchOn = v;
	}

	@Input()
	get limitToList(): boolean {
		return this._component.instance.limitToList;
	}
	set limitToList(v: boolean) {
		this._component.instance.limitToList = v;
	}

	@Input()
	get valueMember(): string {
		return this._component.instance.valueMember;
	}
	set valueMember(v: string) {
		this._component.instance.valueMember = v;
	}

	@Input()
	get minLength(): number {
		return this._component.instance.minLength;
	}
	set minLength(v: number) {
		this._component.instance.minLength = v;
	}

	@Input()
	get waitMs(): number {
		return this._component.instance.waitMs;
	}
	set waitMs(v: number) {
		this._component.instance.waitMs = v;
	}

	@Input()
	get hideButton(): boolean {
		return this._component.instance.hideButton;
	}
	set hideButton(v: boolean) {
		this._component.instance.hideButton = v;
	}

	@Output()
	get itemSelected(): EventEmitter<any> {
		return this._component.instance.itemSelected;
	}
	set itemSelected(v: EventEmitter<any>) {
		this._component.instance.itemSelected = v;
	}

	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef, private elementRef: ElementRef,
		private parserService: ParserService) {
		let factory = this.componentFactoryResolver.resolveComponentFactory(TypeaheadComponent);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.valueChanged.subscribe(v => {
			this.elementRef.nativeElement.style.color = this.elementRef.nativeElement.style.backgroundColor || "white";
			this.ngModelChange.emit(v);
			this.setText(v);
		});
		this._initialColor = this.elementRef.nativeElement.style.color;
		// UGLY, this fires before the initial value
		// blank it first so we can format the text without flickering unformatted
		this.elementRef.nativeElement.style.color = this.elementRef.nativeElement.style.backgroundColor || "white";
	}

	ngOnInit() {
		// UGLY, this fires before the initial value
		window.setTimeout(() => {
			this._component.instance.writeValue(this.ngModel);
			this.setText(this.ngModel);
			this.valueWritten = true;
		}, 10);
	}

	protected keydown(event: any) {
		this._component.instance.keydown(event);
	}

	protected keyup(event: any) {
		this._component.instance.textValue = this.elementRef.nativeElement.value;
		this._component.instance.keyup(event);
	}

	private setText(v: any) {
		let textValue = "";
		if (v === undefined || v === null || v === '') {
			textValue = '';
		}
		else if (!textValue && this.displayMember && !this.valueMember) {
			textValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, v) : v;
		}
		else if (!textValue && this._component.instance.items && this._component.instance.items.length > 0) {
			for (let item of this._component.instance.items) {
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
		}, 10);
	}
}