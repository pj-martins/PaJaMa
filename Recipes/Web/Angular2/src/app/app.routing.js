"use strict";
var router_1 = require('@angular/router');
var recipes_component_1 = require('./recipes/recipes.component');
var appRoutes = [
    {
        path: '',
        component: recipes_component_1.RecipesComponent
    }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes, { useHash: true });
//# sourceMappingURL=app.routing.js.map