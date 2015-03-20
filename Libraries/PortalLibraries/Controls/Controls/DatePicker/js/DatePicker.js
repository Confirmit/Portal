/**
 * @package		PickMeUp - jQuery datepicker plugin
 * @author		Nazar Mokrynskyi <nazar@mokrynskyi.com>
 * @author		Stefan Petre <www.eyecon.ro>
 * @copyright	Copyright (c) 2013-2015, Nazar Mokrynskyi
 * @copyright	Copyright (c) 2008-2009, Stefan Petre
 * @license		MIT License, see license.txt
 */
(function (d) {
    function getMaxDays() {
        var tmpDate = new Date(this.toString()),
			d = 28,
			m = tmpDate.getMonth();
        while (tmpDate.getMonth() === m) {
            ++d;
            tmpDate.setDate(d);
        }
        return d - 1;
    }
    d.addDays = function (n) {
        this.setDate(this.getDate() + n);
    };
    d.addMonths = function (n) {
        var day = this.getDate();
        this.setDate(1);
        this.setMonth(this.getMonth() + n);
        this.setDate(Math.min(day, getMaxDays.apply(this)));
    };
    d.addYears = function (n) {
        var day = this.getDate();
        this.setDate(1);
        this.setFullYear(this.getFullYear() + n);
        this.setDate(Math.min(day, getMaxDays.apply(this)));
    };
    d.getDayOfYear = function () {
        var now = new Date(this.getFullYear(), this.getMonth(), this.getDate(), 0, 0, 0);
        var then = new Date(this.getFullYear(), 0, 0, 0, 0, 0);
        var time = now - then;
        return Math.floor(time / 24 * 60 * 60 * 1000);
    };
})(Date.prototype);
(function ($) {
    var instancesCount = 0;
    $.pickmeup = $.extend($.pickmeup || {}, {
        date: new Date,
        default_date: new Date,
        flat: false,
        first_day: 1,
        prev: '&#9664;',
        next: '&#9654;',
        mode: 'single',
        select_year: true,
        select_month: true,
        select_day: true,
        view: 'days',
        calendars: 1,
        placeholder : {
            'en': 'mm/dd/yyyy',
            'ru': 'дд.мм.гггг'
        },
        localeFormat : {
            'en': 'm/d/Y',
            'ru': 'd.m.Y'
        },
        position: 'bottom',
        trigger_event: 'click touchstart',
        class_name: '',
        separator: ' - ',
        hide_on_select: true,
        min: null,
        max: null,
        render: function () { },
        change: function () { return true; },
        before_show: function () { return true; },
        show: function () { return true; },
        hide: function () { return true; },
        fill: function () { return true; },
        formatValidator : {
            'en': '^(1[0-2]|0[1-9])/(3[01]|[12][0-9]|0[1-9])/[0-9]{4}$',
            'ru': '^(3[01]|[12][0-9]|0[1-9]).(1[0-2]|0[1-9]).[0-9]{4}$'
        },
        localeSeparator : {
            'en': '/',
            'ru': '.'
        },
        language: 'en',
        locale: {
            'en' : {
                days: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'],
                daysShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                daysMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'],
                months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                monthsShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
            },
            'ru' :
			{
			    days: ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"],
			    daysShort: ["Вск", "Пнд", "Втр", "Срд", "Чтв", "Птн", "Суб", "Вск"],
			    daysMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"],
			    months: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
			    monthsShort: ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"],
			    today: "Сегодня"
			}
        }
    });
    var views = {
        years: 'pmu-view-years',
        months: 'pmu-view-months',
        days: 'pmu-view-days'
    },
		tpl = {
		    wrapper: '<div class="pickmeup" />',
		    head: function (d) {
		        var result = '';
		        for (var i = 0; i < 7; ++i) {
		            result += '<div>' + d.day[i] + '</div>';
		        }
		        return '<div class="pmu-instance">' +
					'<nav>' +
						'<div class="pmu-prev pmu-button">' + d.prev + '</div>' +
						'<div class="pmu-month pmu-button" />' +
						'<div class="pmu-next pmu-button">' + d.next + '</div>' +
					'</nav>' +
					'<nav class="pmu-day-of-week">' + result + '</nav>' +
				'</div>';
		    },
		    body: function (elements, containerClassName) {
		        var result = '';
		        for (var i = 0; i < elements.length; ++i) {
		            result += '<div class="' + elements[i].class_name + ' pmu-button">' + elements[i].text + '</div>';
		        }
		        return '<div class="' + containerClassName + '">' + result + '</div>';
		    }
		};

    function formatDate(date, format, locale) {
        var m = date.getMonth();
        var d = date.getDate();
        var y = date.getFullYear();
        var w = date.getDay();
        var hr = date.getHours();
        var pm = (hr >= 12);
        var ir = (pm) ? (hr - 12) : hr;
        var dy = date.getDayOfYear();
        if (ir === 0) {
            ir = 12;
        }
        var min = date.getMinutes();
        var sec = date.getSeconds();
        var parts = format.split(''), part;
        for (var i = 0; i < parts.length; i++) {
            part = parts[i];
            switch (part) {
            case 'a':
                part = locale.daysShort[w];
                break;
            case 'A':
                part = locale.days[w];
                break;
            case 'b':
                part = locale.monthsShort[m];
                break;
            case 'B':
                part = locale.months[m];
                break;
            case 'C':
                part = 1 + Math.floor(y / 100);
                break;
            case 'd':
                part = (d < 10) ? ("0" + d) : d;
                break;
            case 'e':
                part = d;
                break;
            case 'H':
                part = (hr < 10) ? ("0" + hr) : hr;
                break;
            case 'I':
                part = (ir < 10) ? ("0" + ir) : ir;
                break;
            case 'j':
                part = (dy < 100) ? ((dy < 10) ? ("00" + dy) : ("0" + dy)) : dy;
                break;
            case 'k':
                part = hr;
                break;
            case 'l':
                part = ir;
                break;
            case 'm':
                part = (m < 9) ? ("0" + (1 + m)) : (1 + m);
                break;
            case 'M':
                part = (min < 10) ? ("0" + min) : min;
                break;
            case 'p':
            case 'P':
                part = pm ? "PM" : "AM";
                break;
            case 's':
                part = Math.floor(date.getTime() / 1000);
                break;
            case 'S':
                part = (sec < 10) ? ("0" + sec) : sec;
                break;
            case 'u':
                part = w + 1;
                break;
            case 'w':
                part = w;
                break;
            case 'y':
                part = ('' + y).substr(2, 2);
                break;
            case 'Y':
                part = y;
                break;
            }
            parts[i] = part;
        }
        return parts.join('');
    }

    function fill() {
        var options = $(this).data('pickmeup-options'),
			pickmeup = this.pickmeup,
			currentCal = Math.floor(options.calendars / 2),
			actualDate = options.date,
			currentDate = options.current,
			minDate = options.min ? new Date(options.min) : null,
			maxDate = options.max ? new Date(options.max) : null,
			localDate,
			header,
			html,
			instance,
			today = (new Date).setHours(0, 0, 0, 0).valueOf(),
			shownDateFrom,
			shownDateTo,
			tmpDate;
        if (minDate) {
            minDate.setDate(1);
            minDate.addMonths(1);
            minDate.addDays(-1);
        }
        if (maxDate) {
            maxDate.setDate(1);
            maxDate.addMonths(1);
            maxDate.addDays(-1);
        }
        /**
		 * Remove old content except header navigation
		 */
        pickmeup.find('.pmu-instance > :not(nav)').remove();
        /**
		 * If several calendars should be shown
		 */
        for (var i = 0; i < options.calendars; i++) {
            localDate = new Date(currentDate);
            instance = pickmeup.find('.pmu-instance').eq(i);
            if (pickmeup.hasClass('pmu-view-years')) {
                localDate.addYears((i - currentCal) * 12);
                header = (localDate.getFullYear() - 6) + ' - ' + (localDate.getFullYear() + 5);
            } else if (pickmeup.hasClass('pmu-view-months')) {
                localDate.addYears(i - currentCal);
                header = localDate.getFullYear();
            } else if (pickmeup.hasClass('pmu-view-days')) {
                localDate.addMonths(i - currentCal);
                header = formatDate(localDate, 'B, Y', options.locale[options.language]);
            }
            if (!shownDateTo) {
                if (maxDate) {
                    // If all dates in this month (months in year or years in years block) are after max option - set next month as current
                    // in order not to show calendar with all disabled dates
                    tmpDate = new Date(localDate);
                    if (options.select_day) {
                        tmpDate.addMonths(options.calendars - 1);
                    } else if (options.select_month) {
                        tmpDate.addYears(options.calendars - 1);
                    } else {
                        tmpDate.addYears((options.calendars - 1) * 12);
                    }
                    if (tmpDate > maxDate) {
                        --i;
                        currentDate.addMonths(-1);
                        shownDateTo = undefined;
                        continue;
                    }
                }
            }
            shownDateTo = new Date(localDate);
            if (!shownDateFrom) {
                shownDateFrom = new Date(localDate);
                // If all dates in this month are before min option - set next month as current in order not to show calendar with all disabled dates
                shownDateFrom.setDate(1);
                shownDateFrom.addMonths(1);
                shownDateFrom.addDays(-1);
                if (minDate && minDate > shownDateFrom) {
                    --i;
                    currentDate.addMonths(1);
                    shownDateFrom = undefined;
                    continue;
                }
            }
            instance
				.find('.pmu-month')
				.text(header);
            html = '';
            var isYearSelected = function (year) {
                return (
							options.mode === 'range' &&
							year >= new Date(actualDate[0]).getFullYear() &&
							year <= new Date(actualDate[1]).getFullYear()
						) ||
						(
							options.mode === 'multiple' &&
							actualDate.reduce(function (prev, current) {
							    prev.push(new Date(current).getFullYear());
							    return prev;
							}, []).indexOf(year) !== -1
						) ||
						new Date(actualDate).getFullYear() === year;
            };
            var isMonthsSelected = function (year, month) {
                var firstYear = new Date(actualDate[0]).getFullYear(),
					lastyear = new Date(actualDate[1]).getFullYear(),
					firstMonth = new Date(actualDate[0]).getMonth(),
					lastMonth = new Date(actualDate[1]).getMonth();
                return (
							options.mode === 'range' &&
							year > firstYear &&
							year < lastyear
						) ||
						(
							options.mode === 'range' &&
							year === firstYear &&
							year < lastyear &&
							month >= firstMonth
						) ||
						(
							options.mode === 'range' &&
							year > firstYear &&
							year === lastyear &&
							month <= lastMonth
						) ||
						(
							options.mode === 'range' &&
							year === firstYear &&
							year === lastyear &&
							month >= firstMonth &&
							month <= lastMonth
						) ||
						(
							options.mode === 'multiple' &&
							actualDate.reduce(function (prev, current) {
							    current = new Date(current);
							    prev.push(current.getFullYear() + '-' + current.getMonth());
							    return prev;
							}, []).indexOf(year + '-' + month) !== -1
						) ||
						(
							new Date(actualDate).getFullYear() === year &&
							new Date(actualDate).getMonth() === month
						);
            };
            (function () {
                var years = [],
					startFromYear = localDate.getFullYear() - 6,
					minYear = new Date(options.min).getFullYear(),
					maxYear = new Date(options.max).getFullYear(),
					year;
                for (var j = 0; j < 12; ++j) {
                    year = {
                        text: startFromYear + j,
                        class_name: []
                    };
                    if (
						(
							options.min && year.text < minYear
						) ||
						(
							options.max && year.text > maxYear
						)
					) {
                        year.class_name.push('pmu-disabled');
                    } else if (isYearSelected(year.text)) {
                        year.class_name.push('pmu-selected');
                    }
                    year.class_name = year.class_name.join(' ');
                    years.push(year);
                }
                html += tpl.body(years, 'pmu-years');
            })();
            (function () {
                var months = [],
					currentYear = localDate.getFullYear(),
					minYear = new Date(options.min).getFullYear(),
					minMonth = new Date(options.min).getMonth(),
					maxYear = new Date(options.max).getFullYear(),
					maxMonth = new Date(options.max).getMonth(),
					month;
                for (var j = 0; j < 12; ++j) {
                    month = {
                        text: options.locale[options.language].monthsShort[j],
                        class_name: []
                    };
                    if (
						(
							options.min &&
							(
								currentYear < minYear ||
								(
									j < minMonth && currentYear === minYear
								)
							)
						) ||
						(
							options.max &&
							(
								currentYear > maxYear ||
								(
									j > maxMonth && currentYear >= maxYear
								)
							)
						)
					) {
                        month.class_name.push('pmu-disabled');
                    } else if (isMonthsSelected(currentYear, j)) {
                        month.class_name.push('pmu-selected');
                    }
                    month.class_name = month.class_name.join(' ');
                    months.push(month);
                }
                html += tpl.body(months, 'pmu-months');
            })();
            (function () {
                var days = [],
					currentMonth = localDate.getMonth(),
					day;
                // Correct first day in calendar taking into account first day of week (Sunday or Monday)
                (function () {
                    localDate.setDate(1);
                    var day = (localDate.getDay() - options.first_day) % 7;
                    localDate.addDays(-(day + (day < 0 ? 7 : 0)));
                })();
                for (var j = 0; j < 42; ++j) {
                    day = {
                        text: localDate.getDate(),
                        class_name: []
                    };
                    if (currentMonth !== localDate.getMonth()) {
                        day.class_name.push('pmu-not-in-month');
                    }
                    if (localDate.getDay() == 0) {
                        day.class_name.push('pmu-sunday');
                    } else if (localDate.getDay() == 6) {
                        day.class_name.push('pmu-saturday');
                    }
                    var fromUser = options.render(new Date(localDate)) || {},
						val = localDate.valueOf(),
						disabled = (options.min && options.min > localDate) || (options.max && options.max < localDate);
                    if (fromUser.disabled || disabled) {
                        day.class_name.push('pmu-disabled');
                    } else if (
						fromUser.selected ||
						options.date === val ||
						$.inArray(val, options.date) !== -1 ||
						(
							options.mode === 'range' && val >= options.date[0] && val <= options.date[1]
						)
					) {
                        day.class_name.push('pmu-selected');
                    }
                    if (val === today) {
                        day.class_name.push('pmu-today');
                    }
                    if (fromUser.class_name) {
                        day.class_name.push(fromUser.class_name);
                    }
                    day.class_name = day.class_name.join(' ');
                    days.push(day);
                    // Move to next day
                    localDate.addDays(1);
                }
                html += tpl.body(days, 'pmu-days');
            })();
            instance.append(html);
        }
        shownDateFrom.setDate(1);
        shownDateTo.setDate(1);
        shownDateTo.addMonths(1);
        shownDateTo.addDays(-1);
        pickmeup.find('.pmu-prev').css(
			'visibility',
			options.min && options.min >= shownDateFrom ? 'hidden' : 'visible'
		);
        pickmeup.find('.pmu-next').css(
			'visibility',
			options.max && options.max <= shownDateTo ? 'hidden' : 'visible'
		);
        options.fill.apply(this);
    }
    function parseDate(date, format, separator, locale) {
        if (date.constructor === Date) {
            return date;
        } else if (!date) {
            return new Date;
        }
        var splittedDate = date.split(separator);
        if (splittedDate.length > 1) {
            splittedDate.forEach(function (element, index, array) {
                array[index] = parseDate($.trim(element), format, separator, locale);
            });
            return splittedDate;
        }
        var monthsText;
        separator = new RegExp('[^0-9a-zA-Z(' + monthsText + ')]+');
        monthsText = locale.monthsShort.join(')(') + ')(' + locale.months.join(')(');
        var parts = date.split(separator),
			against = format.split(separator),
			d,
			m,
			y,
			h,
			min,
			now = new Date();
        for (var i = 0; i < parts.length; i++) {
            switch (against[i]) {
                case 'b':
                    m = locale.monthsShort.indexOf(parts[i]);
                    break;
                case 'B':
                    m = locale.months.indexOf(parts[i]);
                    break;
                case 'd':
                case 'e':
                    d = parseInt(parts[i], 10);
                    break;
                case 'm':
                    m = parseInt(parts[i], 10) - 1;
                    break;
                case 'Y':
                case 'y':
                    y = parseInt(parts[i], 10);
                    y += y > 100 ? 0 : (y < 29 ? 2000 : 1900);
                    break;
                case 'H':
                case 'I':
                case 'k':
                case 'l':
                    h = parseInt(parts[i], 10);
                    break;
                case 'P':
                case 'p':
                    if (/pm/i.test(parts[i]) && h < 12) {
                        h += 12;
                    } else if (/am/i.test(parts[i]) && h >= 12) {
                        h -= 12;
                    }
                    break;
                case 'M':
                    min = parseInt(parts[i], 10);
                    break;
            }
        }
        var parsedDate = new Date(
			y === undefined ? now.getFullYear() : y,
			m === undefined ? now.getMonth() : m,
			d === undefined ? now.getDate() : d,
			h === undefined ? now.getHours() : h,
			min === undefined ? now.getMinutes() : min,
			0
		);
        if (isNaN(parsedDate * 1)) {
            parsedDate = new Date;
        }
        return parsedDate;
    }

    function prepareDate(options) {
        var result;
        if (options.mode === 'single') {
            result = new Date(options.date);
            return [formatDate(result, options.localeFormat[options.language], options.locale[options.language]), result];
        } else {
            result = [[], []];
            $.each(options.date, function (nr, val) {
                var date = new Date(val);
                result[0].push(formatDate(date, options.localeFormat[options.language], options.locale[options.language]));
                result[1].push(date);
            });
            return result;
        }
    }

    function updateDate() {
        var $this = $(this),
			options = $this.data('pickmeup-options'),
			currentDate = options.current,
			newValue;
        switch (options.mode) {
            case 'multiple':
                newValue = currentDate.setHours(0, 0, 0, 0).valueOf();
                if ($.inArray(newValue, options.date) !== -1) {
                    $.each(options.date, function (index, value) {
                        if (value === newValue) {
                            options.date.splice(index, 1);
                            return false;
                        }
                        return true;
                    });
                } else {
                    options.date.push(newValue);
                }
                break;
            case 'range':
                if (!options.lastSel) {
                    options.date[0] = currentDate.setHours(0, 0, 0, 0).valueOf();
                }
                newValue = currentDate.setHours(0, 0, 0, 0).valueOf();
                if (newValue <= options.date[0]) {
                    options.date[1] = options.date[0];
                    options.date[0] = newValue;
                } else {
                    options.date[1] = newValue;
                }
                options.lastSel = !options.lastSel;
                break;
            default:
                options.date = currentDate.valueOf();
                break;
        }
        var preparedDate = prepareDate(options);
        if ($this.is('input')) {
            $this.val(options.mode === 'single' ? preparedDate[0] : preparedDate[0].join(options.separator));
        }
        options.change.apply(this, preparedDate);
        if (
			!options.flat &&
			options.hide_on_select &&
			(
				options.mode !== 'range' ||
				!options.lastSel
			)
		) {
            options.binded.hide();
            return false;
        }
        return false;
    }
    function click(e) {
        var el = $(e.target);
        if (!el.hasClass('pmu-button')) {
            el = el.closest('.pmu-button');
        }
        if (el.length) {
            if (el.hasClass('pmu-disabled')) {
                return false;
            }
            var $this = $(this),
				options = $this.data('pickmeup-options'),
				instance = el.parents('.pmu-instance').eq(0),
				root = instance.parent(),
				instanceIndex = $('.pmu-instance', root).index(instance);
            if (el.parent().is('nav')) {
                if (el.hasClass('pmu-month')) {
                    options.current.addMonths(instanceIndex - Math.floor(options.calendars / 2));
                    if (root.hasClass('pmu-view-years')) {
                        // Shift back to current date, otherwise with min value specified may jump on few (tens) years forward
                        if (options.mode !== 'single') {
                            options.current = new Date(options.date[options.date.length - 1]);
                        } else {
                            options.current = new Date(options.date);
                        }
                        if (options.select_day) {
                            root.removeClass('pmu-view-years').addClass('pmu-view-days');
                        } else if (options.select_month) {
                            root.removeClass('pmu-view-years').addClass('pmu-view-months');
                        }
                    } else if (root.hasClass('pmu-view-months')) {
                        if (options.select_year) {
                            root.removeClass('pmu-view-months').addClass('pmu-view-years');
                        } else if (options.select_day) {
                            root.removeClass('pmu-view-months').addClass('pmu-view-days');
                        }
                    } else if (root.hasClass('pmu-view-days')) {
                        if (options.select_month) {
                            root.removeClass('pmu-view-days').addClass('pmu-view-months');
                        } else if (options.select_year) {
                            root.removeClass('pmu-view-days').addClass('pmu-view-years');
                        }
                    }
                } else {
                    if (el.hasClass('pmu-prev')) {
                        options.binded.prev(false);
                    } else {
                        options.binded.next(false);
                    }
                }
            } else if (!el.hasClass('pmu-disabled')) {
                if (root.hasClass('pmu-view-years')) {
                    options.current.setFullYear(parseInt(el.text(), 10));
                    if (options.select_month) {
                        root.removeClass('pmu-view-years').addClass('pmu-view-months');
                    } else if (options.select_day) {
                        root.removeClass('pmu-view-years').addClass('pmu-view-days');
                    } else {
                        options.binded.update_date();
                    }
                } else if (root.hasClass('pmu-view-months')) {
                    options.current.setMonth(instance.find('.pmu-months .pmu-button').index(el));
                    options.current.setFullYear(parseInt(instance.find('.pmu-month').text(), 10));
                    if (options.select_day) {
                        root.removeClass('pmu-view-months').addClass('pmu-view-days');
                    } else {
                        options.binded.update_date();
                    }
                    // Move current month to the first place
                    options.current.addMonths(Math.floor(options.calendars / 2) - instanceIndex);
                } else {
                    var val = parseInt(el.text(), 10);
                    options.current.addMonths(instanceIndex - Math.floor(options.calendars / 2));
                    if (el.hasClass('pmu-not-in-month')) {
                        options.current.addMonths(val > 15 ? -1 : 1);
                    }
                    options.current.setDate(val);
                    options.binded.update_date();
                }
            }
            options.binded.fill();
        }
        return false;
    }

    function show(force) {
        var pickmeup = this.pickmeup;
        if (force || !pickmeup.is(':visible')) {
            var $this = $(this),
				options = $this.data('pickmeup-options'),
				pos = $this.offset(),
				viewport = {
				    l: document.documentElement.scrollLeft,
				    t: document.documentElement.scrollTop,
				    w: document.documentElement.clientWidth,
				    h: document.documentElement.clientHeight
				},
				top = pos.top,
				left = pos.left;
            options.binded.fill();
            if ($this.is('input')) {
                $this
					.pickmeup('set_date', parseDate($this.val() ? $this.val() : options.default_date, options.localeFormat[options.language], options.separator, options.locale[options.language]))
					.keydown(function (e) {
					    if (e.which === 9) {
					        $this.pickmeup('hide');
					    }
					});
                options.lastSel = false;
            }
            options.before_show();
            if (options.show() === false) {
                return;
            }
            if (!options.flat) {
                switch (options.position) {
                    case 'top':
                        top -= pickmeup.outerHeight();
                        break;
                    case 'left':
                        left -= pickmeup.outerWidth();
                        break;
                    case 'right':
                        left += this.offsetWidth;
                        break;
                    case 'bottom':
                        top += this.offsetHeight;
                        break;
                }
                if (top + pickmeup.offsetHeight > viewport.t + viewport.h) {
                    top = pos.top - pickmeup.offsetHeight;
                }
                if (top < viewport.t) {
                    top = pos.top + this.offsetHeight + pickmeup.offsetHeight;
                }
                if (left + pickmeup.offsetWidth > viewport.l + viewport.w) {
                    left = pos.left - pickmeup.offsetWidth;
                }
                if (left < viewport.l) {
                    left = pos.left + this.offsetWidth;
                }
                pickmeup.css({
                    display: 'inline-block',
                    top: top + 'px',
                    left: left + 'px'
                });
                $(document)
					.on(
						'mousedown' + options.events_namespace,
						options.binded.hide
					)
					.on(
						'resize' + options.events_namespace,
						[
							true
						],
						options.binded.forced_show
					);
            }
        }
    }
    function forcedShow() {
        show.call(this, true);
    }

    function hide(e) {
        var getCurrentDate = function (options) {
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
            today = dd + options.localeSeparator[options.language] + mm + options.localeSeparator[options.language] + yyyy;

            return today;
        };
        var validateDate = function (date, options) {

            var value = date.value;
            var matches = value.match(options.formatValidator[options.language]);
            if (matches == null)
                date.value = getCurrentDate(options);
        };
        if (
			!e ||
			!e.target ||														//Called directly
			(
				e.target !== this &&												//Clicked not on element itself
				!(this.pickmeup.get(0).compareDocumentPosition(e.target) & 16)	//And not o its children
			)
		) {
            var pickmeup = this.pickmeup,
				options = $(this).data('pickmeup-options');
            if (options.hide() !== false) {
                validateDate(this, options);
                pickmeup.hide();
                $(document)
					.off('mousedown', options.binded.hide)
					.off('resize', options.binded.forced_show);
                options.lastSel = false;
            }
        }
    }
    function update() {
        var options = $(this).data('pickmeup-options');
        $(document)
			.off('mousedown', options.binded.hide)
			.off('resize', options.binded.forced_show);
        options.binded.forced_show();
    }
    function clear() {
        var options = $(this).data('pickmeup-options');
        if (options.mode !== 'single') {
            options.date = [];
            options.lastSel = false;
            options.binded.fill();
        }
    }
    function prev(fill) {
        if (typeof fill == 'undefined') {
            fill = true;
        }
        var root = this.pickmeup;
        var options = $(this).data('pickmeup-options');
        if (root.hasClass('pmu-view-years')) {
            options.current.addYears(-12);
        } else if (root.hasClass('pmu-view-months')) {
            options.current.addYears(-1);
        } else if (root.hasClass('pmu-view-days')) {
            options.current.addMonths(-1);
        }
        if (fill) {
            options.binded.fill();
        }
    }
    function next(fill) {
        if (typeof fill == 'undefined') {
            fill = true;
        }
        var root = this.pickmeup;
        var options = $(this).data('pickmeup-options');
        if (root.hasClass('pmu-view-years')) {
            options.current.addYears(12);
        } else if (root.hasClass('pmu-view-months')) {
            options.current.addYears(1);
        } else if (root.hasClass('pmu-view-days')) {
            options.current.addMonths(1);
        }
        if (fill) {
            options.binded.fill();
        }
    }
    function getDate(formatted) {
        var options = $(this).data('pickmeup-options'),
			preparedDate = prepareDate(options);
        if (typeof formatted === 'string') {
            var date = preparedDate[1];
            if (date.constructor === Date) {
                return formatDate(date, formatted, options.locale[options.language]);
            } else {
                return date.map(function (value) {
                    return formatDate(value, formatted, options.locale[options.language]);
                });
            }
        } else {
            return preparedDate[formatted ? 0 : 1];
        }
    }
    function setDate(date) {
        var $this = $(this),
			options = $this.data('pickmeup-options');
        options.date = date;
        if (typeof options.date === 'string') {
            options.date = parseDate(options.date, options.localeFormat[options.language], options.separator, options.locale[options.language]).setHours(0, 0, 0, 0);
        } else if (options.date.constructor === Date) {
            options.date.setHours(0, 0, 0, 0);
        }
        if (!options.date) {
            options.date = new Date;
            options.date.setHours(0, 0, 0, 0);
        }
        if (options.mode !== 'single') {
            if (options.date.constructor !== Array) {
                options.date = [options.date.valueOf()];
                if (options.mode === 'range') {
                    options.date.push(((new Date(options.date[0])).setHours(0, 0, 0, 0)).valueOf());
                }
            } else {
                for (var i = 0; i < options.date.length; i++) {
                    options.date[i] = (parseDate(options.date[i], options.localeFormat[options.language], options.separator, options.locale[options.language]).setHours(0, 0, 0, 0)).valueOf();
                }
                if (options.mode === 'range') {
                    options.date[1] = ((new Date(options.date[1])).setHours(0, 0, 0, 0)).valueOf();
                }
            }
        } else {
            options.date = options.date.constructor === Array ? options.date[0].valueOf() : options.date.valueOf();
        }
        options.current = new Date(options.mode !== 'single' ? options.date[0] : options.date);
        options.binded.fill();
        if ($this.is('input')) {
            var preparedDate = prepareDate(options);
            $this.val(options.mode === 'single' ? preparedDate[0] : preparedDate[0].join(options.separator));
        }
    }
    function setLocale(language) {
        var $this = $(this),
			options = $this.data('pickmeup-options');
        options.language = language;
    }
    function destroy() {
        var $this = $(this),
			options = $this.data('pickmeup-options');
        $this.removeData('pickmeup-options');
        $this.off(options.events_namespace);
        $(document).off(options.events_namespace);
        $(this.pickmeup).remove();
    }
    $.fn.pickmeup = function (initialOptions) {
        if (typeof initialOptions === 'string') {
            var data,
				parameters = Array.prototype.slice.call(arguments, 1);
            switch (initialOptions) {
                case 'hide':
                case 'show':
                case 'clear':
                case 'update':
                case 'prev':
                case 'next':
                case 'destroy':
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
                case 'set_locale':
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
            if ($this.data('pickmeup-options')) {
                return;
            }
            var i,
				option,
				options = $.extend({}, $.pickmeup, initialOptions || {});
            for (i in options) {
                if (options.hasOwnProperty(i)) {
                    option = $this.data('pmu-' + i);
                    if (typeof option !== 'undefined') {
                        options[i] = option;
                    }
                }
            }
            // 4 conditional statements in order to account all cases
            if (options.view === 'days' && !options.select_day) {
                options.view = 'months';
            }
            if (options.view === 'months' && !options.select_month) {
                options.view = 'years';
            }
            if (options.view === 'years' && !options.select_year) {
                options.view = 'days';
            }
            if (options.view === 'days' && !options.select_day) {
                options.view = 'months';
            }
            options.calendars = Math.max(1, parseInt(options.calendars, 10) || 1);
            options.mode = /single|multiple|range/.test(options.mode) ? options.mode : 'single';
            if (typeof options.min === 'string') {
                options.min = parseDate(options.min, options.localeFormat[options.language], options.separator, options.locale[options.language]).setHours(0, 0, 0, 0);
            } else if (options.min && options.min.constructor === Date) {
                options.min.setHours(0, 0, 0, 0);
            }
            if (typeof options.max === 'string') {
                options.max = parseDate(options.max, options.localeFormat[options.language], options.separator, options.locale[options.language]).setHours(0, 0, 0, 0);
            } else if (options.max && options.max.constructor === Date) {
                options.max.setHours(0, 0, 0, 0);
            }
            if (!options.select_day) {
                if (options.min) {
                    options.min = new Date(options.min);
                    options.min.setDate(1);
                    options.min = options.min.valueOf();
                }
                if (options.max) {
                    options.max = new Date(options.max);
                    options.max.setDate(1);
                    options.max = options.max.valueOf();
                }
            }
            if (typeof options.date === 'string') {
                options.date = parseDate(options.date, options.localeFormat[options.language], options.separator, options.locale[options.language]).setHours(0, 0, 0, 0);
            } else if (options.date.constructor === Date) {
                options.date.setHours(0, 0, 0, 0);
            }
            if (!options.date) {
                options.date = new Date;
                options.date.setHours(0, 0, 0, 0);
            }
            if (options.mode !== 'single') {
                if (options.date.constructor !== Array) {
                    options.date = [options.date.valueOf()];
                    if (options.mode === 'range') {
                        options.date.push(((new Date(options.date[0])).setHours(0, 0, 0, 0)).valueOf());
                    }
                } else {
                    for (i = 0; i < options.date.length; i++) {
                        options.date[i] = (parseDate(options.date[i], options.localeFormat[options.language], options.separator, options.locale[options.language]).setHours(0, 0, 0, 0)).valueOf();
                    }
                    if (options.mode === 'range') {
                        options.date[1] = ((new Date(options.date[1])).setHours(0, 0, 0, 0)).valueOf();
                    }
                }
                options.current = new Date(options.date[0]);
                // Set days to 1 in order to handle them consistently
                if (!options.select_day) {
                    for (i = 0; i < options.date.length; ++i) {
                        options.date[i] = new Date(options.date[i]);
                        options.date[i].setDate(1);
                        options.date[i] = options.date[i].valueOf();
                        // Remove duplicates
                        if (
							options.mode !== 'range' &&
							options.date.indexOf(options.date[i]) !== i
						) {
                            delete options.date.splice(i, 1);
                            --i;
                        }
                    }
                }
            } else {
                options.date = options.date.valueOf();
                options.current = new Date(options.date);
                if (!options.select_day) {
                    options.date = new Date(options.date);
                    options.date.setDate(1);
                    options.date = options.date.valueOf();
                }
            }
            options.current.setDate(1);
            options.current.setHours(0, 0, 0, 0);
            var cnt,
				pickmeup = $(tpl.wrapper);
            this.pickmeup = pickmeup;
            if (options.class_name) {
                pickmeup.addClass(options.class_name);
            }
            this.placeholder = options.placeholder[options.language];
            var html = '';
            for (i = 0; i < options.calendars; i++) {
                cnt = options.first_day;
                html += tpl.head({
                    prev: options.prev,
                    next: options.next,
                    day: [
						options.locale[options.language].daysMin[(cnt++) % 7],
						options.locale[options.language].daysMin[(cnt++) % 7],
						options.locale[options.language].daysMin[(cnt++) % 7],
						options.locale[options.language].daysMin[(cnt++) % 7],
						options.locale[options.language].daysMin[(cnt++) % 7],
						options.locale[options.language].daysMin[(cnt++) % 7],
						options.locale[options.language].daysMin[(cnt++) % 7]
                    ]
                });
            }
            $this.data('pickmeup-options', options);
            for (i in options) {
                if (options.hasOwnProperty(i)) {
                    if (['render', 'change', 'before_show', 'show', 'hide'].indexOf(i) !== -1) {
                        options[i] = options[i].bind(this);
                    }
                }
            }
            options.binded = {
                fill: fill.bind(this),
                update_date: updateDate.bind(this),
                click: click.bind(this),
                show: show.bind(this),
                forced_show: forcedShow.bind(this),
                hide: hide.bind(this),
                update: update.bind(this),
                clear: clear.bind(this),
                prev: prev.bind(this),
                next: next.bind(this),
                get_date: getDate.bind(this),
                set_date: setDate.bind(this),
                set_locale: setLocale.bind(this),
                destroy: destroy.bind(this)
            };
            options.events_namespace = '.pickmeup-' + (++instancesCount);
            pickmeup
				.on('click touchstart', options.binded.click)
				.addClass(views[options.view])
				.append(html)
				.on(
					$.support.selectstart ? 'selectstart' : 'mousedown',
					function (e) {
					    e.preventDefault();
					}
				);
            options.binded.fill();
            if (options.flat) {
                pickmeup.appendTo(this).css({
                    position: 'relative',
                    display: 'inline-block'
                });
            } else {
                pickmeup.appendTo(document.body);
                // Multiple events support
                var triggerEvent = options.trigger_event.split(' ');
                for (i = 0; i < triggerEvent.length; ++i) {
                    triggerEvent[i] += options.events_namespace;
                }
                triggerEvent = triggerEvent.join(' ');
                $this.on(triggerEvent, options.binded.show);
            }
        });
    };
})(jQuery);
