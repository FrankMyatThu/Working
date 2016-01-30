//#region factory
app.factory('orderCreateDataFactory', function ($http) {
    var orderCreateDataFactory = {
        createOrder: createOrder
    };
    return orderCreateDataFactory;

    //////////////////////////////////

    function createOrder(_OrderBindingModel) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Order_Create),
            //headers: {
            //    'accept': 'application/json; charset=utf-8',
            //    'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
            //    'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            //},
            data: _OrderBindingModel
        });
    };
});
//#endregion

//#region controller
//#region Controller for Create order info.
app.controller('OrderCreateController', function ($scope, $timeout, orderCreateDataFactory) {

    //#region Initial declaration
    //#endregion

    //#region Update
    $scope.Create = function () {
        orderCreateDataFactory.createOrder($scope.item)
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