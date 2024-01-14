var partyinfoViewModel;
var partySearchViewModel;
var partySearchViewModelresult;
var myPartyViewModel;
var myPartyViewModelresult;
var partySummaryViewModel;
var userPartySummaryViewModel;
var startPartyViewModel;
var partySearchView;
var myPartyView;
var myPartyInviteView = "";
var myHistoryPartyView;
var myHistoryPartyViewModel;
var partySummaryView;
var startPartyView;
var managePartyView = "";


function initializeParty() {
    partySearchView = $("#partysearchcontainer").html();
    myPartyView = $("#mypartycontainer").html();
    myHistoryPartyView = $("#myHistorypartycontainer").html();
    partySummaryView = $("#partySummarycontainer").html();
    startPartyView = $("#party-startparty").html();

    initializePartyNavigation();
    initializePartySearch();
    setupPartySearch();
    initializeMyParty();
    setupMyParty();
    initializeMyHistoryParty();
    setupMyHistoryParty();
    initializePartySummary();
    setupPartySummary();
    initializeStartParty();
    setupStartParty();
}
function initializePartyNavigation() {
    $('#party-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#party-summary") {
            if (typeof userPartySummaryViewModel == 'undefined') {
                getPartySummary();
            }
            if (partySummaryViewModel.partyTopN().length == 0) {
                getTopNPartySalary();
            }
        }
        else if (self.attr('href') == "#party-myparty") {
            if (mypartyViewModel.partyCodes().length == 0) {
                getMyParty();
                getUserbankAccount();
            }
        }
        else if (self.attr('href') == "#party-history") {
            if (myHistorypartyViewModel.partyCodes().length == 0) {
                getMyHistoryParty();
            }
        }
        else if (self.attr('href') == "#party-startparty") {
            if (startPartyViewModel.emailInviteeIdList().length == 0) {
                startpartyEmailInvitaionList();
            }
        }
        //startPartyViewModel.emailInviteeIdList
    })
}

