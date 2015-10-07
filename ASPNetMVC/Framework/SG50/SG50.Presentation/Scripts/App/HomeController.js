app.controller('HomeController', function ($scope, $http, $window) {
    $scope.greeting = { text: 'Hello' };    
    $scope.sendForm = function () {        
        $http({
            method: 'POST',            
            url: 'https://localhost:44300/api/home/GetUserList',            
            headers: {                
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + $window.sessionStorage.getItem("JWTToken"),
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
                console.log(data);                
            }
        }).error(function (data, status, headers, config) {
            $scope.message = 'Unexpected Error';
        });
    };
});
