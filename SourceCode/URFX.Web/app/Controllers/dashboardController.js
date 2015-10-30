
(function () {
    'use strict';
    var app = angular.module("UrfxApp");

    app.controller("dashboardController", ['$scope', '$rootScope', 'localStorageService', '$state', 'taskService', '$filter', '$window', 'authService', '$routeParams',
    function ($scope, $rootScope, localStorageService, $state, taskService, $filter, $window, authService, $routeParams) {
       
        $scope.userName = $routeParams.userName;

    }]);

}());