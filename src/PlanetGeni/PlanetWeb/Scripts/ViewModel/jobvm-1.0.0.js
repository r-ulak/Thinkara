var jobSearchViewModel;
var jobSearchViewModelresult;
var myJobViewModel;
var myJobViewModelresult;
var jobHistoryViewModel;
var jobHistoryViewModelresult;
var jobSummaryViewModel;
var jobSearchView;
var myJobView;
var jobHistoryView;
var jobSummaryView;
function initializeJob() {
    jobSearchView = $("#jobsearchcontainer").html();
    myJobView = $("#myjobcontainer").html();
    jobHistoryView = $("#jobhistorycontainer").html();
    jobSummaryView = $("#jobSummarycontainer").html();

    initializeJobNavigation();
    initializeJobSearch();
    setupJobSearch();
    initializeMyJob();
    setupMyJob();
    initializeJobHistory();
    setupJobHistory();
    initializeJobSummary();
    setupJobSummary();

}
function initializeJobNavigation() {
    $('#job-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#job-summary") {
            if (jobSummaryViewModel.jobTopN().length == 0
                 && jobSummaryViewModel.contentLoadTriggered == false) {
                jobSummaryViewModel.contentLoadTriggered = true;
                showLoadingImage($("#myjobsummarycontent-box .panel-body"))
                $.when(getJobSummary(), getTopNJobSalary())
                     .done(function (a1, a2) {
                         if (a2[0].length > 0) {
                             for (var i = 0; i < a2[0].length; i++) {
                                 a2[0][i].Rank = ordinal_suffix_of(i + 1);
                                 jobSummaryViewModel.jobTopN.push(a2[0][i]);
                             }
                         }
                         var userJobSummaryViewModel;
                         userJobSummaryViewModel = ko.mapping.fromJS(a1[0]);
                         ko.applyBindings(userJobSummaryViewModel, $("#jobSummarycontent-box").get(0));
                         $("#jobSummarycontent-box").removeClass('hidden');
                         jobSummaryViewModel.contentLoadTriggered = false;
                         hideLoadingImage($("#myjobsummarycontent-box .panel-body"))
                     });
            }
        }
        else if (self.attr('href') == "#job-myjobs") {
            if (myjobViewModel.jobCodes().length == 0) {
                getMyJob();
            }
        }
        else if (self.attr('href') == "#job-history") {
            if (jobHistoryViewModel.jobCodes().length == 0) {
                getJobHistory();
            }
        }
    })
}


