var timeout_time = 5;
var time_remaining = 0;

setInterval(function(){
	time_remaining = localStorage.getItem('timeout_time');
	console.log("time_remaining = ", time_remaining);
	if(time_remaining > 1 || time_remaining != null){
		localStorage.setItem('timeout_time', time_remaining - 1000);
	}
}, 1000);

window.addEventListener('storage', function(e){
  console.log(event.key, event.newValue);
  if(event.key === 'IsManualLoggedout') {
	window.localStorage.setItem('IsManualLoggedout', false);
	window.location = "/ORION/Home/logout";
  }  
});

function SetTimerForAppSession() {
    onInactive(logout, timeout_time * 1000);
}

function logout() {
    console.log('Logout');
	
	if(localStorage.getItem('timeout_time') <= 0)
	{
		window.location = "/ORION/Home/logout";
	}
	else
	{
		console.log("Logout.SetTimerForAppSession");
		SetTimerForAppSession();
	}
}

function onInactive(callback, millisecond) {
    var wait = setTimeout(callback, millisecond);
	
    document.onmousemove =
	document.onmousedown =
	document.onmouseup =
	document.onmousewheel =
	document.DOMMouseScroll =
	document.onkeydown =
	document.onkeyup =
	document.ontouchstart =
	document.ontouchmove =
	document.onscroll =
	document.focus = function () {
		
		// clear
		clearTimeout(wait);
		localStorage.removeItem('timeout_time');
		
		// reset
		wait = setTimeout(callback, millisecond);
		localStorage.setItem('timeout_time', millisecond)
	};
}

