$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/pdfPreparationInTheIndex")
        .build();

    hubConnection.on("Progres", function (id, percent, pdfName) {
        $(`.doc-info-${id}`).text(`PDF: ${pdfName} Ready ${percent} of 100%`);
    });

    hubConnection.on("ReadyPDF", function (id, pdfName) {
        $(`.doc-info-${id}`).text(`PDF: ${pdfName} is ready`);
    });

    hubConnection.on("StopProgres", function (id, pdfName) {
        $(`.doc-info-${id}`).text(`PDF: ${pdfName} is canceled`);
    });    

    hubConnection.start();
});