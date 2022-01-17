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
        $('.zumagame-difficult-preview div').remove();

        for (let y = 0; y < height; y++) {

            let div = $('<div />');
            div.addClass('zumagame-field-row');

            for (let x = 0; x < width; x++) {

                let span = $('<span />');
                span.addClass('zumagame-preview-cell');

                if (colorCount > 0) {
                    span.css('background', colorsArray[getRandomInt(0, colorCount)]);
                }

                div.append(span);
            }

            $('.zumagame-difficult-preview').append(div);
        }
    }

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min; //Максимум не включается, минимум включается
    }
});


