$(document).ready(function () {
    var arrayImages = [];
    var srcImage;
    var indexImage;

    $.post("/Image/GetCountImages", function (data) {
        $(".count-images").text(data);
    });

    function uploadImageView() {
        $.post("/Gallery/GetHistory", function (data) {
            arrayImages = arrayImages.concat(data);
            let viewImages = data;
            if (viewImages.length > 0) {
                for (var i = 0; i < viewImages.length; i++) {
                    var image = '<div class="block-image"><img src="' + viewImages[i] + '" alt="Can not display"/></div>'
                    $(".block-gallery").append(image);
                }
            }
        });
    }

    function getInformationImage() {
        var name = srcImage.substring(srcImage.lastIndexOf("/") + 1, srcImage.lastIndexOf("."));
        $.ajax({
            url: "/Image/GetInformationImage",
            type: "POST",
            data: {
                UrlImage: name
            },
            traditional: true,
            dataType: "json",
            success: resultFunction,
            error: errorFunc
        });
    }

    function resultFunction(data) {
        var info = data;
        $(".view>span").text(info.CountView);
        $(".name-author>a").attr("href", "/Gallery/Home/" + info.UrlUser);
        $(".data").text(info.UploadData.substring(0, 10));
        $(".name-author>a>span").text(info.AuthorName);
        $(".img-author>img").attr("src", info.Avatar);
        $(".like>span").text(info.CountLike);
    }

    function showComments() {
        var name = srcImage.substring(srcImage.lastIndexOf("/") + 1, srcImage.lastIndexOf("."));
        $.ajax({
            url: "/Image/GetComments",
            type: "POST",
            data: {
                UrlImage: name
            },
            traditional: true,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    var info = data;
                    $(".comments").html("");
                    for (var i = 0; i < info.length; i++) {
                        var linkUser = "/Gallery/Home/" + info[i].UrlUser;
                        var comment = '<div class="comment">' +
                            '<div class="left"><a href="' + linkUser + '">' +
                            '<img src="' + info[i].Avatar + '"/></a></div>' +
                            '<div class="right"><div><a href="' + linkUser + '">' + info[i].AuthorName + '</a>' +
                             '<div class="message">' + info[i].TextComment + '</div></div>' +
                            '<div><span>' + info[i].DataComment.substring(0, 10) + '</span></div>' +
                            '</div></div>';
                        $(".comments").append(comment);
                    }
                }
            },
            error: errorFunc
        });
    }

    function errorFunc() {
        
    }

    uploadImageView();

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 10) {
            uploadImageView();
        }
    });

    $(".block-gallery").on("click", "img", function () {
        $(".panel-view-img").addClass("display-block");
        $("html").addClass("hide-scroll");
        $("a.scrollup").hide();
        srcImage = $(this).attr("src");
        indexImage = arrayImages.indexOf(srcImage);
        $(".digits").text(indexImage + 1);
        $("#select-image").attr("src", srcImage);
        if (indexImage == arrayImages.length - 1 || indexImage == arrayImages.length - 2) {
            uploadImageView();
        }
        testButton();
        getInformationImage();
        showComments();
    });

    $("#select-image").click(function () {
        $("html").removeClass("hide-scroll");
        $("a.scrollup").show();
        $(".panel-view-img").removeClass("display-block");
    });

    $("textarea").keyup(function (e) {
        if (e.keyCode == 13) {
            setCommentImage();
        }
    });

    function setCommentImage() {
        var comment = $("textarea").val().replace(/\s{2,}/g, ' ');
        var imageName = srcImage.substring(srcImage.lastIndexOf("/") + 1, srcImage.lastIndexOf("."));
        $.ajax({
            url: "/Image/SetComment",
            type: "POST",
            data: {
                UrlImage: imageName,
                Comment: comment
            },
            traditional: true,
            dataType: "json",
            success: function (data) {
                $("textarea").val("");
                showComments();
            },
            error: errorFunc
        });
    }

    $(".prev-image").click(function () {
        srcImage = arrayImages[--indexImage];
        setImage(srcImage);
    });

    $(".next-image").click(function () {
        srcImage = arrayImages[++indexImage];
        if (indexImage == arrayImages.length - 2) {
            uploadImageView();
        }
        setImage(srcImage);
    });

    function testButton() {
        $(".prev-image").show();
        $(".next-image").show();
        if (indexImage == 0) {
            $(".prev-image").hide();
        }
        if (indexImage == arrayImages.length - 1) {
            $(".next-image").hide();
        }
    }

    function setImage(link) {
        $(".digits").text(indexImage + 1);
        $("#select-image").attr("src", link);
        getInformationImage();
        showComments();
        testButton();
    }

    $(".like").click(function () {
        var imageName = srcImage.substring(srcImage.lastIndexOf("/") + 1, srcImage.lastIndexOf("."));
        $.ajax({
            url: "/Image/PutImageLikes",
            type: "POST",
            data: {
                UrlImage: imageName
            },
            traditional: true,
            dataType: "json",
            success: function (data) {
                $(".like>span").text(data);
            },
            error: errorFunc
        });

    });

});