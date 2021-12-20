$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

    //Что делать, когда пршло новое сообщение
    hubConnection.on("NewMessage", function (message, name) {
        addMessage(name, message);
    });

    hubConnection.on("Enter", function (name) {
        addMessage(name, `enter to site`);
    });

    function addMessage(userName, text) {
        var messageBlock = $('<div>');
        messageBlock.addClass('message');

        var authorSpan = $('<span>');
        authorSpan.addClass('user-name');
        authorSpan.text(userName);
        messageBlock.append(authorSpan);

        var textSpan = $('<span>');
        textSpan.addClass('message-text');
        textSpan.text(text);
        messageBlock.append(textSpan);
        
        $('.messages').append(messageBlock);

        const topPosition = $(".message:last").position().top;
        $(".messages").animate({ scrollTop: topPosition }, 300);
    }

    //Отправить новое сообщзение на сервер
    $('.send-message').click(function () {
        let message = $('.new-message').val();
        hubConnection.invoke("NewMessage", message);
        $('.new-message').val('');
    });

    hubConnection.start();
});