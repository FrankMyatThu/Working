app.controller('CountryController', function ($scope, $http, $window, $q) {
    $scope.DisplayData = "";
    $scope.gridOptions = {};
    $scope.gridOptions.columnDefs = [
      { name: 'Id' },
      { name: 'Name' }
    ];
    $scope.gridOptions.data = "";
    //$scope.gridOptions.data = [{ "Id": "0735aca6-d4fa-40f0-ba7c-000044a0600c", "Name": "Test Data 124870 value" }, { "Id": "4f5b3d6e-6753-48e7-b3fc-0000a10a7df5", "Name": "Test Data 113034 value" }, { "Id": "c722c447-fb33-45dd-b4e9-0000fc5c1b5c", "Name": "Test Data 164240 value" }, { "Id": "c9fb67ff-c178-4ff7-bb51-0002b92f8825", "Name": "Test Data 43956 value" }, { "Id": "2b39f982-ce0c-4314-8101-000313083c94", "Name": "Test Data 57350 value" }, { "Id": "2a3eea10-96c5-455a-bc01-000454ec8674", "Name": "Test Data 98202 value" }, { "Id": "7730db34-6b71-4405-8adb-0004d22c563e", "Name": "Test Data 151804 value" }, { "Id": "1036b382-c586-4665-abb1-00059b42561b", "Name": "Test Data 17865 value" }, { "Id": "3f21d02f-25a7-419e-b19c-00061767dc3e", "Name": "Test Data 71344 value" }, { "Id": "69912479-51cc-43af-b186-000620ccfdb8", "Name": "Test Data 3873 value" }];

    $scope.init = function () {
        console.log("$scope.init...");
        var deferred = $q.defer();
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
                deferred.resolve(data);
                console.log(JSON.stringify(data));                
                //$scope.gridOptions.data = [{ "Id": "0735aca6-d4fa-40f0-ba7c-000044a0600c", "Name": "Test Data 124870 value" }, { "Id": "4f5b3d6e-6753-48e7-b3fc-0000a10a7df5", "Name": "Test Data 113034 value" }, { "Id": "c722c447-fb33-45dd-b4e9-0000fc5c1b5c", "Name": "Test Data 164240 value" }, { "Id": "c9fb67ff-c178-4ff7-bb51-0002b92f8825", "Name": "Test Data 43956 value" }, { "Id": "2b39f982-ce0c-4314-8101-000313083c94", "Name": "Test Data 57350 value" }, { "Id": "2a3eea10-96c5-455a-bc01-000454ec8674", "Name": "Test Data 98202 value" }, { "Id": "7730db34-6b71-4405-8adb-0004d22c563e", "Name": "Test Data 151804 value" }, { "Id": "1036b382-c586-4665-abb1-00059b42561b", "Name": "Test Data 17865 value" }, { "Id": "3f21d02f-25a7-419e-b19c-00061767dc3e", "Name": "Test Data 71344 value" }, { "Id": "69912479-51cc-43af-b186-000620ccfdb8", "Name": "Test Data 3873 value" }];
                $scope.gridOptions.data = [{ "Id": "0735aca6-d4fa-40f0-ba7c-000044a0600c", "Name": "Test Data 124870 value" }, { "Id": "4f5b3d6e-6753-48e7-b3fc-0000a10a7df5", "Name": "Test Data 113034 value" }, { "Id": "c722c447-fb33-45dd-b4e9-0000fc5c1b5c", "Name": "Test Data 164240 value" }, { "Id": "c9fb67ff-c178-4ff7-bb51-0002b92f8825", "Name": "Test Data 43956 value" }, { "Id": "2b39f982-ce0c-4314-8101-000313083c94", "Name": "Test Data 57350 value" }, { "Id": "2a3eea10-96c5-455a-bc01-000454ec8674", "Name": "Test Data 98202 value" }, { "Id": "7730db34-6b71-4405-8adb-0004d22c563e", "Name": "Test Data 151804 value" }, { "Id": "1036b382-c586-4665-abb1-00059b42561b", "Name": "Test Data 17865 value" }, { "Id": "3f21d02f-25a7-419e-b19c-00061767dc3e", "Name": "Test Data 71344 value" }, { "Id": "69912479-51cc-43af-b186-000620ccfdb8", "Name": "Test Data 3873 value" }];
                //$scope.DisplayData = data;
                //$scope.dataLoading = false;
            }
        }).error(function (data, status, headers, config) {
            deferred.reject;
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
        return deferred.promise;
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
