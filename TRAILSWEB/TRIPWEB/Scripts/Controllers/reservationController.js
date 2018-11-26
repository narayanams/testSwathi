$('#drbcountry').change(function () {
    var country = $(this).val().toUpperCase();

    if (country == "USA" || country == "CAN")
    {
        $('#drbstate').attr('disabled', false);
    }
    else
    {
        $('#drbstate').attr('disabled', true);
    }
});



this.validateReservationInfoData = function (event) {

    if ($('#PaymentInfo_UseCardOnFile').val() == true)
        $("#PaymentInfo_CreditCardNumber").val($("#PaymentInfo_CreditCardNumberDefault").val());

    var acceptedPersonal = $('#TermsConditions:checkbox:checked').length > 0;

    var error;
    if ($('#TripCurrentStatus').val() == "Other") {
        if (!acceptedPersonal) {
            error = "Please accept the user agreement";
        }
    }

    if (error != null) {
        $('#TermsConditions').addClass('has-error');
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

        return false;
    }

    error = null;
    if ((new Date($('#tripEndDate').val()).getTime()) < (new Date($('#tripStartDate').val()).getTime())) {
        error = "Departure date should be greater than Arrival date";
    }
    if (error != null) {
        $('#tripEndDate').addClass('has-error');
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

        return false;
    }
    error = null;
    $('#customerDataForm').wrap('<form id="validationForm" />');

    $.validator.addMethod("regularexpression",
        function(value, element) {
            if (value == undefined || value == null)
                return false;

            if (value.toString().length == 0)
                return false;

            return true;
        },
        "Please fix this field.");

    $.validator.addMethod("minStrict",
        function(value, element, param) {
            return value >= param;
        });

    $.validator.addMethod("maxStrict",
        function(value, element, param) {
            return value <= param;
        });

    var validateCCinfo = true;
    if ($('#Action').val() == "Other") {
        validateCCinfo = true;
    } else {
        if ($('#PaymentInfo_UseCardOnFile').val() == "true" || $('#PaymentInfo_UseCardOnFile').val() == "True") {
            validateCCinfo = false;
        } else {
            validateCCinfo = true;
        }
    }
    

    var country = $('#drbcountry').val().toUpperCase();
    var minimumlength, maximumlength;
    if (country == "USA" || country == "CAN") {
        minimumlength = 5;
        maximumlength = 5;
    }
    else {
        minimumlength = 2;
        maximumlength = 10;
    }
$('#validationForm').validate({
        'rules': {
            'ContactInfo.FirstName': {
                'minlength': 1,
                'maxlength': 25,
                'required': true
            },
            'ContactInfo.LastName': {
                'minlength': 1,
                'maxlength': 25,
                'required': true
            },
            'ContactInfo.AddressLine1': {
                'minlength': 1,
                'maxlength': 30,
                'required': true
            },
            'ContactInfo.AddressLine2': {
                'maxlength': 30,
                'required': false
            },
            'ContactInfo.ZipCode': {
                'minlength': minimumlength,
                'maxlength': maximumlength,
                'required': true
            },
            'ContactInfo.City': {
                'minlength': 1,
                'maxlength': 20,
                'required': true
            },
            'ContactInfo.AddressStateSelected': {
                'required': true //($('#dbrstate').val() == "")
            },
            'ContactInfo.AddressCountrySelected': {
                'required': true//($('#dbrcountry').val() == "")
            },
            'ContactInfo.PhoneNumber': {
                'maxlength': 15,
                'required': true
            },
            'ContactInfo.EmailAddress': {
                'minlength': 6,
                'maxlength': 50,
                'email': true,
                'required': true
            },
            'ArrivalDate': {
                'required': true
            },
            'ArrivalTimeSelected': {
                'required': true//($('#dbrArrivalTime').val() == "")
            },
            'DepartureDate': {
                'required': true
            },
            'DepartureTimeSelected': {
                'required': true//($('#dbrDepartureTime').val() == "")
            },
            'RentalAgencySelected': {
                'required': true
            },
            'PaymentInfo.NameOnCard': {
                'required': validateCCinfo,
                'maxlength': 29
            },
            'PaymentInfo.CreditCardNumber': {
                'required': validateCCinfo,
                'creditcard': true,
                'minlength': 12,
                'maxlength': 19
            },
            'PaymentInfo.ExpirationMonth': {
                'required': validateCCinfo
            },
            'PaymentInfo.ExpirationYear': {
                'required': validateCCinfo
            }

        },
        'messages': {
            'CustomerInfo.FirstName': {
                'minlength': 'First Name must contain at least {0} characters',
                'maxlength': 'First Name can not exceed more than {0} characters',
                'required': 'First Name is required.'
            },
            'CustomerInfo.LastName': {
                'minlength': 'Last Name must contain at least {0} characters',
                'maxlength': 'Last Name can not exceed more than {0} characters',
                'required': 'Last Name is required.'
            },
            'ContactInfo.AddressLine1': {
                'minlength': 'Street Address must contain at least {0} characters',
                'maxlength': 'Street Address can not exceed more than {0} characters',
                'required': 'Street Address is required.'
            },
            'ContactInfo.AddressLine2': {
                'maxlength': 'Street Address can not exceed more than {0} characters'
            },
            
            'ContactInfo.ZipCode': {
                'minlength': 'Zip Code must be minimum of {0} digits',
                'maxlength': 'Zip Code can not exceed more than {0} digits',
                'required': 'Zip Code is required.'
                
            },
            'ContactInfo.City': {
                'minlength': 'City must contain at least {0} characters',
                'maxlength': 'City can not exceed more than {0} characters',
                'required': 'City is required.'
            },
            
            'ContactInfo.AddressCountrySelected': {
                'required': 'A valid country is required'
            },
            'ContactInfo.AddressStateSelected': {
                'required': 'A valid state is required'
            },
            'ContactInfo.PhoneNumber': {
                'maxlength': 'Please enter valid Phone Number',
                'required': 'A phone number is required.'
            },
            'ContactInfo.EmailAddress': {
                'minlength': 'Email Address must contain at least {0} characters',
                'maxlength': 'Email Address can not exceed more than {0} characters',
                'email': 'Please enter a valid Email Address',
                'required': 'Email Address is required.'
            },
            'ArrivalDate': {
                'required': 'Valid Day of Arrival is required.'
            },
            'ArrivalTimeSelected': {
                'required': 'Valid Arrival time is required.'
            },
            'DepartureDate': {
                'required': 'Valid Day of Departure is required.'
            },
            
            'DepartureTimeSelected': {
                'required': 'Valid Departure time is required.'
            },
            'RentalAgencySelected': {
                'required': 'Valid Vehicle Agency is required.'
            },
            'PaymentInfo.NameOnCard': {
                'required': 'Please enter the Name as printed on your Credit Card.',
                'maxlength':'Name on Card can contain no more than {0} characters'
            },
            'PaymentInfo.CreditCardNumber': {
                'required': 'A valid credit card number is required.',
                'creditcard': 'A valid Credit Card Number is required.',
                'minlength': 'A Credit Card Number must contain at least {0} characters',
                'maxlength': 'A Credit Card Number must contain no more than {0} characters'
            },
            'PaymentInfo.ExpirationMonth': {
                'required': 'Please select the Expiration Month.'
            },
            'PaymentInfo.ExpirationYear': {
                'required': 'Please select the expiration Year.'
            }
        },
        errorElement: 'div',
        errorClass: 'help-block',
        errorPlacement: function(error, element) {
            error.insertAfter(element);
        },
        showErrors: function(errorMap, errorList) {

            $.each(this.successList,
                function(index, value) {
                    $.notifyClose();

                    $(this).closest('.paddLeft').removeClass('has-error');
                });
            $.each(errorMap,
                function(key, value) {
                    var temp = stringReplace(key, ".", "_");
                    //alert(temp);
                    $('#' + temp).closest('.paddLeft').addClass('has-error');
                    //alert(JSON.stringify(temp));
                });
            return $.each(errorList,
                function(index, value) {

                    $.notify({
                        // Options
                        icon: 'fa fa-exclamation-triangle',
                        message: value.message
                    },
                        { // Settings
                            type: 'danger',
                            delay: 5000,
                            z_index: 3000
                        });
                });
        }
    });



var isValid = $('#validationForm').valid();
    $('#customerDataForm').unwrap();
    //var isValid = null;
    //if (errorList != null){ isValid = false }
    //else {isValid = true;}
return isValid;
}

