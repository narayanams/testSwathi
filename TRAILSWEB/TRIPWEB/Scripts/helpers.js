;
//This helper files store common JQuery functionality

//serializeFormJSON is a common method to serialize form and convert back to JSON formated data
(function ($) {
    $.fn.serializeFormJSON = function () {

        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
})(jQuery);

//toggle display
function ToggleRandom(divId) {
    var element = $('#' + divId);
    var today = new Date();
    var modInt = today.getSeconds() % 3;

    switch (modInt) {
        case 0:
            $(element).toggle(600, "linear", function () { });
            break;
        case 1:
           $(element).fadeToggle(500,"linear", function () { });
            break;
        default:
            $(element).slideToggle(500, "linear", function () { });
    }

}

function stringReplace(thisString, search, replacement) {
    var target = thisString;
    return  target.split(search).join(replacement);
};

function ToggleCheckBox(divId, booleanValue) {
   
    var element = $('#' + divId + ':input:checkbox');
    var elValue = ($(element).val() === "True") ? true : false;

    if (Boolean(booleanValue)) {
        $(element).prop('checked', false);
        return false;
    } else {
        $(element).prop('checked', false);
        return true;
    }

}

// Query String Helper Object
var queryString =
{
    getParameterByName: function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
        var results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

var htmlUtility = new function () {
    this.decodeEntities = function (encodedString) {
        var div = document.createElement('div');
        div.innerHTML = encodedString;
        return div.textContent;
    }
}

// Define String.Format if it isn't implemented yet.
if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}


// Helper Function to Swap Remote Bootstrap Modal Dialogs
function swapRemoteDialog(dialogSelector, nextModalUrl) {
    // Hide Current Modal Dialog
    $(dialogSelector).removeClass('fade').modal('hide');

    // Load Modal Content
    $(dialogSelector).load(nextModalUrl);

    // Display Next Modal Dialog
    $(dialogSelector).addClass('fade').modal('show');
}

// Helper Function to Load Remote Modal Bootstrap Dialog
function showRemoteDialog(dialogSelector, sourceUrl) {
    // Load Modal Dialog Content
    $(dialogSelector).load(sourceUrl);

    // Display Modal Dialog
    $(dialogSelector).modal('show');
}

// Helper Function to Load Remote Modal Bootstrap Dialog
function showNavigationErrors(navigationError) {
    // Load Modal Dialog Content
    //alert(navigationError);

    if (typeof navigationError != "undefined") {
        var navigationData = JSON.parse(navigationError);

        if (navigationData.HasModal) {
            $.each(navigationData.ModalIds, function (index, value) {
                $('#' + value).modal('show');
            });

            //Display Popups
        }

        if (navigationData.HasError)
        {
            $.each(navigationData.Errors, function (index, value) {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    message: value
                },
                    {
                        type: 'danger',  z_index: 3000
                    });
            });
        }


    }


}

function isCapslock(e) {

    e = (e) ? e : window.event;

    var charCode = false;
    if (e.which) {
        charCode = e.which;
    } else if (e.keyCode) {
        charCode = e.keyCode;
    }

    var shifton = false;
    if (e.shiftKey) {
        shifton = e.shiftKey;
    } else if (e.modifiers) {
        shifton = !!(e.modifiers & 4);
    }

    if (charCode >= 97 && charCode <= 122 && shifton) {
        return true;
    }

    if (charCode >= 65 && charCode <= 90 && !shifton) {
        return true;
    }

    return false;

}

$(".navigateModules").click(function (event) {

    var link = this;

    if (window.location.pathname.toLowerCase().indexOf("/activate/index") >= 0 || window.location.pathname.toLowerCase().indexOf("activate/accountdetails") >= 0 || window.location.pathname.toLowerCase().indexOf("sales/payment") >= 0) {

        var $form = $('form');
        var initialChecksum = $form.data('checksum');
        var currentChecksum = $form.serialize().hashCode();
        var isDirty = initialChecksum != currentChecksum;

        if (!isDirty)
        {
            var getBaseUrl = "";
            if (typeof TrailsWebNS.config.baseUrl != "undefined")
            {
                getBaseUrl = TrailsWebNS.config.baseUrl; 
            }

            
            $.ajax({
                url: getBaseUrl + '/Home/IsThereACart',
                async: false,
                type: 'get',
                error: function ()
                {
                    window.onbeforeunload = null;
                    noNavigationMessage = true;
                },
                success: function (response) {
                    if (response === true || response === "true")
                    {
                        event.preventDefault();

                        noNavigationMessage = false;

                        bootbox.confirm("You still have items in your cart which will be lost if you leave this page. <br/> Do you wish to continue?",
                            function (result) {
                                if (result) { window.location = link.href; };
                            });
                    }
                    else
                    {
                        window.onbeforeunload = null;
                        noNavigationMessage = true;
                    }
                },

            });
            /*
            return isUnique;
            //Call Controller to check any session is there to wipe or not
            window.onbeforeunload = null;
            noNavigationMessage = true;*/
        }
        else {
            event.preventDefault();

            noNavigationMessage = false;

            bootbox.confirm(
                {
                    title: "Unsaved Changes",
                    message: "You have unsaved changes which will be lost if you leave this page. <br/> Do you wish to continue?",
                    className: 'customBootBox',
                    buttons: {
                        cancel: {
                            label: 'No', className: 'btn-default pull-right'
                        },
                        confirm: {
                            label: 'Yes',
                            className: 'btn-primary bootboxYes'
                        }
                    },
                    callback: function(result) {
                        if (result) { window.location = link.href; };
                    }
                });
        }

    }
    else
    {
        window.onbeforeunload = null;
        noNavigationMessage = true;

    }
});

