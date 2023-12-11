(function ($) {
    $.fn.donetyping = function (callback) {
        var _this = $(this);
        var x_timer;
        _this.keyup(function (e) {
            clearTimeout(x_timer);
            x_timer = setTimeout(function () { clear_timer(e.target) }, 1000);
        });

        function clear_timer(e) {
            clearTimeout(x_timer);
            callback($(e));
        }
    }
})(jQuery);