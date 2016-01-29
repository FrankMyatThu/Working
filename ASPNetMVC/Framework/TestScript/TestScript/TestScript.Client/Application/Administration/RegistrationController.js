var app = angular.module('AppRegistration', []);
var directiveId_ngMatch = 'ngMatch';
app.directive(directiveId_ngMatch, ['$parse', function ($parse) {

    var directive = {
        link: link,
        restrict: 'A',
        require: '?ngModel'
    };
    return directive;

    function link(scope, elem, attrs, ctrl) {
        // if ngModel is not defined, we don't need to do anything
        if (!ctrl) return;
        if (!attrs[directiveId_ngMatch]) return;

        var firstValue = $parse(attrs[directiveId_ngMatch]);

        var validator = function (value) {
            var temp = firstValue(scope),
            v = value === temp;
            ctrl.$setValidity('match', v);
            return value;
        }

        ctrl.$parsers.unshift(validator);
        ctrl.$formatters.push(validator);
        attrs.$observe(directiveId_ngMatch, function () {
            validator(ctrl.$viewValue);
        });

    }
}]);
app.controller('RegistrationController', function ($scope, $http, $window, $timeout) {
    $scope.Login = function () { $window.location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Login); }
    $scope.register = function () {
        $scope.IsShow_error = false;
        $scope.IsShow_success = false;
        $scope.dataLoading = true;
        $scope.person = {
            "FirstName": $scope.FirstName,
            "LastName": $scope.LastName,
            "Email": $scope.Email,
            "UserName": $scope.Email,            
            "Password": $scope.Password,
            "ConfirmPassword": $scope.ConfirmPassword            
        };
        
        $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_CreateUser),
            data: $scope.person,
            headers: {
                'RequestVerificationToken': $scope.antiForgeryToken
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
                ResetData();
            }
            $scope.dataLoading = false;
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
            console.log("status " + status);
            if (status == "-1") {
                $scope.error = "Service unavailable.";
            }
            if (status == "401") {
                $scope.error = "Unauthorized.";
            }
            $scope.IsShow_error = true;
            $scope.dataLoading = false;
        });

        function ResetData()
        {
            $scope.FirstName = "";
            $scope.LastName = "";
            $scope.Email = "";            
            $scope.Password = "";
            $scope.ConfirmPassword = "";
            $scope.person = {};
            $scope.success = "Registration success";
            $scope.form.$setPristine();            
            $scope.IsShow_success = true;
            $timeout(function () {
                $scope.IsShow_success = false;
            }, 4000);
        }

    };
});
