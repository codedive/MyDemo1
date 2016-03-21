
app.factory('serviceProviderService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var serviceProviderFactory = {};
    var _getAllServiceProviders = function () {
       
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'GET',
            url: serviceBase + '/api/serviceproviders',
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

    var _deleteServiceProvider = function (id) {
      
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Delete',
            url: serviceBase + '/api/serviceproviders/'+ id,
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

    var _getServiceProviderById = function (id) {
        
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'GET',
            url: serviceBase + '/api/serviceproviders/'+ id,
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
    var _deactivateServiceProvider = function (id,IsActive) {
        
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'POST',
            url: serviceBase + '/api/serviceproviders/DeactivateServiceProvider/?Id='+ id+'&IsActive='+IsActive,
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

    var _updateServiceProvider = function (dataModel) {
        tokenData = localStorageService.get('authorizationData');
       
        return $http({
            method: 'PUT',
            url: serviceBase + 'api/serviceproviders',
            //IMPORTANT!!! You might think this should be set to 'multipart/form-data' 
            // but this is not true because when we are sending up files the request 
            // needs to include a 'boundary' parameter which identifies the boundary 
            // name between parts in this multi-part request and setting the Content-type 
            // manually will not set this boundary parameter. For whatever reason, 
            // setting the Content-type to 'false' will force the request to automatically
            // populate the headers properly including the boundary parameter.
            headers: { 'Content-Type': undefined,'Authorization': 'Bearer ' + tokenData.token },
            //This method will allow us to change how the data is sent up to the server
            // for which we'll need to encapsulate the model data in 'FormData'
            transformRequest: function (data) {
             
                var formData = new FormData();
                
                //need to convert our json object to a string version of json otherwise
                // the browser will do a 'toString()' on the object which will result 
                // in the value '[Object object]' on the server.
                formData.append("model", angular.toJson(data.model));
                formData.append("services", angular.toJson(data.services));
                formData.append("LocationModel", angular.toJson(data.locationModel));
                //now add all of the assigned files
                if (data.companyLogoFile !=undefined) {
                    for (var i = 0; i < data.companyLogoFile.length; i++) {

                        //add each file to the form data and iteratively name them
                        formData.append("companyLogo"+i, data.companyLogoFile[0]);
                    }
                }
                if (data.registrationCertificateFile != undefined) {
                    for (var i = 0; i < data.registrationCertificateFile.length; i++) {

                        //add each file to the form data and iteratively name them
                        formData.append("registrationCertificate"+i, data.registrationCertificateFile[i]);
                    }
                }
                if (data.gosiCertificateFile != undefined) {
                    for (var i = 0; i < data.gosiCertificateFile.length; i++) {

                        //add each file to the form data and iteratively name them
                        formData.append("gosiCertificate"+i, data.gosiCertificateFile[i]);
                    }
                }
                //console.log(formData);
                return formData;
            },
            //Create an object that contains the model and files which will be transformed
            // in the above transformRequest method
            data: { model: dataModel.model, locationModel: dataModel.locationModel, services: dataModel.services, companyLogoFile: dataModel.companyLogoFile, registrationCertificateFile: dataModel.registrationCertificateFile, gosiCertificateFile: dataModel.gosiCertificateFile }
        }).
        success(function (data, status, headers, config) {
           
        }).
        error(function (data, status, headers, config) {
            $scope.showDivFailedMsz = true;
            $scope.failedMessage = "Failed";
        });
        //return $http.post(serviceBase + 'api/account/RegisterServiceProvider', dataModel).then(function (response) {
        //    return response;
        //});

    };

    var _paging = function (pagingModel) {
       
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Post',
            url: serviceBase + '/api/serviceproviders/Paging',
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

    var _deleteSelectedServiceProviders = function (serviceProviders) {
       
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Delete',
            url: serviceBase + '/api/serviceproviders/DeleteSelectedServiceProviders',
            data:serviceProviders,
            headers: {
                'Content-Type': 'application / json',
                'Accept-Language': ngAuthSettings.currentLanguage,
'Authorization': 'Bearer ' + tokenData.token,
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _deleteSelectedServices = function (services) {

        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Delete',
            url: serviceBase + '/api/serviceproviders/DeleteSelectedServices',
            data: services,
            headers: {
                'Content-Type': 'application / json',
                'Accept-Language': ngAuthSettings.currentLanguage,
'Authorization': 'Bearer ' + tokenData.token,
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _addServices = function (model) {

        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Post',
            url: serviceBase + '/api/serviceproviders/SaveServices',
            headers: {
'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
            },
            data: model
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _deleteServices = function (id) {

        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Delete',
            url: serviceBase + '/api/serviceproviders/DeleteServicesForServiceProvider/?id=' + id,
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

      serviceProviderFactory.getAllServiceProviders = _getAllServiceProviders;
      serviceProviderFactory.updateServiceProvider = _updateServiceProvider;
      serviceProviderFactory.deleteServiceProvider = _deleteServiceProvider;
      serviceProviderFactory.getServiceProviderById = _getServiceProviderById;
      serviceProviderFactory.deleteSelectedServiceProviders = _deleteSelectedServiceProviders;
      serviceProviderFactory.paging = _paging;
      serviceProviderFactory.addServices = _addServices;
      serviceProviderFactory.deleteServices = _deleteServices;
      serviceProviderFactory.deleteSelectedServices = _deleteSelectedServices;
      serviceProviderFactory.deactivateServiceProvider=_deactivateServiceProvider;
      return serviceProviderFactory;
    
}]);