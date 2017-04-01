import { Component, Input, Compiler, ViewContainerRef, ViewChild, Injectable, OnInit, Directive, ComponentFactoryResolver, ComponentRef } from '@angular/core';
import { GridView, DataColumn, FieldType, IGridViewRowTemplateComponent, IGridViewComponent } from './gridview';
import { GridViewComponent } from './gridview.component';
import { PipesModule } from '../pipes/pipes.module';

@Directive({
	selector: '[gridviewRowTemplate]'
})
export class GridViewRowTemplateDirective implements OnInit, IGridViewRowTemplateComponent {
	private _component: ComponentRef<IGridViewRowTemplateComponent>;
	@Input() row: any;
	@Input() parentGridView: GridView;
	@Input() parentGridViewComponent: IGridViewComponent;

	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef) {
	}

	ngOnInit() {
		let factory = this.componentFactoryResolver.resolveComponentFactory(this.parentGridView.rowTemplate);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.row = this.row;
		this._component.instance.parentGridView = this.parentGridView;
		this._component.instance.parentGridViewComponent = this.parentGridViewComponent;
	}
}