window.onbeforeunload = function () {
    /*if (!noNavigationMessage
        && (window.location.pathname.toLowerCase().indexOf("/activate/index") >= 0 || window.location.pathname.toLowerCase().indexOf("activate/accountdetails") >= 0 || window.location.pathname.toLowerCase().indexOf("sales/payment") >= 0)) {
        return "Are you sure you want to leave this page? Any unsaved changes will be lost.";
    }*/
    
    /*bootbox.confirm("Are you sure you want to leave this page? Any unsaved changes will be lost.",
        function (result) {
            if (!result) { return true; };
        });*/
   /* if (window.location.pathname.toLowerCase().indexOf("/activate/index") >= 0 || window.location.pathname.toLowerCase().indexOf("activate/accountdetails") >= 0 || window.location.pathname.toLowerCase().indexOf("sales/payment") >= 0) {
        alert("going out");

        var $form = $('form');
        var initialChecksum = $form.data('checksum');
        var currentChecksum = $form.serialize().hashCode();
            var isDirty = initialChecksum != currentChecksum;

            if (isDirty)
                alert("Data Changed");
    }*/


   
}

String.prototype.hashCode = function () {
    var hash = 0;
    if (this.length == 0) return hash;
    for (i = 0; i < this.length; i++) {
        char = this.charCodeAt(i);
        hash = ((hash << 5) - hash) + char;
        hash = hash & hash;
    }
    return hash;
};
$(document).ready(function () {

    var $form = $('form');
    $form.data('checksum', $form.serialize().hashCode());

    noNavigationMessage = true;
}); 

var noNavigationMessage = true;

// Creating spinner see http://fgnass.github.com/spin.js for configuration wizard
var opts = {
    lines: 13 // The number of lines to draw
, length: 7 // The length of each line
, width: 4 // The line thickness
, radius: 10 // The radius of the inner circle
, scale: 1 // Scales overall size of the spinner
, corners: 1 // Corner roundness (0..1)
, color: '#fff' // #rgb or #rrggbb or array of colors
, opacity: 0.25 // Opacity of the lines
, rotate: 0 // The rotation offset
, direction: 1 // 1: clockwise, -1: counterclockwise
, speed: 1 // Rounds per second
, trail: 60 // Afterglow percentage
, fps: 20 // Frames per second when using setTimeout() as a fallback for CSS
, zIndex: 2e9 // The z-index (defaults to 2000000000)
, className: 'spinner' // The CSS class to assign to the spinner
, top: '50%' // Top position relative to parent
, left: '50%' // Left position relative to parent
, shadow: false // Whether to render a shadow
, hwaccel: false // Whether to use hardware acceleration
, position: 'absolute' // Element positioning
};

var spinner = new Spinner(opts);

// Spinner Helper Function: Display
function displaySpinner() {
    // Determine Center of Screen (Verticle Center of Window + Current Scroll Position - Spinner Height)
    var spinnerTop = Math.floor(window.innerHeight / 2) + $('body').scrollTop() - 75;

    $('<div id="progress-spinner" style="position: fixed; background: #333; color: #fff; width: 75px; height: 75px; top: ' + spinnerTop + 'px;left: 45%; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;z-index: 100; overflow: visible;"></div>').appendTo('body');
    spinner.spin($('#progress-spinner')[0]);
}

// Spinner Helper Function: Hide
function hideSpinner() {
    spinner.stop();
    $('#progress-spinner').remove();
}

// Helper Function to Convert Date to "JSON" Date Format
function convertToJSONDate(strDate) {
    var dt = new Date(strDate);
    var newDate = new Date(Date.UTC(dt.getFullYear(), dt.getMonth(), dt.getDate(), dt.getHours(), dt.getMinutes(), dt.getSeconds(), dt.getMilliseconds()));
    return newDate.getTime().toString();
}

// Helper Function to determine the Days Between two Dates
function numberOfDaysBetween(d1, d2) {
    var diff = Math.abs(d1.getTime() - d2.getTime());
    return diff / (1000 * 60 * 60 * 24);
};

