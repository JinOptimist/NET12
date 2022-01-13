$(document).ready(function () {
    let animationTime = 2 * 1000;
    $('.image').click(function () {
        let image = $(this);
        image.animate(
            {
                width: "+=100"
            },
            animationTime,
            function () {
                image.animate({ width: "-=100"}, animationTime);
            }
        );


    });


});


