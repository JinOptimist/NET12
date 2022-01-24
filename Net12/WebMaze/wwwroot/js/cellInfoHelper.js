$(document).ready(function () {

    $('.spoiler-body').css({ 'display': 'none' });
    $('.spoiler-botton').click(function () {
        $(this).next('.spoiler-body').slideToggle(500);
        return false;
    });


});


