import { Component, OnInit } from '@angular/core';
import { ApiService } from '../shared/services/api.service';
import { Recipe } from '../shared/dto/entities';

@Component({
    moduleId: module.id,
    selector: 'recipes',
    templateUrl: 'recipes.component.html'
})
export class RecipesComponent implements OnInit {
    recipes: Array<Recipe>;

	constructor(private apiService: ApiService) { }

	ngOnInit() {
		//this.apiService.getRecipeSearchs()
  //      this.recipeService.getRandomRecipes().subscribe(x => {
  //          this.recipes = x.results
  //          this.recipes.sort((a, b) => {
  //              if (a.recipeName > b.RecipeName) {
  //                  return 1;
  //              }

  //              if (a.RecipeName < b.RecipeName) {
  //                  return -1;
  //              }

  //              return 0;
  //          });
  //      });
    }
}