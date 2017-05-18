onload = function ()
{
    $('#ApplicationReferenceIDs').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        disableIfEmpty: true,
        maxHeight: 200,
        buttonText: function (options, select) {
            return 'Application (' + options.length + ')';
        }
    });
}
