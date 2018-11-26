var getEPASSController = new function () {
    var _ActiveModal = '';
    var _EntryPoint = '';

    // Set a local reference to the Active Modal Dialog
    this.SetActiveModal = function (activeModal) {
        _ActiveModal = activeModal;
    };

    // Get Active Modal Dialog
    this.GetActiveModal = function () {
        return _ActiveModal;
    };

    // Set a local reference to the Entry Point Activity
    this.SetEntryPoint = function (entryPoint) {
        _EntryPoint = entryPoint;
    };

    // Get Entry Point Activity Reference
    this.GetEntryPoint = function () {
        return _EntryPoint.length > 0 ? _EntryPoint : 'GetEPASS';
    };
};

$(function () {
    $('#modalDialog').on('show.bs.modal', function (e) {
        var activeModal = getEPASSController.GetActiveModal();
        var transponderNumber = null;
        var agreementConfirmation = null;
        //var entryPoint = $('#EntryPoint').length > 0 ? $('#EntryPoint').val() : 'GetEPASS';
        var entryPoint = getEPASSController.GetEntryPoint();

        setTimeout(function () {
            switch (activeModal) {
                case 'UserLogin':
                    userLoginModalController.initialize(transponderNumber, agreementConfirmation, entryPoint);
                    break;
                case 'AccountLogin':
                    accountLoginModalController.initialize(transponderNumber, agreementConfirmation);
                    break;
                case 'RegisterUser':
                    userRegistrationModalController.initialize(transponderNumber, agreementConfirmation);
                    break;
                case 'ViewAgreement':
                    customerAgreementModalController.initialize();
                    break;
            }
        }, 1000);
    });

    $('#modalDialog').on('hide.bs.modal', function (e) {
        // Remove "Modal Large" Style Class if added previously (Agreement, etc.)
        $(this).removeClass('modal-large');

        // Add the "Fade" Modal Class in case it was removed for another Animation
        if (!$(this).hasClass('fade')) {
            $(this).addClass('fade');
        }
    });


    $('#addToExistingAccount').click(function () {
        if ($(this).hasClass('disabled') == false) {
            var targetDialog = $(this).attr('data-target');

            $(targetDialog).removeClass('fade');
            $(targetDialog).velocity('callout.pulse');

            // Display Modal Dialog
            $(targetDialog).modal('show');
        }
    });

    $('#EPASSConfirmButton').click(function () {
        var targetUrl = getBaseUrl() + $(this).attr('data-url');

        getEPASSController.SetActiveModal('UserLogin');

        // Load Modal Dialog Content
        $('#modalDialog').load(targetUrl);

        $('#modalDialog').removeClass('fade');
        $('#modalDialog').velocity('transition.slideDownBigIn');

        // Display Modal Dialog
        $('#modalDialog').modal('show');
    });

    $('#addToNewAccount, #otherAgencyConfirmButton').click(function (event) {
        event.preventDefault();

        if ($('#addToNewAccount').hasClass('disabled') == false) {
            // Disable Button to prevent double-clicking
            $('#addToNewAccount').addClass('disabled');

            var transponderType = $('#transponderType').val();
            var transponderPrice = $('#transponderPrice').val();

            // Authorize Transponder Number
            accountController.initiateGetEPASSSession(transponderType, transponderPrice);
        }
    });

    $('#viewAgreement').click(function () {
        var targetDialog = $(this).attr('data-target');
        var targetUrl = getBaseUrl() + $(this).attr('data-url');

        getEPASSController.SetActiveModal('ViewAgreement');

        // Load Modal Dialog Content
        $(targetDialog).load(targetUrl);
        $(targetDialog).addClass('modal-large');

        // Display Modal Dialog
        $(targetDialog).modal('show');
    });
});