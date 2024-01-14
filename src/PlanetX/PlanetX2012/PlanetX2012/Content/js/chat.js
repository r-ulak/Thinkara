function addTabSection() {
    $('#chattabs').tabs({ closable: true });
    $('#chattabs').tabs(
        { cache: true },
        {
            ajaxOptions: {
                cache: false,
                error: function (xhr, status, index, anchor) {
                    $(anchor.hash).html("Couldn't load this tab.");
                },
                data: {},
                success: function (data, textStatus) { }
            },
            add: function (event, ui) {
                //select the new tab
                $('#chattabs').tabs('select', '#' + ui.panel.id);
            }
        });
}

function addTab(title, uri, userid, message) {
    // If tab already exist in the list, return
    if (($('#friendsList #Image' + userid).attr('class') != 'friendslistThumbnailOffline') || message) {
        if ($('#chattabs').is(":hidden")) {
            $('#chattabs').show();
            addTabSection();
        }
        if ($("#some_" + userid).length != 0)
            return;
        if (message) {
            uri = uri + '&message=' + message;
        }
        uri = uri + '&imageUrl=' + $('#friendsList #Image' + userid).attr('src');
        var newTab = $("#chattabs").tabs("add", uri, title);
        $('li.ui-state-default:last').attr('id', 'some_' + userid);
    }
}

function addMessageafternewTab(message, fromclientid) {
    $('li.ui-state-default:last').attr('id', 'some_' + fromclientid);
    $('#chattabs #messagelist_' + fromclientid).append('<li>' + message + '</li>');
}

function closeTab() {
    var index = getSelectedTabIndex();
    $("#chattabs").tabs("remove", index)
}

function getSelectedTabIndex() {
    return $("#chattabs").tabs('option', 'selected');
}

function RefershList() {
    $('#frmPeopleList').submit();
}

