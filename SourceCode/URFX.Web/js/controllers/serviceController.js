'use strict'

app.controller('serviceController', ['$scope', '$rootScope', 'modalService', 'serviceService', 'categoryServiceService', '$modal', '$anchorScroll', 'subCategory', 'subService', '$stateParams',
function ($scope, $rootScope, modalService, serviceService, categoryServiceService, $modal, $anchorScroll, subCategory, subService, $stateParams) {

    var modalInstance = '';

    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete Service?',
        bodyText: 'Are you sure you want to delete this Service?'
    };

    $scope.subServiceId = ($stateParams.ServiceId != undefined && $stateParams.ServiceId > 0) ? $stateParams.ServiceId : 0;

    $scope.isUpdate = false;
    $scope.isAdd = false;
    $scope.ServiceList = [];
    $scope.filterServiceCategoryId;
    $scope.alerts = [];
    $scope.value = 0;
    $scope.ServiceModel = {
        Description: null,
        HourlyRate: 0,
        IsActive: true,
        ParentServiceId: 0,
        Price: 0,
        ServiceCategoryId: 0,
        ServiceId: 0,
        ServicePicturePath: null,
        VisitRate: 0
    }

    $scope.ServiceLogo = function (files) {
        var a = document.getElementById('image');
        if (a.value == "") {
            Imagefile.innerHTML = "No files selected";
        }
        else {
            var theSplit = a.value.split('\\');
            Imagefile.innerHTML = theSplit[theSplit.length - 1];
        }
        $scope.ServiceLogoFile = files;
    };



    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.closepopAlert = function (index) {
        $scope.popalerts.splice(index, 1);
    };




    $scope.closeModal = function () {
        if (modalInstance) {
            modalInstance.close('dismiss');
        }
    }


    $scope.SelectedFilter = function (value) {
        if (value == "0") {
            $scope.FilteredList = [];
            angular.forEach($scope.ServiceList, function (value, key) {
                if (value.ServiceCategoryId != null) {

                    angular.forEach($scope.categoryList, function (valuecategory, keycategory) {

                        if (valuecategory.ServiceCategoryId == value.ServiceCategoryId) {

                            $scope.FilteredList.push(
                            {
                                Description: value.Description,
                                IsActive: value.IsActive,
                                ServiceCategoryId: value.ServiceCategoryId,
                                ServiceId: value.ServiceId,
                                ServicePicturePath: value.ServicePicturePath,
                                CategoryName: valuecategory.Description,

                            }
                           );
                        }

                    })
                }
            })
        }
        else {

            $scope.FilteredList = [];

            angular.forEach($scope.ServiceList, function (value, key) {
                var subServiceobj = new Object();
                var categoryobj = new Object();
                if (value.ServiceCategoryId == null) {

                    var index = $scope.SubServiceList.map(function (elem) { return elem.ServiceId }).indexOf(value.ParentServiceId);

                    if (index != undefined && index >= 0) {
                        subServiceobj.Description = $scope.SubServiceList[index].Description;
                        subServiceobj.ServiceId = $scope.SubServiceList[index].ServiceId;
                        subServiceobj.ServiceCategoryId = $scope.SubServiceList[index].ServiceCategoryId;
                    }
                    $scope.FilteredList.push(
                            {
                                Description: value.Description,
                                IsActive: value.IsActive,
                                ServiceCategoryId: value.ServiceCategoryId,
                                ServiceId: value.ServiceId,
                                ServicePicturePath: value.ServicePicturePath,
                                ServiceName: subServiceobj.Description,
                                ParentServiceId: value.ParentServiceId,
                                valuecategoryServiceId: subServiceobj.ServiceId,
                                CategoryName: categoryobj.Description
                            }
                           );

                }
            })

        }
    }

    $scope.GetSubServiceByServiceId = function (serviceId) {

        $scope.SelectedServiceId = serviceId;
        $scope.selectedServiceName = [];
        $scope.serviceModel = {}
        subService.getSubServiceByServiceId(serviceId).then(function (response) {

            if (response != "") {                
                $scope.FilteredList = [];
                angular.forEach(response, function (value, key) {

                    var subServiceobj = new Object();
                    var index = $scope.SubServiceList.map(function (elem) { return elem.ServiceId }).indexOf(serviceId);

                    if (index != undefined && index >= 0) {
                        subServiceobj.Description = $scope.SubServiceList[index].Description;
                        subServiceobj.ServiceId = $scope.SubServiceList[index].ServiceId;
                        subServiceobj.ServiceCategoryId = $scope.SubServiceList[index].ServiceCategoryId;
                    }
                    var Categoryobj = new Object();
                    var categoryindex = $scope.categoryList.map(function (elem) { return elem.ServiceCategoryId }).indexOf(subServiceobj.ServiceCategoryId);                    
                    if (categoryindex != undefined && categoryindex >= 0) {                        
                        Categoryobj.Description = $scope.categoryList[categoryindex].Description;
                        Categoryobj.ServiceId = $scope.categoryList[categoryindex].ServiceId;
                        Categoryobj.ServiceCategoryId = $scope.categoryList[categoryindex].ServiceCategoryId;
                    }
                    $scope.FilteredList.push(
                            {
                                Description: value.Description,
                                IsActive: value.IsActive,
                                ServiceCategoryId: value.ServiceCategoryId,
                                ServiceId: value.ServiceId,
                                ServicePicturePath: value.ServicePicturePath,
                                ServiceName: subServiceobj.Description,
                                ParentServiceId: value.ParentServiceId,

                                CategoryName: Categoryobj.Description
                            }
                           );
                })
            }
            else {
                $scope.FilteredList = [];
            }



        }, function (response) {
            $scope.errorMessage = response.error_description;
        });
    }



    $scope.GetSubCategoryByCategoryId = function (categoryId) {        
        if (categoryId != null) {
            $scope.SelectedCategoryId = categoryId;
            $scope.itemSubService = [];
            $scope.selectedServiceName = [];
            $scope.serviceModel = {}
            subCategory.getSubCategoryByCategoryId(categoryId).then(function (response) {
                if (response != "") {                    
                    $scope.FilterServiceList = [];
                    $scope.FilterServiceList.push(response);
                    $scope.SubServiceList = response;
                    $scope.FilteredList = [];
                    angular.forEach(response, function (value, key) {

                        var subServiceobj = new Object();
                        var index = $scope.categoryList.map(function (elem) { return elem.ServiceCategoryId }).indexOf(categoryId);

                        if (index != undefined && index >= 0) {
                            subServiceobj.Description = $scope.categoryList[index].Description;
                            subServiceobj.ServiceId = $scope.categoryList[index].ServiceId;
                            subServiceobj.ServiceCategoryId = $scope.categoryList[index].ServiceCategoryId;
                        }
                        $scope.FilteredList.push(
                                {
                                    Description: value.Description,
                                    IsActive: value.IsActive,
                                    ServiceCategoryId: value.ServiceCategoryId,
                                    ServiceId: value.ServiceId,
                                    ServicePicturePath: value.ServicePicturePath,
                                    //ServiceName: subServiceobj.Description,
                                    ParentServiceId: value.ParentServiceId,
                                    // valuecategoryServiceId: subServiceobj.ServiceId,
                                    CategoryName: subServiceobj.Description,
                                    IsActualService: value.IsActualService
                                }
                               );
                    })
                }
                else {
                    $scope.FilteredList = [];
                }
                $scope.SelectedServiceId = "";

            }, function (response) {
                $scope.errorMessage = response.error_description;
            });
        }
        else {
            $scope.SelectedCategoryId = null;
            $scope.SelectedServiceId = null;
            getAllService();
        }
    }

    $scope.openAdd = function () {

        $scope.isUpdate = false;
        $scope.popalerts = [];
        $scope.isAdd = true;
        clearModal();

        $scope.ServiceModel = {
            "Description": null,
            "HourlyRate": 0,
            "IsActive": true,
            "ParentServiceId": null,
            "Price": 0,
            "ServiceCategoryId": null,
            "ServiceId": 0,
            "ServicePicturePath": null,
            "VisitRate": 0
        }
        openModal();
    }

    $scope.selectService = function (service) {
        $scope.isUpdate = true;
        $scope.isAdd = false;
        $scope.popalerts = [];
        $scope.ServiceModel = angular.copy(service);        
        if ($scope.ServiceModel.ServiceCategoryId == null) {
            $scope.ServiceModel.ServiceCategoryId = service.ServiceCategory.ServiceCategoryId;
        }        
        var serviceCategoryId = $scope.ServiceModel.ServiceCategoryId;
        $scope.GetSubServicesByCategoryId(serviceCategoryId);
        openModal();
    }

    $scope.addService = function () {        
        if ($scope.value == "1") {
            $scope.ServiceModel.ServiceCategoryId = 0;
        }
        $scope.finalModal = {
            modal: $scope.ServiceModel,
            ServiceLogo: $scope.ServiceLogoFile
        }
        serviceService.addService($scope.finalModal).then(function (response) {
            $scope.ServiceLogoFile = "";
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: 'Service has been added successfully.' });
            //$scope.ServiceList.push(response);
            //if ($scope.value == "0") { 
            //else {
            //    getAllService();
            //}
            //}
            //else {            
            if ($scope.SelectedServiceId != null) {
                $scope.GetSubServiceByServiceId($scope.SelectedServiceId);
            }
            else {
                $scope.GetSubCategoryByCategoryId($scope.SelectedCategoryId);
            }
            //else {
            //    getAllService();
            //}
            // }

            $scope.closeModal();
        }, function (error) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: error.Message });
        });
    }

    $scope.updateService = function () {
        $scope.finalModal = {
            modal: $scope.ServiceModel,
            ServiceLogo: $scope.ServiceLogoFile
        }
        serviceService.updateService($scope.ServiceModel.ServiceId, $scope.finalModal).then(function (response) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: 'Service has been updated successfully.' });
            var index = $scope.ServiceList.map(function (elem) { return elem.ServiceId }).indexOf($scope.ServiceModel.ServiceId);
            //$scope.PlanList[index] = response;

            $scope.ServiceList[index].HourlyRate = response.HourlyRate;
            $scope.ServiceList[index].CreatedDate = response.CreatedDate;
            $scope.ServiceList[index].Description = response.Description;
            $scope.ServiceList[index].ParentServiceId = response.ParentServiceId;
            $scope.ServiceList[index].IsActive = response.IsActive;
            $scope.ServiceList[index].Price = response.Price;
            $scope.ServiceList[index].ServiceCategory = response.ServiceCategory;
            $scope.ServiceList[index].ServiceCategoryId = response.ServiceCategoryId;
            $scope.ServiceList[index].ServicePicturePath = response.ServicePicturePath;
            $scope.ServiceList[index].SubServices = response.SubServices;
            $scope.ServiceList[index].VisitRate = response.VisitRate;
            $scope.closeModal();
        }, function (error) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: error.Message });
        })
    }

    $scope.deleteService = function (Service) {
        modalOptions.headerText = "Delete Service?";
        modalService.showModal({}, modalOptions).then(function (result) {
            serviceService.deleteService(Service.ServiceId).then(function (response) {
                var index = $scope.ServiceList.map(function (elem) { return elem.ServiceId }).indexOf(Service.ServiceId);
                //$scope.ServiceList.splice(index, 1);
                if ($scope.SelectedServiceId != null) {
                    $scope.GetSubServiceByServiceId($scope.SelectedServiceId);
                }
                else {
                    $scope.GetSubCategoryByCategoryId($scope.SelectedCategoryId);
                    getAllService();
                }
                //if ($scope.value == "0") {
                //    if ($scope.SelectedCategoryId != null) {
                //        $scope.GetSubCategoryByCategoryId($scope.SelectedCategoryId);
                //    }
                //    else {
                //        getAllService();
                //    }
                //}
                //else {
                //    if ($scope.SelectedServiceId != null) {
                //        $scope.GetSubServiceByServiceId($scope.SelectedServiceId);
                //    }
                //    else {
                //        getAllService();
                //    }
                //}

                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: 'Service has been deleted successfully.' });
            }, function (error) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: error.Message });
            })
            $anchorScroll();
        });
    }

    $scope.clear = function () {
        $scope.searchText = "";
    }






    $scope.GetSubCategoryByCategoryIdForAdd = function (categoryId) {

        // $scope.SelectedCategoryId = categoryId;
        //$scope.itemSubService = [];
        //$scope.selectedServiceName = [];
        //$scope.serviceModel = {}
        //subCategory.getSubCategoryByCategoryId(categoryId).then(function (response) {
        //    $scope.FilterServiceListForAdd = [];
        //    angular.forEach(response, function (value, key) {
        //        this.push(value);
        //    }, $scope.FilterServiceListForAdd)
        //}, function (response) {
        //    $scope.errorMessage = response.error_description;
        //});

    }



    $scope.SubServiceListByCategoryId = [];
    $scope.GetSubServicesByCategoryId = function (categoryId) {        
        $scope.SubServiceListAsPerCategory = [];
        serviceService.getAllServicesByCategoryId(categoryId).then(function (response) {            
            $scope.FilterServiceListForAdd = [];
            angular.forEach(response, function (value, key) {
                this.push(value);
            }, $scope.FilterServiceListForAdd)
        }, function () {
        });
    }

    init();

    function init() {
        getAllCategory();
    }

    function getAllCategory() {
        $rootScope.showLoader = true;
        categoryServiceService.getAllCategoryServices().then(function (response) {
            $scope.searchText;
            $scope.categoryList = response;
            $rootScope.showLoader = false;
            getAllService();
        }, function (error) { });
    }
    function getAllService() {
        $rootScope.showLoader = true;
        serviceService.getAllServices($scope.subServiceId).then(function (response) {
            $scope.ServiceList = response;
            $scope.SubServiceList = [];
            $scope.FilteredList = [];            
            $scope.FilteredList = response;
            //angular.forEach($scope.ServiceList, function (value, key) {

            //    if (value.ServiceCategoryId != null) {
            //        $scope.SubServiceList.push(value);
            //    }

            //})

            //$scope.SelectedFilter($scope.value);
            $rootScope.showLoader = false;

        }, function (error) { });
    }
    function openModal() {

        $scope.alerts = [];
        modalInstance = $modal.open({
            templateUrl: 'tpl/administration/configuration/services/service.html',
            scope: $scope,
            size: 'md'
        })
    }
    function clearModal() {
        $scope.ServiceModel = {
            Description: null,
            HourlyRate: 0,
            IsActive: true,
            ParentServiceId: 0,
            Price: 0,
            ServiceCategoryId: 0,
            ServiceId: 0,
            ServicePicturePath: null,
            VisitRate: 0
        }
    }
}]);