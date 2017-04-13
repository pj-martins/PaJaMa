import { Component, Input, Compiler, ViewContainerRef, ViewChild, Injectable, OnInit, Directive, ComponentFactoryResolver, ComponentRef } from '@angular/core';
import { ITreeViewNodeTemplateComponent, TreeViewNode } from './treeview';
import { PipesModule } from '../pipes/pipes.module';

@Directive({
	selector: '[treeviewNodeTemplate]'
})
export class TreeViewNodeTemplateDirective implements OnInit, ITreeViewNodeTemplateComponent {
	private _component: ComponentRef<ITreeViewNodeTemplateComponent>;
	@Input() node: TreeViewNode;
	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef) {
	}

	ngOnInit() {
		let factory = this.componentFactoryResolver.resolveComponentFactory(this.node.template);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.node = this.node;
	}
}