(function () {
    'use strict';
    app.controller('SigninFormController', ['$rootScope', '$scope', '$location', '$state', 'authService', 'ngAuthSettings', '$timeout', 'localStorageService', '$window', '$cookies', '$translate', '$remember',
        function ($rootScope, $scope, $location, $state, authService, ngAuthSettings, $timeout, localStorageService, $window, $cookies, $translate, $remember) {
            $scope.Email;
            $scope.Password;
            $scope.ConfirmPassword;
            $scope.alerts = [];

            $scope.showDivSuccessMsz = false;
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
                       $state.go('app.administration.dashboard');
                    }
                }, function (response) {
                    $scope.alerts.push({ type: 'warning', msg: response.error_description });                    
                });
            };

            //$scope.authExternalProvider = function (provider) {
            //    debugger;
            //    var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';
            //    var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
            //                                                                + "&response_type=token&client_id=" + ngAuthSettings.clientId
            //                                                                + "&redirect_uri=" + redirectUri;
            //    window.$windowScope = $scope;
            //    var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
            //};

            //$scope.authCompletedCB = function (fragment) {
            //    $scope.$apply(function () {
            //        if (fragment.haslocalaccount == 'False') {
            //            authService.logOut();
            //            authService.externalAuthData = {
            //                provider: fragment.provider,
            //                userName: fragment.external_user_name,
            //                externalAccessToken: fragment.external_access_token
            //            };
            //            $state.go('associate');
            //            //$location.path('/associate');
            //        }
            //        else {
            //            //Obtain access token and redirect to orders
            //            var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
            //            authService.obtainAccessToken(externalData).then(function (response) {
            //                $state.go('user');
            //            },
            //         function (err) {
            //             $scope.message = err.error_description;
            //         });
            //        }
            //    });
            //};
            //$scope.init = function () {

            //    var config = {
            //        params: {
            //            returnUrl: "/",
            //            generateState: true
            //        }
            //    };
            //    authService.getExternalLogins(config).then(function (response) {

            //        angular.forEach(response, function (value, key) {
            //            this.push(value);
            //        }, $scope.externalLoginProviders);
            //        console.log(JSON.stringify($scope.externalLoginProviders));
            //    }, function (response) {
            //        $scope.errorMessage = response.error_description;
            //    });
            //}
            //$scope.init();            
            var autoLogin = function () {
                debugger;
                if ($remember('Username') !== '' && $remember('Password') !=='') {
                    $scope.remember = true;

                    $scope.username = $remember('Username');
                    $scope.userPassword = $remember('Password');

                    $scope.loginData.userName = $remember('Username');
                    $scope.loginData.password = $remember('Password');
                    $scope.errorMessage = "";
                    authService.login($scope.loginData).then(function (response) {
                        if (response.userName != null && response.userName != '') {
                            $scope.app.currentUser.userName = response.userName;
                            $state.go('app.administration.dashboard');
                        }
                    }, function (response) {
                        $scope.alerts.push({ type: 'warning', msg: response.error_description });                    
                    });
                }
                
            }
            $scope.init = function () {
                autoLogin();
            }
            $scope.init();
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };

            $scope.ForgotPassword = function (Email) {
                
                $scope.ForgotPasswordModel = {
                    Email: Email
                }
                authService.forgotPassword($scope.ForgotPasswordModel).then(function (response) {
                   
                    $scope.alerts.push({ type: 'warning', msg: "A reset link sent to your email address, please check it in 7 days." });

                }, function (response) {
                   
                    $scope.alerts.push({ type: 'warning', msg: response.Message });
                 
                    
                });
            };

            $scope.ResetPassword = function () {
                debugger;
                $scope.ResetPasswordModel = {
                    Email: $scope.Email,
                    Password:$scope.Password,
                    ConfirmPassword:$scope.ConfirmPassword
                }
                authService.resetPassword($scope.ResetPasswordModel).then(function (response) {

                    $scope.alerts.push({ type: 'warning', msg: "Your password reset successfully." });

                }, function (response) {

                    $scope.alerts.push({ type: 'warning', msg: response.Message });


                });
            };
        }]);
}());