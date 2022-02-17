$(document).ready(function () {
    $('.good-button').click(function (evt) {
        evt.preventDefault();

        $.ajax({
            url: '/Gallery/Wonderful',
            data: { imageId: $(this).attr('data-id')}
        })
    });
});