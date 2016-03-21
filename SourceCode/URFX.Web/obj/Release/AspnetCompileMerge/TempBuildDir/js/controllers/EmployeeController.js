app.controller('EmployeeController', ['$scope', '$rootScope', '$http', '$stateParams', '$state', 'employeeService', 'cityService', 'districtService', 'authService', 'pagingservice', 'modalService', '$timeout', 'localStorageService',
function ($scope, $rootScope, $http, $stateParams, $state, employeeService, cityService, districtService, authService, pagingservice, modalService, $timeout, localStorageService) {
    $scope.SelectedCarTypeId;
    $scope.IsMultipleDelete = false;
   
    $scope.ServiceProviderType = -1;
    ////alert($stateParams.serviceProviderId);
    var employeeId = $stateParams.employeeId;
    var serviceProviderId = $stateParams.serviceProviderId;
    var type = $stateParams.type;
    $scope.serviceProviderId = serviceProviderId;
    var getMessages = function () {
        var successMsg = localStorageService.get('SuccessMsg');
        if (successMsg != null && successMsg != "" && successMsg != undefined) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: successMsg });
            localStorageService.remove('SuccessMsg');
        }
    }
    getMessages();
    if (employeeId != null) {

        getAllCarTypes();
        getAllDistrict();
        EditEmployee(employeeId);

    }
    else {
        if (type == null || type == undefined) {
            getMessages();
            getAllEmployee();
        }
    }

    //$scope.checked = false;
   
    var aa = $stateParams.msg;
  
    if ($stateParams.msg != undefined && $stateParams.msg != "") {
        
        $scope.alerts = [];
        $scope.alerts.push({ type: "success", msg: $stateParams.msg });
    }
    $scope.showPaging = false;


    $scope.SelectedCityId;
    $scope.SelectedDistrictId;
    var cityName;
    $scope.empRegistration = {
        ServiceProviderId: $scope.serviceProviderId,
        Email: "",
        FirstName: "",
        LastName: "",
        PhoneNumber: "",
        Password: "",
        ConfirmPassword: "",
        ProfileImage: "",
        NationalIdAndIqamaNumber: "",
        IqamaNumber: "",
        LicensePlateNumber: "",


    };

    function getAllCarTypes() {
        $rootScope.showLoader = true;
        employeeService.getAllCarTypes().then(function (response) {

            $scope.AllCarTypes = [];

            angular.forEach(response, function (value, key) {
                $scope.AllCarTypes.push(value);

            })
            $rootScope.showLoader = false;
        });
    }

    function getAllDistrict() {
        $rootScope.showLoader = true;
        districtService.GetAllDistrict().then(function (response) {

            $scope.AllDistrict = [];

            angular.forEach(response, function (value, key) {
                $scope.AllDistrict.push(value);

            })
            $rootScope.showLoader = false;
        });
    }
    // $scope.SelectedCityId = $scope.empRegistration.City.CityId

    $scope.files = [];

    function EditEmployee(id) {

        employeeService.getEmployeeById(id).then(function (response) {

            if (response.model.CompanyName != null) {
                $scope.ServiceProviderType = response.model.ServiceProviderType;
            }
            if (response != undefined && response != null)
                $scope.EmployeeId = response.EmployeeId;
            $scope.FirstName = response.FirstName;
            $scope.Email = response.Email;
            $scope.LastName = response.LastName;
            $scope.PhoneNumber = response.PhoneNumber;
            $scope.Description = response.Description;
            $scope.DistrictId = response.UserLocationModel.DistrictId;
            $scope.CityId = response.UserLocationModel.CityId;
            $scope.NationalIdAndIqamaNumber = response.NationalIdAndIqamaNumber;
            $scope.IqamaNumber = response.IqamaNumber;
            $scope.ProfileImage = response.ProfileImage;
            $scope.ConfirmPassword = response.ConfirmPassword;
            $scope.Password = response.Password;
            $scope.LicensePlateNumber = response.LicensePlateNumber;
            $scope.CarTypeId = response.CarTypeId;

            angular.forEach($scope.itemCity, function (value, key) {

                if ($scope.CityId == value.CityId) {

                    $scope.SelectedCity = [];
                    $scope.SelectedCity = value;
                    $scope.SelectedCityId = value.CityId;
                }
            })

            //angular.forEach($scope.AllDistrict, function (value, key) {

            //    if ($scope.DistrictId == value.DistrictId) {

            //        $scope.SelectedDistrict = [];
            //        $scope.SelectedDistrict = value;
            //        $scope.SelectedDistrictId = value.DistrictId;
            //    }
            //})
            angular.forEach($scope.AllCarTypes, function (value, key) {

                if ($scope.CarTypeId == value.CarTypeId) {

                    $scope.SelectedCarType = [];
                    $scope.SelectedCarType = value;
                    $scope.SelectedCarTypeId = value.CarTypeId;
                }
            })

        });


    }

    $scope.UpdateEmployee = function () {

        $scope.registration = {
            EmployeeId: $scope.EmployeeId,
            FirstName: $scope.FirstName,
            Email: $scope.Email,
            LastName: $scope.LastName,
            Description: $scope.Description,
            PhoneNumber: $scope.PhoneNumber,
            LicensePlateNumber: $scope.LicensePlateNumber,
            CarTypeId: $scope.SelectedCarTypeId

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
            //  services: $scope.SelectedServices,
            ProfileImageFile: $scope.ProfileImageFile,
            NationalIdFile: $scope.NationalIdFile,
            IqamaNumberFile: $scope.IqamaNumberFile,
            locationModel: $scope.locationDetailsModel,
        }

        employeeService.updateEmployee($scope.dataModel).then(function (response) {


            // $scope.PaymntView();
            //startTimer();
            //$scope.alerts = [];
            //$scope.alerts.push({ type: 'success', msg: " Employee has been updated successfully." });

            if ($scope.app.currentUser.userRole == "ServiceProvider" && $scope.ServiceProviderType == -1) {

                $state.go('app.serviceprovider.employees', { serviceProviderId: $scope.app.currentUser.userId }, { msg: "Employee has been updated successfully." });
                localStorageService.set('SuccessMsg', "");
                localStorageService.set('SuccessMsg', "Employee has been updated successfully.");
            }
            else if ($scope.app.currentUser.userRole == "Admin"){
                  $state.go('app.administration.employees', { msg: "Employee has been updated successfully." });
                  $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: " Employee has been updated successfully." });
            }
              
            else if ($scope.app.currentUser.userRole == "ServiceProvider" && $scope.ServiceProviderType == 1)
                $state.go('app.serviceprovider.profile', { serviceProviderId: $scope.app.currentUser.userId }, { msg: "Employee has been updated successfully." });
            localStorageService.set('SuccessMsg', "");
            localStorageService.set('SuccessMsg', "Employee has been updated successfully.");
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
             if (errors == null) {
                 $scope.alerts = [];
                 $scope.alerts.push({ type: 'warning', msg: "Employee not updated successfully." });
             }
             else {
                 $scope.alerts = [];
                 $scope.alerts.push({ type: 'warning', msg: "Employee not updated successfully." });
             }

         });

    }

    $scope.cancelSignUp = function () {
        $location.path('/login');
    };

    $scope.ProfileImagePath = function (files) {

        var a = document.getElementById('ProfileImage');
        if (a.value == "") {
            ProfileImagePathfileLabel.innerHTML = "No files selected";
        }
        else {
            var theSplit = a.value.split('\\');
            ProfileImagePathfileLabel.innerHTML = theSplit[theSplit.length - 1];
        }
        $scope.ProfileImageFile = files;
    };

    $scope.NationalIdPath = function (files) {

        var a = document.getElementById('IqamaNumber');
        if (a.value == "") {
            IqamaNumberPathfileLabel.innerHTML = "No files selected";
        }
        else {
            var theSplit = a.value.split('\\');
            IqamaNumberPathfileLabel.innerHTML = theSplit[theSplit.length - 1];
        }
        $scope.NationalIdFile = files;
    };

    $scope.IqamaNumberPath = function (files) {

        $scope.IqamaNumberFile = files;
    };




    function getAllEmployee() {
        $rootScope.showLoader = true;
        employeeService.getAllEmployees().then(function (response) {
            if (response != undefined && response != null)
                $scope.employee = response;
            $scope.totalcount = $scope.employee.length;
            sessionStorage.setItem("totalCount", $scope.totalcount);
            for (var i = 1; i <= $scope.totalcount; i++) {
                $scope.items.push({ id: i, name: "name " + i });
            }

            $scope.paging();

        });
    }


    $scope.addEmployee = function () {

        // $scope.registration.Services = $scope.SelectedServices;
        $scope.locationDetailsModel = {
            CityId: $scope.SelectedCityId,
            DistrictId: $scope.SelectedDistrictId,
            Latitude: $scope.Latitude,
            Longitude: $scope.Longitude
        };
        $scope.dataModel = {
            model: $scope.empRegistration,
            companyLogoFile: $scope.CompanyLogo,
            registrationCertificateFile1: $scope.Certificate1,
            registrationCertificateFile2: $scope.Certificate2,
            locationModel: $scope.locationDetailsModel
        }

        employeeService.saveEmployee($scope.dataModel).then(function (response) {
            $scope.alerts = [];

            $scope.registerForm.$setPristine();
            $scope.empRegistration = {
                ServiceProviderId: $scope.serviceProviderId,
                Email: "",
                FirstName: "",
                LastName: "",
                PhoneNumber: "",
                Password: "",
                ConfirmPassword: "",
                ProfileImage: "",
                NationalIdAndIqamaNumber: "",
                IqamaNumber: "",
                LicensePlateNumber: ""
            }
            
            if ($scope.app.currentUser.userRole == "ServiceProvider" && $scope.ServiceProviderType == -1) {

                $state.go('app.serviceprovider.employees', { serviceProviderId: $scope.app.currentUser.userId }, { msg: "Employee has been added successfully." });
                localStorageService.set('SuccessMsg', "");
                localStorageService.set('SuccessMsg', "Employee has been added successfully.");
                //$scope.alerts = [];
                //$scope.alerts.push({ type: 'Success', msg: "Employee has been added successfully." });
            }
            else if ($scope.app.currentUser.userRole == "Admin") {
                if ($stateParams.serviceProviderId == undefined || $stateParams.serviceProviderId == "") {

                    $state.go('app.administration.employees', { msg: "Employee has been added successfully." });
                    localStorageService.set('SuccessMsg', "");
                    localStorageService.set('SuccessMsg', "Employee has been added successfully.");
                }
                else
                    $state.go('app.administration.serviceprovider', { msg: "Employee has been added successfully." });
                localStorageService.set('SuccessMsg', "");
                localStorageService.set('SuccessMsg', "Employee has been added successfully.");
            }

            else if ($scope.app.currentUser.userRole == "ServiceProvider" && $scope.ServiceProviderType == 1)
                $state.go('app.serviceprovider.profile', { serviceProviderId: $scope.app.currentUser.userId }, { msg: "Employee has been added successfully." });
            //    $state.go('app.serviceprovider.employees', { serviceProviderId: $scope.app.currentUser.userId });

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
            $scope.alerts = [];
            $scope.alerts.push({ type: 'warning', msg: "Employee not added successfully." });

        });

    };

    $scope.addServiceProviderAsIndividual = function () {

        // $scope.registration.Services = $scope.SelectedServices;
        $scope.locationDetailsModel = {
            CityId: $scope.SelectedCityId,
            DistrictId: $scope.SelectedDistrictId,
            Latitude: $scope.Latitude,
            Longitude: $scope.Longitude
        };
        $scope.dataModel = {
            model: $scope.empRegistration,
            companyLogoFile: $scope.CompanyLogo,
            registrationCertificateFile1: $scope.Certificate1,
            registrationCertificateFile2: $scope.Certificate2,
            locationModel: $scope.locationDetailsModel
        }

        employeeService.saveServiceProviderAsIndividual($scope.dataModel).then(function (response) {


            if ($scope.app.currentUser.userId != "" && $scope.app.currentUser.userId != undefined) {
                $stateParams.msg = "";
                $state.go('app.administration.serviceprovider', { msg: "Service provider as individual added successfully." });
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
                         $scope.alerts.push({ type: 'warning', msg: "Service provider as individual not added successfully."});
                    }
        //var errors = [];
        //for (var key in response.data.ModelState) {
        //    for (var i = 0; i < response.data.ModelState[key].length; i++) {
        //        errors.push(response.data.ModelState[key][i]);
        //    }
        //}
        //if (errors == []) {
        //    $scope.alerts = [];
        //    $scope.alerts.push({ type: 'Success', msg: "Service provider as individual not added successfully." });

        //}
        //$scope.alerts = [];
        //$scope.alerts.push({ type: 'warning', msg: "Service provider as individual not added successfully."});

    });
    };

    //function getDistrictById(cityId) {

    //    //  $scope.SelectedCityId = cityId.CityId;
    //    // cityName = cityId.Description;
    //    districtService.GetDistrictByCityId(cityId).then(function (response) {
    //        //  $state.go('dashboard');

    //        $scope.itemDistrict = [];
    //        angular.forEach(response, function (value, key) {

    //            this.push(value);
    //        }, $scope.itemDistrict)



    //    }, function (response) {
    //        $scope.errorMessage = response.error_description;
    //    });

    //}
    function IsJsonString(str) {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    }
    function getCity() {
        $rootScope.showLoader = true;
        cityService.getCity().then(function (response) {
            //  $state.go('dashboard');

            $scope.itemCity = [];
            angular.forEach(response, function (value, key) {

                this.push(value);
            }, $scope.itemCity)
            if (employeeId != null) {

                getAllCarTypes();
                getAllDistrict();
                EditEmployee(employeeId);

            }
        }, function (response) {
            $scope.errorMessage = response.error_description;
        });
        $rootScope.showLoader = false;
    };

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

    $scope.GetSelectedCarType = function (carTypeId) {

        $scope.SelectedCarTypeId = carTypeId.CarTypeId;
        var description = carTypeId.Description;

    }


    getCity();

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };


    //paging work
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

        employeeService.paging(pagingModel).then(function (response) {

            if (response.data != null) {
                $scope.unSelect();
                $scope.SelectedEmployee = response.data;

                $scope.totalcount = response.totalRecords;
                if ($scope.currentPage == 1) {
                    $scope.itemCount = $scope.SelectedEmployee.length;
                    $scope.startRecord = 1;

                }
                else {

                    if ($scope.SelectedEmployee.length < $scope.itemsPerPage) {
                        $scope.itemCount = ($scope.itemsPerPage * $scope.currentPage) - $scope.itemsPerPage + $scope.SelectedEmployee.length;
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
    //$(function () {

    //    $('#chkAll').click(function (event) {

    //        if (this.checked) {
    //            $('.SingleChk').each(function () {
    //                this.checked = true;
    //            });

    //        } else {
    //            $('.SingleChk').each(function () {
    //                this.checked = false;
    //            });
    //        }
    //        $scope.$apply(function () {
    //            $scope.refreshIsMultipleDelete();
    //        });

    //    });

    //});
    ////end function

    ////function for selectaunselect checkbox
    //$scope.selectUnselectAll = function () {
    //    //

    //    if ($(".SingleChk").length == $('.SingleChk:checked').length) {

    //        $('.chkAll').prop("checked", true);
    //    }
    //    else {
    //        $('input:checkbox[name="chkAll"]').removeAttr("checked");
    //    }
    //      //if ($('input:checkbox[name="SingleChk"]').length == $('input:checkbox[name="SingleChk"]:checked').length) {

    //    //    $('input:checkbox[name="chkAll"]').attr("checked",true);
    //    //}
    //    //else {
    //    //    $('input:checkbox[name="chkAll"]').removeAttr("checked");
    //    //}
    //    $scope.refreshIsMultipleDelete();
    //}
    ////end function

    ////function for fill values for selected items
    //$scope.fillselectedEmployees = function () {

    //    $scope.selectedEmployees = [];
    //    $('input[name=SingleChk]').each(function () {
    //        if (this.checked) {
    //            $scope.selectedEmployees.push($(this).val());
    //        }
    //    });

    //}
    ////end function


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

    $scope.fillselectedEmployees = function () {

        $scope.selectedEmployees = [];
        $('input[name=SingleChk]').each(function () {
            if (this.checked) {
                $scope.selectedEmployees.push($(this).val());
            }
        });

    }

    $scope.refreshIsMultipleDelete = function () {

        $scope.fillselectedEmployees();
        if ($scope.selectedEmployees.length > 1) {
            $scope.IsMultipleDelete = true;
        }
        else {
            $scope.IsMultipleDelete = false;
        }
    }

    $scope.unSelect = function () {
        $('input:checkbox[name="chkAll"]').removeAttr("checked");
    }


    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete Employee?',
        bodyText: 'Are you sure you want to delete this employee?'
    };
    //function for delete multiple
    $scope.deleteselectedEmployees = function () {

        modalOptions.headerText = "Delete Employees?";
        modalOptions.bodyText = "Are you sure you want to delete selected employees?"
        modalService.showModal({}, modalOptions).then(function (result) {
            $scope.IsMultipleDelete = false;
            $scope.fillselectedEmployees();
            //   $('#deleteAllConfirmModal').modal('hide');
            var employee = $scope.selectedEmployees;

            employeeService.deleteselectedEmployees(employee).then(function (data) {

                if (data != null) {

                    if ((data.totalRecords - (($scope.currentPage - 1) * $scope.itemsPerPage)) < 1) {
                        $scope.currentPage = $scope.currentPage - 1;
                    }
                    getAllEmployee();
                    $scope.alerts = [];
                    $scope.alerts.push({ type: 'success', msg: data.Message });
                }
            }, function (error) {

            });

        });



    }
    //end function

    $scope.deleteEmployee = function (obj) {

        modalOptions.headerText = "Delete Employee(" + obj.FirstName + " " + obj.LastName + ")?";
        modalOptions.bodyText = "Are you sure you want to delete this employee?"
        modalService.showModal({}, modalOptions).then(function (result) {
            employeeService.deleteEmployee(obj.EmployeeId).then(function (response) {
                getAllEmployee();
                // update the employees list
                var index = $scope.EmployeeList.map(function (obj) { return obj.EmployeeId }).indexOf(obj.EmployeeId);
                if (index >= 0) {
                    $scope.EmployeeList.splice(index, 1);
                }
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: "Employee has been deleted successfully." });
              
            }, function (response) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'warning', msg: response.Message });


            });

        });
    }

    var startTimer = function () {

        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $rootScope.showLoader = false;
        }, 2000);
    }
}]);



