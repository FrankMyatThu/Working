function SetTimerForAppSession() {
    onInactive(logout, 5000);
}

function logout() {
    console.log('Logout');
    window.location = "/ORION/Home/logout";
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
		clearTimeout(wait);
		wait = setTimeout(callback, millisecond);
	};
}