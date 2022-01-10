$(document).ready(function () {
    let indexOfOpenedCard = undefined;
    let gameUrls = [];

    let steps = 0;
    let openedCardsCount = 0;

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
                }
            });
    }

    function onCardBlockClick() {
        steps++;
        const cardBlock = $(this);
        if (indexOfOpenedCard == undefined) {
            cardBlock.find('.main-image').toggle();
            cardBlock.find('.cover').toggle();
            indexOfOpenedCard = cardBlock.data('id');
            return;
        }

        let clickedId = cardBlock.data('id');
        if (indexOfOpenedCard == clickedId) {
            cardBlock.find('.main-image').toggle();
            cardBlock.find('.cover').toggle();
            indexOfOpenedCard = undefined;
            return;
        }

        const alreadyOpenedUrl = gameUrls[indexOfOpenedCard];
        const clicekdUrl = cardBlock.find('.main-image').attr('src');

        const firstCardBlock = $(`[data-id=${indexOfOpenedCard}]`);

        cardBlock.find('.main-image').toggle();
        cardBlock.find('.cover').toggle();

        if (alreadyOpenedUrl == clicekdUrl) {
            setTimeout(function () {
                firstCardBlock.find('.main-image').remove();
                firstCardBlock.find('.cover').remove();

                cardBlock.find('.main-image').remove();
                cardBlock.find('.cover').remove();

                openedCardsCount += 2;

                checkWin();
            }, 1000);
        } else {
            setTimeout(function () {
                firstCardBlock.find('.main-image').toggle();
                firstCardBlock.find('.cover').toggle();

                cardBlock.find('.main-image').toggle();
                cardBlock.find('.cover').toggle();
            }, 1000);
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
});

function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min)) + min; //Максимум не включается, минимум включается
}