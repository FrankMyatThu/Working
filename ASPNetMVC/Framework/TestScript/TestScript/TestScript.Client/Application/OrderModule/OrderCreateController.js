//#region factory
app.factory('orderCreateDataFactory', function ($http) {
    var orderCreateDataFactory = {
        createOrder: createOrder,
        selectProduct: selectProduct,
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

    function selectProduct(_Product_Criteria_Model) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Product_GetProductWithoutPager),
            //headers: {
            //    'accept': 'application/json; charset=utf-8',
            //    'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
            //    'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            //},
            data: _Product_Criteria_Model
        });
    };
});
//#endregion

//#region controller
//#region Controller for Create order info.
app.controller('OrderCreateController', function ($scope, $timeout, orderCreateDataFactory) {

    //#region Initial declaration
    $scope.List_Product = {};
    $scope.product_data = {
        availableOptions: [
            { id: '10', name: '10 N' },
            { id: '20', name: '20 N' },
            { id: '50', name: '50 N' }
        ],
        selectedOption: { id: '10', name: '10 N' }
    };
    $scope.Product_Criteria_Model = {        
        // -- Sorting --
        "OrderByClause": "ProductName ASC",

        // -- Data --
        "SrNo": "",
        "ProductID": "",
        "ProductName": "",
        "Description": "",
        "Price": ""
    };
    //#endregion

    //#region Create
    $scope.Create = function () {
        orderCreateDataFactory.createOrder($scope.item)
        .success(function (data, status, headers, config) {
            $scope.MessageNotifier();
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }
    //#endregion

    //#region Retrieve
    $scope.InitialLoad = function () {
        console.log("InitialLoad $scope.Product_Criteria_Model", JSON.stringify($scope.Product_Criteria_Model));
        orderCreateDataFactory.selectProduct($scope.Product_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer(data);            
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }
    $scope.InitialLoad();
    //#endregion

    //#region Optimize data after Ajax request    
    $scope.dataOptimizer = function (data) {
        //console.log("JSON.stringify(data)", JSON.stringify(data));

        if (data[0].List_T.length <= 0) {
            $scope.IsRecordFound = false;
            return;
        }
        $scope.List_Product = data[0].List_T;
        console.log("JSON.stringify($scope.List_Product)", JSON.stringify($scope.List_Product));
        
    };
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