this.validateTripInfoData = function (event) {


    if ($('#PaymentInfo_UseCardOnFile').val() == true)
        $("#PaymentInfo_CreditCardNumber").val($("#PaymentInfo_CreditCardNumberDefault").val());

    var error;


    error = null;
    if ((new Date($('#tripEndDate').val()).getTime()) < (new Date($('#tripStartDate').val()).getTime())) {
        error = "Departure date should be greater than Arrival date";
    }
    if (error != null) {
        $('#tripEndDate').addClass('has-error');
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

        return false;
    }
    error = null;
    $('#customerDataForm').wrap('<form id="validationForm" />');

    $.validator.addMethod("regularexpression",
        function(value, element) {
            if (value == undefined || value == null)
                return false;

            if (value.toString().length == 0)
                return false;

            return true;
        },
        "Please fix this field.");

    $.validator.addMethod("minStrict",
        function(value, element, param) {
            return value >= param;
        });

    $.validator.addMethod("maxStrict",
        function(value, element, param) {
            return value <= param;
        });

    var validateCCinfo = true;
    if ($('#Action').val() == "Other") {
        validateCCinfo = true;
    } else {
        if ($('#PaymentInfo_UseCardOnFile').val() == "true" || $('#PaymentInfo_UseCardOnFile').val() == "True") {
            validateCCinfo = false;
        } else {
            validateCCinfo = true;
        }
    }
    

$('#validationForm').validate({
        'rules': {
            'ContactInfo.PhoneNumber': {
                'maxlength': 15,
                'required': true
            },
            'ContactInfo.EmailAddress': {
                'minlength': 6,
                'maxlength': 50,
                'email': true,
                'required': true
            },
            'ArrivalDate': {
                'required': true
            },
            'ArrivalTimeSelected': {
                'required': true//($('#dbrArrivalTime').val() == "")
            },
            'DepartureDate': {
                'required': true
            },
            'DepartureTimeSelected': {
                'required': true//($('#dbrDepartureTime').val() == "")
            },
            'PaymentInfo.NameOnCard': {
                'required': validateCCinfo,
                'maxlength': 29
            },
            'PaymentInfo.CreditCardNumber': {
                'required': validateCCinfo,
                'creditcard': true,
                'minlength': 12,
                'maxlength': 19
            },
            'PaymentInfo.ExpirationMonth': {
                'required': validateCCinfo
            },
            'PaymentInfo.ExpirationYear': {
                'required': validateCCinfo
            }

        },
        'messages': {
            'ContactInfo.PhoneNumber': {
                'maxlength': 'Please enter valid Phone Number',
                'required': 'A phone number is required.'
            },
            'ContactInfo.EmailAddress': {
                'minlength': 'Email Address must contain at least {0} characters',
                'maxlength': 'Email Address can not exceed more than {0} characters',
                'email': 'Please enter a valid Email Address',
                'required': 'Email Address is required.'
            },
            'ArrivalDate': {
                'required': 'Valid Day of Arrival is required.'
            },
            'ArrivalTimeSelected': {
                'required': 'Valid Arrival time is required.'
            },
            'DepartureDate': {
                'required': 'Valid Day of Departure is required.'
            },
            
            'DepartureTimeSelected': {
                'required': 'Valid Departure time is required.'
            },
            'PaymentInfo.NameOnCard': {
                'required': 'Please enter the Name as printed on your Credit Card.',
                'maxlength':'Name on Card can contain no more than {0} characters'
            },
            'PaymentInfo.CreditCardNumber': {
                'required': 'A valid credit card number is required.',
                'creditcard': 'A valid Credit Card Number is required.',
                'minlength': 'A Credit Card Number must contain at least {0} characters',
                'maxlength': 'A Credit Card Number must contain no more than {0} characters'
            },
            'PaymentInfo.ExpirationMonth': {
                'required': 'Please select the Month of your card\'s expiration.'
            },
            'PaymentInfo.ExpirationYear': {
                'required': 'Please select the Year of your card\'s expiration.'
            }
        },
        errorElement: 'div',
        errorClass: 'help-block',
        errorPlacement: function(error, element) {
            error.insertAfter(element);
        },
        showErrors: function(errorMap, errorList) {

            $.each(this.successList,
                function(index, value) {
                    $.notifyClose();

                    $(this).closest('.paddLeft').removeClass('has-error');
                });
            $.each(errorMap,
                function(key, value) {
                    var temp = stringReplace(key, ".", "_");
                    //alert(temp);
                    $('#' + temp).closest('.paddLeft').addClass('has-error');
                    //alert(JSON.stringify(temp));
                });
            return $.each(errorList,
                function(index, value) {

                    $.notify({
                        // Options
                        icon: 'fa fa-exclamation-triangle',
                        message: value.message
                    },
                        { // Settings
                            type: 'danger',
                            delay: 5000,
                            z_index: 3000
                        });
                });
        }
    });

var isValid = $('#validationForm').valid();
    $('#customerDataForm').unwrap();
    //var isValid = null;
    //if (errorList != null){ isValid = false }
    //else {isValid = true;}
return isValid;
}


