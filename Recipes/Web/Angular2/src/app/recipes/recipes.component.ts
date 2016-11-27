import { Component, OnInit } from '@angular/core';
import { ApiService } from '../shared/services/api.service';
import { GetArguments, Items } from '../shared/services/data.service';
import { IRecipeCover, IRecipeSource } from '../shared/dto/interfaces';
import 'rxjs/add/operator/map';

@Component({
	moduleId: module.id,
	selector: 'recipes',
	templateUrl: 'recipes.component.html'
})
export class RecipesComponent implements OnInit {
	recipes: Array<IRecipeCover>;
	recipeSources: Array<IRecipeSource>;
	totalResults = 0;

	includes: Array<string> = [];
	excludes: Array<string> = [];
	rating = 0;
	recipeSourceID: number;

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

		this.apiService.getRecipeSources().subscribe((rs: Items<IRecipeSource>) => {
			this.recipeSources = rs.results;
		});
	}

	// we have to define the function as a lambda in order for "this" to refer to the component
	// rather than the js function
	getIngredients = (partial: string) => {
		let args = new GetArguments();
		args.filter = `indexof(IngredientName, '${partial.replace("'", "\\'")}') ge 0`;
		return this.apiService.getIngredients(args).map((i: Items<any>) => {
			return i.results;
		});
	}

	search() {


		this.apiService.searchRecipes(
			this.includes.length > 0 ? this.includes.join(';') : '',
			this.excludes.length > 0 ? this.excludes.join(';') : '',
			this.rating,
			false,
			this.recipeSourceID,
			false,
			1,
			20
		).subscribe((r: Items<IRecipeCover>) => this.recipes = r.results);
	}
}