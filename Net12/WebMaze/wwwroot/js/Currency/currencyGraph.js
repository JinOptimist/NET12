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
        let cur_id = `currencyId[${i}]=${currencyIds[i]}&`;
        url = url + cur_id;
    }
    url = url + `onStartDate=${onStartDate}&onEndDate=${onEndDate}`;

    $.get(url)
        .done(function (arrayOfrateList) {
            let dates = [];
            let rates0 = [];
            let rates1 = []

            for (var i = 0; i < arrayOfrateList.length; i++) {
                for (var j = 0; j < arrayOfrateList[i].length; j++) {
                    if (i == 0) {
                        let rate0 = arrayOfrateList[i][j].cur_OfficialRate;
                        rates0.push(rate0);
                        let date = arrayOfrateList[i][j].date;
                        dates.push(date);
                    }
                    if (i == 1) {
                        let rate1 = arrayOfrateList[i][j].cur_OfficialRate;
                        rates1.push(rate1);
                    }                    
                }
            }

            const data = {
                labels: dates,
                datasets: [
                    {
                    label: 'Rate0',
                    data: rates0,//fill from service
                    fill: false,
                    borderColor: 'rgb(75, 192, 192)',
                    tension: 0.1
                    },
                    {
                        label: 'Rate1',
                        data: rates1,//fill from service
                        fill: false,
                        borderColor: 'rgb(88, 19, 33)',
                        tension: 0.1
                    }
                ]
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