this.resendEmail = function (event, tripIdOrAccount, status, transactions) {

    var reservationUrl = getBaseUrl() + '/Reservation/ResendEmail';

    $(event.target).attr('disabled', 'disabled');

    resendEmailData = {
        'TripIdOrAccountNo': tripIdOrAccount,
        'Status': status,
        'Transactions': transactions || false
    }

    $.ajax({
        type: 'POST',
        url: reservationUrl,
        data: JSON.stringify(resendEmailData),
        contentType: 'application/json',
        dataType: 'html',
        encode: true, async: false
    }).then(function (response, action) {
        $(event.target).removeAttr('disabled');
        hideSpinner();

        if (typeof (response) != 'undefined' && action == "success") {
            $("#emailResendConfirmation").html("An email has been sent to registered email address.");
        }
        else {
        }
    })
    .catch(function (error) {
        hideSpinner();
        $(event.target).removeAttr('disabled');

    });

    return true;
}

$('#useCardOnFileSelection').click(function () {
    if ($(this).is(':checked')) {
        $('#PaymentInfo_UseCardOnFile').val(true);

        
        var fieldName = '';
        var defaultValue = '';

        // Disable Input Fields
        $('#cardInformation :input:not(:checkbox):not(:hidden)').each(function () {
            fieldName = '#' + $(this).attr('id') + 'Default';
            defaultValue = $(fieldName).val();

            if (fieldName == "#PaymentInfo_CreditCardNumberDefault")
                var defaultValue = ($(fieldName).val() != "") && defaultValue.length >= 4 ? "Card ending in " + defaultValue.substr(defaultValue.length - 4, 4) : '';


            $(this).val(defaultValue);
            
            if (!$(this).hasClass('always-enable')) {
                // Disable Field
                $(this).attr('disabled', 'disabled');
            }
        });
    }
    else {
        $('#PaymentInfo_UseCardOnFile').val(false);
       

        // Enable Input Fields
        $('#cardInformation :input:not(:checkbox):not(:hidden):not(.always-enable)').each(function () {
            $(this).val('');

            // Enable Field
            $(this).removeAttr('disabled');
        });

       
    }
});
