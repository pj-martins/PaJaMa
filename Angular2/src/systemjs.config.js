﻿var JS_CONFIG_VERSION;

(function (global) {


	// map tells the System loader where to look for things
	var map = {
		'rxjs': 'node_modules/rxjs',
		'@angular/core': 'node_modules/@angular/core/bundles/core.umd.js',
		'@angular/http': 'node_modules/@angular/http/bundles/http.umd.js',
		'@angular/router': 'node_modules/@angular/router/bundles/router.umd.js',
		'@angular/forms': 'node_modules/@angular/forms/bundles/forms.umd.js',
		'@angular/common': 'node_modules/@angular/common/bundles/common.umd.js',
		'@angular/platform-browser': 'node_modules/@angular/platform-browser/bundles/platform-browser.umd.js',
		'@angular/platform-browser-dynamic': 'node_modules/@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
		'@angular/compiler': 'node_modules/@angular/compiler/bundles/compiler.umd.js',
		'moment': 'node_modules/moment/min',
		'css': 'node_modules/systemjs-plugin-css/css.js',
	};

	var dt = new Date();
	JS_CONFIG_VERSION = dt.getTime();

	// packages tells the System loader how to load when no filename and/or no extension
	var packages = {
		'app': { main: 'main.js?v=' + JS_CONFIG_VERSION, defaultExtension: 'js' },
		'rxjs': { defaultExtension: 'js' },
		'moment': { main: 'moment.min.js', defaultExtension: 'js' },
		'pajama': { defaultExtension: 'js?v=' + JS_CONFIG_VERSION }
	};

	var config = {
		map: map,
		packages: packages,
		meta: {
			'*.css': { loader: 'css' }
		}
	}

	if (global.filterSystemConfig) { global.filterSystemConfig(config); }

	//SystemJS.resolve = function (key, valu) {
	//	alert(key);
	//	alert(valu);
	//}

	SystemJS.config(config);
})(this);