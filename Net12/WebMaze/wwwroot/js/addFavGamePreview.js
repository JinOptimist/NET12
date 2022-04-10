$(document).ready(function () {

    $(".favGame-preview-title").text("Your title");

    $('textarea[name="Name"]').bind('input', function () {
        let str = $(this).val();
        $(".favGame-preview-title").text(str);
        if ($(".favGame-preview-title").html() === "") {
            $(".favGame-preview-title").text("Your title");
        }
    });

    $('textarea[name="Genre"]').bind('input', function () {
        let str = $(this).val();
        $(".favGame-preview-genre").text(str);
    });

    $('textarea[name="YearOfProd"]').bind('input', function () {
        let str = $(this).val();
        $(".favGame-preview-year").text(str);
    });

    $('textarea[name="Desc"]').bind('input', function () {
        let str = $(this).val();
        $(".favGame-preview-description").text(str);
    });

    $('input[name="Rating"]').bind('input', function () {
        let str = $(this).val();
        $(".favGame-preview-rating").text(str+"/10");
    });

    let str = $('input[name="Rating"]').val();
    $(".favGame-preview-rating").text(str + "/10");

});


