app.filter('offset', function () {
    return function (input, start) {
        if (!input || !input.length) { return; }
        start = parseInt(start, 10);        
        return input.slice(start);
    };
});
app.controller('CountryController', function ($scope, $http, $window) {
    $scope.DisplayData = "";    
    $scope.currentPage = 0;
    $scope.items = [];
    $scope.itemsLength = 0;        
    $scope.Country_Criteria_Model = {
        // -- Pager --
        "BatchIndex": 1,
        "PagerShowIndexOneUpToX": 5,
        "RecordPerPage": 5,
        "RecordPerBatch": 25,

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
    $scope.sort = {
        sortingOrder: 'CreatedDate',
        reverse: false
    };
   
    $scope.pageCount = function () {
        return Math.ceil($scope.itemsLength / $scope.Country_Criteria_Model.RecordPerPage) - 1;
    };

    $scope.firstPage = function () {
        $scope.Country_Criteria_Model.BatchIndex = 1;
        $scope.init();
        $scope.currentPage = 0;
    }

    $scope.lastPage = function () {
        $scope.Country_Criteria_Model.BatchIndex = Math.ceil($scope.itemsLength / $scope.Country_Criteria_Model.RecordPerBatch);
        $scope.init();
        $scope.currentPage = $scope.Country_Criteria_Model.PagerShowIndexOneUpToX - 1;
    }

    $scope.prevPage = function () {
        if ($scope.Country_Criteria_Model.BatchIndex > 1) {            
            $scope.Country_Criteria_Model.BatchIndex--;            
            $scope.init();
            $scope.currentPage = $scope.Country_Criteria_Model.PagerShowIndexOneUpToX - 1;
        }
    };

    $scope.prevPageDisabled = function () {        
        return $scope.Country_Criteria_Model.BatchIndex === 1 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.Country_Criteria_Model.BatchIndex < Math.ceil($scope.itemsLength / $scope.Country_Criteria_Model.RecordPerBatch)) {            
            $scope.Country_Criteria_Model.BatchIndex++;
            $scope.init();
            $scope.currentPage = 0;
        }
    };

    $scope.nextPageDisabled = function () {        
        return $scope.Country_Criteria_Model.BatchIndex === Math.ceil($scope.itemsLength / $scope.Country_Criteria_Model.RecordPerBatch) ? "disabled" : "";
    };

    $scope.setPage = function (n) {
        $scope.currentPage = n;
    };
    
    $scope.init = function () {        
        //console.log("$scope.Country_Criteria_Model = " + JSON.stringify($scope.Country_Criteria_Model));
        $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_GetCountryList),
            headers: {
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + $window.sessionStorage.getItem("JWTToken"),
                'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey // $scope.$parent.antiForgeryToken
            },
            data: $scope.Country_Criteria_Model
        }).success(function (data, status, headers, config) {
            if (data.success == false) {
                var str = '';
                for (var error in data.errors) {
                    str += data.errors[error] + '\n';
                }
                console.log(str);
            }
            else {                
                //console.log("json string List_tbl_Pager_To_Client : " + JSON.stringify(data[0].List_tbl_Pager_To_Client));
                //console.log(".......................................................................................................................");
                //console.log(".......................................................................................................................");
                //console.log("json string List_T : " + JSON.stringify(data[0].List_T));
                //console.log(".......................................................................................................................");
                //console.log(".......................................................................................................................");
               
                $scope.items = data[0].List_T;
                $scope.itemsLength = data[0].List_T[0].TotalRecordCount;                
                $scope.List_tbl_Pager_To_Client_ByBatchIndex = data[0].List_tbl_Pager_To_Client.filter(function (item) { return item.BatchIndex === $scope.Country_Criteria_Model.BatchIndex; });
            }
        }).error(function (data, status, headers, config) {
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
        });
    };    
});

app.directive("customSort", function () {
    return {
        restrict: 'A',
        transclude: true,
        scope: {
            order: '=',
            sort: '='
        },
        template:
          ' <a ng-click="sort_by(order)" style="color: #555555;">' +
          '    <span ng-transclude></span>' +
          '    <i ng-class="selectedCls(order)"></i>' +
          '</a>',
        link: function (scope) {

            // change sorting order
            scope.sort_by = function (newSortingOrder) {
                var sort = scope.sort;

                if (sort.sortingOrder == newSortingOrder) {
                    sort.reverse = !sort.reverse;
                }

                sort.sortingOrder = newSortingOrder;

                /// invoke server side here ...

            };
            
            scope.selectedCls = function (column) {
                if (column == scope.sort.sortingOrder) {
                    return ('fa fa-chevron-' + ((scope.sort.reverse) ? 'down' : 'up'));
                }
                else {
                    return 'fa fa-sort'
                }
            };
        }// end link
    }
});