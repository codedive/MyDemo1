'use strict';

app.factory('clientPaymentService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', '$remember',
    function ($http, $q, localStorageService, ngAuthSettings, $location, $remember) {
       
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    

    var _payClientPayment = function (model) {
        
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: serviceBase + '/api/Plans/ClientPayment',
            data: model
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    
    authServiceFactory.payClientPayment = _payClientPayment;

    
    return authServiceFactory;
}]);