app.factory('roleService', ['$http' ,'$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

      var serviceBase = ngAuthSettings.apiServiceBaseUri;
   
      var authServiceFactory = {};

      var _getAllRoles = function () {
         
          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'GET',
              url: serviceBase + '/api/Account/Roles',
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


       var _rolePaging = function (pagingModel) {
        
          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'Post',
              url: serviceBase + '/api/Account/RolePaging',
              data: pagingModel,
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
      authServiceFactory.getAllRoles = _getAllRoles;
       authServiceFactory.rolePaging = _rolePaging;
      return authServiceFactory;

     
    
}]);


