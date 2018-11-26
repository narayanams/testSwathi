var transponderCount = 0;

//$(function () {

var currentUniqueTransponderCount = 0;
//Below code will add +/- sign to numbers-row wraping around (text control)
$(".numbers-row").prepend('<a class="dec btn-number number-button update-cart-item" style="font-size:x-large;"><i class="fa fa-minus-circle"></i><p>-</p></a>');
$(".numbers-row").append('<a class="inc btn-number number-button update-cart-item " style="font-size:x-large;"><i class="fa fa-plus-circle"></i><p>+</p></a>');

//var allowAutomaticBillingDataUpdate = function (srcButton) {

//    var paymentDiv;  
//    if (typeof (srcButton) != "undefined")
//        paymentDiv = srcButton.closest(".transponderBodyPanel"); //.find("#paymentRepAmount")


//    // make an ajax call to the getEPASSController
//    $.ajax({
//        url: getBaseUrl() + '/GetEPASS/GetNewReloadLowBalance',
//        data: { 'TransponderCount': transponderCount },
//        type: 'post',
//        success: function (response) {            
//            var data = JSON.parse(response);
//            var url = window.location.pathname;
//            var checked = $('#Add_AutoReload').is(":checked");
///*
//            if (typeof (paymentDiv) != "undefined") {
//                paymentDiv.find("#paymentRepAmount").val(data['NewAutoReplenishAmount'].toFixed(2));
//                paymentDiv.find("#paymentLowAmount").val(data['NewMinBalanceThreshold'].toFixed(2));

//                paymentDiv.find("#lblMinReplAmt").text("$" + data['NewAutoReplenishAmount'].toFixed(2));
//                paymentDiv.find("#lblMinLowBalAmt").text("$" + data['NewMinBalanceThreshold'].toFixed(2));
//            }
//            else {
//                $("#paymentRepAmount").val(data['NewAutoReplenishAmount'].toFixed(2));
//                $("#paymentLowAmount").val(data['NewMinBalanceThreshold'].toFixed(2));

//                $("#lblMinReplAmt").text("$" + data['NewAutoReplenishAmount'].toFixed(2));
//                $("#lblMinLowBalAmt").text("$" + data['NewMinBalanceThreshold'].toFixed(2));
//            }
//            */
            

//            if (url.indexOf("Manage") > 0) {
//                if (typeof (paymentDiv) != "undefined") {
//                    paymentDiv.find("#paymentRepAmount").val(data['NewAutoReplenishAmount'].toFixed(2));
//                    paymentDiv.find("#paymentLowAmount").val(data['NewMinBalanceThreshold'].toFixed(2));

//                    paymentDiv.find("#lblMinReplAmt").text("$" +data['NewAutoReplenishAmount'].toFixed(2));
//                    paymentDiv.find("#lblMinLowBalAmt").text("$" +data['NewMinBalanceThreshold'].toFixed(2));
//                }
//                else
//                {
//                     $("#paymentRepAmount").val(data['NewAutoReplenishAmount'].toFixed(2));
//                    $("#paymentLowAmount").val(data['NewMinBalanceThreshold'].toFixed(2));

//                    $("#lblMinReplAmt").text("$" +data['NewAutoReplenishAmount'].toFixed(2));
//                    $("#lblMinLowBalAmt").text("$" +data['NewMinBalanceThreshold'].toFixed(2));
//                }

//            }
//            else if(url.indexOf("GetEpass") > 0) {
//                    $("#paymentRepAmount").val(data['NewAutoReplenishAmount'].toFixed(2));
//                    $("#paymentLowAmount").val(data['NewMinBalanceThreshold'].toFixed(2));

//                    $("#lblMinReplAmt").text("$" +data['NewAutoReplenishAmount'].toFixed(2));
//                    $("#lblMinLowBalAmt").text("$" +data['NewMinBalanceThreshold'].toFixed(2));
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            console.log(textStatus);
//        }
//    });
//}

