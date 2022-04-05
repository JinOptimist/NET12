$(document).ready(function () {
    console.log("On period")

    let index = 0;
    $('.add').click(function () {
        console.log("222");
        let field = $('.getOnPeriod-field.base').clone().css("display","block").removeClass("base").addClass(`fieldId${index}`);
        index++;
        $('.fields').append(field);
        $('.getOnPeriod').show();
    });

    $('.remove').click(function () {
        console.log("333");
        let field = $(`.getOnPeriod-field.fieldId${index - 1}`).remove();
        index--;

        if (index == 0) {
            $('.getOnPeriod').hide();
        }
    });
});

