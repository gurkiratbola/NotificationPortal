$(document).ready(function ($) {
    $('#ClientReferenceID').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 200,
        buttonText: function (options, select) {
            return 'Client (' + options.length + ')';
        }
    });
    $('#ApplicationReferenceIDs').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 200,
        disableIfEmpty: true,
        buttonText: function (options, select) {
            return 'Application (' + options.length + ')';
        }
    });
})