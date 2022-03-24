$(document).ready(function () {

    document.addEventListener('keydown', function (event) {
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
    });

    $('.step').click(function () {
        var direction = $(this).attr('data-direction');
        SendDirection(direction);
    });


    function SendDirection(MyTurn) {
        event.preventDefault();

        const paramsUrl = new URLSearchParams(window.location.search);
        const mazeId = paramsUrl.get("id");

        let url = `/Maze/GetMazeData?mazeId=${mazeId}&stepDirection=${MyTurn}`;
        $.get(url)
            .done(function (mazeData) {
                for (var i = 0; i < mazeData.cells.length; i++) {
                    let cell = mazeData.cells[i];

                    $(`[data-x=${cell.x}][data-y=${cell.y}] img`)
                        .attr('src', `/images/cells/${cell.cellType}.jpg`);
                }
                $('.myHealth').text(`My Health: ${mazeData.heroNowHp}/${mazeData.heroMaxHp}`);
                $('.myFatigue').text(`My Fatigue: ${mazeData.heroNowFatigure}/${mazeData.heroMaxFatigure}`);
            });
    }
});
