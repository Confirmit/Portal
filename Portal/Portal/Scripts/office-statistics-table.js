(function () {
    addDoubleScrollHandler($('#updatedTable'));
    window.onload = function () {
        initializeAutoResize();
    };
    window.onresize = function () {
        initializeAutoResize();
    };
}());

function addDoubleScrollHandler(element) {
    var body = (element).find('.customTable');
    var firstColumn = (element).find('.firstColumn table');
    var header = (element).find('.customHeader table');
    body.scroll(function () {
        header.css('margin-left', -(body).scrollLeft());
        firstColumn.css('margin-top', -(body).scrollTop());
    });
};
function initializeAutoResize() {
    var updatedTableWidth = $(document).width();

    var widthForSecondColumn = (updatedTableWidth - 250) * 0.90; // 250 px - ширина первой колонки
    if ($('.innerTable:eq(2)').width() < widthForSecondColumn) {
        widthForSecondColumn = $('.innerTable:eq(2)').width() + 20;
    }

    document.getElementsByClassName("customHeader")[0].style.width = widthForSecondColumn - 16 + "px"; // 16px - ширина скроллбара
    document.getElementsByClassName("customTable")[0].style.width = widthForSecondColumn + "px";
    document.getElementById("updatedTable").style.marginLeft = (updatedTableWidth - 250 - widthForSecondColumn) / 2 + "px";
    document.getElementsByClassName("customTable")[0].style.height = (document.body.clientHeight) * 0.8 - 150 + "px"; // 150px - высота футера и шапки
    document.getElementsByClassName("firstColumn")[0].style.height = (document.body.clientHeight) * 0.8 - 150 + "px";
}