function initializeJobSearch() {
    $("#jobsearchcontainer").html(jobSearchView);
    jobSearchViewModel = {
        jobCodes: ko.observableArray(),
        lastJobCodeId: ko.observable(0),
        applyingItems: ko.observableArray(),
        contentLoadTriggered: false
    }
    jobSearchViewModel.showApplyValidation = ko.computed(function () {
        if (jobSearchViewModel.applyingItems().length > 0) {
            return false
        } else {
            return true;
        }
    });

}
function setupJobSearch() {
    $('#applyjobsearch').click(function () {
        var btn = $(this);
        btn.button('loading');
        applyJob(btn, 'new');
    });
    $('#jobmorejobsearch').click(function () {
        initializeJobSearch();
        setupJobSearch();
    });
    $('#searchJob').click(function () {
        var btn = $(this);
        btn.button('loading');
        getJobSearch(btn, 'new');
    });
    applyKoJobSearch();
    $("#jobSearch .panel-body, #jobSearchResultItemcontent-box, #jobIndustry, #jobMajorCodes").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10, alwaysVisibleY: true
        });
    $('.accordionjobsearch').on('shown.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').addClass('active');
        //$(e.target).animate({ height: "390" }, 600);
    });

    $("#jobCodesshowmore").on("click", function () {
        var btn = $(this)
        btn.button('loading')
        getJobSearch(btn, 'append');
    });
    $('.accordionjobsearch').on('hidden.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').removeClass('active');
    });
}
function applyKoJobSearch() {
    ko.applyBindings(industryCodeViewModel, $("#jobindustryCodes").get(0));
    ko.applyBindings(majorCodeViewModel, $("#jobMajorCodes").get(0));
    ko.applyBindings(jobSearchViewModel, $("#jobSearchResultcontent-wrapper").get(0));
    ko.applyBindings(jobSearchViewModel, $("#jobSearchResult .panel-footer").get(0));
    ko.applyBindings(jobSearchViewModel, $("#applyjobsearch").get(0));
    ko.applyBindings(peopleInfo, $("#myjobsearchcontent-box .panel-heading").get(0));
}
function collectJobSearchData() {
    var jobtype = $('#searchjobtype input[name=searchjobtype]:checked').map(function () {
        return $(this).val();
    }).get();
    var overtime = $('#searchjobtype input[name=overtimejobtype]:checked').map(function () {
        return parseInt($(this).val());
    }).get();
    var lrange = 0;
    var hrange = 0;
    var salaryRange = $('#salaryRange input[name=salaryRange]:checked').map(function () {
        lrange = $(this).data("lrange");
        hrange = $(this).data("hrange");
        return $(this).val();
    }).get();
    var industry = $('#jobIndustry input[name=jobIndustry]:checked').map(function () {
        return parseInt($(this).val());
    }).get();

    if (industry.length == 0) {
        industry = $('#jobIndustry input[name=jobIndustry]:not(:checked)').map(function () {
            return parseInt($(this).val());
        }).get();
    }

    var major = $('#jobMajorCodes input[name=jobMajor]:checked').map(function () {
        return parseInt($(this).val());
    }).get();

    if (major.length == 0) {
        major = $('#jobMajorCodes input[name=jobMajor]:not(:checked)').map(function () {
            return parseInt($(this).val());
        }).get();
    }

    if (jobtype.length == 0) {
        jobtype = $('#searchjobtype input[name=searchjobtype]:not(:checked)').map(function () {
            return $(this).val();
        }).get();
    }
    if (overtime.length == 0) {
        overtime[0] = -1;
    }
    var jobCodes = {
        Industry: industry,
        Major: major,
        JobType: jobtype,
        SalaryLowerRange: lrange,
        SalaryHigherRange: hrange,
        LastJobCodeId: jobSearchViewModel.lastJobCodeId(),
        OverTime: overtime[0]
    }
    return jobCodes;
}
function getJobSearch(btn, searchType) {
    if (jobSearchViewModel.contentLoadTriggered == true) {
        return;
    }
    jobSearchViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/jobservice/SearchJob",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(collectJobSearchData()),
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#myjobsearchcontent-box .panel-heading").eq(0)),
        success: function (result) {
            if (searchType == 'new') {
                jobSearchViewModel.jobCodes.removeAll();
                jobSearchViewModel.lastJobCodeId(0);
                console.log("test string");
            }
            if (result.length > 0) {

                for (i = 0; i < result.length; i++) {
                    processJobSearchResult(result[i]);
                    jobSearchViewModel.jobCodes.push(result[i]);
                }
                console.log("test jobSearchViewModel");
                jobSearchViewModel.lastJobCodeId(result[result.length - 1].JobCodeId);
            }
            $("#jobSearchResult").collapse('show');
            $("#jobSearch").collapse('hide');
        }
    }).always(function () {
        btn.button('reset')
        jobSearchViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#myjobsearchcontent-box .panel-heading").eq(0));
    });
}
function processJobSearchResult(item) {
    var result = "FullTime";
    if (item.JobType == 'P') {
        item.JobTypeName = "PartTime";
    }
    else if (item.JobType == 'F') {
        item.JobTypeName = "FullTime";
    } else if (item.JobType == 'C') {
        item.JobTypeName = "Contract";
    }

    console.log("item jobType");
    console.log(item.JobType);
    console.log(item.JobTypeName);


}
function applyJobCode(jobcodeId) {
    this.JobCodeId = jobcodeId;
}
function applyJob(btn) {
    if (jobSearchViewModel.applyingItems().length == 0) {
        return;
    }
    if (jobSearchViewModel.contentLoadTriggered == true) {
        return;
    }
    jobSearchViewModel.contentLoadTriggered = true;
    var applyJobList = new Array();
    for (var i = 0; i < jobSearchViewModel.applyingItems().length; i++) {
        console.log(jobSearchViewModel.applyingItems()[i]);
        applyJobList.push(new applyJobCode(jobSearchViewModel.applyingItems()[i]));
    }
    console.log(applyJobList[0]);
    $.ajax({
        url: "/api/JobService/SaveApplyJobs",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(applyJobList),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            jobSearchViewModelresult = ko.mapping.fromJS(result);
            if (jobSearchViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#myjobsearchcontent-box").html("");
                $("#myjobsearchcontent-box").addClass("hidden");
                $("#jobmorejobsearch").removeClass("hidden");
                $("#myjobsearchcontent-submit").removeClass("hidden");
                $("#myjobsearchcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(jobSearchViewModelresult, $("#myjobsearchcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#jobsearchcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        jobSearchViewModel.contentLoadTriggered = false;
        btn.button('reset')
    });
}

function initializeMyJob() {
    $("#myjobcontainer").html(myJobView);
    myjobViewModel = {
        jobCodes: ko.observableArray(),
        quitingItems: ko.observableArray(),
        contentLoadTriggered: false,
        CheckInOverTime: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            self.button('loading')
            var knobinput = self.closest('.productbox').find('input.dial');
            applyCheckInOverTime(index);
        }
    }
    myjobViewModel.showQuitJobValidation = ko.computed(function () {
        if (myjobViewModel.quitingItems().length > 0) {
            return false
        } else {
            return true;
        }
    });


}
function quitJob(btn) {
    if (myjobViewModel.quitingItems().length == 0) {
        return;
    }
    if (myjobViewModel.contentLoadTriggered == true) {
        return;
    }
    myjobViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/JobService/QuitJob",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(myjobViewModel.quitingItems()),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            myJobViewModelresult = ko.mapping.fromJS(result);
            if (myJobViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#jobMycontent-box").html("");
                $("#jobMycontent-box").addClass("hidden");
                $("#quitmorejob").removeClass("hidden");
                $("#jobMycontent-submit").removeClass("hidden");
                $("#jobMycontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(myJobViewModelresult, $("#jobMycontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#myjobcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        myjobViewModel.contentLoadTriggered = false;
        btn.button('reset')
    });
}
function refreshMyJob() {
    myjobViewModel.jobCodes.removeAll();
    console.log(myjobViewModel.jobCodes().length);
    getMyJob();
}
function setupMyJob() {
    $('#jobrefresh').click(function () {
        refreshMyJob();
    });
    $('#quitjob').click(function () {
        var btn = $(this);
        btn.button('loading');
        quitJob(btn);
    });
    $('#quitmorejob').click(function () {
        initializeMyJob();
        setupMyJob();
        getMyJob();
    });
    applyKoMyJob();
    $("#jobMyItemcontent-box").perfectScrollbar(
{
    suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10,
    alwaysVisibleY: true
});
}
function applyKoMyJob() {
    ko.applyBindings(myjobViewModel, $("#jobMycontent-box .panel-body").get(0));
    ko.applyBindings(myjobViewModel, $("#jobMycontent-box .panel-footer").get(0));
    ko.applyBindings(myjobViewModel, $("#quitjob").get(0));
    ko.applyBindings(peopleInfo, $("#jobMycontent-box .panel-heading").get(0));
}
function getMyJob() {
    if (myjobViewModel.contentLoadTriggered == true) {
        return;
    }
    myjobViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/jobservice/getcurrentjobs",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#jobMycontent-box .panel-body")),
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    processmyJob(result[i], i);
                    if (result[i].CompletionPercent > 100) {
                        console.log("CompletionPercent " + result[i].CompletionPercent);
                        result.splice(i, 1);
                        i--;
                    }
                }
                ko.mapping.fromJS(result, {}, myjobViewModel.jobCodes);

                addmyjobKnob();
            }
        }
    }).always(function () {
        myjobViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#jobMycontent-box .panel-body"));
    });
}
function addmyjobKnob() {
    console.log("ddd");
    $("#jobMycontent-wrapper input.dial").knob(
   {
       'readOnly': true,
       'min': 0,
       'max': 100,
       'height': 100,
       'width': 100,
       'draw': function () {
           $(this.i).val(this.cv + '%')
       }
   });
}
function processmyJob(item, index) {
    item.CompletionPercent = 100;
    console.log(item.CompletionPercent);
    var startDate = moment.utc(item.StartDate);
    var currentDate = moment.utc();
    var completedDuration = currentDate.diff(startDate, 'hours');

    item.CompletionPercent = parseInt(completedDuration / item.Duration * 100);
    console.log(completedDuration);
    console.log(item.CompletionPercent);

    var result = "#9ABC32";
    if (item.CompletionPercent < 50)
        result = "#9ABC32";
    else if (item.CompletionPercent >= 50 && item.CompletionPercent < 80)
        result = "#E8B110";
    else if (item.CompletionPercent >= 80)
        result = "#D53F40";
    item.KnobColor = result;
}
function applyCheckInOverTime(index) {

    $.ajax({
        url: "/api/jobservice/jobovertimecheckin",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify({ TaskId: (myjobViewModel.jobCodes()[index].TaskId()) }),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            myjobViewModel.jobCodes()[index].NextOverTimeCheckIn(result.NextOverTimeCheckIn);
        }
    });
}

