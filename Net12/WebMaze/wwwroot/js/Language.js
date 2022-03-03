$(document).ready(function () {

    let LangSelector = $("select.Language-Selector");
    let myLangNow = getCookieValue("Language");

    //DO: GET Languages from Server
    let AvailableLanguages = {
        1: "RU",
        2: "EN",
    };
    for (let i = 1; i <= Object.keys(AvailableLanguages).length; i++) {
        let option = $("<option>");
        option.attr("value", i);
        option.html(AvailableLanguages[i]);
        if (i == myLangNow) {
            option.attr("selected", "selected");
        }
        LangSelector.append(option);
    }


    $("select.Language-Selector").change(function () {
        let myLang = $(this).val();
        setTimeout(function () {

            location.reload();
        }, 10);
        document.cookie = "Language=" + myLang + "; path =/";
    });

});


function getCookieValue(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = document.cookie.length;
            }
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}