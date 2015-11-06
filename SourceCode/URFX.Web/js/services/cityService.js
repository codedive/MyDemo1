
app.factory('cityService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};
    var _getCity = function () {

        var deferred = $q.defer();

        //tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Cities',

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
    authServiceFactory.getCity = _getCity;
    return authServiceFactory;
}]);