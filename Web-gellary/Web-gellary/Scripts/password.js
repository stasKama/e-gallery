$(document).ready(function () {
    $(".view").mouseover(function () {
        $("#Password").prop("type", "text");
    });
    $(".view").mouseout(function () {
        $("#Password").prop("type", "password");
    });
});