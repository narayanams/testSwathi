$(function() {
    // HtmlHelpers Module
    // Call by using HtmlHelpers.getQueryStringValue("myname");
    var HtmlHelpers = function() {
        return {
            // Based on http://stackoverflow.com/questions/901115/get-query-string-values-in-javascript
            getQueryStringValue: function(name) {
                var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
                return match && decodeURIComponent(match[1].replace( '/+/g' , ' '));
            }
        };
    }();

    // StringHelpers Module
    // Call by using StringHelpers.padLeft("1", "000");
    var StringHelpers = function() {
        return {
            // Pad string using padMask.  string '1' with padMask '000' will produce '001'.
            padLeft: function(string, padMask) {
                string = '' + string;
                return (padMask.substr(0, (padMask.length - string.length)) + string);
            }
        };
    }();

    // SessionManager Module
    var SessionManager = function() {
        // NOTE:  I use @Session.Timeout here, which is Razor syntax, and I am pulling that value
        //        right from the ASP.NET MVC Session variable.  Dangerous!  Reckless!  Awesome-sauce!
        //        You can just hard-code your timeout here if you feel like it.  But I might cry.
        var sessionTimeoutSeconds,
            countdownSeconds = 120, // 2 Minutes
            secondsBeforePrompt = sessionTimeoutSeconds - countdownSeconds,
            displayCountdownIntervalId,
            promptToExtendSessionTimeoutId,
            originalTitle = document.title,
            count = countdownSeconds,
            extendSessionUrl = getBaseUrl() + '/Account/ExtendSession',
            expireSessionUrl = getBaseUrl() +'/Account/LogOff';

        var endSession = function() {
            location.href = expireSessionUrl;
        };

        var displayCountdown = function() {
            var countdown = function() {
                var cd = new Date(count * 1000),
                    minutes = cd.getUTCMinutes(),
                    seconds = cd.getUTCSeconds(),
                    minutesDisplay = minutes === 1 ? '1 minute ' : minutes === 0 ? '' : minutes + ' minutes ',
                    secondsDisplay = seconds === 1 ? '1 second' : seconds + ' seconds',
                    cdDisplay = minutesDisplay + secondsDisplay;

                document.title = 'Timeout in ' +
                    StringHelpers.padLeft(minutes, '00') + ':' +
                        StringHelpers.padLeft(seconds, '00');
                $('#sm-countdown').html(cdDisplay);
                if (count === 0) {
                    document.title = 'Session Expired';
                    endSession();
                }
                count--;
            };
            countdown();
            displayCountdownIntervalId = window.setInterval(countdown, 1000);
        };

        var promptToExtendSession = function() {
            bootbox.confirm({
                title: "Session Timeout Warning",
                message: "Your session is about to expire in <span id='sm-countdown' />.<br /><br />Do you want to extend the session?",
                closeButton: false,
                onEscape: false,
                buttons: {
                    cancel: {
                        label: '<i class="fa fa-sign-out"></i> Log Out'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> Continue'
                    }
                },
                callback: function (result) {
                    console.log('This was logged in the callback: ' + result);

                    if (result === true) {
                        refreshSession(true);
                        document.title = originalTitle;
                    }
                    else {
                        endSession(false);
                    }
                }
            });

            count = countdownSeconds;

            displayCountdown();
        };

        var refreshSession = function(extendDatabaseSession) {
            // Extend Client-side Session Timer
            window.clearInterval(displayCountdownIntervalId);

            var extendSession = (extendDatabaseSession != 'undefined') ? extendDatabaseSession : false;
            //debugger;
            //alert("extend");
            // Extend Database Session (if requested)
            if (extendSession) {
                $.post(extendSessionUrl, function (data) {
                    if (data != 'undefined') {
                        if ((data === true) || (data === "true")) {
                            window.clearTimeout(promptToExtendSessionTimeoutId);
                            startSessionManager();
                        }
                        else {
                            endSession();
                        }
                    }
                });
            }
        };

        var startSessionManager = function() {
            promptToExtendSessionTimeoutId =
                window.setTimeout(promptToExtendSession, secondsBeforePrompt * 1000);
        };

        // Public Functions
        return {
            start: function (timeLimit) {
                //debugger;
                //alert("start");
                // Set Timeout based on Input if provided, otherwise default to 2 minutes
                sessionTimeoutSeconds = (timeLimit != 'undefined') ? timeLimit * 60 : (2 * 60);
                secondsBeforePrompt = sessionTimeoutSeconds - countdownSeconds;

                startSessionManager();
            },

            extend: function(extendDatabaseSession) {
                refreshSession(extendDatabaseSession);
            }
        };
    }();

    // Initialize Session Manager
    SessionManager.start(sessionTimeout);

    // Extend Session (client-side timer only)
    $(document).ajaxStop(function () {
        SessionManager.extend(false);
    });
});

$(document).ready(function () {
    $("#payInvoiceButton").click(function () {
        var logoutToInvoiceURL = getBaseUrl() + '/Account/MoveToPayInvoice';
        $.get(logoutToInvoiceURL, function (response) {
            if (response.result == 'InvalidLogin') {
                //show invalid login
            }
            else if (response.result == 'Error') {
                //show error
            }
            else if (response.result == 'Redirect') {
                //redirecting to main page from here for the time being.
                window.location = response.url;
            }
        });
    });
});
