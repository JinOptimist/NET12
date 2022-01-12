﻿$(document).ready(function () {
    let steps = 0;
    let width;

    $.get('/CellInfo/GetImage')
        .done(function (urls) {

            width = $('.carousel-image').css('width');

            setPrevImage();
            setNextImage();

            $('.carousel-image').click(function () {

                $('.current-image').animate({

                    width: 0
                },
                    2000,
                    function () {

                        setPrevImage();
                    });

                $('.next-image').animate({

                    width: width
                },
                    2000,
                    function () {

                        setNextImage();
                    });
            });

            function setPrevImage() {

                $('.current-image').attr('src', urls[steps]).css('width', width);
            }

            function setNextImage() {

                if (steps >= urls.length - 1) {

                    steps = 0;
                }
                else {

                    steps++;
                }

                $('.next-image').attr('src', urls[steps]).css('width', 0);
            }
        });
});