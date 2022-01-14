$(document).ready(function () {

    let places = new int[4][4];
    //[row][column]
    init();
    //числа получают места
    function init () {
        for (var i = 1; i < 16; i++) {
            let row, column;
            do {
                let randomPlace = getRandomInt(16);
                row = Math.floor(randomPlace / 4);
                column = randomPlace % 4;
            } while (places[row][column] !== undefined);

            places[row][column] = i;
        }

        for (var i = 0; i < 4; i++) {
            for (var j = 0; i < 4; j++) {

            }
        }

    }

    

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min;
    }
});

