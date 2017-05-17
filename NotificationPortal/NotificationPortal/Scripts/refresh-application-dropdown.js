// used for CreateThread,Create,Edit for Notification views

// get all app based on selected servers
var getAppsBasedOnServer = function () {
    var serverReferenceIDs = $('#ServerList').val();
    $('#preloader').show();
    $.ajax({
        type: "POST",
        dataType: "json",
        // domain is defined in ~/Scripts/script.js
        url: domain + "api/Application",
        data: JSON.stringify(serverReferenceIDs),
        contentType: 'application/json',
        success: function (data) {
            refreshApplicationSelectOption(data);
            setupApplicationFilterDropDown()
            hidePreloader();
        },
        error: function (error) {
            jsonValue = jQuery.parseJSON(error.responseText);
            alert("error" + error.responseText);
            hidePreloader();
        }
    });
}

// setup the multi select plugin for the Server dropdown
// also disable the server dropdown menu items when value is changed (i.e. send new request)
var setupServerFilterDropDown = function () {
    $('#ServerList').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        buttonText: function (options, select) {
            return 'Server (' + options.length + ')';
        }
    });
    $('#ServerList').change(function (option, checked, select) {
        $('#ServerList option').each(function () {
            var input = $('input[value="' + $(this).val() + '"]');
            input.prop('disabled', true);
            input.parent('li').addClass('disabled');
        });
        getAppsBasedOnServer();
    });
};

// refresh the application dropdown
var refreshApplicationSelectOption = function (data) {
    $('#ApplicationList').replaceWith(`<div id="ApplicationList">
                <select style="display: none;" class="form-control valid" id="ApplicationReferenceIDs" name="ApplicationReferenceIDs" multiple="multiple" aria-invalid="false"></select>
                <span class="field-validation-valid text-danger" data-valmsg-for="ApplicationReferenceIDs" data-valmsg-replace="true"></span>
            </div>`);
    $.each(data, function (index, app) {
        $('<option value="' + app.ReferenceID + '" class="ApplicationListItem">' + app.ApplicationName + '</option>').appendTo($('#ApplicationReferenceIDs'))
    })
}

// setup the multi select plugin for the Application dropdown
// also re-enable the server dropdown menu items
var setupApplicationFilterDropDown = function () {
    $('#ApplicationReferenceIDs').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        disableIfEmpty: true,
        buttonText: function (options, select) {
            return 'Application (' + options.length + ')';
        }
    });

    $('#ServerList option').each(function () {
        var input = $('input[value="' + $(this).val() + '"]');
        input.prop('disabled', false);
        input.parent('li').addClass('disabled');
    });
}

// hide preloader
var hidePreloader = function () {
    $('#preloader').hide();
}

$(document).ready(function () {
    setupServerFilterDropDown();
    setupApplicationFilterDropDown();
});