$(document).ready(function () {

    $(document).on('click', '.spoiler-trigger', function (e)
    {
        e.preventDefault();
        $(this)
            .toggleClass('active');
        $(this)
            .parent().
            find('.spoiler-block')
            .first()
            .slideToggle(300);
    })
});