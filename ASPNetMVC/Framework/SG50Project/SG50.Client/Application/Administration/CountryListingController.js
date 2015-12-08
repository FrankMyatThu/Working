app.factory('countryListingDataFactory', function ($http, $uibModal) {

    var countryListingDataFactory = {};
    var modalInstance_Loading = "";

    countryListingDataFactory.selectCountry = function (_Country_Criteria_Model) {
        modalInstance_Loading = $uibModal.open({
            animation: true,
            templateUrl: 'ModalContent_PopupLoading.html',
            controller: 'Modal_PopupLoading_Controller',
            backdrop: 'static',
            keyboard: false,
            windowClass: 'app-modal-window-loading',
        });
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_GetCountryList),
            headers: {
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
                'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            },
            data: _Country_Criteria_Model
        });
    };

    countryListingDataFactory.dismissDialog = function () {
        modalInstance_Loading.dismiss();
    };

    return countryListingDataFactory;
});

app.filter('offset', function () {
    return function (input, start) {
        if (!input || !input.length) { return; }
        start = parseInt(start, 10);        
        return input.slice(start);
    };
});

app.controller('Modal_PopupSearch_Controller', function ($scope, $uibModalInstance) {

    $scope.Search = function () {        
        $uibModalInstance.close();
    };

    $scope.Cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});

app.controller('Modal_PopupLoading_Controller', function ($uibModalInstance) { });

app.controller('CountryListingController', function ($scope, $http, $window, $uibModal, countryListingDataFactory) {      
    $scope.IsRecordFound = true;
    $scope.currentPage = 0;
    $scope.items = [];
    $scope.itemsLength = 0;        
    $scope.Country_Criteria_Model = {
        // -- Pager --
        "BatchIndex": 1,
        "PagerShowIndexOneUpToX": 10,
        "RecordPerPage": 10,
        "RecordPerBatch": 100,

        // -- Sorting --
        "OrderByClause": "CreatedDate ASC",

        // -- Data --
        "SrNo": "",
        "Id": "",
        "Name": "",
        "IsActive": "",
        "CreatedDate": "",
        "CreatedBy": "",
        "UpdatedDate": "",
        "UpdatedBy": "",
    };    
    $scope.List_tbl_Pager_To_Client_ByBatchIndex = "";

    $scope.IsSelectedAll = false;    
    $scope.currentDisplayedPageRecord = {};
    $scope.selectAll = function () {        
        $scope.currentDisplayedPageRecord = $scope.items.slice($scope.currentPage * $scope.Country_Criteria_Model.RecordPerPage, $scope.Country_Criteria_Model.RecordPerPage);        
        for (var i = 0; i < $scope.currentDisplayedPageRecord.length; i++) {
            var item = $scope.currentDisplayedPageRecord[i];            
            $scope.currentDisplayedPageRecord[item.Id] = $scope.IsSelectedAll;
        }
    };

    $scope.searchPage = function (size) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'ModalContent_PopupSearch.html',
            controller: 'Modal_PopupSearch_Controller',
            scope: $scope,
            size: size,
        });

        modalInstance.result.then(function () {
            $scope.Country_Criteria_Model.BatchIndex = 1;
            countryListingDataFactory.selectCountry($scope.Country_Criteria_Model)
            .success(function (data, status, headers, config) {
                $scope.dataOptimizer(data);
                $scope.currentPage = 0;
            }).error(function (data, status, headers, config) {
                $scope.errorHandler(data);
            });            
        }, function () {
            console.log('Modal dismissed at: ' + new Date());
        });

    };

    $scope.sort = {
        sortingOrder: 'CreatedDate',
        reverse: false
    };
    
    $scope.firstPage = function () {        
        $scope.Country_Criteria_Model.BatchIndex = 1;
        countryListingDataFactory.selectCountry($scope.Country_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer(data);
            $scope.currentPage = 0;
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data);
        });        
    }

    $scope.lastPage = function () {
        $scope.Country_Criteria_Model.BatchIndex = Math.ceil($scope.itemsLength / $scope.Country_Criteria_Model.RecordPerBatch);        
        countryListingDataFactory.selectCountry($scope.Country_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer(data);
            $scope.currentPage = $scope.List_tbl_Pager_To_Client_ByBatchIndex.length - 1;            
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data);
        });
    };

    $scope.prevPage = function () {
        if ($scope.Country_Criteria_Model.BatchIndex > 1) {            
            $scope.Country_Criteria_Model.BatchIndex--;            
            countryListingDataFactory.selectCountry($scope.Country_Criteria_Model)
            .success(function (data, status, headers, config) {
                $scope.dataOptimizer(data);
                $scope.currentPage = $scope.Country_Criteria_Model.PagerShowIndexOneUpToX - 1;
            }).error(function (data, status, headers, config) {
                $scope.errorHandler(data);
            });            
        }
    };

    $scope.prevPageDisabled = function () {        
        return $scope.Country_Criteria_Model.BatchIndex === 1 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.Country_Criteria_Model.BatchIndex < Math.ceil($scope.itemsLength / $scope.Country_Criteria_Model.RecordPerBatch)) {            
            $scope.Country_Criteria_Model.BatchIndex++;
            countryListingDataFactory.selectCountry($scope.Country_Criteria_Model)
            .success(function (data, status, headers, config) {
                $scope.dataOptimizer(data);
                $scope.currentPage = 0;
            }).error(function (data, status, headers, config) {
                $scope.errorHandler(data);
            });                     
        }
    };

    $scope.nextPageDisabled = function () {        
        return $scope.Country_Criteria_Model.BatchIndex === Math.ceil($scope.itemsLength / $scope.Country_Criteria_Model.RecordPerBatch) ? "disabled" : "";
    };

    $scope.setPage = function (n) {
        $scope.currentPage = n;
    };
    
    $scope.dataOptimizer = function (data) {
        if (data.success == false) {
            var str = '';
            for (var error in data.errors) {
                str += data.errors[error] + '\n';
            }
            console.log(str);
        }
        else {
            if (data[0].List_T.length <= 0) {
                $scope.IsRecordFound = false;
                return;
            }
            $scope.items = data[0].List_T;
            $scope.itemsLength = data[0].List_T[0].TotalRecordCount;
            $scope.List_tbl_Pager_To_Client_ByBatchIndex = data[0].List_tbl_Pager_To_Client.filter(function (item) { return item.BatchIndex === $scope.Country_Criteria_Model.BatchIndex; });
            countryListingDataFactory.dismissDialog();
        }
    };

    $scope.errorHandler = function (data) {
        var ErrorMessageValue = "";
        var ExceptionMessageValue = "";
        ErrorNotifier(data);
        function ErrorNotifier(data) {
            angular.forEach(data, function (value, key) {
                console.log("key = " + key + " value = " + value);
                if (key == "ExceptionMessage") {
                    ExceptionMessageValue = value;
                }
                ErrorMessageValue = value;
                if (typeof value === 'object') {
                    ErrorNotifier(value);
                }
            });
        }
        if (ExceptionMessageValue != "") {
            //$scope.error = ExceptionMessageValue;
        }
        else {
            //$scope.error = ErrorMessageValue;
        }
        //$scope.dataLoading = false;
    };

});

