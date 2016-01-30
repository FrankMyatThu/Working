//#region factory
app.factory('orderListingDataFactory', function ($http) {
    var orderListingDataFactory = {
        selectOrder: selectOrder,
        deleteOrder: deleteOrder
    };
    return orderListingDataFactory;

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

    function deleteOrder(_Order_Criteria_Model) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Order_Delete),
            //headers: {
            //    'accept': 'application/json; charset=utf-8',
            //    'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
            //    'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            //},
            data: _Order_Criteria_Model
        });
    };
});
//#endregion

//#region filter
app.filter('offset', function () {
    return function (input, start) {
        if (!input || !input.length) { return; }
        start = parseInt(start, 10);
        return input.slice(start);
    };
});
//#endregion

//#region controller
//#region Controller for searching popup dialog box.
app.controller('Modal_PopupSearch_Controller', function ($scope, $uibModalInstance) {

    $scope.Search = function () {
        $uibModalInstance.close();
    };

    $scope.Cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});
//#endregion
//#region Controller for listing country info.
app.controller('OrderListingController', function ($scope, $http, $window, $uibModal, $timeout, orderListingDataFactory) {

    //#region Initial declaration        
    $scope.IsRecordFound = true;
    $scope.currentPage = 0;
    // Data to populate grid.
    $scope.items = [];
    $scope.itemsLength = 0;
    // Data to pupulate pager.
    $scope.List_tbl_Pager_To_Client_ByBatchIndex = {};
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
        "OrderId": "",
        "Description": "",
        "OrderDate": ""
    };
    // Sorting criteria.
    $scope.sort = {
        sortingOrder: 'Description',
        reverse: false
    };
    // Checkbox Control
    $scope.checkboxControl = {
        "IsSelectedAll": false,
        "IsSelectedAllTotally": false,
        "SelectItemList": {},
        "currentDisplayedPageRecord": {},
        "currentDisplayedPageRecord_Total": 0
    };
    // Delete
    $scope.Delete = {
        "IsFinishDeleted": false,
        "DeletedRowCount": 0
    };
    //#endregion

    //#region Reset criteria array
    $scope.Reset_Order_Criteria_Model_Data = function () {
        $scope.Order_Criteria_Model.SrNo = "";
        $scope.Order_Criteria_Model.OrderId = "";
        $scope.Order_Criteria_Model.Description = "";        
        $scope.Order_Criteria_Model.OrderDate = "";
    }
    //#endregion

    //#region Grid's Checkbox 
    $scope.ResetCheckBoxControl = function (value) {
        $scope.checkboxControl.IsSelectedAll = value;
        $scope.checkboxControl.IsSelectedAllTotally = false;
        $scope.checkboxControl.SelectItemList = {};
        $scope.checkboxControl.currentDisplayedPageRecord = {};
    };
    $scope.selectAll = function () {
        $scope.ResetCheckBoxControl($scope.checkboxControl.IsSelectedAll);
        $scope.checkboxControl.currentDisplayedPageRecord = $scope.items.slice(0);
        $scope.checkboxControl.currentDisplayedPageRecord = $scope.checkboxControl.currentDisplayedPageRecord.splice($scope.currentPage * $scope.Order_Criteria_Model.RecordPerPage, $scope.Order_Criteria_Model.RecordPerPage);
        for (var i = 0; i < $scope.checkboxControl.currentDisplayedPageRecord.length; i++) {
            var item = $scope.checkboxControl.currentDisplayedPageRecord[i];
            $scope.checkboxControl.SelectItemList[item.OrderId] = $scope.checkboxControl.IsSelectedAll;
        }
        $scope.checkboxControl.currentDisplayedPageRecord_Total = $scope.checkboxControl.currentDisplayedPageRecord.length;
    };
    $scope.selectAllTotally = function (value) {
        $scope.checkboxControl.IsSelectedAllTotally = value;
    };
    $scope.voidSelection = function () {
        $scope.ResetCheckBoxControl(false);
    }
    $scope.checkboxStateChanged = function (OrderId) {
        if ($scope.checkboxControl.SelectItemList[OrderId]) {
            /// Checked  
            //console.log("Checked", OrderId);
        } else {
            /// Not checked
            $scope.checkboxControl.IsSelectedAll = false;
        }
    }
    //#endregion

    //#region Edit
    $scope.Edit = function (OrderId) {
        console.log("OrderId", OrderId);
        sessionStorage.setItem("OrderId", OrderId);
        location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Order_Edit);
    };
    //#endregion

    //#region Detail
    $scope.Detail = function (OrderId) {
        console.log("OrderId", OrderId);
        sessionStorage.setItem("OrderId", OrderId);
        location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Order_Detail);
    };
    //#endregion

    //#region Create New
    $scope.newPage = function () {
        location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Order_Create);
    }
    //#endregion

    //#region Delete    
    $scope.deletePage = function () {
        var result = confirm("Are you sure to delete?");
        if (!result) {
            return;
        }
        if ($scope.checkboxControl.IsSelectedAllTotally) {
            var List_Order_Criteria_Model = [];
            List_Order_Criteria_Model.push($scope.Order_Criteria_Model);
            orderListingDataFactory.deleteOrder(List_Order_Criteria_Model)
            .success(function (data, status, headers, config) {
                $scope.Reset_Order_Criteria_Model_Data();
                $scope.Delete.DeletedRowCount = $scope.itemsLength;
                $scope.firstPage();
                $scope.Delete.IsFinishDeleted = true;
                $timeout(function () { $scope.Delete.IsFinishDeleted = false; }, 4000);
            }).error(function (data, status, headers, config) {
                $scope.errorHandler(data, status, headers, config);
            });

        }
        else {
            var List_Order_Criteria_Model = [];
            $scope.Reset_Order_Criteria_Model_Data();
            angular.forEach($scope.checkboxControl.SelectItemList, function (value, key) {
                if (value) {
                    var _Order_Criteria_Model = angular.copy($scope.Order_Criteria_Model);
                    _Order_Criteria_Model.OrderId = key;
                    List_Order_Criteria_Model.push(_Order_Criteria_Model);
                }
            });
            if (List_Order_Criteria_Model.length <= 0) {
                alert("Please select item(s) which you want to delete.");
                $scope.firstPage();
                return;
            }
            orderListingDataFactory.deleteOrder(List_Order_Criteria_Model)
            .success(function (data, status, headers, config) {
                $scope.Delete.DeletedRowCount = List_Order_Criteria_Model.length;
                $scope.firstPage();
                $scope.Delete.IsFinishDeleted = true;
                $timeout(function () { $scope.Delete.IsFinishDeleted = false; }, 4000);
            }).error(function (data, status, headers, config) {
                $scope.errorHandler(data, status, headers, config);
            });
        }
    };
    //#endregion

    //#region Grid's Searching
    $scope.searchPage = function (size) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'ModalContent_PopupSearch.html',
            controller: 'Modal_PopupSearch_Controller',
            scope: $scope,
            size: size,
        });

        modalInstance.result.then(function () {
            $scope.Order_Criteria_Model.BatchIndex = 1;
            orderListingDataFactory.selectOrder($scope.Order_Criteria_Model)
            .success(function (data, status, headers, config) {
                $scope.dataOptimizer(data);
                $scope.currentPage = 0;
            }).error(function (data, status, headers, config) {
                $scope.errorHandler(data, status, headers, config);
            });
        }, function () {
            console.log('Modal dismissed at: ' + new Date());
        });

    };
    //#endregion

    //#region Pager
    $scope.firstPage = function () {
        $scope.Order_Criteria_Model.BatchIndex = 1;
        orderListingDataFactory.selectOrder($scope.Order_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer(data);
            $scope.currentPage = 0;
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    }

    $scope.lastPage = function () {
        $scope.Order_Criteria_Model.BatchIndex = Math.ceil($scope.itemsLength / $scope.Order_Criteria_Model.RecordPerBatch);
        orderListingDataFactory.selectOrder($scope.Order_Criteria_Model)
        .success(function (data, status, headers, config) {
            $scope.dataOptimizer(data);
            $scope.currentPage = $scope.List_tbl_Pager_To_Client_ByBatchIndex.length - 1;
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
        });
    };

    $scope.prevPage = function () {
        if ($scope.Order_Criteria_Model.BatchIndex > 1) {
            $scope.Order_Criteria_Model.BatchIndex--;
            orderListingDataFactory.selectOrder($scope.Order_Criteria_Model)
            .success(function (data, status, headers, config) {
                $scope.dataOptimizer(data);
                $scope.currentPage = $scope.Order_Criteria_Model.PagerShowIndexOneUpToX - 1;
            }).error(function (data, status, headers, config) {
                $scope.errorHandler(data, status, headers, config);
            });
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.Order_Criteria_Model.BatchIndex === 1 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.Order_Criteria_Model.BatchIndex < Math.ceil($scope.itemsLength / $scope.Order_Criteria_Model.RecordPerBatch)) {
            $scope.Order_Criteria_Model.BatchIndex++;
            orderListingDataFactory.selectOrder($scope.Order_Criteria_Model)
            .success(function (data, status, headers, config) {
                $scope.dataOptimizer(data);
                $scope.currentPage = 0;
            }).error(function (data, status, headers, config) {
                $scope.errorHandler(data, status, headers, config);
            });
        }
    };

    $scope.nextPageDisabled = function () {
        return $scope.Order_Criteria_Model.BatchIndex === Math.ceil($scope.itemsLength / $scope.Order_Criteria_Model.RecordPerBatch) ? "disabled" : "";
    };

    $scope.setPage = function (n) {
        $scope.currentPage = n;
        $scope.ResetCheckBoxControl(false);
    };
    //#endregion

    //#region Optimize data after Ajax request
    $scope.dataOptimizer = function (data) {
        $scope.ResetCheckBoxControl(false);
        if (data[0].List_T.length <= 0) {
            $scope.IsRecordFound = false;
            return;
        }
        $scope.items = data[0].List_T;
        $scope.itemsLength = data[0].List_T[0].TotalRecordCount;
        $scope.List_tbl_Pager_To_Client_ByBatchIndex = data[0].List_tbl_Pager_To_Client.filter(function (item) { return item.BatchIndex === $scope.Order_Criteria_Model.BatchIndex; });
    };
    //#endregion

});
//#endregion
//#endregion

