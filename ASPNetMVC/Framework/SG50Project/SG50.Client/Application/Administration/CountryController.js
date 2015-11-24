app.filter('offset', function () {
    return function (input, start) {
        if (!input || !input.length) { return; }
        start = parseInt(start, 10);
        return input.slice(start);
    };
});
app.controller('CountryController', function ($scope, $http, $window) {
    $scope.DisplayData = "";
    $scope.itemsPerPage = 5;
    $scope.currentPage = 0;
    $scope.items = [];    
    $scope.Country_Criteria_Model = {
        "BatchIndex": "2",
        "PagerShowIndexOneUpToX": "5",
        "RecordPerPage": "5",
        "RecordPerBatch": "25",
        "SrNo": "",
        "Id": "",
        "Name": "",
        "IsActive": "",
        "CreatedDate": "",
        "CreatedBy": "",
        "UpdatedDate": "",
        "UpdatedBy": "",
    };
    $scope.List_tbl_Pager_To_Client = "";

    $scope.range = function () {
        var rangeSize = 5;
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

        ///var _List_tbl_Pager_To_Client = Json.where(x=> x.BatchIndex == 2).select(x=>x).list();
        /// return _List_tbl_Pager_To_Client;

    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.pageCount = function () {        
        return Math.ceil($scope.items.length / $scope.itemsPerPage) - 1;
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage++;
        }
    };

    $scope.nextPageDisabled = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };

    $scope.setPage = function (n) {
        $scope.currentPage = n;
    };
    
    $scope.init = function () {
        console.log("$scope.Country_Criteria_Model = " + $scope.Country_Criteria_Model);
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
                console.log("json string Raw : " + JSON.stringify(data));
                console.log(".......................................................................................................................");
                console.log(".......................................................................................................................");
                console.log("json string List_tbl_Pager_To_Client : " + JSON.stringify(data[0].List_tbl_Pager_To_Client));
                console.log(".......................................................................................................................");
                console.log(".......................................................................................................................");
                console.log("json string List_T : " + JSON.stringify(data[0].List_T));
                console.log(".......................................................................................................................");
                console.log(".......................................................................................................................");
                $scope.items = data[0].List_T;
                //$scope.DisplayData = data;
                //$scope.dataLoading = false;

                var found = data[0].List_tbl_Pager_To_Client.filter(function (item) { return item.BatchIndex === 2; });
                console.log('found', JSON.stringify(found));

                $scope.List_tbl_Pager_To_Client = data[0].List_tbl_Pager_To_Client;

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