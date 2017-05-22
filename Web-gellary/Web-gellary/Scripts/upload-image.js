$(document).ready(function () {
    var previewId = "#preview";
    var fileId = "#file";
    var fileLableId = "#lable-file";
    var submitId = "#upload";
    var messageId = "#message";

    $(fileLableId).addClass("no-choose-file");
    $(submitId).addClass("no-active-submit");

    var fileInfo = {};

    $(fileId).change(function () {
        $(messageId).hide();
        var file = this.files[0];
        var reader = new FileReader();
        reader.onload = function (event) {
            var fileData = event.target.result;
            var theImage = new Image();
            var imageWidth;
            var imageHeight;
            theImage.src = fileData;
            theImage.onload = function () {
                $(previewId).removeClass();
                $(previewId).attr("src", theImage.src);
                imageWidth = theImage.width;
                imageHeight = theImage.height;
                var classImage = imageWidth < 450 && imageHeight < 330 ? "" : imageWidth == imageHeight ? "horizontal-image" :
                 imageWidth > imageHeight ? "vertical-image" : "horizontal-image";
                $(previewId).addClass(classImage);
            };
                       
            fileInfo.data = fileData;
            fileInfo.name = file.name;
            $(messageId).text("");
            $(submitId).attr('disabled', false);
            $(submitId).removeClass("no-active-submit");
            $(submitId).addClass("active-submit");
            $(fileLableId).removeClass("no-choose-file");
            $(fileLableId).addClass("choose-file");
        };
        reader.readAsDataURL(file);
    });

    $(submitId).click(function () {
        ex = fileInfo.name.substring(1 + fileInfo.name.lastIndexOf("."));
        $.ajax({
            type: 'POST',
            url: "/Image/AddImageAjax",
            data: {
                expansion: ex,
                fileData: fileInfo.data
            },
            success: function (data) {
                $(previewId).attr("src", "");
                $(previewId).removeClass();
                $(fileId).val("");
                $(submitId).attr('disabled', true);
                $(submitId).removeClass("active-submit");
                $(fileLableId).removeClass("choose-file");
                $(fileLableId).addClass("no-choose-file");
                $(submitId).addClass("no-active-submit");
                $(messageId).text("The image was sent to the moderator");
            }
        });
        
    });

});