function initializePartySearch() {
    $("#partysearchcontainer").html(partySearchView);
    partySearchViewModel = {
        partyCodes: ko.observableArray(),
        lastStartDate: ko.observable(0),
        applyingItems: ko.observableArray(),
        contentLoadTriggered: false,
        clickgetagenda: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            var vm = partySearchViewModel.partyCodes()[index];

            if (vm.PoliticalPartyAgenda().length <= 0) {
                getpartyAgenda(vm.PartyId, vm).success(function (data) {
                    processpartyAgenda(data, vm);
                })
            }

        },
        clickgetAllMembers: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getAllpartyMemberByPage(index, partySearchViewModel);
        },
        clickgetmoreMembers: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getMoreMembers(index, partySearchViewModel);
        },
        clickgetmoreCoFounders: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getMoreCoFounders(index, partySearchViewModel);
        }

    }
    partySearchViewModel.showJoinValidation = ko.computed(function () {
        if (partySearchViewModel.applyingItems().length > 0) {
            return false
        } else {
            return true;
        }
    });

}
function setupPartySearch() {
    $('#joinPartyRequest').click(function () {
        var btn = $(this);
        btn.button('loading');
        applyParty(btn, 'new');
    });
    $('#partymorepartysearch').click(function () {
        initializePartySearch();
        setupPartySearch();
    });
    $('#searchParty').click(function () {
        var btn = $(this);
        btn.button('loading');
        getPartySearch(btn, 'new');
    });
    $("#partySearch .panel-body,#partyagendaCodes, #partySearchResultItemcontent-box").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10, alwaysVisibleY: true
        });
    applyKoPartySearch();
    $('.accordionpartysearch').on('shown.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').addClass('active');
        //$(e.target).animate({ height: "390" }, 600);
    });

    $("#partyCodesshowmore").on("click", function () {
        var btn = $(this)
        btn.button('loading')
        getPartySearch(btn, 'append');
    });
    $('.accordionpartysearch').on('hidden.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').removeClass('active');
    });
}
function applyKoPartySearch() {
    ko.applyBindings(agendaCodeViewModel, $("#partyagendaCodes").get(0));
    ko.applyBindings(partySearchViewModel, $("#partySearchResultcontent-wrapper").get(0));
    ko.applyBindings(partySearchViewModel, $("#partySearchResult .panel-footer").get(0));
    ko.applyBindings(partySearchViewModel, $("#joinPartyRequest").get(0));
    ko.applyBindings(peopleInfo, $("#mypartysearchcontent-box .panel-heading").get(0));
}
function collectPartySearchData() {
    var agendaType = $('#partyAgenda input[name=partyAgenda]:checked').map(function () {
        return $(this).val();
    }).get();
    if (agendaType.length == 0) {
        agendaType = $('#partyAgenda input[name=partyAgenda]:not(:checked)').map(function () {
            return $(this).val();
        }).get();
    }

    var partysizelrange = 0;
    var partysizehrange = 0;
    $('#partysize input[name=partysize]:checked').map(function () {
        partysizelrange = $(this).data("lrange");
        partysizehrange = $(this).data("hrange");
    }).get();

    var partyVictorylrange = 0;
    var partyVictoryhrange = 0;
    $('#electionvictory input[name=electionvictory]:checked').map(function () {
        partyVictorylrange = $(this).data("lrange");
        partyVictoryhrange = $(this).data("hrange");
    }).get();

    var partyWorthlrange = 0;
    var partyWorthhrange = 0;
    $('#partyworth input[name=partyworth]:checked').map(function () {
        partyWorthlrange = $(this).data("lrange");
        partyWorthhrange = $(this).data("hrange");
    }).get();

    var partyFeelrange = 0;
    var partyFeehrange = 0;
    $('#membershipFee input[name=membershipFee]:checked').map(function () {
        partyFeelrange = $(this).data("lrange");
        partyFeehrange = $(this).data("hrange");
    }).get();

    console.log(partysizehrange);
    console.log(partysizelrange);

    var searchpartyData = {
        AgendaType: agendaType,
        PartySizeRangeUp: partysizehrange,
        PartySizeRangeDown: partysizelrange,
        PartyVictoryRangeUp: partyVictoryhrange,
        PartyVictoryRangeDown: partyVictorylrange,
        PartyFeeRangeUp: partyFeehrange,
        PartyFeeRangeDown: partyFeelrange,
        PartyWorthRangeUp: partyWorthhrange,
        PartyWorthRangeDown: partyWorthlrange,
        LastStartDate: partySearchViewModel.lastStartDate()
    }
    return searchpartyData;
}
function getPartySearch(btn, searchType) {
    if (partySearchViewModel.contentLoadTriggered == true) {
        return;
    }
    partySearchViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/partyservice/SearchParty",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(collectPartySearchData()),
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#mypartysearchcontent-box .panel-heading").eq(0)),
        success: function (result) {
            if (searchType == 'new') {
                partySearchViewModel.partyCodes.removeAll();
                partySearchViewModel.lastStartDate(0);
            }
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    result[i].PoliticalPartyAgenda = ko.observableArray();
                    result[i].AllPartyMembers = ko.observable();
                    result[i].AllPartyMembers.Founder = ko.observableArray();
                    result[i].AllPartyMembers.Members = ko.observableArray();
                    result[i].AllPartyMembers.CoFounder = ko.observableArray();
                    partySearchViewModel.partyCodes.push(result[i]);

                }
                partySearchViewModel.lastStartDate(result[result.length - 1].StartDate);
            }
            $("#partySearchResult").collapse('show');
            $("#partySearch").collapse('hide');
        }
    }).always(function () {
        btn.button('reset')
        partySearchViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#mypartysearchcontent-box .panel-heading").eq(0));
    });
}
function applyPartyCode(partyId) {
    this.PartyId = partyId;
}
function applyParty(btn) {
    if (partySearchViewModel.applyingItems().length == 0) {
        return;
    }
    if (partySearchViewModel.contentLoadTriggered == true) {
        return;
    }
    partySearchViewModel.contentLoadTriggered = true;
    var applyPartyList = new Array();
    for (var i = 0; i < partySearchViewModel.applyingItems().length; i++) {
        applyPartyList.push(new applyPartyCode(partySearchViewModel.applyingItems()[i]));
    }
    $.ajax({
        url: "/api/PartyService/RequestJoinParty",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(applyPartyList),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            partySearchViewModelresult = ko.mapping.fromJS(result);
            if (partySearchViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mypartysearchcontent-box").html("");
                $("#mypartysearchcontent-box").addClass("hidden");
                $("#partymorepartysearch").removeClass("hidden");
                $("#mypartysearchcontent-submit").removeClass("hidden");
                $("#mypartysearchcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(partySearchViewModelresult, $("#mypartysearchcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#partysearchcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        partySearchViewModel.contentLoadTriggered = false;
        btn.button('reset')
    });
}

function getAllpartyMemberByPage(index, vm, callback) {
    if (vm.partyCodes()[index].AllPartyMembers.Members() != 0 || vm.contentLoadTriggered == true) {
        console.log("im at clickgetAllMembers returing");
        return;
    }
    console.log("im at clickgetAllMembers");

    vm.contentLoadTriggered = true;
    showLoadingImage($("#partyMycontent-box .panel-heading"))
    $.when(getPartyCoFounderByPage(index, vm), getPartyMemberByPage(index, vm), getPartyFounderByPage(index, vm))
    .done(function (a1, a2, a3) {
        for (i = 0; i < a1[0].length; i++) {
            a1[0][i].MemberStatusDisabled = ko.observable();
            a1[0][i].MemberStatusDisabledCss = ko.observable();
            if (a1[0][i].MemberStatus == '') {
                a1[0][i].MemberStatusDisabled(false);
            }
            else {
                a1[0][i].MemberStatusDisabled(true);
                a1[0][i].MemberStatusDisabledCss("disabledanchor");
            }
            vm.partyCodes()[index].AllPartyMembers.CoFounder.push(a1[0][i]);
        }
        for (i = 0; i < a2[0].length; i++) {
            a2[0][i].MemberStatusDisabled = ko.observable();
            a2[0][i].MemberStatusDisabledCss = ko.observable();
            if (a2[0][i].MemberStatus == '') {
                a2[0][i].MemberStatusDisabled(false);
            }
            else {
                a2[0][i].MemberStatusDisabled(true);
                a2[0][i].MemberStatusDisabledCss("disabledanchor");
            }
            vm.partyCodes()[index].AllPartyMembers.Members.push(a2[0][i]);
        }
        for (i = 0; i < a3[0].length; i++) {
            a3[0][i].MemberStatusDisabled = ko.observable();
            a3[0][i].MemberStatusDisabledCss = ko.observable();
            if (a3[0][i].MemberStatus == '') {
                a3[0][i].MemberStatusDisabled(false);
            }
            else {
                a3[0][i].MemberStatusDisabled(true);
                a3[0][i].MemberStatusDisabledCss("disabledanchor");
            }
            vm.partyCodes()[index].AllPartyMembers.Founder.push(a3[0][i]);
        }
        if (typeof (callback) == "function")
            callback();
        vm.contentLoadTriggered = false;
        hideLoadingImage($("#partyMycontent-box .panel-heading"));
    });
}
function getPartyCoFounderByPage(index, vm) {
    var partylength = 0;
    var lastStartDate = null;
    if (vm.partyCodes()[index].AllPartyMembers.CoFounder().length != 0) {
        partylength = vm.partyCodes()[index].AllPartyMembers.CoFounder().length;
        lastStartDate = vm.partyCodes()[index].AllPartyMembers.CoFounder()[partylength - 1].MemberStartDate;
    }

    var memberTypeDTO = {
        PartyId: vm.partyCodes()[index].PartyId,
        LastStartDate: lastStartDate
    };
    return $.ajax({
        url: "/api/PartyService/GetPartyCoFounderByPage",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(memberTypeDTO),
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function getPartyMemberByPage(index, vm) {
    var partylength = 0;
    var lastStartDate = null;

    if (vm.partyCodes()[index].AllPartyMembers.Members() != 0) {
        partylength = vm.partyCodes()[index].AllPartyMembers.Members().length;
        lastStartDate = vm.partyCodes()[index].AllPartyMembers.Members()[partylength - 1].MemberStartDate;
    }

    var memberTypeDTO = {
        PartyId: vm.partyCodes()[index].PartyId,
        LastStartDate: lastStartDate
    };
    return $.ajax({
        url: "/api/PartyService/GetPartyMemberByPage",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(memberTypeDTO),
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function getPartyFounderByPage(index, vm) {
    var partylength = 0;
    var lastStartDate = null;
    if (vm.partyCodes()[index].AllPartyMembers.Founder().length != 0) {
        partylength = vm.partyCodes()[index].AllPartyMembers.Founder().length;
        lastStartDate = vm.partyCodes()[index].AllPartyMembers.Founder[partylength].StartDate;
    }

    var memberTypeDTO = {
        PartyId: vm.partyCodes()[index].PartyId,
        LastStartDate: lastStartDate
    };
    return $.ajax({
        url: "/api/PartyService/GetPartyFounderByPage",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(memberTypeDTO),
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function getMoreCoFounders(index, vm) {
    if (vm.contentLoadTriggered == true) {
        return;
    }
    vm.contentLoadTriggered = true;
    showLoadingImage($("#partyMycontent-box .panel-heading"));
    getPartyCoFounderByPage(index, vm).success(function (data) {
        for (i = 0; i < data.length; i++) {
            data[i].MemberStatusDisabled = ko.observable();
            data[i].MemberStatusDisabledCss = ko.observable();
            if (data[i].MemberStatus == '') {
                data[i].MemberStatusDisabled(false);
            }
            else {
                data[i].MemberStatusDisabled(true);
                data[i].MemberStatusDisabledCss("disabledanchor");
            }
            vm.partyCodes()[index].AllPartyMembers.CoFounder.push(data[i]);
        }
        vm.contentLoadTriggered = false;
        hideLoadingImage($("#partyMycontent-box .panel-heading"));
    });
}
function getMoreMembers(index, vm) {
    if (vm.contentLoadTriggered == true) {
        return;
    }
    vm.contentLoadTriggered = true;

    showLoadingImage($("#partyMycontent-box .panel-heading"));
    getPartyMemberByPage(index, vm).success(function (data) {
        for (i = 0; i < data.length; i++) {
            data[i].MemberStatusDisabled = ko.observable();
            data[i].MemberStatusDisabledCss = ko.observable();
            if (data[i].MemberStatus == '') {
                data[i].MemberStatusDisabled(false);
            }
            else {
                data[i].MemberStatusDisabled(true);
                data[i].MemberStatusDisabledCss("disabledanchor");
            }
            vm.partyCodes()[index].AllPartyMembers.Members.push(data[i]);
        }
        vm.contentLoadTriggered = false;
        hideLoadingImage($("#partyMycontent-box .panel-heading"));
    });
}

function initializeMyParty() {
    $("#mypartycontainer").html(myPartyView);
    mypartyViewModel = {
        partyCodes: ko.observableArray(),
        AmountLeft: ko.observable(),
        contentLoadTriggered: false,
        clickgetAllMembers: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getAllpartyMemberByPage(index, mypartyViewModel);
            scrollonClickmyparty();
        },
        clickgetmoreMembers: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getMoreMembers(index, mypartyViewModel);
        },
        clickgetmoreCoFounders: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getMoreCoFounders(index, mypartyViewModel);
        },
        clickgetagenda: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            var vm = mypartyViewModel.partyCodes()[index];
            if (vm.PoliticalPartyAgenda().length <= 0) {
                getpartyAgenda(vm.PartyId, vm).success(function (data) {
                    processpartyAgenda(data, vm);
                })
            }
            scrollonClickmyparty();
        },
        clickDonate: function (data, event) {
            scrollonClickmyparty();
        },
        clickPartyManage: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data('index');
            console.log("clickPartyManage");
            scrollonClickmyparty();
            setupmanageParty(index);
        },
        clickPartyInvite: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data('index');
            console.log(self);
            var selfcontent = self.closest('.navigation')
                .next('.partymy-body')
                .find('[data-class="clickPartyInvite"]')
            getPartyInviteDetails(selfcontent, index);
            scrollonClickmyparty();
        },
        afterPartyMemberRenderAction: function (elem) {
            var self = $(elem[1]).find(".show-menu"); //index 1 as it has 3 array in elem
            self.on('click', function (event) {
                event.preventDefault();
                $(this).closest('li').toggleClass('open');
            });
        },
        afterMyPartyRender: function (elem) {
            var self = $(elem[1]).find(".donateCash"); //index 1 as it has 3 array in elem
            var index = parseInt(self.data('index'));
            var btn = $(elem[1]).find(".donateCashbtn"); //index 1 as it has 3 array in elem

            btn.on('click', function () {
                donatePartyCash(btn, index);
            });
            self.on('change', function () {
                var donationAmount = parseInt(self.val());
                updatemypartyDonationvm(index, donationAmount);
            });
            spinInputwtPrefix(self, "Cash", userBankAccountViewModel.Cash());
        },
        clickejectMember: function (data, event) {
            var self = $(event.currentTarget);
            var parent = self.closest('ul.list-group-submenu');
            var index = parseInt(parent.data("index"));
            var pindex = parseInt(parent.data("pindex"));
            var memberType = parent.data("membertype");
            var vm;
            if (memberType == 'F') {
                vm = mypartyViewModel.partyCodes()[pindex].AllPartyMembers.Founder;
            }
            else if (memberType == 'C') {
                vm = mypartyViewModel.partyCodes()[pindex].AllPartyMembers.CoFounder;
            }
            else if (memberType == 'M') {
                vm = mypartyViewModel.partyCodes()[pindex].AllPartyMembers.Members;
            }
            var memberStatus = vm()[index].MemberStatusDisabled();

            if (memberStatus == true) {
                return;
            }
            var ejectionDTO = {
                InitatorId: mypartyViewModel.partyCodes()[pindex].UserId,
                PartyId: mypartyViewModel.partyCodes()[pindex].PartyId,
                EjecteeId: parseInt(parent.data("userid")),

            };
            ejectMember(self, ejectionDTO, vm, index, pindex);
        },
        clicknominateCoFounder: function (data, event) {
            collectMemberNominationInfo("C", event);
        },
        clicknominateFounder: function (data, event) {
            collectMemberNominationInfo("F", event);
        },
        clicknominateMember: function (data, event) {
            collectMemberNominationInfo("M", event);
        },
        clickmemberProfile: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
        }
    }
}
function collectMemberNominationInfo(nominationType, event) {
    var self = $(event.currentTarget);
    var parent = self.closest('ul.list-group-submenu');
    var index = parseInt(parent.data("index"));
    var pindex = parseInt(parent.data("pindex"));
    var memberType = parent.data("membertype");
    console.log(pindex);
    console.log(index);
    console.log(parent);
    console.log(self);
    var vm;
    if (memberType == 'F') {
        vm = mypartyViewModel.partyCodes()[pindex].AllPartyMembers.Founder;
    }
    else if (memberType == 'C') {
        vm = mypartyViewModel.partyCodes()[pindex].AllPartyMembers.CoFounder;
    }
    else if (memberType == 'M') {
        vm = mypartyViewModel.partyCodes()[pindex].AllPartyMembers.Members;
    }
    var memberStatus = vm()[index].MemberStatusDisabled();
    if (memberStatus == true) {
        return;
    }
    var nominationDTO = {
        InitatorId: mypartyViewModel.partyCodes()[pindex].UserId,
        PartyId: mypartyViewModel.partyCodes()[pindex].PartyId,
        NomineeId: parseInt(parent.data("userid")),
        NominatingMemberType: nominationType,
        NomineeIdMemberType: parent.data("membertype")
    };
    nominateMembers(self, nominationDTO, vm, index, pindex);
}
function leaveParty() {
    var btn = $(this);
    btn.button('loading');
    if (mypartyViewModel.contentLoadTriggered == true) {
        return;
    }
    mypartyViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/PartyService/QuitParty",
        type: "post",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            myPartyViewModelresult = ko.mapping.fromJS(result);
            if (myPartyViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#partyMycontent-box").html("");
                $("#partyMycontent-box").addClass("hidden");
                $("#quitmoreparty").removeClass("hidden");
                $("#partyMycontent-submit").removeClass("hidden");
                $("#partyMycontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(myPartyViewModelresult, $("#partyMycontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#mypartycontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        mypartyViewModel.contentLoadTriggered = false;
        btn.button('reset')
    });
}
function refreshMyParty() {
    mypartyViewModel.partyCodes.removeAll();
    getMyParty();
}
function setupMyParty() {
    $('#partyrefresh').click(function () {
        refreshMyParty();
    });
    applyKoMyParty();
    $("#partyMyItemcontent-box").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10,
            alwaysVisibleY: true
        });
}
function applyKoMyParty() {
    ko.applyBindings(mypartyViewModel, $("#partyMycontent-box .panel-body").get(0));
    ko.applyBindings(mypartyViewModel, $("#partyMycontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#partyMycontent-box .panel-heading").get(0));
}
function getMyParty() {
    if (mypartyViewModel.contentLoadTriggered == true) {
        return;
    }
    mypartyViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/partyservice/getuserparties",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#partyMycontent-box .panel-heading")),
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    result[i].OriginalPartyName = result[i].PartyName;
                    result[i].PoliticalPartyAgenda = ko.observableArray();
                    result[i].agendaCodes = ko.observableArray();
                    result[i].AllPartyMembers = ko.observable();
                    result[i].InviteeId = ko.observableArray();
                    result[i].EmailInviteeId = ko.observableArray();
                    result[i].EmailInviteeIdList = ko.observableArray();
                    result[i].LastEmailInviteeId = ko.observable('');
                    result[i].SubmitInviteResponse = ko.observable();
                    result[i].DonationInCash = ko.observable(0);
                    result[i].DonationList = ko.observableArray();
                    result[i].PartyManageList = ko.observableArray();
                    result[i].PartyUploadManageList = ko.observableArray();
                    result[i].AmountLeft = ko.observable(userBankAccountViewModel.Cash());
                    result[i].AllPartyMembers.Founder = ko.observableArray();
                    result[i].AllPartyMembers.Members = ko.observableArray();
                    result[i].AllPartyMembers.CoFounder = ko.observableArray();
                    result[i].uploadprofilePicObj = new uploadImageObj('partymanage'),
                    mypartyViewModel.partyCodes.push(result[i]);
                }
            }
        }
    }).always(function () {
        mypartyViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#partyMycontent-box .panel-heading"));
    });

}
function scrollonClickmyparty() {
    $("#partyMyItemcontent-box").scrollTop(150);
    $("#partyMyItemcontent-box").perfectScrollbar('update');
}
function ejectMember(btn, ejectionDTO, vm, index, pindex) {
    btn.button('loading');
    btn.addClass('disabled');
    $.ajax({
        url: "/api/PartyService/RequestEjectMember",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(ejectionDTO),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                vm()[index].MemberStatusDisabled(true);
                vm()[index].MemberStatusDisabledCss("disabledanchor");
                console.log(vm()[index]);
                console.log(index);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {

        btn.button('reset')
    });
}

function nominateMembers(btn, nominationDTO, vm, index, pindex) {
    btn.button('loading');
    btn.addClass('disabled');
    $.ajax({
        url: "/api/PartyService/RequestNominationParty",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(nominationDTO),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                vm()[index].MemberStatusDisabled(true);
                vm()[index].MemberStatusDisabledCss("disabledanchor");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {

        btn.button('reset')
    });
}

function updatemypartyDonationvm(index, donationAmount) {
    var leftCash = parseFloat(userBankAccountViewModel.Cash() - donationAmount).toFixed(2);
    mypartyViewModel.AmountLeft(leftCash);
    mypartyViewModel.partyCodes()[index].DonationInCash(donationAmount);
}
function donatePartyCash(btn, index) {
    var donationDTO = {
        PartyId: mypartyViewModel.partyCodes()[index].PartyId,
        Amount: mypartyViewModel.partyCodes()[index].DonationInCash()
    }
    if (donationDTO.Amount <= 0) {
        return;
    }
    btn.button('loading');
    btn.addClass('disabled');
    $.ajax({
        url: "/api/PartyService/DonateParty",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(donationDTO),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                mypartyViewModel.partyCodes()[index].DonationList.push(donationDTO);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {

        btn.button('reset')
    });
}



function populateEmailInviteList(list, index) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            mypartyViewModel.partyCodes()[index].EmailInviteeIdList.push(list[i]);
        }
        mypartyViewModel.partyCodes()[index].LastEmailInviteeId(list[list.length - 1].FriendEmailId);
    }
}
function setupPartyInvite(selfcontent, index) {
    if (selfcontent.children().length == 0) {
        selfcontent.html(myPartyInviteView[0]);
        ko.applyBindings(friendViewModel,
          selfcontent.find('.partyinvitefriendList')
            .get(0));
        ko.applyBindings(mypartyViewModel.partyCodes()[index],
  selfcontent.find('.partyinviteEmailList')
    .get(0));
        ko.applyBindings(mypartyViewModel.partyCodes()[index],
            selfcontent.find('.emailListFooter')
                .get(0));
        ko.applyBindings(mypartyViewModel.partyCodes()[index],
            selfcontent.find('.friendListFooter')
                .get(0));
        ko.applyBindings(mypartyViewModel.partyCodes()[index],
          selfcontent.find('.partyInvitemsg')
        .get(0));
        selfcontent.find('.partyinvitefriendList').perfectScrollbar(
                     {
                         suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
                     });
        selfcontent.find('.partyinviteEmailList').perfectScrollbar(
             {
                 suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
             });

        selfcontent.find('.partyInvitebtn').on('click', function () {
            sendPartyInvite($(this), index, selfcontent);
        });
        selfcontent.find('#partyWebContactshowmore').on('click', function () {
            var lastEmailid = mypartyViewModel.partyCodes()[index].LastEmailInviteeId();
            getEmailInvitationList(lastEmailid).success(function (data) {
                populateEmailInviteList(data, index);
                resetpartyinviteEmailAll(index);

            });
        });
        setupcheckboxEmailContact(index);
        setupcheckboxFriends(index);

    }
}
function sendPartyInvite(btn, index, selfcontent) {
    var partyInviteDTO = [];
    if (mypartyViewModel.partyCodes()[index].InviteeId().length <= 0 &&
        mypartyViewModel.partyCodes()[index].EmailInviteeId().length <= 0) {
        return;
    }
    for (var i = 0; i < mypartyViewModel.partyCodes()[index].InviteeId().length; i++) {
        var inviteedto = {
            FriendId: mypartyViewModel.partyCodes()[index].InviteeId()[i]
        }
        partyInviteDTO.push(inviteedto);
    }
    for (var i = 0; i < mypartyViewModel.partyCodes()[index].EmailInviteeId().length; i++) {
        var inviteedto = {
            EmailId: mypartyViewModel.partyCodes()[index].EmailInviteeId()[i]
        }
        partyInviteDTO.push(inviteedto);
    }

    btn.button('loading');
    btn.addClass('disabled');
    $.ajax({
        url: "/api/PartyService/SendPartyInvite",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify({ PartyInvites: partyInviteDTO }),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                mypartyViewModel.partyCodes()[index].SubmitInviteResponse(result.Message);
                console.log(selfcontent);
                console.log("That was selfContent");
                selfcontent.find('.partyInvitediv').addClass("hidden");
                selfcontent.find('.partyInvitemsg').removeClass("hidden");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {

        btn.button('reset')
    });
}
function getPartyInviteDetails(selfcontent, index) {
    var lastEmailid = mypartyViewModel.partyCodes()[index].LastEmailInviteeId();
    $.when(getpartyinviteview(), getEmailInvitationList(lastEmailid))
    .done(function (a1, a2) {
        if (myPartyInviteView == "") {
            myPartyInviteView = a1;
        }
        populateEmailInviteList(a2[0], index);
        setupPartyInvite(selfcontent, index);
    });
}
function getpartyinviteview() {
    if (myPartyInviteView != "") {
        return;
    }
    return $.ajax("/politicalparty/getpartyinviteview");
}
function setupcheckboxEmailContact(index) {
    var $widget = $('#partyinvitemy' + index).find(".invitepartyEmail"),
    $button = $widget.find('button'),
    $checkbox = $widget.find('input:checkbox');
    console.log("WidgetWidgetWidget" + index.toString());
    console.log($widget);
    $checkbox.on('change', function () {
        updateDisplay($checkbox, $button);
        updateSelection(mypartyViewModel.partyCodes()[index].EmailInviteeId, $checkbox);
    });
    initilaizeUpdatedCheckbox($checkbox, $button);
}
function setupcheckboxFriends(index) {
    var $widget = $('#partyinvitemy' + index).find(".invitepartyFriends"),
    $button = $widget.find('button'),
    $checkbox = $widget.find('input:checkbox');
    console.log("WidgetWidgetWidget" + index.toString());
    console.log($widget);
    $checkbox.on('change', function () {
        updateDisplay($checkbox, $button);
        updateSelection(mypartyViewModel.partyCodes()[index].InviteeId, $checkbox);
    });
    initilaizeUpdatedCheckbox($checkbox, $button);
}
function clickpartyinviteemailcontact(data, event) {
    var self = $(event.currentTarget);
    var index = parseInt(self.closest('[data-class="clickPartyInvite"]').data('index'));
    vm = mypartyViewModel.partyCodes()[index].EmailInviteeId;
    emailinviteupdatecheckbox(vm, event);
    resetpartyinviteEmailAll(index);
}

function resetpartyinviteEmailAll(index) {
    if (mypartyViewModel.partyCodes()[index].EmailInviteeId().length !=
 mypartyViewModel.partyCodes()[index].EmailInviteeIdList().length) {
        var $checkbox = $("#partyinvitewebcontactall");
        $checkbox.prop('checked', false);
        $button = $checkbox.siblings('button');
        updateDisplay($checkbox, $button);
    }
}

function setupmanageParty(index) {
    var vm = mypartyViewModel.partyCodes()[index];
    $.when(getmanagepartyview(index), getpartyAgenda(vm.PartyId, vm))
   .done(function (a1, a2) {
       if (managePartyView == "" && a1 != accessdenied) {
           managePartyView = a1[0];
       }
       else if (a1 == accessdenied) {
           managePartyView = a1;
       }

       $("#partymanagemy" + index).html(managePartyView);
       if (managePartyView == accessdenied) {
           return;
       }
       if (vm.PoliticalPartyAgenda().length <= 0) {
           processpartyAgenda(a2[0], vm);
       }


       for (var i = 0; i < agendaCodeViewModel.agendaCodes().length; i++) {
           vm.agendaCodes.push(agendaCodeViewModel.agendaCodes()[i]);
           for (var j = 0; j < vm.PoliticalPartyAgenda().length; j++) {
               if (agendaCodeViewModel.agendaCodes()[i].AgendaTypeId() ==
                   vm.PoliticalPartyAgenda()[j].AgendaTypeId
                   ) {
                   vm.agendaCodes()[i].selected(true);
                   break;
               }
           }
       }

       ko.applyBindings(vm,
                    $("#partymanagemy" + index).find('.manageparty').get(0));

       defineValidationRulesStartparty($('#managepartyform'));
       $("#managepartyfileupload").change(function (evt) {
           if (this.disabled) return alert('File upload not supported!');
           handleFileSelect(evt,
               $("#partymanagepicprw"), vm.uploadprofilePicObj, vm.LogoPictureId);
       });
       spinInputwtPrefix($("#managepartymembershipFee"), "Membership Fee", 1000000);
       $("#managepartysave").on('click', function () {
           manageChanges($(this));
       });
       $("#managepartyclose").on('click', function () {
           closeParty($(this));
       });
       $("#managepartyupload").on('click', function () {

           var picvm = vm.uploadprofilePicObj;
           $(this).button('loading');
           uploadImage(picvm,
               $("#partymanagepicprw"), function () {
                   manageuploadlogo($("#managepartyupload"), picvm);
               });
       });
   });

}
function manageChanges(btn) {
    if ($('#managepartyform').valid()) {
        $.ajax({
            url: "/api/PartyService/ManageParty",
            type: "post",
            contentType: "application/json",
            data: JSON.stringify(collectmanagetPartyData()),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    userNotificationPollOnce();
                    mypartyViewModel.partyCodes()[0].PartyManageList.push(1);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                btn.button('reset')

            }
        }).always(function () {

            btn.button('reset')
        });
    }
}
function collectmanagetPartyData() {
    var agendaType = $('#startpartyAgenda input[name=partyAgenda]:checked').map(function () {
        return $(this).val();
    }).get();
    var form = $("#managepartyform");
    var startpartyData = {
        AgendaType: agendaType,
        PartyName: form.find('[name="partyName"]').val(),
        MembershipFee: form.find('[name="partymembershipFee"]').val(),
        Motto: form.find('[name="partyMotto"]').val()
    }
    return startpartyData;
}
function closeParty(btn) {

    btn.button('loading');
    btn.addClass('disabled');
    $.ajax({
        url: "/api/PartyService/RequestCloseParty",
        type: "post",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                btn.next('.closerequest').removeClass("hidden");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {

        btn.button('reset')
    });
}
function getmanagepartyview(index) {
    if (mypartyViewModel.partyCodes()[index].MemberType != 'F') {
        return accessdenied;
    }
    if (managePartyView != "") {
        return managePartyView;
    }
    return $.ajax("/politicalparty/getmanagepartyview");
}
function manageuploadlogo(btn, picvm) {
    if (mypartyViewModel.partyCodes()[0].LogoPictureId) {
        //No need for update has fileNameAlready
        mypartyViewModel.partyCodes()[0].PartyUploadManageList.push(1);
        btn.button('reset')
        return;
    }
    $.ajax({
        url: "/api/PartyService/ManagePartyUploadPartyLogo",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify({ LogoPictureId: picvm.imageName }),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            console.log(result);
            console.log(btn);
            console.log(btn.next('.closerequest'));

            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                mypartyViewModel.partyCodes()[0].PartyUploadManageList.push(1);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')

        }
    }).always(function () {

        btn.button('reset')
    });
}

