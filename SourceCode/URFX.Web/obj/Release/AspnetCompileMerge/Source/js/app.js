'use strict';


var app = angular.module('app', [
    'ngAnimate',
    'ngAria',
    'ngCookies',
    'ngMessages',
    'ngResource',
    'ngSanitize',
    'ngTouch',
    'ngStorage',
    'ui.router',
    'ui.bootstrap',
    'ui.utils',
    'ui.load',
    'ui.jq',
    'oc.lazyLoad',
    'pascalprecht.translate',
    //'ngMaterial',
     'LocalStorageModule',
     'validation',
     'validation.rule',
     'validation.provider',
     'validation.directive',
     'angularUtils.directives.dirPagination',
     'angular-loading-bar',
     'daypilot',
     'delete.confirm',
     
]);
app.factory('pagingservice', function () {
    return {
        pagesize: 10,

    };
});