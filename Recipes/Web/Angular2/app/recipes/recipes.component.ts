import { Component, OnInit } from '@angular/core';
import { RecipeFactory } from './recipefactory.service';
import { Recipe } from './recipe';

@Component({
    selector: 'recipes',
    templateUrl: 'recipes.component.html',
    providers: [RecipeFactory]
})
export class RecipesComponent implements OnInit {
    recipes: Array<Recipe>;
    test: string;

    constructor(private recipeFactory: RecipeFactory) { }

    ngOnInit() {
        this.recipeFactory.getRandomRecipes().subscribe(x => {
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

            this.test = JSON.stringify(this.recipes);
        });
    }
}