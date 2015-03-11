function ValidateDate(value)
{
    var matches = value.match(/^(1[0-2]|0[1-9]).(3[01]|[12][0-9]|0[1-9]).[0-9]{4}$/);

    return (matches !== null);
}

function GetTodayDate()
{
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    today = mm + '.' + dd + '.' + yyyy;

    return today;
}