$(document).ready(function () {
    let gameId = $('.seabattle-game').attr('gameId');

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/seaBattle")
        .build();

    //Что делать, когда пршло новое сообщение
    hubConnection.on(gameId, function (seconds) {


        if (seconds == 0) {
            $.get('/SeaBattle/UserIsActive', { id: gameId });
            window.location.href = "/SeaBattle/Game?id=" + gameId;
            //location.reload();
        }
        $('.seaBattleTimer').text(seconds);
    });


    hubConnection.start();
});