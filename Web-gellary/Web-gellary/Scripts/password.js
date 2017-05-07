$(document).ready(function () {
    $(".view").mousedown(function () {
        var password = $($(this).parent()).children(".password-view");
        $(password).prop("type", "text");
    });
    $(".view").mouseup(function () {
        var password = $($(this).parent()).children(".password-view");
        $(password).prop("type", "password");
    });
});