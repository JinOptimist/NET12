$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/documentPreparation")
        .build();

    hubConnection.on("Notification", function (id, percent, pages) {
        if (id == $('.documnetId').val()) {
            updateStatus(id, percent, pages);
        }
    });

    function updateStatus(id, percent, pages) {
        $('.status-document-name').text(`Document Id:${id}`)
        $('.status-percent-text').text(`Ready ${percent} of ${pages}`);
        $('.status-percent-line-bg').width((percent * 150 / pages));


        if (percent == pages) {
            downloadDocument();
        }
    };

    function downloadDocument() {
        $('.status-percent-text').text('Finished');
        $('.cancel-button').css("display", "none");
        $('.download-button').css("display", "block");
    };



    hubConnection.on("stopNotification", function (id) {
        if (id == $('.documnetId').val()) {
            stopUpdateStatus(id);
        }
    });

    function stopUpdateStatus(id) {
        $('.status-percent-text').text("Canceled");
        $('.status-percent-line-bg').css("background-color", "red");

        $('.cancel-button').css("display", "none");
        $('.back-button').css("display", "block");
    };

    $('.cancel-button').click(function (evt) {
        evt.preventDefault();

        stopPreparation();
    });

    function stopPreparation() {
        $.ajax({
            url: '/GetDocument/StopPreparation',
            data: {
                documentId: $('.documnetId').val()
            }
        });
    };



    hubConnection.on("NewDocument", function (id) {
        $('.doc').append($('<div/>').addClass(`doc-info-${id}`).text(`Document ID: ${id}`));
    });

    hubConnection.start();
});