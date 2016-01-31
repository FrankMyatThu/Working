//#region factory
app.factory('orderCreateDataFactory', function ($http) {
    var orderCreateDataFactory = {
        createOrder: createOrder,
        selectProduct: selectProduct,
    };
    return orderCreateDataFactory;

    //////////////////////////////////

    function createOrder(_OrderBindingModel) {
        console.log("createOrder _OrderBindingModel", JSON.stringify(_OrderBindingModel));
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
        availableOptions: [],
        selectedOption: {}
    };
    $scope.childItem = [];
    $scope.currentPrice = 0;
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
        if ($scope.childItem.length <= 0)
        {
            alert("Please add item(s) before saving.");
            return;
        }

        $scope.item.List_OrderDetailBindingModel = [];
        angular.forEach($scope.childItem, function (item) {
            $scope.item.List_OrderDetailBindingModel.push({
                ProductID: item.ProductID,
                ProductName: item.ProductName,
                Quantity: item.Quantity,
                Total: item.Total,
                TotalGST: item.TotalGST,
            });
        });
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
        $scope.ddlProduct_Bind($scope.List_Product);
    };
    $scope.MessageNotifier = function () {
        $scope.success = "Created successfully.";
        $timeout(function () {
            $scope.success = "";
        }, 4000);
    };
    //#endregion

    //#region Helper functions
    $scope.ddlProduct_Bind = function (db_Product_List) {       
        angular.forEach(db_Product_List, function (item) {
            $scope.product_data.availableOptions.push({
                id: item.ProductID,
                name: item.ProductName
            });
        });
        $scope.product_data.selectedOption.id = db_Product_List[0].ProductID;
        $scope.product_data.selectedOption.name = db_Product_List[0].ProductName;
        $scope.currentPrice = db_Product_List[0].Price;
        console.log("Default price", $scope.currentPrice);
    }

    $scope.ddlProduct_Change = function (SelectedValue) {
        $scope.currentPrice = $scope.List_Product.filter(function (item) { return item.ProductID === SelectedValue.id; })[0].Price;        
        $scope.Quantity_OnChange();
    }

    $scope.Quantity_OnChange = function () {
        var Pattern = /^[0-9]*$/;
        if (!Pattern.test($scope.item.Quantity)) {
            $scope.item.Total = "";
            $scope.item.TotalGST = "";
            return;
        }

        $scope.item.Total = $scope.item.Quantity * $scope.currentPrice;
        $scope.item.TotalGST = ($scope.item.Total * 0.07) + $scope.item.Total;
    }

    $scope.AddChildItem = function () {
        $scope.childItem.push({
            TempOrderDetailId : $scope.childItem.length,
            ProductID: $scope.product_data.selectedOption.id,
            ProductName: $scope.product_data.selectedOption.name,
            Price: $scope.currentPrice,
            Quantity: $scope.item.Quantity,
            Total: $scope.item.Total,
            TotalGST: $scope.item.TotalGST
        });
        //$scope.ResetControl();
    }

    $scope.ResetControl = function () {
        $scope.item.Quantity = "";
        $scope.item.Total = "";
        $scope.item.TotalGST = "";
        $scope.form.$setPristine();        
    }

    $scope.RemoveRow = function (_TempOrderDetailId) {
        var index = $scope.childItem.map(function (x) { return x['TempOrderDetailId']; }).indexOf(_TempOrderDetailId)
        console.log("index", index);
        $scope.childItem.splice(index, 1);
    }
    //#endregion

});
//#endregion
//#endregion