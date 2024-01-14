var eleCandidateViewModel;
var runForOffficeViewModel;
var runForOffficeViewModelResult;
var userpartyNames;
var politicalPosition;
var currentelectionterm;
var candidateView = "";
var eleresultview = "";
var runforofficeview = "";

function initializeElection() {

    initializeVoting();
    setupelevoting();

    initializeElectionRunForOffice();
    initializeCandidate();
    initializeElectionNavigation();
}
function initializeElectionNavigation() {
    $('#ele-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#ele-runoffice") {
            setupRunForOffice();
        }
        else if (self.attr('href') == "#ele-candidate") {
            if (eleCandidateViewModel.lastelectiondd().length == 0 || candidateView.length == 0) {
                showLoadingImage($("#ele-candidate"))
                $.when(getcandidateView(),
                getelectionlast12())
           .done(function (a1, a2) {
               eleCandidateViewModel.lastelectiondd(a2);
               candidateView = a1;
               $('#ele-candidate').html(candidateView[0]);
               for (var i = 0; i < eleCandidateViewModel.lastelectiondd()[0].length; i++) {
                   var eleobj = {
                       ElectionId: eleCandidateViewModel.lastelectiondd()[0][i],
                       ElectionName: "Election " + eleCandidateViewModel.lastelectiondd()[0][i]
                   };
                   eleCandidateViewModel.electionList.push(eleobj);
               }
               electiondropdownsetup();
               hideLoadingImage($("#ele-candidate"));
               setupelecandidate();
           });
            }
            else {
                $('#ele-candidate').html(candidateView[0]);
                electiondropdownsetup();
                setupelecandidate();
            }

            getelectionCandidates();// array max in dd

        }

    });
}

