
app.factory('districtService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};
    var _GetDistrictByCityId = function (cityId) {

        var deferred = $q.defer();

        //tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Districts/' + cityId,

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
    authServiceFactory.GetDistrictByCityId = _GetDistrictByCityId;
    return authServiceFactory;
}]);