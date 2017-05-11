var domain = window.location.pathname;
domain = domain.substr(0, domain.lastIndexOf('/'));

$(document).ready(function ($) {
    // for right click functinoality
    const HIDDEN_MENU_WIDTH_OFFSET = 130;
    const HIDDEN_MENU_HEIGHT_OFFSET = 80;
    $(".hidden-menu").hide();
    $(".clickable-row").click(function () {
        window.location = domain + $(this).data("href");
    });
    $(".clickable-row-dashboard").click(function () {
        //window.location = window.location.origin + $(this).data("href");
        window.location = $(this).data("href");
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
                return domain + str + rowId;
            }
        });
        $(".hidden-menu").css({ position: "absolute", top: e.pageY - HIDDEN_MENU_HEIGHT_OFFSET, left: e.pageX - HIDDEN_MENU_WIDTH_OFFSET });
        $(".hidden-menu").toggle();
        return false;
    });


    // for sidebar dropdown
    if (localStorage.getItem("isDropdownVisible") == null) {
        localStorage.setItem("isDropdownVisible", false);
        //$(".sidebar-dropdown").hide();
    } else {
        if (localStorage.getItem("isDropdownVisible") == false) {
            $(".sidebar-dropdown").hide();
        } else {
            $(".sidebar-dropdown").show();
        }
    }
    
    $(".sidebar-dropdown-button").click(function() {
        if ($(".sidebar-dropdown").is(":hidden")){
            localStorage.setItem("isDropdownVisible", true);
            $(".sidebar-dropdown-button .fa-caret-down").addClass("arrow-rotate");
            $(".sidebar-dropdown").slideDown();
        } else {
            $(".sidebar-dropdown").slideUp();
            $(".sidebar-dropdown-button .fa-caret-down").removeClass("arrow-rotate");
            localStorage.removeItem("isDropdownVisible");
        }
    })

    //sidebar state detection
    var url = window.location.href;
    var activePage = url;
    $('.sidebar-dropdown a').each(function () {
        var linkPage = this.href;

        if (activePage == linkPage) {
            $(this).closest("li").addClass("active");
        }
    });

    // for profile email small print
    $(".profile-email").append("<small class='float-right'>Updating email will log user out instantly</small>")
});