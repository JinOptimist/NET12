$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/pdfPreparationInTheIndex")
        .build();

    hubConnection.on("Progres", function (pdfId, pdfName, progress) {
        $(`.doc-info-${pdfId}`).text(`PDF:${pdfName} with ID:${pdfId} Ready ${progress} of 100%`);
    });

    hubConnection.on("ReadyPDF", function (pdfId, pdfName) {
        $(`.doc-info-${pdfId}`).text(`PDF:${pdfName} with ID:${pdfId} is ready`);
    });

    hubConnection.on("StopProgres", function (pdfId, pdfName) {
        $(`.doc-info-${pdfId}`).text(`PDF:${pdfName} with ID:${pdfId} is canceled`);
    });    

    hubConnection.start();
});