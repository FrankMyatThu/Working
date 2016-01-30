//#region factory
app.factory('productDetailDataFactory', function ($http) {
    var productDetailDataFactory = {
        selectProduct: selectProduct,
        deleteProduct: deleteProduct
    };
    return productDetailDataFactory;

    //////////////////////////////////

    function selectProduct(_Product_Criteria_Model) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Product_GetProduct),
            //headers: {
            //    'accept': 'application/json; charset=utf-8',
            //    'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
            //    'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            //},
            data: _Product_Criteria_Model
        });
    };

    function deleteProduct(_ProductBindingModel) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Product_Delete),
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
//#region Controller for Detail product info.
app.controller('ProductDetailController', function ($scope, $timeout, productDetailDataFactory) {

    //#region Initial declaration       
    $scope.IsRecordFound = true;
    // Data to populate grid.
    $scope.item = {};
    $scope.itemLength = 0;
    // Criteria to search in database.
    $scope.Product_Criteria_Model = {
        // -- Pager --
        "BatchIndex": 1,
        "PagerShowIndexOneUpToX": 10,
        "RecordPerPage": 10,
        "RecordPerBatch": 100,

        // -- Sorting --
        "OrderByClause": "ProductName ASC",

        // -- Data --
        "SrNo": "",
        "ProductID": sessionStorage.getItem("ProductID"),
        "ProductName": "",
        "Description": "",
        "Price": "",
    };
    //#endregion

    //#region Retrieve
    $scope.InitialLoad = function () {
        productDetailDataFactory.selectProduct($scope.Product_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer(data);
            $scope.currentPage = 0;
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }
    $scope.InitialLoad();
    //#endregion

    //#region Delete
    $scope.Delete = function () {
        var result = confirm("Are you sure to delete?");
        if (!result) {
            return;
        }

        var List_Product_Criteria_Model = [];
        var _Product_Criteria_Model = angular.copy($scope.Product_Criteria_Model);
        List_Product_Criteria_Model.push(_Product_Criteria_Model);

        productDetailDataFactory.deleteProduct(List_Product_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.MessageNotifier();
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }
    //#endregion

    //#region Optimize data after Ajax request
    $scope.dataOptimizer = function (data) {
        console.log("JSON.stringify(data)", JSON.stringify(data));

        if (data[0].List_T.length <= 0) {
            $scope.IsRecordFound = false;
            return;
        }
        $scope.item = data[0].List_T[0];

        console.log("$scope.item", $scope.item);
        console.log("$scope.item.ProductName", $scope.item.ProductName);

        $scope.itemLength = data[0].List_T[0].TotalRecordCount;
    };
    $scope.MessageNotifier = function () {        
        $scope.item = {};
        alert("The record has been deleted.");
        $scope.ProductByClick();
    };
    //#endregion

});
//#endregion
//#endregion