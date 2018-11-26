"use strict";
var _debug = true;
var currentPaymentHistorySearch = null;
var monthlyStatementParams = null;
var parkingTransactionsParams = null;
var tollTransactionsParams = null;
var financialTransactionsParams = null;


var months = [
    { name: 'January', value: 1 },
    { name: 'February', value: 2 },
    { name: 'March', value: 3 },
    { name: 'April', value: 4 },
    { name: 'May', value: 5 },
    { name: 'June', value: 6 },
    { name: 'July', value: 7 },
    { name: 'August', value: 8 },
    { name: 'September', value: 9 },
    { name: 'October', value: 10 },
    { name: 'November', value: 11 },
    { name: 'December', value: 12 }]

var paymentHistoryColumns =
    [
        {
            "name": "PostingDate", "title": "Date", "type": "date",
            "formatter": function (val) { return new Date(parseInt(val.substring(6))).toLocaleDateString(); }
        },
        {
            "name": "PaymentAmount", "title": "Amount",
            "style": "text-align:right",
            "formatter": function (val) { return '$' + parseFloat(val).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString(); }
        },
        { "name": "PaymentCardNumber", "title": "Card Number" }
    ];

var tollTransactionsColumns =
    [
        {
            "name": "TransactionDate",
            "title": "Date",
            "formatter": function (val) {
                var dt = new Date(parseInt(val.substring(6)));
                return (dt.toLocaleDateString() + " <span class='cfxTollTime'>" + dt.toLocaleTimeString() + "</span>")
            },
        },
        {
            "name": "PostingDate",
            "title": "Posting Date",
            "formatter": function (val) {
                if (typeof val == "undefined" || val == null) {
                    return '';
                }
                else {
                    var dt = new Date(parseInt(val.substring(6)));
                    return (dt.toLocaleDateString() + " " + dt.toLocaleTimeString());
                }
            },
            "breakpoints": "all"
        },
        {
            "name": "LicensePlate",
            "title": "License Plate",
            "breakpoints": "xs sm"
        },
        {
            "name": "LicensePlateState",
            "title": "Plate State",
            "breakpoints": "all"
        },
        {
            "name": "Make",
            "title": "Make",
            "breakpoints": "xs sm"
        },
        {
            "name": "Model",
            "title": "Model",
            "breakpoints": "xs sm"
        },
        {
            "name": "Year",
            "title": "Year",
            "breakpoints": "all"
        },
        {
            "name": "TransponderNumber",
            "title": "Transponder",
            "breakpoints": "all"
        },
        {
            "name": "Location",
            "title": "Location",
            "style": "max-width:50px",
        },
        {
            "name": "Lane",
            "title": "Lane", "style": "text-align:right",
            "breakpoints": "xs sm"
        },
        {
            "name": "Amount",
            "title": "Amount",
            "style": "text-align:right",
            "formatter": function (val) { return '$' + parseFloat(val).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString(); },
        },
        {
            "name": "TransactionType",
            "title": "Type",
            "breakpoints": "all"
        }


    ];

var financialTransactionsColumns =
    [
        {
            "name": "TransactionDate",
            "title": "Date",
            "formatter": function (val) { return new Date(parseInt(val.substring(6))).toLocaleDateString(); }
        },
        {
            "name": "Location",
            "title": "Location",
            "visible": "lg md",
            "breakpoints": "xs sm"
        },
        {
            "name": "TransactionType",
            "title": "Type"
        },
        {
            "name": "Amount",
            "title": "Amount",
            "style": "text-align:right",
            "formatter": function (val) { return '$' + parseFloat(val).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString(); }
        }
    ];

// plate, plate state, plate type, make, moel, year, color, transponder number, transponder type, status
var parkingTransactionsColumns =
    [
        {
            "name": "TransactionDate",
            "title": "Transaction Date",
            "formatter": function (val) { return new Date(parseInt(val.substring(6))).toLocaleDateString(); },
            "breakpoints": "md"
        },
        {
            "name": "PostingDate",
            "title": "Posting Date",
            "formatter": function (val) { return new Date(parseInt(val.substring(6))).toLocaleDateString(); },
            "breakpoints": "all"
        },
        {
            "name": "LicensePlate",
            "title": "License Plate",
            "breakpoints": "md"
        },
        {
            "name": "LicensePlateState",
            "title": "Plate State",
            "breakpoints": "all"
        },
        {
            "name": "Make",
            "title": "Make",
            "breakpoints": "md"
        },
        {
            "name": "Model",
            "title": "Model",
            "breakpoints": "md"
        },
        {
            "name": "Year",
            "title": "Year",
            "breakpoints": "all"
        },
        {
            "name": "TransponderNumber",
            "title": "Transponder",
            "breakpoints": "all"
        },
        {
            "name": "Location",
            "title": "Location",
            "breakpoints": "md"
        },
        {
            "name": "Lane",
            "title": "Lane",
            "breakpoints": "all", "style": "text-align:right",
        },
        {
            "name": "Amount",
            "title": "Amount", "style": "text-align:right",
            "formatter": function (val) { return '$' + parseFloat(val).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString(); },
            "breakpoints": "md"
        },
        {
            "name": "TransactionType",
            "title": "Type",
            "breakpoints": "all"
        }

    ];

var PendingTableSnapShot = []

var pendingTransactionsColumns =
[
    {
        "name": "TransactionIDValue",
        "title": "Dispute",
        "style": "max-width:50px",
        "formatter": function (val) {
            var splitStr;
            var ckval = "";
            splitStr = val.split("|");

            if (splitStr.length >= 2) {
                /*
                                if (typeof ($('#selectAllPendingTransactions').prop('checked')) == "undefined")
                                    ckval = "";
                
                                if (ckval != "" && $('#selectAllPendingTransactions').prop('checked')) {
                                    ckval = "";
                                }
                                else {
                                    ckval = splitStr[1].toLowerCase() == "1" ?  ckval : splitStr[1].toLowerCase() == "2" ? "checked" : ;
                                }
                */

                ckval = splitStr[1].toLowerCase() == "1" ? "checked" : "";
                return "<input class='pendingTransactionChk'" + " type='checkbox' " + ckval + " value='" + splitStr[0] + "' " + " data_amount='" + splitStr[2] + "'  />";

            }
            else {
                return "<input disabled type='checkbox' />";
            }
        }
    },
    {
        "name": "TransactionDate",
        "title": "Transaction DateTime",
        "style": "max-width:150px",
        "formatter": function (val) {
            var dt = new Date(parseInt(val.substring(6)));
            return (dt.toLocaleDateString() + " " + dt.toLocaleTimeString());
        }
    },
     {
         "name": "TransactionDateElapsed",
         "title": "",
         "classes": "footableHideColumn",
     },
    {
        "name": "TransponderNumber",
        "title": "Transponder #",
        "style": "max-width:80px",
    },
    {
        "name": "LicensePlate",
        "title": "License Plate",
        "breakpoints": "xs sm",
        "style": "max-width:70px",
    },
	{
	    "name": "Location",
	    "title": "Location",
	    "breakpoints": "xs sm"
	},
    {
        "name": "Lane",
        "title": "Lane", 
        "breakpoints": "xs sm",
        "style": "max-width:50px,text-align:right",
        "sortable": false
    },
    {
        "name": "Amount",
        "title": "Amount",
        "style": "text-align:right",
        "formatter": function (val) { return '$' + parseFloat(val).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString(); },
    }

];

