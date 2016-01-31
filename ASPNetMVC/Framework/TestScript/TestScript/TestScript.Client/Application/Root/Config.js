/// Configuration Setting(s)
var ApplicationConfig = {

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// General Setting Area
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    MaximumAllowedIdelTime: 10 * 60 * 1000, // 10 Minutes  
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// URL Setting Area    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Client path(s)
    Client_Domain: "https://localhost:44310",
    Client_ServiceLoading : "/Content/img/Loading_Gears.gif",
    Client_Login: "/Account/Login",
    Client_Home: "/Home/Home",
    Client_Registration: "/Account/Register",
    Client_Product_Create: "/Product/Create",
    Client_Product_List: "/Product/List",
    Client_Product_Edit: "/Product/Edit",
    Client_Product_Detail: "/Product/Detail",
    Client_Order_Create: "/Order/Create",
    Client_Order_List: "/Order/List",
    Client_Order_Edit: "/Order/Edit",
    Client_Order_Detail: "/Order/Detail",
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// URL Setting Area    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Service path(s)
    Service_Domain: "https://localhost:44309",
    Service_Logout: "/api/accounts/UserLogout",
    Service_Login: "/api/accounts/UserLogin",    
    Service_CreateUser: "/api/accounts/CreateUser",
    Service_Product_Create: "/api/Product/Create",
    Service_Product_GetProduct: "/api/Product/GetProduct",
    Service_Product_GetProductWithoutPager: "/api/Product/GetProductWithoutPager",    
    Service_Product_Update: "/api/Product/Update",
    Service_Product_Delete: "/api/Product/Delete",
    Service_Order_Create: "/api/Order/Create",
    Service_Order_GetOrder: "/api/Order/GetOrder",
    Service_Order_GetOrderDetail: "/api/Order/GetOrderDetail",    
    Service_Order_Update: "/api/Order/Update",
    Service_Order_Delete: "/api/Order/Delete",
    
};