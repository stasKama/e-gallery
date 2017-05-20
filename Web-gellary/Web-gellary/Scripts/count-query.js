$(document).ready(function () {
    $.post("/Gallery/GetCountQuery", function (data) {
        var count = data;
        $("#count-query").text(count == 0 ? "" : count);
    });
});