/*
$(document).on('click', ".number-button", function (event) {

    var $button = $(this);
    var oldValue = $button.parent().find("input").val();

    var typeValue = $button.parent().find("input").attr('transponder-data-type');
    var priceValue = $button.parent().find("input").attr('transponder-data-price');

    console.log("transponderCount = " + transponderCount);
    console.log('1 call');

    if ($button.text() == "+") {
        if (transponderCount >= 10 && $("#newUser").val() == "True") { //If Transponders added morethan 10 and if its newCustomer then throw an error

            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: 'A maximum of 10 transponders are allowed.'
            },
            {
                // Settings
                type: 'warning',
                delay: 3000,
                z_index: 3000
            });           

            event.stopPropagation();
        } else {
            transponderCount++;

            var newVal = parseFloat(oldValue) + 1;
            currentUniqueTransponderCount = newVal;

            var $divToAppend = $(event.target).parents().find("#addTransponderEditPanel").find(".transponderSelectionArea").find(".transponder-purchase-details[transponder-data-type='" + typeValue + "']");

            if ($divToAppend.length == 0)
                $divToAppend = $(event.target).parents().find(".transponderSelectionArea").find(".transponder-purchase-details[transponder-data-type='" + typeValue + "']");

            //Below code will get the template from HTML, alternatively we can move entire as an
            var template = $('#transponderPurchaseDetailsTemplate').html();
            template = template.replace(/\{0}/g, typeValue);
            template = template.replace(/\{1}/g, newVal);
            template = template.replace(/\{3}/g, priceValue);

            $divToAppend.append(template);
            $button.parent().find("input").val(newVal);

            //If a new user then update PrepaidTolls Amount
            if ($("#newUser").val() == "True")
                $("#PrepaidTollsAmount").val((transponderCount * 10).toFixed(2));

            //allowAutomaticBillingDataUpdate($button);
        }
    } else {
        transponderCount--;
        // Don't allow decrementing below zero
        if (oldValue > 0) {
            var newVal = parseFloat(oldValue) - 1;
            currentUniqueTransponderCount = newVal;

            //If a new user then update PrepaidTolls Amount
            if ($("#newUser").val() == "True")
                $("#PrepaidTollsAmount").val((transponderCount * 10).toFixed(2));

            //transponderPurchaseDetails_22_1
            var detailsToRemove = "#transponderPurchaseDetails_" + typeValue + "_" + oldValue;
            $(detailsToRemove).remove();
            $button.parent().find("input").val(newVal);

            //allowAutomaticBillingDataUpdate($button);
        } else {
            newVal = 0;
            event.stopPropagation();
        }
    }
});
*/

//Use document click as cart items being added dynamically//not used right now but can be used if needed
$(document).on('click', "button.remove-cart-item", function (event) {

    
    var $button = $(this);
    var typeAndDataValue = $button.attr('transponder-data-target');
    var splitedValues = typeAndDataValue.split("_");
    var typeValue = splitedValues.length > 1 ? splitedValues[0] : 0;
    var quantityInGroup = splitedValues.length > 1 ? splitedValues[1] : 0;

    //Get Cart Quantity Value and update it
    var oldValue = $("#transponderPurchase_" + typeValue).val();
    var newVal = parseFloat(oldValue) - 1;
    $("#transponderPurchase_" + typeValue).val(newVal);

    //Get type value and resetting IDs with number system
    var typeValue2 = $("#transponderPurchase_" + typeValue).attr('transponder-data-type');
    var priceValue = $("#transponderPurchase_" + typeValue).attr('transponder-data-price');

    //alert(typeValue + "\n" + oldValue + "\n" + typeValue2 + "\n" + priceValue);

    var detailsToRemove = "#transponderPurchaseDetails_" + typeAndDataValue;
    var localTransponderDetails = $(detailsToRemove).closest("transponder-purchase-details");

    $(detailsToRemove).remove();

    if (parseFloat(oldValue) != quantityInGroup) {
        var startFrom = quantityInGroup;

        var $divToShuffle = $("#addTransponderEditPanel").find(".transponderSelectionArea").find(".transponder-purchase-details[transponder-data-type='" + typeValue + "']");
        if ($divToShuffle.length == 0)
            $divToShuffle = $(".transponderSelectionArea").find(".transponder-purchase-details[transponder-data-type='" + typeValue + "']");
        if ($divToShuffle.length > 0) {
            $divToShuffle.children().each(function (eleNumber) {
                eleNumber++;
                if (eleNumber >= startFrom) {
                    var elem = $(this);
                    var innerHtml = elem.html();
                    var oldId = "" + typeValue2 + "_" + (Number(startFrom) + 1);
                    var newId = "" + typeValue2 + "_" + startFrom;
                    innerHtml = innerHtml.replace(new RegExp(oldId, 'g'), newId);
                    innerHtml = innerHtml.replace("License Plate " + (Number(startFrom) + 1), "License Plate " + startFrom);
                    startFrom++;

                    elem.html(innerHtml);
                    elem.attr('id', 'transponderPurchaseDetails_' + newId);
                }
            });
        }
        // 
        //TODO: Work on Reassigning of IDs
    }
});


//});



