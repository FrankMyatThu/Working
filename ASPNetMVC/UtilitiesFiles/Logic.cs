public class AccountController : BaseApiController
{
	[AllowAnnonymus]
	bool RegisterUser(UserRegistrationInfo _UserRegistrationInfo){
		/// Saving user info to db ...
	}

	[AllowAnnonymus]
	string GetAccessTokenLoggingIn(UserLoginInfo _UserLoginInfo){
		/// Logic Logic ...
		/// Return Access_token
		return "AccessToken";
	}

	[Customized_Authorize]
	string ChangePasswordByUserID(){
		/// Change Password ...
		/// Remove Old Access_token
		/// Return Updated Access_token
		return "AccessToken";
	}

	[Customized_Authorize]
	void AddClaimByUserID(){

	}
}

