import { Component, Inject, SimpleChanges } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-insured-item-list',
  templateUrl: './insured-item-list.component.html'
})
export class InsuredItemListComponent {
  faTrash = faTrash;

  public categories: Category[] = [
    { id: '1', name: "Clothing" },
    { id: '2', name: "Electronics" },
    { id: '3', name: "Kitchen" }
  ]

  public selectedCategory = this.categories[0];

  public insuredItems: Item[] = [];

  public groupedItemsMap: Map<string, CategoryItemContainer> = new Map();

  public groupItemsTotalValue: string = '';

  public itemName: string = '';
  public itemValue: string = '';

  private httpClient: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;

    http.get<Item[]>(baseUrl + 'LoadCategories').subscribe(result => {
      this.categories = result;
    }, error => console.error(error));

    this.populateTableData();

    console.log(this.insuredItems);
  }

  populateTableData(): void {
    this.httpClient.get<Item[]>(this.baseUrl + 'LoadItems').subscribe(result => {
      this.insuredItems = result;
      this.groupedItemsMap = new Map();

      result.forEach(categoryItemContainer => {
        if (!this.groupedItemsMap.has(categoryItemContainer.category.name)) {
          let categoryItems = new CategoryItemContainer();
          this.groupedItemsMap.set(categoryItemContainer.category.name, categoryItems);
        }
        this.groupedItemsMap.get(categoryItemContainer.category.name)?.items.push(categoryItemContainer);
      });

    }, error => console.error(error));
  }

  public deleteItem(item: Item): void {
    console.log("item deleted");

    let url = this.baseUrl + 'DeleteItems';
    let itemsToDelete: Item[] = [{ id: item.id, name: item.name, category: item.category, value: item.value }];

    this.httpClient.post<any>(url, itemsToDelete).subscribe(result => {
      console.log(result);

    }, error => console.error(error));
    location.reload();
  }

  public sumItemCategoryTotals(): string {
    let total: number = 0;
    this.groupedItemsMap.forEach(categoryItemContainer => {
      total += Number(categoryItemContainer.sumItemValues());
    });
    return total.toString();
  }

  public addItem(): void {
    console.log("item added");
    debugger;

    let url = this.baseUrl + 'SaveItems';
    let newItems: Item[] = [{ id: null, name: this.itemName, category: this.selectedCategory, value: this.itemValue.toString() }];

    this.httpClient.post<any>(url, newItems).subscribe(result => {
      console.log(result);

    }, error => console.error(error));

    location.reload();
  }
}

class CategoryItemContainer {
  items: Item[] = [];

  public sumItemValues(): string {
    let total: number = 0;
    if (this.items !== undefined) {
      this.items.forEach(item => {
        total += Number(item.value);
      });
    }

    return total.toString();
  }
}

interface Category {
  id: string | null;
  name: string;
}

interface Item {
  id: string | null;
  name: string;
  category: Category;
  value: string;
}
