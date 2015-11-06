'use strict';

/* Controllers */

angular.module('app')
  .controller('AppCtrl', ['$scope', '$translate', '$localStorage', '$window', 'authService','$state','ngAuthSettings','$cookies',
    function ($scope, $translate, $localStorage, $window,authService,$state,ngAuthSettings,$cookies) {
        // add 'ie' classes to html
        var isIE = !!navigator.userAgent.match(/MSIE/i);
        $scope.theme = "ltr";
        $scope.Lang;
        
        isIE && angular.element($window.document.body).addClass('ie');
        isSmartDevice($window) && angular.element($window.document.body).addClass('smart');       
        
        var loadCurrentUser = function(){
            var currentUser = authService.getAuthenticationData();
            if (currentUser != null && currentUser != '') {
                return currentUser;
            }
            else {
                return '';
            }
        }
        // config
        $scope.app = {
            name: 'URFX',
            version: '2.0.2',
            // for chart colors
            color: {
                primary: '#7266ba',
                info: '#23b7e5',
                success: '#27c24c',
                warning: '#fad733',
                danger: '#f05050',
                light: '#e8eff0',
                dark: '#3a3f51',
                black: '#1c2b36'
            },
            settings: {
                themeID: 1,
                navbarHeaderColor: 'bg-info dker',
                navbarCollapseColor: 'bg-info dk',
                asideColor: 'bg-black',
                headerFixed: true,
                asideFixed: false,
                asideFolded: false,
                asideDock: false,
                container: false
            },
            currentUser: {                
                userName: loadCurrentUser().userName
            },
            currentDate: new Date()
        }

        // save settings to local storage
        //if (angular.isDefined($localStorage.settings)) {
        //    $scope.app.settings = $localStorage.settings;
        //} else {
        //    $localStorage.settings = $scope.app.settings;
        //}
        $scope.$watch('app.settings', function () {
            if ($scope.app.settings.asideDock && $scope.app.settings.asideFixed) {
                // aside dock and fixed must set the header fixed.
                $scope.app.settings.headerFixed = true;
            }
            // for box layout, add background image
            $scope.app.settings.container ? angular.element('html').addClass('bg') : angular.element('html').removeClass('bg');
            // save to local storage
            $localStorage.settings = $scope.app.settings;
        }, true);

        // angular translate
        $scope.lang = { isopen: false };
        $scope.langs = { en: 'English', ar: 'Arabian' };
        $scope.selectLang = $scope.langs[$translate.proposedLanguage()] || "English";
        $scope.setLang = function (langKey, $event) {
            debugger;
            // set the current lang
            $scope.selectLang = $scope.langs[langKey];
            // You can change the language during runtime
            $cookies.APPLICATION_LANGUAGE= langKey;
           
            $translate.use(langKey);
            //$scope.lang.isopen = !$scope.lang.isopen;
            ngAuthSettings.currentLanguage = langKey;
            if (ngAuthSettings.currentLanguage == "ar") {
                $scope.theme = "rtl";
                $scope.Lang = langKey;
                $scope.showArabicLogo = true;
            }
            else {
            $scope.showArabicLogo = false;
                $scope.theme = "ltr";
                $scope.Lang = langKey;
                
            }
        };
        function init() {

            var lang = $cookies.APPLICATION_LANGUAGE || 'en';
            ngAuthSettings.currentLanguage = lang;
            $scope.setLang(lang,'');
        }
        init();
        function isSmartDevice($window) {
            // Adapted from http://www.detectmobilebrowsers.com
            var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
            // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
            return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
        };
        $scope.logout = function () {
            $scope.currentUser = [];
            authService.logOut();
            $state.go('signin');
        };

        

    }]);
