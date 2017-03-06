"use strict";
var router_1 = require('@angular/router');
var gridviewbasicdemo_component_1 = require('./gridview/gridviewbasicdemo.component');
var appRoutes = [
    {
        path: '',
        component: gridviewbasicdemo_component_1.GridViewBasicDemoComponent
    }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes, { useHash: true });
//# sourceMappingURL=app.routing.js.map