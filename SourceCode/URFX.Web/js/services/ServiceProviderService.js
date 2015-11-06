
app.factory('serviceProviderService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var serviceProviderFactory = {};
    var _getAllServiceProviders = function () {
        debugger;
        var deferred = $q.defer();
        //tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'GET',
            url: serviceBase + '/api/serviceproviders',
            headers: {                
                'Accept-Language': ngAuthSettings.currentLanguage,
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;    }
    serviceProviderFactory.getAllServiceProviders = _getAllServiceProviders;
    return serviceProviderFactory;
}]);