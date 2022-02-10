$(document).ready(function () {

    document.addEventListener('keydown', function (event) {
        let turn;
        switch (event.code) {
            case "ArrowDown":
            case "KeyS":
                SendDirection(2)
                break;
            case "ArrowUp":
            case "KeyW":
                SendDirection(1);
                break;
            case "ArrowLeft":
            case "KeyA":
                SendDirection(3);
                break;
            case "ArrowRight":
            case "KeyD":
                SendDirection(4);
                break;
            default: break;
        }
        function SendDirection(MyTurn) {
            event.preventDefault();

            const ParamsUrl = new URLSearchParams(window.location.search);
            const IdParam = ParamsUrl.get("id");
            console.log(IdParam + " | " + turn);
            let data = {
                id: IdParam,
                turn: MyTurn,
            }
            $.post("/Maze/Maze", data).done(function () {
                location.href = location.href;
            });

        }




    });
});
