app.controller('carTypeController', ['$scope','$rootScope', '$http', '$stateParams', '$state', 'carTypeService', 'pagingservice', '$modal', '$log','modalService', '$anchorScroll',
function ($scope,$rootScope, $http, $stateParams, $state, carTypeService, pagingservice, $modal, $log, modalService, $anchorScroll) {
   $scope.CarTypeId,
   $scope.Description,
    getAllCarTypes();
   $scope.isUpdate = false;
   $scope.isAdd = false;
    function getAllCarTypes() {
 $rootScope.showLoader = true;
        $scope.searchCarText;
        carTypeService.getAllCarTypes().then(function (response) {

            $scope.AllCarTypes = [];

            angular.forEach(response, function (value, key) {
                $scope.AllCarTypes.push(value);

            })
 $rootScope.showLoader = false;
        });
    }
    $scope.CarTypeModel = {
        CarTypeId: $scope.CarTypeId,
        Description: $scope.Description,
    }
   
    $scope.addCarType = function () {
        
        $scope.alerts = [];
        
        carTypeService.addCarType($scope.CarTypeModel).then(function (response) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: "Car Type has been added successfully." });
            //$scope.AllCarTypes.push(response);
            getAllCarTypes();
            $scope.closeModal();
        }, function (response) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: response.Message });


        });
    };

    //pagging work
    $scope.itemsPerPage = pagingservice.pagesize;
    $scope.currentPage = 1;
    $scope.items = [];
    $scope.range = function () {
        $scope.totalpage = Math.ceil($scope.totalcount / $scope.itemsPerPage);
        $scope.showPaging = '';
        if ($scope.itemsPerPage == "") {
            $scope.itemsPerPage =pagingservice.pagesize;
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

            }
        },
             function (error) {
                 $scope.errormessage = "Error happend ";
             }
             );
    }
    //End work

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

        if ($('input[name="SingleChk"]').length == $('input[name="SingleChk"]:checked').length) {

            $('input:checkbox[name="chkAll"]').attr("checked", "checked");
        }
        else {
            $('input:checkbox[name="chkAll"]').removeAttr("checked");
        }
        $scope.refreshIsMultipleDelete();
    }
    //end function

    //function for fill values for selected items
    $scope.fillselectedServiceProviders = function () {

        $scope.selectedServiceProviders = [];
        $('input[name=SingleChk]').each(function () {
            if (this.checked) {
                $scope.selectedServiceProviders.push($(this).val());
            }
        });

    }
    //end function

    //function for refresh if multiple select
    $scope.refreshIsMultipleDelete = function () {

        $scope.fillselectedServiceProviders();
        if ($scope.selectedServiceProviders.length > 0) {
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
    $scope.deleteSelectedServiceProviders = function () {

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
    }
    //end function

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

    

    
    $scope.closepopAlert = function (index) {
        $scope.popalerts.splice(index, 1);
    };
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
        
    };
    var modalInstance = "";
    var openModal = function () {
        $scope.alerts = [];
        modalInstance = $modal.open({
            templateUrl: 'tpl/administration/configuration/cartype/cartype.html',
            scope: $scope,
            size: 'md',
        })
    };
    $scope.openAdd = function () {
        $scope.CarTypeModel = {
            CarTypeId: $scope.CarTypeId,
            Description: $scope.Description,
        }
        $scope.popalerts = [];
        $scope.isUpdate = false;
        $scope.isAdd = true;
        openModal();
    };
    $scope.selectCarType = function (selectedCar) {
        $scope.isUpdate = true;
        $scope.isAdd = false;
        $scope.CarTypeModel = angular.copy(selectedCar);
        openModal();
    }
    $scope.closeModal = function () {
        $scope.popalerts = [];
        if (modalInstance) {
            modalInstance.close('dismiss');
        }
    };
    $scope.updateCarType = function () {
        carTypeService.updateCarType($scope.CarTypeModel).then(function (response) {
            var index = $scope.AllCarTypes.map(function (elem) { return elem.CarTypeId }).indexOf($scope.CarTypeModel.CarTypeId);
            //if (index >= 0) {
            //    $scope.AllCarTypes[index].Description = $scope.CarTypeModel.Description;
            //}
            getAllCarTypes();
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: 'Car type has been updated successfully.' });
            $scope.closeModal();
        }, function (error) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: error.Message });
        })
    }
    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete car type?',
        bodyText: 'Are you sure you want to delete this car type?'
    };
    $scope.deleteCarType = function (car) {
        modalOptions.headerText = "Delete Car Type?";
        modalService.showModal({}, modalOptions).then(function (result) {
            carTypeService.deleteCarType(car.CarTypeId).then(function (response) {
                var index = $scope.AllCarTypes.map(function (elem) { return elem.CarTypeId }).indexOf(car.CarTypeId);
                //if (index >= 0) {
                //    $scope.AllCarTypes.splice(index, 1);
                //}
                getAllCarTypes();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: "Car type has been deleted successfully" });
            }, function (error) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'warning', msg: error.Message });
            })
            $anchorScroll();
        });
    };
    $scope.clear = function () {
       
        $scope.searchCarText = "";
        
    }

}]);
