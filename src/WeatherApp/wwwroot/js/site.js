// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

jQuery.extend(jQuery.validator.methods, {
    number: function (value, element) {
        return this.optional(element)
            || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:[,.]\d+)?$/.test(value);
    }
});