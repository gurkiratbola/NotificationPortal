
var filterAppsBasedOnServer = function () {
    var serverRefIDs = $('#ServerList').val();
    $(".ApplicationListItem").hide();
    $.each(serverRefIDs, function (index, value) {
        $("." + value).show();
    });
}
var setupFilterDropDown = function () {
    $('#ServerList').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        buttonText: function (options, select) {
            return 'Server (' + options.length + ')';
        },
        onChange: function (option, checked, select) {
            filterAppsBasedOnServer();
        }
    });

    $('#ApplicationReferenceIDs').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        buttonText: function (options, select) {
            return 'Application (' + options.length + ')';
        }
    });
};

$(document).ready(function () {
    setupFilterDropDown();
    filterAppsBasedOnServer();
});