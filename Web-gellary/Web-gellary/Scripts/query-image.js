$(document).ready(function () {
    var maxIndex;
    var arrayImages;
    var srcImage;
    var indexImage;

    $.post("/Image/GetQueryImages", function (data) {
        arrayImages = data;
        if (arrayImages.length > 0) {
            for (var i = 0; i < arrayImages.length; i++) {
                var image = '<div class="block-image"><img src="' + arrayImages[i] + '" alt="Can not display"/></div>'
                $(".block-gallery").append(image);
            }
        }
        $(".count-images").text(arrayImages.length);
    });

    function errorFunc(errorData) {
        alert("Error upload page");
    }

    $(".block-gallery").on("click", "img", function () {
        $(".block-view-image").addClass("display-block");
        srcImage = $(this).attr("src");
        indexImage = arrayImages.indexOf(srcImage);
        var img = new Image();
        img.src = srcImage;
        imgWigth = img.width;
        imgHeigth = img.height;
        var classImage = imgWigth > imgHeigth ? "display-image-to-width" : "display-image-to-heigth";
        $("#display-image").attr("src", srcImage);
        $("#display-image").removeClass();
        $("#display-image").addClass(classImage);
        testButton();
    });

    $("#display-image").click(function () {
        $(".block-view-image").removeClass("display-block");
    });

    function getImage() {
        return $('.block-gallery .block-image img[src="' + srcImage + '"]');
    }

    $("#block").click(function () {
        var name = srcImage.substring(srcImage.lastIndexOf("/") + 1, srcImage.lastIndexOf("."));
        $.ajax({
            type: "POST",
            url: "/Image/Block",
            data: {
                UrlImage: name
            },
            traditional: true,
            dataType: "json",
            success: resultFunction,
            error: errorFunc
        });
    });

    $("#accept").click(function () {
        var name = srcImage.substring(srcImage.lastIndexOf("/") + 1, srcImage.lastIndexOf("."));
        $.ajax({
            url: "/Image/Accect",
            type: "POST",
            data: {
                UrlImage: name
            },
            traditional: true,
            dataType: "json",
            success: resultFunction,
            error: errorFunc
        });
    });

    function resultFunction(data) {
        $(getImage()).parent().remove();
        arrayImages = arrayImages.filter(function (x) {
            return x != srcImage;
        });
        if (arrayImages.length == 0) {
            $(".block-view-image").removeClass("display-block"); 
        } else {
            if (indexImage < arrayImages.length) {
                nextImage();
            } else {
                prevImage();
            }
        }
        $("#count-query").text(arrayImages.length == 0 ? "" : arrayImages.length);
    }

    $("#prev-image").click(function () {
        prevImage();
    });

    $("#next-image").click(function () {
        nextImage();
    });

    function testButton() {
        $("#prev-image").show();
        $("#next-image").show();
        if (indexImage == 0) {
            $("#prev-image").hide();
        }
        if (indexImage == arrayImages.length - 1) {
            $("#next-image").hide();
        }
    }

    function nextImage() {
        srcImage = arrayImages[++indexImage];
        $("#display-image").attr("src", srcImage);
        testButton();
    }

    function prevImage() {
        srcImage = arrayImages[--indexImage];
        $("#display-image").attr("src", srcImage);
        testButton();
    }
});