// used for Create,Edit for User views

// get all app based on selected client
var getAppsBasedOnClient = function () {
    var clientReferenceID = $('#ClientReferenceID').val();
    $('#preloader').show();
    $.ajax({
        type: "GET",
        // domain is defined in ~/Scripts/Custom/script.js
        url: domain + "api/User/" + clientReferenceID,
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

// setup the multi select plugin for the Client dropdown
// also disable the client dropdown menu items when value is changed (i.e. send new request)
var setupClientFilterDropDown = function () {
    $('#ClientReferenceID').multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 200
    });
    $('#ClientReferenceID').change(function (option, checked, select) {
        $('#ClientReferenceID option').each(function () {
            var input = $('input[value="' + $(this).val() + '"]');
            input.prop('disabled', true);
            input.parent('li').addClass('disabled');
        });
        getAppsBasedOnClient();
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
        maxHeight: 200,
        buttonText: function (options, select) {
            return 'Application (' + options.length + ')';
        }
    });

    $('#ClientReferenceID option').each(function () {
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
    setupClientFilterDropDown();
    var clientReferenceID = $('#ClientReferenceID').val();
    if (clientReferenceID !== null && clientReferenceID !== "" && typeof (clientReferenceID)!=="undefined") {
        getAppsBasedOnClient();
    }
});