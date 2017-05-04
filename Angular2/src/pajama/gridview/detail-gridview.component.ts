import { forwardRef, Component, Input, Output, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { GridViewComponent } from './gridview.component';
import { DataColumn, GridView, DetailGridView, RowArguments } from './gridview';

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

	private get parentKeyFieldName() {
		return this.parentGridViewComponent.grid.keyFieldName;
	}

	private editParent(args: RowArguments) {
		if ((<DetailGridView>args.grid).parentRow[this.parentKeyFieldName] == this.row[this.parentKeyFieldName]) {
			if (!this.parentGridViewComponent.editingRows[this.row[this.parentKeyFieldName]])
				this.parentGridViewComponent.editRow(this.row);
		}
	}

	ngOnInit() {
		this.detailGridViewInstance = this.detailGridView.createInstance(this.row);
		if (this.detailGridView.allowEdit && this.parentGridViewComponent.grid.allowEdit) {
			this.detailGridViewInstance.rowEdit.subscribe((args: RowArguments) => this.editParent(args));
			this.detailGridViewInstance.rowDelete.subscribe((args: RowArguments) => this.editParent(args));
			this.detailGridViewInstance.rowCreate.subscribe((args: RowArguments) => this.editParent(args));
		}
		this.parentGridViewComponent.detailGridViewComponents[this.row[this.parentKeyFieldName]] = this;
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