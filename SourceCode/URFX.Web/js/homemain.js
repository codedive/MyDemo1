'use strict';

/* Controllers */

angular.module('apphome')
//directive for items per page

  .controller('ApphomeCtrl', ['$scope','$translate','$localStorage','$window','authService','ngAuthSettings','$cookies','$rootScope',
    function ($scope, $translate, $localStorage, $window, authService, ngAuthSettings, $cookies,$rootScope) {
        // add 'ie' classes to html
       
        
        $scope.theme = "ltr";
        $scope.Lang;
     $scope.Date= new Date();
        // angular translate
        $scope.lang = { isopen: false };
        $scope.langs = { en: 'English', ar: 'العربية' };
        $scope.selectLang = $scope.langs[$translate.proposedLanguage()] || "English";
        $scope.setLang = function (langKey, $event) {
          
            // set the current lang
            $scope.selectLang = $scope.langs[langKey];
            // You can change the language during runtime
            $cookies.APPLICATION_LANGUAGE = langKey;

            $translate.use(langKey);
            //$scope.lang.isopen = !$scope.lang.isopen;
            ngAuthSettings.currentLanguage = langKey;
            if (ngAuthSettings.currentLanguage == "ar") {
                $scope.theme = "rtl";
                $scope.Lang = langKey;
                $scope.rtlclass = "rtlclass";
                $scope.showArabicLogo = true;
            }
            else {
                $scope.showArabicLogo = false;
                $scope.theme = "ltr";
                $scope.Lang = langKey;
                $scope.rtlclass = "";
            }
        };
        function init() {

            var lang = $cookies.APPLICATION_LANGUAGE || 'en';
            ngAuthSettings.currentLanguage = lang;
            $scope.setLang(lang, '');
        }
        init();
       




    }]);
