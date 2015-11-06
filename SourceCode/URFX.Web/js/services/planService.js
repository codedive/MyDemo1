
app.factory('planService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};
    var _getAllPlans = function () {
        debugger;
        var deferred = $q.defer();

        //tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Plans',

            headers: {
                //'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,

            }

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }
    var _payNow = function (Model) {
        debugger;
        var deferred = $q.defer();

        //tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: 'https://secure.innovatepayments.com/gateway/remote.html',
            data: Model,
            //headers: {
            //    'Authorization': 'Basic 5567073b6126MDdvz7W7RJx7gH$P',
            //    //'Accept-Language': ngAuthSettings.currentLanguage,
            //    'Access-Control-Allow-Origin': '*',
            //    'Access-Control-Allow-Methods': 'POST, GET, OPTIONS, DELETE, PUT',

            //}

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    authServiceFactory.payNow = _payNow;
    authServiceFactory.getAllPlans = _getAllPlans;
    return authServiceFactory;
}]);