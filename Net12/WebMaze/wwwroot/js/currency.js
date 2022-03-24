$(document).ready(function (evt) {
    console.log("1");

    $('.get').click(function (evt) {
        evt.preventDefault();

        window.location.replace(`/Currency/GetRateById?currencyId=${$('select[name="currency"]').val()}`);
    });
});

