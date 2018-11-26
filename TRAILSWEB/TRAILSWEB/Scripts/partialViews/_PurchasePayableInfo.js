///****///
//This is partial script to display payment information like Total, Tax, SubTotal etc... 
///****///

; document.addEventListener("DOMContentLoaded", function (source) {

    console.log('_PurchasePayableInfo.js Script Loading');

    ///Below will be a callback from +/- button clicking on Transponders
    $('.update-cart-item').cartUpdate({
        callback: function (result) {
            //Get Modal div where cart is there
            var e;
            if (typeof (event) == "undefined")
                e = result.event;
            else
                e = event;
            
            var couponValidationURL = getBaseUrl() + '/Manage/ValidateCouponCode';
            //var action = transpondersInfoToPurchase.transponderAction;
            // Display Spinner
            //$(event.target).addClass('fa-spinner fa-spin animated');

            var transpondersToPurchase = shoppingCartController.getShopingCartTransponders(e);

            var couponCodeData = {
                'Transponders': transpondersToPurchase.transponders,
                'IsNewCustomer': transpondersToPurchase.isNewCustomer,
                'CouponCode': $("#CouponCode").val(),
                'IsCouponCodeApplied': false //transpondersToPurchase.isCouponCodeApplied
            }

            $(e.target).attr('disabled', 'disabled');

            $.ajax({
                type: 'POST',
                url: couponValidationURL,
                data: JSON.stringify(couponCodeData),
                contentType: 'application/json',
                dataType: 'html',
                encode: true, async: false
            }).then(function (response, action) {

                //Get all transponders and for specific type apply the coupon code.
                var parsedResponse = $.parseJSON(response);
                if (parsedResponse.success ) { // && parsedResponse.isValidCouponCode) {
                    $("#invalidCouponLabel").hide();
                    shoppingCartController.updateShoppingPaymentInformation(e, parsedResponse);
                }
                else {
                    $("#invalidCouponLabel").val = parsedResponse.message;
                    $("#invalidCouponLabel").show();
                }

                //shoppingCartController.updateShoppingPaymentInformation(e, $.parseJSON(response));
                $(e.target).removeClass('fa-spinner fa-spin animated');
                $(e.target).removeAttr('disabled');

            })
            .catch(function (error) {
                //$(event.target).removeClass('fa-spinner fa-spin animated');
                hideSpinner();
                $(e.target).removeAttr('disabled');
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

            var payableDiv = $(e.target).closest('.transponderBodyPanel').find('.TransponderPurchase').find('.payableInformation');
            payableDiv.parents(".transponderBodyPanel").find('.paymentSection').show();

/*
            var payableDiv = $(e.target).closest('.transponderBodyPanel').find('.TransponderPurchase').find('.payableInformation');

            if (payableDiv.length == 0)
            {
                payableDiv = $(e.target).parents().find('.TransponderPurchase').find('.payableInformation');
            }


            payableDiv.parents(".transponderBodyPanel").find('.paymentSection').show();


            //If div found then update Transponder Cost and other related tax information
            if (typeof (payableDiv) != 'undefined')
            {
                var tranCost = 0, tranTax = 0, transSubTotal = 0, transMinimumBalance = 0, transPrepaidToll = 0, transFinalAmount = 0;

                tranCost = Number(payableDiv.find("#lblTransponderPrice").text());
                tranTax = Number(payableDiv.find("#lblSalesTax").text());

                transPrepaidToll = Number(payableDiv.find("#PrepaidTollsAmount").val());
                transMinimumBalance = Number(payableDiv.find("#minimumToll").text());
            }

            //if not add to existing, start with empty cart
            if (result.addToExisting == true )
            {
                if (result.addPrepaidTolls) {
                    if (typeof (result.transponderType) != "undefined" && result.transponderType != "" && result.transponderType != 0) {
                        transPrepaidToll = (result.price >= 0) ? transPrepaidToll + 10 : transPrepaidToll - 10;
                    }
                }
                result.price = result.price == -1 ? 0 : result.price;
                tranCost = tranCost + Number(result.price);
                tranTax = tranTax + (Number(result.price) * 6.5) / 100;

            }
            else {                    
                tranCost = Number(result.price);
                if (transPrepaidToll == 0)
                {
                    transPrepaidToll = 0;
                    payableDiv.find("#PrepaidTollsAmount").val(transPrepaidToll);
                }
                
            }

            transSubTotal = tranCost + tranTax;

            transFinalAmount = transSubTotal + transPrepaidToll + transMinimumBalance;
                               
            //Update payable information
            payableDiv.find("#lblTransponderPrice").text(tranCost.toFixed(2));
            payableDiv.find("#lblSalesTax").text(Number(tranTax.toFixed(2)).toFixed(2));
            payableDiv.find("#subTotal").text(Number(transSubTotal.toFixed(2)).toFixed(2));

            //transFinalAmount = transFinalAmount.toFixed(2) == "-0.00" ? 0 : Number(transFinalAmount.toFixed(2));
            payableDiv.find("#paymentAmountLabel").text('$' + Number(transFinalAmount.toFixed(2)).toFixed(2)); //No space in $ sign as it brings in next line on mobile devices
      */          
        }
    });
    
    //Below code will mark a CouponCode to False when there is a change in CouponCode
    //This will turn validated when user will click on ApplyButton and validated through services
    //This is applied to class not the ID/Name because there might be multiple CouponCode in UI
    $('body').on('change', '.CouponCode', function (e) {
        $(this).attr('IsValidated', 'false');
    });
    
});
