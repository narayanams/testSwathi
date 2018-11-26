document.addEventListener("DOMContentLoaded", function () {
    console.log('_CCPaymentInfo.js Script Loading');


    // When the user focuses on the credit card input field, hide the status
    $('.CreditCardNumber').bind('focus', function () {
        //debugger;
        //$('.creditCard .status').hide();
    });

    // When the user tabs or clicks away from the credit card input field, show the status
    $('.CreditCardNumber').bind('blur', function () {
        //debugger;
        //$('.creditCard .status').show();
    });

     //Run jQuery.cardcheck on the input
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


    $(function () {

        $('body').on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
            var target = $(e.target).attr("id") // activated tab
            if (target == 'ccTab') {
                shoppingCartController.DoCCPayment(true);
                $('#NameOnCard').focus();
                if ($('#ccpayWarningIndicator').val() == "D") {
                    $('.previewTransactionButton').prop('disabled', true);
                } else {
                    $('.previewTransactionButton').prop('disabled', false);
                }
            }
            else if (target == 'achTab') {
                shoppingCartController.DoCCPayment(false);
                $('#AccountHolderFirstName').focus();
                if ($('#achpayWarningIndicator').val() == "D") {
                    $('.previewTransactionButton').prop('disabled', true);
                } else {
                    $('.previewTransactionButton').prop('disabled', false);
                }
            }

            if (target == 'buyTransponderTab') {
                $('#orderReplacedTransponderButton').show();
                $('#activateReplacedTransponderButton').hide();
                //$(e.target).parents(".transponderBodyPanel").find('.paymentSection').show();
                var isPaymentRequired = $(e.target).parents('.transponderBodyPanel').find('.payableInformation').attr("data-paymentRequired");
                if (isPaymentRequired == "False" || isPaymentRequired == "false") {
                    $(e.target).parents(".transponderBodyPanel").find('.paymentSection').hide();
                }
                else {
                    $(e.target).parents(".transponderBodyPanel").find('.paymentSection').show();
                }
                $('.CouponCode').show();
                $(e.target).parents().find('#ReplaceTransponders_Modal').attr('transponder-role', 'replaceBuy');
            }
            else if (target == 'activateTransponderTab') {
                $('#orderReplacedTransponderButton').hide();
                $('#activateReplacedTransponderButton').show();

                var isPaymentRequired = $(e.target).parents('.transponderBodyPanel').find('.payableInformation').attr("data-paymentRequired");
                if (isPaymentRequired == "False" || isPaymentRequired == "false") {
                    $(e.target).parents(".transponderBodyPanel").find('.paymentSection').hide();
                }
                else {
                    $(e.target).parents(".transponderBodyPanel").find('.paymentSection').show();
                }
                $('.CouponCode').hide();
                $(e.target).parents().find('#ReplaceTransponders_Modal').attr('transponder-role', 'replaceActivate');
                //
            }
        });

        /*
        //global event for CC | ACHPayment | Replace Transponder | Activate Transponder
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            var target = $(e.target).attr("id") // activated tab
            if (target == 'ccTab') {
                shoppingCartController.DoCCPayment(true);
                $('#NameOnCard').focus();
                if ($('#ccpayWarningIndicator').val() == "D") {
                    $('.previewTransactionButton').prop('disabled', true);
                    $('#savePaymentInfo').prop('disabled', true);
                } else {
                    $('.previewTransactionButton').prop('disable', false);
                    $('#savePaymentInfo').prop('disabled', false);
                }
            }
            else if (target == 'achTab') {
                shoppingCartController.DoCCPayment(false);
                $('#AccountHolderFirstName').focus();
                if ($('#achpayWarningIndicator').val() == "D") {
                    $('.previewTransactionButton').prop('disabled', true);
                    $('#savePaymentInfo').prop('disabled', true);
                } else {
                    $('.previewTransactionButton').prop('disabled', false);
                    $('#savePaymentInfo').prop('disabled', false);
                }
            }

            if (target == 'buyTransponderTab') {
                $('#orderReplacedTransponderButton').show();
                $('#activateReplacedTransponderButton').hide();
                $(e.target).parents().find('.SalesCardPayment').show();
            }
            else if (target == 'activateTransponderTab') {
                $('#orderReplacedTransponderButton').hide();
                $('#activateReplacedTransponderButton').show();

                var isPaymentRequired = $(e.target).parents().find('.payableInformation').attr("data-paymentRequired");
                if(isPaymentRequired == "False")
                {
                    $(e.target).parents().find('.SalesCardPayment').hide();
                }
                else
                {
                    $(e.target).parents().find('.SalesCardPayment').show();
                }
                //
            }
        });
        */
        //Global event for existing card selection
        //$('input[type=checkbox][name=Add_useCardOnFileSelection]').change(function (e) {
        $('body').on('change', 'input.useCardOnFileSelection[type=checkbox]', function (e) {
            //alert($(this).is(':checked'));
            var localPaymentArea = $(this).closest('.cardInformation');
            if ($(this).is(':checked')) {
                localPaymentArea.find('.useCardOnFileSelection').val(true);

                // Hide "Save Card On File" Option
                localPaymentArea.find('#saveCardOnFileOptionLabel').prop('style', 'display:none');
                //$('#saveCardOnFileOption').prop('checked', false);
                localPaymentArea.find("input[type='checkbox'][name=saveCardOnFileOption]").attr("checked", false);

                var fieldName = '';
                var defaultValue = '';

                // debugger;
                // Disable Input Fields
                localPaymentArea.find('#cardInformation :input:not(:checkbox):not(:hidden)').each(function () {
                    fieldName = '#' + $(this).attr('id') + 'Default';
                    defaultValue = $(fieldName).val();

                    // Set Reload Value
                    $(this).val(defaultValue);

                    if (this.id == 'CreditCardType') {
                        $(this).val(defaultValue).trigger('change');
                        //$('#CreditCardType option[value="' + defaultValue + '"]').attr('selected', true)
                    }

                    if (!$(this).hasClass('always-enable')) {
                        // Disable Field
                        $(this).attr('disabled', 'disabled');
                    }
                });

                var saveCardOnFile = (localPaymentArea.find('#SaveCardOnFileDefault').val() == "True") ? true : false;
                var autoReload = (localPaymentArea.find('#AutoReloadDefault').val() == "True") ? true : false;

                var paymentSection = localPaymentArea.closest('.paymentSection');

                // Set Replenish and Save Card on File Flags
                paymentSection.find('#SaveCardOnFile:input:checkbox').prop('checked', saveCardOnFile);
                paymentSection.find('#AutoReload:input:checkbox').prop('checked', autoReload);

                paymentSection.find("input[type='checkbox'][name=SaveCardOnFile]").prop("checked", saveCardOnFile);
                paymentSection.find("input[type='checkbox'][name=AutoReload]").prop("checked", autoReload);
            }
            else {
                localPaymentArea.find('.useCardOnFileSelection').val(false);

                // Show Save Card On File Option
                localPaymentArea.find('#saveCardOnFileOptionLabel').attr('style', 'display:block');
                localPaymentArea.find('#saveCardOnFileOptionDiv').attr('style', 'display:block');
                localPaymentArea.find("input[type='checkbox'][name=saveCardOnFileOption]").prop('checked', false);

                //  debugger;
                // Enable Input Fields
                localPaymentArea.find('#cardInformation :input:not(:checkbox):not(:hidden):not(.always-enable)').each(function () {
                    $(this).val('');

                    // Enable Field
                    $(this).removeAttr('disabled');
                });

                var paymentSection = localPaymentArea.closest('.paymentSection');

                // Clear out Replenish and Save Card on File Flags
                paymentSection.find('#SaveCardOnFile').prop('checked', false);
                paymentSection.find('#AutoReload').prop('checked', false);

                paymentSection.find("input[type='checkbox'][name=SaveCardOnFile]").prop("checked", false);
                paymentSection.find("input[type='checkbox'][name=AutoReload]").prop("checked", false);

            }
        });

    });

});
