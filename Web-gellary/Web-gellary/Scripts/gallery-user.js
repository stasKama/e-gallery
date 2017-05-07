$(document).ready(function () {
    var maxIndex;
    var arrayImages;
    
    $.post("/Gallery/GetImages", function (data) {
        arrayImages = data;
        maxIndex = arrayImages.length;
        for (var i = 0; i < maxIndex; i++) {
            var img = new Image();
            img.src = arrayImages[i];
            imgWigth = img.width;
            imgHeigth = img.height;
            var classImage = imgWigth > imgHeigth ? "image-to-width" : "image-to-heigth";
            var image = '<img class="block-image ' + classImage + '" src="' + arrayImages[i] + '" alt="Can not display"/>'
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