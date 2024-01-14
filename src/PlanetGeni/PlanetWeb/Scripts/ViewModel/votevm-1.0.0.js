var viewModelUserVote;
var userVoteDetailsViewModelresult;
function initializeVote() {
    $('#saveVoteResponse').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveVoteResponse(btn);

    });
}
function saveVoteResponse(btn) {
    if (viewModelUserVote.choiceSelected().length < 0) {
        console.log(viewModelUserVote.choiceSelected().length);
        return;
    }

    $.ajax({
        url: "/api/uservoteservice/savevoteresponse",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(
            {
                TaskId: viewModelUserVote.TaskId(),
                TaskTypeId: viewModelUserVote.TaskTypeId(),
                ChoiceIds: viewModelUserVote.choiceSelected(),
                ChoiceRadioId: viewModelUserVote.choiceSelected()
            }
            ),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            userVoteDetailsViewModelresult = ko.mapping.fromJS(result);
            if (userVoteDetailsViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#myuserVotingcontent-box").html("");
                $("#myuserVotingcontent-box").addClass("hidden");
                $("#myuserVotingcontent-submit").removeClass("hidden");
                $("#myuserVotingcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(userVoteDetailsViewModelresult, $("#myuserVotingcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#userVotingcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function getMyUserVoteingDetailsFortask(taskId) {
    viewModelUserVote = null;

    $.ajax({
        url: "/api/uservoteservice/getvotingdetails",
        type: "get",
        contentType: "application/json",
        data: { taskId: taskId },
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    }).done(function (data) {
        if (data) {
            data.Parms = processDateParm(data.Parms);
            processOnClickParm(data);
            data.Description =
                data.Description.format.apply(data.Description,
                data.Parms.split("|"));
            data.Description = replaceAll(data.Description, ":::p-pic", profilepicContainer);

            viewModelUserVote = ko.mapping.fromJS(data);
            viewModelUserVote.choiceSelected = ko.observableArray([]);
            viewModelUserVote.TaskId = ko.observable(taskId);
            viewModelUserVote.choiceClicked = function () {
                if (viewModelUserVote.choiceSelected().length >
                    viewModelUserVote.MaxChoiceCount() &&
                    viewModelUserVote.ChoiceType() == 0) {
                    viewModelUserVote.choiceSelected.pop();

                    $("#userVoteFooter span").addClass("text-danger");
                    $("#userVoteFooter div span").addClass("text-danger");
                    return false;
                }
                $("#userVoteFooter span").removeClass("text-danger");
                $("#userVoteFooter div span").removeClass("text-danger");
                return true;
            };

            viewModelUserVote.showValidation = ko.computed(function () {
                if (viewModelUserVote.choiceSelected().length > 0)
                { return false; }
                else {
                    return true;
                }
                consoloe.log("showValidation")
            });
            ko.applyBindings(viewModelUserVote, $("#myuserVotingcontent-box")[0]);
            ko.applyBindings(viewModelUserVote, $("#saveVoteResponse").get(0));

        }
    });

}