var manageController = new function () {

    /* BEGIN CRUD operations */

    this.deleteTransponder = function (event) {
        var manageData = {
            'UpdateTransponder': {
                'UpdateAction': 8,
                'IssuingAuthority': $('#IssuingAuthority').val(),
                'StateCode': $('#StateCode').val(),
                'TransponderNumber': $('#workingTagId').val(),
                //'TransponderNumber': $('#deleteConfirmationId').text()
                'LicensePlateNumber': $("#LicensePlateNumber").val(),
                'LicenseStateSelected': $("#LicenseStateSelected").val(),
                'LicensePlateTypeId': $("#LicensePlateType option:selected").val(),
                'Make': $("#VehicleMake").val(),
                'Model': $("#VehicleModel").val(),
                'Year': $("#VehicleYear").val(),
                'Color': $("#VehicleColor").val(),
                'Class': $("#VehicleClass").val()
            }
        }

        var manageUrl = getBaseUrl() + '/Manage/Post';
        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };
    this.activateTransponder = function (event) {
        var manageData = {
            'UpdateTransponder': {
                'UpdateAction': 16,
                'IssuingAuthority': $('#IssuingAuthority').val(),
                'StateCode': $('#StateCode').val(),
                'TransponderNumber': $('#activateConfirmationId').text()
            }
        }

        var manageUrl = getBaseUrl() + '/Manage/Post';
        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };
    this.deactivateTransponder = function (event) {
        var manageData = {
            'UpdateTransponder': {
                'UpdateAction': 32,
                'TransponderNumber': $('#deactivateConfirmationId').text(),
                'IssuingAuthority': $('#IssuingAuthority').val(),
                'StateCode': $('#StateCode').val()
            }
        }

        var manageUrl = getBaseUrl() + '/Manage/Post';
        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };
    this.updateTransponder = function (event) {
        //event.preventDefault();
        var manageData = {
            'UpdateTransponder': {
                'UpdateAction': $('#TransponderStatus').is(':checked') == true ? 32 : 16, //depending upon the checkbox pass the value for enabling/disabling the transponder
                'TransponderNumber': $('#workingTagId').val(),
                'LicensePlateNumber': $("#LicensePlateNumber").val(),
                'LicenseStateSelected': $("#LicenseStateSelected").val(),
                //'LicensePlateType': $("#LicensePlateType option:selected").text(),
                'LicensePlateTypeId': $("#LicensePlateType option:selected").val(),
                'IssuingAuthority': $('#IssuingAuthority').val(),
                'StateCode': $('#StateCode').val(),
                'Make': $("#VehicleMake").val(),
                'Model': (($("#VehicleModel").val() == "Other" || $("#VehicleModel").val() == "OTHER") && $("#otherSpecifyModel").val() != "") ? $("#otherSpecifyModel").val() : $("#VehicleModel").val(),
                'Year': $("#VehicleYear").val(),
                'Color': $("#VehicleColor").val(),
                'Class': $("#VehicleClass").val()
            }
        }

        var manageUrl = getBaseUrl() + '/Manage/Post';

        // Display Spinner
        //$('#loginButton span').removeClass('fa-sign-in');
        //$('#loginButton span').addClass('fa-spinner fa-spin animated');
        //$('#loginButton, #cancelLoginButton').attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                // Hide Spinner
                //$('#loginButton span').removeClass('fa-spinner fa-spin animated');
                //$('#loginButton span').addClass('fa-sign-in');
                //$('#loginButton, #cancelLoginButton').removeAttr('disabled');

                //$('#modalDialog').removeClass('fade');
                //$('#modalDialog').velocity('callout.shake');

                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };

    this.saveManagePreferences = function (event) {
        //event.preventDefault();
        var manageData = {
            'AccountPreferences': {
                'OptedForLowBalanceField': ($('#OptedForLowBalanceField').attr("checked") == "checked"),
                'OptedForOutOfFundField': ($('#OptedForOutOfFundField').attr("checked") == "checked"),
                'OptedForMonthlyStatementField': ($('#OptedForMonthlyStatementField').attr("checked") == "checked"),
                'OptedForEmailParkingReceiptField': ($('#OptedForEmailParkingReceiptField').attr("checked") == "checked"),
                'MonthlyEmailStatementType': ($('#MonthlyEmailStatementType').val()),
                'EmailStatementIndicatorListSelected': $('#AccountPreferences_EmailStatementIndicatorListSelected').val()

            }
        }

        var manageUrl = getBaseUrl() + '/Manage/Post';

        // Display Spinner
        //$('#loginButton span').removeClass('fa-sign-in');
        //$('#loginButton span').addClass('fa-spinner fa-spin animated');
        //$('#loginButton, #cancelLoginButton').attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                // Hide Spinner
                //$('#loginButton span').removeClass('fa-spinner fa-spin animated');
                //$('#loginButton span').addClass('fa-sign-in');
                //$('#loginButton, #cancelLoginButton').removeAttr('disabled');

                //$('#modalDialog').removeClass('fade');
                //$('#modalDialog').velocity('callout.shake');

                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };

    this.saveManageSecurity = function (event) {
        //event.preventDefault();

        var manageData = {
            'SecurityInfo': {
                'Password': TrailsSecurity.Encrypt($('#oldPass1').val()) + '',
                'NewPassword': TrailsSecurity.Encrypt($('#newPass1').val()) + '',
                'Pin': $('#newPin').val(),
                'SecretAnswer': TrailsSecurity.Encrypt($('#SecretAnswer').val()) + '',
                'SecretQuestion': $('#SecurityQuestion').val(),
                'SecretQuestionId': $('#SecurityQuestion').val(),
                'UserName': TrailsSecurity.Encrypt($('#newUsername1').val()) + '',
                'Email': TrailsSecurity.Encrypt($('#securityEmail1').val()) + '',
                'confirmEmail': TrailsSecurity.Encrypt($('#securityEmail2').val()) + '',
            }
        }

        var manageUrl = getBaseUrl() + '/Manage/Post';

        // Display Spinner
        //$('#loginButton span').removeClass('fa-sign-in');
        //$('#loginButton span').addClass('fa-spinner fa-spin animated');
        //$('#loginButton, #cancelLoginButton').attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                // Hide Spinner
                //$('#loginButton span').removeClass('fa-spinner fa-spin animated');
                //$('#loginButton span').addClass('fa-sign-in');
                //$('#loginButton, #cancelLoginButton').removeAttr('disabled');

                //$('#modalDialog').removeClass('fade');
                //$('#modalDialog').velocity('callout.shake');

                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };

    this.saveManageContact = function (event) {
        //event.preventDefault();

        var manageData = {
            'ContactInfo': {
                'AddressLine1': $('#contactAddressLine1').val(),
                'AddressLine2': $('#contactAddressLine2').val(),
                'City': $('#contactCity').val(),
                'State': $('#contactState').val(),
                'ZipCode': $('#contactZip').val(),
                'ZipCodeFour': $('#contactZipFour').val(),
                'DayPhone': $('#contactDayPhone').val(),
                'DayPhoneExt': $('#contactDayPhoneExt').val(),
                'EveningPhone': $('#contactEveningPhone').val(),
                'Email': $('#contactEmail').val()
            }

        }



        var manageUrl = getBaseUrl() + '/Manage/Post';

        // Display Spinner
        //$('#loginButton span').removeClass('fa-sign-in');
        //$('#loginButton span').addClass('fa-spinner fa-spin animated');
        //$('#loginButton, #cancelLoginButton').attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                // Hide Spinner
                //$('#loginButton span').removeClass('fa-spinner fa-spin animated');
                //$('#loginButton span').addClass('fa-sign-in');
                //$('#loginButton, #cancelLoginButton').removeAttr('disabled');

                //$('#modalDialog').removeClass('fade');
                //$('#modalDialog').velocity('callout.shake');

                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };

    this.deleteCard = function (event) {
        //event.preventDefault();

        var manageData = {
            'PaymentInfo': {
                'DeleteCard': true
            }

        }

        var manageUrl = getBaseUrl() + '/Manage/Post';

        // Display Spinner
        //$('#loginButton span').removeClass('fa-sign-in');
        //$('#loginButton span').addClass('fa-spinner fa-spin animated');
        //$('#loginButton, #cancelLoginButton').attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                // Hide Spinner
                //$('#loginButton span').removeClass('fa-spinner fa-spin animated');
                //$('#loginButton span').addClass('fa-sign-in');
                //$('#loginButton, #cancelLoginButton').removeAttr('disabled');

                //$('#modalDialog').removeClass('fade');
                //$('#modalDialog').velocity('callout.shake');

                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };

    this.savePaymentDetails = function (event) {
        //event.preventDefault();

        var expireDate = $('#ccMonth').val() + "/" + $('#ccYear').val();

        var manageData = {
            'PaymentInfo': {
                'ExpirationDate': expireDate,
                'CreditCardNumber': TrailsSecurity.Encrypt($('#changeCardNumber').val()) + '',
                'CreditCardType': $('.CreditCardType.static').val(),
                'ExpirationMonth': $('#ccMonth').val(),
                'ExpirationYear': $('#ccYear').val(),
                'AutoBillIndicator': ($('#paymentAutoBill').attr("checked") == "checked"),
                'LowBalanceAmount': $('#paymentLowAmount').val(),
                'ReplenishAmount': $('#paymentRepAmount').val(),
                'DeleteCard': false
            }

        }

        var manageUrl = getBaseUrl() + '/Manage/Post';

        // Display Spinner
        //$('#loginButton span').removeClass('fa-sign-in');
        //$('#loginButton span').addClass('fa-spinner fa-spin animated');
        //$('#loginButton, #cancelLoginButton').attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            if (response.success) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'success',
                    delay: 5000,
                    z_index: 3000
                });
                setTimeout(window.location.reload(true), 5000);
            }
            else {
                // Hide Spinner
                //$('#loginButton span').removeClass('fa-spinner fa-spin animated');
                //$('#loginButton span').addClass('fa-sign-in');
                //$('#loginButton, #cancelLoginButton').removeAttr('disabled');

                //$('#modalDialog').removeClass('fade');
                //$('#modalDialog').velocity('callout.shake');

                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: response.message
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        });

        return true;
    };


    //*************************************



    /* END CRUD Operations */


    /* REPORTS FUNCTIONS */
    this.getFinancialTransactions = function () {

        var manageUrl = getBaseUrl() + '/Manage/GetFinancialTransactions';

        var daysSelected = {
            'start': $('#financialTransactionsStartDate').val(),
            'end': $('#financialTransactionsEndDate').val(),
        };

        // Cache most recent search params.
        financialTransactionsParams = daysSelected;

        $.ajax({
            type: 'GET',
            url: manageUrl,
            data: daysSelected,
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {

            if (typeof (response) != "undefined") {
                if (typeof (response.Message) != "undefined") {
                    $.notify({
                        // Options
                        icon: 'fa fa-exclamation-triangle',
                        message: response.Message
                    },
                    {
                        // Settings
                        type: 'danger',
                        delay: 5000,
                        z_index: 3000
                    });
                }
                else {
                    $(".tableFinancialTransactions").footable({
                        "columns": financialTransactionsColumns,
                        "rows": response.FinancialTransactionHistory
                    });

                    if (response.FinancialTransactionHistory.length > 0) {
                        document.getElementById("financialTransactionsExport").style.display = "block";
                    }
                }
            }
            
        });

        return true;
    };

    this.getParkingTransactions = function () {

        var manageUrl = getBaseUrl() + '/Manage/GetParkingTransactions';


        // Get selected transponders.
        var transponders = "";
        var ft = FooTable.get("#parkingTransactionsVehicleSelection");
        var checked = []
        for (var i = 0, l = ft.rows.all.length, row; i < l; i++) {
            // the FooTable.Row object
            row = ft.rows.all[i];
            // but we want to work with DOM elements so to get access to the row use
            checked.push(row.$el.find("input[type=checkbox]:checked").map(function () { return this.value }).get())
        }

        checked = checked.filter(function (n) { return n[0] != undefined });

        checked.forEach(function (data) {
            transponders += (data) + ",";
        });

        var data = {
            'start': $('#parkingTransactionsStartDate').val(),
            'end': $('#parkingTransactionsEndDate').val(),
            'transponderNumbers': transponders
        };


        // Cache most recent search params.
        parkingTransactionsParams = data;

        // Display Spinner
        $('#getParkingApplyButton span').addClass('fa-spinner fa-spin animated');
        $('#getParkingApplyButton').addClass('disabled');

        $.ajax({
            type: 'GET',
            url: manageUrl,
            data: data,
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            // Remove Spinner
            $('#getParkingApplyButton span').removeClass('fa-spinner fa-spin animated');
            $('#getParkingApplyButton').removeClass('disabled');

            if (typeof (response) != "undefined") {
                if (typeof (response.Message) != "undefined") {
                    $.notify({
                        // Options
                        icon: 'fa fa-exclamation-triangle',
                        message: response.Message
                    },
                    {
                        // Settings
                        type: 'danger',
                        delay: 5000,
                        z_index: 3000
                    });
                }
                else {
                    $(".tableParkingTransactions").footable({
                        "columns": tollTransactionsColumns,
                        "rows": response.TransactionHistory
                    });

                    if (response.TransactionHistory.length > 0) {
                        document.getElementById("parkingTransactionsExport").style.display = "block";
                    }
                }
            }


        });

        return true;
    };

    this.getTollTransactions = function () {

        var manageUrl = getBaseUrl() + '/Manage/GetTollTransactions';

        // Get selected transponders.
        var transponders = "";

        var ft = FooTable.get("#vehicleTransponderGrid");
        var checked = []
        for (var i = 0, l = ft.rows.all.length, row; i < l; i++) {
            // the FooTable.Row object
            row = ft.rows.all[i];
            // but we want to work with DOM elements so to get access to the row use
            checked.push(row.$el.find("input[type=checkbox]:checked").map(function () { return this.value }).get())
        }

        checked = checked.filter(function (n) { return n[0] != undefined });

        checked.forEach(function (data) {
            transponders += (data) + ",";
        });

        // Get selected dates.
        if ($('#tollTransactionsStartDate').val() == "" || $('#tollTransactionsEndDate').val() == "") {
            document.getElementById("tollTransactionsErrorMessage").style.display = "block";
            document.getElementById("tollTransactionsErrorMessage").innerHTML = "Please select Start and End dates.";
        }
        else if (transponders == "") {
            document.getElementById("tollTransactionsErrorMessage").style.display = "block";
            document.getElementById("tollTransactionsErrorMessage").innerHTML = "Please select at least one transponder.";
        }
        else {
            document.getElementById("tollTransactionsErrorMessage").style.display = "none";
        }


        var data = {
            'start': $('#tollTransactionsStartDate').val(),
            'end': $('#tollTransactionsEndDate').val(),
            'transponderNumbers': transponders
        };

        // Cache most recent search params.
        tollTransactionsParams = data;

        // Display Spinner
        $('#getTollTransactionsButton span').addClass('fa-spinner fa-spin animated');
        $('#getTollTransactionsButton').addClass('disabled');

        $.ajax({
            type: 'GET',
            url: manageUrl,
            data: data,
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).then(function (response) {
            // Remove Spinner
            $('#getTollTransactionsButton span').removeClass('fa-spinner fa-spin animated');
            $('#getTollTransactionsButton').removeClass('disabled');

            if (typeof (response) != "undefined") {
                if (typeof (response.Message) != "undefined") {
                    $.notify({
                        // Options
                        icon: 'fa fa-exclamation-triangle',
                        message: response.Message
                    },
                    {
                        // Settings
                        type: 'danger',
                        delay: 5000,
                        z_index: 3000
                    });
                }
                else {
                    $(".tollTransactionsTable").footable({
                        "columns": tollTransactionsColumns,
                        "rows": response.TransactionHistory
                    });

                    if (response.TransactionHistory.length > 0) {
                        document.getElementById("tollTransactionsExport").style.display = "block";
                    }
                }
            }
        });
        return true;
    };

    this.getPaymentHistory = function () {

        var manageUrl = getBaseUrl() + '/Manage/GetPaymentHistory';

        var data = {
            'start': $('#paymentHistoryStartDate').val(),
            'end': $('#paymentHistoryEndDate').val()
        };

        $.ajax({
            type: 'GET',
            url: manageUrl,
            data: data,
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).catch(function (jqXHR, status, error) {

            console.error("Error occurred: " + error);

        }).then(function (response) {

            if (response.PaymentHistory.length > 0) {
                $(".table2").footable({
                    "columns": paymentHistoryColumns,
                    "rows": response.PaymentHistory
                });
            } else {

                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: "No Payments Found. Please review selected date range and try again."
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }


        });
        return true;
    };

    /* END REPORT FUNCTIONS */

    /* REPORT EXPORT FUNCTIONS */
    this.exportParkingTransactions = function () {

        var format = document.getElementById("parkingTransactionsExportType").value;
        var manageUrl = getBaseUrl() + '/Manage/GetParkingTransactionsReport?start=' + parkingTransactionsParams.start + '&end=' + parkingTransactionsParams.end + '&transponderNumbers=' + parkingTransactionsParams.transponderNumbers + '&format=' + format;
        console.log("Parking transactions: " + manageUrl);
        window.location.href = manageUrl;

    };

    this.exportFinancialTransactions = function () {

        var format = document.getElementById("financialTransactionsExportType").value;
        var manageUrl = getBaseUrl() + '/Manage/GetFinancialTransactionsReport?start=' + financialTransactionsParams.start + '&end=' + financialTransactionsParams.end + '&format=' + format;
        console.log("Financial transactions: " + manageUrl);
        window.location.href = manageUrl;
    };

    this.exportTollTransactions = function () {

        var format = document.getElementById("tollTransactionsExportType").value;
        var manageUrl = getBaseUrl() + '/Manage/GetTollTransactionsReport?start=' + tollTransactionsParams.start + '&end=' + tollTransactionsParams.end + '&transponderNumbers=' + tollTransactionsParams.transponderNumbers + '&format=' + format;
        console.log("Toll transactions: " + manageUrl);
        window.location.href = manageUrl;

    };

    this.exportPendingTollTransactions = function (event, format) {

        if (format == null)
            var format = $("#pendingTransactionsExportType").val();

        var manageUrl = getBaseUrl() + '/Manage/GetPendingTransactionsReport?format=' + format;
        console.log("Toll transactions: " + manageUrl);
        window.location.href = manageUrl;

    };

    
    /* END REPORT EXPORT FUNCTIONS */

    /* REPORT DOWNLOAD FUNCTIONS */
    this.downloadActivitySummary = function () {

        var year = document.getElementById("ActivityYearDropdown").value;
        var format = document.getElementById("activitySummaryExportType").value;
        var manageUrl = getBaseUrl() + '/Manage/GetYearlySummary?format=' + format + '&year=' + year + '&format=PDF';

        monthlyStatementParams = {
            'format': format, 'year': year
        };

        // Display Spinner
        $('#activitySummaryDownloadButton i').removeClass('fa-download');
        $('#activitySummaryDownloadButton i').addClass('fa-spinner fa-spin animated');
        $('#activitySummaryDownloadButton, #activitySummaryEmailButton').attr('disabled', 'disabled');

        window.location.href = manageUrl;

        setTimeout(function () {
            // Hide Spinner
            $('#activitySummaryDownloadButton i').removeClass('fa-spinner fa-spin animated');
            $('#activitySummaryDownloadButton i').addClass('fa-download');
            $('#activitySummaryDownloadButton, #activitySummaryEmailButton').removeAttr('disabled');
        }, 3000);

        return true;
    };

    this.downloadMonthlyStatement = function () {

        var month = document.getElementById("StatementsMonthDropdown").value;
        var year = document.getElementById("StatementsYearDropdown").value;
        var format = document.getElementById("MonthlyStatementExportType").value;

        if (month != "" & year != "") {
            var manageUrl = getBaseUrl() + '/Manage/GetMonthlyStatement?statementMonth=' + month + '&statementYear=' + year + '&format=' + format;

            monthlyStatementParams = {
                'statementMonth': 12, 'statementYear': 2016, 'format': format
            };

            window.location.href = manageUrl;
        }
        else {
            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: "Please select a valid Month and Year."
            },
                    {
                        // Settings
                        type: "danger",
                        delay: 5000,
                        z_index: 3000
                    });
        }




        return true;
    };
    /* END REPORT DOWNLOAD FUNCTIONS */

    /* BEGIN REPORT EMAIL FUNCTIONS */
    this.emailActivitySummary = function () {

        var manageUrl = getBaseUrl() + '/Manage/EmailActivitySummary';

        var data = {
            'year': $('#ActivityYearDropdown').val()
        };

        // Display Spinner
        $('#activitySummaryEmailButton i').removeClass('fa-envelope-o');
        $('#activitySummaryEmailButton i').addClass('fa-spinner fa-spin animated');
        $('#activitySummaryEmailButton, #activitySummaryDownloadButton').attr('disabled', 'disabled');

        $.ajax({
            type: 'GET',
            url: manageUrl,
            data: data,
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).catch(function (jqXHR, status, error) {

            console.error("Error occurred: " + error);

        }).then(function (response) {
            var responseMessage;
            var responseType;

            if (response.success) {
                //document.getElementById("activitySummaryModalMessage").innerHTML = "Email of activity statement has been successfully sent.";
                responseMessage = "Email of activity statement has been successfully sent.";
                responseType = "success";
            }
            else {
                //document.getElementById("activitySummaryModalMessage").innerHTML = "Email could not be sent.";
                responseMessage = "Email could not be sent.";
                responseType = "danger";
            }

            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: responseMessage
            },
            {
                // Settings
                type: responseType,
                delay: 5000,
                z_index: 3000
            });
        });

        setTimeout(function () {
            // Hide Spinner
            $('#activitySummaryEmailButton i').removeClass('fa-spinner fa-spin animated');
            $('#activitySummaryEmailButton i').addClass('fa-envelope-o');
            $('#activitySummaryEmailButton, #activitySummaryDownloadButton').removeAttr('disabled');
        }, 2000);

        return true;
    };

    this.emailMonthlyStatement = function () {

        var manageUrl = getBaseUrl() + '/Manage/EmailMonthlyStatement';

        var data = {
            'statementYear': $('#StatementsYearDropdown').val(),
            'statementMonth': $('#StatementsMonthDropdown').val()
        };
        $.ajax({
            type: 'GET',
            url: manageUrl,
            data: data,
            contentType: 'application/json',
            dataType: 'json',
            encode: true
        }).catch(function (jqXHR, status, error) {

            console.error("Error occurred: " + error);

        }).then(function (response) {

            if (response.success) {
                document.getElementById("monthlyStatementsModalMessage").innerHTML = "Email of monthly statement has been successfully sent.";
            }
            else {
                document.getElementById("monthlyStatementsModalMessage").innerHTML = "Email could not be sent.";
            }

        });
        return true;
    };
    /* END REPORT EMAIL FUNCTIONS */


    /* CONFIRM FUCNTIONS */
    function validateEmail(email) {
        var re = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        return re.test(email);
    }
    function validatePassword(password) {
        var re = /^(?=\S{8,32})(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*).{8,32}$/;
        return re.test(password);
    }

    function validateUserName(username) {
        var re = /^[a-zA-Z0-9.@!#$%&'*+-/=?^_`|{}~]{1,}$/;
        return re.test(username);
    }
    this.confirmPassword = function () {

        var newPass1 = $('#newPass1').val();
        var newPass2 = $('#newPass2').val();
        var confirmMessage = $('#confirmPassMatchMessage');

        var passMatchColor = "#0F8908";
        var failMatchColor = "#AE0101";

        if (newPass1 == '') {
            confirmMessage.html("");
        }

        if (newPass1 != '') {
            if (validatePassword(newPass1)) {
                confirmMessage.html("New Password is valid!");
                confirmMessage.css('color', 'green');
            } else {
                confirmMessage.html("Please provide a Password that is at least eight characters. The Password must contain at least one letter and one number and NO spaces.");
                confirmMessage.css('color', 'red');
                $('#saveSecurity').prop("disabled", true);
                return;
            }
        }
        if (newPass2 != '') {
            if (validatePassword(newPass2)) {
                confirmMessage.html("Confirm Password is valid!");
                confirmMessage.css('color', 'green');
            } else {
                confirmMessage.html("Confirm Password is not valid!");
                confirmMessage.css('color', 'red');
                $('#saveSecurity').prop("disabled", true);
                return;
            }
        }

        if ((newPass1 != '') && (newPass2 != '')) {
            if (newPass1 == newPass2) {

                confirmMessage.css('color', 'green');
                confirmMessage.html("Passwords match!");
                $('#saveSecurity').prop("disabled", false);

            } else {
                confirmMessage.css('color', 'red');
                confirmMessage.html("Passwords do not match.");
                $('#saveSecurity').prop("disabled", true);
            }
        }
    };
    this.confirmUsername = function () {

        var newPass1 = $('#newUsername1').val();
        var newPass2 = $('#newUsername2').val();
        var confirmMessage = $('#confirmUserMatchMessage');

        var passMatchColor = "#0F8908";
        var failMatchColor = "#AE0101";

        if (newPass1 == '') {
            confirmMessage.html("");
        }

        if (newPass1 != '') {
            if (validateUserName(newPass1)) {
                confirmMessage.html("Username is valid!");
                confirmMessage.css('color', 'green');

            } else {
                confirmMessage.html("Please provide a valid Username, spaces and special characters are not allowed.");
                confirmMessage.css('color', 'red');
                $('#saveSecurity').prop("disabled", true);
                return;
            }
        }

        if (newPass2 != '') {
            if (validateUserName(newPass2)) {
                confirmMessage.html("Confirm Username is valid!");
                confirmMessage.css('color', 'green');

            } else {
                confirmMessage.html("Confirm Username is not valid!");
                confirmMessage.css('color', 'red');
                $('#saveSecurity').prop("disabled", true);
                return;
            }
        }

        if ((newPass1 != '') && (newPass2 != '')) {
            if (newPass1 == newPass2) {

                confirmMessage.css('color', 'green');
                confirmMessage.html("Usernames match!");
                $('#saveSecurity').prop("disabled", false);

            } else {
                confirmMessage.css('color', 'red');
                confirmMessage.html("Usernames do not match.");
                $('#saveSecurity').prop("disabled", true);
            }
        }
    };
    this.confirmEmail = function () {

        var newPass1 = $('#securityEmail1').val();
        var newPass2 = $('#securityEmail2').val();
        var confirmMessage = $('#confirmEmailMatchMessage');

        var passMatchColor = "#0F8908";
        var failMatchColor = "#AE0101";

        if (newPass1 == '' && newPass2 != '') {
            confirmMessage.css('color', 'green');
            confirmMessage.html("Please provide a valid Email Address!");
            $('#saveSecurity').prop("disabled", false);
            return;
        }
        if (newPass1 != '') {

            if (validateEmail(newPass1)) {
                confirmMessage.html("New Email is valid!");
                confirmMessage.css('color', 'green');
            } else {
                confirmMessage
                    .html("Please provide a valid Email Address. Email is used to recover your Username and/or Password.");
                confirmMessage.css('color', 'red');
                $('#saveSecurity').prop("disabled", true);
                return;
            }
        }

        if (newPass2 != '') {
            if (validateEmail(newPass2)) {
                confirmMessage.html(" Confirm Email is valid!");
                confirmMessage.css('color', 'green');
            } else {
                confirmMessage.html("Confirm Email is not valid!");
                confirmMessage.css('color', 'red');
                $('#saveSecurity').prop("disabled", true);
                return;
            }
        }

        if ((newPass1 != '') && (newPass2 != '')) {
            if (newPass1 == newPass2) {

                confirmMessage.css('color', 'green');
                confirmMessage.html("Emails match!");
                $('#saveSecurity').prop("disabled", false);

            } else {
                confirmMessage.css('color', 'red');
                confirmMessage.html("Emails do not match.");
                $('#saveSecurity').prop("disabled", true);
            }
        }
    };
    /* END CONFIRM FUNCTIONS */

    /* VALIDATION AND HELPER FUNCTIONS */

    function ValidateContact() {

    };

    /* END VALIDATION AND HELPER FUNCTIONS */

    var selectedTransponderVehicle = {
        LicensePlateNumber: "",
        LicensePlateState: "",
        LicensePlateType: "",
        VehicleMake: "",
        VehicleModel: "",
        VehicleYear: "",
        VehicleColor: "",
        IssuingAuthority: "",
        VehicleClass: "",
        TransponderStatus: "",
        TransponderNumber: ""
    };

    function setEmptySelectedTransponderVehicle() {
        selectedTransponderVehicle.LicensePlateNumber = "";
        selectedTransponderVehicle.LicensePlateState = "";
        selectedTransponderVehicle.LicensePlateType = "";
        selectedTransponderVehicle.VehicleMake = "";
        selectedTransponderVehicle.VehicleModel = "";
        selectedTransponderVehicle.VehicleYear = "";
        selectedTransponderVehicle.VehicleColor = "";
        selectedTransponderVehicle.IssuingAuthority = "";
        selectedTransponderVehicle.VehicleClass = "";
        selectedTransponderVehicle.TransponderStatus = "";
        selectedTransponderVehicle.TransponderNumber = "";
    }

    this.getSelectedTransponderVehicle = function () {
        var localValue = selectedTransponderVehicle;
        //setEmptySelectedTransponderVehicle();
        return localValue;
    }

    this.setSelectedTransponderVehicle = function (_selectedTransponderVehicle) {
        selectedTransponderVehicle = _selectedTransponderVehicle;
    }

    $('.transpondersTable').on('click', ".btn[data-target='#EditTransponders_Modal']", function () {
        var editRow = $(this).parents('tr');

        $('#EditTransponders_Modal').modal('hide');

        var vehicleData = editRow[0].dataset;

        populatePlateTypes(vehicleData.licenseStateSelected, vehicleData.licensePlateTypeId);
        populateModels(vehicleData.vehicleMake, vehicleData.vehicleModel);

        $('#workingTagId').val(vehicleData.tagNumber);
        $("#LicensePlateNumber").val(vehicleData.licensePlateNumber);
        $("#LicenseStateSelected").val(vehicleData.licenseStateSelected);
        $("#LicensePlateType").val(vehicleData.licensePlateTypeId);
        $("#VehicleMake").val(vehicleData.vehicleMake);
        $("#VehicleModel").val(vehicleData.vehicleModel);
        /*        if ($("#VehicleModel").val() == null || $("#VehicleModel").val() == "OTHER" || $("#VehicleModel").val() == "Other" || $("#VehicleModel").val() == "")
                {
                    $("#VehicleModel").val("OTHER");
                    $("#OtherSpecifyFormGroup").show();
                    $("#otherSpecifyModel").val(vehicleData.vehicleModel);
                }
        else
                { $("#OtherSpecifyFormGroup").hide(); }
        */
        $("#VehicleYear").val(vehicleData.vehicleYear);
        $("#VehicleColor").val(vehicleData.vehicleColor);
        $("#IssuingAuthority").val(vehicleData.issuingAuthority);
        $("#StateCode").val(vehicleData.stateCode);
        $("#VehicleClass").val(vehicleData.vehicleClass);

        //depending upon the transponder status, set the checkbox
        if (vehicleData.initialStatusCode == 'D' || vehicleData.initialStatusCode == 'I') {
            $('#TransponderStatus').prop('checked', true); // Checks it
            //$('#TransponderStatus').attr('checked', true);
        }
        else {
            $('#TransponderStatus').prop('checked', false); // Checks it
        }
        /* Niraj: disable the code as the buttons have been deleted
        if (vehicleData.initialStatusCode === 'A') {
            $('#enableTransButton').hide();
            $('#disableTransButton').show();
        } else {
            $('#disableTransButton').hide();
            $('#enableTransButton').show();
        }
        */
    });


    $('.transpondersTable').on('click', ".btn[data-target='#ReplaceTransponders_Modal']", function () {
        var editRow = $(this).parents('tr');
        $('#ReplaceTransponders_Modal').modal('hide');

        var vehicleData = editRow[0].dataset;
        var transponderVehicle = new Object();

        transponderVehicle.LicensePlateNumber = vehicleData.licensePlateNumber;
        transponderVehicle.LicensePlateState = vehicleData.licenseStateSelected;
        transponderVehicle.LicensePlateType = vehicleData.licensePlateTypeId;
        transponderVehicle.VehicleMake = vehicleData.vehicleMake;
        transponderVehicle.VehicleModel = vehicleData.vehicleModel;
        transponderVehicle.VehicleYear = vehicleData.vehicleYear;
        transponderVehicle.VehicleColor = vehicleData.vehicleColor;
        transponderVehicle.IssuingAuthority = vehicleData.issuingAuthority;
        transponderVehicle.VehicleClass = vehicleData.vehicleClass;
        transponderVehicle.TransponderStatus = vehicleData.initialStatusCode;
        transponderVehicle.TransponderNumber = vehicleData.transponderNumber;

        manageController.setSelectedTransponderVehicle(transponderVehicle);

    });

    $('#VehicleMake').change(function () {
        var vehicleMakeName = $(this).val();

        populateModels(vehicleMakeName, '');
    });

    function populateModels(vehicleMakeName, defaultSelectedModel) {

        // Let user know we are loading Models
        $('#VehicleModel').empty();
        $('#VehicleModel').html('<option selected="selected">Loading, please wait...</option>');

        var url = getBaseUrl() + '/Manage/GetVehicleModelNames?_' + $.now() + '&vehicleMakeName=' + vehicleMakeName;

        $.getJSON(url, function (modelList) {
            if (modelList !== null && typeof (modelList) !== 'undefined') {
                if (modelList.length > 0) {
                    var modelOptions = $('#VehicleModel');

                    // Clear out current Options
                    modelOptions.empty();

                    var models = '';
                    var selectedOption = '';
                    var index = 0;

                    for (index = 0; index < modelList.length; index += 1) {
                        selectedOption = (modelList[index].Selected === 'true') ? 'selected="selected"' : '';

                        models += '<option value="' + modelList[index].Value + '" ' + selectedOption + '>' + modelList[index].Text + '</option>';
                    }

                    // Add Models based on Make selection
                    modelOptions.html(models);
                }

                $("#VehicleModel").val(defaultSelectedModel);
                if (defaultSelectedModel !== '')
                    $('#EditTransponders_Modal').modal('show');

                if ($("#VehicleModel").val() == null || $("#VehicleModel").val() == "OTHER" || $("#VehicleModel").val() == "Other" || $("#VehicleModel").val() == "") {
                    $("#VehicleModel").val("OTHER");
                    $("#OtherSpecifyFormGroup").show();
                    $("#otherSpecifyModel").val(defaultSelectedModel);
                }
                else { $("#OtherSpecifyFormGroup").hide(); }
            }
        });
    }


    function populatePlateTypes(stateCode, selectedPlateType) {
        // Let user know we are loading Models
        $('#LicensePlateType').empty();
        $('#LicensePlateType').html('<option selected="selected">Loading, please wait...</option>');

        var url = getBaseUrl() + '/Manage/GetPlateTypes?_' + $.now() + '&stateCode=' + stateCode;

        $.getJSON(url, function (modelList) {
            if (typeof (modelList) !== 'undefined') {
                if (modelList.length > 0) {
                    var modelOptions = $('#LicensePlateType');

                    // Clear out current Options
                    modelOptions.empty();

                    var models = '';
                    var selectedOption = '';
                    var index = 0;

                    for (index = 0; index < modelList.length; index += 1) {
                        selectedOption = (modelList[index].Selected === 'true') ? 'selected="selected"' : '';

                        models += '<option value="' + modelList[index].Value + '" ' + selectedOption + '>' + modelList[index].Text + '</option>';
                    }

                    // Add Models based on Make selection
                    modelOptions.html(models);
                }

                $("#LicensePlateType").val(selectedPlateType);
            }
        });
    }

    $('#LicenseStateSelected').change(function () {
        var licenseState = $(this).val();
        populatePlateTypes(licenseState, '');
    });

    $('#StatementsYearDropdown').change(function () {
        updateStateMonths();
    });


    //This method will toggle between ACH and CC Payment
    this.DoCCPayment = function (value) {
        if (value === true) {
            $('.CCPayment').show();
            $('.ACHPayment').hide();
            $('#IsCCPayment').val('True');
        }
        else {
            $('.CCPayment').hide();
            $('.ACHPayment').show();
            $('#IsCCPayment').val('False');
        }

    }


    var allreadySelectAllForPendingTrans = false;
    // Populate Pending Transaction
    this.populatePendingTransactions = function (startDate, endDate, init) {
        //asyncOption = typeof asyncOption !== 'undefined' ? asyncOption : true;
        var data = {
            'start': init ? '0001/1/1' : toCFXDateFormat(startDate), // $('#tollTransactionsStartDate').val(),
            'end': init ? '0001/1/1' : toCFXDateFormat(endDate), //$('#tollTransactionsEndDate').val()
            'init': init
        };
        if (_debug) console.log('Event:' + 'populatePendingTransactions' + 'Event Data: ', data);
        $.ajax({
            url: getBaseUrl() + '/Manage/GetPendingTransactions',
            async: false,
            type: 'post',
            data: {
                'start': $("#pendingTransStartDate").text(), 'end':  $("#pendingTransEndDate").text(), 'init': init
            },
            success: function (response) {
                $("#pendingTransactionTable").html('');
                pendingTransLoaded = true;
                _filtering = false;


                if (_debug) console.log('Event:' + 'populatePendingTransactions |' + 'Event Data: ', data);
                if (response.success) {

                    $("#pendingTransactionTable").on({
                        "ready.ft.table": function (e, ft) {
                            $("#pendingTransactionTable").find(".footable-filtering").hide();
                            //Below code is to add Select All Checkbox
                            /*
                            if (!allreadySelectAllForPendingTrans) {
                                allreadySelectAllForPendingTrans = true;
                                var checkedAll = "checked";
                                $("#pendingTransactionTable").find("thead tr:last").before("<tr class='footable-header'><th><input id='selectAllPendingTransactions' class='selectAllPendingTransactions' " + checkedAll + " type='checkbox' value='true'>  Select All </input></th></tr>");
                            }
                            else{
                                $("#selectAllPendingTransactions").prop('checked', false);
                            }*/
                        },
                        "after.ft.filtering": function (e, ft, filter) {
                            hideSpinner();
                            ReCalculateAndDisplayTotalPendingAmount(e, true, ft);
                            //Below code is to add Select All Checkbox
                            /*
                            if (!allreadySelectAllForPendingTrans) {
                                allreadySelectAllForPendingTrans = true;
                                var checkedAll = "checked";
                                $("#pendingTransactionTable").find("thead tr:last").before("<tr class='footable-header'><th><input id='selectAllPendingTransactions' class='selectAllPendingTransactions' " + checkedAll + " type='checkbox' value='true'>  Select All </input></th></tr>");
                            }
                            else{
                                $("#selectAllPendingTransactions").prop('checked', false);
                            }*/
                        },/*
                        "predraw.ft.table": function (e, ft) {
                            //ReCalculateAndDisplayTotalPendingAmount(e, true, ft);
                            //Below code is to add Select All Checkbox
                        }*/
                    }).
                    footable({
                        "columns": pendingTransactionsColumns,
                        "rows": response.data
                    });

                    $("#pendingTransactionTable").show();

                   

                    $("#pendingTransactionAmount").text(response.paymentAmount.toFixed(2));
                    manageController.ReDisplayTotalPendingAmount(response.paymentAmount);


                }
                //$("#pendingTransactionSlider").enabled;
                if (_debug) console.log('Event:' + 'populatePendingTransactions |' + 'Footable loaded');
            }
        });

        return true;
    };

    this.getPaymentForPendingTransactions = function (event) {
        var localPaymentDiv = $(event.target).parents('#PendingPayment_Modal');

        //Get sales card div
        var $mySalesCardDiv = localPaymentDiv.find('.SalesCardPayment');

        var isCCPayment = true;
        var isPayFromAccount = false;
        var nameOnCard = "", cardNumber = "", cardType = "", expirationMonth = "", expirationYear = "", bankActNumber = "", bankRoutingNumber = "", bankActFirstName = "", bankActLastName = "";
        var paymentAmount = 0, webBalance = 0;
        var useCardOnFile = false;

        webBalance = Number($mySalesCardDiv.attr("data-webBalance"));

        if (typeof ($mySalesCardDiv.find('li.active')) != 'undefined') {

            if ($.trim($mySalesCardDiv.find('li.active').text()).indexOf("Credit Card") >= 0) //Check whether CC or ACH Selected
                isCCPayment = true;
            else
                isCCPayment = false;

            if ($.trim($mySalesCardDiv.find('li.active').text()).indexOf("From Balance") >= 0) //Check whether CC or ACH Selected
                isPayFromAccount = true;
            else
                isPayFromAccount = false;

            if (isPayFromAccount) {

            }
            else {
                if (isCCPayment) {

                    nameOnCard = $mySalesCardDiv.find('#NameOnCard').val();
                    useCardOnFile = $mySalesCardDiv.find('.useCardOnFileSelection').prop('checked');
                    cardNumber = (useCardOnFile == "true" || useCardOnFile == true) ? $mySalesCardDiv.find('.hiddenCardNumber').val() : $mySalesCardDiv.find('#CreditCardNumber').val();
                    expirationMonth = $mySalesCardDiv.find('#ExpirationMonth').val();
                    expirationYear = $mySalesCardDiv.find('#ExpirationYear').val();
                    cardType = $mySalesCardDiv.find('#CreditCardType').val();
                }
                else {

                    bankActFirstName = $mySalesCardDiv.find('#AccountHolderFirstName').val();
                    bankActLastName = $mySalesCardDiv.find('#AccountHolderLastName').val();
                    bankActNumber = $mySalesCardDiv.find('#BankAccountNumber').val();
                    bankRoutingNumber = $mySalesCardDiv.find('#RoutingNumber').val();
                }
            }

        }
        //var startDate = $("#pendingTransactionActualStartDate").text();
        //var endDate = new Date(startDate);
        //var incr = Number($("#pendingTransactionSlider").val());
        //endDate.setDate(endDate.getDate() + incr);
        var startDate = $('#pendingTransStartDate').val();
        var endDate = $('#pendingTransEndDate').val();
        //Return Payment Information JSON Object
        return {
            'IsPayFromAccount': isPayFromAccount,
            'IsCCPayment': isCCPayment,
            'NameOnCard': nameOnCard,
            'CreditCardNumber': cardNumber,
            'CreditCardType': cardType,
            'ExpirationMonth': expirationMonth,
            'ExpirationYear': expirationYear,
            'AccountHolderFirstName': bankActFirstName,
            'AccountHolderLastName': bankActLastName,
            'BankAccountNumber': bankActNumber,
            'RoutingNumber': bankRoutingNumber,
            'PaymentAmount': paymentAmount,
            'UseCardOnFile': useCardOnFile,
            'StartDate': startDate,
            'EndDate': endDate, //toCFXDateFormat(endDate),
            'WebBalance': webBalance
        }
    }

    this.getSelectedPendingTranaction = function (event, ft, isFromFilter)
    {
        var checked = [];
        var unChecked = [];
        var ft = ft || FooTable.get(".pendingTransactionTable");
        var totalAmount = 0;

        var ftRows = isFromFilter ? PendingTableSnapShot || ft.rows.array.all || [] : ft.rows.array.all || [];

        for (var i = 0, l = ftRows.length, row; i < l; i++) {
            // the FooTable.Row object
            row = ftRows[i];

            // but we want to work with DOM elements so to get access to the row use
            // Other not loaded element will be used from display formatter
            if (row.$el == null) {
                //This will a workaround for footable issue, foo table will not find .$el as an element untill it will not rendered (this is because of dynamic element loading for multiple citation
                var splitStr = row.value["TransactionIDValue"].split("|");
                if (splitStr.length >= 3) {
                    if (splitStr[1] != "1") //That means not declined in hidden rows of footable
                    {
                        totalAmount = Number(totalAmount) + Number(splitStr[2]);
                        unChecked.push(splitStr[0]);
                    }
                    else
                    {
                        checked.push(splitStr[0]);
                    }
                }
            }
            else {
                var element = $(row.$el.find("input[type=checkbox]"))[0];
                if (element != null && typeof (element) != "undefined"
                        && !element.checked) //Not checked
                {
                    unChecked.push(element.value);
                    totalAmount = Number(totalAmount) + Number(element.attributes["data_amount"] != "undefined" ? Number(element.attributes["data_amount"].value) : 0);
                }
                else
                {
                    checked.push(element.value);
                }
            }

        }
        totalAmount = totalAmount.toFixed(2);
        return{
            'SelectedIDs': unChecked,
            'DisputedIDs': checked,
            'TotalPayableAmount': totalAmount
        };
    }

    this.getProcessingPendingTransaction = function(event)
    {
        var paymentAndFilterInfo = manageController.getPaymentForPendingTransactions(event);
        var pendingTransactionInfo = manageController.getSelectedPendingTranaction(event, null, true);
        //manageController.PreviewPendingTransactions(event);
        
        var processPendingTransactionData = {
            'Payment': paymentAndFilterInfo,
            'TotalPayableAmount': pendingTransactionInfo.TotalPayableAmount,
            'IsPayingFromAccount': paymentAndFilterInfo.IsPayFromAccount,
            'TransactionIDs': pendingTransactionInfo.SelectedIDs,
            'DisputedTransactionIDs': pendingTransactionInfo.DisputedIDs,
            'StartDate': paymentAndFilterInfo.StartDate,
            'EndDate': paymentAndFilterInfo.EndDate,
            'AccountBalance': paymentAndFilterInfo.WebBalance
        }

        return processPendingTransactionData;
    }

    this.PreviewPendingTransactions = function (event) {

        var validationArea = $(event.target).parents('#PendingPayment_Modal').find('.SalesCardPayment');

        var validPayment = shoppingCartController.validateCartPaymentArea(validationArea);

        if (validPayment) {

            var data = this.getProcessingPendingTransaction(event)

            $.ajax({
                url: getBaseUrl() + '/Manage/PreviewPendingTransactions',
                async: false,
                type: 'post',
                data: JSON.stringify(data),
                contentType: 'application/json',
                dataType: 'html',
                encode: true, 
                success: function (response) {

                    if (response.indexOf('"success":false') > 0) {
                        response = $.parseJSON(response);

                        if (typeof (response.success) != 'undefined' && !response.success) {

                            if (typeof (response.dataFormat) && response.dataFormat == "text") { // Display Error in text
                                $.notify({
                                    icon: 'fa fa-exclamation-triangle',
                                    message: response.message
                                },
                                               {
                                                   type: 'danger',
                                                   delay: 5000,
                                                   z_index: 3000
                                               });
                            }
                            else if (typeof (response.dataFormat) && response.dataFormat == "list") {

                                $.each(response.errors, function (k, v) {
                                    $.notify({
                                        icon: 'fa fa-exclamation-triangle',
                                        message: this.Reference == "" || this.Reference == "null" || this.Reference == null ? this.Message : this.Message + " in " + this.Reference
                                    },
                                               {
                                                   type: 'danger',
                                                   delay: 5000,
                                                   z_index: 3000
                                               });
                                });
                            }

                        }

                    }
                    else {
                        //if (response.success) {
                        //$("#previewPendingPayments").hide();
                        $("#payPendingTransactionsPanel").hide();

                        $("#payPendingTransactionReviewPanelData").html(response);

                        //$("#payPendingPayments").show();
                        $("#payPendingTransactionReviewPanel").show();
                    }
                    //}
                }
            });
        }
        return true;
    }

    this.ProcessPendingTransactions = function (e, transactions, payment) {

        var data = this.getProcessingPendingTransaction(e)

        $.ajax({
            url: getBaseUrl() + '/Manage/ProcessPendingTransactions',
            async: false,
            type: 'post',
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'html',
            encode: true, 
            success: function (response) {


                if (response.indexOf('"success":false') > 0) {
                    response = $.parseJSON(response);

                    if (typeof (response.success) != 'undefined' && !response.success) {

                        if (typeof (response.dataFormat) && response.dataFormat == "text") { // Display Error in text
                            $.notify({
                                icon: 'fa fa-exclamation-triangle',
                                message: response.message
                            },
                                           {
                                               type: 'danger',
                                               delay: 5000,
                                               z_index: 3000
                                           });
                        }
                        else if (typeof (response.dataFormat) && response.dataFormat == "list") {

                            $.each(response.errors, function (k, v) {
                                $.notify({
                                    icon: 'fa fa-exclamation-triangle',
                                    message: this.Reference == "" || this.Reference == "null" || this.Reference == null ? this.Message : this.Message + " in " + this.Reference
                                },
                                           {
                                               type: 'danger',
                                               delay: 5000,
                                               z_index: 3000
                                           });
                            });
                        }

                    }

                }
                else
                {
                    manageController.RefreshScreen = true;

                    $("#payPendingTransactionReviewPanel").hide();

                    $("#payPendingTransactionConfirmationData").html(response);

                    $("#payPendingTransactionConfirmationPanel").show();

                }
            }
        });

        return true;
    }

    this.ProcessDisputePendingTransactions = function (e, transactions, payment) {

        var data = this.getProcessingPendingTransaction(e)

        $.ajax({
            url: getBaseUrl() + '/Manage/ProcessPendingTransactions',
            async: false,
            type: 'post',
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'html',
            encode: true, 
            success: function (response) {

                if (response.indexOf('"success":false') > 0) {
                    response = $.parseJSON(response);

                    if (typeof (response.success) != 'undefined' && !response.success) {

                        if (typeof (response.dataFormat) && response.dataFormat == "text") { // Display Error in text
                            $.notify({
                                icon: 'fa fa-exclamation-triangle',
                                message: response.message
                            },
                                           {
                                               type: 'danger',
                                               delay: 5000,
                                               z_index: 3000
                                           });
                        }
                        else if (typeof (response.dataFormat) && response.dataFormat == "list") {

                            $.each(response.errors, function (k, v) {
                                $.notify({
                                    icon: 'fa fa-exclamation-triangle',
                                    message: this.Reference == "" || this.Reference == "null" || this.Reference == null ? this.Message : this.Message + " in " + this.Reference
                                },
                                           {
                                               type: 'danger',
                                               delay: 5000,
                                               z_index: 3000
                                           });
                            });
                        }

                    }

                }
                else {
                    //if (response.success) {
                    //$("#previewPendingPayments").hide();
                    manageController.RefreshScreen = true;

                    $("#payPendingTransactionReviewPanel").hide();

                    $("#payPendingTransactionConfirmationData").html(response);

                    $("#payPendingTransactionConfirmationPanel").show();
                }
                //}
            }
        });

        return true;
    }

    //Below variables and entire CreatePendingTransactionSlider is for Slider Functionality
    var pendingTransLoaded = false;
    var _minPendingTranDate = null;
    var _maxPendingTranDate = null;
    var _filtering = false;
    this.CreatePendingTransactionSlider = function (minPendingTranCount, maxPendingTranCount, minPendingTranDate, maxPendingTranDate) {

        if (_debug) console.log('EPassWeb :' + 'creating Pending Transaction Slider...');

        _minPendingTranDate = minPendingTranDate;
        _maxPendingTranDate = maxPendingTranDate;
        maxPendingTranCount = maxPendingTranCount - 1;
        maxPendingTranCount = maxPendingTranCount.toFixed(0);

        // Slider Functionality for Pending Transactions
        /*var transactionSlider = $('#pendingTransactionSlider').bootstrapSlider({
            min: minPendingTranCount,
            max: maxPendingTranCount,
            step: 1,
            //range: true,
            value: maxPendingTranCount, // [minPendingTranCount, maxPendingTranCount]
            formatter: function (value) {
                if (_debug) console.log('EPassWeb :' + 'Slider : Tooltip Requested.');

                if (pendingTransLoaded) {
                    if (value[1]) {
                        var newRangeStart = new Date(minPendingTranDate);
                        var newRange = newRangeStart;
                        newRangeStart.setDate(newRangeStart.getDate() + value[0])
                        newRange.setDate(newRange.getDate() + value[1]);
                        //FilterPendingTransactions(newRangeStart, newRange);
                        return toCFXDateFormat(newRangeStart) + ' - ' + toCFXDateFormat(newRange);
                    } else {
                        var newRangeStart = new Date(minPendingTranDate);
                        var newRangeEnd = new Date(minPendingTranDate);;
                        newRangeEnd.setDate(newRangeEnd.getDate() + value);
                        //FilterPendingTransactions(newRangeStart, new Date(maxPendingTranDate));
                        return toCFXDateFormat(newRangeStart) + ' - ' + toCFXDateFormat(newRangeEnd);
                    }
                }

            }
        });
        var persistedSliderEvt = null;
        transactionSlider.on('change', function (evt) {

            persistedSliderEvt = evt;
            _filtering = true;
            if (_debug) console.log('EPassWeb :' + 'Slider : OnChange : Fired.', evt);
            //$("#pendingTransactionSlider").bootstrapSlider("disable");
            if (pendingTransLoaded) {

                if (_debug) console.log('EPassWeb :' + 'Slider : OnChange : Processing.', evt);

                var newRangeStart = new Date(_minPendingTranDate);
                var newRangeEnd = new Date(_minPendingTranDate);

                if (evt.value.newValue[1]) {
                    newRangeStart.setDate(newRangeStart.getDate() + evt.value.newValue[0])
                    newRangeEnd.setDate(newRangeEnd.getDate() + evt.value.newValue[1]);
                    //FilterPendingTransactionsDays(evt.value.newValue[0], evt.value.newValue[1]);
                }
                else {
                    newRangeEnd.setDate(newRangeStart.getDate() + evt.value.newValue)
                    //FilterPendingTransactionsDays(0, evt.value.newValue);
                }

                //displaySpinner();
                //FilterPendingTransactionsDays(0, evt.value.newValue);
                //$(".slider-disabled").removeClass('slider-disabled');


                setTimeout(
                    function () {
                       // if (!_filtering) {
                            displaySpinner();
                            manageController.FilterPendingTransactionsDays(0, persistedSliderEvt.value.newValue);
                            //FilterPendingTransactions(newRangeStart, newRangeEnd);
                            //$(".slider-disabled").removeClass('slider-disabled');
                        //}
                        setTimeout(
                    function () {
                        _filtering = false; hideSpinner();
                        //$(".slider-disabled").removeClass('slider-disabled');
                    }, 500);
                    },500);

            }

        });
        */
    }

    this.InitForPayFromAccount = function(webBalance, webAmountSelected)
    {
    }
    //TransactionDateElapsed

    this.FilterPendingTransactionsDays = function (dayFirst, dayLast) {
        var FT = FooTable.get('#pendingTransactionTable');
        if (typeof FT != "undefined") {
            var filtering = FT.use(FooTable.Filtering);

            if (typeof filtering != "undefined") {
                //filtering.removeFilter('startFilter');
                filtering.removeFilter('endFilter');

                //filtering.addFilter('startFilter', '> ' + Number(dayFirst - 1), ['TransactionDateElapsed']);
                filtering.addFilter('endFilter', '<' + Number(dayLast + 1), ['TransactionDateElapsed']);

                filtering.filter();

                //hideSpinner();
            }
        }
    }

    function ReCalculateAndDisplayTotalPendingAmount(event, isFromFilter, ft) {

        var getTrans = manageController.getSelectedPendingTranaction(event, ft, isFromFilter);
        manageController.ReDisplayTotalPendingAmount(getTrans.TotalPayableAmount)
        
    }
    this.WebBalance = 0;
    this.RefreshScreen = true;

    this.ReDisplayTotalPendingAmount = function (amount)
    {
        var amount = Number(amount).toFixed(2) || 0;

        $("#pendingTransactionAmount").text(amount);


        if (Number(amount) <= 0) {
            $(".PendingTransactionSalesCardPayment").hide();
            $("#payPendingPayments").hide();
            $("#disputePendingPayments").show();
            
        }
        else {
            $(".PendingTransactionSalesCardPayment").show();
            $("#payPendingPayments").show();
            $("#disputePendingPayments").hide();

        }

        var fromAccount = $.trim($("ul#pendingPaymentTabs li.active").text()).indexOf("From Balance") >= 0 ? true : false;
        $("#previewPendingPayments").prop('disabled', false);

        if (fromAccount) {
            $("#tollAmountSelected").text(Number(amount).toFixed(2));

            
            if ( Number(amount) > 0 && manageController.WebBalance < Number(amount)) {
                $("#previewPendingPayments").prop('disabled', true);
                $("#alertNotEnoughFromAccount").show();
                $("#alertPayFromAccount").hide();

            }
            else {
                $("#balanceAfterToll").text((manageController.WebBalance  - Number(amount)).toFixed(2));


                $("#alertNotEnoughFromAccount").hide();
                $("#alertPayFromAccount").show();
            }
        }

        /*
        if (Number(amount) < 0) {
            $("#PendingTransactionSalesCardPayment").hide();
        }
        else {
            $("#previewPendingPayments").prop('disabled', false);

            var fromAccount = false;

            if (fromAccount)
            {
            if (WebBalance < Number(amount)) {
                $("#previewPendingPayments").prop('disabled', true);
            }
            else {
                $("#previewPendingPayments").prop('disabled', false);
            }
            }
        }*/

        


    }

    this.ResetPendingPopup = function()
    {
        $("#payPendingTransactionsPanel").show();
        $("#payPendingTransactionReviewPanel").hide();
        $("#payPendingTransactionConfirmationPanel").hide();

        if (_maxPendingTranDate != null)
            $("#pendingTransEndDate").datepicker('setDate', _maxPendingTranDate);
    }

    function FilterPendingTransactions(startDate, endDate) {
        if (!_filtering) {
            $("#pendingTransactionSlider").disabled;
            _filtering = true;
            if (_debug) console.log('EPassWeb :' + 'Slider : FilterPendingTransactions : Processing.');

            manageController.populatePendingTransactions(startDate, endDate, false);

            if (_debug) console.log('EPassWeb :' + 'Slider : FilterPendingTransactions : Processed.');

           
        }

        return true;

    }

    //Below is Filter of Native Footable Filter but not workking
    //function FilterPendingTransactions(startDate, endDate) {
    //    debugger;

    //    var FT = FooTable.get('#pendingTransactionTable');
    //    if(typeof FT != "undefined") {
    //        var filtering = FT.use(FooTable.Filtering);

    //        if (typeof filtering != "undefined") {
    //            filtering.removeFilter('startFilter');
    //            filtering.removeFilter('endFilter');

    //            filtering.addFilter('startFilter', '>=' +startDate, ['TransactionDate']);
    //            filtering.addFilter('endFilter', '<=' +endDate, ['TransactionDate']);

    //            filtering.filter();
    //            }
    //  }
    //}

    //ProcessPendingTransactions
};

