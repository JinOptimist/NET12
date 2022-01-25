$(document).ready(function () {
    let width = 0;
    let height = 0;
    let colorCount = 0;
    let colorsArray = [];

    $.get('/ZumaGame/GetColors')
        .done(function (colors) {

            colorsArray = JSON.parse(colors);

            checkTextBox();
        });

    function checkTextBox() {

        $('.diffWidth').change(function () {

            width = $(this).val();

            if (width < 5) {
                $(this).val(5);
                width = 5;
            }
            if (width > 20) {
                $(this).val(20);
                width = 20;
            }

            buildField();
        });

        $('.diffHeight').change(function () {

            height = $(this).val();

            if (height < 5) {
                $(this).val(5);
                height = 5;
            }
            if (height > 20) {
                $(this).val(20);
                height = 20;
            }

            buildField();
        });

        $('.diffColorCount').change(function () {

            colorCount = $(this).val();

            if (colorCount < 2) {
                $(this).val(2);
                colorCount = 2;
            }
            if (colorCount > colorsArray.length) {
                $(this).val(colorsArray.length);
                colorCount = colorsArray.length;
            }

            buildField();
        });
    }

    function buildField() {

        mixColors();

        $('.zumagame-difficult-preview div').remove();

        for (let y = 0; y < height; y++) {

            let div = $('<div />');
            div.addClass('zumagame-field-row');

            for (let x = 0; x < width; x++) {

                let span = $('<span />');
                span.addClass('zumagame-preview-cell');

                if (colorCount > 1 && colorCount <= colorsArray.length) {
                    span.css('background', colorsArray[getRandomInt(0, colorCount)]);
                }

                div.append(span);
            }

            $('.zumagame-difficult-preview').append(div);
        }
    }

    function mixColors() {

        for (var i = 0; i < 50; i++) {
            const firstColor = getRandomInt(0, colorsArray.length);
            const secondColor = getRandomInt(0, colorsArray.length);

            const temp = colorsArray[firstColor];
            colorsArray[firstColor] = colorsArray[secondColor];
            colorsArray[secondColor] = temp;
        }
    }

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min; //Максимум не включается, минимум включается
    }
});


