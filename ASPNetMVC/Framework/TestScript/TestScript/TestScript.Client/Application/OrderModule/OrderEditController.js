//#region factory
app.factory('orderEditDataFactory', function ($http) {
    var orderEditDataFactory = {
        selectOrder: selectOrder,
        selectProduct: selectProduct,
        updateOrder: updateOrder
    };
    return orderEditDataFactory;

    //////////////////////////////////

    function selectOrder(_Order_Criteria_Model) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Order_GetOrderDetail),
            //headers: {
            //    'accept': 'application/json; charset=utf-8',
            //    'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
            //    'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            //},
            data: _Order_Criteria_Model
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

    function updateOrder(_OrderBindingModel) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Order_Update),
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
//#region Controller for Edit order info.
app.controller('OrderEditController', function ($scope, $timeout, orderEditDataFactory) {

    //#region Initial declaration    
    $scope.IsRecordFound = true;
    // Data to populate grid.
    $scope.item = {};
    $scope.itemLength = 0;
    // Criteria to search in database.
    $scope.Order_Criteria_Model = {
        // -- Pager --
        "BatchIndex": 1,
        "PagerShowIndexOneUpToX": 10,
        "RecordPerPage": 10,
        "RecordPerBatch": 100,

        // -- Sorting --
        "OrderByClause": "Description ASC",

        // -- Data --
        "SrNo": "",
        "OrderId": sessionStorage.getItem("OrderId"),        
        "Description": "",
        "OrderDate": "",
    };
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

    //#region Retrieve
    $scope.InitialLoad = function () {
        /// Get order by id
        orderEditDataFactory.selectOrder($scope.Order_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer_selectOrder(data);
            $scope.currentPage = 0;
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });

        /// Get product list for dropdownlist
        orderEditDataFactory.selectProduct($scope.Product_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer_selectProduct(data);
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }
    $scope.InitialLoad();
    //#endregion

    //#region Update
    $scope.Update = function () {
        if ($scope.childItem.length <= 0) {
            alert("Please add item(s) before updating.");
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
        orderEditDataFactory.updateOrder($scope.item)
        .success(function (data, status, headers, config) {
            $scope.MessageNotifier();
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }
    //#endregion

    //#region Optimize data after Ajax request
    $scope.dataOptimizer_selectOrder = function (data) {
        console.log("JSON.stringify(data)", JSON.stringify(data));

        if (data[0].List_T.length <= 0) {
            $scope.IsRecordFound = false;
            return;
        }
        $scope.item = data[0].List_T[0];
        $scope.item.OrderDate = new Date(data[0].List_T[0].OrderDate);
        $scope.childItem = data[0].List_T[0].List_OrderDetailBindingModel;        
        var i = 0;
        angular.forEach($scope.childItem, function (item) {
            console.log("item.OrderDetailID", item.OrderDetailID);
            item.OrderDetailID = i;
            i++;
        });

        $scope.itemLength = data[0].List_T[0].TotalRecordCount;
    };
    $scope.dataOptimizer_selectProduct = function (data) {        
        if (data[0].List_T.length <= 0) {
            $scope.IsRecordFound = false;
            return;
        }
        $scope.List_Product = data[0].List_T;        
        $scope.ddlProduct_Bind($scope.List_Product);
    };
    $scope.MessageNotifier = function () {
        $scope.success = "Updated successfully.";
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
        console.log("Before Add Item", JSON.stringify($scope.childItem));
        $scope.childItem.push({
            OrderDetailID: $scope.childItem.length,
            ProductID: $scope.product_data.selectedOption.id,
            ProductName: $scope.product_data.selectedOption.name,
            Price: $scope.currentPrice,
            Quantity: $scope.item.Quantity,
            Total: $scope.item.Total,
            TotalGST: $scope.item.TotalGST
        });
        //$scope.ResetControl();
        console.log("After Add Item", JSON.stringify($scope.childItem));
    }

    $scope.ResetControl = function () {
        $scope.item.Quantity = "";
        $scope.item.Total = "";
        $scope.item.TotalGST = "";
        $scope.form.$setPristine();
    }

    $scope.RemoveRow = function (_OrderDetailId) {
        alert(_OrderDetailId);
        var index = $scope.childItem.map(function (x) { return x['OrderDetailID']; }).indexOf(_OrderDetailId)
        console.log("index", index);
        $scope.childItem.splice(index, 1);
    }
    //#endregion

});
//#endregion
//#endregion