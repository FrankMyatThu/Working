//#region factory
app.factory('productCreateDataFactory', function ($http) {
    var productCreateDataFactory = {        
        createProduct: createProduct
    };
    return productCreateDataFactory;

    //////////////////////////////////

    function createProduct(_ProductBindingModel) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Product_Create),
            //headers: {
            //    'accept': 'application/json; charset=utf-8',
            //    'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
            //    'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            //},
            data: _ProductBindingModel
        });
    };
});
//#endregion

//#region controller
//#region Controller for Create product info.
app.controller('ProductCreateController', function ($scope, $timeout, productCreateDataFactory) {

    //#region Initial declaration
    //#endregion

    //#region Create
    $scope.Create = function () {
        productCreateDataFactory.createProduct($scope.item)
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