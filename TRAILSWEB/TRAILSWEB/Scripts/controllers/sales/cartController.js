;"use strict";
var shoppingCartController = new function () {
    var _Activity = null;

    this.Products = [];
    /*{
        None: 0,
        AddNew: 1,
        Replace: 2,
        Delete: 3,
        Undo: 4
    }*/

    this.SetActivity = function (activityType) {
        _Activity = activityType;
    };

    this.GetActivity = function () {
        return _Activity;
    };
    /*
    ///Below will be a callback from +/- button clicking on Transponders
    $('.updateCart').cartUpdate({
        callback: function (result) {
           //To do code for adding transponder into list
        }
    });*/

    this.validateCartTransponders = function(event)
    {
        var $myPopupOrPageDiv = $(event.target).closest('.editTransactions')
        if ($myPopupOrPageDiv.length == 0)
            $myPopupOrPageDiv = $('.editTransactions');

        var action = $myPopupOrPageDiv.attr('transponder-role');
        var validationArea = $myPopupOrPageDiv.find('.transponderBodyPanel').find('.SalesTransponderSelection');

        if (action == 'addActivate')
        {
            var isNewCustomer = $myPopupOrPageDiv.attr('account-isNewCustomer');

            if (isNewCustomer == "True" || isNewCustomer == "true")
                {
                $myPopupOrPageDiv = $(event.target).parents().find('.transponderBodyPanel.activateTransponder')
                validationArea = $myPopupOrPageDiv.find('#replaceAndActivateTransponder');
            }
        }


        var isValid = false;
        if (action == 'add' || action == 'Add') {
            isValid = this.validateAddTransponders(validationArea, action);
        }
        else if (action == 'replaceActivate' || action == 'ReplaceActivate' || action == 'addActivate' || action == 'AddActivate') {
            isValid = this.validateActivateTransponders($myPopupOrPageDiv, action);
        }
        else if (action == 'replaceBuy' || action == 'ReplaceBuy')
        {
            isValid = this.validateReplaceTransponders($myPopupOrPageDiv, action);
        }


        return isValid;
    }

    this.validateCartTranspondersFromSave = function (event) {
        var $myPopupOrPageDiv = $('.fillTransponderPopup'); //$(event.target).closest('.fillTransponderPopup')
        if ($myPopupOrPageDiv.length == 0)
            $myPopupOrPageDiv = $('.editTransactions');

        var validationArea = $myPopupOrPageDiv.find('.SalesTransponderSelection');
        var action = validationArea.attr('data-trasponder-role');

        if (action == 'addActivate') {
            var isNewCustomer = $myPopupOrPageDiv.attr('account-isNewCustomer');

            if (isNewCustomer == "True" || isNewCustomer == "true") {
                $myPopupOrPageDiv = $(event.target).parents().find('.transponderBodyPanel.activateTransponder')
                validationArea = $myPopupOrPageDiv.find('#replaceAndActivateTransponder');
            }
        }


        var isValid = false;
        if (action == 'add' || action == 'Add') {
            isValid = this.validateAddTransponders(validationArea, action);
        }
        else if (action == 'replaceActivate' || action == 'ReplaceActivate' || action == 'addActivate' || action == 'AddActivate') {
            isValid = this.validateActivateTransponders($myPopupOrPageDiv, action);
        }
        else if (action == 'replaceBuy' || action == 'ReplaceBuy') {
            isValid = this.validateReplaceTransponders($myPopupOrPageDiv, action);
        }


        return isValid;
    }

    this.validateCartPaymentFromSave = function (event) {
        var $myPopupOrPageDiv = $('.fillPaymentPopup');

        var validationArea = $myPopupOrPageDiv.find('.paymentSection');

        return shoppingCartController.validateCartPaymentArea(validationArea);
    }


    this.validateReplaceTransponders = function (validationArea, action) {

        var selectedTransponder = validationArea.find("input[name='selectTransponder']:checked");
        var isValid = false;
        if (typeof (selectedTransponder) == "undefined" || selectedTransponder.length == 0) {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    message: "To Replace a Transponder, please select a transponder from list."
                },
                {
                    // Settings
                    type: 'danger',
                    delay: 5000,
                    z_index: 3000
                });
            }
        else { isValid = true;}

        return isValid;
    }

      this.validateActivateTransponders = function (validationArea, action) {

        var replaceActivateTranNumber = validationArea.find('#Search_TransponderNumber').val();
        replaceActivateTranNumber = validationArea.find('#Search_TransponderNumber').attr('dataValidated') != 'true' ? "" : replaceActivateTranNumber;
        if (replaceActivateTranNumber == '') {
            $.notify({
                icon: 'fa fa-exclamation-triangle',
                message: "To Activate a Transponder, please enter valid transponder number."
            },
            {
            // Settings
                    type: 'danger',
                        delay: 5000,
                        z_index: 3000
                });
            $('#activateTransponderSpinner').addClass('hidden');
        }

        return (replaceActivateTranNumber != '');
        }

    this.validateAddTransponders = function (validationArea, action) {

        validationArea.wrap('<form id="validationForm" />');
        
        var showErrorCalled = false;
        
        $('#validationForm').validate({
            'rules': {
                'transponderPurchasePlate': {
                    'minlength': 3,
                    'maxlength': 10,
                    'required': true
            }
        },
            'messages': {
                'transponderPurchasePlate': {
                    'minlength': 'Licnese Plate must contain at least {0} characters',
                    'maxlength': 'Licnese Plate can not exceed more than {0} characters',
                    'required': 'Licnese Plate is required.'
            }
        },
            errorElement: 'div',
            wrapper: "div",
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
 
                if (element.parent('.input-group').length) {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
        },
                showErrors: function (errorMap, errorList) {
                    if (!showErrorCalled) {
                        showErrorCalled = true;
                    //alert(errorList.length);
                    $.each(this.successList, function (index, value) {
                        $.notifyClose();
                        $(this).closest('.form-group').removeClass('has-error');
                    });
                    $.each(errorMap, function (key, value) {
                        $('#' +key).closest('.form-group').addClass('has-error');
                        $('#' +key).focus();
                    });
                    //var notifyOld = "";
                    return $.each(errorList, function (index, value) {
                        //if (notifyOld == "" || notifyOld != value.message) {
                            //notifyOld = value.message;
                            $.notify({
                                // Options
                                icon: 'fa fa-exclamation-triangle',
                                message: value.message
                            },
                                {
                                    // Settings
                                    type: 'danger',
                                    delay: 5000,
                                    z_index: 3000
                                });
                        //}
                    });

                }
                
        }
    });


        //Add rules dynamically
        var transponderCounts = 0;
        $('#validationForm').find('input.licensePlate[transponder-data-target!="{0}_{1}"]').each(function () {
            transponderCounts++;
            var traponderType = $(this).closest(".transponderSelection").find("#currentTransponderShortDescr_0").text();
            $(this).rules("add",
                {
                    required: true, 'minlength': 3, 'maxlength': 10,
                    messages: {
                        'minlength': 'Licnese Plate must contain at least {0} characters',
                        'maxlength': 'Licnese Plate can not exceed more than {0} characters',
                        'required': 'Licnese Plate is required for' + traponderType + " Transponder"
                    }
                });
    });

        var isValid = transponderCounts > 0;


        if (!isValid) {

            $.notify({
                // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: "To place an order please add atleast one transponder."
            },
                          {
                              // Settings
                                  type: 'danger',
                              delay: 5000,
                              z_index: 3000
});

            //return false;
    }

        isValid = isValid && $('#validationForm').valid();

        validationArea.unwrap();

        return isValid;
}

    this.validateCartPayment = function (event) {
        var $myPopupOrPageDiv = $(event.target).closest('.editTransactions')

        var validationArea = $myPopupOrPageDiv.find('.transponderBodyPanel').find('.SalesCardPayment');
        
        return shoppingCartController.validateCartPaymentArea(validationArea);
    }

    this.validateCartPaymentArea = function (validationArea) {
        var isCCPayment;
        if (typeof (validationArea.find('li.active')) != 'undefined' && $.trim(validationArea.find('li.active').text()) != "") {

            if ($.trim(validationArea.find('li.active').text()) == "Credit Card") //Check whether CC or ACH Selected
                isCCPayment = true;
            else
                isCCPayment = false;
        }


        validationArea.wrap('<form id="validationForm" />');

        $('#validationForm').validate({
            'rules': {
                'NameOnCard': {
                    'required': isCCPayment,
                    'maxlength': 29
                },
                'CreditCardNumber': {
                    'required': isCCPayment,
                    'creditcard': true,
                    'minlength': 12,
                    'maxlength': 19
                },
                'ExpirationMonth': {
                    'required': isCCPayment
                },
                'ExpirationYear': {
                    'required': isCCPayment,
                    'digits': true/*, 'minStrict' : 2017, 'maxStrict' : 3000*/

                },
                'PrepaidTollsAmount': {
                    'required': true
                },
                'AccountHolderFirstName': {
                    'required': !isCCPayment,  //IF ACH Payment Make it mandatory
                    'maxlength': 50
                },
                'AccountHolderLastName': {
                    'required': !isCCPayment,  //IF ACH Payment Make it mandatory
                    'maxlength': 50
                },
                'RoutingNumber': {
                    'required': !isCCPayment,  //IF ACH Payment Make it mandatory
                    'maxlength': 20, 'digits': true
                },
                'BankAccountNumber': {
                    'required': !isCCPayment,  //IF ACH Payment Make it mandatory
                    'maxlength': 20, 'digits': true
                }
            },
            'messages': {
                'NameOnCard': {
                    'required': 'Please enter the Name as printed on your Credit Card.',
                    'maxlength': 'Name on Card can contain no more than {0} characters'
                },
                'CreditCardNumber': {
                    'required': 'A valid Credit Card Number is required.',
                    'creditcard': 'A valid Credit Card Number is required.',
                    'minlength': 'A Credit Card Number must contain at least {0} characters',
                    'maxlength': 'A Credit Card Number must contain no more than {0} characters'
                },
                'ExpirationMonth': {
                    'required': 'Please select the Month of your card\'s expiration.'
                },
                'ExpirationYear': {
                    'required': 'Please enter the Year of your card\'s expiration.',
                    'digits': 'Card number can only contain numbers',
                    /*'minStrict': 'Please enter the valid year of your card\'s expiration.',
                    'maxStrict': 'Please enter the valid year of your card\'s expiration.'*/
                },
                'PrepaidTollsAmount': {
                    'required': 'Please enter the Amount you wish to pay.'
                },
                'AccountHolderFirstName': {
                    'required': 'Please enter the Bank Account Holder First Name.',
                    'maxlength': 'First Name can contain no more than {0} characters.'
                },
                'AccountHolderLastName': {
                    'required': 'Please enter the Bank Account Holder Last Name.',
                    'maxlength': 'Last Name can contain no more than {0} characters.'
                },
                'RoutingNumber': {
                    'required': 'Please enter the valid bank Routing Number.',
                    'maxlength': 'Routing Number can contain no more than {0} characters.',
                    'digits': 'Routing Number can only contain numbers'
                },
                'BankAccountNumber': {
                    'required': 'Please enter the valid bank Account Number.',
                    'maxlength': 'Account Number can contain no more than {0} characters.',
                    'digits': 'Account Number can only contain numbers'

                }
            },
            errorElement: 'div',
            errorClass: 'help-block',
            errorPlacement: function (error, element) {
                if (element.parent('.input-group').length) {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
            },
            showErrors: function (errorMap, errorList) {
                $.each(this.successList, function (index, value) {
                    $.notifyClose();
                    $(this).closest('.form-group').removeClass('has-error');
                });
                $.each(errorMap, function (key, value) {
                    $('#' + key).closest('.form-group').addClass('has-error');
                    $('#' + key).focus();
                });
                return $.each(errorList, function (index, value) {
                    $.notify({
                        // Options
                        icon: 'fa fa-exclamation-triangle',
                        message: value.message
                    },
                    {
                        // Settings
                        type: 'danger',
                        delay: 5000,
                        z_index: 3000
                    });
                });
            }
        });

        var isValid = $('#validationForm').valid();


        validationArea.unwrap();

        return isValid;
    }
    //This method excepts button associated on Vehicle/Transponder selection screen 
    this.getShopingCartTransponders = function (event) {

       
        //var shoppingDiv = $(event.target).parents('div').find('.TransponderPurchase.transactionEditPanel:not(.PreviewOnPage)');
        var $myPopupOrPageDiv = $(event.target).closest('.editTransactions');
        if ($myPopupOrPageDiv.length == 0)
            $myPopupOrPageDiv = $(event.target).closest('.SalesTransponderSelection'); 
        

            var action = $myPopupOrPageDiv.attr('transponder-role') || $myPopupOrPageDiv.attr('data-trasponder-role');
            var isNewCustomer = "false";
            if (action == 'addActivate')
            {
                var isNewCustomer = $myPopupOrPageDiv.attr('account-isNewCustomer');

                if (isNewCustomer == "True" || isNewCustomer == "true" || isNewCustomer == true)
                    $myPopupOrPageDiv = $(event.target).parents().find('.transponderBodyPanel.activateTransponder')
            }

            var isPaymentRequired = true;
            var isCouponCodeApplied = false;
            isCouponCodeApplied = $myPopupOrPageDiv.find('.CouponCode').attr('isvalidated');
            //Init Transponder list
            var transponderInfo = { transponders: [], transponderAction: action, isNewCustomer: isNewCustomer, isCouponCodeApplied: isCouponCodeApplied };


            if (action == 'add' || action == 'Add') {
                $(".transponder-purchase-details > div > div > div > .licensePlate input[type=text]").each(function () {
                    var $licensePlateText = $(this);
                    var licensePlate = $licensePlateText.val();

                    var transponderTarget = $licensePlateText.attr('transponder-data-target');

                    if (transponderTarget != "{0}_{1}") {
                        var transponderPrice = $licensePlateText.attr('transponder-data-price');

                        var licenseState = $("#transponderPurchasePlateState_" + transponderTarget).val();
                        var transponderToPurchase = new Object();
                        transponderToPurchase.LicensePlate = licensePlate;
                        transponderToPurchase.LicensePlateState = licenseState;
                        transponderToPurchase.TranponderPrice = transponderPrice;
                        transponderToPurchase.TransponderType = transponderTarget.split('_')[0];
                        transponderToPurchase.Reference = "transponderPurchaseDetails_" + transponderTarget;
                        transponderInfo.transponders.push(transponderToPurchase);
                    }


                });
            }
            else if (action == 'replaceBuy' || action == 'ReplaceBuy' || action == 'replaceActivate' || action == 'ReplaceActivate') {
                var licensePlateNumber = $myPopupOrPageDiv.attr('transData-licensePlateNumber');
                var licensePlateState = $myPopupOrPageDiv.attr('transData-licensePlateState');
                var transponderNumber = $myPopupOrPageDiv.attr('transData-transponderNumber');

                var existingMake = $myPopupOrPageDiv.attr('transData-vehicleMake');
                var existingModel = $myPopupOrPageDiv.attr('transData-vehicleModel');
                var existingYear = $myPopupOrPageDiv.attr('transData-vehicleYear');
                var existingColor = $myPopupOrPageDiv.attr('transData-vehicleColor');

                var selectedTransponder = $myPopupOrPageDiv.find("input[name='selectTransponder']:checked");
                //$(e.target).parents().find('.payableInformation').attr("data-paymentRequired");
                var transponderPrice;
                var transponderType;
                if (typeof (selectedTransponder) != "undefined") {
                    transponderPrice = selectedTransponder.attr('transponder-data-price');
                    transponderType = selectedTransponder.attr('transponder-data-type');
                }

                var transponderToPurchase = new Object();
                transponderToPurchase.LicensePlate = licensePlateNumber;
                transponderToPurchase.LicensePlateState = licensePlateState;
                transponderToPurchase.TranponderPrice = transponderPrice;
                transponderToPurchase.TransponderType = transponderType;
                transponderToPurchase.Reference = "ReplaceTransponder";
                transponderToPurchase.TransponderNumber = transponderNumber;
                transponderToPurchase.VehicleMake = existingMake;
                transponderToPurchase.VehicleModel = existingModel;
                transponderToPurchase.VehicleYear = existingYear;
                transponderToPurchase.VehicleColor = existingColor;

                if (action == 'replaceActivate' || action == 'ReplaceActivate') {
                    var replaceActivateTranNumber = $myPopupOrPageDiv.find('#Search_TransponderNumber').val();
                    var replaceActivateTranNumberType = $myPopupOrPageDiv.find('#currentTransponderType_0').html();

                    transponderToPurchase.TransponderNumberToReplace = replaceActivateTranNumber;
                    transponderToPurchase.TransponderType = replaceActivateTranNumberType;
                }

                transponderInfo.transponders.push(transponderToPurchase);

            }
            else if (action == 'addActivate' || action == 'AddActivate')
            {
                   var transponderToPurchase = new Object();

                    var replaceActivateTranNumber = $myPopupOrPageDiv.find('#Search_TransponderNumber').val();
                    var replaceActivateTranNumberType = $myPopupOrPageDiv.find('#currentTransponderType_0').html();
                    var licensePlate = $myPopupOrPageDiv.find('.licensePlate input[type=text]').val();
                    var licensePlateState = $myPopupOrPageDiv.find("#transponderPurchasePlateState_0_1").val();

                    transponderToPurchase.TransponderNumber = replaceActivateTranNumber;
                    transponderToPurchase.TransponderType = replaceActivateTranNumberType;
                    transponderToPurchase.licensePlate = licensePlate;
                    transponderToPurchase.licensePlateState = licensePlateState;


                 transponderInfo.transponders.push(transponderToPurchase);
            }

            return transponderInfo;
        
    };

    this.getShoppingPaymentInformation = function (event) {

        var shoppingDiv = $(event.target).parents('.transponderBodyPanel');//.closest('.TransponderPurchase');
        
        var transponderRole = shoppingDiv.attr('data-trasponder-role');

        var $myPopupOrPageDiv;
        if (transponderRole == 'add') {
            $myPopupOrPageDiv = $(event.target).closest('.editTransactions');//$('#AddTransponder_Modal');
            if ($myPopupOrPageDiv.length == 0)
                $myPopupOrPageDiv = $('#addTransponderEditPage');
        }
        else if (transponderRole == 'replace') {
            $myPopupOrPageDiv = $('#ReplaceTransponders_Modal');
        }
        else if (transponderRole == 'activate') {
            $myPopupOrPageDiv = $('#addTransponderEditPage');
            if ($myPopupOrPageDiv.find('li.active').text() == "")
                $myPopupOrPageDiv = $(event.target).closest('.editTransactions');
        }

        //Get sales card div
        var $mySalesCardDiv = shoppingDiv;// $myPopupOrPageDiv.find('.transponderBodyPanel').find('.SalesCardPayment');
        /*
        var $myPopupOrPageDiv = $('.editTransactions.PreviewOnPage');
        if ($myPopupOrPageDiv.length == 0)
            $myPopupOrPageDiv = $('.editTransactions:not(.PreviewOnPage)');
        */

        var action = $myPopupOrPageDiv.attr('transponder-role');

        var isCCPayment = true;
        var nameOnCard = "", cardNumber = "", cardType = "", expirationMonth = "", expirationYear = "", bankActNumber = "", bankRoutingNumber = "", bankActFirstName = "", bankActLastName = "";
        var saveCard = false, autoReplenish = false;
        var prepaidTollsAmount = 10, paymentAmount = 10, salesTax = 0, transponderPrice = 0, minimumBalancePay = 0, discountPrice = 0;
        var couponCode = '';
        var isPaymentRequired = true;
        var useCardOnFile = false;
        var webBalance = 0;
        var lowBalanceAmount = 0, replenishAmount = 0;
        
        if (typeof ($mySalesCardDiv.find('li.active')) != 'undefined') {

            if ($.trim($mySalesCardDiv.find('li.active').text()).indexOf("Credit Card") >= 0) //Check whether CC or ACH Selected
                isCCPayment = true;
            else
                isCCPayment = false;

            if (isCCPayment) {

                nameOnCard = $mySalesCardDiv.find('#NameOnCard').val();
                useCardOnFile = $mySalesCardDiv.find('.useCardOnFileSelection').prop('checked');
                cardNumber = (useCardOnFile == "true" || useCardOnFile == true) ? $mySalesCardDiv.find('.hiddenCardNumber').val() : $mySalesCardDiv.find('#CreditCardNumber').val();
                expirationMonth = $mySalesCardDiv.find('#ExpirationMonth').val();
                expirationYear = $mySalesCardDiv.find('#ExpirationYear').val();
                cardType = $mySalesCardDiv.find('#CreditCardType').val();//                'CardType': $('#calcCardType').html(),

                saveCard = (useCardOnFile == "true" || useCardOnFile == true) ? false : $mySalesCardDiv.find('.SaveCardOnFile').prop('checked');
                autoReplenish = $mySalesCardDiv.find('.AutoReload').prop('checked');

                if (autoReplenish)
                {
                    lowBalanceAmount = $mySalesCardDiv.find('#paymentLowAmount').val();
                    replenishAmount = $mySalesCardDiv.find('#paymentRepAmount').val();
                }
            }
            else {

                bankActFirstName = $mySalesCardDiv.find('#AccountHolderFirstName').val();
                bankActLastName = $mySalesCardDiv.find('#AccountHolderLastName').val();
                bankActNumber = $mySalesCardDiv.find('#BankAccountNumber').val();
                bankRoutingNumber = $mySalesCardDiv.find('#RoutingNumber').val();
            }

            if (action == "replaceActivate" || action == "ReplaceActivate" || action == 'addActivate' || action == 'AddActivate' || action == 'replaceBuy' || action == 'Replacebuy')
            {
                isPaymentRequired = $mySalesCardDiv.find('.payableInformation').attr("data-paymentRequired") == "true" || $(event.target).parents().find('.payableInformation').attr("data-paymentRequired") == "True";
            }
            webBalance = $(event.target).parents().find('.payableInformation').attr("data-webBalance");
            //isPaymentRequired = isPaymentRequired || webBalance > 0;

            transponderPrice = $mySalesCardDiv.find("#lblTransponderPrice").text();
            discountPrice = $mySalesCardDiv.find("#lblTotalDiscount").text();
            
            salesTax = $mySalesCardDiv.find("#lblSalesTax").text();
            prepaidTollsAmount = $mySalesCardDiv.find("#PrepaidTollsAmount").val();
            couponCode = $mySalesCardDiv.find("#CouponCode").val();
            minimumBalancePay = shoppingDiv.find("#minimumToll").text();
            paymentAmount = Number(transponderPrice) + Number(salesTax) + Number(prepaidTollsAmount) + Number(minimumBalancePay) -Number(discountPrice); //No space in $ sign as it brings in next line on mobile devices

            if((action == 'replaceBuy' || action == 'Replacebuy') && !isPaymentRequired && Number(transponderPrice) > 0)
                isPaymentRequired = true;


            if (!isPaymentRequired && transponderPrice <= 0)
                paymentAmount = salesTax = prepaidTollsAmount = minimumBalancePay = transponderPrice = 0;

        }

        //Return Payment Information JSON Object
        return {
            'IsCCPayment': isCCPayment,
            'NameOnCard': nameOnCard,
            'CreditCardNumber': cardNumber,
            'CreditCardType': cardType,
            'ExpirationMonth': expirationMonth,
            'ExpirationYear': expirationYear,
            'AutoReload': autoReplenish,
            'SaveCardOnFile': saveCard,
            'AccountHolderFirstName': bankActFirstName,
            'AccountHolderLastName': bankActLastName,
            'BankAccountNumber': bankActNumber,
            'RoutingNumber': bankRoutingNumber,
            'PaymentAmount': paymentAmount,
            'PrepaidTollsAmount': prepaidTollsAmount,
            'TransponderPrice': transponderPrice,
            'SalesTax': salesTax,
            'IsPaymentRequired': isPaymentRequired,
            'UseCardOnFile': useCardOnFile,
            'WebBalance': webBalance,
            'LowBalanceAmount': lowBalanceAmount,
            'ReplenishAmount': replenishAmount,
            'CouponCode': couponCode,
            'IsCouponCodeApplied': 'true'
        }
    }

    this.updateShoppingPaymentInformation = function (event, paymentData)
    {
        //var shoppingDiv = $(event.target).closest('.payableInformation');
        
        var $myPopupOrPageDiv = $(event.target).parents('.transponderBodyPanel'); // || ;//.closest('.TransponderPurchase');
        
        if ($myPopupOrPageDiv.length == 0)
            $myPopupOrPageDiv = $('.transponderBodyPanel');

        var transponderRole = $myPopupOrPageDiv.attr('data-trasponder-role');
        /*
        var $myPopupOrPageDiv;
        if (transponderRole == 'add') {
            $myPopupOrPageDiv = $(event.target).closest('.editTransactions');//$('#AddTransponder_Modal');
            if ($myPopupOrPageDiv.length == 0)
                $myPopupOrPageDiv = $('#addTransponderEditPage');
        }
        else if (transponderRole == 'replace') {
            $myPopupOrPageDiv = $('#ReplaceTransponders_Modal');
        }
        else if (transponderRole == 'activate') {
            $myPopupOrPageDiv = $('#addTransponderEditPage');
            if ($myPopupOrPageDiv.find('li.active').text() == "")
                $myPopupOrPageDiv = $(event.target).closest('.editTransactions');
        }

        if ($myPopupOrPageDiv.length == 0){
            $myPopupOrPageDiv = $(event.target).parents().find('.TransponderPurchase').find('.payableInformation');
        }
        */

       

        var prepaidTollsAmount = 0, paymentAmount = 0, salesTax = 0, transponderPrice = 0, minimumBalancePay = 0;
        var isPaymentRequired = true;
        var webBalance = 0;

        webBalance = $myPopupOrPageDiv.find('.payableInformation').attr("data-webBalance");

        $myPopupOrPageDiv.find("#lblTransponderPrice").text(paymentData.totalSalesAmount.toFixed(2));
        $myPopupOrPageDiv.find("#lblTotalDiscount").text(paymentData.totalDiscountAmount.toFixed(2));
        
        $myPopupOrPageDiv.find("#lblSalesTax").text(paymentData.totalSalesTax.toFixed(2));
        prepaidTollsAmount = Number($myPopupOrPageDiv.find("#PrepaidTollsAmount").val());
        minimumBalancePay = Number($myPopupOrPageDiv.find("#minimumToll").text());

        paymentAmount = Number(paymentData.totalSubAmount) + Number(prepaidTollsAmount) +Number(minimumBalancePay); //No space in $ sign as it brings in next line on mobile devices

        $myPopupOrPageDiv.find("#subTotal").text(Number(paymentData.totalSubAmount).toFixed(2));

        //transFinalAmount = transFinalAmount.toFixed(2) == "-0.00" ? 0 : Number(transFinalAmount.toFixed(2));
        $myPopupOrPageDiv.find("#paymentAmountLabel").text('$' + Number(paymentAmount).toFixed(2)); //No space in $ sign as it brings in next line on mobile devices
        $myPopupOrPageDiv.find('.CouponCode').attr('IsValidated', paymentData.isValidCouponCode);

        if (($.trim($("#replaceTypeTabs .active a").attr('href')) == "#replaceAndActivateTransponder" && Number(webBalance) <= 0) || $.trim($("#replaceTypeTabs .active a").attr('href')) == "#replaceAndBuyTransponder")
            $myPopupOrPageDiv.find('.paymentSection').show();
    }

    this.validatePaymentData = function(shoppingDiv, manageData)
    {
        var isValid = true
        var errorMessage = "";
        if (typeof (manageData) == "undefined")
            isValid = false;

        if (isValid && manageData.IsNewCustomer == "true")
        {

            if (manageData.TransponderAction == "add" || manageData.TransponderAction == "Add")
            {

                if (manageData.Payment.PrepaidTollsAmount < (manageData.Transponders.length * 10))
                {
                    errorMessage = "Please enter prepaid tolls larger than $ " + (manageData.Transponders.length * 10).toFixed(2);
                    shoppingDiv.find("#PrepaidTollsAmount").focus();
                    isValid = false;
                }
            }
            else if ((manageData.TransponderAction == "replaceBuy" || manageData.TransponderAction == "ReplaceBuy" || manageData.TransponderAction == "replaceActivate" || manageData.TransponderAction == "ReplaceActivate")
                && Number(manageData.Payment.WebBalance) < 0)
            {
                errorMessage = "Please enter prepaid tolls larger than $ 10.00";
                shoppingDiv.find("#PrepaidTollsAmount").focus();
                isValid = false;
            }
            else if ((manageData.TransponderAction == 'addActivate' || manageData.TransponderAction == 'AddActivate') && Number(manageData.Payment.WebBalance) <= 0 && manageData.Payment.PrepaidTollsAmount < (manageData.Transponders.length * 10))
            {
                errorMessage = "Please enter prepaid tolls larger than $ 10.00";
                shoppingDiv.find("#PrepaidTollsAmount").focus();
                isValid = false;
            }

/*
            manageData = {
                'Transponders': transpondersInfoToPurchase.transponders,
                'TransponderAction': transpondersInfoToPurchase.transponderAction,
                'Payment': paymentForPurchase,
                'Customer': customerInfo,
                'Security': securityInfo,
                'SourcePage': '',
                'IsNewCustomer': isNewCustomer,
                'HasAPopup': isPopup,
                'HasCustomerData': hasCustomerData,
                'HasSecurityData': hasSecurityData,
            }
            */
        }

        if(!isValid)
        {
            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: errorMessage
            },
            {
                // Settings
                type: 'danger',
                delay: 5000,
                z_index: 3000
            });

            return false;
        }

        return isValid;

    }

    // this call seems to be triggered from the Activate page and "Activate Transponder" button.
    this.previewTransaction = function (event) {        
        event.preventDefault();

        $('#activateTransponderSpinner').removeClass('hidden');

        var validForm = this.validateCartTransponders(event);

        if (validForm)
            validForm = this.validateCartPayment(event);

        if (validForm) {

            var shoppingDiv = $(event.target).parents('.transponderBodyPanel');//$(event.target).parents('div').find('.TransponderPurchase.transactionEditPanel:not(.PreviewOnPage)');
            var transpondersInfoToPurchase = shoppingCartController.getShopingCartTransponders(event);
            var paymentForPurchase = shoppingCartController.getShoppingPaymentInformation(event);
            var $myPopupOrPageDiv = $(event.target).closest('.editTransactions');

            var isPopup = $("#IsPopup").val();
            var isNewCustomer = $myPopupOrPageDiv.attr('account-isNewCustomer');//$("#IsNewCustomer").val();

            var securityInfo = {};
            var customerInfo = {};
            var hasCustomerData = false, hasSecurityData = false;
            if (isNewCustomer == "True" || isNewCustomer == "true" || isNewCustomer == true) //Get Customer and Security information from Account Controller
            {
                if (typeof (accountController) != "undefined") {
                    securityInfo = accountController.getAccountLoginInfo();
                    customerInfo = accountController.getCustomerDataInfo();
                    hasCustomerData = true;
                    hasSecurityData = true;

                    var existingEmail = $(".emailInSecurity").val();//;
                    var enteredEmail = $(".emailInContact.showOnlyOnPopup").val();
                    if (enteredEmail == "")
                    {
                        $(".emailInContact.showOnlyOnPopup").val(existingEmail); //emailInSecurity
                        enteredEmail = existingEmail;
                    }
                    else if(enteredEmail != existingEmail)
                    {
                        $(".emailInSecurity").val(enteredEmail); //emailInSecurity
                    }

                    securityInfo.Email = enteredEmail;
                        
                }
            }


            var manageData = {
                'Transponders': transpondersInfoToPurchase.transponders,
                'TransponderAction':transpondersInfoToPurchase.transponderAction,
                'Payment': paymentForPurchase,
                'Customer': customerInfo,
                'Security': securityInfo,
                'SourcePage': '',
                'IsNewCustomer': isNewCustomer,
                'HasAPopup': isPopup,
                'HasCustomerData': hasCustomerData,
                'HasSecurityData': hasSecurityData,
            }

            validForm = this.validatePaymentData(shoppingDiv, manageData);
        
            if(validForm)
            {                
                return this.MakePreviewCall(event,$myPopupOrPageDiv, manageData);
            }            
        }
        
        return validForm;
    };

    this.MakePreviewCall = function (event, $myPopupOrPageDiv, manageData)
    {
        // this is the call for the "Order Transponder" button on the GetEpass page              

        var manageUrl = getBaseUrl() + '/Manage/ReviewTransaction';
        //var action = transpondersInfoToPurchase.transponderAction;
        // Display Spinner
        //$(event.target).addClass('fa-spinner fa-spin animated');
        $(event.target).attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: manageUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'html',
            encode: true, async: false
        }).then(function (response, action) {

            //$(event.target).removeClass('fa-spinner fa-spin animated');
            $(event.target).removeAttr('disabled');
            hideSpinner();
            if (typeof (response) != 'undefined' && response.success != false) {
                if (response.indexOf('"success":false') > 0) {
                    response = $.parseJSON(response);

                    if (typeof (response.success) != 'undefined' && !response.success) {

                        if (typeof (response.dataFormat) && response.dataFormat == "text") { // Display Error in text
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
                        else if (typeof (response.dataFormat) && response.dataFormat == "list") {
                            //display error from list

                            $.each(response.errors, function (k, v) {
                                $.notify({
                                    // Options
                                    icon: 'fa fa-exclamation-triangle',
                                    message: this.Reference == "" || this.Reference == "null" || this.Reference == null ? this.Message : this.Message + " in " +this.Reference
                                },
                                           {
                                               // Settings
                                               type: 'danger',
                                               delay: 5000,
                                               z_index: 3000
                                           });
                            });/*
                                response.errors.each(function () {
                                    $.notify({
                                        // Options
                                        icon: 'fa fa-exclamation-triangle',
                                        message: this.Message + " in " + this.Reference
                                    },
                                    {
                                        // Settings
                                        type: 'danger',
                                        delay: 5000,
                                        z_index: 3000
                                    });
                                });*/
                        }

                    }

                }
                else {
                    if (typeof (response.message) == 'undefined') {

                        //Below code will add Partial view dynamically
                        $myPopupOrPageDiv.find(".transponderBodyPanel").find(".transactionReviewPanelData").html(response);

                        //Below code will load a JS script dynamically
                        //We will avoid it as this script has been added from main page
                        /*
                        $.getScript("Scripts/partialViews/_PreviewTransaction.js", function (data, textStatus, jqxhr) {
                            console.log('Script loaded');
                            window.foo = true;
        
                        });
                        */


                        $myPopupOrPageDiv.find(".transponderBodyPanel").find(".transactionReviewPanel").show();
                        $myPopupOrPageDiv.find(".transponderBodyPanel").find(".transactionEditPanel").hide();
                        $myPopupOrPageDiv.parents().find(".addTransponder.transactionEditPanel.activateTransponder.transponderBodyPanel:not(.extraPanel)").hide();

                        $myPopupOrPageDiv.addClass('PreviewOnPage');

                        //If new Customer bring window back to Top, this is to provide a new page expierence
                        //if (isNewCustomer == true || isNewCustomer == "True")
                        $(window).scrollTop(0);
                    }
                }
            }
            else {
                //do nothing


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
                setTimeout(window.location.reload(true), 5000);
            }

        })
        .catch(function (error) {
            //$(event.target).removeClass('fa-spinner fa-spin animated');
            hideSpinner();
            $(event.target).removeAttr('disabled');
            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: error
            },
                 {
                     // Settings
                     type: 'danger',
                     delay: 5000,
                     z_index: 3000
                 });
            setTimeout(window.location.reload(true), 5000);
        });

        //if (isNewCustomer == "True" || isNewCustomer == "true" || isNewCustomer == true) {
        $(".PageTitleDetails").text("Review Order");
        $(".PageTagline").text("Please review all account information for accuracy and click submit to complete your E-PASS account creation. ");
        //}
        //ProgressBar Step 4
        $(".ProgressBarSteps").removeClass("step-3");
        $("#paymentStep .step-circle").addClass("complete");
        $(".ProgressBarSteps").addClass("step-4");
        $("#reviewStep .step-circle").addClass("active");
        return true;
    }

    this.SubmitTransaction = function (event) {
        event.preventDefault();
        
        //var shoppingDiv = $(event.target).parents('div').find('.TransponderPurchase.transactionEditPanel:not(.PreviewOnPage)');
        var transpondersToPurchase; // = shoppingCartController.getShopingCartTransponders(event);
        var paymentForPurchase; // = shoppingCartController.getShoppingPaymentInformation(shoppingDiv);
        var $myPopupOrPageDiv = $(event.target).closest('.editTransactions');


        var isPopup = $("#IsPopup").val();
        var isNewCustomer = $myPopupOrPageDiv.attr('account-isNewCustomer');//$("#IsNewCustomer").val();
        var successResponse = false;

        var securityInfo = {};
        var customerInfo = {};

        if (isNewCustomer == "true" || isNewCustomer == "True" || isNewCustomer == true) //Get Customer and Security information from Account Controller
        {
            if (typeof (accountController) != "undefined") {
                //securityInfo = accountController.getAccountLoginInfo();
                //customerInfo = accountController.getCustomerDataInfo();
            }
        }

        var manageData = {
            'Transponders': transpondersToPurchase,
            'Payment': paymentForPurchase,
            'Customer': customerInfo,
            'Security': securityInfo,
            'SourcePage': '',
            'IsNewCustomer': isNewCustomer
        }


        var submitDataUrl = getBaseUrl() + '/Manage/SaveTransaction';

        $.ajax({
            type: 'POST',
            url: submitDataUrl,
            data: JSON.stringify(manageData),
            contentType: 'application/json',
            dataType: 'html',
            encode: true, async: true,
        }).then(function (response) {
            if (typeof (response) != 'undefined' && response.success != false) {
                if (response.indexOf('"success":true,"dataFormat":"url"') > 0 || response.indexOf('"success":false,"dataFormat":"text"') > 0) {

                    response = $.parseJSON(response);

                    if (typeof (response.success) != 'undefined' && response.success) {

                        if (response.dataFormat == 'html') {
                            //Below code will add Partial view dynamically
                        }
                        else if (typeof (response.Url) != "undefined") {
                            successResponse = true;
                            window.location.href = response.Url;
                        }
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
                        //setTimeout(window.location.reload(true), 5000);

                    }

                }
                else {
                    successResponse = true;
                    $myPopupOrPageDiv.find(".transactionReviewPanel").hide();
                    //TDOO: Remove comments below
                    //$myPopupOrPageDiv.find(".transactionEditPanel").html(''); //Empty the review and edit transactions to avoid potential DOM menuplation
                    //$myPopupOrPageDiv.find(".transactionReviewPanel").html(''); //Empty the review and edit transactions to avoid potential DOM menuplation
                    $myPopupOrPageDiv.find(".transactionConfirmationPanelData").html(response);
                    $myPopupOrPageDiv.find(".transactionConfirmationPanel").show();
                    $myPopupOrPageDiv.attr("data-refresh", "true"); //Refresh the page on closing of confirmation popup

                }
            }
            else { //if (response.success) == 'false') {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: "There is an error sending your request, please try again."
                },
                            {
                                // Settings
                                type: 'danger',
                                delay: 5000,
                                z_index: 3000
                            });
                //setTimeout(window.location.reload(true), 5000);
            }
        })
        .catch(function (error) {
            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: "There is an error sending your request, please try again."
            },
                            {
                                // Settings
                                type: 'danger',
                                delay: 5000,
                                z_index: 3000
                            });
            //setTimeout(window.location.reload(true), 5000);
        });

        return successResponse;
    };

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
};

