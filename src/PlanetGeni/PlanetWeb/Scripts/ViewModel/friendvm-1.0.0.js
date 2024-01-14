var friendViewModel = {
    friendDetailsDTO: ko.observableArray(),
    conatctsProviderDTO: ko.observableArray(),
    emailInviteeIdList: ko.observableArray([]),
    filteredsearchlist: ko.observableArray([]),
    suggestlist: ko.observableArray([]),
    lastSuggestionUserId: ko.observableArray(0),
    lastsearchterm: '',
    searchuserList: ko.observableArray([]),
    contentLoademailTriggered: false,
    contactsloginView: '',
    contentLoadcontactsproviderTriggered: false,
    lastEmailInviteeId: ko.observable(''),
    previewContent: ko.observable(''),
    fullName: ko.observable(),
    picture: ko.observable(),
    emailInviteeId: ko.observableArray(),
    clickpartyinvitefriends: function (data, event) {
        var self = $(event.currentTarget);
        var inputcheck = self.children('input');
        var imgcheck = self.children('.img-check');
        var index = parseInt(self.closest('[data-class="clickPartyInvite"]').data('index'));

        if (inputcheck.prop('checked') == true) {
            inputcheck.prop('checked', false);
            imgcheck.css('opacity', '0.2');
            mypartyViewModel.partyCodes()[index].InviteeId.pop(inputcheck.val());

        } else {
            inputcheck.prop('checked', true);
            imgcheck.css('opacity', '1');
            mypartyViewModel.partyCodes()[index].InviteeId.push(inputcheck.val());
        }
        if (mypartyViewModel.partyCodes()[index].InviteeId().length !=
                friendViewModel.friendDetailsDTO().length) {
            var $checkbox = $("#partyinvitefriendsall");
            $checkbox.prop('checked', false);
            $button = $checkbox.siblings('button');
            updateDisplay($checkbox, $button);
        }
    },
    clickgiftfriends: function (data, event) {
        var self = $(event.currentTarget);
        var inputcheck = self.children('input');
        var imgcheck = self.children('.img-check');

        if (inputcheck.prop('checked') == true) {
            inputcheck.prop('checked', false);
            imgcheck.css('opacity', '0.2');
            gitfCartViewModel.ToId.pop(inputcheck.val());

        } else {
            inputcheck.prop('checked', true);
            imgcheck.css('opacity', '1');
            gitfCartViewModel.ToId.push(inputcheck.val());
        }
        if (gitfCartViewModel.ToId().length !=
            friendViewModel.friendDetailsDTO().length) {
            var $checkbox = $("#giftfriendall");
            $checkbox.prop('checked', false);
            $button = $checkbox.siblings('button');
            updateDisplay($checkbox, $button);
        }
        updategitfCartViewModel();
    },
    clickgiftmerchandisefriends: function (data, event) {
        var self = $(event.currentTarget);
        var inputcheck = self.children('input');
        var imgcheck = self.children('.img-check');

        if (inputcheck.prop('checked') == true) {
            inputcheck.prop('checked', false);
            imgcheck.css('opacity', '0.2');
            merchandiseGiftViewModel.ToId.pop(inputcheck.val());

        } else {
            inputcheck.prop('checked', true);
            imgcheck.css('opacity', '1');
            merchandiseGiftViewModel.ToId.push(inputcheck.val());
        }
        if (merchandiseGiftViewModel.ToId().length !=
            friendViewModel.friendDetailsDTO().length) {
            var $checkbox = $("#giftfriendmerchandiseall");
            $checkbox.prop('checked', false);
            $button = $checkbox.siblings('button');
            updateDisplay($checkbox, $button);
        }
        updatemerchandiseGiftViewModel();
    },
    clickfoundersinvite: function (data, event) {
        var self = $(event.currentTarget);
        var inputcheck = self.children('input');
        var imgcheck = self.children('.img-check');
        if (inputcheck.prop('checked') == true) {
            inputcheck.prop('checked', false);
            imgcheck.css('opacity', '0.2');
            startPartyViewModel.founderInviteSelected.pop(inputcheck.val());
        } else {
            inputcheck.prop('checked', true);
            imgcheck.css('opacity', '1');
            startPartyViewModel.founderInviteSelected.push(inputcheck.val());
        }
        if (startPartyViewModel.founderInviteSelected().length !=
             friendViewModel.friendDetailsDTO().length) {
            var $checkbox = $("#partyinvitefriendall");
            $checkbox.prop('checked', false);
            $button = $checkbox.siblings('button');
            updateDisplay($checkbox, $button);
        }
    }
};
$(document).ready(function () {
    ko.applyBindings(friendViewModel, $("#sidebar")[0]);

});
function initializeInviteNavigation() {
    $('#invite-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#invite-friends") {

        }
        else if (self.attr('href') == "#find-friend") {
            if (friendViewModel.contentLoadcontactsproviderTriggered == false
                ) {
                showLoadingImage($("#find-friend"))
                friendViewModel.contentLoadcontactsproviderTriggered = true;
                $.when(getcontactsproviderview(), getcontactsource())
                       .done(function (a1, a2) {
                           friendViewModel.contactsloginView = a1;
                           loadContactSource(a2[0])
                           $('#find-friend').html(friendViewModel.contactsloginView[0]);
                           setupImportContact();
                           friendViewModel.contentLoadcontactsproviderTriggered = false;
                           hideLoadingImage($("#find-friend"));
                           getfriendsuggestion();
                       });
            }
        }
    });
}
function getDataFriend() {
    return $.ajax({
        url: "/api/friendservice/getfriendsdtos",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getFriendById(id) {
    return ko.utils.arrayFirst(friendViewModel.friendDetailsDTO(),
        function (item) {
            return item.FriendUserId == id;
        });
};
function inviteusingEmail() {
    var lastEmailid = friendViewModel.lastEmailInviteeId();
    if (friendViewModel.contentLoademailTriggered == false) {
        friendViewModel.contentLoademailTriggered = true;
        getEmailInvitationList(lastEmailid).success(function (data) {
            friendViewModel.contentLoademailTriggered = false;
            populatepartyemailInviteListvm(data, friendViewModel);
            resetinviteEmailAll(friendViewModel, $("#friendinvitewebcontactall"));
        });
    }
}
function clickinviteemailcontact(data, event) {
    vm = friendViewModel.emailInviteeId;
    vmRoot = friendViewModel;
    emailinviteupdatecheckbox(vm, event);
    resetinviteEmailAll(vmRoot, $("#friendinvitewebcontactall"));
}
function getEmailInvitationList(lastEmailid) {
    return $.ajax({
        url: "/api/partyservice/getemailinvitationlist",
        type: "get",
        contentType: "application/json",
        data: { lastEmailId: lastEmailid },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function populatepartyemailInviteListvm(list, vm) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            vm.emailInviteeIdList.push(list[i]);
        }
        vm.lastEmailInviteeId(list[list.length - 1].FriendEmailId);
    }
}
function resetinviteEmailAll(vm, elemId) {
    if (vm.emailInviteeId().length !=
 vm.emailInviteeIdList().length) {
        var $checkbox = elemId;
        $checkbox.prop('checked', false);
        $button = $checkbox.siblings('button');
        updateDisplay($checkbox, $button);
    }
}
function emailinviteupdatecheckbox(vm, event) {
    var self = $(event.currentTarget);
    var inputcheck = self.children('input');
    var imgcheck = self.children('.img-check');
    if (inputcheck.prop('checked') == true) {
        inputcheck.prop('checked', false);
        imgcheck.css('opacity', '0.2');
        vm.pop(inputcheck.val());
    } else {
        inputcheck.prop('checked', true);
        imgcheck.css('opacity', '1');
        vm.push(inputcheck.val());
    }
}

function initializeInvite() {
    initializeInviteNavigation();
    friendViewModel.emailInviteeId.removeAll();
    friendViewModel.searchuserList.removeAll();
    friendViewModel.filteredsearchlist.removeAll();
    friendViewModel.lastsearchterm = '';
    $('#friend-term').val('');
    friendViewModel.conatctsProviderDTO.removeAll();
    for (var i = 0; i < codeTables.ContactProvider().length; i++) {
        var contactObj = {
            ImageFont: codeTables.ContactProvider()[i].ImageFont(),
            ProviderName: codeTables.ContactProvider()[i].ProviderName(),
            ProviderId: codeTables.ContactProvider()[i].ProviderId(),
            Total: ko.observable(0),
            UpdatedAt: ko.observable('')
        }
        friendViewModel.conatctsProviderDTO.push(contactObj);
    }
    setupInviteView();
}
function setupInviteView() {
    ko.applyBindings(peopleInfo, $("#inviteview-box .panel-heading").get(0));
    ko.applyBindings(friendViewModel, $("#emailcontacts").get(0));
    ko.applyBindings(peopleInfo, $("#facebookinvite").get(0));
    ko.applyBindings(friendViewModel, $("#sendInvite").get(0));
    setupcheckboxinvitecontact();
    $("#webContactshowmore").on('click', function () {
        inviteusingEmail();
    });
    $("#addemail").on('click', function () {
        addnewEmailContact($(this).parent('.input-group-btn').siblings('input').val());
        $('#emailcontacts .invitemyfriends').scrollTop(1000000);
        $(this).parent('.input-group-btn').siblings('input').val('');
    });
    $("#sendInvite").on('click', function () {
        sendFriendInvite($(this));
    });
}
function addnewEmailContact(emails) {
    emails = emails.split(",");
    console.log(emails);
    for (var i = 0; i < emails.length; i++) {

        if (validateEmail(emails[i].trim())) {
            var match = ko.utils.arrayFirst(friendViewModel.emailInviteeIdList(), function (item) {
                return emails[i].toLowerCase().trim() === item.FriendEmailId.toLowerCase().trim();
            });
            if (!match) {
                var emailobj = {
                    FriendEmailId: emails[i].trim().toLowerCase(),
                    FullName: "N/A",
                    NewEmail: true
                };
                console.log(emailobj);
                friendViewModel.emailInviteeIdList.push(emailobj);
            }
        }
    }
}
function setupcheckboxinvitecontact() {
    var $widget = $("#emailcontacts .button-checkbox"),
    $button = $widget.find('button'),
    $checkbox = $widget.find('input:checkbox');
    $checkbox.on('change', function () {
        updateDisplay($checkbox, $button);
        updateSelection(friendViewModel.emailInviteeId, $checkbox);
    });
    initilaizeUpdatedCheckbox($checkbox, $button);
    $("#emailcontacts .invitemyfriends").perfectScrollbar(
         {
             suppressScrollX: true, wheelPropagation: true, wheelSpeed: 2, minScrollbarLength: 10, alwaysVisibleY: true
         });
}
function collectsendInviteData() {
    var postEmailList = [];
    for (var i = 0; i < friendViewModel.emailInviteeId().length; i++) {
        var match = ko.utils.arrayFirst(friendViewModel.emailInviteeIdList(), function (item) {
            return friendViewModel.emailInviteeId()[i].toLowerCase() === item.FriendEmailId.toLowerCase()
                && item.NewEmail == true;
        });
        var isNewEmail = false;
        if (match) {
            isNewEmail = true;
        }
        var emailObj = {
            FriendEmailId: friendViewModel.emailInviteeId()[i],
            NewEmail: isNewEmail
        }
        postEmailList.push(emailObj);
    }
    var sendInvite = {
        Message: friendViewModel.previewContent(),
        InvitationList: postEmailList
    }

    return sendInvite;
}
function sendFriendInvite(btn) {
    if (friendViewModel.emailInviteeId().length == 0 && friendViewModel.previewContent().length > 1000) {
        return false;
    }
    $.ajax({
        url: "/api/FriendService/SendFriendInvite",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(collectsendInviteData()),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var resultinvite = ko.mapping.fromJS(result);
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#inviteview-box").html("");
                $("#inviteview-box").addClass("hidden");
                $("#inviteview-submit").removeClass("hidden");
                $("#inviteview-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(resultinvite, $("#inviteview-submit").get(0));
                $("#main").animate({ scrollTop: $("#inviteviewcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')

        }
    }).always(function () {

        btn.button('reset')
    });
}


function setupImportContact() {
    ko.applyBindings(peopleInfo, $("#contactview-box .panel-heading").get(0));
    ko.applyBindings(friendViewModel, $("#contactview-box .panel-body").get(0));
    $("#contatcrefresh").on('click', function () {
        getcontactsource().done(function (data) {
            loadContactSource(data)
        });
    });
    $("#firendsearchprofile, #firendsuggestprofile").perfectScrollbar({
        suppressScrollX: true, wheelPropagation: true,
        wheelSpeed: 2, minScrollbarLength: 10
    });
    $('#searchfriends').on('click', function () {
        friendViewModel.searchuserList.removeAll();
        searchfriend();
    });
    $('#friend-term').keyup(function (e) {
        if (e.keyCode == 13) {
            friendViewModel.searchuserList.removeAll();
            searchfriend();
        }
    });
    $('#searchshowmore').on('click', function () {
        $('#searchshowmore').addClass('hidden');
        if (friendViewModel.lastsearchterm != $('#friend-term').val()) {
            friendViewModel.searchuserList.removeAll();
        }
        searchfriend();
    });
    $('#suggestshowmore').on('click', function () {
        $('#suggestshowmore').addClass('hidden');
        getfriendsuggestion();
    });

}
function getcontactsproviderview() {
    if (friendViewModel.contactsloginView != "") {
        return friendViewModel.contactsloginView;
    }
    return $.ajax("/Friend/ImportContacts");
}
function getcontactsource() {
    return $.ajax({
        url: "/api/friendservice/getcontactsource",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function Ignorefriendsuggestion(userId) {
    $.ajax({
        url: "/api/friendservice/ignoresuggestion",
        type: "POST",
        data: JSON.stringify({
            SuggestionUserId: userId
        }),
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var match = ko.utils.arrayFirst(friendViewModel.suggestlist(), function (item) {
                return userId === item.UserId;
            });
            if (match) {
                friendViewModel.suggestlist.remove(match);
            }
        }
    });
}
function getfriendsuggestion() {
    $.ajax({
        url: "/api/friendservice/getfriendsuggestion",
        type: "POST",
        data: JSON.stringify({
            SuggestionUserId: friendViewModel.lastSuggestionUserId()
        }),
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    friendViewModel.suggestlist.push(result[i]);
                }
                friendViewModel.lastSuggestionUserId(result[result.length - 1].UserId);
                $('#suggestshowmore').removeClass('hidden');
            }
        }
    });
}
function loadContactSource(result) {
    for (var i = 0; i < result.length; i++) {
        friendViewModel.conatctsProviderDTO()[result[i].ProviderId - 1].Total(result[i].Total);
        friendViewModel.conatctsProviderDTO()[result[i].ProviderId - 1].UpdatedAt(result[i].UpdatedAt);
    }
}
function searchfriend() {
    if ($('#friend-term').val().trim().length > 2) {
        $('#searchfriends').button('loading');
        friendViewModel.lastsearchterm = $('#friend-term').val();
        $.ajax({
            url: "/api/webuserservice/autocompleteusersearch",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({

                QueryString: $('#friend-term').val(),
                Skip: friendViewModel.searchuserList().length,
            }),
            cache: true,
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                if (friendViewModel.searchuserList().length > 0) {
                    processDuplicatesfriend(result);
                }
                else {
                    for (i = 0; i < result.length; i++) {
                        result[i].FullName = result[i].NameFirst + ' ' + result[i].NameLast
                        friendViewModel.searchuserList.push(result[i]);
                    }
                }
                processSearchList();
            }
        }).always(function () {
            $('#searchfriends').button('reset')
        });
    }
}
function processDuplicatesfriend(result) {
    for (i = 0; i < result.length; i++) {
        var match = ko.utils.arrayFirst(friendViewModel.searchuserList(), function (item) {
            return result[i].UserId === item.UserId;
        });
        if (!match) {
            friendViewModel.searchuserList.push(result[i]);
        }
    }
}
function processSearchList() {
    var oldlength = friendViewModel.filteredsearchlist().length;

    friendViewModel.filteredsearchlist(
     ko.utils.arrayFilter(friendViewModel.searchuserList(), function (item) {
         if (item.UserId == peopleInfo.UserId()) {
             console.log(item);
             return false;
         }
         for (i = 0; i < friendViewModel.friendDetailsDTO().length; i++) {
             if (friendViewModel.friendDetailsDTO()[i].FriendUserId == item.UserId) {
                 if (friendViewModel.friendDetailsDTO()[i].RelationDirection == 'FD') {
                     return true;
                 }
                 else {
                     console.log("userID {0}  fromfreind {1} RelationDirection {2}",
                            item.UserId,
                            friendViewModel.friendDetailsDTO()[i].FriendUserId,
                            friendViewModel.friendDetailsDTO()[i].RelationDirection
                            );
                     return false;
                 }
             }
         }
         return true;
     }));

    if (oldlength < friendViewModel.filteredsearchlist().length) {
        $('#searchshowmore').removeClass('hidden');
    }

}

function followsearchfriend(data, event) {
    var btn = $(event.currentTarget);
    var userId = btn.data('userid')
    var profileData = {
        ActionType: 'F',
        FriendId: userId
    };
    updateFriendRelation(btn, profileData);
    var match = ko.utils.arrayFirst(friendViewModel.suggestlist(), function (item) {
        return userId === item.UserId;
    });
    if (match) {
        friendViewModel.suggestlist.remove(match);
    }
    match = ko.utils.arrayFirst(friendViewModel.filteredsearchlist(), function (item) {
        return userId === item.UserId;
    });
    if (match) {
        friendViewModel.filteredsearchlist.remove(match);
    }
}
function followallsearchfriend(data, event) {
    var btn = $(event.currentTarget);
    var profileData = {
        FriendId: ko.utils.arrayMap(ko.toJS(friendViewModel.filteredsearchlist), function (item) {
            return item.UserId;
        })
    };
    follwoAllFriend(btn, profileData);
    friendViewModel.filteredsearchlist.removeAll();

}
function followallsuggestfriend(data, event) {
    var btn = $(event.currentTarget);
    var profileData = {
        FriendId: ko.utils.arrayMap(ko.toJS(friendViewModel.suggestlist), function (item) {
            return item.UserId;
        })
    };
    follwoAllFriend(btn, profileData);
    friendViewModel.suggestlist.removeAll();
}
function follwoAllFriend(btn, profileData) {
    btn.button('loading');
    $.ajax({
        url: "/api/FriendService/FollwoAllFriend",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(profileData),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                setTimeout(updateFriends
                , pollFequency * pollFactor);
                friendViewModel.searchuserList.removeAll();
                friendViewModel.filteredsearchlist.removeAll();
                friendViewModel.lastsearchterm = '';
                $('#friend-term').val('');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')

        }
    }).always(function () {

        btn.button('reset')
    });
}