var userTaskMyViewModel;

function initialzieVm() {
    userTaskMyViewModel = {
        userTaskMyDetailsDTO: ko.observableArray([]),
        lastTaskId: ko.observable(''),
        lastCreatedAt: ko.observable(''),
        initialLoad: true,
        contentLoadTriggered: false,
        taskCount: ko.observable(0)

    };
}
function userTaskPoll() {
    console.log("Poll user task please");
    setInterval(function () {
        getDataMyUserTask();
    }, longPollFrequency);
}
function initialzieFirst() {
    initialzieVm();
    setupInitial();
    
}
function initializeusertask() {
    initialzieVm();
    setupUsertask();
}
function setupInitial() {
    ko.applyBindings(userTaskMyViewModel, $("#taskbadge")[0]);
    userTaskPoll();
    getDataMyUserTask();
}
function setupUsertask() {
    ko.applyBindings(userTaskMyViewModel, $("#myuserTaskcontent-box .panel-body")[0]);
    ko.applyBindings(peopleInfo, $("#myuserTaskcontent-box .panel-heading")[0]);
    getDataMyUserTask();

    $("#myuserTaskcontent-box .panel-body ul").perfectScrollbar(
    {
        suppressScrollX: true
    });

    $("#taskshowmore").on("click", function () {
        getDataMyUserTask();
    });

    $("#tasksrefresh").on("click", function () {
        refreshTaskList();
    });
}


function refreshTaskList() {
    userTaskMyViewModel.userTaskMyDetailsDTO.removeAll();
    userTaskMyViewModel.lastTaskId('');
    userTaskMyViewModel.lastCreatedAt('');
    userTaskMyViewModel.taskCount(0);
    getDataMyUserTask();
}

function voteOnTask(taskId) {
    voteViewClick("/UserVote/GetUserVote", voteCarosuelId, function () {
        getMyUserVoteingDetailsFortask(taskId);
        $('#myCarousel').carousel(voteCarosuelId);
    });
}

function getTax(taxId) {
    taxViewClick("/CountryTax/GetTaxDetails", taxCarosuelId, function () {
        getCountryTaxViewModel(taxId);
        $('#myCarousel').carousel(taxCarosuelId);
    });

}

function getBudget(taskId) {
    budgetViewClick("/CountryBudget/GetBudgetDetails", budgetCarosuelId, function () {
        getCountryBudgetViewModel(taskId);
        $('#myCarousel').carousel(budgetCarosuelId);
    });

}
function addTaskCss(userTask) {
    userTask.taskCss = ko.computed(function () {
        var result = "item-green";
        if (userTask.CompletionPercent == 0)
            result = "item-red";
        if (userTask.CompletionPercent > 0 && userTask.CompletionPercent < 51)
            result = "item-orange";
        if (userTask.CompletionPercent > 51)
            result = "item-green";
        return result;
    });

    userTask.dialColor = ko.computed(function () {
        var result = "#9ABC32";
        if (userTask.CompletionPercent == 0)
            result = "#D53F40";
        if (userTask.CompletionPercent > 0 && userTask.CompletionPercent < 51)
            result = "#E8B110";
        if (userTask.CompletionPercent > 51)
            result = "#9ABC32";
        return result;
    });

    userTask.flagCss = ko.computed(function () {
        var result = "fa-flag-o";
        if (userTask.Flagged == true)
            result = "fa-flag colorred";

        return result;
    });

    userTask.flagClicked = function (data, event) { //toggle Task Flag
        var self = $(event.currentTarget);
        var flag = userTaskMyViewModel.userTaskMyDetailsDTO()[self.data("index")].Flagged;
        if (flag) {
            flag = false;
            self.removeClass("fa-flag colorred");
            self.addClass("fa-flag-o");
        }
        else {
            flag = true;
            self.removeClass("fa-flag-o");
            self.addClass("fa-flag colorred");

        }
        userTaskMyViewModel.userTaskMyDetailsDTO()[self.data("index")].Flagged = flag;
        userTaskMyViewModel.userTaskMyDetailsDTO()[self.data("index")].Dirty = true;
    }

    userTask.trashClicked = function (data, event) { //RemoveTask
        var self = $(event.currentTarget);
        userTaskMyViewModel.userTaskMyDetailsDTO()[self.data("index")].Status = "D";
        userTaskMyViewModel.taskCount(userTaskMyViewModel.taskCount() - 1);
        self.parent().closest("li").fadeOut("slow", function () {
            var scrollHeight = $("#myuserTaskcontent-box .panel-body ul")[0].scrollHeight;
            var height = $("#myuserTaskcontent-box .panel-body ul").height();
            var topheight = (scrollHeight - height);
            $("#myuserTaskcontent-box .panel-body ul")
                .scrollTop(topheight -
                scrollHeight / userTaskMyViewModel.taskCount());

            $("#myuserTaskcontent-box .panel-body ul").perfectScrollbar('update');
        });
        userTaskMyViewModel.userTaskMyDetailsDTO()[self.data("index")].Dirty = true;


    }

    userTask.dueDateCss = ko.computed(function () {
        var result = "label-danger";
        var dateDue = new Date(userTask.DueDate);
        var today = new Date();

        var timeDiff = (dateDue.getTime() - today.getTime());
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

        if (diffDays > 3)
            result = "label-success";
        else if (diffDays > 1)
            result = "label-warning";
        return result;
    });
}
function getDataMyUserTask() {

    $.ajax({
        url: "/api/usertaskservice/gettasklist",
        type: "get",
        contentType: "application/json",
        data: {
            lastTaskId: userTaskMyViewModel.lastTaskId(),
            lastCreatedAt: userTaskMyViewModel.lastCreatedAt()
        },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    }).done(function (data) {
        if (data.length > 0) {
            userTaskMyViewModel.lastTaskId(data[data.length - 1].TaskId);
            userTaskMyViewModel.lastCreatedAt(data[data.length - 1].CreatedAt);
            userTaskMyViewModel.taskCount(userTaskMyViewModel.taskCount() + data.length);
            data.sort(function (l, r) {
                return l.Priority === r.Priority
                ? l.DueDate > r.DueDate ? 1 : -1
                : l.Priority > r.Priority ? 1 : -1
            })
            for (i = 0; i < data.length; i++) {
                data[i].Parms = processDateParm(data[i].Parms);
                processOnClickParm(data[i]);
                data[i].Description =
                    data[i].Description.format.apply(data[i].Description,
                    data[i].Parms.split("|"));
                data[i].Description = replaceAll(data[i].Description, ":::p-pic", profilepicContainer);
                addTaskCss(data[i]);
                userTaskMyViewModel.userTaskMyDetailsDTO.push(data[i]);
            }
            knobusertask();
            userTaskMyViewModel.contentLoadTriggered = false;
            if (userTaskMyViewModel.initialLoad == true) {
                $("#myuserTaskcontent-box").scrollTop(0);
                userTaskMyViewModel.initialLoad = false;
            }
        }
    });
}
function knobusertask() {
    $("#myuserTaskcontent-box .dial").knob(
      {
          'readOnly': true, 'min': 0
          , 'max': 100,
          'height': 36,
          'draw': function () {
              $(this.i).val(this.cv + '%')
          }
      });
}
