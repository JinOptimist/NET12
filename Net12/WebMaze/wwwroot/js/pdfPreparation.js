$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/pdfPreparation")
        .build();

    hubConnection.on("Progres", function (percent) {
        updateStatus(percent);
    });

    function updateStatus(percent) {
        $('.status-percent-text').text(`Ready ${percent} of 100%`);
        $('.status-percent-line-bg').width((percent * 150 / 100));

        if (percent == 100) {
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
            url: '/GetPDF/StopPreparationPDF',
            data: {
                pdfId: $('.pdfId').val()
            }
        });
    };

    hubConnection.start();
});