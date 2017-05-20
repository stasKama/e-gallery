$(document).ready(function () {
    $.post("/Gallery/GetCountAnswer", function (data) {
        var count = data;
        $("#count-answer").text(count == 0 ? "" : count);
    });
});