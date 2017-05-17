$(document).ready(function ($) {
    $('#ClientRefID').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 200,
        buttonText: function (options, select) {
            return 'Client (' + options.length + ')';
        }
    });
    $('#ServerReferenceIDs').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 200,
        buttonText: function (options, select) {
            return 'Server (' + options.length + ')';
        }
    });
})