app.directive("customSort", function () {
    return {
        restrict: 'A',
        transclude: true,
        scope: true,
        template:
          '<a href="#" ng-click="sort_by(order)" style="color: #555555;">' +
          '    <span ng-transclude></span>' +
          '    <i ng-class="selectedCls(order)"></i>' +
          '</a>',
        link: function (scope, elem, attrs) {            
            scope.order = attrs.order;            

            scope.sort_by = function (newSortingOrder) {                
                var sort = scope.sort;
                if (sort.sortingOrder == newSortingOrder) {
                    sort.reverse = !sort.reverse;
                    console.log(newSortingOrder + " : " + sort.reverse);
                }

                sort.sortingOrder = newSortingOrder;
                scope.Country_Criteria_Model.OrderByClause = sort.sortingOrder + ((scope.sort.reverse) ? ' DESC' : ' ASC');                
                scope.firstPage();
            };
            
            scope.selectedCls = function (column) {
                if (column == scope.sort.sortingOrder) {
                    return ('fa fa-chevron-' + ((scope.sort.reverse) ? 'down' : 'up'));
                }
                else {
                    return 'fa fa-sort'
                }
            };
        }
    }
});

app.directive("ddlRecordFilterCount", ['countryListingDataFactory', function(countryListingDataFactory) {
    return {
        restrict: 'A',
        transclude: true,
        scope: false,
        template:
            ' Filter By&nbsp;<span class="glyphicon glyphicon-filter"></span> ' +
            ' <select id="ddlRecordFilterCount" name="ddlRecordFilterCount"  ' +
            ' ng-options="option.name for option in data.availableOptions track by option.id" ng-model="data.selectedOption" ' +
            ' ng-change="ddlRecordFilterCount_Change(data.selectedOption)" ></select> ',
        link: function (scope, elem, attrs) {
            scope.data = {
                availableOptions: [
                    { id: '10', name: '10' },
                    { id: '20', name: '20' },
                    { id: '50', name: '50' }
                ],
                selectedOption: { id: '10', name: '10' }
            };
            scope.ddlRecordFilterCount_Change = function (SelectedValue) {
                scope.Country_Criteria_Model.BatchIndex = 1;
                scope.Country_Criteria_Model.RecordPerPage = SelectedValue.id;
                scope.Country_Criteria_Model.RecordPerBatch = scope.Country_Criteria_Model.RecordPerPage * scope.Country_Criteria_Model.PagerShowIndexOneUpToX;
                countryListingDataFactory.selectCountry(scope.Country_Criteria_Model)
                .success(function (data, status, headers, config) {
                    scope.dataOptimizer(data);
                    scope.currentPage = 0;
                }).error(function (data, status, headers, config) {
                    scope.errorHandler(data);
                });                
            };
        }
    }
}]);