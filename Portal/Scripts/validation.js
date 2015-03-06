function Validatedate() {
    var beginDate = document.getElementById('<%=tbReportFromDate.ClientID %>').value;
    var endDate = document.getElementById('<%=tbReportToDate.ClientID %>').value;

    var datelPat = "(1[0-2]|0[1-9])/(3[01]|[12][0-9]|0[1-9])/[0-9]{4}$";

    var test = beginDate.match(datelPat);
    if (test == null) {
        alert("Web URL does not look valid");
       // document.getElementById("<%=txtURL.ClientID %>").focus();
        return false;
    }

}