"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var recipe_service_1 = require('./recipe.service');
var RecipesComponent = (function () {
    function RecipesComponent(recipeService) {
        this.recipeService = recipeService;
    }
    RecipesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.recipeService.getRandomRecipes().subscribe(function (x) {
            _this.recipes = x.Entities;
            _this.recipes.sort(function (a, b) {
                if (a.RecipeName > b.RecipeName) {
                    return 1;
                }
                if (a.RecipeName < b.RecipeName) {
                    return -1;
                }
                return 0;
            });
        });
    };
    RecipesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'recipes',
            templateUrl: 'recipes.component.html'
        }), 
        __metadata('design:paramtypes', [recipe_service_1.RecipeService])
    ], RecipesComponent);
    return RecipesComponent;
}());
exports.RecipesComponent = RecipesComponent;
//# sourceMappingURL=recipes.component.js.map