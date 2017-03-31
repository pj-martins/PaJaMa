import { Directive, ViewContainerRef, ComponentFactoryResolver, TemplateRef, Input, Output, OnInit, ComponentRef, ElementRef, EventEmitter } from '@angular/core';
import { MultiTypeaheadComponent } from './multi-typeahead.component';
import { ParserService } from '../services/parser.service';
import * as moment from 'moment'

@Directive({
	selector: '[multiTypeahead]',
	host: { '(keydown)': 'keydown($event)', '(keyup)': 'keyup($event)', '(focus)': 'resize()' }
})
export class MultiTypeaheadDirective implements OnInit {
	private _component: ComponentRef<MultiTypeaheadComponent>;

	@Input("multiTypeahead")
	get multiTypeahead(): Array<any> {
		return this._component.instance.items;
	}
	set multiTypeahead(v: Array<any>) {
		this._component.instance.items = v;
	}

	@Input()
	get dataSource(): any {
		return this._component.instance.typeahead.dataSource;
	}
	set dataSource(v: any) {
		this._component.instance.typeahead.dataSource = v;
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

	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef, private elementRef: ElementRef,
		private parserService: ParserService) {
		let factory = this.componentFactoryResolver.resolveComponentFactory(MultiTypeaheadComponent);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.itemsChanged.subscribe(i => {
			this.elementRef.nativeElement.value = this._component.instance.currText || "";
		});
	}

	ngOnInit() {
		this._component.instance.paddingChanged.subscribe(p => {
			if (this._component.instance.originalPaddingLeft == -999) {
				let padding = window.getComputedStyle(this.elementRef.nativeElement, null).getPropertyValue('padding-left');
				if (padding) {
					this._component.instance.originalPaddingLeft = parseInt(padding.replace('px', ''));
					if (isNaN(this._component.instance.originalPaddingLeft))
						this._component.instance.originalPaddingLeft = 0;
				}
				else {
					this._component.instance.originalPaddingLeft = 0;
				}
			}
			this.elementRef.nativeElement.style.paddingLeft = this._component.instance.paddingLeft + 'px';
		});
		window.setTimeout(() => {
			this._component.instance.resize();
		}, 100);
	}

	protected keydown(event: any) {
		this._component.instance.typeahead.keydown(event);
		this._component.instance.keydown(event, true);
	}

	protected keyup(event: any) {
		this._component.instance.typeahead.textValue = this.elementRef.nativeElement.value;
		this._component.instance.typeahead.keyup(event);
	}

	protected resize() {
		this._component.instance.resize();
	}
}