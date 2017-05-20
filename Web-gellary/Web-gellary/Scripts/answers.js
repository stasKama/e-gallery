$(document).ready(function () {
    $.post("/Image/GetAnswers", function (data) {
        let answers = data;
        console.log(data);
        for (let i = 0; i < answers.length; i++) {
            let text = answers[i].Text.split("image");
            let message = "<div class='text'><div class='up'>" + text[0] + "<span class='tooltip'>image" +
                "<img src='" + answers[i].UrlImage + "'/>" + text[1] + "</span></div><div class='down'>" +
                answers[i].DateAnswer.split(" ")[0] + "</div></div>";
            let block = "<div class='answer'>" + message +
                "<div class='button-read'><div>Read</div></div>" +
                "</div>";
            $(".block-answers").append(block);
        }
    });

    $(".block-answers").on("click", ".button-read", function () {
        console.log("text");
    });

});