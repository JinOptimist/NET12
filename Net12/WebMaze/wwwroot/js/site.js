$(document).ready(function () {


    $('h1').click(function () {
        const second = new Date().getSeconds();
        $('h1 .seconds').text(second);

    });

    $("a[href='/Miner/StartGame']").click(function () {
        sessionStorage.clear();
    })

});


