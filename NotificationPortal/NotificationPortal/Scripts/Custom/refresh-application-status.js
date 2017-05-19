// get all reference ids for applications on the page
var getAppReferenceIDs = function () {
    var referenceIDs = [];
    $.each($('.clickable-row'), function (index, row) {
        referenceIDs.push($(row).attr('id'));
    });
    return referenceIDs;
}

// send a ajax request
var sendRequest = function (refIds) {
    if (!waiting) {
        showPreloader(refIds);
        $.ajax({
            type: "PUT",
            dataType: "json",
            url: domain + "api/Application",
            data: JSON.stringify(refIds),
            contentType: 'application/json',
            success: function (data) {
                displayNewStatus(data);
            },
            error: function (e) {
                displayFailedStatus();
            }
        });
    }
}

// deletes the current status and shows a preloader to indicate ajax request has been sent
var showPreloader = function (refIds) {
    waiting = true;
    $('.preloader').show();
}

// hide the preloader and display 
var displayNewStatus = function (data) {
    waiting = false;
    $('.preloader').hide();
    $('.current-status').remove();
    $.each(data, function (index, appStatus) {
        if (appStatus.Status == "Online") {
            $('<span class="current-status badge badge-success">Online</span>').appendTo($('#' + appStatus.ReferenceID).find('[data-title="Status"]'));
        } else {
            $('<span class="current-status badge badge-danger">Offline</span>').appendTo($('#' + appStatus.ReferenceID).find('[data-title="Status"]'));
        }
    });
}

// hide preloader and show failed badge instead of status (for failed ajax request)
var displayFailedStatus = function () {
    waiting = false;
    $('.preloader').hide();
}

// refresh application statuses displayed on the page
var refreshAppStatuses = function () {
    referenceIDs = getAppReferenceIDs();
    sendRequest(referenceIDs);
}

$(document).ready(function ($) {
    // initialize waiting as false
    waiting = false;
    refreshAppStatuses()
});