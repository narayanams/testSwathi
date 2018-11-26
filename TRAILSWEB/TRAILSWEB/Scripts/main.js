$(function() {
    // Define Base Popover Template
    var popoverBaseTemplate = '<div class="popover"><div class="arrow"></div><h3 class="popover-title {Style}"></h3><div class="popover-content"></div></div>';

    // Configure Styled Popover Templates
    var popoverInfoTemplate = popoverBaseTemplate.replace('{Style}', 'info');
    var popoverWarningTemplate = popoverBaseTemplate.replace('{Style}', 'warning');
    var popoverErrorTemplate = popoverBaseTemplate.replace('{Style}', 'danger');

    // Initialize Carousel
    $('.carousel').carousel()

    // Configure Event Handler to Hide Popovers when they lose focus
    $('body').on('click', function (e) {
        $('[data-toggle="popover"]').each(function () {
            //the 'is' for buttons that trigger popups
            //the 'has' for icons within a button that triggers a popup
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
        });
    });

    $('#customerRegistrationForm').validate({
        rules: {
            firstName: {
                minlength: 1,
                maxlength: 100,
                required: true
            },
            lastName: {
                minlength: 1,
                maxlength: 100,
                required: true
            },
            addressLine1: {
                minlength: 2,
                maxlength: 100,
                required: true
            },
            addressLine2: {
                minlength: 2,
                maxlength: 100
            },
            city: {
                minlength: 2,
                maxlength: 30,
                required: true
            },
            addressState: {
                selectcheck: true
            },
            zipCode: {
                minlength: 5,
                maxlength: 5,
                digits: true,
                required: true
            },
            emailAddress: {
                minlength: 6,
                maxlength: 150,
                email: true,
                required: true
            },
            licensePlateNumber: {
                minlength: 7,
                maxlength: 7,
                pattern: /^[A-Z]{3}-[0-9]{3}$/, // Add a more appropriate Expression here
                required: true
            },
            licenseState: {
                selectcheck: true
            },
            driverLicenseNumber: {
                minlength: 3,
                maxlength: 15,
                required: true
            },
            driveLicenseState: {
                selectcheck: true
            }
        },
        messages: {
            firstName: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters',
                required: 'This field is required.'
            },
            lastName: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters',
                required: 'This field is required.'
            },
            addressLine1: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters',
                required: 'This field is required.'
            },
            addressLine2: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters'
            },
            city: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters',
                required: 'This field is required.'
            },
            addressState: {
                selectcheck: 'A valid State is required.'
            },
            zipCode: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters',
                digits: 'This field can only contain numbers',
                required: 'This field is required.'
            },
            emailAddress: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters',
                email: 'Please enter a valid Email Address',
                required: 'This field is required.'
            },
            licensePlateNumber: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters',
                pattern: 'A valid License Plate Number must be supplied (ABC-123)', // Update to be more correct for Expression used
                required: 'This field is required.'
            },
            licenseState: {
                selectcheck: 'A valid State is required.'
            },
            driverLicenseNumber: {
                minlength: 'This field must contain at least {0} characters',
                maxlength: 'This field can not exceed more than {0} characters',
                required: 'This field is required.'
            },
            driveLicenseState: {
                selectcheck: 'A valid State is required.'
            }
        },
        highlight: function(element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function(element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function(error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    jQuery.validator.addMethod('selectcheck', function (value) {
        return (value != '0');
    }, "A valid selection is required.");
});