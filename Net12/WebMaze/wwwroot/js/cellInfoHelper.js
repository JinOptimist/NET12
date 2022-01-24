$(document).ready(function () {

    $('.spoiler-body').css({ 'display': 'none' });
    $('.botton').click(function () {
        $(this).next('.spoiler-body').slideToggle(500);
        return false;
    });


});


