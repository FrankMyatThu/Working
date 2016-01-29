//#region factory
app.factory('countryCreateDataFactory', function ($http) {
    var countryCreateDataFactory = {        
        createCountry: createCountry
    };
    return countryCreateDataFactory;

    //////////////////////////////////

    function createCountry(_CountryBindingModel) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_CreateCountry),
            headers: {
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
                'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            },
            data: _CountryBindingModel
        });
    };
});
//#endregion

//#region controller
//#region Controller for Create country info.
app.controller('CountryCreateController', function ($scope, $timeout, countryCreateDataFactory) {

    //#region Initial declaration
    //#endregion

    //#region Update
    $scope.Create = function () {
        countryCreateDataFactory.createCountry($scope.item)
        .success(function (data, status, headers, config) {
            $scope.MessageNotifier();
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }
    //#endregion

    //#region Optimize data after Ajax request    
    $scope.MessageNotifier = function () {
        $scope.success = "Created successfully.";
        $timeout(function () {
            $scope.success = "";
        }, 4000);
    };
    //#endregion

});
//#endregion
//#endregion