$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/documentUpdateStatusAll")
        .build();

    hubConnection.on("UpdateStatus", function (id, percent, pages) {        
        $(`.doc-info-${id}`).text(`Document ID: ${id} Ready ${percent} of ${pages}`);
    });

    hubConnection.on("ReadyDocument", function (id, percent, pages) {
        $(`.doc-info-${id}`).text(`Document ID: ${id} is ready`);        
    });

    hubConnection.on("CancelPreparation", function (id) {
        $(`.doc-info-${id}`).text(`Document ID: ${id} is canceled`);
    });

    hubConnection.on("NewDocument", function (id) {
        $('.doc').append($('<div/>').addClass(`doc-info-${id}`).text(`Document ID: ${id}`));
    });

    hubConnection.start();
});