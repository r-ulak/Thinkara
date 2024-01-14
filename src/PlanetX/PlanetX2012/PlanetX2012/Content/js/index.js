var posturlContent = [];
var chaturlContent = [];
$(function () {
    var lastPostId = 0;
    $.ajax({
        async: false,
        type: 'POST',
        url: '../Post/GetPostsForUser',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            postid: -1
        }),
        success: function (result, data) {
            $.each(result.Posts, function (i, item) {
                AddPost(item.PostContent, item.PostId, true);
                lastPostId = item.PostId;
            });
            $.each(result.Comments, function (j, postComment) {
                AddChildComment(postComment.PostCommentId,
                    postComment.ParentCommentId, postComment.Comment, postComment.PostId);
            });

            $.each(result.WebContent, function (k, postWebContent) {
                AddWebContent(postWebContent.Content, postWebContent.PostId)
            });


        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
    proxyHub.client.recieveComment = function (fromClientId, postCommentId, parentCommentId, postComment, postId) {
        AddChildComment(postCommentId, parentCommentId, postComment, postId);

    };
    proxyHub.client.recievePost = function (fromClientId, postContent, postId, postWebContent) {

        AddPost(postContent, postId, false);
        AddWebContent(postWebContent, postId);
    };
    proxyHub.client.recieveTrendingTopic = function (trendTopics) {
        var items = [];
        $.each(trendTopics, function (i, item) {
            items.push('<li data-count=' + item.tagCount
                + '><a href="yourlink?topicId=' + item.tagId + '">' + item.tag + '</a></li>');

        });
        $('#trendingTopics ul').append(items.join(''));
    };

    $('#postList').on('click', '.submitCommentReply', function () {

        var parentCommentId = $(this).attr('id').replace("submitCommentReply", "");
        var comment = $("#commentReplyParentId" + parentCommentId).val();
        var postId = $(this).closest('div.post').data("postid");
        if (comment == "")
            return false;
        PostComment(parentCommentId, comment, postId)
    });
    $('#postList').on('click', '.postComment', function () {

        var parentCommentId = 0;
        var postId = $(this).attr('data-postId');
        var comment = $("#commentOnPost" + postId).val();
        if (comment == "")
            return false;
        PostComment(parentCommentId, comment, postId)
    });
    $('#postList').on('click', '.commentReply', function () {

        var postCommentId = $(this).attr('data-commentId');
        AddCommentReplyDiv(postCommentId);

    });
    $('#postFeed').on('click', '.collapsible_title', function () {

        $(this).next('.collapsible_box').toggle();


    });

    var tagThis = $("#positiveTags");
    var itemId;
    tagThis.tagit({
        allowSpaces: true,
        minLength: 2,
        removeConfirmation: true,
        requireAutocomplete: true,
        placeholderText: "Share with...",
        autocomplete: {
            delay: 0, minLength: 2,
            source: function (search, showChoices) {
                $.ajax({
                    url: "../Club/GetClubAndFriendList",
                    data: { term: search.term },
                    dataType: "json",
                    success: function (data) {
                        var assigned = tagThis.tagit("assignedTagsDataId");
                        var filtered = [];

                        for (var i = 0; i < data.length; i++) {
                            if ($.inArray(data[i].id, assigned) == -1) {
                                filtered.push(data[i]);
                            }
                        }

                        showChoices(filtered);


                    }
                    //,
                    //select: function (event, ui) {
                    //    itemId = ui.item.id;
                    //    tagThis.tagit("createTag", ui.item.value);
                    //    return false;
                    //}
                });
            }
        },
        allowDuplicates: true,
    });

    tagThis = $("#negativeTags");
    tagThis.tagit({
        allowSpaces: true,
        minLength: 2,
        removeConfirmation: true,
        requireAutocomplete: true,
        placeholderText: "Don't Share with...",
        autocomplete: {
            delay: 0, minLength: 2,
            source: function (search, showChoices) {
                $.ajax({
                    url: "../Club/GetClubAndFriendList",
                    data: { term: search.term },
                    dataType: "json",
                    success: function (data) {
                        var assigned = tagThis.tagit("assignedTagsDataId");
                        var filtered = [];

                        for (var i = 0; i < data.length; i++) {
                            if ($.inArray(data[i].id, assigned) == -1) {
                                filtered.push(data[i]);
                            }
                        }

                        showChoices(filtered);
                    }
                });
            }
        },
        allowDuplicates: true,

    });
    function AddWebContent(postwebContent, postId) {
        $("div[data-postid=" + postId + "]").append(postwebContent);
    }
    function AddChildComment(postCommentId, parentCommentId, postComment, postId) {
        var comment = '<div class="postComment" style="padding-left:10px"'
                + ' data-commentId=' + postCommentId
                + ' data-parentCommentId=' + parentCommentId
                + '>'
                + postComment
                + '<div>'
                + '<a class="commentReply" data-commentId=' + postCommentId + ' >Reply</a>'

                + '</div>'
                + '</div>';

        if (parentCommentId == 0) {
            $('div[data-postId=' + postId + ']').append(comment);
        }
        else {
            $('div[data-commentId=' + parentCommentId + ']').append(comment);
        }
    }
    function AddCommentReplyDiv(postCommentId) {
        var newTextBoxDiv = $("div[data-commentId=" + postCommentId + "]div");
        var nextbox = '<div id="commentReplyForm' + postCommentId + '"'
                       + '>'
                       + '<input type="text" id="commentReplyParentId' + postCommentId + '">'
                       + '<input type="submit" value="Reply" id="submitCommentReply' + postCommentId + '"'
                       + ' class="demo-button ui-state-default ui-corner-all submitCommentReply" /></div>'
        newTextBoxDiv.append(nextbox);
    }
    function PostComment(parentCommentId, comment, postId) {
        if (typeof comment === "undefined")
            return false;
        var dataString = {
            parentCommentId: parentCommentId,
            comment: comment,
            postId: postId
        };

        $.ajax({
            type: "POST",
            url: "../Post/AddPostCommentForUser",
            data: JSON.stringify(dataString),
            cache: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data, status, xhr) {
                PostCommentAdded(data, status, xhr);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }
    function PostCommentAdded(postData, status, xhr) {
        if (postData.success == "1") {
            if (postData.parentCommentId > 0) {
                $("#commentReplyForm" + postData.parentCommentId).remove();
            }
            else {
                $("#commentOnPost" + postData.postId).val('');
            }
            proxyHub.server.broadCastComment(postData.postCommentId,
                postData.parentCommentId, postData.postComment, postData.postId);

        }
        else {

        }


    }
    function AddPost(postContent, postId, append) {
        var post = '<div class="post" '
              + ' data-postId=' + postId
              + '>'
              + postContent
              + '</div>'
              + '<div>'
              + '<input type="text" id="commentOnPost' + postId + '" placeholder="Post Your Comment..."/>'
              + ' <input type="submit" value="Post"  data-postId =' + postId + ' class="demo-button ui-state-default ui-corner-all postComment" />'
              + '</div>'

        ;
        if (append == true) {
            $("#postList").append(post);
        }
        else {
            $("#postList").prepend(post);
        }

    }
    $("input#submitPost").click(function (e) {
        var postContent = $("textarea#PostContent").val();
        if (postContent == "")
            return false;
        submitPostForm();
        e.preventDefault();
    });
    $(document).scroll(function () {
        if ($(window).scrollTop() + $(window).height() == $(document).height()) {
            $("#loadingmessage").show();
            $.ajax({
                async: false,
                type: 'POST',
                url: '../Post/GetPostsForUser',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    postid: lastPostId
                }),
                success: function (result, data) {
                    $("#loadingmessage").hide();
                    $.each(result.Posts, function (i, item) {
                        AddPost(item.PostContent, item.PostId, true);
                        lastPostId = item.PostId;
                    });
                    $.each(result.Comments, function (j, postComment) {
                        AddChildComment(postComment.PostCommentId,
                            postComment.ParentCommentId, postComment.Comment, postComment.PostId);
                    });

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $("#loadingmessage").hide();
                }
            });
        }
    });

    $('#PostContent').typing({
        start: function (e) {

            var code = (e.keyCode ? e.keyCode : e.which);
            console.log(code);
            if (code == 32) {
                BuildWebContent($('#PostContent').val(), 'Post', false);
            }

        },
        stop: function (e) {

            var code = (e.keyCode ? e.keyCode : e.which);
            console.log(code);
            if (code == 8 || code == 46) {
                CleanUpUrls($('#PostContent').val(), 'Post');
            }

        },
        delay: 100
    });

    var tid = setInterval(RequestTrendingTopics, 15000);

});

function PostAdded(postData, status, xhr) {
    if (postData.success == "1") {

        proxyHub.server.broadCastPost(postData.PostContent, postData.PostId, postData.WebContent);
        $("textarea#PostContent").val('');
        $("#posturlconetnt").html('');
        $("#positiveTags").tagit("removeAll");
        $("#negativeTags").tagit("removeAll");
    }
    else {
        $("#errSummaryPost").html(postData.ex);
        $("#errSummaryPost").show();
    }


}

function submitPostForm() {
    console.log("submitPostForm Executing");
    BuildWebContent($('#PostContent').val(), 'Post', true, function () {
        console.log("Submit in progress");
        var dataString = {
            PostPositiveACLList: $("#positiveTags").tagit("assignedTagsDataIdandType"),
            PostNegativeACLList: $("#negativeTags").tagit("assignedTagsDataIdandType"),
            PostContent: $("textarea#PostContent").val(),
            CommentEnabled: true,
            PostUrlContent: posturlContent
        };

        $.ajax({
            type: "POST",
            url: "../Post/AddPostsForUser",
            data: JSON.stringify(dataString),
            cache: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data, status, xhr) {
                PostAdded(data, status, xhr);
                posturlContent = [];
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    });
}

function findUrls(text) {
    var source = (text || '').toString();
    var urlArray = [];
    var url;
    var matchArray;

    // Regular expression to find FTP, HTTP(S) and email URLs.
    var regexToken = /(((https?):\/\/)[\-\w@:%_\+.~#?,&\/\/=]+)/gi;

    // Iterate through any URLs in the text.
    while ((matchArray = regexToken.exec(source)) !== null) {
        var token = matchArray[0];
        urlArray.push(encodeURI(token)); //.replace("http://", "h");
    }

    return urlArray;
}

function GetWebContent(content, callback) {
    console.log("GetWebContent Executing");
    if (content.length < 1) {
        if (typeof (callback) == "function") {
            console.log("GetWebContent doing callback");
            callback();
        }
        return;
    }
    var Type = "GET";
    var postdata = content.join("[stop]");
    var Url = "http://planetx.com/FetchWebContentService/FetchWebContent.svc/GetUrlContent?urls=" + encodeURIComponent(postdata);
    var ContentType = "application/json; charset=utf-8";
    var DataType = "json";


    console.log("CallService" + posturlContent);
    $.ajax({
        type: Type, //GET or POST or PUT or DELETE verb
        url: Url, // Location of the service        
        contentType: ContentType, // content type sent to server
        dataType: DataType, //Expected data format from server        
        success: function (msg) {//On Successfull service call
            result = msg.GetUrlContentResult;
            for (var i = 0; i < posturlContent.length; i++) {
                var detail = posturlContent[i];
                for (var j = 0; j < result.length; j++) {
                    var item = result[j];
                    if (item.Uri.toLowerCase() == detail.Uri.toLowerCase()) {
                        {

                            posturlContent[i].Title = item.Title;
                            posturlContent[i].Content = item.Content;
                            posturlContent[i].Processed = true;
                            if (item.Content != null)
                                AddWebContenttoShareDiv(item.Content.replace('class="collapsible_content"', 'class="collapsible_content" data-url="' + item.Uri + '"'));
                        }
                    }
                }

                for (var k = 0; k < content.length; k++) {
                    var item = content[k];
                    if (item.toLowerCase() == detail.Uri.toLowerCase()) {
                        {
                            posturlContent[i].Processed = true;

                        }
                    }
                }



            }
            console.log(posturlContent);
            if (typeof (callback) == "function") {
                callback();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });


}

function BuildWebContent(textWithUrl, textSource, isPostSubmit, callback) {

    if ($.grep(posturlContent, function (item) {
        return item.Title != ''
            && item.Processed == true
    }).length >= 3)
        return;


    var uris = findUrls(textWithUrl);
    if (uris.length < 0)
        return;
    CleanUpUrls(textWithUrl, textSource);
    var contentLength = 0;
    if (textSource == 'Post')
        contentLength = posturlContent.length;
    else if (textSource == 'Chat')
        contentLength = 0;
    var sendurls = [];

    if (uris.length > 0) {
        for (var j = 0; j < uris.length; j++) {
            if ($.grep(posturlContent, function (item) { return item.Uri == uris[j] }).length == 0) {
                if (textSource == 'Post')
                    posturlContent.push({ "Title": "", "Content": "", "Uri": uris[j], "Processed": false });
                else if (textSource == 'Chat')
                    posturlContent.push({ "Title": "", "Content": "", "Uri": uris[j], "Processed": false });
            }

        }
        for (var i = 0; i < posturlContent.length; i++) {
            if (posturlContent[i].Processed == false)
                sendurls.push(posturlContent[i].Uri);
        }


        if (isPostSubmit == false) {
            GetWebContent(sendurls);
        }
        else {
            console.log("GetWebContent in Progress");
            GetWebContent(sendurls, function () {
                console.log("GetWebContent done");
                callback();
            });
        }

    } else {

        if (isPostSubmit == true)
            callback();
    }

}

function CleanUpUrls(textWithUrl, textSource) {

    var uris = findUrls(textWithUrl);
    if (uris.length <= 0) {
        var contentLength = 0;
        if (textSource == 'Post')
            posturlContent = [];
        else if (textSource == 'Chat')
            chaturlContent = [];

    }

    $.each(uris, function () {
        var siteUrl = this.valueOf();
        if (textSource == 'Post') {
            posturlContent = $.grep(posturlContent, function (item) {
                if (uris.indexOf(item.Uri) < 0)
                    $('#posturlconetnt [data-url="' + item.Uri + '"]').remove();

                return uris.indexOf(item.Uri) > -1;
            });
        }
        else if (textSource == 'Chat') {
            chaturlContent = $.grep(chaturlContent, function (item) {
                return uris.indexOf(item.Uri) > -1;
            });
        }
    });
    console.log(posturlContent);
}

function AddWebContenttoShareDiv(content) {

    if ($("#posturlconetnt > div").size() < 3) {

        $("#posturlconetnt").append(content);
    }
}

function findAndRemove(array, property, value) {
    $.each(array, function (index, result) {
        if (result[property] == value) {
            //Remove from array
            array.splice(index, 1);
        }
    });
}

function RequestTrendingTopics() {

    $.ajax({
        async: true,
        type: 'GET',
        url: 'http://planetx.com/TrendingTopics/TrendingService.svc/GetTrendingTopics',
        contentType: 'application/json; charset=utf-8',

        success: function (result, data) {            
            var items = [];
            $.each(result, function (i, item) {
                items.push('<li data-count=' + item.value
                    + '><a href="yourlink?topicId=' + item.value + '">' + item.key + '</a></li>');

            });
            $('#trendingTopics ul').html(items.join(''));


        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}

