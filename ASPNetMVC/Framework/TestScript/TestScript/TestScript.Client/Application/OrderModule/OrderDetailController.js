//#region factory
app.factory('orderDetailDataFactory', function ($http) {
    var orderDetailDataFactory = {
        selectOrder: selectOrder,
        deleteOrder: deleteOrder
    };
    return orderDetailDataFactory;

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

    function deleteOrder(_OrderBindingModel) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Order_Delete),
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
//#region Controller for Detail order info.
app.controller('OrderDetailController', function ($scope, $timeout, orderDetailDataFactory) {

    //#region Initial declaration       
    $scope.IsRecordFound = true;
    // Data to populate grid.
    $scope.item = {};
    $scope.childItem = {};
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
        orderDetailDataFactory.selectOrder($scope.Order_Criteria_Model)
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

        var List_Order_Criteria_Model = [];
        var _Order_Criteria_Model = angular.copy($scope.Order_Criteria_Model);
        List_Order_Criteria_Model.push(_Order_Criteria_Model);

        orderDetailDataFactory.deleteOrder(List_Order_Criteria_Model)
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
        $scope.childItem = data[0].List_T[0].List_OrderDetailBindingModel;

        console.log("$scope.item", $scope.item);
        console.log("$scope.item.Description", $scope.item.Description);

        $scope.itemLength = data[0].List_T[0].TotalRecordCount;
    };
    $scope.MessageNotifier = function () {
        $scope.item = {};
        alert("The record has been deleted.");
        $scope.OrderByClick();
    };
    //#endregion

});
//#endregion
//#endregion