var postCommentViewModel = {
    lastPostId: ko.observable(),
    lastCreatedAt: ko.observable(),
    recentPostId: ko.observable(),
    recentCreatedAt: ko.observable(),
    uploadprofilePicObj: new uploadImageObj('ads'),
    posturlContent: [],
    previewContent: ko.observable(''),
    msgContent: ko.observable(''),
    cost: ko.observable(0),
    totalCost: ko.observable(0),
    availableCash: ko.observable(0),
    tax: ko.observable(0),
    taxRate: ko.observable(0),
    picture: ko.observable(0),
    fullName: ko.observable(0),
    userId: ko.observable(0),

};
var postViewTemplate = "";
var commentsViewTemplate = "";
var commentsTreeViewTemplate = "";
var postViewData;
var postCommentViewData;
var commentReplyViewData;
var postData;
var postCommentData;
function initializebuyspot() {
    postCommentViewModel.availableCash = userBankAccountViewModel.Cash;
    postCommentViewModel.picture = peopleInfo.Picture;
    postCommentViewModel.fullName = peopleInfo.FullName;
    postCommentViewModel.userId = peopleInfo.UserId;
    postCommentViewModel.taxRate(taxViewModel.TaxType()[taxadsCode - 1].TaxPercent());
    setupspot();
}
function setupspot() {
    $("#spotupload").change(function (evt) {
        if (this.disabled) return alert('File upload not supported!');
        handleFileSelect(evt,
            $("#spotuploadprw"), postCommentViewModel.uploadprofilePicObj, '', spotpreview);
    });
    ko.applyBindings(postCommentViewModel, $("#newsspot")[0]);
    $('#spotContent').on('keydown blur', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (e.type == 'blur') {
            spotpreview();
        }

        if (code == 32 || code == 13) {
            spotpreview();
        }
        if (code == 8 || code == 46) {
            CleanUpUrls(postCommentViewModel.msgContent(), 'Post', postCommentViewModel);
            spotpreview();
        }
    });
    $('#savespot').click(function () {
        var btn = $(this)
        var vm = postCommentViewModel.uploadprofilePicObj;
        btn.button('loading')
        uploadImage(vm, $("#spotuploadprw"), function () {
            SaveSpot(btn);
        });
    });
    $("#spotreset").on("click", function () {
        resetSpot();
    });
}
function resetSpot() {
    $('#spotContent').val('');
    $('#spotuploadprw').html('');
    postCommentViewModel.uploadprofilePicObj.blobSasUrl('');
    postCommentViewModel.uploadprofilePicObj.buffer = '';
    postCommentViewModel.uploadprofilePicObj.validImage = false;
    spotpreview();
}
$(document).ready(function () {

    $("#timefeed").on("click", "li a.morepostcomment", function () {
        var childcount = $(this).closest('li').data("child");
        console.log($(this));
        if (parseInt(childcount) > 0) {
            postId = $(this).closest('li').data("postid");
            lastDateTime = $(this).closest('li').find('aside >article').last().data("lastdatetime");
            getDataMoreComment(postId, lastDateTime);
        }
        else {
            $(this).addClass('hidden');
        }
    });
    $("#timefeed").on("click", "li .timeline-stat .dig-it", function () {
        var coolit = $(this).closest('.timeline-stat').find(".cool-it");
        toggledigit($(this), $(this).closest('li').data("postid"), coolit, 'P');
    });

    $("#timefeed").on("click", "li .timeline-stat .cool-it", function () {
        var digit = $(this).closest('.timeline-stat').find(".dig-it");
        togglecoolit($(this), $(this).closest('li').data("postid"), digit, 'P');
    });

    $("#timefeed").on("click", "li aside article .dig-it", function () {
        var coolit = $(this).closest('footer').find(".cool-it");
        toggledigit($(this), $(this).closest('article').data("commentid"), coolit, 'C');
    });

    $("#timefeed").on("click", "li aside article .cool-it", function () {
        var digit = $(this).closest('footer').find(".dig-it");
        togglecoolit($(this), $(this).closest('article').data("commentid"), digit, 'C');
    });
    $("#timefeed").on("click", "li aside article a.morechildcomment", function () {
        var childcount = $(this).closest('article').data("child");
        if (parseInt(childcount) > 0) {
            parentCommentId = $(this).closest('article').data("commentid");
            lastDateTime = $(this).closest('article').find('article').last().data("lastdatetime");
            getDataMoreChildComment(parentCommentId, lastDateTime);
        }
        else {
            $(this).addClass('hidden');
        }
    });

    $("#timefeed").on("click", "a.reply-link", function () {
        console.log($(this));
        var commentbox = $(this).parent().find('.commentbox');
        commentbox.removeClass('hidden');
        commentbox.focus();
        $(this).addClass('hidden');
    });


    $("#timefeed").on("keydown", "li .post-reply", function (event) {
        if (event.which == 13) {
            var self = $(this);
            SaveComment(self, CollectPostCommentData(self));
        }
    });
    $("#timefeed").on("keydown", "article .comment-reply", function (event) {
        if (event.which == 13) {
            var self = $(this);
            SaveComment(self, CollectChildCommentData(self));
            $(this).addClass('hidden');
            $(this).next('.reply-link').removeClass('hidden');
        }
    });
});

