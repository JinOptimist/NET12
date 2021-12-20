$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

    //Что делать, когда пршло новое сообщение
    hubConnection.on("NewMessage", function (message, name) {
        addMessage(`${name}: ${message}`);
    });

    hubConnection.on("Enter", function (name) {
        addMessage(`User '${name}' enter to site`);
    });

    hubConnection.on("ZumaGameWin", function (name) {
        addMessage(`User '${name}' WIN in the ZumaGame`);
    });

    function addMessage(text) {
        var messageBlock = $('<div>');
        messageBlock.text(text);
        messageBlock.addClass('message');
        $('.messages').append(messageBlock);
    }

    //Отправить новое сообщзение на сервер
    $('.send-message').click(function () {
        let message = $('.new-message').val();
        hubConnection.invoke("NewMessage", message);
    });

    hubConnection.start();
});