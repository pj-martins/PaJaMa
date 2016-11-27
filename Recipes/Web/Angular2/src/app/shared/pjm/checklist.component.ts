import { Component, Input, Output, EventEmitter, OnInit, ElementRef, NgZone } from '@angular/core';
import { CheckList, CheckListItem } from './checklist';

@Component({
    moduleId: module.id,
    selector: 'check-list',
    template: `
    <div class='check-list id_{{uniqueId}}'>
        <button (click)='dropdownVisible = !dropdownVisible' class="check-list-button check-list-button-formcontrol">{{selectedText ? selectedText : '&nbsp;'}}
            <div class="pull-right drop-down-image glyphicon {{ allSelected ? 'glyphicon-triangle-bottom' : 'glyphicon-filter'}}"></div>
        </button>
        <div class='check-list-dropdown' [hidden]='!dropdownVisible'>
            <div (click)='selectAll()' class='check-list-item check-list-item-all id_{{uniqueId}}'>
                <div class="check-list-check glyphicon {{ allSelected ? 'glyphicon-ok' : 'glyphicon-none'}}"></div>(Select All)</div>
                <div *ngFor='let item of checklist.displayitems' (click)='selectItem(item)' class='check-list id_{{uniqueId}}'>
                <div class='check-list-item id_{{uniqueId}}'>
                    <div class="check-list-check glyphicon {{ item.selected ? 'glyphicon-ok' : 'glyphicon-none'}}"></div>{{item.item}}
                </div>
            </div>
        </div>
    </div>
`,
    styleUrls: ['checklist.css']
})
export class CheckListComponent implements OnInit {
    @Input() checklist: CheckList;
    @Output() selectionChanged: EventEmitter<any> = new EventEmitter<any>();

    constructor(private elementRef: ElementRef, private zone: NgZone) { }

    selectedText: string;
    allSelected: boolean;
    dropdownVisible: boolean;
    currentonclick: any;
    uniqueId = Math.floor((1 + Math.random()) * 0x10000).toString();

    private updateSelection(suspendChange = false) {
        this.selectedText = "";
        this.checklist.selectedItems = [];
        var firstIn = true;
        for (let i = 0; i < this.checklist.displayitems.length; i++) {
            if (this.checklist.displayitems[i].selected || this.allSelected) {
                this.checklist.selectedItems.push(this.checklist.displayitems[i].item);
                if (!this.allSelected && firstIn)
                    this.selectedText = this.checklist.displayitems[i].item;
                firstIn = false;
            }
        }

        if (!this.allSelected && this.checklist.selectedItems.length > 1)
            this.selectedText = "(...)";

        if (!suspendChange)
            this.selectionChanged.emit(this);
    }

    selectItem(item: CheckListItem) {
        item.selected = !item.selected;
        this.allSelected = false;
        this.updateSelection();
    }

    selectAll(forceTrue = false) {
        this.allSelected = forceTrue ? true : !this.allSelected;
        for (var i = 0; i < this.checklist.displayitems.length; i++) {
            this.checklist.displayitems[i].selected = this.allSelected;
        }
        this.updateSelection(forceTrue);
    }

    ngOnInit() {
        this.selectAll(true);
        this.checklist.itemsChanged.subscribe(() => this.selectAll(true));

        // TODO: hackish, need to find a better way to hide drop down when they click off of it, can't use blur
        // since blur will fire when the dropdown div is clicked in which case we don't want to hide the dropdown
        let self = this;
        this.currentonclick = document.onclick;
        document.onclick = (event: any) => {
            if (this.currentonclick) this.currentonclick(event);
            if (self.dropdownVisible && event.target && event.target.className.indexOf(`id_${this.uniqueId}`) < 0 && event.target.offsetParent.className.indexOf(`id_${this.uniqueId}`) < 0) {
                self.zone.run(() => self.dropdownVisible = false);
            }
        };
    }
}