function initializeMyHistoryParty() {
    $("#myHistorypartycontainer").html(myHistoryPartyView);
    myHistorypartyViewModel = {
        partyCodes: ko.observableArray(),
        AmountLeft: ko.observable(),
        contentLoadTriggered: false,
        clickgetAllMembers: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getAllpartyMemberByPage(index, myHistorypartyViewModel);
        },
        clickgetmoreMembers: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getMoreMembers(index, myHistorypartyViewModel);
        },
        afterPartyMemberRenderAction: function (elem) {
            var self = $(elem[1]).find(".show-menu"); //index 1 as it has 3 array in elem
            self.on('click', function (event) {
                event.preventDefault();
                $(this).closest('li').toggleClass('open');
            });
        },
        clickgetmoreCoFounders: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getMoreCoFounders(index, myHistorypartyViewModel);
        },
        clickgetagenda: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            var vm = myHistorypartyViewModel.partyCodes()[index];

            if (vm.PoliticalPartyAgenda().length <= 0) {
                getpartyAgenda(vm.PartyId, vm).success(function (data) {
                    processpartyAgenda(data, vm);
                })
            }
        },
        clickDonate: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
        },
        afterMyHistoryPartyRender: function (elem) {
            var self = $(elem[1]).find(".donateCash"); //index 1 as it has 3 array in elem
            var index = parseInt(self.data('index'));
            var btn = $(elem[1]).find(".donateCashbtn"); //index 1 as it has 3 array in elem

            btn.on('click', function () {
                donatePartyCashHistory(btn, index);
            });
            self.on('change', function () {
                var donationAmount = parseInt(self.val());
                updatemyHistorypartyDonationvm(index, donationAmount);
            });
            spinInputwtPrefix(self, "Cash", userBankAccountViewModel.Cash());
        },
        clickmemberProfile: function (data, event) {
            //TODO
            var self = $(event.currentTarget);
            var index = self.data("index");
        }
    }


}
function refreshMyHistoryParty() {
    myHistorypartyViewModel.partyCodes.removeAll();
    getMyHistoryParty();
}
function setupMyHistoryParty() {
    $('#partyhistoryrefresh').click(function () {
        refreshMyHistoryParty();
    });
    applyKoMyHistoryParty();
    $("#partyMyHistoryItemcontent-box").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10,
            alwaysVisibleY: true
        });
}
function applyKoMyHistoryParty() {
    ko.applyBindings(myHistorypartyViewModel, $("#partyMyHistorycontent-box .panel-body").get(0));
    ko.applyBindings(myHistorypartyViewModel, $("#partyMyHistorycontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#partyMyHistorycontent-box .panel-heading").get(0));
}
function getMyHistoryParty() {
    if (myHistorypartyViewModel.contentLoadTriggered == true) {
        return;
    }
    myHistorypartyViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/partyservice/getpastuserparties",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#partyMyHistorycontent-box .panel-heading").eq(0)),
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    result[i].PoliticalPartyAgenda = ko.observableArray();
                    result[i].AllPartyMembers = ko.observable();
                    result[i].DonationList = ko.observableArray();
                    result[i].DonationInCash = ko.observable(0);
                    result[i].AmountLeft = ko.observable(userBankAccountViewModel.Cash());
                    result[i].AllPartyMembers.Founder = ko.observableArray();
                    result[i].AllPartyMembers.Members = ko.observableArray();
                    result[i].AllPartyMembers.CoFounder = ko.observableArray();
                    myHistorypartyViewModel.partyCodes.push(result[i]);
                }
            }
        }
    }).always(function () {
        myHistorypartyViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#partyMyHistorycontent-box .panel-heading").eq(0));
    });
}
function updatemyHistorypartyDonationvm(index, donationAmount) {
    var leftCash = parseFloat(userBankAccountViewModel.Cash() - donationAmount).toFixed(2);
    myHistorypartyViewModel.AmountLeft(leftCash);
    myHistorypartyViewModel.partyCodes()[index].DonationInCash(donationAmount);
}
function donatePartyCashHistory(btn, index) {
    var donationDTO = {
        PartyId: myHistorypartyViewModel.partyCodes()[index].PartyId,
        Amount: myHistorypartyViewModel.partyCodes()[index].DonationInCash()
    }
    if (donationDTO.Amount <= 0) {
        return;
    }
    btn.button('loading');
    btn.addClass('disabled');
    $.ajax({
        url: "/api/PartyService/DonateParty",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(donationDTO),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                myHistorypartyViewModel.partyCodes()[index].DonationList.push(donationDTO);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {

        btn.button('reset')
    });
}


function initializePartySummary() {
    $("#partySummarycontainer").html(partySummaryView);
    partySummaryViewModel = {
        partyTopN: ko.observableArray()
    }
}
function setupPartySummary() {
    applyKoPartySummary();
}
function applyKoPartySummary() {
    ko.applyBindings(peopleInfo, $("#partySummarycontainer .panel-heading").get(0));
    ko.applyBindings(partySummaryViewModel, $("#partyTopNSummary").get(0));
}
function getPartySummary() {
    $.ajax({
        url: "/api/partyservice/GetUserPartySummary",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            userPartySummaryViewModel = ko.mapping.fromJS(result);
            ko.applyBindings(userPartySummaryViewModel, $("#partySummarycontent-box").get(0));

        }
    });
}
function getTopNPartySalary() {
    $.ajax({
        url: "/api/partyservice/GetTopTenPartyByMember",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    result[i].Rank = ordinal_suffix_of(i + 1);
                }
                ko.mapping.fromJS(result, {}, partySummaryViewModel.partyTopN);

            }
        }
    });
}

