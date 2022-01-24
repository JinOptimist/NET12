$(document).ready(function () {
    let width = 0;
    let height = 0;
    let colorCount = 0;
    let colorsArray = [];

    $.get('/ZumaGame/GetColors')
        .done(function (colors) {

            colorsArray = JSON.parse(colors);

            $('.diffWidth').change(function () {

                width = $(this).val();
                buildField();
            })

            $('.diffHeight').change(function () {

                height = $(this).val();
                buildField();
            })

            $('.diffColorCount').change(function () {

                colorCount = $(this).val();
                buildField();
            })
        });

    function buildField() {

        mixColors();

        $('.zumagame-difficult-preview div').remove();

        if (height <= 20 && height >= 5 && width <= 20 && width >= 5) {

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


