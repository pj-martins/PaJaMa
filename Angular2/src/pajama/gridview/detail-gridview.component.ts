import { forwardRef, Component, Input, Output, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { GridViewComponent } from './gridview.component';
import { DataColumn, GridView, DetailGridView } from './gridview';

@Component({
	selector: 'detail-gridview',
	template: "<gridview [grid]='detailGridViewInstance'></gridview>",
})
export class DetailGridViewComponent implements OnInit {
	@Input() parentGridViewComponent: GridViewComponent;
	@Input() detailGridView: DetailGridView;
	@Input() row: any;

	@ViewChild(GridViewComponent) gridViewComponent: GridViewComponent;

	detailGridViewInstance: DetailGridView;
	private _expanded: boolean;
	private _inited: boolean;

	private editParent() {
		if (!this.parentGridViewComponent.editingRows[this.parentGridViewComponent.getKeyValue(this.row)])
			this.parentGridViewComponent.editRow(this.row);
	}

	ngOnInit() {
		this.detailGridViewInstance = this.detailGridView.createInstance();
		if (this.detailGridView.allowEdit && this.parentGridViewComponent.grid.allowEdit) {
			this.detailGridViewInstance.rowEdit.subscribe(re => this.editParent());
			this.detailGridViewInstance.rowDelete.subscribe(re => this.editParent());
			this.detailGridViewInstance.rowCreate.subscribe(re => this.editParent());
		}
		this.parentGridViewComponent.detailGridViewComponents[this.parentGridViewComponent.getKeyValue(this.row)] = this;
	}

	isExpanded() {
		return this._expanded;
	}

	expandCollapse() {
		this._expanded = !this._expanded;
		if (!this._inited) {
			this._inited = true;
			this.detailGridView.getChildData(this.row).subscribe(d => this.detailGridViewInstance.data = d);
		}
	}
}