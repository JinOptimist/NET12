$(document).ready(function () {
    $('.button').click(function () {

        let fullPath = $(this).attr('data-button-type');
        let path = $(this);
        
        switch (fullPath) {
            case 'bookName':
                colorIfPressed(path, 'bookName', 'change-color');
                break;
            case 'author':
                colorIfPressed(path, 'change-color');
                break;
            case 'oldDate':
                colorIfPressed(path, 'change-color');
                break;
            case 'newDate':
                colorIfPressed(path, 'change-color');
                break;
            case 'creator':
                colorIfPressed(path, 'change-color');
                break;          
        }
    });

    function colorIfPressed(path, button, newClass) {
        if (path.hasClass(newClass)) {
            path.toggleClass(newClass);
        }
        else {
            path.addClass(newClass);
        }
    }

    
});