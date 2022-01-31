$(document).ready(function () {
    let answerArray = [];

    $('.transactionButton').click(function () {

        let transactionUserName = $('.transactionUserName').val();
        let transactionCoins = $('.transactionCoins').val();

        $.post('/User/JSTransactionCoins', { userName: transactionUserName, coins: transactionCoins})
            .done(function (answer) {

                answerArray = JSON.parse(answer);

                $('.transaction-info').text(answerArray[0]).css('color', answerArray[1]);
                $('#Coins').attr('value', answerArray[2]);
                $('.profile-coins').text(answerArray[2]);
            });
    });
});


