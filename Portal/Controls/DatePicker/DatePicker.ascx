<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="Controls_DatePicker" CodeBehind="DatePicker.ascx.cs"  %>



 <link rel="stylesheet" href="../Controls/DatePicker/cs/pickmeup.css">
 <script src="../Controls/DatePicker/js/jquery.js"></script>
 <script src="../Controls/DatePicker/js/jquery.pickmeup.min.js"></script>
 <script src="../Controls/DatePicker/js/validation.js"></script>

  <script>
      $(function () {
          $("[id$=datePicker]").pickmeup({
              hide_on_select: true,
              locale: {
                  days: ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"],
                  daysShort: ["Вск", "Пнд", "Втр", "Срд", "Чтв", "Птн", "Суб", "Вск"],
                  daysMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"],
                  months: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                  monthsShort: ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"],
                  today: "Сегодня"
              },
              format: 'm.d.Y'
          });

          $("[id$=datePicker]").focusout(function () {
              var date = this.value;
              if (!ValidateDate(date))
                  this.value = GetTodayDate();
          });

      });
  </script>

<asp:TextBox ID="datePicker" placeholder="MM.DD.YYYY" runat="server"></asp:TextBox>
