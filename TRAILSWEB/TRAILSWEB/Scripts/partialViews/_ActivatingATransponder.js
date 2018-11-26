document.addEventListener("DOMContentLoaded", function () {
    console.log('_ActivatingATransponder Script Loading');

    $('body').on('keyup', '#Search_TransponderNumber', function (e) {
    //$('#Search_TransponderNumber').keyup(function (e) {
        var l = $('#Search_TransponderNumber').val().length;

        if (l === 13) {
            if ($('#Search_TransponderNumber').val() !== '') {
                //$("#saveTransponderInfo").prop("disabled", false);

                $.ajax({
                    url: getBaseUrl() + '/Sales/ValidateTransponder',
                    data: { 'transponderNumber': $('#Search_TransponderNumber').val(), 'selectedTrasnponderType': $('#CurrentSelectionTransponder').val() },
                    async: false,
                    type: 'post',
                    dataType: 'json',
                    beforeStart: function () {
                        $('#activateTransponderSpinner').removeClass('hidden');
                    },
                    success: function (response) {
                        // Return Validation Results                    
                        if (response.TransponderType == -1) {
                            //Error out

                            $.notify({
                                // Options
                                icon: 'fa fa-exclamation-triangle',
                                message: 'There was an error reading transponder number you entered. Please check transponder number.'
                            },
                            {
                                // Settings
                                type: 'warning',
                                delay: 5000,
                                z_index: 3000
                            });

                            $('#transponderNumberPlaceHolder').attr('style', 'display:none');
                        }
                        else {
                            $("#transponderNumberPlaceHolder").removeClass("hidden");
                            $('#transponderNumberPlaceHolder').attr('style', 'display:block');

                            var productTitle = response.ShortDescription == "Mini" ? "Sticker" : response.ShortDescription;
                            $(e.target).parents("#replaceAndActivateTransponder").find("#validateTransponder_0").html("true");
                            $('#Search_TransponderNumber').attr('dataValidated', 'true');
                            $(e.target).parents("#replaceAndActivateTransponder").find("#currentTransponderShortDescr_0").html(productTitle);
                            $(e.target).parents("#replaceAndActivateTransponder").find('#currentTransponderLongDescr_0').html(response.LongDescription);
                            $(e.target).parents("#replaceAndActivateTransponder").find('#currentTransponderType_0').text(response.TransponderType);
                            $(e.target).parents("#replaceAndActivateTransponder").find('#transponderImage_0').attr('src', response.ImagePath); //+ "?" + d.getTime()         
                        }
                    },
                    always: function () {
                        $('#activateTransponderSpinner').addClass('hidden');
                    }
                })
            }
        } else {
            $("#transponderNumberPlaceHolder").addClass("hidden");
            //$("#saveTransponderInfo").prop("disabled", true);
        }
    });

    $("#activateReplacedTransponderButton").click(function (event) {
        if ($('#Search_TransponderNumber').val().length != 13) {
            $.notify({
                // Options
                icon: 'fa fa-exclamation-triangle',
                message: 'Transponder number entered is invalid. Please check transponder number.'
            },
                       {
                           // Settings
                           type: 'warning',
                           delay: 5000,
                           z_index: 3000
                       });

            return false;
        }        
    });


    //$('#saveTransponderInfo').click(function (event) {       
    //    if (len != $("#Search_TransponderNumber").val().length) {
    //        $.notify({
    //            // Options
    //            icon: 'fa fa-exclamation-triangle',
    //            message: 'Transponder number entered is invalid. Please check transponder number.'
    //        },
    //        {
    //            // Settings
    //            type: 'warning',
    //            delay: 5000,
    //            z_index: 3000
    //        });

    //        event.preventDefault();
    //        return false;
    //    }
    //});
});