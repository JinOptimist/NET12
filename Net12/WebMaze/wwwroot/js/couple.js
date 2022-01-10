$(document).ready(function () {
    let indexOfOpenedCard = undefined;
    let gameUrls = [];

    let steps = 0;
    let openedCardsCount = 0;
    let rotationTime= 1000;

    init();

    function init() {
        $.get('/Maze/GetUrlsForCouple')
            .done(function (urls) {
                gameUrls = urls.concat(urls);

                for (var i = 0; i < 100; i++) {
                    const firstCardIndex = getRandomInt(0, gameUrls.length);
                    const secondCardIndex = getRandomInt(0, gameUrls.length);

                    const temp = gameUrls[firstCardIndex];
                    gameUrls[firstCardIndex] = gameUrls[secondCardIndex];
                    gameUrls[secondCardIndex] = temp;
                }

                for (let i = 0; i < gameUrls.length; i++) {
                    const url = gameUrls[i];

                    let newCardBlock = $('.card-block.template').clone();

                    newCardBlock.removeClass('template');

                    newCardBlock
                        .find('.main-image')
                        .attr('src', url);

                    newCardBlock.click(onCardBlockClick);

                    newCardBlock.attr('data-id', i);

                    $('.game-field').append(newCardBlock);

                    setTimeout(function () { spin(newCardBlock, 'X'); }, 2000);
                }
            });
    }

    function onCardBlockClick() {
        steps++;
        const cardBlock = $(this);
        if (indexOfOpenedCard == undefined) {
            spin(cardBlock);
            indexOfOpenedCard = cardBlock.data('id');
            return;
        }

        let clickedId = cardBlock.data('id');
        if (indexOfOpenedCard == clickedId) {
            spin(cardBlock);
            indexOfOpenedCard = undefined;
            return;
        }

        const alreadyOpenedUrl = gameUrls[indexOfOpenedCard];
        const clicekdUrl = cardBlock.find('.main-image').attr('src');

        const firstCardBlock = $(`[data-id=${indexOfOpenedCard}]`);

        spin(cardBlock);

        if (alreadyOpenedUrl == clicekdUrl) {
            setTimeout(function () {
                spin(firstCardBlock);
                spin(cardBlock);
            }, rotationTime * 2);

            setTimeout(function () {
                firstCardBlock.find('.main-image').remove();
                firstCardBlock.find('.cover').remove();

                cardBlock.find('.main-image').remove();
                cardBlock.find('.cover').remove();

                openedCardsCount += 2;

                checkWin();
            }, rotationTime * 3);
        } else {
            setTimeout(function () {
                spin(firstCardBlock);
                spin(cardBlock);
            }, rotationTime * 2);
        }

        indexOfOpenedCard = undefined;
    }

    function checkWin() {
        if (gameUrls.length == openedCardsCount) {
            //win
            let data = {
                CardsCount: openedCardsCount,
                Steps: steps
            };
            $.post('/Maze/CoupleWin', data)
                .done(function () {
                    location.href = '/Home/Index';
                });
        }
    }

    function spin(block) {
        let side = (block.data('id') - 0) % 2 == 0 ? 'X' : 'Y';
        rotation(block, 90, side, function () {
            block.css('transform', `rotate${side}(-90deg)`);

            block.find('.main-image').toggle();
            block.find('.cover').toggle();

            rotation(block, 0, side);
        });
    }

    function rotation(block, toAngle, side, complete) {
        block.animate({ now: toAngle }, {
            duration: rotationTime,
            step: function (now, fx) {
                block.css('transform', `rotate${side}(${now}deg)`);
            },
            complete: complete
        });
    }
});

function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min)) + min; //Максимум не включается, минимум включается
}