$(function () {
    // Bind Body Click Event in order to track "Remove" Button
    // This is necessary to properly work with Table Pagination
    $('body').click(function (event) {
        var source;

        // Determine Source Element
        if (event.srcElement) {
            source = event.srcElement;
        }
        else if (event.target) {
            source = event.target;
        }

        if ($(source).is('i')) {
            source = $(source).parent();
        }

        // Process Tag Replace
        if ($(source).hasClass('removeItemButton')) {
            event.preventDefault();

            var licensePlate = $(source).attr('data-license-plate');

            var itemRemoved = shoppingCartController.RemoveItem(licensePlate);

            if (itemRemoved === true) {
                // Get a reference to the Table
                var footable = $('#shoppingCart'); //.data('footable'); // $(row).closest('table').data('footable');

                // Determine Item Row
                var row = $(source).closest('tr');

                // Remove Row from Cart
                footable.removeRow(row);
            }
        }

        return true;
    });

    //----------------------------------------
    //Global event trigger to PreviewTransaction and display on screen/popup
    $(document).on("click", ".previewTransactionButton", function (event) {        
        event.preventDefault();
        event.stopPropagation();
        
        var validForm = shoppingCartController.previewTransaction(event);
        hideSpinner();
        return validForm;
    });

    $(document).on("click", ".submitTransactionButton.ajaxButton", function (event) {
        event.preventDefault();
        event.stopPropagation();

        var validForm = shoppingCartController.SubmitTransaction(event);        
    });


    

    //------------------------------------


    // **** Tooltip and callouts ***//

    // Define Base Popover Template
    var popoverBaseTemplate = '<div class="popover"><div class="arrow"></div><h3 class="popover-title {Style}"></h3><div class="popover-content"></div></div>';
    var popoverTollTemplate = '<div class="popover" style="width:300px!important"><div class="arrow"></div><h3 class="popover-title {Style}"></h3><div class="popover-content"></div></div>';

    // Configure Styled Popover Templates
    var popoverInfoTemplate = popoverBaseTemplate.replace('{Style}', 'info');
    var popoverWarningTemplate = popoverBaseTemplate.replace('{Style}', 'warning');
    var popoverErrorTemplate = popoverBaseTemplate.replace('{Style}', 'danger');

    var popoverInfoTemplateLarge = popoverTollTemplate.replace('{Style}', 'info');

    // Configure Reload and Low Balance Help Content
    var reloadAmountHelpInfo = "<p>Reload Amount is the amount your credit card on file will be charged when your balance reaches your low balance amount.</p>";
    var lowBalanceAmountHelpInfo = "<p>Low Balance Amount is the dollar amount at which your account requires additional funds.</p>";
    //var paymentAmountHelpInfo = "<p>A Minimum Payment amount of $10 per transponder is required.</p>";
    var paymentAmountHelpInfo = "<p>New E-PASS customers, a minimum of $10 per transponder in prepaid tolls is required to activate a new E-PASS account. Each time you pass through a toll point, E-PASS will automatically deduct funds from your account balance to pay the required tolls.</p> " +
        "<p>If you are an existing customer, a minimum of $10 is required to activate a new <br />E-PASS if your current account balance is below $10 in prepaid tolls.</p>" +
        "<p>There is no minimum amount if you are purchasing a new transponder and your account is above the low balance amount.</p>";
    var promotionHelpInfo = "<p>If you have coupon, enter it to get discount on your transponder purchase. </p>";
    var automaticBillingHelpInfo = "<p>An amount of $40.00 for one transponder plus $15.00 for each additional transponder will be automatically added to your account using the credit card on file when your E-PASS account balance reaches your low balance amount.</p>";


    $('body').popover({
        selector: '.automaticBillingHelp',
        placement: 'top',
        template: popoverInfoTemplateLarge,
        title: 'Automatic Billing',
        content: automaticBillingHelpInfo,
        html: true
    });

    $('body').on('click', '#automaticBillingHelp', function (e) {
        $(this).popover({
            placement: 'top',
            template: popoverInfoTemplateLarge,
            title: 'Automatic Billing',
            content: automaticBillingHelpInfo,
            html: true,
        });
    });

    $('#secondaryUserInfo').popover({
        placement: 'top',
        template: popoverInfoTemplate,
        title: 'Add secondary user',
        content: 'Add a second user that is authorized to make changes to the account.',
        html: true
    });

    //PIN Number Tooltip
    $('#pinNumberInfo').popover({
        placement: 'top',
        template: popoverInfoTemplate,
        title: 'Create Pin Number',
        content: 'A unique 4 digit Personal Identification Number (PIN) used for account recovery. Pin must not start with 0.',
        html: true
    });


    $('#bankAccountHelp').popover({
        placement: 'top',
        template: popoverInfoTemplate,
        title: 'Account Number',
        content: 'Account number is usually to the left of the <B>⑈</B> symbol on your bank check',
        html: true
    });

    // Configure Payment Amount Help Popover
    $('#bankRoutingHelp').popover({
        placement: 'top',
        template: popoverInfoTemplate,
        title: 'Routing Number',
        content: 'Routing Number is usually 9 digits number between the <B>⑆</B> symbols on your bank check.',
        html: true
    });


    $('body').on('click', '.bankAccountHelp', function (e) {
        $(this).popover({
            placement: 'top',
            template: popoverInfoTemplateLarge,
            title: 'Account Number',
            content: 'Account number is usually to the left of the <B>⑈</B> symbol on your bank check',
            html: true,
        });
    });

    $('body').on('click', '.bankRoutingHelp', function (e) {
        $(this).popover({
            placement: 'top',
            template: popoverInfoTemplateLarge,
            title: 'Automatic Billing',
            content: 'Routing Number is usually 9 digits number between the <B>⑆</B> symbols on your bank check.',
            html: true,
        });
    });
    
    $('body').popover({
        selector: '.paymentAmountHelp',
        placement: 'top',
        template: popoverInfoTemplateLarge,
        title: 'Minimum Payment',
        content: paymentAmountHelpInfo,
        html: true
    });

    // Configure Promotion Amount Help Popover
    $('body').popover({
        selector: '.promotionHelp',
        placement: 'top',
        template: popoverInfoTemplateLarge,
        title: 'Coupon Code',
        content: promotionHelpInfo,
        html: true
    });

    $('body').on('click', '.promotionHelp', function (e) {
        $(this).popover({
            placement: 'top',
            template: popoverInfoTemplateLarge,
            title: 'Coupon Code',
            content: promotionHelpInfo,
            html: true
        });
    });

    $('body').on('click','.paymentAmountHelp',  function (e) {
        $(this).popover({
            placement: 'top',
            template: popoverInfoTemplateLarge,
            title: 'Minimum Payment',
            content: paymentAmountHelpInfo,
            html: true,
        });
    });
    
    $('body').on('click', '.minimumPayment', function (e) {
        var minimumTollAmount = $(this).attr('minimumTollAmount');
        $(this).popover({
            placement: 'top',
            template: popoverInfoTemplateLarge,
            title: 'Minimum Payment',
            content: "<p>Amount of $" + minimumTollAmount + " is added to your final payment amount to match the account balance with minimum account balance amount.</p>",
            html: true,
        });
    });


    $('body').on('click', '#applyPromoCoupon', function (event) {
        var couponValidationURL = getBaseUrl() + '/Manage/ValidateCouponCode';
        //var action = transpondersInfoToPurchase.transponderAction;
        // Display Spinner
        //$(event.target).addClass('fa-spinner fa-spin animated');

        var transpondersToPurchase = shoppingCartController.getShopingCartTransponders(event);
        if (transpondersToPurchase.transponders.length == 0) {
            var messag = "Please select atleast one transponder before you apply coupon";
            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: messag
            },
                      {
                          // Settings
                          type: 'danger',
                          delay: 5000,
                          z_index: 3000
                      });
            return;
        }

        var couponCode = $("#CouponCode").val();
        if (couponCode == null || couponCode == "" || typeof (couponCode) == "undefined")
        {
            var tempPaymentData = shoppingCartController.getShoppingPaymentInformation(event);
            couponCode = tempPaymentData.CouponCode;
        }

        var couponCodeData = {
            'Transponders': transpondersToPurchase.transponders,
            'IsNewCustomer': transpondersToPurchase.isNewCustomer,
            'CouponCode': couponCode, //"1234548", 
            'IsCouponCodeApplied': 'true'
        }

        $(event.target).attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: couponValidationURL,
            data: JSON.stringify(couponCodeData),
            contentType: 'application/json',
            dataType: 'html',
            encode: true, async: false
        }).done(function (response, action) {

            //Get all transponders and for specific type apply the coupon code.
            var parsedResponse = $.parseJSON(response);

            if (parsedResponse.success ) {// && parsedResponse.isValidCouponCode) {
                $("#invalidCouponLabel").hide();
                shoppingCartController.updateShoppingPaymentInformation(event, parsedResponse);
            }
            else {
                $("#invalidCouponLabel").show();
                $("#invalidCouponLabel").text(parsedResponse.message);
                //$("#invalidCouponLabel").innerHTML = parsedResponse.message;
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: parsedResponse.message
                },
                      {
                          // Settings
                          type: 'danger',
                          delay: 5000,
                          z_index: 3000
                      });
            }

            $(event.target).removeClass('fa-spinner fa-spin animated');
            $(event.target).removeAttr('disabled');

        })
        .fail(function(error) {
            //$(event.target).removeClass('fa-spinner fa-spin animated');
            hideSpinner();
            $(event.target).removeAttr('disabled');
            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: error
            },
                 {
                     // Settings
                     type: 'danger',
                     delay: 5000,
                     z_index: 3000
                 });
            setTimeout(window.location.reload(true), 5000);
        });
    });


    /*
    $('body').on('click', '#Add_AutoReload', function (event) {
        var url = window.location.pathname;

        var checked = $('#Add_AutoReload').is(":checked");

        var paymentDiv;
        paymentDiv = $(event.target).closest(".transponderBodyPanel"); //.find("#paymentRepAmount")

        if (typeof (paymentDiv) != "undefined")
        {
            if (checked) {
                paymentDiv.find("#divReplenishAmount").removeClass("hidden");
                allowAutomaticBillingDataUpdate($(event.target));
            } else {
                paymentDiv.find("#divReplenishAmount").addClass("hidden");
            }
        }
        else
            {
                if (checked) {
                    $("#divReplenishAmount").removeClass("hidden");
                    allowAutomaticBillingDataUpdate($(event.target));
                } else {
                    $("#divReplenishAmount").addClass("hidden");
                }
        }

    });

    $('body').on('click', '#Replace_AutoReload', function (event) {
        var url = window.location.pathname;

        var checked = $('#Replace_AutoReload').is(":checked");

        var paymentDiv;
        paymentDiv = $(event.target).closest(".transponderBodyPanel"); //.find("#paymentRepAmount")

        if (typeof (paymentDiv) != "undefined") {
            if (checked) {
                paymentDiv.find("#divReplenishAmount").removeClass("hidden");
                allowAutomaticBillingDataUpdate($(event.target));
            } else {
                paymentDiv.find("#divReplenishAmount").addClass("hidden");
            }
        }
        else {
            if (checked) {
                $("#divReplenishAmount").removeClass("hidden");
                allowAutomaticBillingDataUpdate($(event.target));
            } else {
                $("#divReplenishAmount").addClass("hidden");
            }
        }

       
    });
   
    $('body').on('blur', '#paymentRepAmount', function (event) {
        var inputAmount = parseFloat($(this).val());
        var labelAmount = parseFloat($(event.target).closest('#divReplenishAmount').find('#lblMinReplAmt').text().replace('$', ''));

        if (inputAmount < labelAmount) {
            $(event.target).focus();
            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: 'The amount entered cannot be < ' + Math.round(labelAmount) + ' -> ' + this.id
            },
        {
            // Settings
            type: 'warning',
            delay: 3000,
            z_index: 3000
        });
        }
    });
    
    $('body').on('blur', '#paymentLowAmount', function (e) {
        var inputAmount = parseFloat($(this).val());
        var labelAmount = parseFloat($(event.target).closest('#divReplenishAmount').find('#lblMinLowBalAmt').text().replace('$', ''));

        if (inputAmount < labelAmount) {
            $(event.target).focus();

            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: 'The amount entered cannot be < ' + Math.round(labelAmount) + ' -> ' + this.id
            },
        {
            // Settings
            type: 'warning',
            delay: 3000,
            z_index: 3000
        });
        }
    });*/
    // Configure Payment Amount Help Popover
    
   
    /*$('.automaticBillingHelp').popover({
        selector: 'automaticBillingHelp',
        placement: 'top',
        template: popoverInfoTemplateLarge,
        title: 'Automatic Billing',
        content: automaticBillingHelpInfo,
        html: true
    });*/
    
});



