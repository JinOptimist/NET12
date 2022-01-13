$(document).ready(function () {
    let width = 0;
    let height = 0;
    let colorCount = 0;
    let allColors = [];
    let colorsPreview = [];

    $.get('/ZumaGame/GetColors')
        .done(function (colors) {

            allColors = colors;

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
                getColorPreview();
            })
        });


    function buildField() {
        $('.zumagame-difficult-preview div').remove();

        for (let y = 0; y < height; y++) {

            let div = $('<div />');
            div.addClass('zumagame-field-row');
            div.attr('data-y', y);

            for (let x = 0; x < width; x++) {

                let span = $('<span />');
                span.addClass('zumagame-preview-cell');

                getRandomInt(0, colorCount);
                span.css('background', 'red');

                div.append(span);
            }

            $('.zumagame-difficult-preview').append(div);
        }
    }

    function getColorPreview() {

        for (let i = 0; i < allColors.length - colorCount; i++) {

            let rand = getRandomInt(0, allColors.length);

            console.log(rand);

            console.log(allColors.length);
            allColors.splice(rand, 1);

            console.log('lenght: ' + allColors.length);
        }

    }

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min; //Максимум не включается, минимум включается
    }
});


