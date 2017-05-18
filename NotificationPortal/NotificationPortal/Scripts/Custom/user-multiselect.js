$(document).ready(function ($) {
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