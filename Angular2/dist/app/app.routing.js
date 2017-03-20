"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var demo_grid_component_1 = require("./demo/demo-grid.component");
var demo_editors_component_1 = require("./demo/demo-editors.component");
var appRoutes = [
    {
        path: 'grid',
        component: demo_grid_component_1.DemoGridComponent
    },
    {
        path: '',
        component: demo_editors_component_1.DemoEditorsComponent
    }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes, { useHash: true });
//# sourceMappingURL=app.routing.js.map