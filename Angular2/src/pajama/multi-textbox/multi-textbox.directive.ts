import { Directive, ViewContainerRef, ComponentFactoryResolver, TemplateRef, Input, Output, OnInit, ComponentRef, ElementRef, EventEmitter } from '@angular/core';
import { MultiTextboxComponent } from './multi-textbox.component';
import { ParserService } from '../services/parser.service';
import * as moment from 'moment'

@Directive({
	selector: '[multiTextbox]',
	host: { '(keydown)': 'keydown($event)', '(blur)': 'blur()', '(focus)': 'resize()', '(keyup)': 'keyup()' }
})
export class MultiTextboxDirective implements OnInit {
	private _component: ComponentRef<MultiTextboxComponent>;

	@Input("multiTextbox")
	get multiTextbox(): Array<any> {
		return this._component.instance.items;
	}
	set multiTextbox(v: Array<any>) {
		this._component.instance.items = v;
	}
	
	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef, private elementRef: ElementRef,
		private parserService: ParserService) {
		let factory = this.componentFactoryResolver.resolveComponentFactory(MultiTextboxComponent);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.itemsChanged.subscribe(i => {
			this.elementRef.nativeElement.value = this._component.instance.currText || "";
		});
	}
	
	protected keydown(event: any) {
		this._component.instance.keydown(event, false);
	}

	protected resize() {
		this._component.instance.resize();
	}

	protected blur() {
		this._component.instance.blur();
	}

	protected keyup() {
		this._component.instance.currText = this.elementRef.nativeElement.value;
	}

	ngOnInit() {
		this._component.instance.paddingChanged.subscribe(p => {
			this.elementRef.nativeElement.style.paddingLeft = this._component.instance.paddingLeft + 'px';
		});
		window.setTimeout(() => {
			this._component.instance.resize();
		}, 10);
	}
}