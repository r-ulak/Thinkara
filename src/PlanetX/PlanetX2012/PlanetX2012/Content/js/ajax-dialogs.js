(function ($, window, undefined) {

    var Dialog = window.Dialog = {};

    $.extend(Dialog, {

        dialog: null,

        hasValidationErrors: false,

        Opening: function (options) {
            options = $.extend(true, {
                id: "modal-dialog"
            }, options || {}, {
                css: {
                    "display": "none"
                }
            });

            this.dialog = $('<div/>', options).appendTo(document.body);

            this._execOnCallback();
        },

        Updating: function () {
            this._execOnCallback();
        },

        Closing: function () {
            this._execOnCallback();
        },

        Open: function (options) {
            options = $.extend(true, {
                modal: true,
                width: "auto",
                resizable: false
            }, options || {}, {
                close: function () {
                    $(this).dialog('destroy').remove();
                }
            });

            this.dialog.dialog(options);

            this._execOnCallback();

            $.validator.unobtrusive.parse(this.dialog);
        },

        Update: function (options) {
            this._parseResponse(arguments);

            if (!this.hasValidationErrors) {
                options = options || {};
                for (var option in options) {
                    this.dialog.dialog("option", option, options[option]);
                }
            }

            this.dialog.dialog("option", "position", "center");

            this._execOnCallback();

            $.validator.unobtrusive.parse(this.dialog);
        },

        Close: function () {
            this._parseResponse(arguments);

            this._execOnCallback();

            if (!this.hasValidationErrors) {
                this.dialog.dialog('destroy').remove();
            }

            else $.validator.unobtrusive.parse(this.dialog);
        },

        _execOnCallback: function () {
            var caller = arguments.callee.caller,
					fn = (function (obj) {
					    for (var name in obj)
					        if (obj[name] === caller)
					            return obj["On" + name];
					})(this);

            if ($.isFunction(fn))
                fn.apply(this, caller.arguments);
        },

        _parseResponse: function (args) {
            var request = args.callee.caller.caller.arguments[2],
					headers = request.getAllResponseHeaders(),
					errors = (/X\-Validation\-Errors\s*:\s*(true|false)/i.exec(headers) || [, false])[1];

            this.hasValidationErrors = $.parseJSON(errors);
        }
    });

    var Dialogs = window.Dialogs = {};

//    Dialogs.CreateEvent = $.extend({}, Dialog, {
//        OnOpen: function () {
//            $('.date', this.dialog).datepicker();
//        },
//        OnUpdate: function () {
//            $('.date', this.dialog).datepicker();
//        }
//    });

} (jQuery, window));