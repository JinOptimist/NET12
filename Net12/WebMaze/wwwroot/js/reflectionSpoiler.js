$(document).ready(function () {

    $('.spoiler-trigger').click(function (e)
    {
        e.preventDefault();
        $(this)
            .toggleClass('active');
        $(this)
            .parent()
            .find('.spoiler-block')
            .first()
            .slideToggle(300);
    })
});