function initializeStartParty() {
    $("#party-startparty").html(startPartyView);
    startPartyViewModel = {
        contentLoademailTriggered: false,
        founderInviteSelected: ko.observableArray(),
        emailInviteeIdList: ko.observableArray(),
        emailInviteeId: ko.observableArray(),
        uploadprofilePicObj: new uploadImageObj('partynew'),
        lastEmailInviteeId: ko.observable('')
    };
}
function setupStartParty() {

    spinInputwtPrefix($("#partymembershipFee"), "Membership Fee",
        1000000);
    $("#startPartysubmit").on('click', function () {
        console.log("Clicked Start done");
        saveStartParty($(this));
    });

    $("#partystartWebContactshowmore").on('click', function () {
        startpartyEmailInvitaionList();
    });
    $("#partyfileupload").change(function (evt) {
        if (this.disabled) return alert('File upload not supported!');
        handleFileSelect(evt,
            $("#partypicprw"), startPartyViewModel.uploadprofilePicObj, '');
    });

    ko.applyBindings(agendaCodeViewModel, $("#startpartyagendaCodes").get(0));
    ko.applyBindings(friendViewModel, $("#partyinvitefriends")[0]);
    ko.applyBindings(startPartyViewModel, $("#startpartyinviteemailcontacts")[0]);
    $("#startpartyinviteemailcontacts .partyinviteEmailList, #partyinvitefriends, #startpartyagendaCodes").perfectScrollbar({
        suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
    });
    ko.applyBindings(startPartyViewModel, $("#partyfriendsFooter")[0]);
    ko.applyBindings(peopleInfo, $("#startPartycontent-box .panel-heading").get(0));
    setupcheckboxStartParty();
    setupcheckboxstartpartyEmailContact();
    defineValidationRulesStartparty($('#startpartyform'));
}
function setupcheckboxStartParty() {
    var $widget = $("#inviteFounderParty .button-checkbox"),
    $button = $widget.find('button'),
    $checkbox = $widget.find('input:checkbox');
    $checkbox.on('change', function () {
        updateDisplay($checkbox, $button);
        updateSelection(startPartyViewModel.founderInviteSelected, $checkbox);
    });
    initilaizeUpdatedCheckbox($checkbox, $button);
}
function setupcheckboxstartpartyEmailContact() {
    var $widget = $('#startpartyinviteemailcontacts').find(".button-checkbox"),
    $button = $widget.find('button'),
    $checkbox = $widget.find('input:checkbox');
    $checkbox.on('change', function () {
        updateDisplay($checkbox, $button);
        updateSelection(startPartyViewModel.emailInviteeId, $checkbox);
    });
    initilaizeUpdatedCheckbox($checkbox, $button);
}
function defineValidationRulesStartparty(elem) {
    elem.validate({
        ignore: "",
        onkeyup: false,
        rules: {
            partyName: {
                required: true,
                minlength: 5,
                maxlength: 60,
                remote: {
                    url: "/api/PartyService/IsUniquePartyName",
                    type: "get",
                    contentType: "application/json",
                    data: {
                        countryId: function () {
                            if (elem.attr('id') == 'startpartyform') {
                                return peopleInfo.CountryId();
                            }
                            else {
                                return mypartyViewModel.partyCodes()[0].CountryId;
                            }
                        },
                        originalName: function () {

                            if (elem.attr('id') == 'startpartyform') {
                                return "";
                            }
                            else {
                                return mypartyViewModel.partyCodes()[0].OriginalPartyName;
                            }
                        }
                    },
                    headers: {
                        RequestVerificationToken: Indexreqtoken
                    }
                },
            },
            partyMotto: {
                required: true,
                minlength: 5,
                maxlength: 240
            },
            partymembershipFee: {
                required: true,
                min: 0,
                max: 1000000
            },
            partyAgenda: {
                required: function (element) {
                    var boxes = $('.checkbox');
                    if (boxes.filter(':checked').length == 0) {
                        return true;
                    }
                    return false;
                }
            }
        },
        messages: {
            partyName: {
                required: "Enter a Party Name",
                minlength: jQuery.format("Enter at least {0} characters"),
                maxlength: jQuery.format("Enter a max of {0} characters"),
                remote: "Party Name already taken"
            },
            partyMotto: {
                required: "Enter a Party Motto",
                minlength: jQuery.format("Enter at least {0} characters"),
                maxlength: jQuery.format("Enter a max of {0} characters")
            },
            partymembershipFee: {
                required: "Enter a MemberShip Fee",
                min: jQuery.format("Must be atleast {0}"),
                max: jQuery.format("Must be less that {0}")
            },
            partyAgenda: {
                required: jQuery.format("Must select at least 1 agenda")
            }
        },
        // specifying a submitHandler prevents the default submit, good for the demo
        submitHandler: function () {
            alert("submitted!");
        },
        // set this class to error-labels to indicate valid fields
        success: function (label) {
            // set &nbsp; as text for IE
            //label.html("&nbsp;").addClass("checked");
        },
        errorPlacement: function (error, element) {
            console.log(element.parent());
            console.log(error);
            if (element.parent('li').length) {
                error.insertBefore(element.parents('.checkbox'));
            } else if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            }
            else {
                error.insertAfter(element);
            }
        }
    });
    console.log("applying validation");

}
function collectDataUniqueNameValidation(elem) {
    console.log(elem.find('input[name^=partyName]').val());
    var partyname = elem.find('input[name^=partyName]').val();
    var countyrid = peopleInfo.CountryId();
    if (elem.attr('id') != 'startpartyform') {
        countyrid = myPartyViewModel.partyCodes()[0].CountryId;
    }
    var originalName = "";

    if (elem.attr('id') != 'startpartyform') {
        return myPartyViewModel.partyCodes()[0].PartyName;
    }

    var validationdata = {
        PartyName: partyname,
        CountryId: countyrid,
        OriginalName: originalName
    };
    console.log(validationdata);
    return validationdata;
}
function saveStartParty(btn) {
    if ($('#startpartyform').valid()) {
        var vm = startPartyViewModel.uploadprofilePicObj;
        uploadImage(vm, null);

        $.ajax({
            url: "/api/PartyService/StartParty",
            type: "post",
            contentType: "application/json",
            data: JSON.stringify(collectstartPartyData()),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                var startpartyViewModelresult = ko.mapping.fromJS(result);
                if (result.StatusCode == 200) {
                    userNotificationPollOnce();
                    console.log(result);
                    btn.addClass("hidden");
                    $("#startPartycontent-box").html("");
                    $("#startPartycontent-box").addClass("hidden");
                    $("#startPartycontent-submit").removeClass("hidden");
                    $("#startPartycontent-submit").next(".backbtn").removeClass("hidden");
                    ko.applyBindings(startpartyViewModelresult, $("#startPartycontent-submit").get(0));
                    $("#main").animate({ scrollTop: $("#startpartycontainer").offset().top }, "slow");

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                btn.button('reset')

            }
        }).always(function () {

            btn.button('reset')
        });
    }

}
function startpartyEmailInvitaionList() {
    var lastEmailid = startPartyViewModel.lastEmailInviteeId();
    if (startPartyViewModel.contentLoademailTriggered == false) {
        startPartyViewModel.contentLoademailTriggered = true;
        getEmailInvitationList(lastEmailid).success(function (data) {
            startPartyViewModel.contentLoademailTriggered = false;
            populatepartyStartEmailInviteList(data);
            resetpartystartinviteEmailAll();
        });
    }
}
function resetpartystartinviteEmailAll() {
    if (startPartyViewModel.emailInviteeId().length !=
 startPartyViewModel.emailInviteeIdList().length) {
        var $checkbox = $("#partyinvitestartwebcontactall");
        $checkbox.prop('checked', false);
        $button = $checkbox.siblings('button');
        updateDisplay($checkbox, $button);
    }
}
function populatepartyStartEmailInviteList(list) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            startPartyViewModel.emailInviteeIdList.push(list[i]);
        }
        startPartyViewModel.lastEmailInviteeId(list[list.length - 1].FriendEmailId);
    }
}
function clickpartystartinviteemailcontact(data, event) {
    vm = startPartyViewModel.emailInviteeId;
    emailinviteupdatecheckbox(vm, event);
    resetpartystartinviteEmailAll();
}
function collectstartPartyData() {
    var agendaType = $('#startpartyAgenda input[name=partyAgenda]:checked').map(function () {
        return $(this).val();
    }).get();
    var form = $("#startpartyform");

    var startpartyData = {
        AgendaType: agendaType,
        PartyName: form.find('[name="partyName"]').val(),
        LogoPictureId: startPartyViewModel.uploadprofilePicObj.imageName,
        MembershipFee: form.find('[name="partymembershipFee"]').val(),
        Motto: form.find('[name="partyMotto"]').val(),
        FriendInvitationList: startPartyViewModel.founderInviteSelected(),
        ContactInvitationList: startPartyViewModel.emailInviteeId()
    }
    return startpartyData;
}
function checkifUniquePartyName(name, self) {
    console.log(self);
    console.log(self.parent());
    $.ajax({
        url: "/api/PartyService/IsUniquePartyName",
        type: "get",
        contentType: "application/json",
        data: { partyName: name },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (data) {
            if (data == true) {
                self.parent().parent().insertAfter
                    ("<span style='color:green'>Available</span>");
                self.css('background-color', '');
            }
            else {
                $("<span style='color:red'>Not Available</span>").insertAfter(self.parent());
                self.css('background-color', '#e97878');
            }
        }
    });

}