function initializeElectionRunForOffice() {
    runForOffficeViewModel = {
        userParty: ko.observable(''),
        uploadprofilePicObj: new uploadImageObj('partynew'),
        currentelectionId: ko.observable(),
        cash: ko.observable(userBankAccountViewModel.Cash()),
        StartDate: ko.observable(),
        VotingStartDate: ko.observable(),
        EndDate: ko.observable(),
        electiontermFee: ko.observable(),
        electionPositions: ko.observableArray(),
        runforSelected: ko.observable(0),
        runasSelected: ko.observable('I'),
        friendSelected: ko.observableArray(),
        friendDetailsDTO: ko.observableArray(),
        electionagendas: ko.observableArray(agendaCodeViewModel.agendaCodes.slice(0)),
        electionAgendasSelected: ko.observableArray([]),
        countCheck: function () {
            if (runForOffficeViewModel.electionAgendasSelected().length > 6) {
                runForOffficeViewModel.electionAgendasSelected.pop();
                return false;
            }
            return true;
        },
        clickelectionfriends: function (data, event) {
            var self = $(event.currentTarget);
            var inputcheck = self.children('input');
            var imgcheck = self.children('.img-check');

            if (inputcheck.prop('checked') == true) {
                inputcheck.prop('checked', false);
                imgcheck.css('opacity', '0.2');
                runForOffficeViewModel.friendSelected.pop(inputcheck.val());

            } else {
                inputcheck.prop('checked', true);
                imgcheck.css('opacity', '1');
                runForOffficeViewModel.friendSelected.push(inputcheck.val());
            }
            if (runForOffficeViewModel.friendSelected().length !=
                friendViewModel.friendDetailsDTO().length) {
                var $checkbox = $("#elefriendall");
                $checkbox.prop('checked', false);
                $button = $checkbox.siblings('button');
                updateDisplay($checkbox, $button);
            }


        },
    }
    runForOffficeViewModel.friendDetailsDTO =
        ko.utils.arrayFilter(friendViewModel.friendDetailsDTO(), function (item) {
            return item.CountryId === peopleInfo.CountryId();
        });
}
function setupRunForOffice() {
    showLoadingImage($("#ele-runoffice"));
    $.when(getpoliticalpostions(), getUserPartyData(), getCurrentElection(), getrunforOfficeView())
        .done(function (a1, a2, a3, a4) {
            politicalPosition = a1;
            userpartyNames = a2;
            currentelectionterm = a3;
            runforofficeview = a4;
            $('#ele-runoffice').html(runforofficeview[0]);
            runForOffficeViewModel.currentelectionId(currentelectionterm[0].ElectionId);
            runForOffficeViewModel.StartDate(currentelectionterm[0].StartDate);
            runForOffficeViewModel.VotingStartDate(currentelectionterm[0].VotingStartDate);
            runForOffficeViewModel.EndDate(currentelectionterm[0].EndDate);
            runForOffficeViewModel.electiontermFee(currentelectionterm[0].Fee);
            ko.mapping.fromJS(politicalPosition, {}, runForOffficeViewModel.electionPositions);
            runForOffficeViewModel.userParty(userpartyNames[0]);
            if (runForOffficeViewModel.userParty() == '') {
                $("#radioParty").prop("disabled", true)
            }
            ko.applyBindings(runForOffficeViewModel, $("#electionPositions").parent()[0]);
            var electionpostiondd = new DropDown($('#electionPositions'));
            electionpostiondd.opts.on('click', function () {
                runForOffficeViewModel.runforSelected($(this).data("id"));
            });
            applyKoRunForOffice();
            $("#applyelection").on('click', function () {
                applyforOffice($(this));
            });
            $('#candidateIndependent, #radioIndependet').on('click', function () {
                $("#radioIndependet").prop("checked", true)
                runForOffficeViewModel.runasSelected('I');

            });
            $('#candidateParty, #radioParty').on('click', function () {
                if ($("#radioParty").is(":disabled")) {
                    return false;
                }
                $("#radioParty").prop("checked", true)
                runForOffficeViewModel.runasSelected('P');
            });
            $('#electionPositions >span').removeClass("disableColor");
            $("#electionPositions ul, #electionAgenda, #electionfriends").perfectScrollbar(
                {
                    suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
                });

            setupcheckboxelectionfriend();
            $("#runofficefileupload").change(function (evt) {
                if (this.disabled) return alert('File upload not supported!');
                handleFileSelect(evt,
                    $("#elepicprw"), runForOffficeViewModel.uploadprofilePicObj, '');
            });
            hideLoadingImage($("#ele-runoffice"));
        });

}
function applyKoRunForOffice() {
    ko.applyBindings(runForOffficeViewModel, $("#candidateParty")[0]);
    ko.applyBindings(runForOffficeViewModel, $("#electionAgenda")[0]);
    ko.applyBindings(runForOffficeViewModel, $("#electionAgendaFooter")[0]);
    ko.applyBindings(runForOffficeViewModel, $("#eleFriends")[0]);
    ko.applyBindings(runForOffficeViewModel, $("#radioParty")[0]);
    ko.applyBindings(runForOffficeViewModel, $("#radioIndependet")[0]);
    ko.applyBindings(runForOffficeViewModel, $("#uploadrunoffice")[0]);
    ko.applyBindings(peopleInfo, $("#electionapply-box .panel-heading").get(0));
    ko.applyBindings(runForOffficeViewModel, $("#electionapply-box-footer").get(0));
}
function setupcheckboxelectionfriend() {
    var $widget = $("#elefriendall").parent(".button-checkbox"),
    $button = $widget.find('button'),
    $checkbox = $widget.find('input:checkbox');
    $checkbox.on('change', function () {
        updateDisplay($checkbox, $button);
        updateSelection(runForOffficeViewModel.friendSelected, $checkbox);
    });
    initilaizeUpdatedCheckbox($checkbox, $button);
}
function getUserPartyData() {
    if (typeof userpartyNames != 'undefined') {
        return userpartyNames;
    }
    return $.ajax({
        url: "/api/PartyService/GetUserPartyName",
        type: "get",
        contentType: "application/text",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getpoliticalpostions() {
    if (typeof politicalPosition != 'undefined') {
        return politicalPosition;
    }
    return $.ajax({
        url: "/api/electionservice/getpoliticalpostions",
        type: "get",
        contentType: "application/text",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function validateapplyelection() {
    if (
        runForOffficeViewModel.runforSelected() == 0 ||
        runForOffficeViewModel.runasSelected() == '' ||
        runForOffficeViewModel.electionAgendasSelected().length < 3 ||
        userBankAccountViewModel.Cash() < runForOffficeViewModel.electiontermFee()
        ) {
        return false;
    }

    if (
        runForOffficeViewModel.runasSelected() == 'I' &&
        runForOffficeViewModel.friendSelected().length < 10
        ) {
        return false;
    }
    return true;
}
function collectapplyPartyData() {
    var applyPartyData = {
        PositionTypeId: runForOffficeViewModel.runforSelected(),
        LogoPictureId: runForOffficeViewModel.uploadprofilePicObj.imageName,
        CandidateTypeId: runForOffficeViewModel.runasSelected(),
        FriendSelected: runForOffficeViewModel.friendSelected(),
        Agendas: runForOffficeViewModel.electionAgendasSelected(),
    }
    return applyPartyData;
}
function applyforOffice(btn) {
    if (validateapplyelection() == true) {
        var vm = runForOffficeViewModel.uploadprofilePicObj;
        uploadImage(vm, null);
        $.ajax({
            url: "/api/electionservice/applyforoffice",
            type: "post",
            contentType: "application/json",
            data: JSON.stringify(collectapplyPartyData()),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                var runForOffficeViewModelResult = ko.mapping.fromJS(result);
                if (result.StatusCode == 200) {
                    userNotificationPollOnce();
                    btn.addClass("hidden");
                    $("#electionapply-box").html("");
                    $("#electionapply-box").addClass("hidden");
                    $("#electionapply-submit").removeClass("hidden");
                    $("#electionapply-submit").next(".backbtn").removeClass("hidden");
                    ko.applyBindings(runForOffficeViewModelResult, $("#electionapply-submit").get(0));
                    $("#main").animate({ scrollTop: $("#electionapplycontainer").offset().top }, "slow");
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
function getCurrentElection() {
    if (typeof currentelectionterm != 'undefined') {
        return currentelectionterm;
    }
    return $.ajax({
        url: "/api/ElectionService/GetCurrentElectionTerm",
        type: "get",
        contentType: "application/json",
        data: { countyrId: peopleInfo.CountryId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getrunforOfficeView() {
    if (runforofficeview != "") {
        return runforofficeview;
    }
    return $.ajax("/election/getrunforofficeview");
}

function initializeRunforOfficeTicket() {
    runForOffficeViewModel = ko.observable();
    ko.applyBindings(peopleInfo, $("#electionticketcontainer .panel-heading").get(0));

}
function getRunforOfficeById(taskId) {
    $.ajax({
        url: "/api/ElectionService/GetRunForOfficeTicket",
        type: "get",
        contentType: "application/json",
        data: { taskId: taskId },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.LogoPictureId && result.LogoPictureId.indexOf(".") != -1) {
                result.LogoPictureId = partypicContainer + result.LogoPictureId;
            }
            else {
                result.LogoPictureId = "fa icon-man371  marginright6";
            }
            result.CandidateAgenda = ko.observableArray();
            for (var i = 0; i < result.CandidateAgendaId.length; i++) {
                var agendaobj = {
                    Description: agendaCodeViewModel.agendaCodes()[result.CandidateAgendaId[i] - 1].AgendaName(),
                }
                result.CandidateAgenda.push(agendaobj);
            }
            runForOffficeViewModel = ko.mapping.fromJS(result);
            ko.applyBindings(runForOffficeViewModel, $("#electionticketcontainer .panel-body").get(0));
        }
    });
}


function initializeCandidate() {
    eleCandidateViewModel = {
        contentLoadTriggered: false,
        electionId: ko.observable(''),
        candidates: ko.observableArray([]),
        filteredcandidates: ko.observableArray([]),
        AmountLeft: ko.observable(),
        lastelectiondd: ko.observableArray([]),
        electionList: ko.observableArray([]),
        aftereleCandidateRender: function (elem) {
            var self = $(elem[1]).find(".donateCash"); //index 1 as it has 3 array in elem
            var index = parseInt(self.data('index'));
            var btn = $(elem[1]).find(".donateCashbtn"); //index 1 as it has 3 array in elem
            btn.on('click', function () {
                donateeleCandidateCash(btn, index);
            });
            self.on('change', function () {
                var donationAmount = parseInt(self.val());
                updateelecanddateDonationvm(index, donationAmount);
            });
            spinInputwtPrefix(self, "Cash", userBankAccountViewModel.Cash());
        },
        quitElection: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            console.log(self);
            withdrawelection(index, self);
        },
        clickAgenda: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            var vm = eleCandidateViewModel.filteredcandidates()[index];
            if (vm.CandidateAgenda().length <= 0) {
                getcandidateAgenda(vm).success(function (data) {
                    processcandidateAgenda(data, vm);
                })
            }
        },
        clickResult: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data('index');
            var resultuser = eleCandidateViewModel.filteredcandidates()[index].UserId;
            var firstindex = arrayFirstIndexOf(eleCandidateViewModel.candidates(), function (item) {
                return item.ElectionId === eleCandidateViewModel.electionId()
                && item.UserId === resultuser;
            })
            console.log(firstindex);
            setupeleresult(firstindex, index, resultuser);
        },
    };
    eleCandidateViewModel.filteredcandidates =
    ko.computed(function () {
        return ko.utils.arrayFilter(eleCandidateViewModel.candidates(), function (item) {
            return item.ElectionId == eleCandidateViewModel.electionId();
        });
    });
}
function updateelecanddateDonationvm(index, donationAmount) {
    var leftCash = parseFloat(userBankAccountViewModel.Cash() - donationAmount).toFixed(2);
    eleCandidateViewModel.AmountLeft(leftCash);
    eleCandidateViewModel.filteredcandidates()[index].DonationInCash(donationAmount);
}
function donateeleCandidateCash(btn, index) {
    var donationDTO = {
        UserId: eleCandidateViewModel.filteredcandidates()[index].UserId,
        CountryId: peopleInfo.CountryId(),
        Amount: eleCandidateViewModel.filteredcandidates()[index].DonationInCash()
    }
    if (donationDTO.Amount <= 0) {
        return;
    }
    btn.button('loading');
    btn.addClass('disabled');
    $.ajax({
        url: "/api/ElectionService/DonateElection",
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
                eleCandidateViewModel.filteredcandidates()[index].DonationList.push(donationDTO);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {

        btn.button('reset')
    });
}

function getelectionlast12() {
    if (eleCandidateViewModel.lastelectiondd().length > 0) {
        return eleCandidateViewModel.lastelectiondd();
    }
    return $.ajax({
        url: "/api/electionservice/GetElectionLast12",
        type: "get",
        contentType: "application/json",
        data: {
            countryId: peopleInfo.CountryId()
        },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function electiondropdownsetup() {
    ko.applyBindings(eleCandidateViewModel,
        $("#electioncandidatedd .dropdown").get(0));

    var electionpostiondd = new DropDown($('#electioncandidatedd'));
    electionpostiondd.opts.on('click', function () {
        eleCandidateViewModel.electionId($(this).data("id"));
        getelectionCandidates($(this).data("index"));
    });
    $("#electioncandidatedd ul").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
        });
    var len = eleCandidateViewModel.electionList().length;
    electionpostiondd.setValue(
        eleCandidateViewModel.electionList()[len - 1].ElectionName,
        len - 1,
        eleCandidateViewModel.electionList()[len - 1].ElectionId
        );
    eleCandidateViewModel.electionId(eleCandidateViewModel.electionList()[len - 1].ElectionId);

}
function collectcandidateData(index) {
    var electionId = eleCandidateViewModel.electionId();
    if (electionId == '') {
        electionId = runForOffficeViewModel.currentelectionId();
    }
    var electionData = {
        CountryId: peopleInfo.CountryId(),
        ElectionId: electionId,
        LastPage: eleCandidateViewModel.filteredcandidates().length
    }
    return electionData;
}
function getelectionCandidates(index) {
    if (eleCandidateViewModel.contentLoadTriggered == true) {
        return;
    }
    eleCandidateViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/electionservice/getcandidatebyelection",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(collectcandidateData(index)),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#elecandidate-box .panel-heading").eq(0)),
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    if (result[i].LogoPictureId && result[i].LogoPictureId.indexOf(".") != -1) {
                        result[i].LogoPictureId = partypicContainer + result[i].LogoPictureId;
                    }
                    else {
                        result[i].LogoPictureId = "fa icon-man371  marginright6";
                    }
                    if (result[i].ElectionResult == "W") {
                        console.log(result[i].ElectionResult);
                        result[i].ElectionResultCss = "text-success";
                        result[i].ElectionResult = "Won"
                    }
                    else if (result[i].ElectionResult == "L") {
                        result[i].ElectionResultCss = "text-danger";
                        result[i].ElectionResult = "Lost"
                    }
                    else if (result[i].ElectionResult == "Q") {
                        result[i].ElectionResultCss = "text-warning";
                        result[i].ElectionResult = "Withdrawn"
                    }
                    else if (result[i].ElectionResult == "") {
                        result[i].ElectionResultCss = "text-mute";
                        result[i].ElectionResult = "Don't Know Yet"
                    }
                    result[i].DonationInCash = ko.observable(0);
                    result[i].DonationList = ko.observableArray();
                    result[i].EleResults = ko.observableArray();
                    result[i].CandidateAgenda = ko.observableArray();
                    eleCandidateViewModel.candidates.push(result[i]);
                }
                $("#electioncandidatedd ul").scrollTop(0);
                $("#electioncandidatedd ul").perfectScrollbar('update');
            }
        }
    }).always(function () {
        eleCandidateViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#elecandidate-box .panel-heading").eq(0));
    });

}
function setupelecandidate() {
    ko.applyBindings(eleCandidateViewModel,
    $("#elecandidate-box .candidates").get(0));
    ko.applyBindings(eleCandidateViewModel,
        $("#elecandidate-box-footer").get(0));
    ko.applyBindings(peopleInfo, $("#elecandidate-box .panel-heading").get(0));

    $("#elecandidate-box .candidates").perfectScrollbar(
    {
        suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10,
        includePadding: true
    });
    $("#candidateshowmore").on('click', function () {
        var index = eleCandidateViewModel.lastelectiondd()[0].indexOf
              (eleCandidateViewModel.electionId());
        getelectionCandidates(index);
    });

}
function getcandidateView() {
    if (candidateView != "") {
        return candidateView;
    }
    return $.ajax("/election/getelectioncandidateview");
}
function getcandidateAgenda(vm) {
    if (vm.CandidateAgenda().length > 0) {
        return;
    }
    return $.ajax({
        url: "/api/ElectionService/GetCandidateAgenda",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ userId: vm.UserId, electionId: vm.ElectionId }),
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function processcandidateAgenda(data, vm) {
    if (data.length > 0) {
        for (var j = 0; j < data.length; j++) {
            var agendaobj = {
                Description: agendaCodeViewModel.agendaCodes()[data[j] - 1].AgendaName(),
                AgendaTypeId: data[j]
            }
            vm.CandidateAgenda.push(agendaobj);
        }
    }
}
function setupeleresult(index, viewindex, resultuser) {
    if (eleCandidateViewModel.candidates()[index].EleResults().length <= 0) {
        $.when(geteleresultview(), getuserelectionresult(index, resultuser))
        .done(function (a1, a2) {
            for (var i = 0; i < a2[0].length; i++) {
                if (a2[0][i].ElectionResult == "W") {
                    console.log(a2[0][i].ElectionResult);
                    a2[0][i].ElectionResultCss = "text-success";
                    a2[0][i].ElectionResult = "Won"
                }
                else if (a2[0][i].ElectionResult == "L") {
                    a2[0][i].ElectionResultCss = "text-danger";
                    a2[0][i].ElectionResult = "Lost"
                }
                else if (a2[0][i].ElectionResult == "Q") {
                    a2[0][i].ElectionResultCss = "text-warning";
                    a2[0][i].ElectionResult = "Withdrawn"
                }
                else if (a2[0][i].ElectionResult == "") {
                    a2[0][i].ElectionResultCss = "text-mute";
                    a2[0][i].ElectionResult = "Don't Know Yet"
                }
            }
            eleresultview = a1;
            eleCandidateViewModel.candidates()[index].EleResults = ko.mapping.fromJS(a2[0]);
            $("#eleresult" + viewindex).html(eleresultview[0]);
            ko.applyBindings(eleCandidateViewModel.candidates()[index],
                    $("#eleresult" + viewindex).find('div').get(0));


        });
    }

}
function geteleresultview() {
    if (eleresultview != "") {
        return eleresultview;
    }
    return $.ajax("/election/getelectionresultview");
}
function getuserelectionresult(index, resultuser) {
    if (eleCandidateViewModel.candidates()[index].EleResults().length > 0) {
        return;
    }
    return $.ajax({
        url: "/api/electionservice/getelectionresult",
        type: "get",
        contentType: "application/text",
        data: { candidate: resultuser },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function withdrawelection(index, btn) {
    btn.button('loading');
    if (eleCandidateViewModel.contentLoadTriggered == true) {
        return;
    }
    eleCandidateViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/ElectionService/QuitElection",
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
                console.log(index);
                eleCandidateViewModel.filteredcandidates()[index].Status = "Q";
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        eleCandidateViewModel.contentLoadTriggered = false;
    });
}


function initializeVoting() {
    eleVotingViewModel = {
        contentLoadTriggered: false,
        candidates: ko.observableArray([]),
        positionTypeId: ko.observable('1'),
        electionPositions: ko.observableArray(),
        votingCandidates: ko.observableArray([]),
        countCheck: function () {
            if (eleVotingViewModel.votingCandidates().length > 5) {
                eleVotingViewModel.votingCandidates.pop();
                return false;
            }
            return true;
        }
    };

}
function setupelevoting() {
    ko.applyBindings(eleVotingViewModel, $("#elevoting-box .candidates").get(0));
    ko.applyBindings(eleVotingViewModel, $("#elevoting-box .candidates").get(1));
    getelectionVoting();
    $.when(getpoliticalpostions())
     .done(function (a1) {
         politicalPosition = a1
         for (var i = 0; i < politicalPosition.length; i++) {
             eleVotingViewModel.electionPositions.push(politicalPosition[i]);
         }
         ko.applyBindings(eleVotingViewModel, $("#elevoting-box .electionPositions").parent()[0]);
         var electionpostiondd = new DropDown($('#elevoting-box .electionPositions'));
         electionpostiondd.opts.on('click', function () {
             eleVotingViewModel.positionTypeId($(this).data("id"));
             getelectionVoting();
         });
     });

    ko.applyBindings(peopleInfo, $("#elevoting-box .panel-heading").get(0));
    ko.applyBindings(eleVotingViewModel, $("#elevoting-box-footer").get(0));

    $("#elevote").on('click', function () {
        voteelection($(this));
    });
}
function getelectionVoting() {
    if (eleVotingViewModel.contentLoadTriggered == true) {
        return;
    }

    eleVotingViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/electionservice/getcurrentvotinginfo",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(collectvotingData()),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#elevoting-box .panel-heading").eq(0)),
        success: function (result) {
            if (result.length > 0) {
                eleVotingViewModel.candidates.removeAll();
                for (var i = 0; i < result.length; i++) {
                    if (result[i].LogoPictureId && result[i].LogoPictureId.indexOf(".") != -1) {
                        result[i].LogoPictureId = partypicContainer + result[i].LogoPictureId;
                    }
                    else {
                        result[i].LogoPictureId = "fa icon-man371  marginright6";
                    }
                    eleVotingViewModel.candidates.push(result[i]);
                }
            }

            $("#elevoting-box .candidates").perfectScrollbar(
    {
        suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10,
        includePadding: true
    });
        }
    }).always(function () {
        eleVotingViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#elevoting-box .panel-heading").eq(0));
    });

}
function collectvotingData() {
    var electionData = {
        CountryId: peopleInfo.CountryId(),
        PositionTypeId: eleVotingViewModel.positionTypeId(),
    }
    return electionData;
}
function collectvotersData() {
    var electionData = {
        CountryId: peopleInfo.CountryId(),
        PositionTypeId: eleVotingViewModel.positionTypeId(),
        Candidates: eleVotingViewModel.votingCandidates()
    }
    return electionData;
}
function voteelection(btn) {
    btn.button('loading');
    if (eleVotingViewModel.contentLoadTriggered == true) {
        return;
    }
    eleVotingViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/ElectionService/VoteElection",
        type: "post",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(collectvotersData()),
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var elevotingViewModelResult = ko.mapping.fromJS(result);
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#elevoting-box").html("");
                $("#elevoting-box").addClass("hidden");
                $("#elevoting-submit").removeClass("hidden");
                $("#elevoting-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(elevotingViewModelResult, $("#elevoting-submit").get(0));
                $("#main").animate({ scrollTop: $("#elevotingcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        eleVotingViewModel.contentLoadTriggered = false;
    });
}