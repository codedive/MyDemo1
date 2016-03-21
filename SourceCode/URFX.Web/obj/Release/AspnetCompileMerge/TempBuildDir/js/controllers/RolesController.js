app.controller('RoleController', ['$scope', '$http', '$stateParams', '$state', 'cityService', 'districtService', 'authService','roleService','pagingservice',
function ($scope, $http, $stateParams, $state, cityService, districtService, authService, roleService,pagingservice) {
   
   getRoles();
    function getRoles() {
       
        roleService.getAllRoles().then(function (response) {
         
            $scope.allRoles = response;
            $scope.totalcount = $scope.allRoles.length;
            sessionStorage.setItem("totalCount", $scope.totalcount);
            for (var i = 1; i <= $scope.totalcount; i++) {
                $scope.items.push({ id: i, name: "name " + i });
            }
            $scope.paging();
        })
    }
       
    
      $scope.itemsPerPage = '';
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
         
     $scope.SearchText="";
     $scope.paging();
    }
   

     $scope.paging = function () {
    
        var pagingModel = {
            CurrentPageIndex: $scope.currentPage,
            Pagesize: $scope.itemsPerPage,
            SearchText: $scope.SearchText
        }

        roleService.rolePaging(pagingModel).then(function (response) {
           
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

            }
        },
             function (error) {
                 $scope.errormessage = "Error happend ";
             }
             );
    }
       
    
     $scope.unSelect = function () {
         $('input:checkbox[name="chkAll"]').removeAttr("checked");
     }


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
        

         if ($(".SingleChk").length == $('.SingleChk:checked').length) {

             $('.chkAll').prop("checked", true);
         }
         else {
             $('input:checkbox[name="chkAll"]').removeAttr("checked");
         }
        
         $scope.refreshIsMultipleDelete();
     }
    //end function
}]);