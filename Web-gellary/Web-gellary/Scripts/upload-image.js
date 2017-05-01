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
            $(previewId).attr("src", fileData);
            var tmpImg = new Image();
            tmpImg.src = fileData;
            orgWidth = tmpImg.width;
            orgHeight = tmpImg.height;
            $(previewId).addClass(orgWidth > orgHeight ? "vertical-image" : "horizontal-image");
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

    $(submitId).click(function (e) {
        $.ajax({
            type: 'POST',
            url: "/Image/AddImageAjax",
            data: {
                fileName: fileInfo.name,
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
            },
            error: function () {
                alert('Error upload image');
            }
        });

        e.preventDefault();

    });

});