function initializeJobHistory() {
    $("#jobhistorycontainer").html(jobHistoryView);
    jobHistoryViewModel = {
        jobCodes: ko.observableArray(),
        parmLastDateTime: ko.observable(),
        jobHistoryWithdrawList: ko.observableArray(),
        contentLoadTriggered: false
    }
    jobHistoryViewModel.showHistoryValidation = ko.computed(function () {
        if (jobHistoryViewModel.jobHistoryWithdrawList().length > 0) {
            return false
        } else {
            return true;
        }
    });

}
function setupJobHistory() {
    $('#jobhistoryrefresh').click(function () {
        refreshJobHistory();
    });
    $('#historyjob').click(function () {
        var btn = $(this);
        btn.button('loading');
        withDrawJob(btn);
    });
    $("#jobhistoryshowmore").on("click", function () {
        getJobHistory();
    });
    $("#historymorejob").on("click", function () {
        initializeJobHistory();
        setupJobHistory();
        getJobHistory();
    });
    applyKoJobHistory();
    $("#jobHistoryItemcontent-box").perfectScrollbar(
            {
                suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10,
                alwaysVisibleY: true
            });
}
function applyKoJobHistory() {
    ko.applyBindings(jobHistoryViewModel, $("#jobHistorycontent-box .panel-body").get(0));
    ko.applyBindings(jobHistoryViewModel, $("#jobHistorycontent-box .panel-footer").get(0));
    ko.applyBindings(jobHistoryViewModel, $("#historyjob").get(0));
    ko.applyBindings(peopleInfo, $("#jobHistorycontent-box .panel-heading").get(0));
}
function getJobHistory() {
    if (jobHistoryViewModel.contentLoadTriggered == true) {
        return;
    }
    jobHistoryViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/jobservice/GetJobHistory",
        type: "get",
        contentType: "application/json",
        data: { parmlastDateTime: jobHistoryViewModel.parmLastDateTime() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#jobHistorycontent-box .panel-heading")),
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    processJobSearchResult(result[i]);
                    jobHistoryViewModel.jobCodes.push(result[i]);
                }
                jobHistoryViewModel.parmLastDateTime(result[result.length - 1].UpdatedAt);
            }
        }
    }).always(function () {
        jobHistoryViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#jobHistorycontent-box .panel-heading"));
    });
}
function refreshJobHistory() {
    jobHistoryViewModel.jobCodes.removeAll();
    jobHistoryViewModel.parmLastDateTime(null);
    console.log(jobHistoryViewModel.jobCodes().length);
    getJobHistory();
}
function withDrawJob(btn) {
    console.log(btn);
    if (jobHistoryViewModel.jobHistoryWithdrawList().length == 0) {
        return;
    }
    if (jobHistoryViewModel.contentLoadTriggered == true) {
        return;
    }
    jobHistoryViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/JobService/WithDrawJob",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(jobHistoryViewModel.jobHistoryWithdrawList()),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            jobHistoryViewModelresult = ko.mapping.fromJS(result);
            if (jobHistoryViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#jobHistorycontent-box").html("");
                $("#jobHistorycontent-box").addClass("hidden");
                $("#historymorejob").removeClass("hidden");
                $("#jobHistorycontent-submit").removeClass("hidden");
                $("#jobHistorycontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(jobHistoryViewModelresult, $("#jobHistorycontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#jobhistorycontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        jobHistoryViewModel.contentLoadTriggered = false;
        btn.button('reset')
    });
}


function initializeJobSummary() {
    $("#jobSummarycontainer").html(jobSummaryView);
    jobSummaryViewModel = {
        contentLoadTriggered: false,
        jobTopN: ko.observableArray()

    }
}
function setupJobSummary() {
    applyKoJobSummary();
}
function applyKoJobSummary() {
    ko.applyBindings(peopleInfo, $("#jobSummarycontainer .panel-heading").get(0));
    ko.applyBindings(jobSummaryViewModel, $("#jobTopNSummary").get(0));
}
function getJobSummary() {
    return $.ajax({
        url: "/api/jobservice/GetJobSummaryDTO",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getTopNJobSalary() {
    return $.ajax({
        url: "/api/jobservice/gettoptenincomesalary",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}