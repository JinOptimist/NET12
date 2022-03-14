$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/pdfPreparation")
        .build();

    hubConnection.on("Progres", function (percent, pdfId) {
        updateStatus(percent, pdfId);

        if (id == $('.pdfId').val()) {
            updateStatus(id, percent);
        }
    });

    function updateStatus(percent, pdfId) {
        $('.status-percent-text').text(`Ready ${percent} of 100%`);
        $('.status-percent-line-bg').width((percent * 150 / 100));

        hubConnection.on("downloadPDF", function (pdfId, percent, pdfName) {
            $(`.doc-info-${pdfId}`).text(`Document Name: ${pdfName} is ready`);

            if (id == $('.pdfId').val()) {
                if (percent == 100) {
                    downloadPDF();
                }
            };

            hubConnection.on("stopProgres", function (pdfId, pdfName) {
                $(`.doc-info-${pdfId}`).text(`Document Nane: ${pdfName} is canceld`);

                if (id == $('.documnetId').val()) {
                    stopUpdateStatus(id);
                }
            });

            function stopUpdateStatus(id) {
                $('.status-percent-text').text("Canceled");
                $('.status-percent-line-bg').css("background-color", "red");
                $('.cancel-button').css("display", "none");
                $('.back-button').css("display", "block");
            }

            $('.cancel-button').click(function (evt) {
                evt.preventDefault();
                stopPreparation();
        });

        function downloadPDF() {
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