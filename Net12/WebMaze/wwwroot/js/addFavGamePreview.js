$(document).ready(function () {

    $('textarea[name="Name"]').bind('input', function () {
        let str = $(this).val();
        $(".favGame-preview-title").text(str);
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

});


