/*window.addEventListener('contextmenu', function (e) {
    e.preventDefault();
}, false);*/

let cells = [];
let stillDown = false;
let timerImages = ["/images/miner_buttons/number0.png", "/images/miner_buttons/number1.png", "/images/miner_buttons/number2.png", "/images/miner_buttons/number3.png", "/images/miner_buttons/number4.png", "/images/miner_buttons/number5.png", "/images/miner_buttons/number6.png", "/images/miner_buttons/number7.png", "/images/miner_buttons/number8.png", "/images/miner_buttons/number9.png"];
let intervalId;

$(document).ready(function () {

    $(".pressClosed").on("click", openCell).on("contextmenu", setFlag);

    /*$(".pressOpen")
        .mousedown(function () {
            $(".face.miner-elements")
                .attr('src', '/images/miner_buttons/face_worry.png');

            stillDown = true;
        })
        .mouseup(function () {
            $(".face.miner-elements")
                .attr('src', '/images/miner_buttons/face_smile.png');

            updateImagesToCommon(cells);

            stillDown = false;
        });*/

    $(".pressOpen")
        .on("mousedown", pressNear)
        //.on("click", OpenWithFlags)
        //.on("mouseup", unpressNear)
        .on("click", unpressNear)
        .on("click", OpenWithFlags);

    $("a[href='/Miner/StartGame']").click(function () {
        sessionStorage.removeItem('isPlaying');
        sessionStorage.removeItem('secUnits');
        sessionStorage.removeItem('secTens');
        sessionStorage.removeItem('secHundreds');
    })

    if (sessionStorage.getItem('isPlaying').localeCompare("true") == 0) {
        timer();
        intervalId = setInterval(timer, 1000);
    }
    else if (sessionStorage.getItem('isPlaying').localeCompare("false") == 0) {
        $(".miner-number3.miner-elements.miner-numbers").attr('src', timerImages[parseInt(sessionStorage.getItem('secHundreds'))]);
        $(".miner-number4.miner-elements.miner-numbers").attr('src', timerImages[parseInt(sessionStorage.getItem('secTens'))]);
        $(".miner-number5.miner-elements.miner-numbers").attr('src', timerImages[parseInt(sessionStorage.getItem('secUnits'))]);
    }

});

    //после проигрыша открыть все бомбы
    //после выигрыша расставить флажки

function openCell() {
    let data = {
        idCell: $(this).attr('id')
    };
    $.get('/Miner/Step', data, function (field) {
        updateField(field);
    });

    if (sessionStorage.getItem('isPlaying') == null) {

        sessionStorage.setItem('secUnits', '0');
        sessionStorage.setItem('secTens', '0');
        sessionStorage.setItem('secHundreds', '0');
        sessionStorage.setItem('isPlaying', 'true');

        intervalId = setInterval(timer, 1000);
    }
}

function setFlag() {
    let cellId = $(this).attr('id');
    let data = {
        idCell: cellId
    };
    $.get('/Miner/SetFlag', data)
        .done(function (isFlag) {
            if (isFlag) {
                $("#" + cellId)
                    .removeClass("pressClosed")
                    .addClass("flag")
                    .off("click")
                    .attr('src', '/images/miner_buttons/miner_flag.png').attr('width', '29');
            } else {
                $("#" + cellId)
                    .addClass("pressClosed")
                    .removeClass("flag")
                    .on("click", openCell)
                    .attr('src', '/images/miner_buttons/miner_button.png').attr('width', '29');
            }

            changeFlagsCount(cellId);

        });
}

function updateField(field) {
    for (let y = 0; y < field.height; y++) {
        for (let x = 0; x < field.width; x++) {
            let cell = field.cells.find(function (c) {
                return c.x == x && c.y == y;
            });
            if (cell.isOpen) {
                $("#" + cell.id)
                    .removeClass("pressClosed")
                    .off('contextmenu')
                    .off("click", openCell);
                if (cell.firstOpenedBomb) {
                    $("#" + cell.id)
                        .attr('src', '/images/miner_buttons/red_bomb.png');
                }
                else if (cell.isBomb) {
                    $("#" + cell.id)
                        .attr('src', '/images/miner_buttons/bomb.png');
                }
                else {
                    let str = cell.nearBombsCount.toString();
                    let minerSource = "/images/miner_buttons/bomb_amount" + str + ".png";
                    if (field.isOver || field.isWon || cell.nearBombsCount == 0) {
                        $("#" + cell.id)
                            .off('click');
                    }
                    else {
                        $("#" + cell.id)
                            .addClass("pressOpen")
                            .on("mousedown", pressNear)
                            //.on("mouseup", unpressNear)
                            .on("click", unpressNear)
                            .on("click", OpenWithFlags);
                            //.on("click", OpenWithFlags);
                    }
                    $("#" + cell.id)
                        .attr('src', minerSource)
                }
            }
            else {
                if (field.isOver && cell.isFlag && !cell.isBomb) {
                    $("#" + cell.id)
                        .removeClass("pressClosed")
                        .off('contextmenu')
                        .off('click')
                        .attr('src', '/images/miner_buttons/crossed_bomb.png');
                }
                else if (field.isOver || field.isWon) {
                    $("#" + cell.id)
                        .removeClass("pressClosed")
                        .off('contextmenu')
                        .off('click');
                }
            }

            $("#" + cell.id).attr('width', '29');
        }
    }

    if (field.isOver || field.isWon) {
        sessionStorage.setItem('isPlaying', 'false');
        clearInterval(intervalId);
    }

    if (field.isOver) {
        $(".face.miner-elements")
            .attr('src', '/images/miner_buttons/face_dead.png');

        if ($(".miner-all").find(".won-and-over h1").length <= 0) {
            $(".miner-all").prepend('<div class="won-and-over h1">Game</div>');
            $(".miner-all").append('<div class="won-and-over h1">over!</div>');
        }

    }
    else if (field.isWon) {

        $(".face.miner-elements")
            .attr('src', '/images/miner_buttons/face_win.png');

        if ($(".miner-all").find(".won-and-over h2").length <= 0) {
            $(".miner-all").prepend('<div class="won-and-over h2">You</div>');
            $(".miner-all").append('<div class="won-and-over h2">won!</div>');
        }
    }
}

