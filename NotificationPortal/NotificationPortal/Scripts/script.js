var subdir = '/';
if (window.location.origin.indexOf('localhost') === -1) {
    subdir = '/notificationportal/'
} else {
    subdir = '/';
}
// used for clickable row and ajax scripts (for right-click on all tables)
var domain = window.location.origin + subdir;

var clickableRow = function () {
    const HIDDEN_MENU_WIDTH_OFFSET = 170;
    const HIDDEN_MENU_HEIGHT_OFFSET = 50;
    $(".hidden-menu").hide();
    $(".clickable-row").click(function () {
        var x = $(this).data("href");
        window.location =  domain + $(this).data("href");
    });

    $('*').click(function (e) {
        if (e.target.className !== 'clickable-row') {
            $(".hidden-menu").hide();
        }
    });
    $(".clickable-row").contextmenu(function (e) {
        var rowId = $(this).attr("id");
        $('.hidden-menu li a').attr('href', function (i, str) {
            if (str.indexOf(rowId) >= 0) {
                return str;
            } else {
                return window.location.origin + subdir + str + rowId;
            }
        });
        $(".hidden-menu").css({ position: "absolute", top: e.pageY - HIDDEN_MENU_HEIGHT_OFFSET, left: e.pageX - HIDDEN_MENU_WIDTH_OFFSET });
        $(".hidden-menu").toggle();
        return false;
    });
}

// localstorage for sidebar dropdown
var sidebarDropdown = function () {
    if (localStorage.getItem("isDropdownVisible") === null) {
        localStorage.setItem("isDropdownVisible", false);
        //$(".sidebar-dropdown").hide();
    } else {
        if (localStorage.getItem("isDropdownVisible") === false) {
            $(".sidebar-dropdown").hide();
        } else {
            $(".sidebar-dropdown").show();
        }
    }

    $(".sidebar-dropdown-button").click(function () {
        if ($(".sidebar-dropdown").is(":hidden")) {
            localStorage.setItem("isDropdownVisible", true);
            $(".sidebar-dropdown-button .fa-caret-down").addClass("arrow-rotate");
            $(".sidebar-dropdown").slideDown();
        } else {
            $(".sidebar-dropdown").slideUp();
            $(".sidebar-dropdown-button .fa-caret-down").removeClass("arrow-rotate");
            localStorage.removeItem("isDropdownVisible");
        }
    })
}

// state detection for dropdown on sidebar
var sidebarStateDetection = function () {
    var url = window.location.href;
    var activePage = url;
    $('.sidebar-dropdown a').each(function () {
        var linkPage = this.href;

        if (activePage === linkPage) {
            $(this).closest("li").addClass("active");
        }
    });
}

// hide alert messages after 2s
var hideAlert = function () {
    const DELAY = 2000;
    var alertSuccess = $(".alert-success");
    var alertDanger = $(".alert-danger");
    if (alertSuccess.css('display') !== 'none' || alertDanger.css('display') !== 'none') {
        setTimeout(function () {
            alertSuccess.parent('.form-group > div').slideUp();
            alertDanger.parent('.form-group > div').slideUp();
            alertSuccess.slideUp();
            alertDanger.slideUp();
        }, DELAY);
    }
}

var truncate = function () {
    var e = $("h5");
    //console.log(e.prop('scrollWidth'))
    //console.log(e.width())

    //$('h5').each(function (index, obj) {
    //    while ($(this).prop('scrollWidth') > $(this).width()) {
    //        //alert("Overflow");
    //        //drop last word
    //        var str = e.children("a").html().trim();
    //        console.log(str);
    //        $(this).children("a").text(str.substring(0, str.lastIndexOf(" ")));
    //    }
    //});

    // if "... exist", hide word bn space and "..." 
    $('h5').each(function (index, obj) {
        if (e.children("a").html().trim().indexOf("…") > 0) {
            console.log("exists");
        }
    });
}

$(document).ready(function ($) {
    
    clickableRow();// for clickable table rows
    sidebarDropdown();// for sidebar dropdown
    sidebarStateDetection();//sidebar state detection
    hideAlert();// hiding alert boxes after 2s
    truncate();
    $(window).on('resize', function () {
        //var win = $(this); //this = window
        truncate();
    });
});