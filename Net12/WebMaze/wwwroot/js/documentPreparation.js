$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/documentPreparation")
        .build();

    hubConnection.on("Notification", function (percent, pages) {
        updateStatus(percent, pages);
    });

    function updateStatus(data, pages) {
        $('.status-percent-text').text(`Ready ${data} of 100`);

        $('.status-percent-line-bg').width((data * 150 / pages) )
    };

    hubConnection.start();
});