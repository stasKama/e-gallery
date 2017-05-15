$(document).ready(function () {
    var users;
    $.post("/Gallery/GetUsers", function (data) {
        users = data;
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
    });
});