function updateStateMonths() {
    var currentYear = new Date().getFullYear();
    var currentMonth = new Date().getMonth();
    var accountOpenedYear = parseInt($('#AccountOpenedYear').val());
    var accountOpenedMonth = parseInt($('#AccountOpenedMonth').val());
    var recentStatementMonth = parseInt($('#RecentStatementMonth').val());

    var selectedYear = parseInt($('#StatementsYearDropdown').val());
    var monthsDropdown = document.getElementById("StatementsMonthDropdown");

    monthsDropdown.options.length = 0;

    if (selectedYear == accountOpenedYear) {
        var EndMonths = accountOpenedYear == currentYear ? recentStatementMonth : 11;

        for (var x = EndMonths; x >= (accountOpenedMonth - 1) ; x--) {
            var option = document.createElement("option");
            option.text = months[x].name;
            option.value = months[x].value;
            monthsDropdown.options.add(option);
        }
    }
    else if (selectedYear == currentYear) {
        for (var x = recentStatementMonth - 1 ; x >= 0; x--) {
            var option = document.createElement("option");
            option.text = months[x].name;
            option.value = months[x].value;
            monthsDropdown.options.add(option);
        }
    }
    else {
        for (var x = 11; x >= 0; x--) {
            var option = document.createElement("option");
            option.text = months[x].name;
            option.value = months[x].value;
            monthsDropdown.options.add(option);
        }
    }
}

