$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/documentPreparation")
        .build();

    hubConnection.on("Notification", function (percent, pages) {
        updateStatus(percent, pages);
    });
    function updateStatus(data, pages) {
        $('.status-percent-text').text(`Ready ${data} of ${pages}`);

        $('.status-percent-line-bg').width((data * 150 / pages));
    };

    $('.stop-button').click(function (evt) {
        evt.preventDefault();

        $.ajax({
            url: '/GetDocument/StopPreparation',
            data: {
                documentId: 1
            }
        });

        $('.status-percent-text').text("Stopped");
        $('.status-percent-line-bg').css("background-color", "red");

        $('.stop-button').css("display", "none");
    });    

    hubConnection.start();
});