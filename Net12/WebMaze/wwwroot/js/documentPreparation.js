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

        if (data == pages) {
            downloadDocument();
        }
    };

    $('.cancel-button').click(function (evt) {
        evt.preventDefault();

        $('.status-percent-text').text("Canceled");
        $('.status-percent-line-bg').css("background-color", "red");

        $('.cancel-button').css("display", "none");
        $('.back-button').css("display", "block");

        $.ajax({
            url: '/GetDocument/StopPreparation',
            data: {
                documentId: 1
            }
        });

    });

    function downloadDocument() {
        $('.status-percent-text').text('Finished');
        $('.cancel-button').css("display", "none");
        $('.download-button').css("display", "block");

        $('.cancel-button').click(function (evt) {
            evt.preventDefault();

            $.ajax({
                url: '/GetDocument/StopPreparation',
                data: {
                    documentId: 1
                }
            });

        });


    };

    hubConnection.start();
});