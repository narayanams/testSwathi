var customerAgreementModalController = new function () {
    this.initialize = function () {
        $('#printAgreement').on('click', function () {
            var win = null;
            var content = $('#agreementDetail');

            // Create new "Printer Friendly" Page
            win = window.open("width=200,height=200");
            self.focus();
            win.document.open();
            win.document.write('<' + 'html' + '><' + 'head' + '><' + 'style' + '>');
            win.document.write('body, td { font-family: Verdana; font-size: 10pt; } pre { overflow-x: auto; white-space: pre-wrap; white-space: -moz-pre-wrap !important; white-space: -pre-wrap; white-space: -o-pre-wrap; word-wrap: break-word; background: #fff; }');
            win.document.write('<' + '/' + 'style' + '><title>E-PASS&reg; Customer Agreement</title><' + '/' + 'head' + '><' + 'body' + '>');
            win.document.write(content.html());
            win.document.write('<' + '/' + 'body' + '><' + '/' + 'html' + '>');
            win.document.close();

            // Print Page
            win.print();

            // Close "Printer Friendly" Page
            win.close();
        });
    };
};

var userLoginModalController = new function () {
    this.initialize = function (transponderNumber, agreementSelection, entryPoint) {
        // Initialize Transponder Number
        $('#TransponderNumber').val(transponderNumber);

        // Initialize Agreement Selection
        $('#AgreementConfirmation').val(agreementSelection);

        if (typeof (entryPoint) != 'undefined') {
            // Initialize Entrypoint
            $('#EntryPoint').val(entryPoint);
        }

        // Initialize Login Button Click Handler
        $('#loginButton').on('click', function (event) {
            event.preventDefault();

            var validForm = false;

            validForm = accountController.validateLogin(event);

            if (validForm) {
                accountController.login(event);
            }
        });


        

        // Set Focus to UserName Field
        $('#UserName').focus();
    };
};

var accountLoginModalController = new function () {
    this.initialize = function (transponderNumber, agreementSelection) {
        // Initialize Transponder Number
        $('#TransponderNumber').val(transponderNumber);

        // Initialize Agreement Selection
        $('#AgreementConfirmation').val(agreementSelection);

        // Define Base Popover Template
        var popoverBaseTemplate = '<div class="popover"><div class="arrow"></div><h3 class="popover-title {Style}"></h3><div class="popover-content"></div></div>';

        // Configure Styled Popover Templates
        var popoverInfoTemplate = popoverBaseTemplate.replace('{Style}', 'info');
        var popoverWarningTemplate = popoverBaseTemplate.replace('{Style}', 'warning');
        var popoverErrorTemplate = popoverBaseTemplate.replace('{Style}', 'danger');

        var pinAccountHelp = "<p>Your Account Number is listed on your statements.</p>";
        pinAccountHelp += "<p>If you do not know your PIN Number, please contact the <br /><a href=\"https://epass.cfxway.com/ServiceCenter/Help/Contact%20Information.pdf\" target=\"_blank\">Service Center <i class=\"fa fa-external-link\"></i></a>.</p>";

        // Account and Pin Popover Help Configuration
        $('#accountNumberHelp,#pinNumberHelp').popover({
            placement: 'right',
            template: popoverInfoTemplate,
            title: 'Account and PIN Help',
            content: pinAccountHelp,
            html: true
        });

        // Initialize Password Field
        $('#PinNumber').password();

        // Initialize User Login Button Click Handler
        $('#userLoginButton').on('click', function () {
            // Set Active Modal Dialog
            transponderController.SetActiveModal('UserLogin');

            // Display User Login
            swapRemoteDialog('#modalDialog', getBaseUrl() + '/Sales/UserLoginModal');
        });
    };
};

var userRegistrationModalController = new function () {
    this.validate = function () {
        var isUsernameUnique = accountController.UserNameIsUnique($('#UserName').val(), false);

        $('#userRegistrationForm').wrap('<form id="validationForm" />');

        // Check results of previous Unique Username Validation Check
        $.validator.addMethod("uniqueUsername", function (value, element) {
            return isUsernameUnique;
        }, "Username must be unique.");

        $('#validationForm').validate({
            'rules': {
                'TransponderNumber': {
                    'required': true,
                    'minlength': 13,
                    'maxlength': 13
                },
                'AgreementConfirmation': {
                    'required': true
                },
                'UserName': {
                    'required': true,
                    'uniqueUsername': true
                },
                'Password': {
                    'required': true
                },
                'ComparePassword': {
                    'required': true
                },
                'Email': {
                    'required': true,
                    'email': true,
                    'minlength': 6,
                    'maxlength': 150
                }
            },
            'messages': {
                'TransponderNumber': {
                    'required': 'Transponder Number is a required field',
                    'minlength': 'This field must contain at least {0} characters',
                    'maxlength': 'This field can not exceed more than {0} characters'
                },
                'AgreementConfirmation': {
                    'required': 'Acceptance of the Customer Agreement is required.'
                },
                'UserName': {
                    'required': 'User name is a required field',
                    'uniqueUsername': 'The chosen Username is already taken.'
                },
                'Password': {
                    'required': 'Password is a required field'
                },
                'ComparePassword': {
                    'required': 'Password is a required field'
                },
                'Email': {
                    'required': 'Email is a required field',
                    'minlength': 'This field must contain at least {0} characters',
                    'maxlength': 'This field can not exceed more than {0} characters',
                    'email': 'Please enter a valid Email Address'
                }
            },
            errorElement: 'span',
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

    this.registerUser = function () {
        /*
        if(this.validate()) {
            alert("Let's do something good!");
        }
        */
    };

    this.initialize = function (transponderNumber, agreementSelection) {
        // Initialize Transponder Number
        $('#TransponderNumber').val(transponderNumber);

        // Initialize Agreement Selection
        $('#AgreementConfirmation').val(agreementSelection);

        // Initialize Show/Hide Fields
        $('#Password,#ConfirmPassword,#PinNumber').password();

        $('#registerButton').on('click', function () {
            userRegistrationModalController.registerUser();
        });

        // Modal Close Event Handler
        $('#modalDialog').on('hidden.bs.modal', function () {
            // Close all active Notifications (if any)
            $.notifyClose();
        });
    };
};