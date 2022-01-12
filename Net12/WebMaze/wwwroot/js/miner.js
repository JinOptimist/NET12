"use strict";
document.body.oncontextmenu = function (e) {
    return false;
};

function setFlag(cellId) {
    let data = {
        idCell: cellId
    };
    $.post('/Miner/SetFlag', data)
        .done(function () {
            location.reload();
        });
};

function openNearWithFlags(cellId) {
    let data = {
        idCell: cellId
    };
    $.post('/Miner/OpenNearWithFlags', data)
        .done(function () {
            location.reload();
            location.reload();
        });
};

timer();

function timer() {
    $.get('/Miner/SetUpTimerForStartedGame')
        .done(function (fieldId) {
            if (!localStorage.getItem(fieldId)) {

            }
        })
    };



let i = 0;
let k = 0;
let h = 0;

let timer = 0;


let cl1 = new Image(); cl1.src = '../../../images/miner_buttons/d1.png';
let cl2 = new Image(); cl2.src = '../../../images/miner_buttons/d2.png';
let cl3 = new Image(); cl3.src = '../../../images/miner_buttons/d3.png';
let cl4 = new Image(); cl4.src = '../../../images/miner_buttons/d4.png';
let cl5 = new Image(); cl5.src = '../../../images/miner_buttons/d5.png';
let cl6 = new Image(); cl6.src = '../../../images/miner_buttons/d6.png';
let cl7 = new Image(); cl7.src = '../../../images/miner_buttons/d7.png';
let cl8 = new Image(); cl8.src = '../../../images/miner_buttons/d8.png';
let cl9 = new Image(); cl9.src = '../../../images/miner_buttons/d9.png';
let cl0 = new Image(); cl0.src = '../../../images/miner_buttons/d0.png';
let clock_numbers = new Array(cl0, cl1, cl2, cl3, cl4, cl5, cl6, cl7, cl8, cl9);






function time1()
{
    if (i >= 10) {
        i = 0;
    }
    if (h = 10) {
        return false;
    }
    document.getElementById('imag1').src = clock_numbers[i].src;
    i += 1;
    setTimeout(time1, 1000);
}
function time2() {
    if (k >= 10) {
        k = 0;
    }
    if (h = 10) {
        return false;
    }
    document.getElementById('imag2').src = clock_numbers[k].src;
    k += 1;
    setTimeout(time2, 10000);
}
function time3() {
    if (h = 10) {
        return false;
    }
    document.getElementById('imag3').src = clock_numbers[h].src;
    h += 1;
    setTimeout(time3, 100000);
}
