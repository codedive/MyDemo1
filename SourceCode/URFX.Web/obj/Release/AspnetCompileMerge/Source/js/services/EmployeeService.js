
app.factory('employeeService', ['$http' ,'$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

      var serviceBase = ngAuthSettings.apiServiceBaseUri;
   
      var authServiceFactory = {};

      var _updateEmployee = function (dataModel) {
        tokenData = localStorageService.get('authorizationData');
           
          return $http({
              
              method: 'PUT',
              url: serviceBase + 'api/employees',
              //IMPORTANT!!! You might think this should be set to 'multipart/form-data' 
              // but this is not true because when we are sending up files the request 
              // needs to include a 'boundary' parameter which identifies the boundary 
              // name between parts in this multi-part request and setting the Content-type 
              // manually will not set this boundary parameter. For whatever reason, 
              // setting the Content-type to 'false' will force the request to automatically
              // populate the headers properly including the boundary parameter.
              headers: { 'Content-Type': undefined ,'Authorization': 'Bearer ' + tokenData.token},
              //This method will allow us to change how the data is sent up to the server
              // for which we'll need to encapsulate the model data in 'FormData'
              transformRequest: function (data) {
                
                  var formData = new FormData();

                  //need to convert our json object to a string version of json otherwise
                  // the browser will do a 'toString()' on the object which will result 
                  // in the value '[Object object]' on the server.
                  formData.append("model", angular.toJson(data.model));
                 // formData.append("employees", angular.toJson(data.services));
                  formData.append("LocationModel", angular.toJson(data.locationModel));
                  //now add all of the assigned files
                  if (data.ProfileImageFile != undefined) {
                      for (var i = 0; i < data.ProfileImageFile.length; i++) {

                          //add each file to the form data and iteratively name them
                          formData.append("file" + i, data.ProfileImageFile[0]);
                      }
                  }
                  if (data.NationalIdFile != undefined) {
                      for (var i = 0; i < data.NationalIdFile.length; i++) {

                          //add each file to the form data and iteratively name them
                          formData.append("file" + i, data.NationalIdFile[i]);
                      }
                  }
                  if (data.IqamaNumberFile != undefined) {
                      for (var i = 0; i < data.IqamaNumberFile.length; i++) {

                          //add each file to the form data and iteratively name them
                          formData.append("file" + i, data.IqamaNumberFile[i]);
                      }
                  }
                  //console.log(formData);
                  return formData;
              },
              //Create an object that contains the model and files which will be transformed
              // in the above transformRequest method
              data: { model: dataModel.model, locationModel: dataModel.locationModel, ProfileImageFile: dataModel.ProfileImageFile, NationalIdFile: dataModel.NationalIdFile, IqamaNumberFile: dataModel.IqamaNumberFile }
          }).
          success(function (data, status, headers, config) {

          }).
          error(function (data, status, headers, config) {
              //$scope.showDivFailedMsz = true;
              //$scope.failedMessage = "Failed";
          });
          //return $http.post(serviceBase + 'api/account/RegisterServiceProvider', dataModel).then(function (response) {
          //    return response;
          //});

      };

      var _saveEmployee = function (dataModel) {
     // tokenData = localStorageService.get('authorizationData');
          
                return $http({
                    method: 'POST',
                    url: serviceBase + 'api/Employees',
                    //IMPORTANT!!! You might think this should be set to 'multipart/form-data' 
                    // but this is not true because when we are sending up files the request 
                    // needs to include a 'boundary' parameter which identifies the boundary 
                    // name between parts in this multi-part request and setting the Content-type 
                    // manually will not set this boundary parameter. For whatever reason, 
                    // setting the Content-type to 'false' will force the request to automatically
                    // populate the headers properly including the boundary parameter.
                    headers: { 'Content-Type': undefined },
                    //This method will allow us to change how the data is sent up to the server
                    // for which we'll need to encapsulate the model data in 'FormData'
                    transformRequest: function (data) {
             
                        var formData = new FormData();
                        //need to convert our json object to a string version of json otherwise
                        // the browser will do a 'toString()' on the object which will result 
                        // in the value '[Object object]' on the server.
                        formData.append("model", angular.toJson(data.model));
                        formData.append("locationModel", angular.toJson(data.locationModel));
             
                        //now add all of the assigned files
                        if (data.companyLogoFile !=undefined) {
                            for (var i = 0; i < data.companyLogoFile.length; i++) {

                                //add each file to the form data and iteratively name them
                                formData.append("file" + i, data.companyLogoFile[0]);
                            }
                        }
                        if (data.registrationCertificateFile1 != undefined) {
                            for (var i = 0; i < data.registrationCertificateFile1.length; i++) {

                                //add each file to the form data and iteratively name them
                                formData.append("file" + i, data.registrationCertificateFile1[i]);
                            }
                        }
                        if (data.registrationCertificateFile2 != undefined) {
                            for (var i = 0; i < data.registrationCertificateFile2.length; i++) {

                                //add each file to the form data and iteratively name them
                                formData.append("file" + i, data.registrationCertificateFile2[i]);
                            }
                        }
                        //console.log(formData);
                        return formData;
                    },
                    //Create an object that contains the model and files which will be transformed
                    // in the above transformRequest method
                  //  data: { model: dataModel.model, companyLogoFile: dataModel.companyLogoFile, registrationCertificateFile1: dataModel.registrationCertificateFile1, registrationCertificateFile2: dataModel.registrationCertificateFile2 }
                    data: { model: dataModel.model,locationModel:dataModel.locationModel, companyLogoFile: dataModel.companyLogoFile, registrationCertificateFile1: dataModel.registrationCertificateFile1, registrationCertificateFile2: dataModel.registrationCertificateFile2 }
                }).
                success(function (data, status, headers, config) {
           
                }).
                error(function (data, status, headers, config) {
                  //  $scope.showDivFailedMsz = true;
                  //  $scope.failedMessage = "Failed";
                });
                //return $http.post(serviceBase + 'api/account/RegisterServiceProvider', dataModel).then(function (response) {
                //    return response;
                //});

      };

      var _saveServiceProviderAsIndividual = function (dataModel) {

     // tokenData = localStorageService.get('authorizationData');
          return $http({
              method: 'POST',
              url: serviceBase + 'api/Employees/RegisterServiceProviderAsAnIndividual',
              //IMPORTANT!!! You might think this should be set to 'multipart/form-data' 
              // but this is not true because when we are sending up files the request 
              // needs to include a 'boundary' parameter which identifies the boundary 
              // name between parts in this multi-part request and setting the Content-type 
              // manually will not set this boundary parameter. For whatever reason, 
              // setting the Content-type to 'false' will force the request to automatically
              // populate the headers properly including the boundary parameter.
              headers: {
                  'Content-Type': undefined,
                 // 'Authorization': 'Bearer ' + tokenData.token
              },
              //This method will allow us to change how the data is sent up to the server
              // for which we'll need to encapsulate the model data in 'FormData'
              transformRequest: function (data) {

                  var formData = new FormData();
                  //need to convert our json object to a string version of json otherwise
                  // the browser will do a 'toString()' on the object which will result 
                  // in the value '[Object object]' on the server.
                  formData.append("model", angular.toJson(data.model));
                 
                  formData.append("locationModel", angular.toJson(data.locationModel));

                  //now add all of the assigned files
                  if (data.companyLogoFile != undefined) {
                      for (var i = 0; i < data.companyLogoFile.length; i++) {

                          //add each file to the form data and iteratively name them
                          formData.append("file" + i, data.companyLogoFile[0]);
                      }
                  }
                  if (data.registrationCertificateFile1 != undefined) {
                      for (var i = 0; i < data.registrationCertificateFile1.length; i++) {

                          //add each file to the form data and iteratively name them
                          formData.append("file" + i, data.registrationCertificateFile1[i]);
                      }
                  }
                  if (data.registrationCertificateFile2 != undefined) {
                      for (var i = 0; i < data.registrationCertificateFile2.length; i++) {

                          //add each file to the form data and iteratively name them
                          formData.append("file" + i, data.registrationCertificateFile2[i]);
                      }
                  }
                  //console.log(formData);
                  return formData;
              },
              //Create an object that contains the model and files which will be transformed
              // in the above transformRequest method
              //  data: { model: dataModel.model, companyLogoFile: dataModel.companyLogoFile, registrationCertificateFile1: dataModel.registrationCertificateFile1, registrationCertificateFile2: dataModel.registrationCertificateFile2 }
              data: { model: dataModel.model, locationModel: dataModel.locationModel, companyLogoFile: dataModel.companyLogoFile, registrationCertificateFile1: dataModel.registrationCertificateFile1, registrationCertificateFile2: dataModel.registrationCertificateFile2 }
          }).
          success(function (data, status, headers, config) {

          }).
          error(function (data, status, headers, config) {
            //  $scope.showDivFailedMsz = true;
           //   $scope.failedMessage = "Failed";
          });
          //return $http.post(serviceBase + 'api/account/RegisterServiceProvider', dataModel).then(function (response) {
          //    return response;
          //});

      };

      var _getAllEmployee = function () {
    
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'GET',
            url: serviceBase + '/api/Employees',
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

      var _getEmployeeByServiceProviderId = function (serviceProviderId,jobId) {
          
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'GET',
            url: serviceBase + 'api/Employees/GetEmployeeByServiceProviderId/?id=' + serviceProviderId+'&jobId='+jobId,
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

      var _paging = function (pagingModel) {
      
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Post',
            url: serviceBase + '/api/Employees/Paging',
            data:pagingModel,
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;    }

      var _deleteselectedEmployees = function (employee) {
        
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Delete',
            url: serviceBase + '/api/Employees/DeleteSelectedEmployees',
            data:employee,
            headers: {
                'Content-Type': 'application / json',
                'Accept-Language': ngAuthSettings.currentLanguage,
                'Authorization': 'Bearer ' + tokenData.token
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
      }

      var _getEmployeeById = function (id) {
       
          var deferred = $q.defer();
          tokenData = localStorageService.get('authorizationData');
          $http({
              method: 'GET',
              url: serviceBase + '/api/Employees/' + id,
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

      var _deleteEmployee = function (id) {

         var deferred = $q.defer();
         tokenData = localStorageService.get('authorizationData');
         $http({
             method: 'Delete',
             url: serviceBase + '/api/Employees/' + id,
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


      authServiceFactory.paging = _paging;
      authServiceFactory.saveEmployee = _saveEmployee;
      authServiceFactory.getAllEmployees = _getAllEmployee;
      authServiceFactory.deleteselectedEmployees = _deleteselectedEmployees;
      authServiceFactory.deleteEmployee = _deleteEmployee;
      authServiceFactory.getEmployeeById = _getEmployeeById;
      authServiceFactory.updateEmployee = _updateEmployee;
      authServiceFactory.getEmployeeByServiceProviderId = _getEmployeeByServiceProviderId;
      authServiceFactory.saveServiceProviderAsIndividual = _saveServiceProviderAsIndividual;
      authServiceFactory.getAllCarTypes = _getAllCarTypes;
      return authServiceFactory;
    
}]);