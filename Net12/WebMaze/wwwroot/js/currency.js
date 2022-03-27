$(document).ready(function () {

    $('.get').click(function (evt) {
        evt.preventDefault();

        window.location.replace(`/Currency/GetRateById?currencyId=${$('select[name="currency"]').val()}`);
    });
});

