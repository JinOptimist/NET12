$(document).ready(function () {

    var places = [];
    places.push(new Array(4));
    places.push(new Array(4));
    places.push(new Array(4));
    places.push(new Array(4));
    //[row][column]
    init();
    //числа получают места
    function init () {
        for (var i = 1; i < 16; i++) {
            let row, column;
            do {
                let randomPlace = getRandomInt(0, 16);
                row = Math.floor(randomPlace / 4);
                column = randomPlace % 4;
            } while (places[row][column] !== undefined);            

            places[row][column] = i;                
        }

        for (var i = 0; i < 4; i++) {        
            for (var j = 0; j < 4; j++) {
                
                var element = document.querySelector('[id-row="' + i + '"]', '[id-column="' + j + '"]');
                console.log(element);
                var my_html = '<div class="number"><span class="number-style">' + places[i][j] + '</span></div>';
                element.innerHTML = my_html;
                
            }
        }

    }
    
    

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min;
    }
});

