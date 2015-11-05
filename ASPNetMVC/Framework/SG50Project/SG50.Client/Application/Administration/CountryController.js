app.controller('CountryController', function ($scope, $http, $window, uiGridConstants) {
    $scope.DisplayData = "";
    $scope.CustomizedGridFilter = [
                    {
                        condition: uiGridConstants.filter.CONTAINS,
                        placeholder: 'Contains'
                    },
                    {
                        condition: uiGridConstants.filter.GREATER_THAN,
                        placeholder: 'Greater than'
                    },
                    {
                        condition: uiGridConstants.filter.LESS_THAN,
                        placeholder: 'Less than'
                    },
    ];
    $scope.gridOptions = {
        enableFiltering: true,
        paginationPageSizes: [25, 50, 75],
        paginationPageSize: 25,
        columnDefs: [
            { field: 'Id', filters: $scope.CustomizedGridFilter },
            { field: 'Name', filters: $scope.CustomizedGridFilter },
            { field: 'IsActive', filters: $scope.CustomizedGridFilter },
            { field: 'CreatedDate', filters: $scope.CustomizedGridFilter },
            { field: 'CreatedBy', filters: $scope.CustomizedGridFilter },
            { field: 'UpdatedDate', filters: $scope.CustomizedGridFilter },
            { field: 'UpdatedBy', filters: $scope.CustomizedGridFilter },
        ]
    };
    $scope.toggleFiltering = function () {
        $scope.gridOptions.enableFiltering = !$scope.gridOptions.enableFiltering;
        alert("$scope.gridOptions.enableFiltering " + $scope.gridOptions.enableFiltering);
        /// Need to add animate angular js ....
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
    };
    $scope.init = function () {
        console.log("$scope.init...");        
        $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_GetCountryList),
            headers: {
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + $window.sessionStorage.getItem("JWTToken"),
                'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey // $scope.$parent.antiForgeryToken
            }
        }).success(function (data, status, headers, config) {
            if (data.success == false) {
                var str = '';
                for (var error in data.errors) {
                    str += data.errors[error] + '\n';
                }
                console.log(str);
            }
            else {
                console.log(JSON.stringify(data));
                $scope.gridOptions.data = data;                
                //$scope.dataLoading = false;
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

    //angular.element(document).ready(function () {
    //    //console.log("inside jqlite ApplicationConfig.AntiForgeryTokenKey " + ApplicationConfig.AntiForgeryTokenKey);
    //    //console.log("CountryController $scope.$parent.antiForgeryToken ... " + $scope.$parent.antiForgeryToken);
    //    //console.log("CountryController init event " + ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_GetCountryList));
    //    //console.log("$scope.$parent.antiForgeryToken " + $scope.$parent.antiForgeryToken);        
    //});
    
    $scope.Home = function () {
        $scope.dataLoading = true;
        $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_GetUserList),
            headers: {
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + $window.sessionStorage.getItem("JWTToken"),
                'RequestVerificationToken': $scope.$parent.antiForgeryToken
            }
        }).success(function (data, status, headers, config) {
            if (data.success == false) {
                var str = '';
                for (var error in data.errors) {
                    str += data.errors[error] + '\n';
                }
                console.log(str);
            }
            else {
                console.log(data);
                $scope.DisplayData = data;
                $scope.dataLoading = false;
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
                $scope.error = ExceptionMessageValue;
            }
            else {
                $scope.error = ErrorMessageValue;
            }
            $scope.dataLoading = false;
        });
    };
});
