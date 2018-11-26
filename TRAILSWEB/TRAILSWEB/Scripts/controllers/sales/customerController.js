var customerController = new function () {
    this.initializeCustomerInfoForm = function () {
        // Set Focus to first Data Entrypoint
        $('#FirstName').focus();

        var formIsValid = false;

        $('#saveCustomer').click(function (event) {
            if (!formIsValid) {
                event.stopPropagation();

                if (customerController.validate()) {
                    formIsValid = true;
                    $('#customerInfoForm').submit();
                }
            }
        })
    };

    this.validate = function () {
        $('#customerInfoForm').wrap('<form id="validationForm" />');

        $('#validationForm').validate({
            'rules': {
                'FirstName': {
                    'minlength': 1,
                    'maxlength': 100,
                    'required': true
                },
                'LastName': {
                    'minlength': 1,
                    'maxlength': 100,
                    'required': true
                },
                'AddressLine1': {
                    'minlength': 2,
                    'maxlength': 100,
                    'required': true
                },
                'AddressLine2': {
                    'minlength': 2,
                    'maxlength': 100
                },
                'City': {
                    'minlength': 2,
                    'maxlength': 30,
                    'required': true
                },
                'AddressStateSelected': {
                    'required': true
                },
                'ZipCode': {
                    'minlength': 5,
                    'maxlength': 5,
                    'digits': true,
                    'required': true
                },
                'PhoneNumber': {
                    'phoneUS': true
                },
                'EmailAddress': {
                    'minlength': 6,
                    'maxlength': 150,
                    'email': true,
                    'required': true
                },
                'DriversLicense': {
                    'minlength': 7,
                    'required': true
                },
                'DriversLicenseStateSelected': {
                    'required': true
                }
            },
            'messages': {
                'FirstName': {
                    'minlength': 'First Name must contain at least {0} characters',
                    'maxlength': 'First Name can not exceed more than {0} characters',
                    'required': 'First Name is required.'
                },
                'LastName': {
                    'minlength': 'Last Name must contain at least {0} characters',
                    'maxlength': 'Last Name can not exceed more than {0} characters',
                    'required': 'Last Name is required.'
                },
                'AddressLine1': {
                    'minlength': 'Address must contain at least {0} characters',
                    'maxlength': 'Address can not exceed more than {0} characters',
                    'required': 'Address is required.'
                },
                'AddressLine2': {
                    'minlength': 'Address Line 2 must contain at least {0} characters',
                    'maxlength': 'Address Line 2 can not exceed more than {0} characters'
                },
                'City': {
                    'minlength': 'City must contain at least {0} characters',
                    'maxlength': 'City can not exceed more than {0} characters',
                    'required': 'City is required.'
                },
                'AddressStateSelected': {
                    'required': 'A valid State is required.'
                },
                'ZipCode': {
                    'minlength': 'Zip Code must contain at least {0} characters',
                    'maxlength': 'Zip Code can not exceed more than {0} characters',
                    'digits': 'Zip Code can only contain numbers',
                    'required': 'Zip Code is required.'
                },
                'PhoneNumber': {
                    'phoneUS': 'A Valid U.S. Phone Number is required'
                },
                'EmailAddress': {
                    'minlength': 'Email Address must contain at least {0} characters',
                    'maxlength': 'Email Address can not exceed more than {0} characters',
                    'email': 'Please enter a valid Email Address',
                    'required': 'Email Address is required.'
                },
                'DriversLicense': {
                    'minlength': 'Driver License must contain at least {0} characters',
                    'required': 'Driver License is required'
                },
                'DriversLicenseStateSelected': {
                    'required': 'A valid Driver License issuing State is required'
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

        $('#customerInfoForm').unwrap();

        return isValid;
    };

};

$(function () {
    // Initialize Form
    customerController.initializeCustomerInfoForm();
});