function getTimeLineView() {
    return $.loadViewNoCallBack("/postcomment/getcommentview");
}
function getreplyview() {
    return $.loadViewNoCallBack("/postcomment/getreplyview");
}
function getpostview() {
    return $.loadViewNoCallBack("/postcomment/getpostview");
}
function getDataPostComment(newpost) {
    var datapost;
    if (newpost) {
        datapost = {
            LastPostId: postCommentViewModel.recentPostId(),
            LastCreatedAt: postCommentViewModel.recentCreatedAt(),
            NewPost: newpost
        };
    }
    else {
        datapost = {
            LastPostId: postCommentViewModel.lastPostId(),
            LastCreatedAt: postCommentViewModel.lastCreatedAt(),
            NewPost: newpost
        };
    }

    return $.ajax({
        url: "/api/postcommentservice/getpostcommentlist",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(datapost),
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function getDataMoreComment(postId, lastDateTime) {
    $.ajax({
        url: "/api/postcommentservice/getmorecommentlist",
        type: "get",
        contentType: "application/json",
        data: {
            postId: postId,
            lastDateTime: lastDateTime
        },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    }).done(function (data) {
        if (data.length > 0) {
            for (i = 0; i < data.length; i++) {
                postCommentViewData = ko.mapping.fromJS(data[i]);
                AddComment(postCommentViewData);
            }
        } else {
            $('li[data-postid=' + postId + '] a.morepostcomment').first().addClass("hidden");
        }
    });
}
function getDataMoreChildComment(parentCommentId, lastDateTime) {
    $.ajax({
        url: "/api/postcommentservice/getmorechildcommentlist",
        type: "get",
        contentType: "application/json",
        data: {
            parentCommentId: parentCommentId,
            lastDateTime: lastDateTime
        },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    }).done(function (data) {
        if (data.length > 0) {
            for (i = 0; i < data.length; i++) {
                postCommentViewData = ko.mapping.fromJS(data[i]);
                AddComment(postCommentViewData);
            }
        } else {
            $('article[data-commentId=' + parentCommentId + ']  a.morechildcomment').first().addClass("hidden");

        }
    });
}
function AddComment(data) {
    if ($('article[data-commentid=' + data.PostCommentId() + ']').length == 0) {
        if (data.ParentCommentId() == null) {
            $('li[data-postId=' + data.PostId() + '] aside').removeClass("hidden");
            $('li[data-postId=' + data.PostId() + '] aside').append(commentsViewTemplate);
            ko.applyBindings(data,
       $('li[data-postId=' + data.PostId() + '] aside article').last().get(0));

        }
        else {
            $('article[data-commentid=' + data.ParentCommentId() + ']').append(commentsTreeViewTemplate);
            ko.applyBindings(data,
    $('article[data-commentId=' + data.ParentCommentId() + '] > article').last().get(0));

        }
        var childcount = $('article[data-commentId=' + data.ParentCommentId() + ']').data('child');
        var postcount = $('li[data-postid=' + data.PostId() + ']').data('child');
        if ($('article[data-commentId=' + data.ParentCommentId() + '] > article').size() >= childcount) {
            $('article[data-commentId=' + data.ParentCommentId() + ']  a.morechildcomment').first().addClass("hidden");

        }

        if ($('li[data-postid=' + data.PostId() + ']  article').size() >= postcount) {
            $('li[data-postid=' + data.PostId() + '] a.morepostcomment').first().addClass("hidden");
        }
    }
}
function AddPost(data, append) {
    if ($('li[data-postid=' + data.PostId() + ']').length == 0) {
        if (append == true) {
            $("#timefeed").append(postViewTemplate);
            ko.applyBindings(data, $("#timefeed li.post").last().get(0));
        }
        else {
            $("#timefeed").prepend(postViewTemplate);
            ko.applyBindings(data, $("#timefeed li.post").first().get(0));
        }

    }
}
function SaveComment(btn, postData) {
    if (postData.CommentText.length > 0) {
        $.ajax({
            url: "/api/postcommentservice/savepostcomment",
            type: "post",
            contentType: "application/json",
            data: ko.toJSON(postData),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                postCommentViewData = ko.mapping.fromJS(result);
                postCommentViewData.FullName = ko.observable(peopleInfo.FullName());
                postCommentViewData.Picture = ko.observable(peopleInfo.Picture());
                AddComment(postCommentViewData);
                btn.val('');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                btn.button('reset');
            }
        }).always(function () {
            btn.button('reset');
            $(btn).parent().prev("input").val("");
        });
    } else {
        btn.button('reset');
    }
}
function CollectPostCommentData(btn) {
    var postId = $(btn).closest('li').data("postid");
    var msg = $(btn).val();
    var postComment = {
        PostId: postId,
        CommentText: msg
    };
    return postComment;
}
function CollectChildCommentData(btn) {
    var postId = $(btn).closest('article').data("parentpostid");
    var paremntCommentid = $(btn).closest('article').data("commentid");
    var msg = $(btn).val();
    var childComment = {
        PostId: postId,
        CommentText: msg,
        ParentCommentId: paremntCommentid
    };
    return childComment;
}
function ProcessPost(append) {
    if (postData.Posts != null) {
        if (postData.Posts.length > 0) {
            postCommentViewModel.recentPostId(postData.Posts[0].PostId);
            postCommentViewModel.recentCreatedAt(postData.Posts[0].CreatedAt);
            for (i = 0; i < postData.Posts.length; i++) {
                var match = getFriendById(postData.Posts[i].UserId);
                if (match != null) {
                    postData.Posts[i].FullName = match.FullName;
                    postData.Posts[i].Picture = match.Picture;
                }
                else if (postData.Posts[i].UserId == peopleInfo.UserId()) {
                    postData.Posts[i].FullName = peopleInfo.FullName();
                    postData.Posts[i].Picture = peopleInfo.Picture();
                }
                postData.Posts[i].ShortDescription = ko.observable('');
                if (postData.Posts[i].PostContentTypeId > 0) {
                    var index = postData.Posts[i].PostContentTypeId - 1;
                    postData.Posts[i].PostContent = codeTables.PostContentType()[index].Description();
                    postData.Posts[i].ShortDescription = codeTables.PostContentType()[index].ShortDescription();
                    postData.Posts[i].FontCss = codeTables.PostContentType()[index].FontCss();
                    postData.Posts[i].ImageFont = codeTables.PostContentType()[index].ImageFont();

                    if (postData.Posts[i].Parms != "") {
                        postData.Posts[i].PostContent =
                       postData.Posts[i].PostContent.format.apply(postData.Posts[i].PostContent,
                               postData.Posts[i].Parms.split("|"));
                        postData.Posts[i].PostContent = replaceAll(postData.Posts[i].PostContent, ":::p-pic", profilepicContainer);
                    }
                }
                postViewData = ko.mapping.fromJS(postData.Posts[i]);
                AddPost(postViewData, append);
            }
        }
        if (postData.PostComments != null && postData.PostComments.length > 0) {
            for (i = 0; i < postData.PostComments.length; i++) {
                postCommentViewData = ko.mapping.fromJS(postData.PostComments[i]);
                AddComment(postCommentViewData);
            }
            postCommentViewModel.lastPostId(postData.Posts[postData.Posts.length - 1].PostId);
            postCommentViewModel.lastCreatedAt(postData.Posts[postData.Posts.length - 1].CreatedAt);
        }
    }
}

function savePostVote(btn, voteData, newcss, oldcss, newvote) {
    $.ajax({
        url: "/api/postcommentservice/SavePostVote",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(voteData),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                btn.removeClass(oldcss);
                btn.addClass(newcss);
                btn.next().text(newvote);
                btn.data("digtype", voteData.DigType);
            }
        }
    });
}
function toggledigit(btn, postId, nextdig, postcommentType) {
    var digtype = btn.data("digtype");
    var newcss, oldcss = '';
    var newvote = parseInt(btn.next().text());
    if (digtype == 1) {
        newcss = 'fa icon-emoticon101';
        oldcss = 'fa icon-emoticon37';
        newvote--;
    }
    else {
        newcss = 'fa icon-emoticon37';
        oldcss = 'fa icon-emoticon101';
        newvote++;
    }
    var voteData = {
        DigType: digtype == 1 ? 0 : 1,
        PostCommentId: postId,
        PostCommentType: postcommentType,
        OldDigType: Math.max(digtype, nextdig.data("digtype"))
    }
    digtype = nextdig.data("digtype");
    savePostVote(btn, voteData, newcss, oldcss, newvote);
    newvote = parseInt(nextdig.next().text());
    if (digtype == 2) {
        newcss = 'fa icon-happy38';
        oldcss = 'fa icon-happy24';
        newvote--;
        nextdig.removeClass(oldcss);
        nextdig.addClass(newcss);
        nextdig.next().text(newvote);
        nextdig.data("digtype", 0);
    }

}
function togglecoolit(btn, postId, nextdig, postcommentType) {

    var digtype = btn.data("digtype");
    var newcss, oldcss = '';
    var newvote = parseInt(btn.next().text());

    if (digtype == 2) {
        newcss = 'fa icon-happy38';
        oldcss = 'fa icon-happy24';
        newvote--;
    }
    else {
        newcss = 'fa icon-happy24';
        oldcss = 'fa icon-happy38';
        newvote++;
    }
    var voteData = {
        DigType: digtype == 2 ? 0 : 2,
        PostCommentId: postId,
        PostCommentType: postcommentType,
        OldDigType: Math.max(digtype, nextdig.data("digtype"))
    }
    digtype = nextdig.data("digtype");
    savePostVote(btn, voteData, newcss, oldcss, newvote);
    newvote = parseInt(nextdig.next().text());
    if (digtype == 1) {
        newcss = 'fa icon-emoticon101';
        oldcss = 'fa icon-emoticon37';
        newvote--;
        nextdig.removeClass(oldcss);
        nextdig.addClass(newcss);
        nextdig.next().text(newvote);
        nextdig.data("digtype", 0);
    }
}
function spotpreview() {
    var content = htmlEscape($('#spotContent').val());
    var imageurl = '';
    buildWebContent(content, 'Post', true, postCommentViewModel, function () {
        for (var i = 0; i < postCommentViewModel.posturlContent.length; i++) {
            var item = postCommentViewModel.posturlContent[i];
            if (item.Content) {
                content = content.split(item.Uri).join(item.Content) //find and replace
            }
        }
        content = content.replace(/\r\n|\r|\n/g, "<br />");
        postCommentViewModel.previewContent(content);
        calcSpotTotal();
    });
}
function calcSpotTotal() {

    var costTotal = postCommentViewModel.previewContent().length;
    var taxTotal = 0;
    var total = 0;
    if (typeof postCommentViewModel.uploadprofilePicObj.buffer.byteLength != 'undefined') {
        costTotal += postCommentViewModel.uploadprofilePicObj.buffer.byteLength * .05;
    }
    console.log(costTotal);
    costTotal = costTotal * .05;
    taxTotal = (postCommentViewModel.taxRate() / 100) * costTotal;
    total = costTotal + taxTotal;
    postCommentViewModel.cost(costTotal);
    postCommentViewModel.tax(taxTotal);
    postCommentViewModel.totalCost(total);

}
function SaveSpot(btn) {
    $.ajax({
        url: "/api/PostCommentService/SaveSpot",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(collectDataSpot()),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var adsDetailsViewModelresult = ko.mapping.fromJS(result);
            if (adsDetailsViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                resetSpot();
                getDataPostComment(true).done(function (result) {
                    postData = result;
                    ProcessPost(false);

                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset');
        }
    }).always(function () {
        btn.button('reset');
    });
}
function collectDataSpot() {
    spotpreview();
    var spotdata = {
        TotalCost: postCommentViewModel.totalCost(),
        Cost: postCommentViewModel.cost(),
        ImageName: postCommentViewModel.uploadprofilePicObj.imageName,
        PreviewMsg: postCommentViewModel.previewContent(),
        Message: postCommentViewModel.msgContent()
    }
    return spotdata;
}
function postToFB(message, caption) {
    FB.login(function (response) {
        console.log(striphtml(message));
        if (response.status === 'connected') {
            var parms = {
                "link": "https://thinkara.com",
                "description": striphtml(message),
                "caption": caption,
            }

            FB.api('/me/feed', 'post', parms, function (response) {
                if (!response || response.error) {
                    ga('send', 'facebookError', response.error);
                    notifcationGrowl("Could not push your share request",
                      " Facebook Share",
                      "fa fa-share-alt text-danger fa-3x", "", 8);
                } else {
                    ga('send', 'facebookPublished', peopleInfo.UserId());
                    notifcationGrowl("Published on your Wall",
                      " Facebook Share",
                      "fa fa-share-alt text-success fa-3x", "", 4);
                }
            });
        }
    }, { scope: 'publish_actions' });


}