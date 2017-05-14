$(document).ready(function () {
    var maxIndex;
    var arrayImages;
    
    $.post("/Gallery/GetImages", function (data) {
        arrayImages = data;
        maxIndex = arrayImages.length;
        for (var i = 0; i < maxIndex; i++) {
            var image = '<div class="block-image"><img src="' + arrayImages[i] + '" alt="Can not display"/></div>'
            $(".user-gallery").append(image);
        }
    });
    
    $(".user-gallery").on("click", "img", function () {
        $(".block-view-image").addClass("display-block");
        srcImage = $(this).attr("src");
        $("#view-image").attr("src", srcImage);
    });

    $("#view-image").click(function () {
        $(".block-view-image").removeClass("display-block");
    });

});