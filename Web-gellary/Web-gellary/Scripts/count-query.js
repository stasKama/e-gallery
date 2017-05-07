$(document).ready(function () {
    $.post("/Image/GetCountQuery", function (data) {
        var count = data;
        $("#count-query").text(count == 0 ? "" : count);
    });
});