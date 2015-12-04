app.filter('offset', function () {
    return function (input, start) {
        if (!input || !input.length) { return; }
        start = parseInt(start, 10);        
        return input.slice(start);
    };
});
app.controller('CountryController', function ($scope, $http, $window) {
    $scope.DisplayData = "";
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
    $scope.sort = {
        sortingOrder: 'CreatedDate',
        reverse: false
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
        console.log("$scope.Country_Criteria_Model = " + JSON.stringify($scope.Country_Criteria_Model));
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

                if (data[0].List_T.length <= 0)
                {
                    $scope.IsRecordFound = false;
                    return;
                }

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

app.directive("ddlRecordFilterCount", function () {
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
                scope.init();
                scope.currentPage = 0;
            };
        }
    }
});