$(document).ready(function () {
    $('.spoiler-body').css('display', 'none');
    $('.spoiler-botton').click(function (event) {
        event.preventDefault();
        $(this).next('.spoiler-body').slideToggle(500);
    });
});


