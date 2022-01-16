$(document).ready(function () {

    let i = 0;
    let j = 0;
    let count = $('.pic-item').length;
    let animationTime = 2000;

    console.log(count);

    $('.pic-item:not([index="' + i + '"])').hide();

    $('.right-button').click(function () {
        console.log("change ");
        console.log(i);
        j = (i + 1) % count;
        console.log("to ");
        console.log(j);

        $('[index="' + j + '"]').css('left', '+=300');
        $('[index="' + j + '"]').show();

        $(function () {
            let k = i;
            $('[index="' + i + '"]').animate({
                left: "-=300"
            }, {
                duration: animationTime, queue: false, complete: function () {
                    $('[index="' + k + '"]').hide();
                    $('[index="' + k + '"]').css('left', '+=300');
                }
            });

            $('[index="' + j + '"]').animate({
                left: "-=300"
            }, {
                duration: animationTime, queue: false, complete: function () {
                    i = j;
                    console.log("after i");
                    console.log(i);
                    console.log("after j");
                    console.log(j);
                    console.log(" ");
                }
            });
        });
       
    });

    $('.left-button').click(function () {
        console.log("change ");
        console.log(i);
        j = i;
        if (i == 0) {
            j = count - 1;
        }
        else {
            j -= 1;
        }
        console.log("to ");
        console.log(j);

        $('[index="' + j + '"]').css('left', '-=300');
        $('[index="' + j + '"]').show();
       
        $(function () {
            let k = i;
            $('[index="' + i + '"]').animate({
                left: "+=300"
            }, {
                duration: animationTime, queue: false, complete: function() {
                    $('[index="' + k + '"]').hide();
                    $('[index="' + k + '"]').css('left', '-=300');
                }
            });

            $('[index="' + j + '"]').animate({
                left: "+=300"
            }, {
                duration: animationTime, queue: false, complete: function () {
                    i = j;
                    console.log("after i");
                    console.log(i);
                    console.log("after j");
                    console.log(j);
                    console.log(" ");
                }
            });
        });
        
    });


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


