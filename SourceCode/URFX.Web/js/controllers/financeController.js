app.controller('financeController', ['$scope', '$rootScope', '$http', '$stateParams', '$state', 'financeService', 'cityService', 'districtService', 'authService', 'pagingservice', 'modalService', '$timeout',
function ($scope, $rootScope, $http, $stateParams, $state, financeService, cityService, districtService, authService, pagingservice, modalService, $timeout) {
    $scope.ServiceProviderName;
    
    var date = new Date();
    var day = date.getUTCDate();
    var month = date.getUTCMonth()+1;
    var year = date.getUTCFullYear();
    $scope.DateToBeshow = month + '/' + day + '/' + year;

    var serviceProviderId = $stateParams.serviceProviderId;
    showFinanceDetails(serviceProviderId);
    function showFinanceDetails(serviceProviderId) {
        $rootScope.showLoader = true;
        financeService.showDetails(serviceProviderId).then(function (response) {
            $scope.financeDetails = response;
            $scope.ServiceProviderName = response.ServiceProviderName;
            startTimer();

        })

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
            paging();
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
            paging();
        }
    };

    $scope.nextPageDisabled = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };

    $scope.setPage = function (n) {

        $scope.currentPage = n;
        paging();
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
            paging();
        }
    }
    $scope.lastPage = function () {
        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage = $scope.pageCount();
            paging();
        }
    }

    $scope.searchRecord = function () {

        $scope.currentPage = 1;
        paging();
    }
    $scope.clear = function () {

        $scope.SearchText = "";

    }
    function paging() {

        var pagingModel = {
            CurrentPageIndex: $scope.currentPage,
            Pagesize: $scope.itemsPerPage,
            SearchText: $scope.SearchText
        }

        planService.paging(pagingModel).then(function (response) {

            if (response.data != null) {
                //$scope.unSelect();
                $scope.SelectedTransactions = response.data;
                $scope.totalcount = response.totalRecords;
                if ($scope.currentPage == 1) {
                    $scope.itemCount = $scope.SelectedTransactions.length;
                    $scope.startRecord = 1;

                }
                else {

                    if ($scope.SelectedTransactions.length < $scope.itemsPerPage) {
                        $scope.itemCount = ($scope.itemsPerPage * $scope.currentPage) - $scope.itemsPerPage + $scope.SelectedTransactions.length;
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
        $rootScope.showLoader = false;
    }
    //End work
    var startTimer = function () {

        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $rootScope.showLoader = false;
        }, 1500);
    }


    $scope.exportAction = function () {
        
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Report.xls");



    }

    $scope.exportActionPdf = function () {
        
        var pdf = new jsPDF('p', 'pt', [800, 900]);
        pdf.setFontSize(24);
        var date = new Date();
        var day=date.getUTCDate();
        var month=date.getUTCMonth()+1;
        var year=date.getUTCFullYear();
        var DateToBeshow= month+'/'+day+'/'+year;
        pdf.text("Finance Report", 300, 50, null);
        pdf.setFontSize(15);
        pdf.text("Date:" + DateToBeshow, 15, 70, null);
        pdf.setFontSize(15);
        pdf.text("Service Provider:"+$scope.ServiceProviderName, 550, 70, null);
        
           // pdf.addImage(imgData, 'PNG', 12, 30, 130, 40);
            pdf.cellInitialize();
            
            pdf.setFontSize(8);
            var headcount = 0;
           
                $.each($('#export tr'), function (i, row) {

                    $.each($(row).find("th"), function (j, cell) {

                        var txt = $(cell).text();
                        var width = (j == 4) ? 100 : 135; //make with column smaller

                        pdf.cell(15, 100, width, 40, txt, i);


                    });
                    
                    if (headcount > 0) {
                        $.each($(row).find("td"), function (j, cell) {

                            var txt = $(cell).text().trim() || " ";
                            var width = (j == 4) ? 100 : 135; //make with column smaller
                            pdf.cell(15, 100, width, 30, txt, i);

                        });
                    }
                    headcount++;
                });
           
            pdf.save('Report.pdf');
        
       
    }







}]);



