app.controller('cityController', ['$scope','$rootScope', '$http', '$stateParams', '$state', 'cityService', 'pagingservice', '$modal', '$log', 'modalService', '$anchorScroll','$timeout',
function ($scope,$rootScope, $http, $stateParams, $state, cityService, pagingservice, $modal, $log, modalService, $anchorScroll,$timeout) {
    $scope.CityName;
    $scope.CityId;
    $scope.selectedCity={};
    $scope.popalerts = [];
    //console.log($scope.range);
    getCity();
    function getCity() {
      $rootScope.showLoader = true;
        $scope.searchCityText;
        cityService.getCity().then(function (response) {
            $scope.itemCity = [];
            angular.forEach(response, function (value, key) {
               
                this.push(value);
            }, $scope.itemCity)
            startTimer();
        }, function (response) {
            $scope.errorMessage = response.error_description;
            startTimer();
        });
        
    };
    $scope.CityModel = {
            CityId:0,
            Description: "",
        }
    $scope.addCity = function () {

        cityService.addCity($scope.CityModel).then(function (response) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: "City has been added successfully." });
            //$scope.itemCity.push(response);
            getCity();
            $scope.closeModal();
        }, function (response) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: "City not added successfully." });


        });

    };
    $scope.updateCity = function () {
       
        cityService.updateCity($scope.CityModel).then(function (response) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: "City has been updated successfully." });
            var index = $scope.itemCity.map(function (elem) { return elem.CityId }).indexOf($scope.CityModel.CityId);
            if (index >= 0) {
                $scope.itemCity[index].Description = $scope.CityModel.Description;
            }
            $scope.closeModal();
        }, function (response) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: "City not updated successfully" });


        });

    };

    //pagging work
    $scope.itemsPerPage = pagingservice.pagesize;
    $scope.currentPage = 1;
    $scope.items = [];
    $scope.range123 = function () {
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

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    var modalInstance = "";
    $scope.isUpdate = false;
    $scope.isAdd = false;
    $scope.selectCity = function (city) {
        $scope.alerts = [];
        $scope.isUpdate = true;
        $scope.isAdd = false;
        $scope.CityModel = angular.copy(city);
        openModal();
    }
    var openModal = function () {
        modalInstance = $modal.open({
            templateUrl: 'tpl/administration/configuration/city/city.html',
            scope: $scope,
            size:'md'
        })
    }
    $scope.openAddCity = function () {
        $scope.CityModel = {
            CityId: 0,
            Description: "",
        }
        $scope.popalerts = [];
        $scope.alerts = [];
        $scope.isUpdate = false;
        $scope.isAdd = true;
        openModal();
    }
    $scope.closeModal = function () {
        $scope.popalerts = [];
        if (modalInstance) {
            modalInstance.close('dismiss');
        }
    }

    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete city?',
        bodyText: 'Are you sure you want to delete this city?'
    };
    $scope.deleteCity = function (city) {

        modalOptions.headerText = "Delete city?";
        modalService.showModal({}, modalOptions).then(function (result) {
            cityService.deleteCity(city.CityId).then(function (response) {
                //var index = $scope.itemCity.map(function (elem) { return elem.CityId }).indexOf(city.CityId);
                //if (index >= 0) {
                //    $scope.itemCity.splice(index, 1);
                //}
                getCity();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: "City has been deleted successfully." });
            }, function (error) {

            })
            $anchorScroll();

        });
    };
    $scope.closepopAlert = function (index) {
        $scope.popalerts.splice(index, 1);
    };

    $scope.clear = function () {
       
        $scope.searchCityText = "";
        
    }
    var startTimer = function () {

        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $rootScope.showLoader = false;
        }, 2000);
    }
}]);
