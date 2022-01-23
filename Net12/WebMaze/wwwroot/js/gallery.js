$(document).ready(function () {

    let currentPicture = 0;
    let count = $('.pic-item').length;
    let animationTime = 2*1000;

    console.log("count of pictires " + count);

    $('.pic-item').hide();
    $('.pic-item[index="' + currentPicture + '"]').show(); 

    $('.right-button').click(function () {

        let nextPicture = (currentPicture + 1) % count;
        console.log("Swap picture " + currentPicture + " to " + nextPicture);

        swap('right', currentPicture, nextPicture);
        currentPicture = nextPicture;   
    });

    $('.left-button').click(function () {

        let nextPicture = currentPicture;
        if (currentPicture == 0) {
            nextPicture = count - 1;
        }
        else {
            nextPicture -= 1;
        }
        console.log("Swap picture " + currentPicture + " to " + nextPicture);

        swap('left', currentPicture, nextPicture);
        currentPicture = nextPicture;       
    });

    function swap(sideToMove, currentPicture, nextPicture) {
        let preparetingPosition, mainMove;
        if (sideToMove === 'left') {
            preparetingPosition = '-=300';
            mainMove = "+=300";
        } else {
            preparetingPosition = '+=300';
            mainMove = "-=300";
        }

        console.log(sideToMove + preparetingPosition + mainMove);
        $('[index="' + nextPicture + '"]').css('left', preparetingPosition);
        $('[index="' + nextPicture + '"]').show();

        let CopyCurrentPicture = currentPicture;
        $('[index="' + currentPicture + '"]').animate({
            left: mainMove
        }, {
            duration: animationTime, queue: false, complete: function() {
                $('[index="' + CopyCurrentPicture + '"]').hide();
                $('[index="' + CopyCurrentPicture + '"]').css('left', preparetingPosition);
            }
        });

        $('[index="' + nextPicture + '"]').animate({
            left: mainMove
        }, {
            duration: animationTime, queue: false, complete: function () {
                currentPicture = nextPicture;
                console.log("Swapped. Now current picture " + currentPicture);
            }
        });
    }
});


