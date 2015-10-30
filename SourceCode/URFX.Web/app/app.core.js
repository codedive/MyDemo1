(function () {
    'use strict';
    var app = angular.module("UrfxApp", ['ui.router', 'LocalStorageModule', 'ngCookies', 'validation', 'validation.rule', 'validation.provider', 'validation.directive', 'ngCookies', 'pascalprecht.translate']);
    var currentLanguage;
    app.config(function ($stateProvider, $urlRouterProvider, $httpProvider, $translateProvider) {
        $translateProvider.useUrlLoader('/api/account/Get');
        debugger;
        $stateProvider
        .state('login', {
            url: '/login',
            templateUrl: 'app/Views/Account/login.html',
            data: {
                requireLogin: false,
                pageTitle: 'Urfx - Login'
            }
        })
       .state('service-provider-register', {
           url: '/service-provider-register',
           templateUrl: 'app/Views/Account/service-provider-register.html',
           data: {
               requireLogin: false,
               pageTitle: 'Urfx -Service Provider Register'
           }
       })
         .state('dashboard', {
             url: '/dashboard',
             templateUrl: 'app/Views/dashboard.html',
             data: {
                 requireLogin: true,
                 pageTitle: 'Urfx - Dashboard'
             }
         })
         .state('associate', {
             url: '/associate',
             templateUrl: '/app/Views/associate.html',
             data: {
                 requireLogin: true
             }
         })

        .state('payment', {
            url: '/payment',
            templateUrl: '/app/Views/Payment/payment.html',
            data: {
                requireLogin: false,
                pageTitle: 'Urfx - Payment'
            }
        });
        $urlRouterProvider.otherwise('/login');
    });

    var serviceBase = 'http://localhost:29317/'
    // var serviceBase = 'http://112.196.25.162:7040/'
    

    app.constant('ngAuthSettings', {
        apiServiceBaseUri: serviceBase,
        clientId: 'ngAuthApp',
        loginPageTitle: 'Login',
        currentLanguage: '',
    });

    app.run(function ($rootScope, $state, authService, $cookies, $translate, ngAuthSettings, $location) {
        authService.fillAuthData();
        
      //  alert($translate.instant('Register'));
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            debugger;
            if (toState == '') {
                $rootScope.bodyClass = 'login-page';
            }
            else {
                $rootScope.bodyClass = 'login-page';
            }
            authService.fillAuthData();
            var requireLogin = toState.data.requireLogin;
            
            //if (requireLogin) {
            //    $state.go('login');
            //}
            $rootScope.pageTitle = toState.data.pageTitle;
        });
        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            var restrictedPage = $.inArray($location.path(), ['/login', '/service-provider-register']) === -1;
            var loggedIn = authService.authentication.isAuth;
        
            //if (restrictedPage && !loggedIn) {
            //    $location.path('/login');
            //}
            //else if (restrictedPage && loggedIn) {
            //    $location.path('/dashboard');
            //}
        });
        $rootScope.logOut = function () {
            authService.logOut();
            $state.go('login');
        };
         $rootScope.setLanguage=function(lang) {
        
            $cookies.put('APPLICATION_LANGUAGE',lang);
            $translate.use(lang);
            ngAuthSettings.currentLanguage = lang;
            if (lang == 'ar') {
                $rootScope.showArabicLogo = true;
            }
            else {
                $rootScope.showArabicLogo = false;
            }
        }

        function init() {
           
            var lang = $cookies.get('APPLICATION_LANGUAGE') || 'en';
            ngAuthSettings.currentLanguage = lang;            
            $rootScope.setLanguage(lang);
        }
        init();        
    });

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
        $httpProvider.defaults.useXDomain = true;
    });

}());