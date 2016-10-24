import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Recipe } from './recipe';
import { DataService, Entities, Parameter, Arguments } from '../shared/services/data.service';

@Injectable()
export class RecipeService {
    constructor(private dataService: DataService) { }

    getRandomRecipes(): Observable<Entities<Recipe>> {
        let args = new Arguments();
        args.parameters = new Array<Parameter>();
        args.parameters.push(new Parameter("random", "20"));

        return this.dataService.getEntities<Recipe>('RecipeSearch', args);
    }
}