(function (window, document, $) {
    var defaults;

    // Plugin Core
    $.cartUpdate = function (opts) {

    }

    $.fn.cartUpdate = function (opts) {

        //return this.bind('click', function () {

        $('body').on('click', '.number-button', function (event) {
            var $button = $(this);
            var oldValue = $button.parent().find("input").val();

            var typeValue = $button.parent().find("input").attr('transponder-data-type');
            var priceValue = $button.parent().find("input").attr('transponder-data-price');


            if ($button.text() == "+") {
                if (transponderCount >= 10 && $("#newUser").val() == "True") { //If Transponders added morethan 10 and if its newCustomer then throw an error

                    $.notify({
                        // Options
                        icon: 'fa fa-exclamation-triangle',
                        message: 'A maximum of 10 transponders are allowed.'
                    },
                    {
                        // Settings
                        type: 'warning',
                        delay: 3000,
                        z_index: 3000
                    });

                    event.stopPropagation();
                } else {
                    transponderCount++;

                    var newVal = parseFloat(oldValue) + 1;
                    currentUniqueTransponderCount = newVal;

                    var $divToAppend = $(event.target).parents().find("#addTransponderEditPanel").find(".transponderSelectionArea").find(".transponder-purchase-details[transponder-data-type='" + typeValue + "']");

                    if ($divToAppend.length == 0)
                        $divToAppend = $(event.target).parents().find(".transponderSelectionArea").find(".transponder-purchase-details[transponder-data-type='" + typeValue + "']");

                    //Below code will get the template from HTML, alternatively we can move entire as an
                    var template = $('#transponderPurchaseDetailsTemplate').html();
                    template = template.replace(/\{0}/g, typeValue);
                    template = template.replace(/\{1}/g, newVal);
                    template = template.replace(/\{3}/g, priceValue);

                    $divToAppend.append(template);
                    $button.parent().find("input").val(newVal);

                    //If a new user then update PrepaidTolls Amount
                    if ($("#newUser").val() == "True")
                        $("#PrepaidTollsAmount").val((transponderCount * 10).toFixed(2));

                    //allowAutomaticBillingDataUpdate($button);
                }
            } else {
                transponderCount--;
                // Don't allow decrementing below zero
                if (oldValue > 0) {
                    var newVal = parseFloat(oldValue) - 1;
                    currentUniqueTransponderCount = newVal;

                    //If a new user then update PrepaidTolls Amount
                    if ($("#newUser").val() == "True")
                        $("#PrepaidTollsAmount").val((transponderCount * 10).toFixed(2));

                    var detailsToRemove = "#transponderPurchaseDetails_" + typeValue + "_" + oldValue;
                    $(detailsToRemove).remove();
                    $button.parent().find("input").val(newVal);

                    //allowAutomaticBillingDataUpdate($button);
                } else {
                    newVal = 0;
                    event.stopPropagation();
                }
            }
        });

        return $('body').on('click change', '.update-cart-item', function (event) {

            if (event.isPropagationStopped()) return false;

            var $button = $(this);
            var price = 0;
            var transponderType = "";
            var addToExisting = false;
            var addPrepaidTolls = false;
            var taxApplicable = true;

            if ($button.hasClass("newCart")) { //Mark all price to 0 and start shopping again
                addToExisting = false;
                $("input:radio.update-cart-item:checked").prop('checked', false);;
            }
            else if ($button.hasClass("number-button")) {

                if ($button.text() == "+" && transponderCount >= 10 && $("#newUser").val() == "True") {
                    //event.stopPropagation();
                    //return false;
                    addToExisting = true;
                    addPrepaidTolls = true;
                    taxApplicable = false;
                    price = 0; //$button.val();
                    transponderType = "";
                }
                else {
                    var inputText = $button.parent().find("input");
                    //if (inputText.val() <= 0 && ($button.hasClass("fa-minus-circle") || $button.hasClass("dec"))) return false;

                    price = inputText.attr('transponder-data-price');
                    transponderType = inputText.attr('transponder-data-type');

                    if ($button.text() != "+")
                        price = Number(price) == 0 ? -1: Number(price) * - 1; // IF Sticker price is changing please change this

                    addPrepaidTolls = $("#newUser").val() == "True" ? true : false;
                    addToExisting = true;
                    taxApplicable = true;
                }

                var product = opts.product || {},
                    transponderType = transponderType,
                    price = price,
                    addToExisting = addToExisting,
                    addPrepaidTolls = addPrepaidTolls,
                    taxApplicable = taxApplicable;
            }
            else if ($button.hasClass("radio-button")) {
                addToExisting = false;
                taxApplicable = true;
                price = $button.attr('transponder-data-price');
                transponderType = $button.attr('transponder-data-type');
            }
            else if ($button.hasClass("prepaidTollsAmount")) {
                //If prepaid tolls amount entered into textbox, just return
                return false;
            }
            else if ($button.hasClass("remove-cart-item")) {
                price = $button.attr('transponder-data-price');
                price = price * -1;
                addToExisting = true;
                taxApplicable = true;
            }

            // Invoke callback
            opts.callback.call(this, {
                event: event,
                price: price,
                transponderType: transponderType,
                opts: opts,
                addToExisting: addToExisting,
                taxApplicable: taxApplicable,
                addPrepaidTolls: addPrepaidTolls,
            });

        }).
        on('blur', '.update-cart-item.prepaidTollsAmount', function (event) {
            var $button = $(this);
            var price = 0;
            var transponderType = "";
            var addToExisting = true;
            var taxApplicable = true;

            if ($button.hasClass("prepaidTollsAmount")) {
                addToExisting = true;
                taxApplicable = false;
                price = 0; //$button.val();
                transponderType = "";
            }


            // Invoke callback
            opts.callback.call(this, {
                event: event,
                price: price,
                transponderType: transponderType,
                opts: opts,
                addToExisting: addToExisting,
                taxApplicable: taxApplicable,
            });
        });

    };


})(this, this.document, this.jQuery);