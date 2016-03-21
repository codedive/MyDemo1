app.factory('jobService', ['$http' ,'$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

      var serviceBase = ngAuthSettings.apiServiceBaseUri;
   
      var authServiceFactory = {};

      var _getAllJobs = function () {
         
          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'GET',
              url: serviceBase + '/api/Jobs',
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

      var _getJobByServiceProviderId = function (serviceProviderId) {
         
          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'GET',
              url: serviceBase + '/api/Jobs/GetJobByServiceProviderId/?serviceProviderId=' + serviceProviderId,
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
     
      var _getJobByJobId = function (JobId) {
      tokenData = localStorageService.get('authorizationData');
          var deferred = $q.defer();
          $http({
              method: 'GET',
              url: serviceBase + '/api/Jobs/GetJobByJobId/?jobId=' + JobId,
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

      var _updateJob = function (jobId) {
          tokenData = localStorageService.get('authorizationData');
          return $http({
              method: 'Post',
              url: serviceBase + 'api/Jobs/RejectJob/?userId=&jobId='+jobId+'&comment=',
              headers: {
                  'Authorization': 'Bearer ' + tokenData.token,
                  'Accept-Language': ngAuthSettings.currentLanguage,
              }
          }).
          success(function (data, status, headers, config) {

          }).
          error(function (data, status, headers, config) {
              $scope.showDivFailedMsz = true;
              $scope.failedMessage = "Failed";
          });

      };

      var _paging = function (pagingModel) {
          
          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'Post',
              url: serviceBase + '/api/Jobs/Paging',
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

      var _getEmployeeScheduleByEmployeeId = function (employeeId) {

          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'GET',
              url: serviceBase + '/api/Employees/GetEmployeeSchedule/?employeeId=' + employeeId,
              headers: {
                  'Authorization': 'Bearer ' + tokenData.token,
                  'Accept-Language': ngAuthSettings.currentLanguage,
              },
              
          }).success(function (data) {
              deferred.resolve(data);
          }).error(function (error) {
              deferred.reject(error);
          });
          return deferred.promise;
      }

      var _getEmployeeScheduleByDateAndTime = function (params) {

          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'GET',
              url: serviceBase + '/api/Employees/GetEmployeeScheduleByDateAndTime/?start=' + params.start + '&end=' + params.end + '&employeeId=' + params.employeeId,
              headers: {
                  'Authorization': 'Bearer ' + tokenData.token,
                  'Accept-Language': ngAuthSettings.currentLanguage,
              },
              //data: params,
          }).success(function (data) {
              deferred.resolve(data);
          }).error(function (error) {
              deferred.reject(error);
          });
          return deferred.promise;
      }
      
      var _addscheduleForEmployee = function (param) {
          tokenData = localStorageService.get('authorizationData');
          return $http({
              method: 'Post',
              url: serviceBase + 'api/Employees/addscheduleForEmployee',
              data: param,
              headers: {
                 'Authorization': 'Bearer ' + tokenData.token,
                  'Accept-Language': ngAuthSettings.currentLanguage,
              },
          }).
          success(function (data, status, headers, config) {

          }).
          error(function (data, status, headers, config) {
              $scope.showDivFailedMsz = true;
              $scope.failedMessage = "Failed";
          });

      };

      var _updatescheduleForEmployee = function (param) {
          tokenData = localStorageService.get('authorizationData');
          return $http({
              method: 'Post',
              url: serviceBase + 'api/Employees/UpdatescheduleForEmployee',
              data: param,
              headers: {
                  'Authorization': 'Bearer ' + tokenData.token,
                  'Accept-Language': ngAuthSettings.currentLanguage,
              },
          }).
          success(function (data, status, headers, config) {

          }).
          error(function (data, status, headers, config) {
              $scope.showDivFailedMsz = true;
              $scope.failedMessage = "Failed";
          });

      };

      var _deleteEmployeeScheduleById = function (id) {

          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'Delete',
              url: serviceBase + '/api/Employees/DeleteEmployeeSchedule/?id=' + id,
              headers: {
                 'Authorization': 'Bearer ' + tokenData.token,
                  'Accept-Language': ngAuthSettings.currentLanguage,
              },
              
          }).success(function (data) {
              deferred.resolve(data);
          }).error(function (error) {
              deferred.reject(error);
          });
          return deferred.promise;
      }

      var _Confirm = function (employeeId, jobId) {
          tokenData = localStorageService.get('authorizationData');
          return $http({
              method: 'Post',
              url: serviceBase + 'api/Employees/Confirm/?employeeId=' + employeeId + '&jobId=' + jobId,
              
              headers: {
                  'Authorization': 'Bearer ' + tokenData.token,
                  'Accept-Language': ngAuthSettings.currentLanguage,
              },
          }).
          success(function (data, status, headers, config) {

          }).
          error(function (data, status, headers, config) {
              
          });

      };
      
      var _Cancel = function (employeeId, jobId) {
          tokenData = localStorageService.get('authorizationData');
          return $http({
              method: 'Post',
              url: serviceBase + 'api/Employees/Cancel/?employeeId=' + employeeId + '&jobId=' + jobId,

              headers: {
                  'Authorization': 'Bearer ' + tokenData.token,
                  'Accept-Language': ngAuthSettings.currentLanguage,
              },
          }).
          success(function (data, status, headers, config) {

          }).
          error(function (data, status, headers, config) {

          });

      };

      authServiceFactory.paging = _paging;
      authServiceFactory.getAllJobs = _getAllJobs;
      authServiceFactory.getJobByServiceProviderId = _getJobByServiceProviderId;
      authServiceFactory.getJobByJobId = _getJobByJobId;
      authServiceFactory.updateJob = _updateJob;
      authServiceFactory.getEmployeeScheduleByEmployeeId = _getEmployeeScheduleByEmployeeId;
      authServiceFactory.getEmployeeScheduleByDateAndTime = _getEmployeeScheduleByDateAndTime;
      authServiceFactory.addscheduleForEmployee = _addscheduleForEmployee;
      authServiceFactory.updatescheduleForEmployee = _updatescheduleForEmployee;
      authServiceFactory.deleteEmployeeScheduleById = _deleteEmployeeScheduleById;
      authServiceFactory.Confirm = _Confirm;
      authServiceFactory.Cancel = _Cancel;
      return authServiceFactory;

     
    
}]);


