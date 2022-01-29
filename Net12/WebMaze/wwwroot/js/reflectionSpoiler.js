$(document).ready(function () {

    $('.spoiler-trigger').click(function (e) {
        e.preventDefault();
        $(this)
            .toggleClass('active');
        $(this)
            .parent()
            .children('.spoiler-block')
            .slideToggle(300);
    });
});