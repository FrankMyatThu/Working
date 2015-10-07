var app = angular.module('Application_Root', []);
app.run(function ($rootScope, $timeout, $document, $window) {
    console.log('starting run');

    // Timeout timer value
    var TimeOutTimerValue = 5000;    

    // Start a timeout
    var TimeOut_Thread = $timeout(function () { LogoutByTimer() }, TimeOutTimerValue);
    var bodyElement = angular.element($document);

    /// Keyboard Events
    bodyElement.bind('keydown', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('keyup', function (e) { TimeOut_Resetter(e) });

    /// Mouse Events    
    bodyElement.bind('click', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('mousemove', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('DOMMouseScroll', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('mousewheel', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('mousedown', function (e) { TimeOut_Resetter(e) });

    /// Touch Events
    bodyElement.bind('touchstart', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('touchmove', function (e) { TimeOut_Resetter(e) });

    /// Common Events
    bodyElement.bind('scroll', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('focus', function (e) { TimeOut_Resetter(e) });

    function LogoutByTimer() {
        console.log('Logout');

        ///////////////////////////////////////////////////
        /// redirect to another page(eg. Login.html) here
        ///////////////////////////////////////////////////
        $window.location.href = '../Account/Login';
    }

    function TimeOut_Resetter(e) {
        console.log('' + e);

        /// Stop the pending timeout
        $timeout.cancel(TimeOut_Thread);

        /// Reset the timeout
        TimeOut_Thread = $timeout(function () { LogoutByTimer() }, TimeOutTimerValue);
    }

    function RemoveCurrentUser()
    {
        
    }
})