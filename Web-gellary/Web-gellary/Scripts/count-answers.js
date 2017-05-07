$(document).ready(function () {
    $.post("/Image/GetCountAnswer", function (data) {
        var count = data;
        $("#count-answer").text(count == 0 ? "" : count);
    });
});