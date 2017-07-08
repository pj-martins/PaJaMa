﻿import { Component, OnInit } from '@angular/core';
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
	protected recipes: Array<IRecipeCover>;
	protected recipeSources: Array<IRecipeSource>;
	protected totalResults = 0;

	protected includes: Array<string> = [];
	protected excludes: Array<string> = [];
	protected rating = 0;
	protected recipeSourceID: number;
	protected bookmarked = false;
	protected picturesOnly = false;

	protected loading = false;

	constructor(private apiService: ApiService) { }

	ngOnInit() {
		this.loading = true;
		this.apiService.getRandomRecipes(50).subscribe(x => {
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
			this.loading = false;
		});

		this.apiService.getRecipeSources().subscribe((rs: Items<IRecipeSource>) => {
			this.recipeSources = rs.results;
		});
	}

	// we have to define the function as a lambda in order for "this" to refer to the component
	// rather than the js function
	protected getIngredients = (partial: string) => {
		let args = new GetArguments();
		args.filter = `indexof(IngredientName, '${partial.replace("'", "\\'")}') ge 0`;
		args.pageSize = 50;
		return this.apiService.getIngredients(args).map((i: Items<any>) => {
			return i.results;
		});
	}

	protected search() {
		this.loading = true;
		this.apiService.searchRecipes(
			this.includes.length > 0 ? this.includes.join(';') : '',
			this.excludes.length > 0 ? this.excludes.join(';') : '',
			this.rating,
			this.bookmarked,
			this.recipeSourceID,
			this.picturesOnly,
			1,
			20
		).subscribe((r: Items<IRecipeCover>) => {
			this.loading = false;
			this.recipes = r.results;
		});
	}
}