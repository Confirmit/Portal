(function () {
    addDoubleScrollHandler($('#updatedTable'));
    window.onresize = function () {
        initializeAutoResize();
    };
    initializeAutoResize();

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
    var updatedTableWidth = document.getElementById("updatedTable").clientWidth; //container
    //var updatedTableWidth = document.body.clientWidth;
    document.getElementsByClassName("customHeader")[0].style.width = (updatedTableWidth - 252) * 0.8 - 16 + "px"; // 250 - width of first column
    document.getElementsByClassName("customTable")[0].style.width = (updatedTableWidth - 252) * 0.8 + "px";
    document.getElementById("updatedTable").style.marginLeft = updatedTableWidth * 0.1 + "px";

    document.getElementsByClassName("customTable")[0].style.height = (document.body.clientHeight) * 0.8 - 150 + "px"; // total height of header and footer
    document.getElementsByClassName("firstColumn")[0].style.height = (document.body.clientHeight) * 0.8 - 150 + "px";
}