(function() {
    window.onresize = function() {
        initializeAutoResize();
    };
    initializeAutoResize();
}());

function initializeAutoResize() {
    var updatedTableWidth = document.getElementById("updatedTable").clientWidth; //container
    document.getElementsByClassName("customHeader")[0].style.width = (updatedTableWidth - 250) * 0.8 + "px"; // 250 - width of first column
    document.getElementsByClassName("customTable")[0].style.width = (updatedTableWidth - 250) * 0.8 + "px";
    document.getElementById("updatedTable").style.marginLeft = updatedTableWidth * 0.1 + "px";

    document.getElementsByClassName("customTable")[0].style.height = (document.body.clientHeight) * 0.8 - 150 + "px"; // total height of header and footer
    document.getElementsByClassName("firstColumn")[0].style.height = (document.body.clientHeight) * 0.8 - 150 + "px";
}
