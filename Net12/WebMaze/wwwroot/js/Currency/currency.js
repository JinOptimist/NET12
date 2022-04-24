$(document).ready(function () {

    $('.type').change(function () {
        $('.GetById').hide();
        $('.GetOnDate').hide();
        $('.GetOnPeriod').hide();
        switch ($('select[name="typeOfRequest"]').val()) {
            case "1":
                $('.GetById').show();
                break;
            case "2":
                $('.GetOnDate').show();
                break;
            case "3":
                $('.GetOnPeriod').show()
                break;
            default:
                break;
        }
    });

    $('.getById').click(function (evt) {
        evt.preventDefault();

        const url = `/Currency/GetRateById?currencyId=${$('select[name="currency"]').val()}`;

        window.location.replace(url);
    });

    $('.getOnDate').click(function (evt) {
        evt.preventDefault();

        const url = `/Currency/GetRateByIdOnDate?currencyId=${$('select[name="currencyOnDate"]').val()}&date=${$('input[name="onDate"]').val()}`;

        window.location.replace(url);
    });

    let index = 0;
    $('.add').click(function () {
        let field = $('.getOnPeriod-field.base').clone().css("display", "block").removeClass("base").addClass(`fieldId${index}`);
        index++;

        $('.fields').append(field);
        $('.dates').show();
        $('.getOnPeriod-button').show();
    });

    $('.remove').click(function () {
        index--;
        $(`.getOnPeriod-field.fieldId${index}`).remove();
        /*index--;*/

        if (index == 0) {
            $('.getOnPeriod-button').hide();
            $('.dates').hide();
        }
    });

    $('.getOnPeriod-button').click(function (evt) {
        evt.preventDefault();

        let url = "/Currency/GetRateByIdOnPeriod?"
        for (var i = 0; i < index; i++) {
            let cur_id = `currencyId[${i}]=${$(`.fieldId${i} select[name="currency"]`).val()}&`;
            url = url + cur_id;
        }
        url = url + `onStartDate=${$('input[name="startDate"]').val()}&onEndDate=${$('input[name="endDate"]').val()}`;

        window.location.replace(url);
    });
});

