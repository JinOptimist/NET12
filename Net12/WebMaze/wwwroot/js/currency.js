$(document).ready(function () {

    $('.type').change(function () {

        switch ($('select[name="typeOfRequest"]').val()) {
            case "1":
                $('.GetById').show();
                $('.GetOnDate').hide();
                $('.GetOnPeriod').hide();
                break;
            case "2":
                $('.GetOnDate').show();
                $('.GetById').hide();
                $('.GetOnPeriod').hide();
                break;
            case "3":
                $('.GetOnPeriod').show()
                $('.GetById').hide();
                $('.GetOnDate').hide();;
                break;
            default:
                break;
        }
    });

    $('.getById').click(function (evt) {
        evt.preventDefault();

        window.location.replace(`/Currency/GetRateById?currencyId=${$('select[name="currency"]').val()}`);
    });

    $('.getOnDate').click(function (evt) {
        evt.preventDefault();

        window.location.replace(`/Currency/GetRateByIdOnDate?currencyId=${$('select[name="currency"]').val()}&date=${$('input[name="onDate"]').val()}`);
    });

    $('.getOnPeriod').click(function (evt) {
        evt.preventDefault();

        window.location.replace(`/Currency/GetRateByIdOnPeriod?currencyId=${$('select[name="currency"]').val()}&onStartDate=${$('input[name="startDate"]').val()}&onEndDate=${$('input[name="endDate"]').val()}`);
    });
});

