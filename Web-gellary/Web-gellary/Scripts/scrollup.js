$(document).ready(function () {

    $(window).scroll(function () {
        if ($(document).scrollTop() > 75) {
            $("#menu").addClass("fixed");
            $(".scrollup").fadeIn();
        }
        else {
            $("#menu").removeClass("fixed");
            $(".scrollup").fadeOut();
        }
    });

    $(".scrollup").click(function () {
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });

});