// Initialize correct months for statement dropdown.
$(document).ready(function () {

    updateStateMonths();

    $("#vehicleTransponderGrid").footable();
    $("#parkingTransactionsVehicleSelection").footable();

    // On Key Up Event Handler to ensure Numeric Inputs adhere to their Max Length
    $('input[type=number]').on('keyup', function (event) {
        var maxlength = $(this).attr('maxlength');

        if (this.value.length > maxlength) {
            this.value = this.value.slice(0, maxlength);
        }
    });



    //Below method will fire when user clicks on Replace Transponder and modal is displaying
    $('#ReplaceTransponders_Modal').on('show.bs.modal', function (e) {
        var vehicleToReplace = manageController.getSelectedTransponderVehicle();

        $("#replace_LicensePlateNumber").text(vehicleToReplace.LicensePlateNumber);
        $("#replace_LicensePlateState").text(vehicleToReplace.LicensePlateState);
        $('#ReplaceTransponders_Modal').attr('transData-licensePlateNumber', vehicleToReplace.LicensePlateNumber);
        $('#ReplaceTransponders_Modal').attr('transData-licensePlateState', vehicleToReplace.LicensePlateState);
        $('#ReplaceTransponders_Modal').attr('transData-transponderNumber', vehicleToReplace.TransponderNumber);
        $('#ReplaceTransponders_Modal').attr('transData-vehicleMake', vehicleToReplace.VehicleMake);
        $('#ReplaceTransponders_Modal').attr('transData-vehicleModel', vehicleToReplace.VehicleModel);
        $('#ReplaceTransponders_Modal').attr('transData-vehicleYear', vehicleToReplace.VehicleYear);
        $('#ReplaceTransponders_Modal').attr('transData-vehicleColor', vehicleToReplace.VehicleColor);

        //As par new requirements/changes prepaidToll Amount will set to 0
        $('#ReplaceTransponders_Modal').find("#PrepaidTollsAmount").val(0);

    });

    // Modal Close Event Handler for Pending Payments 
    $('#PendingPayment_Modal').on('hidden.bs.modal', function () {
        console.log("Screen Refreshing" + manageController.RefreshScreen);

        if (manageController.RefreshScreen)
            location.reload();
    });

    //Below code will execute when user clicks on 
    $('#PendingPayment_Modal').on('show.bs.modal', function (e) {

        //AJAX Call to get pending transactions
        manageController.ResetPendingPopup();
        manageController.RefreshScreen = false;
        manageController.populatePendingTransactions(null, null, true);
        /*
                $('#pendingTransactionTable').on({
                    "init.ft.table": function (e, ft) {
                        debugger;
                        // bind to the plugin initialize event to do something
                    }
                }).footable();*/
        /*
        $("#pendingTransactionTable").footable({
            'on': {
                'after.ft.paging': function (e, ft) {
                    debugger;
                    var checkedAll = "checked";// : "";
                    $("#pendingTransactionTable").find("thead").append("<tr><input id='selectAllPendingTransactions' class='selectAllPendingTransactions' " + checkedAll + " type='checkbox' value='true' /></tr>");
                }
            }
        });*/

    });




    // Define Base Popover Template
    var popoverBaseTemplate = '<div class="popover"><div class="arrow"></div><h3 class="popover-title {Style}"></h3><div class="popover-content"></div></div>';
    var popoverTollTemplate = '<div class="popover" style="width:300px!important"><div class="arrow"></div><h3 class="popover-title {Style}"></h3><div class="popover-content"></div></div>';


    var popoverInfoTemplateLarge = popoverTollTemplate.replace('{Style}', 'info');

    var makeModelHelpInfo = "<p>To enter a new model, please choose 'Other' from list and specify your vehicle model.</p> ";

    /*
        $('body').on('click', '.makeModelHelp', function (e) {
            $(this).popover({
                placement: 'top',
                template: popoverInfoTemplateLarge,
                title: 'Enter Vehicle Model',
                content: makeModelHelpInfo,
                html: true,
            });
        });*/

    $('#makeModelHelp').popover({
        placement: 'top',
        template: popoverInfoTemplateLarge,
        title: 'Enter Vehicle Model',
        content: makeModelHelpInfo,
        html: true
    });

    $('#previewPendingPayments').on('click', function (event) {

        manageController.PreviewPendingTransactions(event);

    });

    $('#payPendingPayments').on('click', function (event) {


        manageController.ProcessPendingTransactions(event);

    });
    $('#disputePendingPayments').on('click', function (event) {


        manageController.ProcessDisputePendingTransactions(event);

    });

    $('body').on('click', '.newPopupFromPopup', function (e) {
        var propValue = false;
        var $currentModal = $(e.target).parents(".modal");

        if(typeof $currentModal != "undefine")
        {
            $currentModal.modal('hide');
        }

    });


    $('#payPendingTransactionsPanel a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var pendingTransactionAmount = Number($("#pendingTransactionAmount").text());
        manageController.ReDisplayTotalPendingAmount(pendingTransactionAmount);
    });
    

    $('body').on('change', '.pendingTransactionChk', function (e) {
        var propValue = false;

        var element = $(this)[0];
        if (element.checked) {
            propValue = true;
        }
        var pendingTransactionAmount = Number($("#pendingTransactionAmount").text());
        var amount = element.attributes["data_amount"] != "undefined" ? Number(element.attributes["data_amount"].value) : 0;

        //If checked then remove amount else add
        if (propValue) {
            pendingTransactionAmount -= amount;
        }
        else {
            pendingTransactionAmount += amount;
        }

        //$("#pendingTransactionAmount").text(pendingTransactionAmount.toFixed(2));
        manageController.ReDisplayTotalPendingAmount(pendingTransactionAmount.toFixed(2));
    });


    $('body').on('change', '#selectAllPendingTransactions', function (e) {
        var propValue = false;
        if ($(this).is(':checked')) {
            propValue = true;
            //$(".pendingTransactions").prop('checked', true);
        }


        var checked = [];
        var ft = FooTable.get(".pendingTransactionTable");
        var totalAmount = 0;

        for (var i = 0, l = ft.rows.all.length, row; i < l; i++) {
            // the FooTable.Row object
            row = ft.rows.all[i];

            // but we want to work with DOM elements so to get access to the row use
            // Other not loaded element will be used from display formatter
            if (row.$el == null) {
                //This will a workaround for footable issue, foo table will not find .$el as an element untill it will not rendered (this is because of dynamic element loading for multiple citation
                var splitStr = row.value["TransactionIDValue"].split("|");
                if (splitStr.length >= 3) {
                    totalAmount = Number(totalAmount) + Number(splitStr[2]);
                }
            }
            else {
                row.$el.find("input[type=checkbox]").prop('checked', propValue);

                // if marked then only include into process elements.
                if (propValue) {
                    totalAmount = Number(totalAmount) + Number(row.$el.find("input[type=checkbox]:checked").map(function () {
                        return this.attributes["data_amount"] != "undefined" ? Number(this.attributes["data_amount"].value) : 0;
                    }).get());
                }
                //checked.push(row.$el.find("input[type=checkbox]:checked").map(function () { return this.value }).get());
            }

        }

        if (propValue) {
            $("#previewPendingPayments").prop("disabled", false);
            $("#pendingTransactionAmount").text(totalAmount.toFixed(2));
            //calculate Sum of payment
        }
        else {
            //Disable Preview button
            //Mark payment amount to 0
            $("#previewPendingPayments").prop("disabled", true);
            $("#pendingTransactionAmount").text("0.00");
        }

    });

});



