(function ($) {
    $.pickmeup = $.extend($.pickmeup || {}, {
        hideOnSelect: true
    });

    function validateDate() {
        function isDate(testValue) {

            var returnValue = false;
            var testDate;
            try {
                testDate = new Date(testValue);
                if (!isNaN(testDate)) {
                    returnValue = true;
                }
                else {
                    returnValue = false;
                }
            }
            catch (e) {
                returnValue = false;
            }
            return returnValue;
        }
       
        var date = this.binded.get_date();
        if (!isDate(date))
            this.binded.set_date(new Date());
    }
    function hideHandler() {
        var options = $(this).data('pickmeup-options');
        options.validateDate();
    }
    function renderHandler() {
        var options = $(this).data('pickmeup-options');
        var placeholder = options.format;
        placeholder = placeholder.replace("m", "mm").replace("d", "dd").replace("Y", "yyyy");

        this.placeholder = placeholder;
    }
   
    $.fn.datePickerControl = function (initialOptions) {
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
            if ($this.data('pickmeup-options')) {
                return;
            }
            var i,
                option,
                options = $.extend({}, $.pickmeup, initialOptions || {});
            for (i in options) {
                if (options.hasOwnProperty(i)) {
                    option	= $this.data('pmu-' + i);
                    if (typeof option !== 'undefined') {
                        options[i] = option;
                    }
                }
            }

            options.format = options.format.toLowerCase();
            options.format = options.format.replace("mm", "m").replace("dd", "d").replace("yyyy", "Y");

            options.hide_on_select = options.hideOnSelect;
            options.hide = hideHandler;
            options.render = renderHandler;
            options.validateDate = validateDate;

            $this.pickmeup(options);
        });
    };
})(jQuery);