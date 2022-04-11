$(document).ready(function () {
    $(".asc-button-press").click(function () {

        $(".sorting-button").each(function (link) {
            let template = $(this).attr('href');
            template = template.substring(0, template.indexOf('&'));
            $(this).attr('href', template + "&ascDirection=true");
        })

    });

    $(".desc-button-press").click(function () {

        $(".sorting-button").each(function () {
            let template = $(this).attr('href');
            template = template.substring(0, template.indexOf('&'));
            $(this).attr('href', template + "&ascDirection=false");
        })

    });

});