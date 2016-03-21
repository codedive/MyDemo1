
app.factory('subService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};
    var _getSubService = function (checkedValue) {
       
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Services/SubServicesByServiceIds',

            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
                'ServiceIds': checkedValue
            }

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }
     var _getSubServiceByServiceId = function (id) {
       
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Services/SubServicesByServiceIds/?id='+id,

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
    var _getAllSubServices = function () {
       
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Services',

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
    var _saveService = function (serviceModal) {
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({
            method: "POST",
            url: serviceBase + "api/Services/SaveService",
            headers: { 'Content-Type': undefined, 'Authorization': 'Bearer ' + tokenData.token },
            transformRequest: function (dataModal) {
                var formData = new FormData();
                angular.forEach(dataModal, function (key, value) {
                    formData.append(key,angular.toJson(value));
                })
            },
            data:serviceModal
        }).success(function (response) {
            deferred.resolve(response)
        }).error(function (error) {
            deferred.reject(error);
        })
        return deferred.promise;
    };
   
    authServiceFactory.getSubService = _getSubService;
    authServiceFactory.getAllSubServices = _getAllSubServices;
    authServiceFactory.getSubServiceByServiceId = _getSubServiceByServiceId;
    return authServiceFactory;
}]);