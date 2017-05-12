$(document).ready(function () {
    var arrayImages = [];
    var srcImage;
    var indexImage;

    function uploadImageView() {
        $.post("/Gallery/GetHistory", function (data) {
            arrayImages = arrayImages.concat(data);
            let viewImages = data;
            if (viewImages.length > 0) {
                for (var i = 0; i < viewImages.length; i++) {
                    var image = '<div class="block-image"><img class="image" src="' + viewImages[i] + '" alt="Can not display"/></div>'
                    $(".block-history").append(image);
                }
            }
        });
    }

    uploadImageView();

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 10) {
            uploadImageView();
        }
    });
});