(function () {
     function fixedTable(element) {
        var body, header, firstColumn;
        body = (element).find('.customTable');
        firstColumn = (element).find('.firstColumn table');
        header = (element).find('.customHeader table');
        body.scroll(function () {
            header.css('margin-left', -(body).scrollLeft());
            firstColumn.css('margin-top', -(body).scrollTop());
        });
    };

    fixedTable($('#updatedTable'));
}());
