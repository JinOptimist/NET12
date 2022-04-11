$(document).ready(function () {
    $('#ios-toggle').val($(this).is(':checked'));

    $('#ios-toggle').change(function () {
        if ($(this).is(":checked")) {
            //url
            $(this).attr('value', 'true');
            $('.file').prop("disabled", true);
            $('.addFieldURL').prop("disabled", false);
            $('.file').val('');
        } else {
            //file
            $(this).attr('value', 'false');
            $('.file').prop("disabled", false);
            $('.addFieldURL').attr("disabled", true);
            $('.addFieldURL').val('');
        }

    });
});