$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/documentPreparation")
        .build();

    hubConnection.on("Notification", function (percent, pages) {
        updateStatus(percent, pages);
    });

    function updateStatus(percent, pages) {
        $('.status-percent-text').text(`Ready ${percent} of ${pages}`);
        $('.status-percent-line-bg').width((percent * 150 / pages));

        if (percent == pages) {
            downloadDocument();
        }
    };

    $('.cancel-button').click(function (evt) {
        evt.preventDefault();

        $('.status-percent-text').text("Canceled");
        $('.status-percent-line-bg').css("background-color", "red");

        $('.cancel-button').css("display", "none");
        $('.back-button').css("display", "block");

        stopPreparation();

    });

    function downloadDocument() {
        $('.status-percent-text').text('Finished');
        $('.cancel-button').css("display", "none");
        $('.download-button').css("display", "block");        
    };

    function stopPreparation() {
        $.ajax({
            url: '/GetDocument/StopPreparation',
            data: {
                documentId: $('.documnetId').val()
            }
        });
    };

    hubConnection.start();
});