$(document).ready(function () {
    let timer = 10;

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/seaBattle")
        .build();

    //Что делать, когда пршло новое сообщение
    hubConnection.on("NewMessage", function (message) {
        location.reload();
    });


    setInterval(function () {

        if (timer < 0) {
            timer = 10;
        }
        $('.seaBattleTimer').text(timer);


        if (timer == 0) {
            //Отправить новое сообщзение на сервер
            hubConnection.invoke("AIShooting");
        }


        timer--;
    }, 1000);

    hubConnection.start();
});