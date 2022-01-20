$(document).ready(function () {

    $('textarea[name="Name"]').bind('input', function () {
        let str = $(this).val();
        $("#i").text(str);
    });

});


