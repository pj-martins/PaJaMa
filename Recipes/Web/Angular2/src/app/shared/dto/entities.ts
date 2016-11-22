// !!!! DO NOT MAKE CHANGES IN HERE HERE THEY WILL GET OVERWRITTEN WHEN TEMPLATE GETS GENERATED !!!!
import { EntityBase } from './entity-base';
import { IRecipeSource, IRecipeImage, IRecipeIngredientMeasurement } from './interfaces';
import { RecipeType } from './enums';

export class Recipe extends EntityBase {
	recipeSource: IRecipeSource;
	recipeImages: Array<IRecipeImage>;
	recipeIngredientMeasurements: Array<IRecipeIngredientMeasurement>;
	recipeName: string;
	directions: string;
	numberOfServings: number;
	rating: number;
	inactive: boolean;
	recipeURL: string;
	recipeType: RecipeType;
	recipeTypeString: string;
}

