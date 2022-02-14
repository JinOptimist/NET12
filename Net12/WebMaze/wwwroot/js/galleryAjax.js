$(document).ready(function () {
    $('.good-button').click(function (evt) {
        evt.preventDefault();

        let temp = $(this).attr('data-id');
        console.log(temp);

        $.ajax({
            url: '/Gallery/Wonderful',
            data: { imageId: temp }
        })
    });
});