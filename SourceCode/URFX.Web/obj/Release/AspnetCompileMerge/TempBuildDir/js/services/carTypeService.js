
app.factory('carTypeService', ['$http' ,'$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

      var serviceBase = ngAuthSettings.apiServiceBaseUri;
   
      var authServiceFactory = {};
      var _getAllCarTypes = function () {

          var deferred = $q.defer();

          tokenData = localStorageService.get('authorizationData');
          $http({

              method: 'GET',
              url: serviceBase + '/api/Employees/GetAllCarTypes',

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

      
       var _addCarType = function (carTypeModel) {
       
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'Post',
            url: serviceBase + '/api/Employees/AddCarType',
            data:carTypeModel,
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
    var _updateCarType = function (carTypeModel) {

        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'Put',
            url: serviceBase + '/api/Employees/UpdateCarType',
            data:carTypeModel,
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
    var _deleteCarType = function (carTypeId) {

        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'delete',
            url: serviceBase + '/api/Employees/DeleteCarType?carTypeId=' + carTypeId,
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
   
       authServiceFactory.getAllCarTypes = _getAllCarTypes;
       authServiceFactory.addCarType = _addCarType;
       authServiceFactory.updateCarType = _updateCarType;
       authServiceFactory.deleteCarType = _deleteCarType;
      return authServiceFactory;
    
}]);