$(document).ready(function () {
    /*let animationTime = 2 * 1000;
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

    });*/

    let i = 0;
    let count = $('.pic-item').length;

    $('.pic-item:not([index="' + i + '"])').hide();

    $('.right-button').click(function () {
        console.log(i);
        $('[index="' + i + '"]').hide();
        i = (i + 1) % count;
        $('[index="' + i + '"]').show();
        console.log(i);
    });

   /* $('.left-button').click(function () {
        console.log(i);
        $('[index="' + i + '"]').hide();
        i = Math.abs((i - 1) % count);
        $('[index="' + i + '"]').show();
        console.log(i);
    });*/

});


