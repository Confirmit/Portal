var strDataValue;

function OnDataPickerFocus(oTxt)
{
    strDataValue = oTxt.value;
}

function OnChangeDateByType(oTxt)
{
   var txtvalue = oTxt.value;
   if  (IsRealTimeValue(txtvalue) == 1)
   {
       Ext.MessageBox.alert('Date Picker', "Date must be in format dd.mm.yyyy");
       txtvalue = strDataValue;
   }

   oTxt.value = txtvalue;
}

function IsRealTimeValue(txtvalue)
{
    if (txtvalue == "")
      return 0;
    var b = 0;
    
        var aDt = txtvalue.split(".");
        if(aDt && (aDt.length == 3))
        {
            if (aDt[2].length != 4 || isNaN(parseInt(aDt[2]))) b = 1;
            else if (parseInt(aDt[2]) == 0 && aDt[2] != "08" && aDt[2] != "09") { b = 1;}
            
            if (aDt[1].length != 2 || isNaN(parseInt(aDt[1]))) b = 1;
            else if (parseInt(aDt[1]) == 0 && aDt[1] != "08" && aDt[1] != "09") { b = 1;}
            
            if (parseInt(aDt[1]) < 0 || parseInt(aDt[1]) > 12)
                b = 1;

            if (aDt[0].length != 2 || isNaN(parseInt(aDt[0]))) b = 1;
            else if (parseInt(aDt[0]) == 0 && aDt[0] != "08" && aDt[0] != "09") { b = 1;}
            
            if (parseInt(aDt[1]) < 0 || parseInt(aDt[1]) > 31)
                b = 1;
        }
        else b = 1;
    
    return b;
}

function DatePicker(name)
{
     this.name = name;
     this.dt = new Date();
//     document.write('<span id="'+name+'" name="'+name+'" style="z-index:1; position:absolute; visibility:hidden" class="DatePicker"></span>');
}

DatePicker.prototype.show = function(dt, x, y, callback)
{
     if(dt) this.dt = dt;
     this.callback = callback;
     // if not rendered yet, do so
     if(!this.oSpan) this.render();
     // set coordinates
     this.oSpan.style.left = (x + 'px');// - 810;
     this.oSpan.style.top= (y + 'px');// - 77;
     this.oSpan.style.zIndex = 1000;
     this.fill();
     this.oSpan.style.visibility = "visible";
     this.oMonth.focus();
}

DatePicker.prototype.hide = function()
{
     if ( this.oSpan ) this.oSpan.style.visibility = "hidden";
}

