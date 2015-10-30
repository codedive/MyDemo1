'use strict';
var app = angular.module("UrfxApp");
app.controller('signupController', ['$scope', '$rootScope', '$location', '$timeout', 'authService', 'categoryService', 'subService', 'subCategory', 'countryService', '$state',
function ($scope, $rootScope, $location, $timeout, authService, categoryService, subService, subCategory, countryService,$state) {
        $scope.showDivSuccessMsz = false;
        $scope.showDivFailedMsz = false;
        $scope.showPaymentPage = false;
        $scope.showRegisterPage = true;
        $scope.SelectedServices = [];
        $scope.registration = {
            
            Email: "",
            CompanyName: "",
            CompanyLogoPath: "",
            GeneralManagerName: "",
            CompanyRegistrationNumber: "",
            RegistrationCertificatePath: "",
            GosiCertificatePath: "",
            PhoneNumber: "",
            FaxNumber: "",
            MinimumServicePrice: "",
            Location: "",
            Password: "",
            ConfirmPassword: "",
            Description: "",
           
           

   };
  
        $scope.files = [];

        $scope.savedSuccessfully = false;
        $scope.message = "";

        $scope.errorMessage = "";
        $scope.successMessage = "";

        $scope.cancelSignUp = function () {
            $location.path('/login');
        };

        $scope.CompanyLogo = function (files) {
           
            $scope.CompanyLogoFile = files;
        };

        $scope.RegistrationCertificate = function (files) {
           
            $scope.RegistrationCertificateFile = files;
        };

        $scope.GosiCertificate = function (files) {
            
            $scope.GosiCertificateFile = files;
        };
        
        $scope.PaymntView = function () {
            debugger;
            $scope.showPaymentPage = true;
            $scope.showRegisterPage = false;
        }

        $scope.signUp = function () {
            debugger;
           // $scope.registration.Services = $scope.SelectedServices;
            $scope.isSubmit = true;
            $scope.dataModel = {
                model: $scope.registration,
                services:$scope.SelectedServices,
                companyLogoFile: $scope.CompanyLogoFile,
                registrationCertificateFile: $scope.RegistrationCertificateFile,
                gosiCertificateFile: $scope.GosiCertificateFile
            }
           
            authService.saveRegistration($scope.dataModel).then(function (response) {
                
                $scope.showDivSuccessMsz = true;
                $scope.showPaymentPage = false;
                $scope.showRegisterPage = true;
                $scope.successMessage = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                startTimer();

            },
             function (response) {
                
                 $scope.showPaymentPage = false;
                 $scope.showRegisterPage = true;
                 var errors = [];
                 for (var key in response.data.ModelState) {
                     for (var i = 0; i < response.data.ModelState[key].length; i++) {
                         errors.push(response.data.ModelState[key][i]);
                     }
                 }
                
                 $scope.showDivFailedMsz = true;
                 $scope.failedMessage = "Failed to register user" + errors.join(' ');
             });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/login');
            }, 2000);
        }

        function getServiceCategory() {
           
            categoryService.getServiceCategory().then(function (response) {
                //  $state.go('dashboard');
                
                $scope.itemServiceCategory = [];
                angular.forEach(response, function (value, key) {
                   
                    this.push(value);
                }, $scope.itemServiceCategory)

            }, function (response) {
                $scope.errorMessage = response.error_description;
            });
        };

        $scope.GetSubCategory = function () {
           
           var checkedValue = [];
            var inputElements = document.getElementsByClassName('chkbox');
            for (var i = 0; inputElements[i]; ++i) {
                if (inputElements[i].checked) {
                    checkedValue.push(parseInt(inputElements[i].value));
                   
                }
            }
            if (checkedValue.length == 0) {
                
                $scope.itemSubCategory = [];
                $scope.itemSubService = [];
            }
            else {
                subCategory.getSubCategory(checkedValue).then(function (response) {
                    //  $state.go('dashboard');

                    $scope.itemSubCategory = [];
                    angular.forEach(response, function (value, key) {

                        this.push(value);
                    }, $scope.itemSubCategory)

                }, function (response) {
                    $scope.errorMessage = response.error_description;
                });
            }
          
            
        };

        $scope.GetSubService = function () {
          var checkedValue = [];
            var inputElements = document.getElementsByClassName('subchkbox');
            for (var i = 0; inputElements[i]; ++i) {
                if (inputElements[i].checked) {
                    checkedValue.push(parseInt(inputElements[i].value));
                    
                    
                }
            }
            if (checkedValue.length == 0) {
                $scope.itemSubService = [];
               
            }
            else {
                subService.getSubService(checkedValue).then(function (response) {
                    //  $state.go('dashboard');
                   
                    $scope.itemSubService = [];
                    angular.forEach(response, function (value, key) {
                      
                        this.push(value);
                    }, $scope.itemSubService)

                }, function (response) {
                    $scope.errorMessage = response.error_description;
                });
            }
           
            
        };

        $scope.GetSelectedService = function () {
            
            var SelectedValue = [];
            var inputElementsForService = document.getElementsByClassName('subchkbox');
            for (var i = 0; inputElementsForService[i]; ++i) {
                if (inputElementsForService[i].checked) {
                    SelectedValue.push(parseInt(inputElementsForService[i].value));


                }
            }
            var inputElementsForSubService = document.getElementsByClassName('subServicechkbox');
            for (var i = 0; inputElementsForSubService[i]; ++i) {
                if (inputElementsForSubService[i].checked) {
                    SelectedValue.push(parseInt(inputElementsForSubService[i].value));


                }
            }
           
           
            $scope.SelectedServices = SelectedValue;
        }

        function getCountry() {

            countryService.getCountry().then(function (response) {
                //  $state.go('dashboard');

                $scope.itemCountry = [];
                angular.forEach(response, function (value, key) {

                    this.push(value);
                }, $scope.itemCountry)

            }, function (response) {
                $scope.errorMessage = response.error_description;
            });
        };
      
        getServiceCategory();

        getCountry();
       
    }]);