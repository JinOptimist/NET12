$(document).ready(function () {

    $(".click-to-add-color").click(function () {

        if ($(".area-to-change-color").hasClass("click-to-change-color"))
        {
            $(".area-to-change-color").toggleClass("click-to-change-color");
        }
        else
        {
            $(".area-to-change-color").addClass("click-to-change-color");
        }
    });
    //".area-to-change-color"
    //'.area-to-change-color'
});



