$(document).ready(function (evt) {
    console.log("1");

    $('input').click(function (evt) {
        evt.preventDefault();

        window.location.replace(`/Currency/GetRateById?currencyId=${$('select').val()}`);
    });
});

