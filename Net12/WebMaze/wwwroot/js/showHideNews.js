$(document).ready(function () {
    let btn = $(".btn");
    let news = $(".users-news");

    btn.click(showHideNews);

    function showHideNews() {

        if (news.hasClass('hidden')) {
            btn.text('HIDE');
        }
        else {
            btn.text('SHOW');
        }

        news.toggleClass("hidden");
    }
});
