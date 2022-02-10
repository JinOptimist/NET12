$(document).ready(function () {
    $('.linkToPage').click(function (evt) {
        const perPage = $('.perPageOption').val();
        const urlTemplate = $(this).attr('href');
        const url = urlTemplate + perPage;
        window.location.href = url;

        evt.preventDefault();
    });

    $('.perPageOption').change(function () {
        const perPage = $(this).val();
        const currentUrl = new URLSearchParams(window.location.href);
        currentUrl.set("perPage", perPage);
        if (!currentUrl.has('page')) {
            currentUrl.set("page", 1);
        }
        const newUrl = currentUrl.toString();
        
        window.location.href = decodeURIComponent(decodeURIComponent(newUrl));
    });
});