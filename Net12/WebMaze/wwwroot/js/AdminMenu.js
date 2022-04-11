$(document).ready(function () {


    $(".switch").click(function () {
        let url = $(this).attr("data-url");
        $.get(url)
            .done(function (httpCode) {

            });


    });
});

