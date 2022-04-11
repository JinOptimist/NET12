$(document).ready(function () {

    const paramsUrl = new URLSearchParams(window.location.search);
    const currencyId = paramsUrl.get("currencyId");
    const onStartDate = paramsUrl.get("onStartDate");
    const onEndDate = paramsUrl.get("onEndDate");       

    let url = `/Currency/GetRateByIdOnPeriodJson?currencyId=${currencyId}&onStartDate=${onStartDate}&onEndDate=${onEndDate}`;
    $.get(url)
        .done(function (rateList) {
            let dates = [];
            let rates = [];

            for (var i = 0; i < rateList.length; i++) {
                let rate = rateList[i].cur_OfficialRate;
                let date = rateList[i].date;

                rates.push(rate);
                dates.push(date);
            }

            const data = {
                labels: dates,
                datasets: [{
                    label: 'Rate',
                    data: rates,//fill from service
                    fill: false,
                    borderColor: 'rgb(75, 192, 192)',
                    tension: 0.1
                }]
            };

            const ctx = document.getElementById('myChart').getContext('2d');
            const myChart = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                }
            });
        }); 
});