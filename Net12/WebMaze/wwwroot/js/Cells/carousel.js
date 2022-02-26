$(document).ready(function () {
    let index = 0;
    let width = $('.carousel-image').css('width');
    let animateTime = 2 * 1000;

    $.get('/CellInfo/GetImage')
        .done(function (urls) {

            setCurrImage();
            setNextIndex();
            setNextImage();

            $('.carousel-image').click(function () {

                $('.current-image').animate({

                    width: 0
                },
                    animateTime,
                    function () {

                        setCurrImage();
                    });

                $('.next-image').animate({

                    width: width
                },
                    animateTime,
                    function () {

                        setNextIndex();
                        setNextImage();
                    });
            });

            function setCurrImage() {

                $('.current-image').attr('src', urls[index]).css('width', width);
            }

            function setNextImage() {

                $('.next-image').attr('src', urls[index]).css('width', 0);
            }

            function setNextIndex() {

                if (index >= urls.length - 1) {

                    index = 0;
                }
                else {

                    index++;
                }
            }
        });
});