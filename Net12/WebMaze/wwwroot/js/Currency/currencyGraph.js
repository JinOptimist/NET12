$(document).ready(function () {

    const paramsUrl = new URLSearchParams(window.location.search);
    let index = 0;
    let currencyIds = [];
    while (true) {
        let currencyId = paramsUrl.get(`currencyId[${index}]`);
        if (currencyId === null) {
            break;
        }
        currencyIds.push(currencyId);
        index++;
    }
    const onStartDate = paramsUrl.get("onStartDate");
    const onEndDate = paramsUrl.get("onEndDate");

    let url = "/Currency/GetRateByIdOnPeriodJson?"
    for (var i = 0; i < currencyIds.length; i++) {
        let cur_idUrl = `currencyId[${i}]=${currencyIds[i]}&`;
        url = url + cur_idUrl;
    }
    url = url + `onStartDate=${onStartDate}&onEndDate=${onEndDate}`;

    $.get(url)
        .done(function (arrayOfratesList) {
            let ratesListArray = [];
            let dates = [];

            for (var i = 0; i < arrayOfratesList.length; i++) {
                let ratesList = [];
                ratesListArray.push(ratesList);
                for (var j = 0; j < arrayOfratesList[i].length; j++) {
                    let rate = arrayOfratesList[i][j].cur_OfficialRate;
                    ratesListArray[i].push(rate);
                }
            }
            let index = 0;
            for (var j = 0; j < arrayOfratesList[index].length; j++) {
                let date = arrayOfratesList[index][j].date;
                dates.push(date);
            }            

            let datasetsArray = [];
            for (var i = 0; i < ratesListArray.length; i++) {
                let color = random_rgb();
                let dataset = new Dataset(`Rate${i}`, ratesListArray[i], color);
                datasetsArray.push(dataset);
            }

            const data = {
                labels: dates,
                datasets: datasetsArray,
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

    class Dataset {
        constructor(label, data, borderColor) {
            this.label = label;
            this.data = data;
            this.borderColor = borderColor;
        };
        fill = false;
        tension = 0.1;
    }

    function random_rgb() {
        var o = Math.round, r = Math.random, s = 255;
        return 'rgb(' + o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s) + ')';
    }
});