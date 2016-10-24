import { Component, OnInit } from '@angular/core';
import { RecipeService } from './recipe.service';
import { Recipe } from './recipe';

@Component({
    moduleId: module.id,
    selector: 'recipes',
    templateUrl: 'recipes.component.html'
})
export class RecipesComponent implements OnInit {
    recipes: Array<Recipe>;

    constructor(private recipeService: RecipeService) { }

    ngOnInit() {
        this.recipeService.getRandomRecipes().subscribe(x => {
            this.recipes = x.Entities
            this.recipes.sort((a, b) => {
                if (a.RecipeName > b.RecipeName) {
                    return 1;
                }

                if (a.RecipeName < b.RecipeName) {
                    return -1;
                }

                return 0;
            });
        });
    }
}