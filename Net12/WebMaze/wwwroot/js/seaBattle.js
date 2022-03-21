$(document).ready(function () {
    let gameId = $('.seabattle-game').attr('gameId');
    let isLoseGame = false;

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/seaBattle")
        .build();

    hubConnection.on(gameId + "LoseGame", function () {

        isLoseGame = true;
        window.location.href = "/SeaBattle/LoseGame?gameId=" + gameId;
    });

    if (!isLoseGame) {
        //Что делать, когда пршло новое сообщение
        hubConnection.on(gameId, function (seconds) {

            if (seconds == 0) {

                $.get('/SeaBattle/UserIsActive', { gameId: gameId })
                    .done(function () {

                        $.get('/SeaBattle/EnemyTurn', { gameId: gameId })
                            .done(function () {

                                //window.location.href = "/SeaBattle/Game?gameId=" + gameId;
                                location.reload();

                            });
                    });
            }

            $('.seaBattleTimer').text(seconds);
        });
    }
    hubConnection.start();
});