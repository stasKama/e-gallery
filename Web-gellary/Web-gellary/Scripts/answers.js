$(document).ready(function () {
    var countAnswers = 0;

    $.post("/Image/GetAnswers", function (data) {
        let answers = data;
        countAnswers = answers.length;
        for (let i = 0; i < answers.length; i++) {
            let text = answers[i].Text.split("image");
            let message = "<div class='text'><div class='up'>" + text[0] + "<span class='tooltip'>image" +
                "<img class='image-answer' src='" + answers[i].UrlImage + "'/>" + text[1] + "</span></div><div class='down'>" +
                answers[i].DateAnswer.split(" ")[0] + "</div></div>";
            let block = "<div class='answer'>" + message +
                "<div class='button-read'><div>Read</div></div>" +
                "</div>";
            $(".block-answers").append(block);
        }
    });

    $(".block-answers").on("click", ".button-read", function () {
        var block = this;
        var srcImageAnswer = $(".image-answer").attr("src");
        srcImageAnswer = srcImageAnswer.split("Images/")[1].split("/");
        var directory = srcImageAnswer[0];
        var nameImage = srcImageAnswer[1].split(".")[0];
        $.ajax({
            url: "/Image/ReadAnswer",
            type: "POST",
            data: {
                NameImage: nameImage,
                Directory: directory
            },
            traditional: true,
            dataType: "json",
            success: function (data) {
                $(block).parent().remove();
                $("#count-answer").text(--countAnswers == 0 ? "" : countAnswers);
            }
        });
    });

});