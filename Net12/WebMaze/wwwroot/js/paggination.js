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
        moveToPage(1, perPage);
    });

    $('.next').click(moveNext);

    $('.prev').click(movePrev);

    $(document).keydown(function (evt) {
        //arrow right
        if (event.which == 39) {
            moveNext();
        }

        //arrow left
        if (event.which == 37) {
            movePrev();
        }
    });

    function moveNext(evt) {
        let page = $('.CurrPage').val() - 0;
        let TotalPageCount = $('.TotalPageCount').val() - 0;
        if (page < TotalPageCount) {
            page++;
        }

        moveToPage(page);

        evt.preventDefault();
    }

    function movePrev(evt) {
        let page = $('.CurrPage').val() - 0;
        if (page > 1) {
            page--;
        }

        moveToPage(page);

        evt.preventDefault();
    }

    function moveToPage(page, perPage) {
        if (!perPage) {
            perPage = $('.perPageOption').val();
        }
        const domain = window.location.href.split('?')[0];
        const url = `${domain}?page=${page}&perPage=${perPage}`;
        window.location.href = url;
    }
});