(function(F){
    /**
     * Checks if the row is filtered using the supplied filters.
     * @this FooTable.Row
     * @param {Array.<FooTable.Filter>} filters - The filters to apply.
     * @returns {boolean}
     */
    F.Row.prototype.filtered = function(filters){
        var result = true, self = this;
        F.arr.each(filters, function(f){
            if (result){ //check if its already flag to false;
                if (f.query._original.charAt(0) == ">"){
                    result = Number(self.value[f.columns["0"].name]) > Number(f.query._original.substring(1));
                    return (Number(self.value[f.columns["0"].name]) > Number(f.query._original.substring(1)));
                }else{
                    if (f.query._original.charAt(0) == "<"){
                        result = Number(self.value[f.columns["0"].name]) < Number(f.query._original.substring(1));
                        //if (_debug) console.log("value of result : " + result + " for " + self.value[f.columns["0"].name], f);
                        return (Number(self.value[f.columns["0"].name]) < Number(f.query._original.substring(1)));
                    }else {
                        if ((result = f.matchRow(self)) == false) return false;
                    }
                }
            }
        });
        return result;
    };
})(FooTable);

//https://github.com/fooplugins/FooTable/issues/454
(function ($, F) {

    F.Sum = F.Component.extend(/** @lends FooTable.Sum */{
        /**
		 * @summary The construct method for the component.
		 * @memberof FooTable
		 * @constructs Sum
		 * @param {FooTable.Table} table - The current instance of the plugin.
		 */
        construct: function (table) {
            // call the constructor of the base class
            this._super(table, true);
            /**
			 * @summary A snapshot of the working set of rows prior to being trimmed by the paging component.
			 * @memberof FooTable.Sum#
			 * @name snapshot
			 * @type {Array}
			 */
            this.loadSnapShot = table.$el[0].id == "pendingTransactionTable" ? true : false;

        },
        /**
		 * @summary Initializes the component and holds a reference to the two labels used to display the totals.
		 * @memberof FooTable.Sum#
		 * @function init
		 */
        init: function () {
        },
        /**
		 * @summary Hooks into the predraw pipeline after sorting and filtering have taken place but prior to paging.
		 * @memberof FooTable.Sum#
		 * @function predraw
		 * @description This method allows us to take a snapshot of the working set of rows before they are trimmed by the paging component and is called by the plugin instance.
		 */
        predraw: function () {
            if (this.loadSnapShot)
                PendingTableSnapShot = this.ft.rows.array.slice(0);
        },
        /**
		 * @summary Performs the actual updating of any UI as required.
		 * @memberof FooTable.Sum#
		 * @function draw
		 */
        draw: function () {
        },
        /**
		 * @summary Sums all values of the specified column and returns the total.
		 * @param {string} name - The name of the column to sum.
		 * @param {boolean} [filtered=false] - Whether or not to exclude filtered rows from the result.
		 * @returns {number}
		 */
        column: function (name, filtered) {
            /*
            filtered = F.is.boolean(filtered) ? filtered : false;
            var total = 0, rows = filtered ? this.snapshot : this.ft.rows.all;
            for (var i = 0, l = rows.length, row; i < l; i++) {
                row = rows[i].val();
                total += F.is.number(row[name]) ? row[name] : 0;
            }
            return total;*/
        }
    });

    // register the component using a priority of 450 which falls between filtering (500) and paging (400).
    F.components.register("sum", F.Sum, 450);

})(jQuery, FooTable);