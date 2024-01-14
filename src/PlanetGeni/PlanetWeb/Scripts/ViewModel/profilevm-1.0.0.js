var profileviewModel;
function initializeProfile(profileid) {
    initializeProfileView(profileid);
    setupProfileView();
}
function initializeProfileView(profileid) {
    profileviewModel = {
        contentprofileLoadTriggered: false,
        contentprofilestatLoadTriggered: false,
        awards: ko.observableArray([]),
        partyMembership: ko.observableArray([]),
        partyMembershiptriggred: false,
        jobs: ko.observableArray([]),
        jobstriggred: false,
        property: ko.observableArray([]),
        propertytriggred: false,
        education: ko.observableArray([]),
        educationtriggred: false,
        friends: ko.observableArray([]),
        friendstriggred: false,
        profileInfo: ko.observable(),
        peopleInfo: ko.observable(),
        profileId: ko.observable(profileid),
        friendRelationResult: ko.observableArray([]),
        afterProfileRenderAction: function (elem) {
            $(elem).find('div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            });
        },
    };
}
function setupProfileView() {
    ko.applyBindings(peopleInfo, $("#profileview-box .panel-heading").get(0));
    $("#profileAccordion .panel-heading").each(function () {
        ko.applyBindings(profileviewModel, this);
    });

    if (profileviewModel.profileId() == peopleInfo.UserId()) {
        ko.applyBindings(peopleInfo, $("#profilenetworth").get(0));
        ko.applyBindings(peopleInfo, $("#profilename").get(0));
        ko.applyBindings(peopleInfo, $("#profileuserLevel").get(0));
        ko.applyBindings(peopleInfo, $("#profilerelation").get(0));
    }
    else {
        getProfileInfo();
    }
    getProfileStat();
    $("#awardsprofile, #educationprofile,#partyprofile, #jobprofile, #propertyprofile, #firendsprofile").perfectScrollbar(
       {
           suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
       });

    ko.applyBindings(profileviewModel, $("#partyprofile").get(0));
    ko.applyBindings(profileviewModel, $("#jobprofile").get(0));
    ko.applyBindings(profileviewModel, $("#educationprofile").get(0));
    ko.applyBindings(profileviewModel, $("#propertyprofile").get(0));
    ko.applyBindings(profileviewModel, $("#firendsprofile").get(0));
    ko.applyBindings(profileviewModel, $("#friendrelation").get(0));


    $('.accordionprofile').on('shown.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').addClass('active');
        //$(e.target).animate({ height: "350" }, 600);
        var depId = $(e.target).attr('id').replace('collapse', '');
        switch (depId) {
            case 'awardsprofile':
                avatar = "fa-thumbs-up";
                break;
            case 'partyprofile':
                if (profileviewModel.partyMembershiptriggred == false) {
                    getProfileparty();
                }
                break;
            case 'jobprofile':
                if (profileviewModel.jobstriggred == false) {
                    getProfilejob();
                }
                break;
            case 'propertyprofile':
                if (profileviewModel.propertytriggred == false) {
                    getProfileproperty();
                }
                break;
            case 'educationprofile':
                if (profileviewModel.educationtriggred == false) {
                    getProfileeducation();
                }
                break;
            case 'firendsprofile':
                if (profileviewModel.friendstriggred == false) {
                    getProfilefriends();
                }
                break;

        }
    });

    $('.accordionprofile').on('hidden.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').removeClass('active');
    });
}
function getProfileInfo() {
    if (profileviewModel.contentprofileLoadTriggered == true) {
        return;
    }
    profileviewModel.contentprofileLoadTriggered = true;
    $.ajax({
        url: "/api/WebUserService/GetWebUserInfo",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: profileviewModel.profileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {

            profileviewModel.peopleInfo = ko.mapping.fromJS(result);
            profileviewModel.peopleInfo.ProfileId = ko.observable(profileviewModel.profileId());
            profileviewModel.peopleInfo.RelationDirection = ko.observable('');

            var match = ko.utils.arrayFirst(friendViewModel.friendDetailsDTO(), function (item) {
                return profileviewModel.profileId() === item.FriendUserId;
            });
            if (match) {
                profileviewModel.peopleInfo.RelationDirection(match.RelationDirection);
            }

            profileviewModel.peopleInfo.UserId = ko.observable(peopleInfo.UserId());
            ko.applyBindings(profileviewModel.peopleInfo, $("#profilenetworth").get(0));
            ko.applyBindings(profileviewModel.peopleInfo, $("#profilerelation").get(0));
            ko.applyBindings(profileviewModel.peopleInfo, $("#profilename").get(0));
            ko.applyBindings(profileviewModel.peopleInfo, $("#profileuserLevel").get(0));
        }
    }).always(function () {
        profileviewModel.contentprofileLoadTriggered = false;
    });
}
function getProfileStat() {
    if (profileviewModel.contentprofilestatLoadTriggered == true) {
        return;
    }
    profileviewModel.contentprofilestatLoadTriggered = true;
    $.ajax({
        url: "/api/WebUserService/GetProfileStat",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: profileviewModel.profileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#profileview-box .panel-body").eq(0)),
        success: function (result) {
            $("#profileitemHeader, #profileAccordion").removeClass("hidden");
            profileviewModel.profileInfo = ko.mapping.fromJS(result);
            ko.applyBindings(profileviewModel.profileInfo, $("#profilestat").get(0));
        }
    }).always(function () {
        hideLoadingImage($("#profileview-box .panel-body").eq(0));
        profileviewModel.contentprofilestatLoadTriggered = false;
    });
}
function getProfileparty() {
    $.ajax({
        url: "/api/partyService/GetAllUserParty",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: profileviewModel.profileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#partyprofile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                profileviewModel.partyMembership.push(result[i]);
            }
            profileviewModel.partyMembershiptriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#partyprofile .panel-body").eq(0));
    });
}
function getProfilejob() {
    $.ajax({
        url: "/api/jobService/GetJobProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: profileviewModel.profileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#jobprofile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                profileviewModel.jobs.push(result[i]);
            }
            profileviewModel.jobstriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#jobprofile .panel-body").eq(0));
    });
}
function getProfileproperty() {
    $.ajax({
        url: "/api/MerchandiseService/GetMerchandiseProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: profileviewModel.profileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#propertyprofile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                addpropertyItemCss(result[i]);
                profileviewModel.property.push(result[i]);
                addpropertycondtionKnob($("#propertyprofile input[data-index=" + i + "]"));
            }
            profileviewModel.propertytriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#propertyprofile .panel-body").eq(0));
    });
}
function getProfileeducation() {
    $.ajax({
        url: "/api/EducationService/GetEducationProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: profileviewModel.profileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#educationprofile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                addeducationItemCss(result[i]);
                profileviewModel.education.push(result[i]);
            }
            profileviewModel.educationtriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#educationprofile .panel-body").eq(0));
    });
}
function getProfilefriends() {
    $.ajax({
        url: "/api/FriendService/GetFriendsProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: profileviewModel.profileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#firendsprofile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                profileviewModel.friends.push(result[i]);
            }
            profileviewModel.friendstriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#firendsprofile .panel-body").eq(0));
    });
}


function addeducationItemCss(item) {
    item.CertificateTextFont = ko.computed(function () {
        return certificateFonttext(item.DegreeId);
    });
    item.DegreeTextFont = ko.computed(function () {
        return degreeFonttext(item.DegreeId);
    });
}
function followfriend(data, event) {
    var btn = $(event.currentTarget);
    var profileData = {
        ActionType: 'F',
        FriendId: profileviewModel.profileId()
    };
    updateFriendRelation(btn, profileData);
}
function unfollowfriend(data, event) {
    var btn = $(event.currentTarget);
    var profileData = {
        ActionType: 'U',
        FriendId: profileviewModel.profileId()
    };
    updateFriendRelation(btn, profileData);
}
function blockfriend(data, event) {
    var btn = $(event.currentTarget);
    var profileData = {
        ActionType: 'B',
        FriendId: profileviewModel.profileId()
    };
    updateFriendRelation(btn, profileData);
}