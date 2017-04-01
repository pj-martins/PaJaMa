import { Component, Input, Output, EventEmitter } from '@angular/core';
import { GridView, DataColumn, FieldType, ColumnSortDirection, PagingType } from './gridview';
import { GridViewComponent } from './gridview.component';
import { PipesModule } from '../pipes/pipes.module';
import { ParserService } from '../services/parser.service';

@Component({
	moduleId: module.id,
	selector: 'gridview-pager',
	styleUrls: ['gridview-pager.css', 'gridview.css'],
	template: `
<div class='show-hide-animation grid-pagination' [hidden]='!(parentGridView.pagingType < 2 && parentGridViewComponent.displayData && parentGridView.data && parentGridView.data.length > 0)'>
    <div [hidden]='parentGridView.pageSize <= 0 || (parentGridView.pagingType == pagingType.Auto ? parentGridViewComponent.unpagedData.length : parentGridView.totalRecords) <= parentGridView.pageSize'>
        <ul class="pagination">
            <li (click)='setPage(1)'>First</li>
            <li (click)='setPage(parentGridView.currentPage - 1)'>Previous</li>
			<li [style.display]="!moreToLeft?'none':'inline-block'" (click)="setPage(parentGridView.currentPage - parentGridView.pageSize)">...</li>
            <li *ngFor="let p of getPageArray()" [ngClass]="{'pagination-selected' : p == parentGridView.currentPage}" (click)='setPage(p)'>{{p}}</li>
			<li [style.display]="!moreToRight?'none':'inline-block'" (click)="setPage(parentGridView.currentPage + parentGridView.pageSize)">...</li>
            <li (click)='setPage(parentGridView.currentPage + 1)'>Next</li>
            <li (click)='gotoLast()'>Last</li>
        </ul>
        <br />
    </div>
	<div [hidden]="(parentGridView.pagingType == 0 ? parentGridViewComponent.unpagedData.length : parentGridView.totalRecords) <= 10">
		Show: <select [(ngModel)]='parentGridView.pageSize' (ngModelChange)='setPage(1)'><option *ngFor='let ps of pageSizes' [value]="ps.size">{{ps.label}}</option></select>&nbsp;&nbsp;&nbsp;&nbsp;
		Showing {{ parentGridViewComponent.displayData.length }} of {{ parentGridView.pagingType == 0 ? parentGridViewComponent.unpagedData.length : parentGridView.totalRecords }} total records.
	</div>
</div>
`
})
export class GridViewPagerComponent {
	@Input() parentGridViewComponent: GridViewComponent;
    @Input() parentGridView: GridView;

	@Output() pageChanged = new EventEmitter<number>();
	@Output() pageChanging = new EventEmitter<any>();

	protected pagingType = PagingType;
	protected moreToLeft = false;
	protected moreToRight = false;

	pageSizes: Array<any> = [{ size: 10, label: '10' }, { size: 25, label: '25' }, { size: 50, label: '50' }, { size: 100, label: '100' }, { size: 0, label: 'All' }];

	setPage(page: number) {
		if (this.pageChanging)
			this.pageChanging.emit(null);
		if (page !== undefined) {
			if (page < 1) page = 1;
			let totalPages = this.getTotalPages();
			if (page > totalPages)
				page = totalPages;

			this.parentGridView.currentPage = page;
		}
		this.parentGridViewComponent.resetDisplayData();
		this.pageChanged.emit(page);
	}

	private getTotalPages(): number {
		let totalItems = (this.parentGridView.pagingType == PagingType.Auto ? (this.parentGridViewComponent.unpagedData ? this.parentGridViewComponent.unpagedData.length : 0) : this.parentGridView.totalRecords);
		let totalPages = Math.ceil(totalItems / this.parentGridView.pageSize);
		return totalPages;
	}

	protected getPageArray() {
		let pageArray: number[] = [];
		let totalPages = this.getTotalPages();
		let start = 1;
		let end = totalPages > 10 ? 10 : totalPages;
		if (totalPages > 10 && this.parentGridView.currentPage > 5) {
			end = this.parentGridView.currentPage + 4;
			if (end > totalPages) {
				end = totalPages;
			}
			start = end - 9;

		}
		for (let i = start; i <= end; i++) {
			pageArray.push(i);
		}

		this.moreToRight = totalPages > end;
		this.moreToLeft = start > 1;

		return pageArray;
	}

	gotoLast() {
		this.setPage(this.getTotalPages());
	}
}
