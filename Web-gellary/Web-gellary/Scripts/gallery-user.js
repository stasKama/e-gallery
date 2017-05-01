$(document).ready(function () {
    var maxIndex;
    var arrayImages;
    var url = window.location.href;
    url = url.substring(url.lastIndexOf("/") + 1);
    console.log(url);

    $.ajax({
        url: "/Gallery/GetImages",
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
        console.log(arrayImages);
        console.log(maxIndex);
        for (var i = 0; i < maxIndex; i++) {
            var img = new Image();
            img.src = arrayImages[i];
            imgWigth = img.width;
            imgHeigth = img.height;
            var classImage = imgWigth > imgHeigth ? "image-to-width" : "image-to-heigth";
            console.log(classImage);
            var image = '<img class="' + classImage + '" src="' + arrayImages[i] + '" alt="Can not display"/>'
            $(".user-gallery").append("<div class='block-image'>" + image + "</div>");
        }
    }

    function errorFunc(errorData) {
        alert('Ошибка загрузки');
    }

});