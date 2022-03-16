$(document).ready(function () {
    let gameId = $('.seabattle-game').attr('gameId');

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/seaBattle")
        .build();

    //Что делать, когда пршло новое сообщение
    hubConnection.on(gameId, function (seconds) {


        if (seconds == 0) {
            $.post('/SeaBattle/EnemyTurn', { gameId: gameId });

            $.get('/SeaBattle/UserIsActive', { id: gameId });

            setTimeout(function () {

                //window.location.href = "/SeaBattle/Game?id=" + gameId;
                location.reload();

            }, 200)
        }
        $('.seaBattleTimer').text(seconds);
    });


    hubConnection.start();
});