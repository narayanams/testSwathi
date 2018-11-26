"use strict";
$(function () {
    // Creating spinner see http://fgnass.github.com/spin.js for configuration wizard
    var opts = {
        "lines": 13, // The number of lines to draw
        "length": 7, // The length of each line
        "width": 4, // The line thickness
        "radius": 10, // The radius of the inner circle
        "scale": 1, // Scales overall size of the spinner
        "corners": 1, // Corner roundness (0..1)
        "color": '#fff', // #rgb or #rrggbb or array of colors
        "opacity": 0.25, // Opacity of the lines
        "rotate": 0, // The rotation offset
        "direction": 1, // 1: clockwise, -1: counterclockwise
        "speed": 1, // Rounds per second
        "trail": 60, // Afterglow percentage
        "fps": 20, // Frames per second when using setTimeout() as a fallback for CSS
        "zIndex": 2e9, // The z-index (defaults to 2000000000)
        "className": 'spinner', // The CSS class to assign to the spinner
        "top": '50%', // Top position relative to parent
        "left": '50%', // Left position relative to parent
        "shadow": false, // Whether to render a shadow
        "hwaccel": false, // Whether to use hardware acceleration
        "position": 'absolute' // Element positioning
    };

    var spinner = new Spinner(opts);
    var ajax_cnt = 0; // Support for parallel AJAX requests

    /* Global functions to show/hide on AJAX requests */

    // Display Spinner when AJAX Call is Made (Get or Post)
    $(document).ajaxStart(function () {
        // Determine Center of Screen (Verticle Center of Window + Current Scroll Position - Spinner Height)
        var spinnerTop = Math.floor(window.innerHeight / 2) + $('body').scrollTop() - 75;

        $('<div id="progress-spinner" style="position: fixed; background: #333; color: #fff; width: 75px; height: 75px; top: ' + spinnerTop + 'px;left: 45%; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;z-index: 10000; overflow: visible;"></div>').appendTo('body');
        spinner.spin($('#progress-spinner')[0]);
        ajax_cnt++;
    });

    // Hide Spinner when AJAX Call is complete
    $(document).ajaxStop(function () {
        ajax_cnt--;
        spinner.stop();
        $('#progress-spinner').remove();
        ajax_cnt = 0;
    });

    // Hide Spinner when Error occurs during AJAX Call
    $(document).ajaxError(function () {
        spinner.stop();
        $('#progress-spinner').remove();
        ajax_cnt = 0;
    });
});
