$(document).ready(function () {
    $('.button').click(function () {

        let fullPath = $(this).attr('data-button-type');
        let path = $(this);
        
        switch (fullPath) {
            case 'bookName':
                colorIfPressed(path);
                break;
            case 'author':
                colorIfPressed(path);
                break;
            case 'oldDate':
                colorIfPressed(path);
                break;
            case 'newDate':
                colorIfPressed(path);
                break;
            case 'creator':
                colorIfPressed(path);
                break;          
        }
    });

    function colorIfPressed(path) {

        if (path.hasClass('change-color')) {
            path.toggleClass('change-color');
        }
        else {
            path.addClass('change-color');
        }
    }

    
});