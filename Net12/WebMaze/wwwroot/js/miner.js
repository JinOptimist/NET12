/*window.addEventListener('contextmenu', function (e) {
    e.preventDefault();
}, false);*/

let cells = [];
let stillDown = false;

function setFlag(cellId) {
    let data = {
        idCell: cellId
    };
    $.post('/Miner/SetFlag', data)
        .done(function () {
            location.reload();
        });
};

//******************************************
function openNearWithFlagsOrPressNear(cellId) {
    console.log('main click 01');
    let data = {
        idCell: cellId
    };
    $.get('/Miner/CheckFlagsAndNearBombsCount', data, function (answer) {
        console.log('bomb count 02');
        if (answer) {
            $.post('/Miner/OpenNearWithFlags', data)
                .done(function () {
                    location.reload();
                });
        } else {
            $.get('/Miner/GetNearToPress', data, function (cellsFromServer) {
                console.log('near data 03');
                
                cells = cellsFromServer;

                if (stillDown) {
                    updateImagesToEmpty(cellsFromServer);
                }

            }).fail(function () { alert("error1"); });
        }

    }).fail(function () { alert("error2"); });



};

$(document).ready(function () {
    $(".press")
        .mousedown(function () {
            console.log('mousedown 04');
            $(".face.miner-elements")
                .attr('src', '/images/miner_buttons/face1.png');

            stillDown = true;
        })
        .mouseup(function () {
            console.log('mouseup 04');
            $(".face.miner-elements")
                .attr('src', '../../../images/miner_buttons/face0.png');
            for (let i = 0; i < cells.length; i++) {
                $("." + cells[i]).attr('src', "../../../images/miner_buttons/miner_button.png");
            }

            stillDown = false;
            //location.reload();
        });
});

function updateImagesToEmpty(cells) {
    for (let i = 0; i < cells.length; i++) {
        $("." + cells[i]).attr('src', '../../../images/miner_buttons/t0.png');
    }
}

function updateImagesToCommon(cells) {
    for (let i = 0; i < cells.length; i++) {
        $("." + cells[i]).attr('src', "../../../images/miner_buttons/miner_button.png");
    }
}

//******************************************


