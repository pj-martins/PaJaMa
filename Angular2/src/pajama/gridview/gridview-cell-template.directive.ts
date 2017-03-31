import { Component, Input, Compiler, ViewContainerRef, ViewChild, Injectable, OnInit, Directive, ComponentFactoryResolver, ComponentRef } from '@angular/core';
import { GridView, DataColumn, FieldType, IGridViewCellTemplateComponent, IGridViewComponent } from './gridview';
import { GridViewComponent } from './gridview.component';
import { PipesModule } from '../pipes/pipes.module';

@Directive({
	selector: '[gridviewCellTemplate]'
})
export class GridViewCellTemplateDirective implements OnInit, IGridViewCellTemplateComponent {
	private _component: ComponentRef<IGridViewCellTemplateComponent>;
	@Input() column: DataColumn;
	@Input() row: any;
	@Input() parentGridView: GridView;
	@Input() parentGridViewComponent: IGridViewComponent;
	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef) {
	}

	ngOnInit() {
		let factory = this.componentFactoryResolver.resolveComponentFactory(this.column.template);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.row = this.row;
		this._component.instance.column = this.column;
		this._component.instance.parentGridView = this.parentGridView;
		this._component.instance.parentGridViewComponent = this.parentGridViewComponent;
	}
}