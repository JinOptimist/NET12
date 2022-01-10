let go = 0;
let bright = 255;
$(document).ready(function () {

    let Mybody = $("body");
    let ico = $("div.ico-info");

    changeColor();



    function changeColor() {
        if (go == 0) {
            bright = bright + 1;
        }
        else {
            bright = bright - 1;
        }
        if (bright > 254) {
            go = 1;
        }
        if (bright < 0) {
            go = 0;
        }
        Mybody.css("background-color", "rgb(255," + bright + "," + bright + ")");
        setTimeout(changeColor, 10)
    }
});