//#region directive
app.directive("customSort", function () {
    return {
        restrict: 'A',
        transclude: true,
        scope: true,
        template:
          '<a href="#" ng-click="sort_by(order)" style="color: #555555;">' +
          '    <span ng-transclude></span>' +
          '    <i ng-class="selectedCls(order)"></i>' +
          '</a>',
        link: function (scope, elem, attrs) {
            scope.order = attrs.order;

            scope.sort_by = function (newSortingOrder) {
                var sort = scope.sort;
                if (sort.sortingOrder == newSortingOrder) {
                    sort.reverse = !sort.reverse;
                }

                sort.sortingOrder = newSortingOrder;
                scope.Order_Criteria_Model.OrderByClause = sort.sortingOrder + ((scope.sort.reverse) ? ' DESC' : ' ASC');
                scope.firstPage();
            };

            scope.selectedCls = function (column) {
                if (column == scope.sort.sortingOrder) {
                    return ('fa fa-chevron-' + ((scope.sort.reverse) ? 'down' : 'up'));
                }
                else {
                    return 'fa fa-sort'
                }
            };
        }
    }
});

app.directive("ddlRecordFilterCount", ['orderListingDataFactory', function (orderListingDataFactory) {
    return {
        restrict: 'A',
        transclude: true,
        scope: false,
        template:
            ' Filter By&nbsp;<span class="glyphicon glyphicon-filter"></span> ' +
            ' <select id="ddlRecordFilterCount" name="ddlRecordFilterCount"  ' +
            ' ng-options="option.name for option in data.availableOptions track by option.id" ng-model="data.selectedOption" ' +
            ' ng-change="ddlRecordFilterCount_Change(data.selectedOption)" ></select> ',
        link: function (scope, elem, attrs) {
            scope.data = {
                availableOptions: [
                    { id: '10', name: '10' },
                    { id: '20', name: '20' },
                    { id: '50', name: '50' }
                ],
                selectedOption: { id: '10', name: '10' }
            };
            scope.ddlRecordFilterCount_Change = function (SelectedValue) {
                scope.Order_Criteria_Model.BatchIndex = 1;
                scope.Order_Criteria_Model.RecordPerPage = SelectedValue.id;
                scope.Order_Criteria_Model.RecordPerBatch = scope.Order_Criteria_Model.RecordPerPage * scope.Order_Criteria_Model.PagerShowIndexOneUpToX;
                orderListingDataFactory.selectOrder(scope.Order_Criteria_Model)
                .success(function (data, status, headers, config) {
                    scope.dataOptimizer(data);
                    scope.currentPage = 0;
                }).error(function (data, status, headers, config) {
                    scope.errorHandler(data, status, headers, config);
                });
            };
        }
    }
}]);
//#endregion