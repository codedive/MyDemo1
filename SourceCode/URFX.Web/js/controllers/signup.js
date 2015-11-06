'use strict';

// signup controller
//app.controller('SignupFormController', ['$scope', '$http', '$state', function($scope, $http, $state) {
//    $scope.user = {};
//    $scope.authError = null;
//    $scope.signup = function() {
//      $scope.authError = null;
//      // Try to create
//      $http.post('api/signup', {name: $scope.user.name, email: $scope.user.email, password: $scope.user.password})
//      .then(function(response) {
//        if ( !response.data.user ) {
//          $scope.authError = response;
//        }else{
//          $state.go('app.dashboard-v1');
//        }
//      }, function(x) {
//        $scope.authError = 'Server Error';
//      });
//    };
//  }]);
app.controller('SignupFormController', ['$scope', '$rootScope', '$location', '$timeout', 'authService', 'categoryService', 'subService', 'subCategory', 'cityService', '$state', 'planService', 'districtService',
function ($scope, $rootScope, $location, $timeout, authService, categoryService, subService, subCategory, cityService, $state, planService, districtService) {
    $scope.alerts = [];
        $scope.showPaymentPage = false;
        $scope.showRegisterPage = true;
        $scope.checked = false;
        $scope.SelectedPlan;
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
        // $scope.PayNow = function () {
        //    debugger;
        //    $scope.Model = {
        //        ivp_store:'14900',
        //        ivp_amount:$scope.TotalValue,
        //        ivp_currency:"SAR",
        //        ivp_test:'1',
        //       // ivp_timestamp:'0',
        //        ivp_cart:'ABC123',
        //        ivp_desc:'Items',
        //       // ivp_extra:'none',
        //       // ivp_signature: '937e1f58ce1e0e46e680cc32529b207301380f0f',
        //       ivp_authkey:'ZBcVH~6bMZn-mwH6',
        //       ivp_trantype:'sale',
        //       ivp_tranclass:'ecom',
        //       ivp_cn:'4111111111111111',
        //       ivp_exm:'12',
        //       ivp_exy:'2016',
        //       bill_fname:'Card',
        //       bill_sname:'Holder',
        //       bill_addr1:'Rayad',
        //       bill_city:'Rayad',
        //       bill_country:'SA',
        //       bill_email:'test@test.com',
        //       bill_ip:'112.196.23.174',
        //       ivp_cv:'123'
        //    }
        //   // window.location.href = 'https://secure.innovatepayments.com/gateway/index.html';
        //   // $state.go('https://secure.innovatepayments.com/gateway/index.html', $scope.Model);
        //    planService.payNow($scope.Model).then(function (response) {
              
                
        //    });
           

        //}
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

                }
               
                debugger;
                $scope.SelectedPlan = $scope.itemPlans[0].PlanId;
                $scope.checked = true;
                $scope.SetValuesForPlan($scope.SelectedPlan);
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
                
               // $scope.showDivSuccessMsz = true;
                $scope.showPaymentPage = false;
                $scope.showRegisterPage = true;
               // $scope.successMessage = "User has been registered successfully.";
                $scope.alerts.push({ type: 'warning', msg: "User has been registered successfully please pay for the plan." });
                $scope.PaymntView();
                //startTimer();

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
                 $scope.alerts.push({ type: 'warning', msg: "Failed to register user" + errors.join(' ') });
                
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

        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };
    }]);