function initializePartyInfoView() {
    partyinfoViewModel = {
        partyCodes: ko.observableArray(),
        contentLoadTriggered: false,
        clickgetAllMembers: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getAllpartyMemberByPage(index, partyinfoViewModel);
        },
        clickgetmoreMembers: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getMoreMembers(index, partyinfoViewModel);
        },
        clickgetagenda: function (data, event) {
            var vm = partyinfoViewModel.partyCodes()[0];
            if (vm.PoliticalPartyAgenda().length <= 0) {
                getpartyAgenda(vm.PartyId, vm).success(function (data) {
                    processpartyAgenda(data, vm);
                })
            }
        },
        clickgetmoreCoFounders: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            getMoreCoFounders(index, partyinfoViewModel);
        }

    }
}
function getpartyAgenda(parmpartyId, vm) {
    if (vm.PoliticalPartyAgenda().length > 0) {
        return;
    }
    return $.ajax({
        url: "/api/PartyService/GetPartyAgendasJson",
        type: "get",
        contentType: "application/json",
        data: { partyId: parmpartyId },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function processpartyAgenda(data, vm) {
    if (data.length > 0) {
        for (var j = 0; j < data.length; j++) {
            var agendaobj = {
                Description: agendaCodeViewModel.agendaCodes()[data[j].AgendaTypeId - 1].AgendaName(),
                AgendaTypeId: data[j].AgendaTypeId
            }
            vm.PoliticalPartyAgenda.push(agendaobj);
        }
    }
}
function getpartyinfoById(taskId) {
    $.ajax({
        url: "/api/PartyService/GetUserPartById",
        type: "get",
        contentType: "application/json",
        data: { partyId: taskId },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            console.log(result);
            if (result.PartyId == taskId) {

                result.PoliticalPartyAgenda = ko.observableArray();
                result.AllPartyMembers = ko.observable();
                result.AllPartyMembers.Founder = ko.observableArray();
                result.AllPartyMembers.Members = ko.observableArray();
                result.AllPartyMembers.CoFounder = ko.observableArray();
                partyinfoViewModel.partyCodes.push(result);
            }
        }
    });
}
function applyKoPartyInfo() {
    ko.applyBindings(partyinfoViewModel, $("#partyViewItemcontent-box").get(0));
}
