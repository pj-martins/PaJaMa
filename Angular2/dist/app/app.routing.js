"use strict";
var router_1 = require('@angular/router');
var demo_component_1 = require('./demo/demo.component');
var appRoutes = [
    {
        path: '',
        component: demo_component_1.DemoComponent
    }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes, { useHash: true });
//# sourceMappingURL=app.routing.js.map