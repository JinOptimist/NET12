$(document).ready(function () {

    $('textarea[name="Name"]').bind('input', function () {
        var stt = $(this).val();
        $("#i").text(stt);
    });

});


