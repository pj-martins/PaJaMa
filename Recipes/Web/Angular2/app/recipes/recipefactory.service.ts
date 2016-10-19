import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Recipe } from './recipe';
import { EntityFactory, Entities, Parameter, Arguments } from '../services/entityfactory.service';

@Injectable()
export class RecipeFactory {
    constructor(private entityFactory: EntityFactory) { }

    getRandomRecipes(): Observable<Entities<Recipe>> {
        let args = new Arguments();
        args.parameters = new Array<Parameter>();
        args.parameters.push(new Parameter("random", "20"));

        return this.entityFactory.getEntities<Recipe>('RecipeSearch', args);
    }
}