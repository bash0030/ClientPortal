$(document).ready(function () {
    setActiveLink();

    $('select').chosen();

    $(`[data-toggle="tooltip"]`).tooltip();

    $('.date-picker').datepicker({
        showOn: 'both',
        dateFormat: 'yy-mm-dd',
        onSelect: function () {
            toggleSubmit();
        }
    });

    $('#back_btn').click(function (e) {
        e.preventDefault();

        var url = $(this)[0].href;

        $('#question_form').animate({
            left: '100vw'
        }, 500, function () {
            location.href = url;
        });
    });

    $('#question_form').submit(function (e) {
        e.preventDefault();

        var form = this;

        $('#question_form').animate({
            left: '-100vw'
        }, 500, function () {
            form.submit();
        });
    });

    $('#cancelBtn').click(function (e) {
        e.preventDefault();

        var url = $(this)[0].href;

        $('#question_form').animate({
            top: '100vh'
        }, 500, function () {
            location.href = url;
        });
    });

    $('.nav-link').click(function (e) {
        e.preventDefault();
        $(this).toggleClass('active');
        window.location.href = $(this)[0].href;
    });
});