(function ($) {
    $.DatePickerControl = $.extend($.DatePickerControl || {}, {
        basePicker: null,
        hideOnSelect: true,
        placeholder: {
            'en': 'mm/dd/yyyy',
            'ru': 'дд.мм.гггг'
        },
        validation: {
            'en': '^(1[0-2]|0[1-9])/(3[01]|[12][0-9]|0[1-9])/[0-9]{4}$',
            'ru': '^(3[01]|[12][0-9]|0[1-9]).(1[0-2]|0[1-9]).[0-9]{4}$'
        },
        format: {
            'en': 'm/d/Y',
            'ru': 'd.m.Y'
        },
        language: 'ru',
        separator: {
            'en': '/',
            'ru': '.'
        },
        locale: {
            'en': {
                days: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'],
                daysShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                daysMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'],
                months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                monthsShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
            },
            'ru': {
                   days: ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"],
                   daysShort: ["Вск", "Пнд", "Втр", "Срд", "Чтв", "Птн", "Суб", "Вск"],
                   daysMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"],
                   months: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                   monthsShort: ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"]
            }
        }
    });

    function getCurrentDate() {
        var options = $(this).data('DatePickerControl-options');

        var sep = options.separator[options.language];
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


        if (options.language === 'en')
            today = mm + sep + dd + sep + yyyy;
        else
            today = dd + sep + mm + sep + yyyy;

        return today;
    }
    function validateDate(date) {
        var options = $(this).data('DatePickerControl-options');
       
        var value = date.value;
        var matches = value.match(options.validation[options.language]);
        if (matches == null)
            date.value = options.binded.getCurrentDate();
    }
    function hideHandler() {
        var options = $(this).data('DatePickerControl-options');
        options.binded.validateDate(this);
    }
    function renderHandler() {
        var options = $(this).data('DatePickerControl-options');
        this.placeholder = options.placeholder[options.language];
    }
    function showHandler() {
        //var options = $(this).data('DatePickerControl-options');
    }
    function show() {
        var options = $(this).data('DatePickerControl-options');
        options.basePicker.call($(this), 'show');
    }
    function hide() {
        var options = $(this).data('DatePickerControl-options');
        options.basePicker.call($(this), 'hide');
    }
    function setDate(date) {
        var options = $(this).data('DatePickerControl-options');
        if (typeof (date) !== "undefined" && date !== null)
            options.basePicker.call($(this), 'set_date', date);
    }
    function getDate(formatted) {
        var options = $(this).data('DatePickerControl-options');
        return options.basePicker.call($(this), 'get_date', formatted);
    }
    $.fn.DatePickerControl = function (initialOptions) {
        if (typeof initialOptions === 'string') {
            var data,
				parameters = Array.prototype.slice.call(arguments, 1);
            switch (initialOptions) {
                case 'hide':
                case 'show':
                    this.each(function () {
                        data = $(this).data('pickmeup-options');
                        if (data) {
                            data.binded[initialOptions]();
                        }
                    });
                    break;
                case 'get_date':
                    data = this.data('pickmeup-options');
                    if (data) {
                        return data.binded.get_date(parameters[0]);
                    } else {
                        return null;
                    }
                case 'set_date':
                    this.each(function () {
                        data = $(this).data('pickmeup-options');
                        if (data) {
                            data.binded[initialOptions].apply(this, parameters);
                        }
                    });
            }
            return this;
        }

        return this.each(function () {
            var $this = $(this);
            if ($this.data('DatePickerControl-options')) {
                return;
            }
            var i,
                option,
                options = $.extend({}, $.DatePickerControl, initialOptions || {});
            for (i in options) {
                if (options.hasOwnProperty(i)) {
                    option = $this.data('dpc-' + i);
                    if (typeof option !== 'undefined') {
                        options[i] = option;
                    }
                }
            }

            options.binded = {
                validateDate    : validateDate.bind(this),
                getCurrentDate  : getCurrentDate.bind(this),
                hide            : hide.bind(this),
                show            : show.bind(this),
                setDate         : setDate.bind(this),
                getDate         : getDate.bind(this)
            };

            options.basePicker = $this.pickmeup;
            $this.data('DatePickerControl-options', options);

            var settings = {};
            settings.locale = options.locale[options.language];
            settings.format = options.format[options.language];
            settings.hide_on_select = options.hideOnSelect;
            settings.hide = hideHandler;
            settings.show = showHandler;
            settings.render = renderHandler;

            $this.pickmeup(settings);
        });

    };
})(jQuery);