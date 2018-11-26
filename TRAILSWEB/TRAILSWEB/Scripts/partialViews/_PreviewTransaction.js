///****///
//This is partial script to display Preview Transaction like Total, Tax, SubTotal etc... 
///****///


$(document).ready(function () {

    /// Below code is good for anchros added at runtime , mainly through reusable PartialViews
   /* $('body').on('click', 'a.editPage', function(e) 
    {
        debugger;
        var target = $(e.target).parent().attr("edit-for") // activated tab

        if (typeof (target) != 'undefined') {
            var editTarget = '#' + target + 'EditPanel';
            var reviewTarget = '#' + target + 'ReviewPanel';
            $(reviewTarget).hide();
            $(editTarget).show();
        }
    });*/
});


(function (window, document, $) {
    var defaults;

    // Plugin Core
    $.editSummary = function (opts) {

    }
    function loadScriptAgain()
    {

    }

    $.fn.editSummary = function (opts) {

        //return this.bind('click', function () {
        return $('body').on('click', '.editMyPage', function (e) {
            //debugger;
            var target = $(e.target).parent().attr("edit-for") // activated tab
            var role = $(e.target).parent().attr("edit-role") // activated tab

            if (typeof (target) != 'undefined') {
                var editTarget = '#' + target + 'EditPanel';
                var reviewTarget = '#' + target + 'ReviewPanel';
            }

            // Invoke callback
            opts.callback.call(this, {
                EditTarget: editTarget,
                EditAction: role,
                opts: opts
            });

        });

    };
})(this, this.document, this.jQuery);

/*
$('#purchaseTransponders').DataTable({
   
    "footerCallback": function (row, data, start, end, display) {
        alert('inCallBack');
        debugger;
            var api = this.api(), data;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;
            };

            // Total over all pages
            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(4, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(4).footer()).html(
                '$' + pageTotal + ' ( $' + total + ' total)'
            );
        }
    });


    -*/