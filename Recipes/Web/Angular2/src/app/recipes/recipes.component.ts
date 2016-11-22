import { Component, OnInit } from '@angular/core';
import { ApiService } from '../shared/services/api.service';
import { IRecipeCover } from '../shared/dto/interfaces';

@Component({
	moduleId: module.id,
	selector: 'recipes',
	templateUrl: 'recipes.component.html'
})
export class RecipesComponent implements OnInit {
	recipes: Array<IRecipeCover>;
	totalResults = 0;

	constructor(private apiService: ApiService) { }

	ngOnInit() {
		this.apiService.getRandomRecipes(20).subscribe(x => {
			this.recipes = x.results;
			this.recipes.sort((a, b) => {
				if (a.recipeName > b.recipeName) {
					return 1;
				}

				if (a.recipeName < b.recipeName) {
					return -1;
				}

				return 0;
			});

			this.totalResults = x.totalRecords;
		});
	}
}