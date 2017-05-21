$(document).ready(function () {
    var nick = "";
    var avatar = false;
    var online = false;
    var popular = false;

    $.post("/Gallery/GetUsers", function (data) {
        viewUsers(data);
    });

    function filter() {
        $(".list-users").html("");
        $.ajax({
            url: "/Gallery/Filter",
            type: "POST",
            data: {
                Nick: nick,
                Online: online,
                Avatar: avatar,
                Popular: popular 
            },
            traditional: true,
            dataType: "json",
            success: function (data) {
                viewUsers(data);
            }
        });
    }

    $(".avatar-filter").on("change", function () {
        avatar = $(this).prop("checked");
        console.log(avatar);
        filter();
    });

    $(".online-filter").on("change", function () {
        online = $(this).prop("checked");
        console.log(online);
        filter();
    });

    $(".popular-filter").on("change", function () {
        popular = $(this).prop("checked");
        console.log(popular);
        filter();
    });

    $(".nick-filter").keyup(function () {
        nick = $(".nick-filter").val();
        filter();
    });

    function viewUsers(users) {
        for (let i = 0; i < users.length; i++) {
            let icon = users[i].Status == "online" ? "fa fa-circle online" : "fa fa-circle no-online";
            let user = '<div class="user">' +
            '<div>' +
                '<a href="/Gallery/Home/' + users[i].Url + '">' +
                    '<img src="' + users[i].Avatar + '" class="avatar-user"/>' +
                '</a>' +
            '</div>' +
            '<div class="information">' +
                '<div class="status">' +
                     users[i].Status + '<i class="' + icon + '"></i>' +
                '</div>' +
                '<div>' +
                    '<a href="/Gallery/Home/' + users[i].Url + '">' +
                        users[i].Nick +
                    '</a>' +
                '</div>' +
                '<div class="upload-info">' +
                    'In the user gallery ' + users[i].CountUploadImages + ' images' +
               '</div>' +
            '</div>' +
        '</div>';
            $(".list-users").append(user);
        }
    }
});