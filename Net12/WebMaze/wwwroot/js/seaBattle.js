$(document).ready(function () {
    let gameId = $('.seabattle-game').attr('gameId');

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/seaBattle")
        .build();

    //Что делать, когда пршло новое сообщение
    hubConnection.on(gameId, function (seconds) {

        if (seconds == 0) {

            $.get('/SeaBattle/UserIsActive', { gameId: gameId })
                .done(function () {

                    $.get('/SeaBattle/EnemyTurn', { gameId: gameId })
                        .done(function (isLoseGame) {

                            console.log(isLoseGame);

                            if (isLoseGame) {
                                //window.location.href = "/SeaBattle/Game?gameId=" + gameId;
                                location.reload();

                            }
                            else {

                                window.location.href = "/SeaBattle/LoseGame?gameId=" + gameId;
                            }

                        });
                });

        }

        $('.seaBattleTimer').text(seconds);
    });
    hubConnection.start();
});