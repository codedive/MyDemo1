(function () {
    'use strict';
    app.controller('SigninFormController', ['$rootScope',
        '$scope',
        '$location',
        '$state',
        'authService',
        'ngAuthSettings',
        '$timeout',
        'localStorageService',
        '$window',
        '$cookies',
        '$translate',
        '$remember',
        '$http',
        '$stateParams',
        function ($rootScope, $scope, $location, $state, authService, ngAuthSettings, $timeout, localStorageService, $window, $cookies, $translate, $remember, $http, $stateParams) {

            var UserId = $stateParams.UserId;
            var email = $stateParams.email;
            var code = $stateParams.code;
            var userName=$stateParams.userName;
            var password = $stateParams.password;
            debugger;
           
            $scope.ServiceProvider = 0;
            $scope.IndividualUser = 1;

            if (email) {
                authService.decrept(email).then(function (response) {
                    $scope.Email = response;
                })
            }

            $scope.Password;
            $scope.ConfirmPassword;
            $scope.UserId;
            $scope.Code;

            $scope.showPlanButton = false;
            $scope.showDivFailedMsz = false;

            $scope.remember = false;
            $rootScope.showMenu = false;
            $scope.externalLoginProviders = [];

            $scope.loginData = {
                userName: "",
                password: "",
                useRefreshTokens: false
            };

            $scope.message = "";

            $scope.login = function (username, userPassword) {
                debugger;
                if ($scope.remember) {
                    $remember('Username', username);
                    $remember('Password', userPassword);
                } else {
                    $remember('Username', '');
                    $remember('Password', '');
                }
                $scope.errorMessage = "";
                $scope.loginData.userName = username;
                $scope.loginData.password = userPassword;

                authService.login($scope.loginData).then(function (response) {

                    if (response.userName != null && response.userName != '') {
                        $scope.app.currentUser.userName = response.userName;
                        $scope.app.currentUser.userRole = response.roles;
                        $scope.app.currentUser.userId = response.userId;
                        $scope.app.currentUser.planId = parseInt(response.planId);
                       
                         $scope.app.currentUser.type=response.type;
                        if (response.roles == "ServiceProvider") {
                            $state.go('app.serviceprovider.profile', { 'serviceProviderId': response.userId });
                        }
                        else if (response.roles == "Admin") {
                            $state.go('app.administration.serviceprovider');
                        }
                        else {                            
                            $scope.alerts = [];
                            $scope.alerts.push({ type: 'warning', msg: "You are not authorized to login. Please contact at support@urfxco.com" });
                        }
                    }
                }, function (response) {
                    $scope.alerts = [];
                    $scope.alerts.push({ type: 'warning', msg: response.error_description });
                });
            };

            $scope.authExternalProvider = function (provider) {
                var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';
                var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                                                                            + "&response_type=token&client_id=" + ngAuthSettings.clientId
                                                                            + "&redirect_uri=" + redirectUri;
                window.$windowScope = $scope;
                var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
                //// authService.getExternalLogin(provider).then(function (response) {
                //     //$http({
                //     //    method: 'GET',
                //     //    url: serviceBase + 'api/account/ExternalLogin/?provider=' + provider,
                //     //    headers: {
                //     //        'Access-Control-Allow-Origin': '*'
                //     //    }
                //     //});
                //     var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';
                //     var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                //                                                                + "&response_type=token&client_id=" + ngAuthSettings.clientId
                //                                                                + "&redirect_uri=" + redirectUri;
                //     window.$windowScope = $scope;

                //     var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
                // }, function (response) {
                //     $scope.errorMessage = response.error_description;
                //// });
            };

            $scope.authCompletedCB = function (fragment) {
                $scope.$apply(function () {
                    if (fragment.haslocalaccount == 'False') {
                        authService.logOut();
                        authService.externalAuthData = {
                            provider: fragment.provider,
                            userName: fragment.external_user_name,
                            externalAccessToken: fragment.external_access_token
                        };
                        $state.go('associate');
                        //$location.path('/associate');
                    }
                    else {
                        //Obtain access token and redirect to orders
                        var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                        authService.obtainAccessToken(externalData).then(function (response) {
                            $state.go('user');
                        },
                     function (err) {
                         $scope.message = err.error_description;
                     });
                    }
                });
            };            
            var autoLogin = function () {

                if ($remember('Username') !== '' && $remember('Username') !== null) {
                    $scope.remember = true;

                    $scope.username = $remember('Username');
                    $scope.userPassword = $remember('Password');

                    $scope.loginData.userName = $remember('Username');
                    $scope.loginData.password = $remember('Password');
                    $scope.errorMessage = "";
                    authService.login($scope.loginData).then(function (response) {
                        if (response.userName != null && response.userName != '') {
                            $scope.app.currentUser.userName = response.userName;
                            $scope.app.currentUser.userRole = response.roles;
                            $scope.app.currentUser.userId = response.userId;
                            $scope.app.currentUser.planId = parseInt(response.planId);
                            $scope.app.currentUser.type = response.type;
                            if (response.roles == "ServiceProvider") {

                                $state.go('app.serviceprovider.profile', { 'serviceProviderId': response.userId });
                            }
                            else if (response.roles == "Admin") {
                                $state.go('app.administration.serviceprovider');
                            }
                            else {                                
                                $scope.alerts = [];
                                $scope.alerts.push({ type: 'warning', msg: "You are not authorized to login. Please contact at support@urfxco.com" });
                            }
                        }
                    }, function (response) {
                        $scope.alerts = [];
                        $scope.alerts.push({ type: 'warning', msg: response.error_description });
                    });
                }

            }
            
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };

            $scope.ForgotPassword = function (Email) {

                $scope.ForgotPasswordModel = {
                    Email: Email
                }
                authService.forgotPassword($scope.ForgotPasswordModel).then(function (response) {
                    $scope.alerts = [];
                    $scope.alerts.push({ type: 'success', msg: "A reset link has been sent to your email. Please reset your password within 7 days." });

                    $scope.forgotPasswordForm.$setPristine();
                    $scope.ForgotPasswordModel.Email = "";
                    $scope.Email = "";
                    startTimer();
                }, function (response) {                    
                    if (IsJsonString(response.Message)) {
                        var response = JSON.parse(response.Message);
                        if (response.status != undefined) {
                            $scope.alerts = [];
                            $scope.alerts.push({ type: 'warning', msg: response.message });
                        }
                    }
                    else {
                        $scope.alerts = [];
                        $scope.alerts.push({ type: 'warning', msg: "Some thing went wrong please try again" });
                    }
                });
            };

            $scope.ResetPassword = function () {

                $scope.ResetPasswordModel = {
                    Email: $scope.Email,
                    Password: $scope.Password,
                    ConfirmPassword: $scope.ConfirmPassword,
                    Code:code
                }
                authService.resetPassword($scope.ResetPasswordModel).then(function (response) {
                    $scope.alerts = [];


                    $scope.alerts.push({ type: 'success', msg: "Your password has been changed successfully." });

                    $scope.Password = "";
                    $scope.ConfirmPassword = "";
                    $scope.resetPasswordForm.$setPristine();
                    startTimer();
                }, function (response) {
                    
                    if (IsJsonString(response.Message)) {
                        var response = JSON.parse(response.Message);
                        if (response.status != undefined) {
                            $scope.alerts = [];
                            $scope.alerts.push({ type: 'warning', msg: response.message });
                        }
                    }
                    else {
                        $scope.alerts = [];
                        $scope.alerts.push({ type: 'warning', msg: "Your password not changed successfully." });
                    }
                    


                });
            };
            $scope.resendConfirmation = function (Email) {

                
                authService.resendConfirmation(Email).then(function (response) {
                    $scope.alerts = [];
                    $scope.alerts.push({ type: 'success', msg: "Please check your email and confirm your email address." });

                    $scope.resendConfirmationForm.$setPristine();
                   
                    $scope.Email = "";
                    startTimer();
                }, function (response) {
                    if (IsJsonString(response.Message)) {
                        var response = JSON.parse(response.Message);
                        if (response.status != undefined) {
                            $scope.alerts = [];
                            $scope.alerts.push({ type: 'warning', msg: response.message });
                        }
                    }
                    else {
                        $scope.alerts = [];
                        $scope.alerts.push({ type: 'warning', msg: "Some thing went wrong please try again" });
                    }
                });
            };
            if (userName != null && password != null) {
                debugger;
                $scope.login(userName, password);
            }

            $scope.confirmEmail = function () {

                authService.confirmEmail(UserId).then(function (response) {
                    $state.go('confirmemailsuccessfull');

                }, function (response) {
                    $state.go('confirmemailfailed');

                });

            };

            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $state.go('signin');
                }, 10000);
            }
            function IsJsonString(str) {
                try {
                    JSON.parse(str);
                } catch (e) {
                    return false;
                }
                return true;
            }
        }]);
}());