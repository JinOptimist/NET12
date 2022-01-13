$(document).ready(function () {

    let i = 0;
    let j = 0;
    let count = $('.pic-item').length;

    $('.pic-item:not([index="' + i + '"])').hide();

    $('.right-button').click(function () {     
        console.log(i);
        $('[index="' + i + '"]').hide();
        i = (i + 1) % count;
        $('[index="' + i + '"]').show();
        console.log(i);
    });

    $('.left-button').click(function () {
        console.log(count);
        console.log(i);
        $('[index="' + i + '"]').hide();
        if (i == 0) {
            i = count;
        }
        else {
            i -= 1;
        }
        $('[index="' + i + '"]').show();
        console.log(i);
    });

    /*$('.right-button').click(function () {
        console.log(i);
        j = (i + 1) % count;

        function () {
            $('[index="' + i + '"]').animate({

            });
        }

        *//*$('[index="' + i + '"]').animate();*//*
       *//* $('[index="' + i + '"]').show();*//*

        i = j;
        console.log(i);
    });*/
    

    /*$(function () {
        $("#first").animate({
            width: '200px'
        }, { duration: 200, queue: false });

        $("#second").animate({
            width: '600px'
        }, { duration: 200, queue: false });
    });*/

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
});


