app.controller('ServiceProviderController', ['$scope', '$http', 'serviceProviderService', '$stateParams', '$state', 'cityService', 'districtService', 'subCategory', 'subService', 'categoryService', 'authService', 'employeeService', 'pagingservice', '$modal', '$log', 'modalService', '$anchorScroll', '$rootScope', 'localStorageService', '$timeout',
function ($scope, $http, serviceProviderService, $stateParams, $state, cityService, districtService, subCategory, subService, categoryService, authService, employeeService, pagingservice, $modal, $log, modalService, $anchorScroll, $rootScope, localStorageService, $timeout) {
    $scope.IsMultipleDelete = false;
    // $scope.PlanPurchased = false;
    $scope.filteredValues;
    //$scope.ServiceTypeText = "";
    $scope.ServiceProviderType;
    $scope.Price;
    
    
    $scope.Latitude;
    $scope.Longitude;
    $scope.SelectedServiceDetails = [];
    $scope.RegistrationCertificateFile;
    getCity();
    //  $scope.alerts = [];

    var serviceProviderId = $stateParams.serviceProviderId;

    var getMessages = function () {

        var successMsg = localStorageService.get('SuccessMsg');
        if (successMsg != null && successMsg != "" && successMsg != undefined) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: successMsg });
            localStorageService.remove('SuccessMsg');
        }
    }
    getMessages();
    getAllServiceProviders = function () {
        $rootScope.showLoader = true;
        serviceProviderService.getAllServiceProviders().then(function (response) {

            if (response != undefined && response != null)
                $scope.serviceProviders = response;
            $scope.totalcount = $scope.serviceProviders.length;
            sessionStorage.setItem("totalCount", $scope.totalcount);
            for (var i = 1; i <= $scope.totalcount; i++) {
                $scope.items.push({ id: i, name: "name " + i });
            }

            $scope.paging();
        });
    }
    var type = $stateParams.type;

    if (type == null || type == undefined) {
        getAllCarTypes();
        getMessages();
        getAllServiceProviders();
        getServiceCategory();
    }
    var aa = $stateParams.msg;
    if ($stateParams.msg != undefined && $stateParams.msg != "") {

        $scope.alerts = [];
        $scope.alerts.push({ type: "success", msg: $stateParams.msg });
    }


    $scope.checked = false;
    $scope.showRegisterPage = false;
    $scope.showPaging = false;
    $scope.SelectedServices = [];
    $scope.SelectedCityId;
    $scope.SelectedDistrictId;
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
        //ServiceProviderType: type
    };

    $scope.serviceModel = {
        ServiceProviderId: "",
        ServiceId: "",
        Price: "",
        VisitRate: "",
        HourlyRate: "",
        RateType: "",
        Duration: ""
    }
    var oriServiceModel = angular.copy($scope.serviceModel);
    $scope.files = [];


    function getAllCarTypes() {
        $rootScope.showLoader = true;
        employeeService.getAllCarTypes().then(function (response) {

            $scope.AllCarTypes = [];

            angular.forEach(response, function (value, key) {
                $scope.AllCarTypes.push(value);

            })
            startTimer();
        });
    }

    $scope.cancelSignUp = function () {
        $location.path('/login');
    };

    $scope.CompanyLogo = function (files) {

        var a = document.getElementById('CompanyLogo');
        if (a.value == "") {
            CompanyLogoPathfileLabel.innerHTML = "No files selected";
        }
        else {
            var theSplit = a.value.split('\\');
            CompanyLogoPathfileLabel.innerHTML = theSplit[theSplit.length - 1];
        }
        $scope.CompanyLogoFile = files;
    };

    $scope.RegistrationCertificate = function (files) {

        var a = document.getElementById('CertificateRegistrationPath');
        if (a.value == "") {
            CertificateRegistrationPathfileLabel.innerHTML = "No files selected";
        }
        else {
            var theSplit = a.value.split('\\');
            CertificateRegistrationPathfileLabel.innerHTML = theSplit[theSplit.length - 1];
        }
        $scope.RegistrationCertificateFile = files;
    };

    $scope.GosiCertificate = function (files) {
        var a = document.getElementById('GOSICertificatePath');
        if (a.value == "") {
            GOSICertificatePathfileLabel.innerHTML = "No files selected";
        }
        else {
            var theSplit = a.value.split('\\');
            GOSICertificatePathfileLabel.innerHTML = theSplit[theSplit.length - 1];
        }
        $scope.GosiCertificateFile = files;
    };

    $scope.DeActivateServiceProvider = function (serviceProviderId, IsActive) {

        serviceProviderService.deactivateServiceProvider(serviceProviderId, IsActive).then(function (response) {

            EditServiceProvider(serviceProviderId);

        });
    }

    function getAllSubCategory() {

        subCategory.getAllSubServices().then(function (response) {

            $scope.AllSubCategory = [];
            $scope.AllSubService = [];
            angular.forEach(response, function (value, key) {

                if (value.ParentServiceId == 0) {
                    $scope.AllSubCategory.push(value);


                }
                else if (value.ParentServiceId != 0) {
                    $scope.AllSubService.push(value);
                }
            })


        });
    }

    function getAllDistrict() {

        districtService.GetAllDistrict().then(function (response) {

            $scope.AllDistrict = [];

            angular.forEach(response, function (value, key) {
                $scope.AllDistrict.push(value);

            })
        });
    }

    function EditServiceProvider(id) {

        serviceProviderService.getServiceProviderById(id).then(function (response) {

            if (response != undefined && response != null)
                $scope.ServiceProviderId = response.ServiceProviderId;
            $scope.CompanyName = response.CompanyName;
            $scope.Email = response.Email;
            $scope.GeneralManagerName = response.GeneralManagerName;
            $scope.CompanyRegistrationNumber = response.CompanyRegistrationNumber;
            $scope.PhoneNumber = response.PhoneNumber;
            $scope.FaxNumber = response.FaxNumber;
            $scope.MinimumServicePrice = response.MinimumServicePrice;
            $scope.Location = response.Location;
            $scope.IsActive = response.IsActive;
            $scope.PlanName = response.PlanModel.Description;

            $scope.NumberOfTeams = response.UserPlan.NumberOfTeams;
            $scope.CreatedDate = response.UserPlan.CreatedDate;
            $scope.ExpiredDate = response.UserPlan.ExpiredDate;
            $scope.ServiceProviderUserName = response.UserName;

            $scope.ServiceTypeText = response.Type;
            //if (response.UserPlan.PlanId == 0 && $scope.app.currentUser.userRole == "ServiceProvider") {
            //    $scope.alerts = [];
            //    $scope.alerts.push({ type: 'success', msg: "Please purchase a plan to continue with URFX" });
            //}
            //else {
            //    $scope.app.currentUser.planId = response.UserPlan.PlanId;
            //}




            if (response.PlanModel.TeamRegistrationType == 0) {

                $scope.TeamRegistrationType = "Monthly";
            }

            $scope.CompanyLogoPath = response.CompanyLogoPath;
            $scope.GosiCertificatePath = response.GosiCertificatePath;
            $scope.RegistrationCertificatePath = response.RegistrationCertificatePath;
            $scope.Description = response.Description;
            $scope.CityId = response.UserLocationModel.CityId;
            $scope.DistrictId = response.UserLocationModel.DistrictId;
            $scope.ServiceProviderType = response.ServiceProviderType;
            $scope.IsActiveStatus = response.IsActiveStatus;
            $scope.ServiceTypeText = response.Type;
            if ($scope.ServiceProviderType == 1) {
                $scope.LicensePlateNumber = response.EmployeesList[0].LicensePlateNumber;
                $scope.NationalIdAndIqamaNumber = response.EmployeesList[0].NationalIdAndIqamaNumber;
                //$scope.IqamaNumber = response.EmployeesList[0].IqamaNumber;

                $scope.ProfileImage = response.EmployeesList[0].ProfileImage;
                $scope.CarTypeId = response.CarTypeId;
                $scope.ServiceTypeText = response.Type;
                $scope.IsActiveStatus = response.IsActiveStatus;
                $scope.ServiceProviderType = response.ServiceProviderType;
                angular.forEach($scope.AllCarTypes, function (value, key) {

                    if ($scope.CarTypeId == value.CarTypeId) {

                        $scope.SelectedCarType = [];
                        $scope.SelectedCarType = value;
                        $scope.SelectedCarTypeId = value.CarTypeId;
                    }
                })

                angular.forEach($scope.itemCity, function (value, key) {

                    if ($scope.CityId == value.CityId) {

                        $scope.SelectedCity = [];
                        $scope.SelectedCity = value;
                        $scope.SelectedCityId = value.CityId;
                    }
                })
            }

            $scope.ServiceProviderRating = response.RatingModel;

            $scope.EmployeeList = response.EmployeesList;

            angular.forEach(response.ServicesList, function (value, key) {

                $scope.CategoryId = value.CategoryId;
                $scope.ServiceId = value.ServiceId;
                $scope.Price = value.Price;
                $scope.VisitRate = value.VisitRate;
                $scope.HourlyRate = value.HourlyRate;

                angular.forEach($scope.itemServiceCategory, function (Categoryvalue, Categorykey) {

                    if ($scope.CategoryId == Categoryvalue.ServiceCategoryId) {
                        var inputElements = document.getElementsByClassName('chkbox');
                        for (var i = 0; inputElements[i]; ++i) {
                            if (inputElements[i].value == "" + $scope.CategoryId + "") {
                                inputElements[i].checked = true;

                            }
                        }

                    }
                })
                angular.forEach($scope.AllSubCategory, function (subCategoryvalue, subCategorykey) {

                    if ($scope.ServiceId == subCategoryvalue.ServiceId) {
                        debugger;
                        $scope.SelectedServiceDetails.push(subCategoryvalue);
                        $scope.SelectedServiceDetails[key].Price = $scope.Price;
                        if ($scope.VisitRate != 0) {
                            $scope.SelectedServiceDetails[key].VisitRate = $scope.VisitRate;
                            $scope.SelectedServiceDetails[key].HourlyRate = $scope.HourlyRate;
                        }
                        else {
                            $scope.SelectedServiceDetails[key].HourlyRate = $scope.HourlyRate;
                            $scope.SelectedServiceDetails[key].VisitRate = $scope.VisitRate;
                        }
                        var inputElements = document.getElementsByClassName('subchkbox');
                        for (var i = 0; inputElements[i]; ++i) {
                            if (inputElements[i].value == "" + $scope.ServiceId + "") {

                                inputElements[i].checked = true;
                            }
                        }
                    }
                    //}
                });

                angular.forEach($scope.AllSubService, function (subCategoryvalue, subCategorykey) {
                  
                    if ($scope.ServiceId == subCategoryvalue.ServiceId) {
                        debugger;
                        $scope.SelectedServiceDetails.push(subCategoryvalue);
                        $scope.SelectedServiceDetails[key].Price = $scope.Price;
                        if ($scope.VisitRate != 0) {
                            $scope.SelectedServiceDetails[key].VisitRate = $scope.VisitRate;
                        }
                        else {
                            $scope.SelectedServiceDetails[key].HourlyRate = $scope.HourlyRate;
                        }
                        var inputElements = document.getElementsByClassName('subServicechkbox');
                        for (var i = 0; inputElements[i]; ++i) {
                            if (inputElements[i].value == "" + $scope.ServiceId + "") {

                                inputElements[i].checked = true;



                            }
                        }
                    }
                });

            })
            angular.forEach($scope.itemCity, function (value, key) {

                if ($scope.CityId == value.CityId) {

                    $scope.SelectedCity = [];
                    $scope.SelectedCity = value;
                    $scope.SelectedCityId = value.CityId;
                }
            })

            angular.forEach($scope.AllDistrict, function (value, key) {

                if ($scope.DistrictId == value.DistrictId) {

                    $scope.SelectedDistrict = [];
                    $scope.SelectedDistrict = value;
                    $scope.SelectedDistrictId = value.DistrictId;
                }
            });
            $scope.IsServiceProviderLoaded = true;


        });


    }




    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete Service Provider',
        headerText: 'Delete?',
        bodyText: 'Are you sure you want to delete this service provider?'
    };

    $scope.deleteServiceProvider = function (obj) {

        modalOptions.headerText = "Delete '" + obj.CompanyName + "'?";
        modalOptions.bodyText = "Are you sure you want to delete this Service Provider?";
        modalOptions.actionButtonText = "Delete";
        modalService.showModal({}, modalOptions).then(function (result) {
            serviceProviderService.deleteServiceProvider(obj.ServiceProviderId).then(function (response) {
                $scope.getAllServiceProviders();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: "Service provider has been deleted successfully." });

            }, function (response) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'warning', msg: response.Message });


            });

        });
    }

    $scope.signUp = function () {

        // $scope.registration.Services = $scope.SelectedServices;
        $scope.planDetailModel = {
            NumberOfTeams: $scope.NumberOfTeams,
            PlanId: $scope.selectedPlan
        };
        $scope.locationDetailsModel = {
            CityId: $scope.SelectedCityId,
            DistrictId: $scope.SelectedDistrictId,
            Latitude: $scope.Latitude,
            Longitude: $scope.Longitude
        };
        $scope.isSubmit = true;
        $scope.dataModel = {
            model: $scope.registration,
            services: $scope.SelectedServices,
            companyLogoFile: $scope.CompanyLogoFile,
            registrationCertificateFile: $scope.RegistrationCertificateFile,
            gosiCertificateFile: $scope.GosiCertificateFile,
            planModel: $scope.planDetailModel,
            locationModel: $scope.locationDetailsModel,
        }

        authService.saveRegistration($scope.dataModel).then(function (response) {
            if ($scope.app.currentUser.userId != "" && $scope.app.currentUser.userId != undefined) {
                $stateParams.msg = "";
                $state.go('app.administration.serviceprovider', { msg: "Service provider as corporate added successfully." })
            }
            else {
                window.location.href = '/App/#/sentemailsuccessfull';
                //$state.go('sentemailsuccessfull');
            }


        },
         function (response) {
             if (IsJsonString(response.data.Message)) {
                 var response = JSON.parse(response.data.Message);
                 if (response.status != undefined) {
                     $scope.alerts = [];
                     $scope.alerts.push({ type: 'warning', msg: response.message });
                 }
             }
             else {
                 $scope.alerts = [];
                 $scope.alerts.push({ type: 'warning', msg: "Service provider as corporate not added successfully." });
             }
             //$scope.showRegisterPage = false;
             //var errors = [];
             //for (var key in response.data.ModelState) {
             //    for (var i = 0; i < response.data.ModelState[key].length; i++) {
             //        errors.push(response.data.ModelState[key][i]);
             //    }
             //}

             //if (errors == [] || errors == null) {
             //    $scope.alerts = [];
             //    $scope.alerts.push({ type: 'warning', msg: "Service provider as corporate not added successfully." });
             //}
             //$scope.alerts = [];
             //$scope.alerts.push({ type: 'warning', msg: "Service provider as corporate not added successfully." });

         });
    };
    function IsJsonString(str) {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    }
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

    $scope.GetSubCategoryByCategoryId = function (categoryId) {

        $scope.SelectedCategoryId = categoryId;
        $scope.itemSubService = [];
        $scope.selectedServiceName = [];
        $scope.serviceModel = {}
        subCategory.getSubCategoryByCategoryId(categoryId).then(function (response) {
            //  $state.go('dashboard');

            $scope.itemSubCategory = [];
            angular.forEach(response, function (value, key) {

                this.push(value);
            }, $scope.itemSubCategory)



        }, function (response) {
            $scope.errorMessage = response.error_description;
        });
    }

    $scope.GetSubServiceByServiceId = function (serviceId) {

        $scope.SelectedServiceId = serviceId;
        $scope.selectedServiceName = [];
        $scope.serviceModel = {}
        subService.getSubServiceByServiceId(serviceId).then(function (response) {

            if (response != "") {
                //  $state.go('dashboard');

                $scope.itemSubService = [];
                angular.forEach(response, function (value, key) {

                    this.push(value);
                }, $scope.itemSubService)
            }
            else {
                $scope.GetSeletedService(serviceId);
            }


        }, function (response) {
            $scope.errorMessage = response.error_description;
        });
    }

    $scope.GetSeletedService = function (serviceId) {

        $scope.SelectedServiceId = serviceId;
        $scope.serviceModel = {}
        angular.forEach($scope.itemSubService, function (subServicevalue, subServicekey) {
            if (serviceId == subServicevalue.ServiceId) {

                $scope.selectedServiceName = [];
                $scope.selectedServiceName.push(subServicevalue);
            }
        })
        angular.forEach($scope.itemSubCategory, function (subCategoryvalue, subCategorykey) {
            if (serviceId == subCategoryvalue.ServiceId) {

                $scope.selectedServiceName = [];
                $scope.selectedServiceName.push(subCategoryvalue);
            }
        })
    }



    $scope.addServices = function () {
        if ($scope.SelectedServiceId != "" && $scope.SelectedServiceId != null) {
            $scope.serviceModel.HourlyRate = 0;
            $scope.serviceModel.VisitRate = 0;
            $scope.serviceModel.ServiceId = $scope.SelectedServiceId;
            $scope.serviceModel.ServiceProviderId = serviceProviderId;
            if ($scope.serviceModel.RateType == "0") {

                $scope.serviceModel.VisitRate = $scope.serviceModel.Duration;
            }
            else {
                $scope.serviceModel.HourlyRate = $scope.serviceModel.Duration;
            }
            serviceProviderService.addServices($scope.serviceModel).then(function (response) {
                //$scope.serviceModel = {
                //    ServiceId: "",
                //    Price: "",
                //    VisitRate: "",
                //    HourlyRate: "",
                //    RateType: "",
                //    Duration: ""
                //}
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: "Service has been added successfully." });
                //angular.forEach($scope.SelectedServiceDetails, function (value, key) {
                angular.forEach($scope.AllSubCategory, function (subCategoryvalue, subCategorykey) {
                   
                    if ($scope.SelectedServiceId == subCategoryvalue.ServiceId) {
                        
                        $scope.SelectedServiceDetails.push(subCategoryvalue);
                        var length = $scope.SelectedServiceDetails.length;
                        $scope.SelectedServiceDetails[length - 1].Price = $scope.serviceModel.Price;
                        if ($scope.serviceModel.VisitRate != 0) {
                            $scope.SelectedServiceDetails[length - 1].VisitRate = $scope.serviceModel.VisitRate;
                            $scope.SelectedServiceDetails[length - 1].HourlyRate = $scope.serviceModel.HourlyRate;
                        }
                        else {
                            $scope.SelectedServiceDetails[length - 1].HourlyRate = $scope.serviceModel.HourlyRate;
                            $scope.SelectedServiceDetails[length - 1].VisitRate = $scope.serviceModel.VisitRate;
                        }


                    }

                });
                angular.forEach($scope.AllSubService, function (subCategoryvalue, subCategorykey) {
                   
                    if ($scope.SelectedServiceId == subCategoryvalue.ServiceId) {
                       
                        $scope.SelectedServiceDetails.push(subCategoryvalue);
                        var length = $scope.SelectedServiceDetails.length;
                        $scope.SelectedServiceDetails[length - 1].Price = $scope.serviceModel.Price;
                        if ($scope.serviceModel.VisitRate != 0) {
                            $scope.SelectedServiceDetails[length - 1].VisitRate = $scope.serviceModel.VisitRate;
                        }
                        else {
                            $scope.SelectedServiceDetails[length - 1].HourlyRate = $scope.serviceModel.HourlyRate;
                        }


                    }

                });
                setTimeout(function () {
                 $scope.alerts = [];
                //    //$scope.serviceForm.$setPristine();
                //    ////$scope.alerts = [];
                //    //getServiceCategory();
                //    //$scope.itemSubCategory = [];
                //    //if ($scope.itemSubService != null) {
                //    //    $scope.itemSubService = [];
                //    //}
                   $scope.SelectedServiceId = "";
                //    //$scope.selectedServiceName = null;
                }, 2000);
                //angular.element('#resetServices').triggerHandler('click');
                //  });
            },

              function (response) {
                  $scope.alerts = [];
                  $scope.alerts.push({ type: 'warning', msg: response.Message });
                  $scope.addMoreService();
              });
           
        }
        else {
            //$scope.serviceModel.ServiceId = "";
            //$scope.serviceModel.Price = "";
            //$scope.serviceModel.VisitRate = "";
            //$scope.serviceModel.HourlyRate = "";
            //$scope.serviceModel.RateType = "";
            //$scope.serviceModel.Duration = "";
            //$scope.SelectedCategory = "";
            //$scope.SelectedService = "";
            //$scope.SelectedSubService = "";
            $scope.alerts = [];
           
           
        }
    }

    $scope.addMoreService = function () {
        $scope.serviceForm.$setPristine();
        //$scope.alerts = [];
        getServiceCategory();
        $scope.itemSubCategory = [];
        if ($scope.itemSubService != null) {
            $scope.itemSubService = [];
        }

        $scope.selectedServiceName = null;
        $scope.serviceModel.ServiceId = "";
        $scope.serviceModel.Price = "";
        $scope.serviceModel.VisitRate = "";
        $scope.serviceModel.HourlyRate = "";
        $scope.serviceModel.RateType = "";
        $scope.serviceModel.Duration = "";




    }

    $scope.deleteServices = function (obj) {
        modalOptions.headerText = "Delete Service?";
        modalService.showModal({}, modalOptions).then(function (result) {

            serviceProviderService.deleteServices(obj.ServiceId).then(function (response) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: "Service has been deleted successfully." });
                angular.forEach($scope.SelectedServiceDetails, function (value, key) {
                    if (value.ServiceId == obj.ServiceId) {

                        $scope.SelectedServiceDetails.splice(key, 1);
                    }

                });
                $anchorScroll();

            });
        });

    }

    function getCity() {

        cityService.getCity().then(function (response) {
            //  $state.go('dashboard');

            $scope.itemCity = [];
            angular.forEach(response, function (value, key) {

                this.push(value);
            }, $scope.itemCity)
            if (serviceProviderId != null) {


                getAllSubCategory();
                getAllDistrict();
                getMessages();
                EditServiceProvider(serviceProviderId);

            }
        }, function (response) {
            $scope.errorMessage = response.error_description;
        });
    };

    var getMessages = function () {

        var successMsg = localStorageService.get('SuccessMsg');
        if (successMsg != null && successMsg != "" && successMsg != undefined) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: successMsg });
            localStorageService.remove('SuccessMsg');
        }

    }
    //$scope.GetDistrictByCityId = function (cityId) {

    //    $scope.SelectedCityId = cityId.CityId;
    //    cityName = cityId.Description;
    //    districtService.GetDistrictByCityId(cityId.CityId).then(function (response) {
    //        //  $state.go('dashboard');

    //        $scope.itemDistrict = [];
    //        angular.forEach(response, function (value, key) {

    //            this.push(value);
    //        }, $scope.itemDistrict)



    //    }, function (response) {
    //        $scope.errorMessage = response.error_description;
    //    });
    //}

    $scope.GetSelectedCity = function (cityId) {

        //$scope.SelectedDistrictId = districtId.DistrictId
        // var districtName = districtId.Description;
        $scope.SelectedCityId = cityId.CityId;
        cityName = cityId.Description;
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': cityName }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                // alert("location : " + results[0].geometry.location.lat() + " " + results[0].geometry.location.lng());
                $scope.Latitude = results[0].geometry.location.lat();
                $scope.Longitude = results[0].geometry.location.lng();
            } else {
                //  alert("Something got wrong " + status);
            }
        });
    }






    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };


    $scope.UpdateServiceProvider = function () {

        $scope.registration = {
            ServiceProviderId: $scope.ServiceProviderId,
            CompanyName: $scope.CompanyName,
            Email: $scope.Email,
            GeneralManagerName: $scope.GeneralManagerName,
            CompanyRegistrationNumber: $scope.CompanyRegistrationNumber,
            PhoneNumber: $scope.PhoneNumber,
            FaxNumber: $scope.FaxNumber,
            MinimumServicePrice: $scope.MinimumServicePrice,
            Description: $scope.Description,
            //ServiceProviderType:$scope.ServiceProviderType,
            IsActive: $scope.IsActive
        };
        $scope.locationDetailsModel = {
            CityId: $scope.SelectedCityId,
            DistrictId: $scope.SelectedDistrictId,
            Latitude: $scope.Latitude,
            Longitude: $scope.Longitude
        };
        $scope.isSubmit = true;
        $scope.dataModel = {
            model: $scope.registration,
            services: $scope.SelectedServices,
            companyLogoFile: $scope.CompanyLogoFile,
            registrationCertificateFile: $scope.RegistrationCertificateFile,
            gosiCertificateFile: $scope.GosiCertificateFile,
            locationModel: $scope.locationDetailsModel,
        }

        serviceProviderService.updateServiceProvider($scope.dataModel).then(function (response) {
            $anchorScroll();
            $scope.isSubmit = false;



            if ($scope.app.currentUser.userRole == "Admin") {
                //$scope.alerts = [];
                //$scope.alerts.push({ type: 'success', msg: "Service provider has been updated successfully." });
                $state.go('app.administration.serviceprovider', { serviceProviderId: serviceProviderId }, { msg: "Service provider has been updated successfully." });
                localStorageService.set('SuccessMsg', "");
                localStorageService.set('SuccessMsg', "Service provider has been updated successfully.");
            }
            else {
                //$scope.alerts = [];
                //$scope.alerts.push({ type: 'success', msg: "Service provider has been updated successfully." });
                $state.go('app.serviceprovider.profile', { serviceProviderId: $scope.app.currentUser.userId })
                localStorageService.set('SuccessMsg', "");
                localStorageService.set('SuccessMsg', "Service provider has been updated successfully.");
            }

        },
         function (response) {
             $anchorScroll();
             $scope.isSubmit = false;

             $scope.showPaymentPage = false;
             $scope.showRegisterPage = true;
             var errors = [];

             for (var key in response.data.ModelState) {

                 for (var i = 0; i < response.data.ModelState[key].length; i++) {
                     errors.push(response.data.ModelState[key][i]);
                 }
             }

             if (errors == null) {
                 $scope.alerts = [];
                 $scope.alerts.push({ type: 'warning', msg: "Service provider not updated successfully." });
             }
             else {
                 $scope.alerts = [];
                 $scope.alerts.push({ type: 'warning', msg: "Service provider not updated successfully." });
             }

         });
    }

    //pagging work
    $scope.itemsPerPage = pagingservice.pagesize;
    $scope.currentPage = 1;
    $scope.items = [];
    $scope.range = function () {
        $scope.totalpage = Math.ceil($scope.totalcount / $scope.itemsPerPage);
        $scope.showPaging = '';
        if ($scope.itemsPerPage == "") {
            $scope.itemsPerPage = pagingservice.pagesize;
        }
        if (Math.ceil($scope.totalcount / $scope.itemsPerPage) >= 2) {
            $scope.showPaging = true;
        }
        else {
            $scope.showPaging = false;
        }
        if (Math.ceil($scope.totalcount / $scope.itemsPerPage) <= 5) {
            var rangeSize = Math.ceil($scope.totalcount / $scope.itemsPerPage);
        }
        else {
            var rangeSize = 5;
        }
        var ret = [];

        var start;
        start = $scope.currentPage;
        if (start > $scope.pageCount() - rangeSize) {
            start = $scope.pageCount() - rangeSize + 1;
        }

        for (var i = start; i < start + rangeSize; i++) {
            ret.push(i);
        }

        return ret;
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 1) {
            $scope.currentPage--;
            $scope.paging();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 1 ? "disabled" : "";
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.totalcount / $scope.itemsPerPage);
    };

    $scope.nextPage = function () {

        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage++;
            $scope.paging();
        }
    };

    $scope.nextPageDisabled = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };

    $scope.setPage = function (n) {

        $scope.currentPage = n;
        $scope.paging();
    };

    $scope.firstPageDisabled = function () {
        return $scope.currentPage === 1 ? "disabled" : "";
    };
    $scope.lastPageDisabled = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };
    $scope.firstPage = function () {
        if ($scope.currentPage > 1) {
            $scope.currentPage = 1;
            $scope.paging();
        }
    }
    $scope.lastPage = function () {
        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage = $scope.pageCount();
            $scope.paging();
        }
    }

    $scope.searchRecord = function () {

        $scope.currentPage = 1;
        $scope.paging();
    }
    $scope.clear = function () {

        $scope.SearchText = "";
        $scope.paging();
    }
    $scope.paging = function () {

        var pagingModel = {
            CurrentPageIndex: $scope.currentPage,
            Pagesize: $scope.itemsPerPage,
            SearchText: $scope.SearchText
        }

        serviceProviderService.paging(pagingModel).then(function (response) {

            if (response.data != null) {
                $scope.unSelect();
                $scope.SelectedServiceProvider = response.data;
                $scope.totalcount = response.totalRecords;
                if ($scope.currentPage == 1) {
                    $scope.itemCount = $scope.SelectedServiceProvider.length;
                    $scope.startRecord = 1;

                }
                else {

                    if ($scope.SelectedServiceProvider.length < $scope.itemsPerPage) {
                        $scope.itemCount = ($scope.itemsPerPage * $scope.currentPage) - $scope.itemsPerPage + $scope.SelectedServiceProvider.length;
                        $scope.startRecord = ($scope.itemsPerPage * ($scope.currentPage - 1)) + 1;
                    }
                    else {
                        $scope.itemCount = ($scope.itemsPerPage * ($scope.currentPage));//$scope.itemCount + data.data.length;
                        $scope.startRecord = ($scope.itemsPerPage * ($scope.currentPage - 1)) + 1; //data.data.length + $scope.startRecord +1;
                    }
                }

                $scope.totalpage = Math.ceil($scope.totalcount / $scope.itemsPerPage);
                if (Math.ceil($scope.totalcount / $scope.itemsPerPage) >= 2) {
                    $scope.showPaging = true;
                }
                else {
                    $scope.showPaging = false;
                }
                startTimer();
            }
        },
             function (error) {
                 $scope.errormessage = "Error happend ";
                 startTimer();
             }
             );

    }
    //End work

    //function for checked and refresh
    //function for checked and refresh
    $(function () {

        $('#chkAll').click(function (event) {

            if (this.checked) {
                $('.SingleChk').each(function () {
                    this.checked = true;
                });

            } else {
                $('.SingleChk').each(function () {
                    this.checked = false;
                });
            }
            $scope.$apply(function () {
                $scope.refreshIsMultipleDelete();
            });

        });

    });
    //end function

    //function for selectaunselect checkbox
    $scope.selectUnselectAll = function () {

        var aa = $(".SingleChk").length;
        if ($(".SingleChk").length == $('.SingleChk:checked').length) {

            $('#chkAll').prop("checked", true);
        }
        else {
            $('input:checkbox[name="chkAll"]').removeAttr("checked");
        }

        $scope.refreshIsMultipleDelete();
    }
    //end function

    $scope.fillselectedServiceProviders = function () {

        $scope.selectedServiceProviders = [];
        $('input[name=SingleChk]').each(function () {
            if (this.checked) {
                $scope.selectedServiceProviders.push($(this).val());
            }
        });

    }

    $scope.refreshIsMultipleDelete = function () {

        $scope.fillselectedServiceProviders();
        if ($scope.selectedServiceProviders.length > 1) {
            $scope.IsMultipleDelete = true;
        }
        else {
            $scope.IsMultipleDelete = false;
        }
    }

    $scope.unSelect = function () {
        $('input:checkbox[name="chkAll"]').removeAttr("checked");
    }


    //function for delete multiple
    //$scope.deleteSelectedServiceProviders = function () {

    //    $scope.IsMultipleDelete = false;
    //    $scope.fillselectedServiceProviders();
    //    //   $('#deleteAllConfirmModal').modal('hide');
    //    var serviceProviders = $scope.selectedServiceProviders;
    //    serviceProviderService.deleteSelectedServiceProviders(serviceProviders).then(function (data) {

    //        if (data != null) {

    //            if ((data.totalRecords - (($scope.currentPage - 1) * $scope.itemsPerPage)) < 1) {
    //                $scope.currentPage = $scope.currentPage - 1;
    //            }
    //            $scope.getAllServiceProviders();
    //            $scope.alerts = [];
    //            $scope.alerts.push({ type: 'success', msg: data.Message });
    //        }
    //    }, function (error) {

    //    });
    //}
    //$scope.dismissModal = function () {
    //    dialog.close('dismiss');
    //}

    //end function


    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete Category service?',
        bodyText: 'Are you sure you want to delete selected service?'
    };


    $scope.deleteSelectedServiceProviders = function () {
        modalOptions.headerText = "Delete Selected Service Providers?";
        modalService.showModal({}, modalOptions).then(function (result) {
            $scope.IsMultipleDelete = false;
            $scope.fillselectedServiceProviders();
            //   $('#deleteAllConfirmModal').modal('hide');
            var serviceProviders = $scope.selectedServiceProviders;
            serviceProviderService.deleteSelectedServiceProviders(serviceProviders).then(function (data) {

                if (data != null) {

                    if ((data.totalRecords - (($scope.currentPage - 1) * $scope.itemsPerPage)) < 1) {
                        $scope.currentPage = $scope.currentPage - 1;
                    }
                    $scope.getAllServiceProviders();
                    $scope.alerts = [];
                    $scope.alerts.push({ type: 'success', msg: data.Message });
                }
            }, function (error) {

            });
        });
    }


    $scope.deleteSelectedServices = function () {

        $scope.IsMultipleDelete = false;
        $scope.fillselectedServices();
        //   $('#deleteAllConfirmModal').modal('hide');
        var services = $scope.fillselectedServices;
        serviceProviderService.deleteSelectedServices(services).then(function (data) {

            if (data != null) {

                //if ((data.totalRecords - (($scope.currentPage - 1) * $scope.itemsPerPage)) < 1) {
                //    $scope.currentPage = $scope.currentPage - 1;
                //}
                //$scope.getAllServiceProviders();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: data.Message });
            }
        }, function (error) {

        });
    }

    $scope.fillselectedServices = function () {

        $scope.fillselectedServices = [];
        $('input[name=SingleChk]').each(function () {
            if (this.checked) {
                $scope.fillselectedServices.push($(this).val());
            }
        });

    }



    $scope.deleteEmployee = function (employee) {
        modalOptions.headerText = "Delete '" + employee.FirstName + " " + employee.LastName + "'?";
        modalOptions.bodyText = "Are you sure you want to delete this employee?";
        modalOptions.actionButtonText = "Delete Employee";
        modalService.showModal({}, modalOptions).then(function (result) {
            employeeService.deleteEmployee(employee.EmployeeId).then(function (response) {
                //getAllEmployee();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: "Employee has been deleted successfully." });
                var index = $scope.EmployeeList.map(function (elem) { return elem.EmployeeId }).indexOf(employee.EmployeeId);
                if (index >= 0) {
                    $scope.EmployeeList.splice(index, 1);
                }

            }, function (response) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'warning', msg: response.Message });


            });
            $anchorScroll();
        });
    };

    $scope.deleteselectedEmployees = function () {
        modalOptions.headerText = "Delete Employees?";
        modalOptions.bodyText = "Are you sure you want to delete selected employees?";
        modalOptions.actionButtonText = "Delete Employee";
        modalService.showModal({}, modalOptions).then(function (result) {
            employeeService.deleteselectedEmployees($scope.selectedServiceProviders).then(function (response) {
                //getAllEmployee();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: "Employee has been deleted successfully." });
                angular.forEach($scope.selectedServiceProviders, function (key, value) {
                    var index = $scope.EmployeeList.map(function (elem) { return elem.EmployeeId }).indexOf(key);
                    if (index >= 0) {
                        $scope.EmployeeList.splice(index, 1);
                    }
                })


            }, function (response) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'warning', msg: response.Message });


            });
            $anchorScroll();
        });
    }
    var startTimer = function () {

        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $rootScope.showLoader = false;
        }, 2000);
    }
}]);
