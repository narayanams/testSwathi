

var paymentController = new function () {

    var _originalWebBalance = 0;

    this.validate = function () {
        $.notifyClose('top-right');

        $('#paymentForm').wrap('<form id="validationForm" />');

        $('#validationForm').validate({
            'rules': {
                'CreditCardType': {
                    'required': ($('#IsCCPayment').val() == "True") //IF CC Payment Make it mandatory
                },
                'NameOnCard': {
                    'required': ($('#IsCCPayment').val() == "True"),
                    'maxlength': 29
                },
                'CreditCardNumber': {
                    'required': ($('#IsCCPayment').val() == "True"),
                    'creditcard': true,
                    'minlength': 12,
                    'maxlength': 19
                },
                'ExpirationMonth': {
                    'required': ($('#IsCCPayment').val() == "True")
                },
                'ExpirationYear': {
                    'required': ($('#IsCCPayment').val() == "True")
                },
                'PaymentAmount': {
                    'required': true
                },
                'AccountHolderFirstName': {
                    'required': !($('#IsCCPayment').val() == "True"),  //IF ACH Payment Make it mandatory
                    'maxlength': 50
                },
                'AccountHolderLastName': {
                    'required': !($('#IsCCPayment').val() == "True"),  //IF ACH Payment Make it mandatory
                    'maxlength': 50
                },
                'RoutingNumber': {
                    'required': !($('#IsCCPayment').val() == "True"),  //IF ACH Payment Make it mandatory
                    'maxlength': 20, 'digits': true
                },
                'AccountNumber': {
                    'required': !($('#IsCCPayment').val() == "True"),  //IF ACH Payment Make it mandatory
                    'maxlength': 20, 'digits': true
                }
            },
            'messages': {
                'CreditCardType': {
                    'required': 'Please select the Credit Card you wish to use.'
                },
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
                    'required': 'Please select the Year of your card\'s expiration.'
                },
                'PaymentAmount': {
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
                'AccountNumber': {
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

        // Validate Form
        var isValid = $('#validationForm').valid();

        $('#paymentForm').unwrap();

        return isValid;
    };

    this.SetWebBalance = function (webBalance) {
        _originalWebBalance = webBalance;
    }

    this.GetWebBalance = function () {
        return _originalWebBalance;
    }

    //This method will toggle between ACH and CC Payment
    this.DoCCPayment = function (value) {
        if (value === true) {
            $('.CCPayment').show();
            $('.ACHPayment').hide();
            $('#IsCCPayment').val('True');
            $('.nav-tabs a[id="ccTab"]').tab('show');
            //activationController.IsTransponderSelection = true;
        }
        else {
            $('.CCPayment').hide();
            $('.ACHPayment').show();
            $('#IsCCPayment').val('False');
            $('.nav-tabs a[id="achTab"]').tab('show');
            //activationController.IsTransponderSelection = false;
        }

    }

};

$(function () {
    $('#accountNumber,#routingNumber').password('show');

    $('#accountNumber,#routingNumber').focus(function (event) {
        $('#accountNumber,#routingNumber').password('show');
        $('#accountNumber,#routingNumber').focus();
    });

    $('#accountNumber,#routingNumber').prev().blur(function (event) {
        $('#accountNumber,#routingNumber').password('hide');
    });

    /* [Begin] Prevent User from Returning to this Page after Submit */
    function disableBack() {
        window.history.forward()
    }

    window.onload = disableBack();
    window.onpageshow = function (event) { if (event.persisted) disableBack() }

    $(window).bind('unload', function () {
        disableBack();
    });
    /* [End] Prevent User from Returning to this Page after Submit */

    // On Key Up Event Handler to ensure Numeric Inputs adhere to their Max Length
    $('input[type=number]').on('keyup', function (event) {
        var maxlength = $(this).attr('maxlength');

        if (this.value.length > maxlength) {
            this.value = this.value.slice(0, maxlength);
        }
    });

    // Prevent users from typing anything but Digits into the Credit Card Number
    // NOTE: Not using "number" for INPUT Type as we Mask with "X" for "User Card on File"
    $('#CreditCardNumber').on('keyup', function (event) {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });



    // Auto-Reload / Card-on-File Checkbox Click Event
    // Handle Enable/Disable of Data Input Fields
    $('#Reload_useCardOnFileSelection').click(function () {
        if ($(this).is(':checked')) {
            $('#UseCardOnFile').val(true);

            // Hide "Save Card On File" Option
            $('#saveCardOnFileOptionLabel').attr('style', 'display:none');
            $('#saveCardOnFileOption').prop('checked', false);

            var fieldName = '';
            var defaultValue = '';

            // Disable Input Fields
            $('#cardInformation :input:not(:checkbox):not(:hidden)').each(function () {
                fieldName = '#' + $(this).attr('id') + 'Default';
                defaultValue = $(fieldName).val();

                // Set Reload Value
                $(this).val(defaultValue);

                if (!$(this).hasClass('always-enable')) {
                    // Disable Field
                    $(this).attr('disabled', 'disabled');
                }
            });

            var saveCardOnFile = ($('#SaveCardOnFileDefault').val() === "True") ? true : false;
            var autoReload = ($('#AutoReloadDefault').val() === "True") ? true : false;

            // Set Replenish and Save Card on File Flags
            $('#SaveCardOnFile:input:checkbox').prop('checked', saveCardOnFile);
            $('#AutoReload:input:checkbox').prop('checked', autoReload);
        }
        else {
            $('#UseCardOnFile').val(false);

            // Show Save Card On File Option
            $('#saveCardOnFileOptionLabel').attr('style', 'display:block');
            $('#saveCardOnFileOption').prop('checked', false);

            // Enable Input Fields
            $('#cardInformation :input:not(:checkbox):not(:hidden):not(.always-enable)').each(function () {
                $(this).val('');

                // Enable Field
                $(this).removeAttr('disabled');
            });

            // Clear out Replenish and Save Card on File Flags
            $('#SaveCardOnFile').prop('checked', false);
            $('#AutoReload').prop('checked', false);
        }
    });

    $('.CreditCardNumber').cardcheck({
        callback: function (result) {

            var status = (result.validLen && result.validLuhn) ? 'valid' : 'invalid',
                message = '',
                types = '';

            // Get the names of all accepted card types to use in the status message.
            for (i in result.opts.types) {
                types += result.opts.types[i].name + ", ";
            }
            types = types.substring(0, types.length - 2);

            $(".CreditCardType").val(result.cardType);

            //debugger;
            // Set status message
            if (result.len < 1) {
                message = 'Please provide a credit card number.';
            } else if (!result.cardClass) {
                message = 'We accept the following types of cards: ' + types + '.';
            } else if (!result.validLen) {
                message = 'Please check that this number matches your ' + result.cardName + ' (it appears to be the wrong number of digits.)';
            } else if (!result.validLuhn) {
                message = 'Please check that this number matches your ' + result.cardName + ' (did you mistype a digit?)';
            } else {
                message = 'Great, looks like a valid ' + result.cardName + '.';
            }

            // Show credit card icon
            $('.creditCard .card_icon').removeClass().addClass('card_icon ' + result.cardClass);

            // Show status message
            $('.creditCard .status').removeClass('invalid valid').addClass(status).children('.status_message').text(message);
        }
    });

    $('input[type=checkbox][name=useCardOnFileSelection]').change(function () {
        //alert($(this).is(':checked'));

        if ($(this).is(':checked')) {
            $('#UseCardOnFile').val(true);

            // Hide "Save Card On File" Option
            $('#saveCardOnFileOptionLabel').prop('style', 'display:none');
            //$('#saveCardOnFileOption').prop('checked', false);
            $("input[type='checkbox'][name=saveCardOnFileOption]").attr("checked",false); 

            var fieldName = '';
            var defaultValue = '';

           // debugger;
            // Disable Input Fields
            $('#cardInformation :input:not(:checkbox):not(:hidden)').each(function () {
                fieldName = '#' + $(this).attr('id') + 'Default';
                defaultValue = $(fieldName).val();

                // Set Reload Value
                $(this).val(defaultValue);

                if (this.id == 'CreditCardType')
                {
                    $(this).val(defaultValue).trigger('change');
                    //$('#CreditCardType option[value="' + defaultValue + '"]').attr('selected', true)
                }

                if (!$(this).hasClass('always-enable')) {
                    // Disable Field
                    $(this).attr('disabled', 'disabled');
                }
            });

            var saveCardOnFile = ($('#SaveCardOnFileDefault').val() === "True") ? true : false;
            var autoReload = ($('#AutoReloadDefault').val() === "True") ? true : false;

            // Set Replenish and Save Card on File Flags
            $('#SaveCardOnFile:input:checkbox').prop('checked', saveCardOnFile);
            $('#AutoReload:input:checkbox').prop('checked', autoReload);

            $("input[type='checkbox'][name=SaveCardOnFile]").prop("checked",saveCardOnFile); 
            $("input[type='checkbox'][name=AutoReload]").prop("checked",autoReload); 
        }
        else {
            $('#UseCardOnFile').val(false);

            // Show Save Card On File Option
            $('#saveCardOnFileOptionLabel').attr('style', 'display:block');
            $('#saveCardOnFileOptionDiv').attr('style', 'display:block');
            $("input[type='checkbox'][name=saveCardOnFileOption]").prop('checked', false);

          //  debugger;
            // Enable Input Fields
            $('#cardInformation :input:not(:checkbox):not(:hidden):not(.always-enable)').each(function () {
                $(this).val('');

                // Enable Field
                $(this).removeAttr('disabled');
            });

            // Clear out Replenish and Save Card on File Flags
            $('#SaveCardOnFile').prop('checked', false);
            $('#AutoReload').prop('checked', false);

            $("input[type='checkbox'][name=SaveCardOnFile]").prop("checked",false); 
            $("input[type='checkbox'][name=AutoReload]").prop("checked",false); 

        }
    });
    /*
    $('input[type=checkbox][name=useCardOnFileSelection]').change(function () {
        alert($(this).is(':checked'));

        if ($(this).is(':checked')) {
        }
    });*/

    var formIsValid = false;

    // Submit Form
    $('#yesButton').click(function (event) {
        // Close Confirmation Dialog
        $('#confirmationModal').modal('hide');

        if (!formIsValid) {
            // Disable Submit Button
            if ($('#IsZeroPayment').val() !== "undefined" && $('#IsZeroPayment').val() === 'True') {
                displaySpinner();

                event.stopPropagation();
                $('#paymentForm').submit();
            }
            else {
                $('#submitPayment').addClass('disabled');

                displaySpinner();

                event.stopPropagation();

                if (paymentController.validate()) {
                    formIsValid = true;
                    $('#paymentForm').submit();
                }
                else {
                    // Disable Submit Button
                    $('#submitPayment').removeClass('disabled');

                    hideSpinner();
                }
            }

        }
    });

    // Update Payment Confirmation Details before displaying Dialog
    $('#submitPayment').click(function () {
        var message = '';

        var isValid = true;


       

        var amount = Number($('#paymentAmountLabel').text().replace(/[^0-9\.]+/g, ""));
        amount = amount <= 0 ? typeof ($('#PaymentAmount').val()) === "undefined" ? typeof ($('#PrepaidTollsAmount').val()) === "undefined" ? 0 : Number($('#PrepaidTollsAmount').val()) : Number($('#PaymentAmount').val()) : amount;

        if ($('#IsCCPayment').val() == "True") // CC Payment
        {

            var cardType = $('#CreditCardType').val();
            var cardNumber = $('#CreditCardNumber').val();
            var last4 = cardNumber.substr(cardNumber.length - 4, 4);

            isValid = cardNumber != "" && cardType != "" && cardType != '-' ? true : false;

            message = 'Your ' + jQuery.camelCase('-' + cardType) + ' card ending with ' + last4 + ' will be charged $' + amount.toFixed(2);
        }
        else {
            var accountNumber = $('#BankAccountNumber').val();
            var last4Account = accountNumber.substr(accountNumber.length - 4, 4);

            isValid = accountNumber != "" && $('#RoutingNumber').val()  != "" ? true : false;


            message = 'Your Bank Account Number ending with ' + last4Account + ' will be charged $' + amount.toFixed(2);
        }
        if (isValid)
        {
            $('#confirmationMessage').text(message);
            $('#confirmationModal').modal('show');
        }
        else
        {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: 'Please verify your payment information.'
                },
                {
                    // Settings
                    type: 'warning',
                    delay: 3000,
                    z_index: 3000
                });

        }

    });


    // Update Payment Confirmation Details before displaying Dialog
    $('#submitContinue').click(function () {
        //amount = amount;// + absoluteWebBalance;
        var message = 'Do you want to continue with your Activation/Request?';

        $('#confirmationMessage').text(message);
    });

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
        "<p>If you are an existing customer, a minimum of $10 is required to activate a new E-PASS if your current account balance is below $10 in prepaid tolls</p>" +
        "<p>There is no minimum amount if you are purchasing a new transponder and your account is above the low balance amount.</p>";
    var promotionHelpInfo = "<p>Need to add coupon help</p>";

    var automaticBillingHelpInfo = "<p>An amount of $40.00 for one transponder plus $15.00 for each additional transponder will be automatically added to your account using the credit card on file when your E-PASS account balance reaches your low balance amount.</p>";

    // Configure Reload Help Popover
    $('#reloadAmountHelp').popover({
        placement: 'top',
        template: popoverInfoTemplate,
        title: 'Reload Amount',
        content: reloadAmountHelpInfo,
        html: true
    });

    // Configure Low Balance Help Popover
    $('#lowBalanceHelp').popover({
        placement: 'top',
        template: popoverInfoTemplate,
        title: 'Low Balance Amount',
        content: lowBalanceAmountHelpInfo,
        html: true
    });

    // Configure Payment Amount Help Popover
    $('#paymentAmountHelp').popover({
        placement: 'top',
        template: popoverInfoTemplateLarge,
        title: 'Minimum Payment',
        content: paymentAmountHelpInfo,
        html: true
    });

    // Configure Promotion Amount Help Popover
    $('#promotionAmountHelp').popover({
        placement: 'top',
        template: popoverInfoTemplateLarge,
        title: 'Coupon Code',
        content: promotionHelpInfo,
        html: true
    });

    // Configure Payment Amount Help Popover
    $('#automaticBillingHelp').popover({
        placement: 'top',
        template: popoverInfoTemplateLarge,
        title: 'Automatic Billing',
        content: automaticBillingHelpInfo,
        html: true
    });
    //For MICR fonts visit https://en.wikipedia.org/wiki/Magnetic_ink_character_recognition
    // Configure Payment Amount Help Popover
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

    // Configure Payment Amount Help Popover
    $('#minimumPayment').popover({
        placement: 'top',
        template: popoverTollTemplate,
        title: 'Minimum Payment',
        content: "<p>Amount of $" + Math.abs(paymentController.GetWebBalance()) + " is added to your final payment amount to match the account balance with minimum account balance amount.</p>",
        html: true
    });

    $('#PrepaidTollsAmount').blur(function () {
        var transponderPrice = $('#lblTransponderPrice').text();
        var salesTax = $('#lblSalesTax').text();
        var prepaidTollsAmount = $(this).val();
        var webBalance = $('#WebBalance').val();
        //webBalance = Number(webBalance) < 0 ? Number(webBalance) * -1 : Number(webBalance);
        var isPaymentRequired = $('#hiddenIsPaymentRequired').val();

        // Calculate Total based on any Changes
        var total = Number(transponderPrice.replace(/[^0-9\.]+/g, "")) + Number(salesTax.replace(/[^0-9\.]+/g, "")) + Number(prepaidTollsAmount) + (webBalance < 0 ? Math.abs(Number(webBalance)) : 0);

        total = total.toFixed(2);
        
        // Set Values on Form
        $('#PaymentAmount').val(total);
        $('#paymentAmountLabel').text("$" + total);

        if (prepaidTollsAmount < 10) {
                $.notify({
                    // Options
                    icon: 'fa fa-exclamation-triangle',
                    message: 'Please enter $10.00 or more under Prepaid Tolls.'
                },
                {
                    // Settings
                    type: 'warning',
                    delay: 3000,
                    z_index: 3000
                });

                $(this).focus();
        }
    });


});


$(function () {

  
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("id") // activated tab
        if (target == 'ccTab') 
        {
            paymentController.DoCCPayment(true);
            $('#NameOnCard').focus();
        }
        else
        {
            paymentController.DoCCPayment(false);
            $('#AccountHolderFirstName').focus();
        }
    });


});