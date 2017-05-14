var currentSort = "";
var pageNumber = 1;

var sort = function (x) {
    currentSort = x;
    getNewList();
}

var refillTBody = function (threads) {
    $('#tbody').replaceWith('<tbody id="tbody" />');
    // add rows to table
    threads.forEach(
        function (thread) {
            addRow(thread);
        }
    );
    // make the row (right)clickable
    clickableRow();
}

var refillPagination = function (model) {
    // refill page info (Displaying x - y of z items)
    $('#pageinfo-display').replaceWith('<p id="pageinfo-display">' + model.ItemStart + " - " + model.ItemEnd + " of " + model.TotalItemsCount + (model.TotalItemsCount === 1 ? " item" : " items") + '</p>');
    // refill page numbers
    $('#pagination').replaceWith('<ul id="pagination" class="pagination" />');

    var leftBound, rightBound;
    switch (model.PageNumber) {
        case 1:
            leftBound = 1;
            rightBound = model.PageNumber + 4 > model.PageCount ? model.PageCount : model.PageNumber + 4;
            break;
        case 2:
            leftBound = 1;
            rightBound = model.PageNumber + 3 > model.PageCount ? model.PageCount : model.PageNumber + 3;
            break;
        case model.PageCount - 1:
            leftBound = model.PageNumber - 3 < 1 ? 1 : model.PageNumber - 3;
            rightBound = model.PageCount;
            break;
        case model.PageCount:
            leftBound = model.PageNumber - 4 < 1 ? 1 : model.PageNumber - 4;
            rightBound = model.PageCount;
            break;
        default:
            leftBound = model.PageNumber - 2 < 1 ? 1 : model.PageNumber - 2;
            rightBound = model.PageNumber + 2 > model.PageCount ? model.PageCount : model.PageNumber + 2;
            break;
    }

    if (model.PageNumber > 1) {
        $('<a onclick="changePage(' + (model.PageNumber - 1) + ')"><i class="fa fa-chevron-left" aria-hidden="true"></i></a>').appendTo($('<li/>').appendTo($('#pagination')));
    }
    for (var i = leftBound; i <= rightBound; i++) {
        var pageAnchor = '<a onclick="changePage(' + i + ')">' + i + '</a>';
        var pageListItem = '<li/>';
        if (model.PageNumber === i) {
            pageAnchor = '<a>' + i + '</a>';
            pageListItem = '<li  class="active"/>';
        }
        $(pageAnchor).appendTo($(pageListItem).appendTo($('#pagination')));
    }
    if (model.PageNumber < model.PageCount) {
        $('<a onclick="changePage(' + (model.PageNumber + 1) + ')"><i class="fa fa-chevron-right" aria-hidden="true"></i></a>').appendTo($('<li/>').appendTo($('#pagination')));
    }
}

var changePage = function (x) {
    pageNumber = x;
    getNewList(true);
}

var search = function (e) {
    var key = e.keyCode || e.which;
    if (key === 13) {
        getNewList();
    }
}