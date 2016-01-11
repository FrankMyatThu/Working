//#region factory
app.factory('countryEditDataFactory', function ($http) {
    var countryEditDataFactory = {
        selectCountry: selectCountry,
        updateCountry: updateCountry
    };
    return countryEditDataFactory;

    //////////////////////////////////

    function selectCountry(_Country_Criteria_Model) {
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_GetCountryList),
            headers: {
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + sessionStorage.getItem("JWTToken"),
                'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            },
            data: _Country_Criteria_Model
        });
    };

    function updateCountry(_CountryBindingModel) {        
        return $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_UpdateCountry),
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
//#region Controller for Edit country info.
app.controller('CountryEditController', function ($scope, $timeout, countryEditDataFactory) {

    //#region Initial declaration       
    $scope.IsRecordFound = true;    
    // Data to populate grid.
    $scope.item = {};
    $scope.itemLength = 0;   
    // Criteria to search in database.
    $scope.Country_Criteria_Model = {
        // -- Pager --
        "BatchIndex": 1,
        "PagerShowIndexOneUpToX": 10,
        "RecordPerPage": 10,
        "RecordPerBatch": 100,

        // -- Sorting --
        "OrderByClause": "Name ASC",

        // -- Data --
        "SrNo": "",
        "Id": sessionStorage.getItem("CountryId"),
        "Name": "",
        "IsActive": "",
        "CreatedDate": "",
        "CreatedBy": "",
        "UpdatedDate": "",
        "UpdatedBy": "",
    };    
    //#endregion

    //#region Retrieve
    $scope.InitialLoad = function () {
        countryEditDataFactory.selectCountry($scope.Country_Criteria_Model)
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
        countryEditDataFactory.updateCountry($scope.item)
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
        console.log("$scope.item.Name", $scope.item.Name);

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