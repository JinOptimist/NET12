$(document).ready(function () {
    $(".asc-button1").click(function () {

        let count = $(".sorting-buttons a").length;

        for (let i = 0; i < count; i++) {
            let template = $("#sorting-button"+i).attr('href');
            template = template.substring(0, template.indexOf('&'));
            $("#sorting-button"+i).attr('href', template + "&ascDirection=true");
        }

    });

    $(".desc-button1").click(function () {

        let count = $(".sorting-buttons a").length;

        for (let i = 0; i < count; i++) {
            let template = $("#sorting-button" + i).attr('href');
            template = template.substring(0, template.indexOf('&'));
            $("#sorting-button" + i).attr('href', template + "&ascDirection=false");
        }

    });

});