// Helper Function to determine if a given Value is Numeric (as if the name of the method didn't give that away)
function isNumeric(value) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(value);
}

// Browser Information Object
var browserInfo =
{
    init: function () {
        this.name = this.searchString(this.dataBrowser) || "Other";
        this.version = this.searchVersion(navigator.userAgent) || this.searchVersion(navigator.appVersion) || "Unknown";
        this.supportsMultiFileUpload = 'multiple' in document.createElement('input');
        this.IECompatibilityView = new function () {
            var agentStr = navigator.userAgent;
            this.IsOn = false;

            if (agentStr.indexOf("MSIE 7.0") > -1) {
                this.IsOn = true;
            }
        };
    },

    searchString: function (data) {
        for (var i = 0 ; i < data.length ; i++) {
            var dataString = data[i].string;
            this.versionSearchString = data[i].subString;

            if (dataString.indexOf(data[i].subString) != -1) {
                return data[i].identity;
            }
        }
    },

    searchVersion: function (dataString) {
        var index = dataString.indexOf(this.versionSearchString);
        if (index == -1) return;
        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    },

    dataBrowser:
    [
        { string: navigator.userAgent, subString: "Chrome", identity: "Chrome" },
        { string: navigator.userAgent, subString: "MSIE", identity: "Explorer" },
        { string: navigator.userAgent, subString: "Firefox", identity: "Firefox" },
        { string: navigator.userAgent, subString: "Safari", identity: "Safari" },
        { string: navigator.userAgent, subString: "Opera", identity: "Opera" }
    ]

};
browserInfo.init();


$('body').on('keyup', '.fourDigitNumber', function (e) {
    if (this.value.length > this.maxLength)
        this.value = this.value.slice(0, this.maxLength);

});

$('body').on('keyup', '.SearchTransponderNumber', function (e) {
    if (this.value.length > this.maxLength)
        this.value = this.value.slice(0, this.maxLength);

});

$(".cfxPasswordPreference, .cfxCapslockPreference").on('focus keypress', function (e) {
    var $this = this;
    if (!this.suggestion) {
        if ($($this).hasClass("cfxPasswordPreference"))
            this.suggestion = $('<br><div class="passwordPreferences"><div class="capsLock" id="psdCapsLockMessage" style="display: none"> <i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Caps lock is on! Passwords are case sensitive </div> <h1>Your password must</h1> <ul><li> <div id="psdPreferenceCharsCountRule"  class="checkmark off"><span></span></div> Contain at least 8 characters </li> <li> <div id="psdPreferenceCharsRule" class="checkmark off"><span></span></div> Include at least 1 letter </li> </ul> </div>');
        else
            this.suggestion = $('<br> <div class="capsLock" id="psdCapsLockMessage" style="display: none" > <i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Caps lock is on! Passwords are case sensitive </div>');
    }

    this.suggestion.removeClass('hidden');
    var newAbsText = $(e.target).val() + (typeof (e.key) != "undefined" ? e.key : "");

    newAbsText.length < 8 ? $("#psdPreferenceCharsCountRule").removeClass("on").removeClass("off").addClass("off"): $("#psdPreferenceCharsCountRule").removeClass("on").removeClass("off").addClass("on");

    var re = /[a-zA-Z]/;
    re.test(newAbsText) ? $("#psdPreferenceCharsRule").removeClass("on").removeClass("off").addClass("on"): $("#psdPreferenceCharsRule").removeClass("on").removeClass("off").addClass("off");
    //psdPreferenceCharsCountRule
    //psdPreferenceCharsRule
    if ($($this).hasClass("parent1"))
        $($this).parent().append(this.suggestion);
    else
        $($this).parent().parent().append(this.suggestion);

});

$(".cfxCapslockCheck").keypress(function (e) {
    isCapslock(e) ? $("#psdCapsLockMessage").show() : $("#psdCapsLockMessage").hide();
});

$(".cfxPasswordPreference").blur(function (e) {
    if (this.suggestion) {
        this.suggestion.addClass('hidden');
    }
});

function makeCFXPreferences(e)
{
    var $this = $(e.target);
    if (!this.suggestion) {
        this.suggestion = $('<div class="passwordPreferences"> <div class="capsLock" id="psdPreferenceCapsLock" > Caps lock is on! Passwords are case sensitive </div> <div id="psdPreferenceCharsCountRule" class=""> Contains at least 8 characters </div> <div id="psdPreferenceCharsRule" class=""> Includes at least 1 letter </div> </div>');
    }

    this.suggestion.removeClass('hidden');

    if ($($this).hasClass("parent1"))
        $($this).parent().append(this.suggestion);
    else
        $($this).parent().parent().append(this.suggestion);
}
function hideBreadcrumb() {
	$('#bcFlowImage').css("visibility", 'hidden');
}

function showBreadcrumb() {
	$('#bcFlowImage').css("visibility", 'visible');
}

