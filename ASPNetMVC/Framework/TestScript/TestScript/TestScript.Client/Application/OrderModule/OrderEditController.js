//#region factory
app.factory('orderEditDataFactory', function ($http) {
    var orderEditDataFactory = {
        selectOrder: selectOrder,
        updateOrder: updateOrder
    };
    return orderEditDataFactory;

    //////////////////////////////////

    function selectOrder(_Order_Criteria_Model) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Order_GetOrder),
            //headers: {
            //    'accept': 'application/json; charset=utf-8',
            //    'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
            //    'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            //},
            data: _Order_Criteria_Model
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
    //#endregion

    //#region Retrieve
    $scope.InitialLoad = function () {
        orderEditDataFactory.selectOrder($scope.Order_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer(data);
            $scope.currentPage = 0;
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }
    $scope.InitialLoad();
    //#endregion

    //#region Update
    $scope.Update = function () {
        orderEditDataFactory.updateOrder($scope.item)
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
        $scope.item.OrderDate = new Date(data[0].List_T[0].OrderDate);

        console.log("$scope.item", $scope.item);
        console.log("$scope.item.Description", $scope.item.Description);

        $scope.itemLength = data[0].List_T[0].TotalRecordCount;
    };
    $scope.MessageNotifier = function () {
        $scope.success = "Updated successfully.";
        $timeout(function () {
            $scope.success = "";
        }, 4000);
    };
    //#endregion

});
//#endregion
//#endregion