DatePicker.prototype.render = function()
{
     var oT1, oTR1, oTD1, oTH1;
     var oT2, oTR2, oTD2;
     this.oSpan = document.getElementById(this.name);
     this.oSpan.appendChild(oT1 = document.createElement("table"));
     //oT1.width = "200";
     oT1.style.background = "white";
     oT1.border = 1;
     oT1.style.left = this.oSpan.style.left;
     oT1.style.top = this.oSpan.style.top;
     oTR1 = oT1.insertRow(oT1.rows.length);
     oTD1 = oTR1.insertCell(oTR1.cells.length);
     oTD1.colSpan = 7;
     oTD1.className = 'DatePickerHdr';
     oT2 = document.createElement("table");
     oT2.width = "100%";
     oTD1.appendChild(oT2);
     oT2.border = 0;
     // New row.
     oTR2 = oT2.insertRow(oT2.rows.length);
     // Previous month.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.prevMonth;
     oTD2.onclick = function() { this.oDatePicker.onPrev(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_prev").value + "\">";
     oTD2.className = 'DatePickerHdrBtn';
     // Month combo.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.monthTitle;
     this.oMonth = document.createElement("select");
     oTD2.appendChild(this.oMonth);
     this.oMonth.oDatePicker = this;
     this.oMonth.onchange = this.oMonth.onkeyup = function() { this.oDatePicker.onMonth(); }
     for(var i = 0; i < 12; i++)
     {
		this.oMonth.add(new Option(this.texts.months[i], i),undefined);
     }
     this.oMonth.className = 'DatePickerHdrBtn';
     // Year combo.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.yearTitle;
     this.oYear = document.createElement("select");
     oTD2.appendChild(this.oYear);
     this.oYear.oDatePicker = this;
     this.oYear.onchange = this.oYear.onkeyup = function() { this.oDatePicker.onYear(); }
     for(i = this.minYear; i <= this.maxYear; i++)
     {
		this.oYear.add(new Option(i, i),undefined);
     }
     this.oYear.className = 'DatePickerHdrBtn';
     // Next month.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.nextMonth;
     oTD2.onclick = function() { this.oDatePicker.onNext(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_next").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
	 // Close button.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.close;
     oTD2.onclick = function() { this.oDatePicker.hide(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_close").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
     oTD2.title = "Close";
	 // Next rows (dates).
     oTR1 = oT1.insertRow(oT1.rows.length);
     for ( i = 0; i < 7; i++ )
     {
        oTH1 = document.createElement("th");
        oTR1.appendChild(oTH1);
        oTH1.innerHTML = this.texts.days[i];
        oTH1.className = 'DatePickerHdr';
     }
     this.aCells = new Array;
     for ( var j = 0; j < 6; j++ )
     {
        this.aCells.push(new Array);
        oTR1 = oT1.insertRow(oT1.rows.length);
        for ( i = 0; i < 7; i++ )
        {
           this.aCells[j][i] = oTR1.insertCell(oTR1.cells.length);
           this.aCells[j][i].oDatePicker = this;
           this.aCells[j][i].onclick =
              function() { this.oDatePicker.onDay(this); }
        }
     }
     // New buttons.
     oTR1 = oT1.insertRow(oT1.rows.length);
     oTD1 = oTR1.insertCell(oTR1.cells.length);
     oTD1.colSpan = 7;
     oTD1.className = 'DatePickerHdr';
     oT2 = document.createElement("table");
     oT2.width = "100%";
     oTD1.appendChild(oT2);
     oT2.border = 0;
     // New row.
     oTR2 = oT2.insertRow(oT2.rows.length);
     // Month ago button.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.monthAgo;
     oTD2.align = "center";
     oTD2.onclick = function() { this.oDatePicker.onMonthAgo(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_monthago").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
     // Week ago button.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.weekAgo;
     oTD2.align = "center";
     oTD2.onclick = function() { this.oDatePicker.onWeekAgo(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_weekago").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
     // Yesterday button.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.yesterday;
     oTD2.align = "center";
     oTD2.onclick = function() { this.oDatePicker.onYesterday(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_yesterday").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
     // Today button.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.today;
     oTD2.align = "center";
     oTD2.onclick = function() { this.oDatePicker.onToday(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_today").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
     // Tomorrow button.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.tomorrow;
     oTD2.align = "center";
     oTD2.onclick = function() { this.oDatePicker.onTomorrow(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_tomorrow").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
     // Week after button.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.weekAfter;
     oTD2.align = "center";
     oTD2.onclick = function() { this.oDatePicker.onWeekAfter(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_weekafter").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
     // Month after button.
     oTD2 = oTR2.insertCell(oTR2.cells.length);
     oTD2.title = this.texts.monthAfter;
     oTD2.align = "center";
     oTD2.onclick = function() { this.oDatePicker.onMonthAfter(); }
     oTD2.oDatePicker = this;
     oTD2.innerHTML = "<img src=\"" + document.getElementById("btn_monthafter").value + "\">";;
     oTD2.className = 'DatePickerHdrBtn';
}

DatePicker.prototype.fill = function()
{
     // first clear all
     this.clear();
     // place the dates in the calendar
     var nRow = 0;
     var d = new Date(this.dt.getTime());
     var m = d.getMonth();
     
     //this is for bug with 00:xx time
     if (d.getHours() == 0)
        d.setHours(1);
     
     for ( d.setDate(1); d.getMonth() == m; d.setTime(d.getTime() + 86400000) ) {
        var nCol = d.getDay();
        
        if(nCol == 0) nCol = 7;
        nCol = nCol-1;
        
        this.aCells[nRow][nCol].innerHTML = d.getDate();
        if ( d.getDate() == this.dt.getDate() ) {
           this.aCells[nRow][nCol].className = 'DatePickerBtnSelect';
        }
        if ( nCol == 6 ) nRow++;
     }
     // set the month combo
     this.oMonth.value = m;
     // set the year text
     this.oYear.value = this.dt.getFullYear();
}

DatePicker.prototype.clear = function()
{
	for(var j = 0; j < 6; j++)
	{
		for(var i = 0; i < 7; i++)
        {
           this.aCells[j][i].innerHTML = "&nbsp;"
           this.aCells[j][i].className = 'DatePickerBtn';
		}
	}
}

DatePicker.prototype.onPrev = function()
{
     if(this.dt.getMonth() == 0)
     {
        this.dt.setFullYear(this.dt.getFullYear()-1);
        this.dt.setMonth(11);
     }
     else
     {
        this.dt.setMonth(this.dt.getMonth()-1);
     }
     //this.callback(this.dt);
     this.fill();
}

DatePicker.prototype.onNext = function()
{
     if(this.dt.getMonth() == 11)
     {
        this.dt.setFullYear(this.dt.getFullYear()+1);
        this.dt.setMonth(0);
     }
     else
     {
        this.dt.setMonth(this.dt.getMonth()+1);
     }
     //this.callback(this.dt);
     this.fill();
}

DatePicker.prototype.onMonth = function()
{
     this.dt.setMonth(this.oMonth.value);
     this.fill();
}

DatePicker.prototype.onYear = function()
{
     this.dt.setYear(this.oYear.value);
     this.fill();
}

DatePicker.prototype.onDay = function(oCell)
{
     var d = parseInt(oCell.innerHTML);
     if(d > 0)
     {
        this.dt.setDate(d);
        this.hide();
        this.callback(this.dt, this.oDatePicker);
     }
}

DatePicker.prototype.onToday = function()
{
	this.dt = new Date();
	this.hide();
    this.callback(this.dt, this.oDatePicker);
}

DatePicker.prototype.onYesterday = function()
{
	today = this.dt;
	this.dt.setDate(today.getDate() - 1);
	
	this.hide();
    this.callback(this.dt, this.oDatePicker);
}

DatePicker.prototype.onTomorrow = function()
{
	today = this.dt;
	this.dt.setDate(today.getDate() + 1);
	
	this.hide();
    this.callback(this.dt, this.oDatePicker);
}

DatePicker.prototype.onWeekAgo = function()
{
	today = this.dt;
	this.dt.setDate(today.getDate() - 7);
	
	this.hide();
    this.callback(this.dt, this.oDatePicker);
}

DatePicker.prototype.onWeekAfter = function()
{
	today = this.dt;
	this.dt.setDate(today.getDate() + 7);

	this.hide();
    this.callback(this.dt, this.oDatePicker);
}

DatePicker.prototype.onMonthAgo = function()
{
    today = this.dt;
    this.dt.setMonth(today.getMonth() - 1);

	this.hide();
    this.callback(this.dt, this.oDatePicker);
}

DatePicker.prototype.onMonthAfter = function()
{
	today = this.dt;
	this.dt.setMonth(today.getMonth() + 1);
	
    this.hide();
    this.callback(this.dt, this.oDatePicker);
}

DatePicker.prototype.texts = {
     months: [ "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" ],
     close: "Hide",
     days: ["MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN"],
     monthTitle: "Month",
     prevMonth: "Last month",
     nextMonth: "Next month",
     yearTitle: "Year",
     today: "Today",
     yesterday: "Day ago",
     tomorrow: "Day after",
     weekAgo: "Week ago",
     weekAfter: "Week after",
     monthAgo: "Month ago",
     monthAfter: "Month After"
}

function callback(dt, oDatePicker)
{
    var strDate = "";
    if (parseInt(dt.getDate()) <= 9) {strDate = "0";} 
    strDate = strDate + dt.getDate() + ".";
             
    if (parseInt(dt.getMonth()) <= 8) {strDate = strDate + "0";}
    strDate = strDate + (dt.getMonth()+1) + ".";
         
    strDate = strDate + dt.getFullYear();
   
    if (oDatePicker != null && oDatePicker.client != null)
    {
        oDatePicker.client.value = strDate;
        oDatePicker.client.onchange();   
        if (oDatePicker.autoPostBack == "True")  
        {
            document.hiddenElement.value = 'change';
            try{
                document.aspnetForm.submit();
            }catch(err){
                Ext.MessageBox.alert('Date Picker', err.description);
            }
        }
    }
    //oDatePicker = null;  
}

function DatePickerValue_onkeypress(oTxt, autoPostBack, hiddenId)
{
    if (autoPostBack == "True")
    {
        if (window.event.keyCode == 13)
        {
            if (IsRealTimeValue(oTxt.value) == 0)
            {
                var hiddenElement = document.getElementById(hiddenId);
                hiddenElement.value = 'change';
                try{
                    document.aspnetForm.submit();
                }catch(err){
                    Ext.MessageBox.alert('Date Picker', err.description);
                }
            }
        }
    }
}

function getElementPosition(elem)
{
    var offsetTrail = elem.offsetParent;
    var offsetLeft = 0;
    var offsetTop = 0;

    while(offsetTrail != null && offsetTrail.style.position != "absolute")
    {
        offsetLeft += offsetTrail.offsetLeft;
        offsetTop += offsetTrail.offsetTop;
        offsetTrail = offsetTrail.offsetParent;
    }
    
    offsetTop += elem.offsetHeight;
    offsetTop += 5;
    if ((navigator.userAgent.indexOf("Mac") != -1)&&(typeof document.body.leftMargin != "undefined"))
    {
        offsetLeft += document.body.leftMargin;
        offsetTop += document.body.topMargin;
    }
    return {left:offsetLeft, top:offsetTop};
}

function DatePickerShow(oTxt, oBtn, clientID, oDataP, minYear, maxYear, valueIfNull, autoPostBack, hiddenId)
{
   if (oDataP == null) 
    oDataP = new DatePicker(clientID);
      
   oDataP.oDatePicker = oDataP;
  
  oDataP.oDatePicker.autoPostBack = autoPostBack;
  if (autoPostBack = 'True')
  {
    document.hiddenElement = document.getElementById(hiddenId);
  }
   
     if(!document.getElementById) return;
     // since we control the text format in callback(), getting the date is easy
     
     var strText = oTxt.value;
     
     if (strText == null || strText.length == 0)
        strText = valueIfNull;
        
     var aDt = strText.split(".");
     var dt = null;
     if(aDt && (aDt.length == 3))
     {
        dt = new Date();
        if (parseInt(aDt[2]) == 0) {
            if (aDt[2] == "08") {dt.setFullYear(8);}
            if (aDt[2] == "09") {dt.setFullYear(9);}
        }
        else {dt.setFullYear(parseInt(aDt[2]));}
		if (parseInt(aDt[1]) == 0) {
            if (aDt[1] == "08") {dt.setMonth(7);}
            if (aDt[1] == "09") {dt.setMonth(8);}
        }
        else {dt.setMonth(parseInt(aDt[1])-1);}
        if (parseInt(aDt[0]) == 0) {
            if (aDt[0] == "08") {dt.setDate(8);}
            if (aDt[0] == "09") {dt.setDate(9);}
        }
        else {dt.setDate(parseInt(aDt[0]));}
	 }
	 
	 if (maxYear < minYear)
	    maxYear = minYear;
	 
	 oDataP.oDatePicker.minYear = minYear;
     oDataP.oDatePicker.maxYear = maxYear;
     
     oDataP.oDatePicker.client = oTxt;
     pos = getElementPosition(oBtn);
     oDataP.oDatePicker.show(dt, pos.left, pos.top, callback);
     return (oDataP.oDatePicker);
}
