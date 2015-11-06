// config

//var serviceBase = 'http://localhost:29317/';


var serviceBase = 'http://112.196.25.162:7040/';

var app =
angular.module('app')
  .config(
    ['$controllerProvider', '$compileProvider', '$filterProvider', '$provide',
    function ($controllerProvider, $compileProvider, $filterProvider, $provide) {

        // lazy controller, directive and service
        app.controller = $controllerProvider.register;
        app.directive = $compileProvider.directive;
        app.filter = $filterProvider.register;
        app.factory = $provide.factory;
        app.service = $provide.service;
        app.constant = $provide.constant;
        app.value = $provide.value;
    }
    ])
  .config(['$translateProvider', function ($translateProvider) {
      // Register a loader for the static files
      // So, the module will search missing translation tables under the specified urls.
      // Those urls are [prefix][langKey][suffix].
      $translateProvider.useStaticFilesLoader({
          prefix: 'l10n/',
          suffix: '.js'
      });
      // Tell the module what language to use by default
      $translateProvider.preferredLanguage('en');
      // Tell the module to store the language in the local storage
      $translateProvider.useLocalStorage();

      $translateProvider.useUrlLoader('/api/account/Get');
  }])
  .config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
        $httpProvider.defaults.useXDomain = true;
    })
    .constant('ngAuthSettings', {
        apiServiceBaseUri: serviceBase,
        clientId: 'ngAuthApp',
        loginPageTitle: 'Login',
        currentLanguage: '',
    });