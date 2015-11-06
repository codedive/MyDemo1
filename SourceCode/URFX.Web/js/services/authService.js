'use strict';
var languageType ={
    english:'en-GB',
    arabic: 'ar-SA'
}
app.factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', '$remember',
    function ($http, $q, localStorageService, ngAuthSettings, $location, $remember) {
  
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        useRefreshTokens: false,
        firstName: "",
        lastName: ""
    };

    var _externalAuthData = {
        provider: "",
        userName: "",
        externalAccessToken: ""
    };

    var _saveRegistration = function (dataModel) {
        debugger;
         _logOut();
        return $http({
            method: 'POST',
            url: serviceBase + 'api/account/RegisterServiceProvider',
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
                debugger;
                var formData = new FormData();
                //need to convert our json object to a string version of json otherwise
                // the browser will do a 'toString()' on the object which will result 
                // in the value '[Object object]' on the server.
                formData.append("model", angular.toJson(data.model));
                formData.append("services", angular.toJson(data.services));
                formData.append("planModel", angular.toJson(data.planModel));
                formData.append("LocationModel", angular.toJson(data.locationModel));
                //now add all of the assigned files
                if (data.companyLogoFile !=undefined) {
                    for (var i = 0; i < data.companyLogoFile.length; i++) {

                        //add each file to the form data and iteratively name them
                        formData.append("file" + i, data.companyLogoFile[0]);
                    }
                }
                if (data.registrationCertificateFile != undefined) {
                    for (var i = 0; i < data.registrationCertificateFile.length; i++) {

                        //add each file to the form data and iteratively name them
                        formData.append("file" + i, data.registrationCertificateFile[i]);
                    }
                }
                if (data.gosiCertificateFile != undefined) {
                    for (var i = 0; i < data.gosiCertificateFile.length; i++) {

                        //add each file to the form data and iteratively name them
                        formData.append("file" + i, data.gosiCertificateFile[i]);
                    }
                }
                //console.log(formData);
                return formData;
            },
            //Create an object that contains the model and files which will be transformed
            // in the above transformRequest method
            data: { model: dataModel.model, locationModel: dataModel.locationModel, planModel: dataModel.planModel, services: dataModel.services, companyLogoFile: dataModel.companyLogoFile, registrationCertificateFile: dataModel.registrationCertificateFile, gosiCertificateFile: dataModel.gosiCertificateFile }
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

    var _login = function (loginData) {
        
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        if (loginData.useRefreshTokens) {
            data = data + "&client_id=" + ngAuthSettings.clientId;
        }

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
           
            if (loginData.useRefreshTokens) {
                localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true, firstName: response.firstName, lastName: response.lastName });
            }
            else {
                localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false, firstName: response.firstName, lastName: response.lastName });
            }
            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = response.useRefreshTokens;
            _authentication.firstName = response.firstName;
            _authentication.lastName = response.lastName;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _logOut = function () {
       
        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.useRefreshTokens = false;
        _authentication.firstName = "";
        _authentication.lastName = "";
        $remember('Username', '');
        $remember('Password', '');
    };

    var _fillAuthData = function () {
      
        var authData = localStorageService.get('authorizationData');
        if (authData) {
             debugger;
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
            _authentication.firstName = authData.firstName;
            _authentication.lastName = authData.lastName;
        }
    };

    var _getAuthData = function () {
        _fillAuthData();
        return _authentication;
    };



    var _refreshToken = function () {
        var deferred = $q.defer();
        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }
        return deferred.promise;
    };

    var _obtainAccessToken = function (externalData) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getExternalLogins = function (config) {
       
        var deferred = $q.defer();

        $http.get(serviceBase + 'api/Account/ExternalLogins', config).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });
        return deferred.promise;
    }

    var _registerExternal = function (registerExternalData) {

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

   var _forgotPassword = function (ForgotPasswordModel) {
        debugger;
        var deferred = $q.defer();
        $http({
            method: 'POST',
            url: serviceBase + '/api/Account/ForgotPassword',
            data: ForgotPasswordModel
        }).success(function (data) {
            debugger;
            deferred.resolve(data);
        }).error(function (error) {
            debugger;
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _resetPassword = function (resetpassword) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: serviceBase + '/api/Account/ResetPassword',
            data: resetpassword,
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    };

    var _confirmEmail = function (confirmEmailParams) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: serviceBase + '/api/Account/ConfirmEmail?userId=' + confirmEmailParams.userId + '&code=' + confirmEmailParams.code,
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    

    

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;

    authServiceFactory.obtainAccessToken = _obtainAccessToken;


    authServiceFactory.externalAuthData = _externalAuthData;

    authServiceFactory.getExternalLogins = _getExternalLogins;
    authServiceFactory.registerExternal = _registerExternal;

    authServiceFactory.forgotPassword = _forgotPassword;
    authServiceFactory.resetPassword = _resetPassword;
    authServiceFactory.confirmEmail = _confirmEmail;

    authServiceFactory.getAuthenticationData = _getAuthData;

    

  


    return authServiceFactory;
}]);