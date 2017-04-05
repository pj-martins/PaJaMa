import { Directive, ViewContainerRef, ComponentFactoryResolver, TemplateRef, Input, Output, ComponentRef, ElementRef, EventEmitter } from '@angular/core';
import { CheckListComponent } from './checklist.component';
import * as moment from 'moment'

@Directive({
	selector: '[checkList]',
	host: { '(click)' : 'editorClick()' }
})
export class CheckListDirective {
	private _component: ComponentRef<CheckListComponent>;

	@Input()
	get showMultiplesEllipses(): boolean {
		return this._component.instance.showMultiplesEllipses;
	}
	set showMultiplesEllipses(v: boolean) {
		this._component.instance.showMultiplesEllipses = v;
		this.elementRef.nativeElement.style.textAlign = v ? "center" : "";
	}

	@Input("checkList")
	get checkList(): Array<any> {
		return this._component.instance.selectedItems;
	}
	set checkList(v: Array<any>) {
		this._component.instance.selectedItems = v;
		this.elementRef.nativeElement.value = this._component.instance.selectedText;
	}

	@Input()
	get dataSource(): Array<any> {
		return this._component.instance.dataSource;
	}
	set dataSource(v: Array<any>) {
		this._component.instance.dataSource = v;
		this.elementRef.nativeElement.value = this._component.instance.selectedText;
	}

	@Input()
	get showFilterIcon(): boolean {
		return this._component.instance.showFilterIcon;
	}
	set showFilterIcon(v: boolean) {
		this._component.instance.showFilterIcon = v;
	}

	@Input()
	get displayMember(): string {
		return this._component.instance.displayMember;
	}
	set displayMember(v: string) {
		this._component.instance.displayMember = v;
	}

	@Input()
	get disableAll(): boolean {
		return this._component.instance.disableAll;
	}
	set disableAll(v: boolean) {
		this._component.instance.disableAll = v;
	}
	
	@Output()
	get selectionChanged() : EventEmitter<any> {
		return this._component.instance.selectionChanged;
	}
	set selectionChanged(v: EventEmitter<any>) {
		this._component.instance.selectionChanged = v;
	}

	protected editorClick() {
		this._component.instance.dropdownVisible = !this._component.instance.dropdownVisible;
	}

	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef, private elementRef: ElementRef) {
		let factory = this.componentFactoryResolver.resolveComponentFactory(CheckListComponent);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.selectionChanged.subscribe(s => {
			this.elementRef.nativeElement.value = this._component.instance.selectedText;
		});
		this.elementRef.nativeElement.readOnly = true;
	}
}