$(document).ready(function () {
    let gameId = $('.seabattle-game').attr('gameId');

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/seaBattle")
        .build();

    //Что делать, когда пршло новое сообщение
    hubConnection.on(gameId, function (seconds) {

        if (seconds == 0) {
            location.reload();
        }
        $('.seaBattleTimer').text(seconds);
    });

    setInterval(function () {

        $.set('/SeaBattle/UserIsActive');

    }, 20 * 1000);

    hubConnection.start();
});