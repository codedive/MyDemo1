'use strict'

app.controller('categoryServiceController', ['$scope','$rootScope', 'modalService', 'categoryServiceService', '$modal',
function ($scope,$rootScope, modalService, categoryServiceService,$modal) {
    $scope.isUpdate = false;
    $scope.isAdd = false;
    $scope.ServiceList = [];
    $scope.alerts = [];
getAllCategory();
    $scope.ServiceModel = {
        ServiceCategoryId:0,
        Description: null,
        CategoryPicturePath:null,
    }
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    $scope.closepopAlert = function (index) {
        $scope.popalerts.splice(index, 1);
    };

    $scope.ServiceCategoryLogo = function (files) {

        var a = document.getElementById('image');
        if (a.value == "") {
            Imagefile.innerHTML = "No files selected";
        }
        else {
            var theSplit = a.value.split('\\');
            Imagefile.innerHTML = theSplit[theSplit.length - 1];
        }
         
        $scope.ServiceCategoryLogoFile = files;
    };
    var modalInstance = '';
    var openModal = function () {
        $scope.alerts = [];
        $scope.popalerts = [];
        modalInstance = $modal.open({
            templateUrl: 'tpl/administration/configuration/categoryservices/category.html',
            scope: $scope,
            size:'md'
        })
    }
    $scope.closeModal = function () {
        if (modalInstance) {
            modalInstance.close('dismiss');
        }
    }
function getAllCategory(){
$rootScope.showLoader = true;
    categoryServiceService.getAllCategoryServices().then(function (response) {
        $scope.searchText;
        //console.log(response)
        $scope.CategoryServiceList = response;
$rootScope.showLoader = false;
    },function(error){});
}
    $scope.openAdd = function () {
        $scope.popalerts = [];
        $scope.isUpdate = false;
        $scope.isAdd = true;
        $scope.ServiceModel = {
            ServiceCategoryId: 0,
            Description: null,
            CategoryPicturePath: null,
        }
        openModal();
    }
    $scope.selectCategoryService = function (Service) {
        $scope.isUpdate = true;
        $scope.isAdd = false;
        $scope.ServiceModel = angular.copy(Service);
        openModal();
    }
   
    $scope.addCategoryService = function () {
        $scope.dataModel = {
            model: $scope.ServiceModel,
            serviceCategoryLogoFile: $scope.ServiceCategoryLogoFile
        }
        categoryServiceService.addCategory($scope.dataModel).then(function (response) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: 'Category service has been added successfully.' });
            //$scope.CategoryServiceList.push(response);
          getAllCategory();
            $scope.closeModal();
        }, function (error) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: error.Message });
        });
    }
    $scope.updateCategoryService = function () {
        $scope.dataModel = {
            model: $scope.ServiceModel,
            serviceCategoryLogoFile: $scope.ServiceCategoryLogoFile
        }
        categoryServiceService.updateCategory($scope.ServiceModel.ServiceCategoryId,$scope.dataModel).then(function (response) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: 'Category service has been updated successfully.' });
            var index = $scope.CategoryServiceList.map(function (elem) { return elem.ServiceCategoryId }).indexOf($scope.ServiceModel.ServiceCategoryId);
            //$scope.PlanList[index] = response;
            $scope.CategoryServiceList[index].CategoryPicturePath = response.CategoryPicturePath;
            $scope.CategoryServiceList[index].Description = response.Description;
            $scope.closeModal();
        }, function (error) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: error.Message });
        })
    }
    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete Category service?',
        bodyText: 'Are you sure you want to delete this category service?'
    };
    $scope.deleteCategoryService = function (service) {
        modalOptions.headerText = "Delete Category?";
        modalService.showModal({}, modalOptions).then(function (result) {
            categoryServiceService.deleteCategory(service.ServiceCategoryId).then(function (response) {
                var index = $scope.CategoryServiceList.map(function (elem) { return elem.ServiceCategoryId }).indexOf(service.ServiceCategoryId);
                //$scope.CategoryServiceList.splice(index, 1);
                getAllCategory();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: 'Category service has been deleted successfully.' });
            }, function (error) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: error.Message });
            })
        });
    }

    $scope.clear = function () {
       
        $scope.searchText = "";
        
    }
}]);