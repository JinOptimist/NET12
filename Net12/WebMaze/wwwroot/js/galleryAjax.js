$(document).ready(function () {
    $('.good-button').click(function (evt) {
        if ($(this).attr('data-id') != "sortButton") {
            evt.preventDefault();

            if ($(this).hasClass("change-color-good")) {
                $(this).toggleClass("change-color-good");
            }
            else {
                $(this).addClass("change-color-good");
                $('.bad-button').removeClass("change-color-bad");
            }

            $.ajax({
                url: '/Gallery/Wonderful',
                data: { imageId: $(this).attr('data-id') }
            })
        }
        
    });

    $('.bad-button').click(function (evt) {
        evt.preventDefault();

        if ($(this).hasClass("change-color-bad")) {
            $(this).toggleClass("change-color-bad");
        }
        else {
            $(this).addClass("change-color-bad");
            $('.good-button').removeClass("change-color-good");
        }

        $.ajax({
            url: '/Gallery/Awful',
            data: { imageId: $(this).attr('data-id') }
        })
    });
});