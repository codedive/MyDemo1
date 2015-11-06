'use strict';
var app = angular.module("UrfxApp");
app.controller('signupController', ['$scope', '$rootScope', '$location', '$timeout', 'authService', 'categoryService', 'subService', 'subCategory', 'cityService', '$state', 'planService', 'districtService',
function ($scope, $rootScope, $location, $timeout, authService, categoryService, subService, subCategory, cityService, $state, planService, districtService) {
        $scope.showDivSuccessMsz = false;
        $scope.showDivFailedMsz = false;
        $scope.showPaymentPage = false;
        $scope.showRegisterPage = true;
        $scope.selectedPlan;
        $scope.NumberOfTeams = 1;
        $scope.SelectedServices = [];
        $scope.SelectedCity;
        $scope.SelectedDistrict;
        var cityName;
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
       
        $scope.itemPlans = [];
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
            planService.getAllPlans().then(function (response) {
                debugger;
                if ($scope.itemPlans.length==0) {
                    angular.forEach(response, function (value, key) {
                        this.push(value);
                    }, $scope.itemPlans)
                    debugger;
                    $scope.selectedPlan = $scope.itemPlans[0].PlanId;
                }
            });

        }

        $scope.SetValuesForPlan = function (selectedPlan) {
           
           $scope.selectedPlan = selectedPlan;
            angular.forEach($scope.itemPlans, function (value, key) {
                
                
                if (value.PlanId == selectedPlan) {
                    $scope.TeamCost = value.TeamRegistrationFee;
                    $scope.ApplicationFee = value.ApplicationFee;
                    $scope.TotalValue = value.TeamRegistrationFee + value.ApplicationFee;

                }
                
               
            });
            
        }
        $scope.SetValuesForPlanForNumerOfEmployee = function (NumberOfTeams) {
           
            $scope.NumberOfTeams = NumberOfTeams;
            angular.forEach($scope.itemPlans, function (value, key) {
                

                if (value.PlanId == $scope.selectedPlan) {
                    $scope.TeamCost = value.TeamRegistrationFee;
                    $scope.ApplicationFee = value.ApplicationFee;
                    $scope.TotalValue = value.TeamRegistrationFee + value.ApplicationFee;

                }
                

            });
            if ($scope.NumberOfTeams != null && $scope.NumberOfTeams != "" && $scope.NumberOfTeams!=0) {
                $scope.TeamCost = $scope.TeamCost * $scope.NumberOfTeams;
                $scope.TotalValue = $scope.TeamCost + $scope.ApplicationFee;
            }

        }
        
        $scope.signUp = function () {
            debugger;
            // $scope.registration.Services = $scope.SelectedServices;
            $scope.planDetailModel = {
                NumberOfTeams: $scope.NumberOfTeams,
                PlanId: $scope.selectedPlan
            };
            $scope.locationDetailsModel = {
                CityId: $scope.SelectedCity,
                DistrictId: $scope.SelectedDistrict,
            };
            $scope.isSubmit = true;
            $scope.dataModel = {
                model: $scope.registration,
                services:$scope.SelectedServices,
                companyLogoFile: $scope.CompanyLogoFile,
                registrationCertificateFile: $scope.RegistrationCertificateFile,
                gosiCertificateFile: $scope.GosiCertificateFile,
                planModel: $scope.planDetailModel,
                locationModel: $scope.locationDetailsModel,
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

        function getCity() {

            cityService.getCity().then(function (response) {
                //  $state.go('dashboard');

                $scope.itemCity = [];
                angular.forEach(response, function (value, key) {

                    this.push(value);
                }, $scope.itemCity)

            }, function (response) {
                $scope.errorMessage = response.error_description;
            });
        };

        $scope.GetDistrictByCityId = function (cityId) {
            debugger;
            $scope.SelectedCity = cityId.CityId;
             cityName = cityId.Description;
            districtService.GetDistrictByCityId(cityId.CityId).then(function (response) {
                //  $state.go('dashboard');

                $scope.itemDistrict = [];
                angular.forEach(response, function (value, key) {

                    this.push(value);
                }, $scope.itemDistrict)

               

            }, function (response) {
                $scope.errorMessage = response.error_description;
            });
        }

        $scope.GetSelectedDistrict = function (districtId) {
            debugger;
            $scope.SelectedDistrict = districtId.DistrictId
            var districtName = districtId.Description;
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': cityName }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    alert("location : " + results[0].geometry.location.lat() + " " + results[0].geometry.location.lng());
                } else {
                    alert("Something got wrong " + status);
                }
            });
        }
      
        getServiceCategory();

        getCity();

       
    }]);