function pressNear() {
    let data = {
        idCell: $(this).attr('id')
    };
    $.get('/Miner/GetNearToPress', data, function (cellsFromServer) {

        cells = cellsFromServer;
        updateImagesToEmpty(cellsFromServer);
    });
}

function unpressNear() {
    let data = {
        idCell: $(this).attr('id')
    };
    $.get('/Miner/GetNearToPress', data, function (cellsFromServer) {

        cells = cellsFromServer;
        updateImagesToCommon(cellsFromServer);
    });
}

function OpenWithFlags() {
    let data = {
        idCell: $(this).attr('id')
    };
    $.get('/Miner/CheckFlagsAndNearBombsCount', data, function (answer) {
        if (answer) {
            $.get('/Miner/OpenNearWithFlags', data, function (field) {
                updateField(field);
            });
        }
    });
}

function pressNearOrOpenWithFlags() {
        let data = {
            idCell: $(this).attr('id')
        };
        $.get('/Miner/CheckFlagsAndNearBombsCount', data, function (answer) {
            if (answer) {
                $.get('/Miner/OpenNearWithFlags', data, function (field) {
                    updateField(field);
                })
            } else {
                $.get('/Miner/GetNearToPress', data, function (cellsFromServer) {

                    cells = cellsFromServer;

                    if (stillDown) {
                        updateImagesToEmpty(cellsFromServer);
                    }
                });
            }
        });
}

function changeFlagsCount(cellId) {
    let data = {
        idCell: cellId
    };
    $.get('/Miner/CheckFlagsAmount', data)
        .done(function (flagsAmount) {
            let numbers = flagsAmount.toString();
            if (numbers.length == 1) {
                numbers = "00" + numbers;
            }
            else if (numbers.length == 2) {
                if (flagsAmount == 10) {
                    numbers = "0" + numbers;
                }
                else {
                    numbers = numbers.replace('-', '-0');
                }
            }

            numbers = numbers.split('');

            for (let i = 0; i < 3; i++) {
                let newNumberClass = "miner-number" + i;
                let newNumberSrc = "/images/miner_buttons/number" + numbers[i] + ".png";
                $("." + newNumberClass).attr('src', newNumberSrc);
            }
        });
}

function timer() {
    
    let secUnits = parseInt(sessionStorage.getItem('secUnits'));
    let secTens = parseInt(sessionStorage.getItem('secTens'));
    let secHundreds = parseInt(sessionStorage.getItem('secHundreds'));

    secUnits += 1;
    if (secUnits == 10) {
        secUnits = 0;
        secTens += 1;

        if (secTens == 10) {
            secTens = 0;
            secHundreds += 1;

            if (secHundreds == 10) {
                secHundreds = 9;
                sessionStorage.setItem('startTimer', 'false');
                clearInterval(intervalId);
            }
        }
    }

    $(".miner-number3.miner-elements.miner-numbers").attr('src', timerImages[secHundreds]);
    $(".miner-number4.miner-elements.miner-numbers").attr('src', timerImages[secTens]);
    $(".miner-number5.miner-elements.miner-numbers").attr('src', timerImages[secUnits]);

    sessionStorage.setItem('secUnits', secUnits);
    sessionStorage.setItem('secTens', secTens);
    sessionStorage.setItem('secHundreds', secHundreds);

}



function updateImagesToEmpty(cells) {
    for (let i = 0; i < cells.length; i++) {
        $("#" + cells[i]).attr('src', '/images/miner_buttons/bomb_amount0.png');
    }
}

function updateImagesToCommon(cells) {
    for (let i = 0; i < cells.length; i++) {
        $("#" + cells[i]).attr('src', "/images/miner_buttons/miner_button.png");
    }
}