$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/pdfPreparation")
        .build();

    hubConnection.on("Progres", function (pdfId, pdfName, percent) {
        if (pdfId == $('.pdfId').val()) {
            updateStatus(pdfId, pdfName, percent);
        }
    });

    function updateStatus(pdfId, pdfName, percent) {
        $('.status-document-name').text(`PDF:${pdfName} with ID:${pdfId}`)
        $('.status-percent-text').text(`Ready ${percent} of 100%`);
        $('.status-percent-line-bg').width((percent * 150 / 100));
    };


    hubConnection.on("ReadyPDF", function (pdfId) {
        if (pdfId == $('.pdfId').val()) {
            location.reload();
        }
    });   


    hubConnection.on("StopProgres", function (pdfId) {
        if (pdfId == $('.pdfId').val()) {
            cancelUpdateStatus(pdfId);
        }
    });

    function cancelUpdateStatus(pdfId) {
        $('.status-percent-text').text("Canceled");
        $('.status-percent-line-bg').css("background-color", "red");
        $('.cancel-button').css("display", "none");
        $('.download').css("display", "none");
    };

    $('.cancel-button').click(function (evt) {
        evt.preventDefault();

        cancelPreparation();
    });

    function cancelPreparation() {
        $.ajax({
            url: '/GetPDF/StopPreparationPDF',
            data: {
                pdfId: $('.pdfId').val()
            }
        });
    };   

    hubConnection.start();
});