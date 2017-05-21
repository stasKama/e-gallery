$(document).ready(function () {
    var textStatus;

    $(".user-status").click(function () {
        $(this).hide();
        textStatus = $(this).html();
        $(".enter-status").show();
        $(".enter-status").val($(this).html() == '"Click to change your status"' ? "" : $(this).html());
        $(".enter-status").focus();
    });

    $(".enter-status").blur(function () {
        if (textStatus != $(this).val()) {
            var status = this;
            $.ajax({
                url: "/Gallery/SetStatusUser",
                type: "POST",
                data: {
                    Status: $(status).val()
                },
                traditional: true,
                dataType: "json",
                success: function (data) {
                   
                }
            });
        }
        $(this).hide();
        $(".user-status").html($(this).val() == "" ? '"Click to change your status"' : $(this).val());
        $(".user-status").show();
    });

});