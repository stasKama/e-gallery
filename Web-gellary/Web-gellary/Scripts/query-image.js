$(document).ready(function () {
    var maxIndex;
    var arrayImages;
    var srcImage;

    $.ajax({
        url: "/Gallery/GetQueryImages",
        type: "POST",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(data, status) {
        arrayImages = data;
        maxIndex = arrayImages.length;
        for (var i = 0; i < maxIndex; i++) {
            var img = new Image();
            img.src = arrayImages[i];
            imgWigth = img.width;
            imgHeigth = img.height;
            var classImage = imgWigth > imgHeigth ? "image-to-width" : "image-to-heigth";
            var image = '<img class="' + classImage + '" src="' + arrayImages[i] + '" alt="Can not display"/>'
            $(".block-user-query").append("<div class='block-image'>" + image + "</div>");
        }
    }

    function errorFunc(errorData) {
        alert('Ошибка загрузки');
    }

    $(document).on("click", ".block-user-query .block-image img", function () {
        $("html>body>a").removeClass("scroll-html");
        $(".block-view-image").addClass("display-block");
        srcImage = $(this).attr("src");
        var img = new Image();
        img.src = srcImage;
        imgWigth = img.width;
        imgHeigth = img.height;
        var classImage = imgWigth > imgHeigth ? "display-image-to-width" : "display-image-to-heigth";
        $("#display-image").attr("src", srcImage);
        $("#display-image").removeClass();
        $("#display-image").addClass(classImage);
    });

    $("#one-img").click(function () {
        $("html>body>a").addClass("scroll-html");
        $(".block-view-image").removeClass("display-block");
    });
});