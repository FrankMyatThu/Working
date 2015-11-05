app.controller('CountryController', function ($scope, $http, $window) {
    $scope.DisplayData = "";    
    $scope.gridOptions = {
        paginationPageSizes: [25, 50, 75],
        paginationPageSize: 25,
        columnDefs: [
            { name: 'Id' },
            { name: 'Name' }
        ]
    };
    //$scope.gridOptions.data = [{ "Id": "0735aca6-d4fa-40f0-ba7c-000044a0600c", "Name": "Test Data 124870 value" }];
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
                //$scope.DisplayData = data;
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
