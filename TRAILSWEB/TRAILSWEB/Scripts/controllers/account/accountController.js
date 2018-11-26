"use strict";
var accountController = new function () {

    // Validate Unique User Name
    this.UserNameIsUnique = function (value, asyncOption) {
        asyncOption = typeof asyncOption !== 'undefined' ? asyncOption : true;

        //todo added for test purposes
        return true;
        var isUnique = false;

        $.ajax({
            url: getBaseUrl() + '/Account/UniqueUsername',
            data: { 'userName': value },
            async: asyncOption,
            type: 'post',
            success: function (response) {
                isUnique = response === true || response === "true";
            }
        });        
        return isUnique;
    };


    //Below code is not being used but may be use when user provides license plate on GetEpass or Activate
    var LicensePlateIsUnique = function (licensePlate, state, asyncOption) {
        asyncOption = typeof asyncOption !== 'undefined' ? asyncOption : true;

        var isUnique = false;

        $.ajax({
            url: getBaseUrl() + '/Account/UniqueLicensePlate',
            data: { 'licensePlate': licensePlate, 'state': state },
            async: asyncOption,
            type: 'post',
            success: function (response) {
                isUnique = response === true || response === "true";
            }
        });

        return isUnique;
    };

    this.validateLogin = function () {
        $('#loginFormData').wrap('<form id="validationForm" />');

        $.validator.addMethod("regex", function (value, element, regexpr) {
            var regexToValidate = new RegExp(regexpr);
            return regexToValidate.test(value);
        }, "Please enter a valid pasword.");


        $('#validationForm').validate({
            'rules': {
                'UserName': {
                    'minlength': 1,
                    'maxlength': 32,
                    'required': true,
                    'regex': /^[a-zA-Z0-9 .@!#$%&'*+-\/=?^_`|{}~]{1,}$/
                },
                'Password': {
                    'minlength': 1,
                    'maxlength': 32,
                    'required': true,
                    'regex': /^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*).{8,}$/
                }
            },
            'messages': {
                'UserName': {
                    'minlength': 'Username must contain at least {0} characters',
                    'maxlength': 'Username can not exceed more than {0} characters',
                    'required': 'Username is required.',
                    'regex': 'Please verify your User Name (no special characters).'
                },
                'Password': {
                    'minlength': 'Password must contain at least {0} characters',
                    'maxlength': 'Password can not exceed more than {0} characters',
                    'required': 'Password is required.',
                    'regex': 'Please enter valid password.'
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

        var isValid = $('#validationForm').valid();

        $('#loginFormData').unwrap();

        return isValid;
    };
    this.validate = function () {
        var userName = $('#SecurityPreferences_UserName').val();

        // jQuery Validate's "remote" option alwas returns 'pending' and making an AJAX call within  a Custom Validation Rule
        // is extremely problematic so, let's call our own "validate" method and our Custom Validator Rule will just check
        // the results.
        var userNameIsUnique = this.UserNameIsUnique(userName);

        $('#userRegistrationForm').wrap('<form id="validationForm" />');

        // Check results of previous License Plate Number Validation Check
        $.validator.addMethod("uniqueUserName", function (value, element) {
            return userNameIsUnique;
        }, "Please fix this field.");

        $('#validationForm').validate({
            'rules': {
                'SecurityPreferences.UserName': {
                    'uniqueUserName': true,
                    'minlength': 1,
                    'maxlength': 100,
                    'required': true
                },
                'SecurityPreferences.Password': {
                    'minlength': 8,
                    'maxlength': 100,
                    'required': true
                },
                'SecurityPreferences.ConfirmPassword': {
                    'equalTo': "#SecurityPreferences_Password",
                    'required': true
                },
                'SecurityPreferences.PinNumber': {
                    'minlength': 4,
                    'maxlength': 4,
                    'digits': true,
                    'required': true
                }
            },
            'messages': {
                'SecurityPreferences.UserName': {
                    'uniqueUserName': 'The Username entered already exists. Please select another.',
                    'minlength': 'Username must contain at least {0} characters',
                    'maxlength': 'Username can not exceed more than {0} characters',
                    'required': 'Username is required.'
                },
                'SecurityPreferences.Password': {
                    'minlength': 'Password must contain at least {0} characters',
                    'maxlength': 'Password can not exceed more than {0} characters',
                    'required': 'Password is required.'
                },
                'SecurityPreferences.ConfirmPassword': {
                    'equalTo': 'Passwords do not match',
                    'required': 'Password confirmation is required.'
                },
                'SecurityPreferences.PinNumber': {
                    'minlength': 'PIN Number must contain at least {0} characters',
                    'maxlength': 'PIN Number can not exceed more than {0} characters',
                    'digits': 'PIN Number can only contain numbers',
                    'required': 'PIN Number is required.'
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

        var isValid = $('#validationForm').valid();

        $('#userRegistrationForm').unwrap();

        return isValid;
    };



    this.validateNewAccountContact = function (event) {

			var isPersonalAccount = true;

        if (typeof ($("ul#accountTypeTabs li.active")) != 'undefined') {
            if ($.trim($("ul#accountTypeTabs li.active").text()) != "Personal") //Check whether Personal or Business Account
                isPersonalAccount = false;
        }

        var acceptedPersonal = $('#TermsConditions:checkbox:checked').length > 0;

        var error;
        if (!acceptedPersonal) {
            error = "Please accept the user agreement";

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

        $('#createNewCustomertForm').wrap('<form id="validationForm" />');

        $.validator.addMethod("regularexpression", function (value, element) {
            if (value == undefined || value == null)
                return false;

            if (value.toString().length == 0)
                return false;

            return true;
        }, "Please fix this field.");

        $.validator.addMethod("minStrict", function (value, element, param) {
            return value >= param;
        });

        $.validator.addMethod("maxStrict", function (value, element, param) {
            return value <= param;
        });

        $('#validationForm').validate({
            'rules': {
                'Customer.CustomerInfo.PrimaryFirstName': {
                    'minlength': 1,
                    'maxlength': 25,
                    'required': isPersonalAccount
                },
                'Customer.CustomerInfo.PrimaryLastName': {
                    'minlength': 1,
                    'maxlength': 25,
                    'required': isPersonalAccount
                },
                'Customer.CustomerInfo.BusinessName': {
                    'minlength': 1,
                    'maxlength': 80,
                    'required': !isPersonalAccount
                },
                'Customer.CustomerInfo.BusinessContactName': {
                    'minlength': 1,
                    'maxlength': 50,
                    'required': !isPersonalAccount
                },
                'Customer.CustomerInfo.BusinessTIN': {
                    'minlength': 1,
                    'maxlength': 25,
                    'required': !isPersonalAccount
                },
                'Customer.ContactInfo.AddressLine1': {
                    'minlength': 1,
                    'maxlength': 30,
                    'required': true
                },
                'Customer.ContactInfo.AddressLine2': {
                    'maxlength': 30,
                    'required': false
                },
                'Customer.ContactInfo.ZipCode': {
                    'minlength': 5,
                    'maxlength': 5,
                    'required': true,
                    'digits': true
                },
                'Customer.ContactInfo.City': {
                    'minlength': 1,
                    'maxlength': 20,
                    'required': true
                },
                'CustomerContactState': {
                    'required': ($('#CustomerContactState').val() == "")
                },
                'Customer.ContactInfo.EveningPhone': {
                    'phoneUS': true,
                    'required': true
                },
                'Customer.ContactInfo.BusinessPhone': {
                    'phoneUS': true,
                    'required': true
                },
                'Customer.CustomerInfo.Pin': {
                    'minlength': 4,
                    'maxlength': 4,
                    'required': true,
                    'digits': true,
                    'minStrict': 1000, 'maxStrict': 9999,
                    'regularexpression': /^([1-9][0-9]{0,3})$/
                },
                'Customer.CustomerInfo.PrimaryLicense': {
                    'minlength': 1,
                    'maxlength': 25,
                    'required': isPersonalAccount
                },
                'PrimaryLicenseState': {
                    'required': ($('#Customer_CustomerInfo_PrimaryLicense').val() != "" && $('#PrimaryLicenseState').val() == "")
                },
                'Customer.CustomerInfo.SecondaryFirstName': {
                    'minlength': 1,
                    'maxlength': 25,
                    'required': function (element) {
                        return ($('#Seconduser:input:checkbox').length > 0);
                    }
                },
                'Customer.CustomerInfo.SecondaryLastName': {
                    'minlength': 1,
                    'maxlength': 25,
                    'required': function (element) {
                        return ($('#Seconduser:input:checkbox').length > 0);
                    }
                },
                'Customer.CustomerInfo.SecondaryLicense': {
                    'minlength': 1,
                    'maxlength': 25,
                    'required': function (element) {
                        return ($('#Seconduser:input:checkbox').length > 0);
                    }
                },
                'SecondaryLicenseState': {
                    'required': ($('#Customer_CustomerInfo_SecondaryLicense').val() != "" && $('#PrimaryLicenseState').val() == "")

                }
            },
            'messages': {
                'Customer.CustomerInfo.PrimaryFirstName': {
                    'minlength': 'First Name must contain at least {0} characters',
                    'maxlength': 'First Name can not exceed more than {0} characters',
                    'required': 'First Name is required.'
                },
                'Customer.CustomerInfo.PrimaryLastName': {
                    'minlength': 'Last Name must contain at least {0} characters',
                    'maxlength': 'Last Name can not exceed more than {0} characters',
                    'required': 'Last Name is required.'
                },
                'Customer.CustomerInfo.BusinessName': {
                    'minlength': 'Business Name must contain at least {0} characters',
                    'maxlength': 'Business Name can not exceed more than {0} characters',
                    'required': 'Business Name is required.'
                },
                'Customer.CustomerInfo.BusinessContactName': {
                    'minlength': 'Business Contact Name must contain at least {0} characters',
                    'maxlength': 'Business Contact Name can not exceed more than {0} characters',
                    'required': 'Business Contact Name is required.'
                },
                'Customer.CustomerInfo.BusinessTIN': {
                    'minlength': 'Business Tax ID must contain at least {0} characters',
                    'maxlength': 'Business Tax ID can not exceed more than {0} characters',
                    'required': 'Business Tax ID is required.'
                },
                'Customer.ContactInfo.AddressLine1': {
                    'minlength': 'Street Address must contain at least {0} characters',
                    'maxlength': 'Street Address can not exceed more than {0} characters',
                    'required': 'Street Address is required.'
                },
                'Customer.ContactInfo.AddressLine2': {
                    'maxlength': 'Street Address can not exceed more than {0} characters'
                },
                'Customer.ContactInfo.ZipCode': {
                    'minlength': 'Zip Code must be {0} digits',
                    'maxlength': 'Zip Code can not exceed more than {0} digits',
                    'required': 'Zip Code is required.',
                    'digits': 'This field can only contain numbers'
                },
                'Customer.ContactInfo.City': {
                    'minlength': 'City must contain at least {0} characters',
                    'maxlength': 'City can not exceed more than {0} characters',
                    'required': 'City is required.'
                },
                'CustomerContactState': {
                    'required': 'A valid state is required'
                },
                'Customer.ContactInfo.EveningPhone': {
                    'phoneUS': 'Please enter a United States phone number',
                    'required': 'A phone number is required.'
                },
                'Customer.ContactInfo.BusinessPhone': {
                    'phoneUS': 'Please enter a United States phone number',
                    'required': 'A phone number is required.'
                },
                'Customer.CustomerInfo.Pin': {
                    'minlength': 'PIN Number must be {0} digits',
                    'maxlength': 'PIN Number can not exceed more than {0} digits',
                    'minStrict': 'PIN Number must be between 1000 to 9999',
                    'maxStrict': 'PIN Number must be between 1000 to 9999',
                    'required': 'PIN Number is required.',
                    'digits': 'This field can only contain numbers',
                    'regularexpression': 'Please enter the valid Pin'
                },
                'Customer.CustomerInfo.PrimaryLicense': {
                    'minlength': 'Drivers License must contain at least {0} characters',
                    'maxlength': 'Drivers License can not exceed more than {0} characters',
                    'required': 'Drivers License is required.'
                },
                'PrimaryLicenseState': {
                    'required': 'A valid License State is required'
                },
                'Customer.CustomerInfo.SecondaryFirstName': {
                    'minlength': 'Secondary User First Name must contain at least {0} characters',
                    'maxlength': 'Secondary User First Name can not exceed more than {0} characters',
                    'required': 'Secondary User First Name is required.'
                },
                'Customer.CustomerInfo.SecondaryLastName': {
                    'minlength': 'Secondary User Last Name must contain at least {0} characters',
                    'maxlength': 'Secondary User Last Name can not exceed more than {0} characters',
                    'required': 'Secondary User Last Name is required.'
                },
                'Customer.CustomerInfo.SecondaryLicense': {
                    'minlength': 'Secondary User Drivers License must contain at least {0} characters',
                    'maxlength': 'Secondary User Drivers License can not exceed more than {0} characters',
                    'required': 'Secondary User Drivers License is required.'
                },
                'SecondaryLicenseState': {
                    'required': 'A valid Secondary User License State is required'
                }

            },
            errorElement: 'div',
            errorClass: 'help-block',
            errorPlacement: function (error, element) {
                error.insertAfter(element);
            },
            showErrors: function (errorMap, errorList) {

                $.each(this.successList, function (index, value) {
                    $.notifyClose();

                    $(this).closest('.paddLeft').removeClass('has-error');
                });
                $.each(errorMap, function (key, value) {
                    var temp = stringReplace(key, ".", "_");
                    //alert(temp);
                    $('#' + temp).closest('.paddLeft').addClass('has-error');
                    //alert(JSON.stringify(temp));
                });
                return $.each(errorList, function (index, value) {

                    $.notify({
                        // Options
                        icon: 'fa fa-exclamation-triangle',
                        message: value.message
                    },
                    {// Settings
                        type: 'danger',
                        delay: 5000,
                        z_index: 3000
                    });
                });
            }
        });

        var isValid = $('#validationForm').valid();
        $('#createNewCustomertForm').unwrap();
        return isValid;
    }

    this.validateNewAccount = function () {

        var userName = $('#Security_UserName').val();


        var transponderProvided = $("#addTransponderBodyPanel").attr('data-trasponder-role') == "activate" ? ($("#Search_TransponderNumber").val() == "" || $("#Search_TransponderNumber").val().length != 13 || $("#validateTransponder_0").text() != "true" ? false : true) : true;

        if (!transponderProvided) {
            $("#Search_TransponderNumber").focus();

            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: "Please Provide a Valid Transponder"
            },
                           {
                               // Settings
                               type: 'danger',
                               delay: 5000,
                               z_index: 3000
                           });

            return false;
        }

        //var userNameIsUnique = accountController.UserNameIsUnique(userName, false);

        $('#createNewLoginForm').wrap('<form id="validationForm" />');

        // Check results of previous License Plate Number Validation Check
        //$.validator.addMethod("uniqueUserName", function (value, element) {
        //    return userNameIsUnique;
        //}, "Please fix this field.");

       

        //$.validator.addMethod("regularexpression", function (value, element) {
        //    if (value == undefined || value == null)
        //        return false;

        //    if (value.toString().length == 0)
        //        return false;

        //    return true;
        //}, "Please fix this field.");


        $('#validationForm').validate({
            'rules': {
                'Security.UserName': {
                    //'uniqueUserName': true,
                    'minlength': 1,
                    'maxlength': 100,
                    'required': true
                },
                'Security.Password': {
                    'minlength': 8,
                    'maxlength': 100,
                    'required': true
                },
                'Security.ConfirmPassword': {
                    'equalTo': "#Security_Password",
                    'required': true
                },
                'Security.Email': {
                    'minlength': 6,
                    'maxlength': 150,
                    'email': true,
                    'required': true
                },
                'Security.ConfirmEmail': {
                    'minlength': 6,
                    'maxlength': 150,
                    'email': true,
                    'required': true,
                    'equalTo': "#Security_Email",
                },
            },
            'messages': {
                'Security.Email': {
                    'minlength': 'Email Address must contain at least {0} characters',
                    'maxlength': 'Email Address can not exceed more than {0} characters',
                    'email': 'Please enter a valid Email Address',
                    'required': 'Email Address is required.'
                },
                'Security.ConfirmEmail': {
                    'minlength': 'Email Address must contain at least {0} characters',
                    'maxlength': 'Email Address can not exceed more than {0} characters',
                    'email': 'Please enter a valid Email Address',
                    'required': 'Confirm Email Address is required.',
                    'equalTo': 'Email do not match',
                },
                'Security.UserName': {
                   // 'uniqueUserName': 'The Username entered already exists. Please select another.',
                    'minlength': 'Username must contain at least {0} characters',
                    'maxlength': 'Username can not exceed more than {0} characters',
                    'required': 'Username is required.'
                },
                'Security.Password': {
                    'minlength': 'Password must contain at least {0} characters',
                    'maxlength': 'Password can not exceed more than {0} characters',
                    'required': 'Password is required.'
                },
                'Security.ConfirmPassword': {
                    'equalTo': 'Passwords do not match',
                    'required': 'Password confirmation is required.'
                },
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


        var isValid = $('#validationForm').valid();

        $('#createNewLoginForm').unwrap();

        return isValid;
    };

    this.validateSecurityInfoData = function () {
    	
        var validated = false;
        var securityInfoModel = this.getAccountLoginInfo("");

        var validateSecurityDataURL = getBaseUrl();

        validateSecurityDataURL = validateSecurityDataURL + '/Account/ValidateSecurityData';        

        // Display Spinner
        //$('#createNewAccount span').removeClass('fa-sign-in');
        $('#createNewAccount span').addClass('fa-spinner fa-spin animated');
        $('#createNewAccount, #cancelLoginButton').addClass('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: validateSecurityDataURL,
            data: JSON.stringify(securityInfoModel),
            contentType: 'application/json',
            dataType: 'json',
            encode: true,
            async: false
        }).done(function (response) {
            if (response && (response.success || response.Success)) {
                validated = true;
                $(".PageTagline").text("Please fill out the information below.");
            }
            else {
                // Hide Spinner
                $('#createNewAccount span').removeClass('fa-spinner fa-spin animated');
                $('#createNewAccount, #cancelLoginButton').removeClass('disabled');
                if (response) {
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
                validated = false;
            }
        });

        hideSpinner();
        return validated;
      

       
    
    };

    this.validateCustomerInfoData = function (event) {

        var validated = false;
        var customerInfoModel = this.getCustomerDataInfo(event);

		var validateCustomerDataURL = getBaseUrl();

        validateCustomerDataURL = validateCustomerDataURL + '/Account/ValidateCustomerData';

	// Display Spinner
	//$('#createNewAccount span').removeClass('fa-sign-in');
        $('#createNewAccount span').addClass('fa-spinner fa-spin animated');
        $('#createNewAccount, #cancelLoginButton').addClass('disabled', 'disabled');
  
        $.ajax({
        		type: 'POST',
        		url: validateCustomerDataURL,
        		data: JSON.stringify(customerInfoModel),
        		contentType: 'application/json',
        		dataType: 'json',
        	encode: true,
        		async: false
        }).done(function (response) {
            $("#progress-spinner").hide();

            if (response && (response.success || response.Success)) {
                validated = true;
            }
            else {
            	// Hide Spinner
                $('#createNewAccount span').removeClass('fa-spinner fa-spin animated');
                $('#createNewAccount, #cancelLoginButton').removeClass('disabled');
                if (response) {
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
                validated = false;
        }
        }).fail(function() {
            $("#progress-spinner").hide();
            $('#createNewAccount span').removeClass('fa-spinner fa-spin animated');
            $('#createNewAccount, #cancelLoginButton').removeClass('disabled');
});

        return validated;
    };

    $("#Security_Password").blur(function () {
        var valid = false;  // assuming all input as invalid until we valid to determine otherwise
        var pw = $('#Security_Password').val();

        var rePass = new RegExp('^[^ ]{8,32}$');

        if (rePass.test(pw) && rePass.test(pw)) {
            valid = true;
        } else {
        	//$('#Security_Password').focus();
            $.notify({
            	// Options
            		icon: 'fa fa-exclamation-triangle',
            		message: 'Password does not satisfy the password requirements. Check the tooltip for password requirements.'
            },
            {
            	// Settings
            		type: 'warning',
            	delay: 2000,
            	z_index: 3000
        });

            valid = false;
    }
    });

    $('#Security_ConfirmPassword').blur(function () {
        var valid = false;  // assuming all input as invalid until we valid to determine otherwise
        var pw = $('#Security_Password').val();
        var cpw = $('#Security_ConfirmPassword').val();

        var rePass = new RegExp('^[^ ]{8,32}$');

        if (rePass.test(cpw)) {
            valid = true;
        } else {
        	//$('#Security_ConfirmPassword').focus();

            $.notify({
            	// Options
            		icon: 'fa fa-exclamation-triangle',
            		message: 'Confirm Password does not satisfy the password requirements. Check the tooltip for password requirements.'
            },
            {
            	// Settings
            		type: 'warning',
            	delay: 3000,
            	z_index: 3000
        });

            valid = false;

            return;
    }

        if (pw == cpw) {
            valid = true;
        } else {
            $('#Security_ConfirmPassword').focus();
            $.notify({
            	// Options
            		icon: 'fa fa-exclamation-triangle',
            		message: 'Password and confirm password does not match.'
            },
            {
            	// Settings
            		type: 'warning',
            	delay: 3000,
            	z_index: 3000
        });

            valid = false;
    }

    	// when all checks are done, if 'valid' is true then enable 'continue' button otherwise leave as disable
    });

    $('#Security_ConfirmEmail').blur(function () {
        var validEmail = false;  // assuming all input as invalid until we valid to determine otherwise
        var validConfirmEmail = false;  // assuming all input as invalid until we valid to determine otherwise
        var email = $('#Security_Email').val();
        var cemail = $('#Security_ConfirmEmail').val();
        var reEmail = new RegExp(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/igm);

    	// validate email
        validEmail = reEmail.test(email);
        validConfirmEmail = new RegExp(reEmail).test(cemail);

        if (!validEmail) {
            $.notify({
            	// Options
            		icon: 'fa fa-exclamation-triangle',
            		message: 'Email address is not valid.'
            },
                        {
                        	// Settings
                        		type: 'warning',
                        	delay: 3000,
                        	z_index: 3000
        });

            valid = false;
            $('#Security_Email').focus();
    }

        if (!validConfirmEmail) {
            $.notify({
            	// Options
            		icon: 'fa fa-exclamation-triangle',
            		message: 'Confirm email address is not valid.'
            },
                        {
                        	// Settings
                        		type: 'warning',
                        	delay: 3000,
                        	z_index: 3000
        });

            valid = false;
            $('#Security_ConfirmEmail').focus();
    }

        if (email != cemail) {
            $.notify({
            	// Options
            		icon: 'fa fa-exclamation-triangle',
            		message: 'Email Address and Confirm Email Address does not match.'
            },
                        {
                        	// Settings
                        		type: 'warning',
                        	delay: 3000,
                        	z_index: 3000
        });

            valid = false;
            $('#Security_Email').focus();
    }
    });
    

    this.validatePasswordReset = function (event) {
        $.notifyClose('top-right');

        $('#resetPasswordFormData').wrap('<form id="validationForm" />');

        $('#validationForm').validate({
            'rules': {
                'NewPassword': {
                    'maxlength': 15,
                    'required': true
            },
                'ConfirmNewPassword': {
                    'maxlength': 15,
                    'equalTo': "#NewPassword",
                    'required': true
            }
        },
            'messages': {
                'NewPassword': {
                    'maxlength': 'New Password can not exceed more than {0} characters',
                    'required': 'New Password is required'
            },
                'ConfirmNewPassword': {
                    'maxlength': 'Confirm Password can not exceed more than {0} characters',
                    'equalTo': 'Confirm Password does not match',
                    'required': 'Confirm Password is required'
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

        var isValid = $('#validationForm').valid();

        $('#resetPasswordFormData').unwrap();

        return isValid;
    }

    this.CreateUserForAccountNPin = function (event) {
        event.preventDefault();
        var registerDodel = {
            'AccountNumber': $('#AccountNumber').val(),
            'PinNumber': $('#PinNumber').val(),
            'UserName': $('#RegisterModel_UserName').val(),//TrailsSecurity.Encrypt($('#UserName').val()) + '',
            'ConfirmUserName': $('#RegisterModel_ConfirmUserName').val(),
            'Password': $('#RegisterModel_Password').val(),//TrailsSecurity.Encrypt($('#Password').val()) + '',
            'ConfirmPassword': $('#RegisterModel_ConfirmPassword').val(),//TrailsSecurity.Encrypt($('#Password').val()) + '',
            'Email': $('#RegisterModel_Email').val(),
            'SecurityAnswer': $('#RegisterModel_SecurityAnswer').val(),
            'EntryPoint': $('#EntryPoint').val(),
            'SessionId': $('#RegisterModel_SessionId').val(),
            'SecurityQuestion': $('#RegisterModel_SecurityQuestionSelected').val()
    }

        var loginAndRegister = {
            'LoginModel': {}, 'RegisterModel': registerDodel
    };
        var baseUrl = getBaseUrl();

        $.ajax({
        		type: "POST",
        		url: baseUrl + '/Account/RegisterAccountWithUser',
        		data: loginAndRegister,
        	//dataType: 'json',
        	//encode: true,
        		success: function (response) {

                if (response.success == true) {
                    if (typeof response.newUrl != 'undefined')
                        window.location.href = response.newUrl;
                }
                else {
                    var errorResponse = "";
                    if (typeof response.errors === 'undefined') {
                        errorResponse = "There is an error updating your information. Please try again, and if the issue persists, please contact the E-PASS Service Center: 407-82-EPASS (407-823-7277).";

                        $.notify({
                        		icon: 'fa fa-exclamation-triangle',
                        		message: errorResponse
                        },
                                {
                                		type: 'danger', delay: 5000, z_index: 3000
                    });
                    }
                    else {
                        return $.each(response.errors, function (index, value) {
                            $.notify({
                            		icon: 'fa fa-exclamation-triangle',
                            		message: value
                            },
                            {
                            		type: 'danger', delay: 5000, z_index: 3000
                        });
                    });
                }



        		}
        },
            	error: function (xhr, ajaxOptions, thrownError) {
                $.notify({
                	// Options
                		icon: 'fa fa-exclamation-triangle',
                		message: "There is an error updating your information.  Please try again, and if the issue persists, please contact the E-PASS Service Center: 407-82-EPASS (407-823-7277)"
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
    

    this.getAccountLoginInfo = function (event) {

    	//alert($('#loginDataForm').serializeArray());
        var form = $('#loginDataForm');
        var accountLoginInfoData = {};// serialize(form, { hash: true, empty: true });
        var crudeData = $('#loginDataForm').serializeObject();
        accountLoginInfoData = typeof (crudeData.Security) != "undefined" ? crudeData.Security : accountLoginInfoData;

        return accountLoginInfoData;
    	/*
        return {
            UserName: accountLoginInfoData["Security.UserName"],
            Password: accountLoginInfoData["Security.Password"],
            ConfirmPassword: accountLoginInfoData["Security.ConfirmPassword"],
            Email: accountLoginInfoData["Security.Email"],
            ConfirmEmail: accountLoginInfoData["Security.ConfirmEmail"],
        };*/
    };

    this.getCustomerDataInfo = function (event) {

    	//alert($('#customerDataForm').serializeArray());
        var form = $('#customerDataForm');
    	//var accountLoginInfoData = serialize(form, { hash: true, empty: true });

        var isPersonalAccount = true;

        if (typeof ($("ul#accountTypeTabs li.active")) != 'undefined') {
            if ($.trim($("ul#accountTypeTabs li.active").text()) != "Personal") //Check whether Personal or Business Account
                isPersonalAccount = false;
    }

        var customerInfoData = $('#customerDataForm').serializeObject();
        var customerInfoDataFinal = {};
        customerInfoDataFinal =
        {
        		ContactInfo: customerInfoData.Customer.ContactInfo,
        		CustomerInfo: customerInfoData.Customer.CustomerInfo,
    };

        customerInfoDataFinal.ContactInfo.State = customerInfoData.CustomerContactState;
        customerInfoDataFinal.CustomerInfo.PrimaryLicenseState = customerInfoData.PrimaryLicenseState;
        customerInfoDataFinal.CustomerInfo.SecondaryLicenseState = customerInfoData.SecondaryLicenseState;
        customerInfoDataFinal.CustomerInfo.IsBusinessAccount = !isPersonalAccount;

        return customerInfoDataFinal;

    }



};

$(function () {
    // If we are on the Login Page, set Focus to Username field
    if ($("#LoginModel_UserName").length) {
        $("#LoginModel_UserName").focus();
    }
    /*
    $('#newAccount').click(function () {
        if ($(this).hasClass('disabled') == false) {
            // Disable Button to prevent double-clicking
            $(this).addClass('disabled');

            var transponderNumber = $('#transponderNumber').val();

            // Authorize Transponder Number
            accountController.authorizeTransponder(transponderNumber);
        }
    });*/

    $('.new-account.continue-transaction').click(function (event) {

			var passesValidation = accountController.validateNewAccount();

        if (passesValidation) {
            passesValidation = false;
            passesValidation = accountController.validateSecurityInfoData();

            if (passesValidation) {

                //hide
                $("#createNewLoginFormPage").hide();
                $(".activateTransponder").hide();//Hide incase Activate transponder
                //$('#replaceAndActivateTransponder').append(".SalesTransponderSelection");

                $('.SalesTransponderSelection').prependTo("#replaceAndActivateTransponder");                //show
                $("#createNewCustomertFormPage").show();
                // $('#bcFlowImage').attr('src', getBaseUrl() + '/Content/img/ProgressBarAccount.svg');
                //ProgressBar Step 3
                $(".ProgressBarSteps").addClass("step-2").delay(730).queue(function () {
                    $("#accountStep .step-circle").addClass("active");
                });
                $(window).scrollTop(0);
            }
        }
    });

    $('.new-customer.continue-transaction').click(function (event) {    	
		var passesValidation = accountController.validateNewAccountContact(event);		
        //var passesValidation = true;
        if (passesValidation) {

            passesValidation = false;
            passesValidation = accountController.validateCustomerInfoData(event);

            if (passesValidation) {
                //hide
                $("#createNewCustomertFormPage").hide();

                $(".addTransponder").show();//Hide incase Activate transponder
                $('.addActivateTransponder').show();
                $('.searchTransponder').hide();
                $('.transponderSearch').hide();
                $('.transponderSearchDone').show();


                //$('#bcFlowImage').attr('src', getBaseUrl() + '/Content/img/ProgressBarPayment.svg');
                //ProgressBar Step 3
                $(".ProgressBarSteps").removeClass("step-2");
                $("#accountStep .step-circle").addClass("complete"); // - This makes sure the previous step is activated 
                $(".ProgressBarSteps").addClass("step-3");
                $("#paymentStep .step-circle").addClass("active");
               
                //Added per Document Flow Diagram to Fix Create Account
                //if used in another part of web page must be able to identify that...
                $(".PageTitleDetails").text("Payment Information");
                $(".PageTagline").text("Select your preferred E-PASS transponder(s) and complete the vehicle and payment information below.");
                $(window).scrollTop(0);
               
            }
        }

    });

    $('.new-customer.save-contact').click(function (event) {    	
        var passesValidation = accountController.validateNewAccountContact(event);
        //var passesValidation = true;
        if (passesValidation) {

            passesValidation = false;
            passesValidation = accountController.validateCustomerInfoData(event);

            if (passesValidation) {
                //hide

                $("#customerInfoForm").appendTo("#onboardingPart2");
                $("#contactEditPopup").modal('hide');

                $(".previewTransactionButton").click();
               
            }
        }

    });

    $('.new-customer.save-security').click(function (event) {
        var passesValidation = accountController.validateNewAccount(event);
        //var passesValidation = true;
        if (passesValidation) {

            passesValidation = false;
            passesValidation = accountController.validateSecurityInfoData(event);

            if (passesValidation) {
                //hide

                $("#securityInfoForm").appendTo("#onboardingPart1");
                $("#securityEditPopup").modal('hide');

                $(".previewTransactionButton").click();
            }
        }

    });

    $('.get-epass.save-payment, .activate-epass.save-payment').click(function (event) {
        var passesValidation = true;

        passesValidation = shoppingCartController.validateCartPaymentFromSave(event);

        if (passesValidation) {

            $(".paymentSection").appendTo("#addTransponderEditPanel");
            $("#paymentEditPopup").modal('hide');

            $(".previewTransactionButton").click();
        }

    });

    $('.get-epass.save-transponder, .activate-epass.save-transponder').click(function (event) {
        var passesValidation = true;
        passesValidation = shoppingCartController.validateCartTranspondersFromSave(event);


        if (passesValidation) {
            //hide
                $(".SalesTransponderSelection").appendTo("#addTransponderEditPanel:not(.PreviewOnPage):not(.activateTransponder)");
                $("#transponderEditPopup").modal('hide');
                $(".previewTransactionButton").click();

/*
            var returned = shoppingCartController.previewTransaction(event);

            $(".SalesTransponderSelection").appendTo("#addTransponderEditPanel:not(.PreviewOnPage):not(.activateTransponder)");
            $("#transponderEditPopup").modal('hide');
*/
            /*
            $(".transponderBodyPanel").find(".SalesTransponderSelection").appendTo(".fillTransponderPopup");
            $(".noShowOnPreviewSummaryPopup").hide();

            $(".showOnlyOnPopup").show();
            $("#transponderEditPopup").modal('show');
            */
        }

    });

    	$('#usernameHelp').popover({
    	    placement: 'top',
    	    html: true,
    	    trigger: 'hover', //<--- you need a trigger other than manual
    	    delay: {
    	        show: "500",
    	                hide: "100"
    	        }
    	        });

    $('#passwordHelp').popover({
        placement: 'top',
        html: true,
        trigger: 'hover', //<--- you need a trigger other than manual
        delay: {
            show: "500",
            hide: "100"
        }
    });

    $('#confirmPasswordHelp').popover({
        placement: 'top',
        html: true,
        trigger: 'hover', //<--- you need a trigger other than manual
        delay: {
            show: "500",
            hide: "100"
        }
    });

    /* [Begin] Prevent User from Returning to this Page after Submit */
    function disableBack() {
        window.history.forward();
    }

    window.onload = disableBack();
    window.onpageshow = function (event) { if (event.persisted) disableBack() }

    $(window).bind('unload', function () {
        disableBack();
    });
    /* [End] Prevent User from Returning to this Page after Submit */
});