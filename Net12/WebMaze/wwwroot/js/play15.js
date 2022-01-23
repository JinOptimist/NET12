$(document).ready(function () {

    var places = [];
    places.push(new Array(4));
    places.push(new Array(4));
    places.push(new Array(4));
    places.push(new Array(4));
    //[row][column]
    //place 0..15
    //number 1..16

    init();
    checkWin();
    

    function init () {
        for (var number = 1; number < 16; number++) {
            let row, column;
            do {
                let randomPlace = getRandomInt(0, 16);
                cell = getRowAndColumnByPlace(randomPlace);
            } while (places[cell.row][cell.column] !== undefined);

            places[cell.row][cell.column] = number;
        }

        for (let row = 0; row < 4; row++) {        
            for (let column = 0; column < 4; column++) {
                if (places[row][column]) {
                    var element = $('[id-row="' + row + '"][id-column="' + column + '"]');
                    var piece = $('<div class="number"><span class="number-style">' + places[row][column] + '</span></div>');
                    piece.click(function () {
                        step(row, column);
                        });
                    element.html(piece);
                } 
            }
        }

    }

    function step(row, column) {
        /*alert('row ' + row + ', column ' + column);*/
      /*  $('[id-row="' + row + '"][id-column="' + column + '"] > div')*/

        if (places[row - 1][column] === undefined) {
            let temp = places[row][column];
            places[row][column] = undefined;
            places[row - 1][column] = temp;
            
            $('[id-row="' + row + '"][id-column="' + column + '"] > div').appendTo('[id-row="' + (row - 1) + '"][id-column="' + column + '"]');
            console.log('up');            
        }

            /*|| places[cell.row + 1][cell.column] == undefined
            || places[cell.row][cell.column - 1] == undefined || places[cell.row][cell.column + 1] == undefined)*/
    }

    function checkWin() {
        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 4; j++) {
                number = places[i][j];                
                place = number - 1;
                cell = getRowAndColumnByPlace(place);                

                if (places[i][j] != places[cell.row][cell.column]) {
                    console.log(false);
                    return false;
                }

            }
        }
        console.log('win');
        return true;
    }

    function getRowAndColumnByPlace(place) {
        row = Math.floor(place / 4);
        column = place % 4;
        return {
            row: row,
            column: column
        };
    }

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min;
    }
});