(function () {
    var c = $.ui.tabs.prototype._tabify; $.extend($.ui.tabs.prototype, {
        _tabify: function () {
            var a = this; c.apply(this, arguments); a.options.closable === true && this.lis.filter(function () { return $("span.ui-icon-circle-close", this).length === 0 }).not(":first").each(function () {
                $(this).append('<a href="#"><span class="ui-icon ui-icon-circle-close"></span></a>').find("a:last").hover(function () { $(this).css("cursor", "pointer") }, function () { $(this).css("cursor", "default") }).click(function () {
                    var b = a.lis.index($(this).parent());
                    if (b > -1) { if (false === a._trigger("closableClick", null, a._ui($(a.lis[b]).find("a")[0], a.panels[b]))) return; a.remove(b) } return false
                }).end()
            })
        }
    })
})(jQuery);
var proxyHub = $.connection.signalRHub;
$(function () {
    proxyHub.client.send = function (fromclientid, message) {
        //open new tab if tab is not open for reciving user
        if ($('#chattabs #messagelist_' + fromclientid).length) {
            $('#chattabs #messagelist_' + fromclientid).append('<li>' + message + '</li>');
        }
        else {

            var tabParms = $('#friendsList #Image' + fromclientid).attr('onClick').replace(/'/g, '').split(',');

            addTab($.trim(tabParms[0].replace('addTab(', '')), $.trim(tabParms[1]), $.trim(tabParms[2].replace(')', '')), message);
        }

        $("#chatContainer_" + fromclientid).scrollTop($("#chatContainer_" + fromclientid)[0].scrollHeight);
        $("#chatContainer_" + fromclientid).mCustomScrollbar("update"); //update scrollbar according to newly loaded content
        $("#chatContainer_" + fromclientid).mCustomScrollbar("scrollTo", "last", { scrollInertia: 2500, scrollEasing: Power3.easeInOut }); //scroll to appended content

    };
    
    $('#chatText').typing({
        start: function () { notifyUserTyping(true); },
        stop: function () { notifyUserTyping(false); },
        delay: 5000
    });
    function notifyUserTyping(typingStatus) {

        var sendTo = $('#chattabs .ui-state-active').attr('id');
        sendTo = sendTo.replace('some_', '');
        proxyHub.server.sendUserTyping(sendTo, typingStatus);

    };

    // Set the name
    $.connection.hub.qs = "clientid=" + clientid;
    $("#chatText").keyup(function (event) {
        if (event.keyCode == 13) {
            $("#send").click();

        }
    });

    $.connection.hub.start().done(function () {
        document.title = $.connection.hub.id;
        $('#send').click(function () {
            var sendTo = $('#chattabs .ui-state-active').attr('id');
            sendTo = sendTo.replace('some_', '');
            proxyHub.server.send(sendTo, $('#chatText').val(), fromUser);
            proxyHub.server.sendUserTyping(sendTo, false);
            $('#chatText').val('');
        });
    });
    
    proxyHub.client.friendConnected = function (friendUserId) {

        $('#friendsList #Image' + friendUserId).attr('class', 'friendslistThumbnailOnline')
    };
    
    proxyHub.client.friendDisconnected = function (friendUserId) {

        $('#friendsList #Image' + friendUserId).attr('class', 'friendslistThumbnailOffline')
    };

    proxyHub.client.friendStatusChange = function (friendUserId, userStatus) {
        setFriendStatus(friendUserId, userStatus);
    };

    proxyHub.client.sendUserTyping = function (fromclientid, isTyping) {

        if (isTyping) {
            $('#chatContainerright_' + fromclientid + ' span').visible();
        }
        else {
            $('#chatContainerright_' + fromclientid + ' span').invisible();
        }
    };

    function sendUserStatus(userStatus) {
        proxyHub.server.userStatusChange(userStatus);
    };

    function setFriendStatus(friendUserId, userStatus) {
        var className = '';
        switch (userStatus) {
            case 0:
                className = "friendslistThumbnailOffline";
                break;
            case 1:
                className = "friendslistThumbnailOnline";
                break;
            case 2:
                className = "friendslistThumbnailAway";
                break;
            case 3:
                className = "friendslistThumbnailBusy";
                break;
            default:
                break;

        }
        $('#friendsList #Image' + friendUserId).attr('class', className)
    };

    function setUserStatus(userStatus) {
        var className = '';
        switch (userStatus) {
            case 0:
                className = "userThumbnailOffline";
                break;
            case 1:
                className = "userThumbnailOnline";
                break;
            case 2:
                className = "userThumbnailAway";
                break;
            case 3:
                className = "userThumbnailBusy";
                break;
            default:
                break;

        }
        $('#userProfileImage').attr('class', className)
    };
    
    $.idleTimer(10000);
    $(document).bind("idle.idleTimer", function () {
        if ($('#userProfileImage').attr('data-userSetStatus') == 'false') {
            sendUserStatus(2);
            setUserStatus(2);
        }
    });
    $(document).bind("active.idleTimer", function () {
        if ($('#userProfileImage').attr('data-userSetStatus') == 'false') {
            sendUserStatus(1);
            setUserStatus(1);
        }
    });

    $('#sendUserOnline').click(function () {
        closePopoverUserStatus();
        sendUserStatus(1);
        setUserStatus(1);
        $('#userProfileImage').attr('data-userSetStatus', 'false')
    });

    $('#sendUserAway').click(function () {
        closePopoverUserStatus();
        sendUserStatus(2);
        setUserStatus(2);
        $('#userProfileImage').attr('data-userSetStatus', 'true')
    });

    $('#sendUserBusy').click(function () {
        closePopoverUserStatus();
        sendUserStatus(3);
        setUserStatus(3);
        $('#userProfileImage').attr('data-userSetStatus', 'true')
    });

    $('#sendUserOffline').click(function () {
        closePopoverUserStatus();
        sendUserStatus(0);
        setUserStatus(0);
        $('#userProfileImage').attr('data-userSetStatus', 'true')
    });
    
    function closePopoverUserStatus() {
        var container = $('#personPopupContainer');
        container.slideUp('fast', function () { container.css('display', 'none'); });
    }
});

(function ($) {
    $.fn.invisible = function () {
        return this.each(function () {
            $(this).css("visibility", "hidden");
        });
    };
    $.fn.visible = function () {
        return this.each(function () {
            $(this).css("visibility", "visible");
        });
    };
}(jQuery));

