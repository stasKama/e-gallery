$(document).ready(function () {
    var buttonForm = ".button-form";
    var buttonFormText = ".button-form>div";
    var contentForm = ".content-form";
    var previewId = "#preview";
    var fileId = "#file";
    var fileLableId = "#lable-file";
    var submitId = "#upload";
    var save = ".save";
    var fieldNick = "#field-nick";
    var messageEditNick = "#message";

    $(buttonFormText).html("Open form");
    $(fileLableId).addClass("no-choose-file");
    $(submitId).addClass("no-active-submit");
    $(contentForm).hide();
    $(previewId).hide();

    $(buttonForm).click(function () {
        if ($(contentForm).is(':hidden')) {
            $(contentForm).show();
            $(buttonFormText).html("Close form");
        } else {
            $(contentForm).hide();
            $(buttonFormText).html("Open form");
        }
    });

    var fileInfo = {};

    $(fileId).change(function () {
        var file = this.files[0];
        var reader = new FileReader();
        reader.onload = function (event) {
            var fileData = event.target.result;
            $(previewId).attr("src", fileData);
            $(previewId).show();
            fileInfo.data = fileData;
            fileInfo.name = file.name;
            $(submitId).attr('disabled', false);
            $(submitId).removeClass("no-active-submit");
            $(submitId).addClass("active-submit");
            $(fileLableId).removeClass("no-choose-file");
            $(fileLableId).addClass("choose-file");
        };
        reader.readAsDataURL(file);
    });

    $(submitId).click(function () {
        fileInfo.name = fileInfo.name.split(".");
        ex = fileInfo.name[fileInfo.name.length - 1];
        $.ajax({
            type: 'POST',
            url: "/Edit/EditAvatar",
            data: {
                expansion: ex,
                fileData: fileInfo.data
            },
            success: function (data) {
                $(previewId).attr("src", "");
                $(previewId).hide();
                $(fileId).val("");
                $(submitId).attr('disabled', true);
                $(submitId).removeClass("active-submit");
                $(fileLableId).removeClass("choose-file");
                $(fileLableId).addClass("no-choose-file");
                $(submitId).addClass("no-active-submit");
            }
        });
    });

    $(save).click(function () {
        var nick = $(fieldNick).val();
        console.log(nick);
        $.ajax({
            type: 'POST',
            url: "/Edit/EditNick",
            data: {
                Nick: nick
            },
            success: function (data) {
                $(fieldNick).val("");
                $(messageEditNick).html("You update nick");
            }
        });
    });
});