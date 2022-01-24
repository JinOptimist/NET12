$(document).ready(function () {
 
    $(".header").click(function () {
        if ($(".form").hasClass("change-background-color"))
        {
            $(".form").toggleClass("change-background-color");
        }
        else
        {
            $(".form").addClass("change-background-color");
        }
        
    });
    
});


