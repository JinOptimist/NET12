console.log(1);

$(document).ready(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/documentPreparation")
        .build();



    hubConnection.start();
});