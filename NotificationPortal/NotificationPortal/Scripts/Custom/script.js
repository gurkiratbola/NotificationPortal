var subdir = '/';
if (window.location.origin.indexOf('localhost') === -1) {
    subdir = '/notificationportal/'
} else {
    subdir = '/';
}
// used for clickable row and ajax scripts (for right-click on all tables)
var domain = window.location.origin + subdir;
var clickableRow = function () {
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
        var hidden_menu_width_offset = $('table').offset().left - 15; //absolute position of table + ???
        var hidden_menu_height_offset = 54 - 3; // height of navbar + ???
        var rowId = $(this).attr("id");
        $('.hidden-menu li a').attr('href', function (i, str) {
            if (str.indexOf(domain) >= 0) {
                return str;
            } else {
                return domain + str + rowId;
            }
        });
        $(".hidden-menu").css({ position: "absolute", top: e.pageY - hidden_menu_height_offset, left: e.pageX - hidden_menu_width_offset });
        $(".hidden-menu").toggle();
        return false;
    });
}
// localstorage for sidebar dropdown
var sidebarDropdown = function () {
    if (localStorage.getItem("isDropdownVisible") == null) {
        localStorage.setItem("isDropdownVisible", false);

    } else {
        if (JSON.parse(localStorage.getItem("isDropdownVisible")) == false) {
            $(".sidebar-dropdown").hide();
        } else {
            $(".sidebar-dropdown").show();
        }
    }

    $(".sidebar-dropdown-button").click(function () {
        if (!JSON.parse(localStorage.getItem("isDropdownVisible"))) {
            localStorage.setItem("isDropdownVisible", true);
            $(".sidebar-dropdown-button .fa-caret-down").addClass("arrow-rotate");
            $(".sidebar-dropdown").slideDown();
        } else {
            $(".sidebar-dropdown").slideUp();
            if ($(".sidebar-dropdown-button .fa-caret-down").hasClass("arrow-rotate")) {
                $(".sidebar-dropdown-button .fa-caret-down").removeClass("arrow-rotate");
            } else {
                $(".sidebar-dropdown-button .fa-caret-down").addClass("arrow-rotate");
            }
            //localStorage.removeItem("isDropdownVisible");
            localStorage.setItem("isDropdownVisible", false);
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

$(document).ready(function ($) {
    clickableRow();// for clickable table rows
    sidebarDropdown();// for sidebar dropdown
    sidebarStateDetection();//sidebar state detection
});