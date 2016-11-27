import { EventEmitter } from '@angular/core';

export class CheckList {
    displayitems: CheckListItem[] = [];
    itemsChanged = new EventEmitter<any>();
    selectedItems: any[];
    
    private _items: any[];

    get items() : any[] {
        return this._items;
    }

    set items(value: any[]) {
        if (!value) return;
        this._items = value;
        this.displayitems = [];
        for (let i = 0; i < value.length; i++) {
            var newItem = new CheckListItem(value[i]);
            this.displayitems.push(newItem);
        }
        this.itemsChanged.emit(this);
    }
}
export class CheckListItem {
    selected: boolean;
    
    constructor(public item: any) {
        this.selected = true;
    }
}