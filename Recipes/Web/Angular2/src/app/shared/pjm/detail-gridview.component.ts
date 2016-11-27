import { forwardRef, Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { GridViewComponent } from './gridview.component';
import { DataColumn, GridView, DetailGridView, DetailGridViewDataEventArgs } from './gridview';

@Component({
    selector: 'detail-gridview',
    template: "<gridview [grid]='detailGridViewInstance'></gridview>",
    // directives: [forwardRef(() => GridViewComponent)]
})
export class DetailGridViewComponent implements OnInit {
    @Input() parentGridViewComponent: GridViewComponent;
    @Input() detailGridView: DetailGridView;
    @Input() row: any;
    @Input() rowIndex: number;
    
    detailGridViewInstance: DetailGridView;
    private _expanded: boolean;
    private _inited: boolean;

    ngOnInit() {
        this.detailGridViewInstance = this.detailGridView.createInstance();
        this.parentGridViewComponent.detailGridViewComponents[this.rowIndex] = this;
    }

    isExpanded() {
        return this._expanded;
    }

    expandCollapse() {
        this._expanded = !this._expanded;
        if (!this._inited)
        {
            this._inited = true;
            this.detailGridView.setChildData.emit(new DetailGridViewDataEventArgs(this.row, this.detailGridViewInstance));
        }
    }
}