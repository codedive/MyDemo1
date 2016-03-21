
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
    var _getCityById = function (cityId) {

        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Cities/'+cityId,

            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,

            }

        }).success(function (data) {

            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _addCity = function (cityName) {

        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'Post',
            url: serviceBase + '/api/Cities',
            data:cityName,
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
                
            }

        }).success(function (data) {
       
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _updateCity = function (data) {

        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'Put',
            url: serviceBase + '/api/Cities',
            data:data,
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
                
            }

        }).success(function (data) {
       
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }
    var _deleteCity = function (cityId) {

        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'Delete',
            url: serviceBase + '/api/Cities/'+cityId,
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
                'X-HTTP-Method-Override':'Delete'

            }

        }).success(function (data) {

            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }
    authServiceFactory.getCity = _getCity;
    authServiceFactory.getCityById = _getCityById;
    authServiceFactory.addCity=_addCity;
    authServiceFactory.updateCity = _updateCity;
    authServiceFactory.deleteCity = _deleteCity;
    return authServiceFactory;
}]);