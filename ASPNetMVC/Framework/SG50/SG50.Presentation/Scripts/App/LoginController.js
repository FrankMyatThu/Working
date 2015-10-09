var app = angular.module('AppLogin', []);
app.controller('LoginController', function ($scope, $http, $window) {    
    $scope.greeting = { text: 'Hello' };    
    $scope.login = function () {
        $scope.dataLoading = true;
        $scope.person = {
            "UserName": $scope.username,
            "Password": $scope.password
        };
        console.log("$scope.person\n " + JSON.stringify($scope.person) + "\n $scope.antiForgeryToken " + $scope.antiForgeryToken);
        $http({
            method: 'POST',
            url: 'https://localhost:44305/api/accounts/UserLogin',            
            data: $scope.person,
            headers: {
                'RequestVerificationToken': $scope.antiForgeryToken
            }
        }).success(function (data, status, headers, config) {
            $scope.message = '';
            if (data.success == false) {
                var str = '';
                for (var error in data.errors) {
                    str += data.errors[error] + '\n';
                }
                $scope.message = str;
            }
            else {                
                $window.sessionStorage.setItem("JWTToken", data);
                $scope.person = {};                
                $window.location.href = '../Home/Home';                
            }
        }).error(function (data, status, headers, config) {            
            angular.forEach(data, function (value, key) {
                console.log(key + ': ' + value);
                if (key == "ExceptionMessage") {
                    $scope.error = value;
                }
            });
            $scope.dataLoading = false;
        });
    };
});
