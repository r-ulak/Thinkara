var educationEnrollViewModel;
var educationDetailsViewModelresult;
var educationEnrollViewModel;
var educationEnrollViewModelresult;
var educationSummaryViewModel;
var enrollEducationView;
var enrolledEducationView;
var summaryEducationView;
var completedEducationView;
var cachedegreeCodes = [];
var cacheenrolledDegree = [];
function initializeeducation() {
    enrolledEducationView = $("#enrollededucationcontainer").html();
    enrollEducationView = $("#enrolleducationcontainer").html();
    summaryEducationView = $("#educationSummarycontainer").html();
    completedEducationView = $("#completeddegreecontainer").html();

    initializeeducationNavigation();
    initializeEducationEnroll();
    initializeEducationEnrolled();
    initializeEducationSummary();
    initializeEducationCompleted();
    setupEducationEnroll();

}
function initializeeducationNavigation() {
    $('#edu-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#edu-summary") {
            if (educationSummaryViewModel.educationTopN().length == 0
                         && educationSummaryViewModel.contentLoadTriggered == false) {
                educationSummaryViewModel.contentLoadTriggered = true;
                setupEducationSummary();
                showLoadingImage($("#myeducationsummarycontent-box .panel-body"))
                $.when(getEducationSummary(), getTopNDegreeOwner())
                     .done(function (a1, a2) {
                         if (a2[0].length > 0) {
                             for (var i = 0; i < a2[0].length; i++) {
                                 a2[0][i].Rank = ordinal_suffix_of(i + 1);
                                 educationSummaryViewModel.educationTopN.push(a2[0][i]);
                             }
                         }
                         if (a1[0].length > 0) {
                             for (var i = 0; i < a1[0].length; i++) {
                                 educationSummaryViewModel.educationSummaryDTO.push(a1[0][i]);
                             }
                         }
                         educationSummaryViewModel.contentLoadTriggered = false;
                         hideLoadingImage($("#myeducationsummarycontent-box .panel-body"))
                     });
            }
        }
        else if (self.attr('href') == "#edu-enrolled") {
        }
        else if (self.attr('href') == "#edu-completed") {
        }
    })
}


function initializeEducationSummary() {
    $("#educationSummarycontainer").html(summaryEducationView);
    educationSummaryViewModel = {
        contentLoadTriggered: false,
        educationSummaryDTO: ko.observableArray(),
        educationTopN: ko.observableArray()
    };
    educationSummaryViewModel.DegreeScore = ko.computed(function () {
        var result = 0;
        for (var i = 0; i < educationSummaryViewModel.educationSummaryDTO().length; i++) {
            result = result + educationSummaryViewModel.educationSummaryDTO()[i].Total *
                 educationSummaryViewModel.educationSummaryDTO()[i].DegreeRank;
        }
        return result;
    });
}
function setupEducationSummary() {
    ko.applyBindings(peopleInfo, $("#myeducationsummarycontent-box .panel-heading").get(0));
    ko.applyBindings(educationSummaryViewModel, $("#educationSummaryInprogresscontent").get(0));
    ko.applyBindings(educationSummaryViewModel, $("#educationSummaryCompletedcontent").get(0));
    ko.applyBindings(educationSummaryViewModel, $("#degreeScore").get(0));
    ko.applyBindings(educationSummaryViewModel, $("#educationTopNSummary").get(0));
}
function getEducationSummary() {
    return $.ajax({
        url: "/api/educationservice/geteducationsummary",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getTopNDegreeOwner() {
    return $.ajax({
        url: "/api/educationservice/gettoptendegreeholder",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}


function initializeEducationEnrolled() {
    $("#enrollededucationcontainer").html(enrolledEducationView);
    applyKoEducationEnrolled();
    $('#educationrefresh').click(function () {
        refreshEducationEnrolled();
    });
    $("#educationEnrolledItemcontent-box").perfectScrollbar(
    {
        suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10, alwaysVisibleY: true
    });
}
function applyKoEducationEnrolled() {
    ko.applyBindings(educationEnrollViewModel, $("#educationEnrolledcontent-box .panel-body").get(0));
    ko.applyBindings(educationEnrollViewModel, $("#educationEnrolledcontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#educationEnrolledcontent-box .panel-heading").get(0));
}
function addeducationInvItemCss(completionpercent) {
    var result = "#9ABC32";
    if (completionpercent < 50)
        result = "#D53F40";
    else if (completionpercent >= 50 && completionpercent < 80)
        result = "#E8B110";
    else if (completionpercent >= 80)
        result = "#9ABC32";
    return result;
}
function refreshEducationEnrolled() {
    getallEducationcodes(true, function () {
        addeducationKnob();
    });

}
function loadEnrolledDegreeInfo() {
    for (var i = 0; i < educationEnrollViewModel.userEducationDegreeCodes().length ; i++) {
        loadEnrolledDegreeInfoByIndex(i);
    }
}
function loadEnrolledDegreeInfoByIndex(i) {
    var createdate = moment.utc(educationEnrollViewModel.userEducationDegreeCodes()[i].CreatedAt());
    var expectedAt = moment.utc(educationEnrollViewModel.userEducationDegreeCodes()[i].ExpectedCompletion());
    var totalDuration = parseFloat(expectedAt.diff(createdate, 'seconds'));
    var completionpercent = parseFloat(moment.utc().diff(createdate, 'seconds'));
    completionpercent = parseInt((completionpercent / totalDuration) * 100);
    educationEnrollViewModel.userEducationDegreeCodes()[i].CompletionPercent(completionpercent);
    educationEnrollViewModel.userEducationDegreeCodes()[i].KnobColor(addeducationInvItemCss(completionpercent));
    var degreeId = educationEnrollViewModel.userEducationDegreeCodes()[i].DegreeId();
    educationEnrollViewModel.userEducationDegreeCodes()[i].DegreeTextFont(degreeFonttext(degreeId));
    educationEnrollViewModel.userEducationDegreeCodes()[i].CertificateTextFont(certificateFonttext(degreeId));


}
function addeducationKnob() {
    $("#educationEnrolledcontent-wrapper input.dial").knob(
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
function ApplyBoostTime(index, btn, knobinput) {
    $.ajax({
        url: "/api/educationservice/applyboosttime",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(educationEnrollViewModel.userEducationDegreeCodes()[index]),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            educationEnrollViewModel.userEducationDegreeCodes()[index].Status(result.Status);
            educationEnrollViewModel.userEducationDegreeCodes()[index].ExpectedCompletion(result.ExpectedCompletion);
            educationEnrollViewModel.userEducationDegreeCodes()[index].NextBoostAt(result.NextBoostAt);
            loadEnrolledDegreeInfoByIndex(index);
            var competionpercent = educationEnrollViewModel.userEducationDegreeCodes()[index].CompletionPercent();
            knobinput.val(competionpercent).trigger('change');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}


function initializeEducationEnroll() {
    $("#enrolleducationcontainer").html(enrollEducationView);
    educationEnrollViewModel = {
        contentLoadTriggered: false,
        majorCodes: ko.observableArray(),
        userEducationDegreeCodes: ko.observableArray(),
        enrollingDegreeCodes: ko.observableArray(),
        enrollingDegreeCodesSubmit: ko.observableArray(),
        degreeCodes: ko.observableArray(),
        amountBeforeTax: ko.observable(0),
        tax: ko.observable(0),
        taxRate: ko.observable(taxViewModel.TaxType()[taxeducationCode - 1].TaxPercent()),
        OriginalAmountAvailable: ko.observable(userBankAccountViewModel.Cash()),
        AfterEducationRenderAction: function (elem) {
            $(elem).find('div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            });
        },
        ApplyEducationBoost: function (data, event) {
            var self = $(event.currentTarget);
            var index = self.data("index");
            self.button('loading')
            var knobinput = self.closest('.productbox').find('input.dial');
            ApplyBoostTime(index, self, knobinput);
        }
    };
    educationEnrollViewModel.Amount = ko.computed(function () {
        var total = 0;
        for (var i = 0; i < educationEnrollViewModel.enrollingDegreeCodes().length; i++) {
            total = total + educationEnrollViewModel.
                majorCodes()[educationEnrollViewModel.enrollingDegreeCodes()[i]].Cost();
        }
        educationEnrollViewModel.amountBeforeTax(total);
        educationEnrollViewModel.tax(total * educationEnrollViewModel.taxRate() / 100);

        return total + educationEnrollViewModel.tax();
    });
    educationEnrollViewModel.AmountAvailable = ko.computed(function () {
        var result = educationEnrollViewModel.OriginalAmountAvailable() - educationEnrollViewModel.Amount();
        return result;
    });
    educationEnrollViewModel.EnrollingCount = ko.computed(function () {
        var items = ko.utils.arrayFilter(educationEnrollViewModel.userEducationDegreeCodes(), function (item) {
            return item.Status() == "I";
        });
        return items.length;
    });
    educationEnrollViewModel.CompletedCount = ko.computed(function () {
        var items = ko.utils.arrayFilter(educationEnrollViewModel.userEducationDegreeCodes(), function (item) {
            return item.Status() == "C";
        });
        return items.length;
    });
    educationEnrollViewModel.ShowValidation = ko.computed(function () {
        if (educationEnrollViewModel.AmountAvailable() < 0) {
            return true;
        }
        else {
            return false;
        }
    });
    educationEnrollViewModel.showEnrollValidation = ko.computed(function () {
        if (educationEnrollViewModel.AmountAvailable() < 0 || educationEnrollViewModel.Amount() == 0) {
            return true;
        }
        else {
            return false;
        }
    });
}
function setupEducationEnroll() {
    applyKoEducationEnroll();
    getallEducationcodes(true, function () {
        addeducationKnob();
    });
    $('#saveeducation').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveEducationCart(btn);
    });
    seteducationScorllbar();
    $('#enrollmoreeducation').click(function () {
        initializeEducationEnroll();
        setupEducationEnroll();
    });
}
function seteducationScorllbar() {

    $("#educationEnrollItemcontent-box").perfectScrollbar(
       {
           suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10, alwaysVisibleY: true
       });
}
function getDegreeData() {
    if (cachedegreeCodes.length > 0) {
        return cachedegreeCodes
    }
    return $.ajax({
        url: "/api/educationservice/getdegreecodes",
        type: "get",
        contentType: "application/text",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })

}
function getMajorData() {
    if (cachemajorCodes.length > 0) {
        return cachemajorCodes;
    }
    return $.ajax({
        url: "/api/educationservice/getmajorcodes",
        type: "get",
        contentType: "application/text",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function saveEducationCart(btn) {
    for (var i = 0; i < educationEnrollViewModel.enrollingDegreeCodes().length; i++) {
        var enrolling = new EnrollingDegree
            (educationEnrollViewModel.majorCodes()[educationEnrollViewModel.enrollingDegreeCodes()[i]].MajorId(),
        educationEnrollViewModel.majorCodes()[educationEnrollViewModel.enrollingDegreeCodes()[i]].DegreeId());
        educationEnrollViewModel.enrollingDegreeCodesSubmit.push(enrolling);
    }

    $.ajax({
        url: "/api/EducationService/saveenrolldegree",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(educationEnrollViewModel.enrollingDegreeCodesSubmit),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            educationDetailsViewModelresult = ko.mapping.fromJS(result);
            if (educationDetailsViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#myeducationcontent-box").html("");
                $("#myeducationcontent-box").addClass("hidden");
                $("#enrollmoreeducation").removeClass("hidden");
                $("#myeducationcontent-submit").removeClass("hidden");
                $("#myeducationcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(educationDetailsViewModelresult, $("#myeducationcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#enrolleducationcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function getEducationViewModel(refresh) {
    if (cacheenrolledDegree.length > 0 && refresh == false) {
        return cacheenrolledDegree;
    }
    return $.ajax({
        url: "/api/educationservice/geteducationdetails",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getallEducationcodes(refresh, callback) {
    if (educationEnrollViewModel.contentLoadTriggered == false) {
        educationEnrollViewModel.contentLoadTriggered = true;
        showLoadingImage($("#myeducationcontent-box .panel-body"))
        showLoadingImage($("#educationEnrolledcontent-box .panel-body"))
        showLoadingImage($("#completedDegreecontent-box .panel-body"))
        $.when(getMajorData(), getDegreeData(), getEducationViewModel(refresh))
            .done(function (a1, a2, a3) {
                cachemajorCodes = a1;
                cachedegreeCodes = a2;
                cacheenrolledDegree = a3;

                ko.mapping.fromJS(a1[0], {}, educationEnrollViewModel.majorCodes);
                ko.mapping.fromJS(a2[0], {}, educationEnrollViewModel.degreeCodes);
                ko.mapping.fromJS(a3[0], {}, educationEnrollViewModel.userEducationDegreeCodes);
                if (refresh == true) {
                    processEducationCollection();
                }
                if (typeof (callback) == "function") {
                    callback();
                }
                educationEnrollViewModel.contentLoadTriggered = false;
                hideLoadingImage($("#myeducationcontent-box .panel-body"))
                hideLoadingImage($("#educationEnrolledcontent-box .panel-body"))
                hideLoadingImage($("#completedDegreecontent-box .panel-body"))

            });
    }
}
function processEducationCollection() {
    for (i = 0; i < educationEnrollViewModel.userEducationDegreeCodes().length; i++) {
        if (educationEnrollViewModel.userEducationDegreeCodes()[i].DegreeId() < 3
            && educationEnrollViewModel.userEducationDegreeCodes()[i].Status() == "C") {
            educationEnrollViewModel.majorCodes()[educationEnrollViewModel.userEducationDegreeCodes()[i].MajorId() - 1].DegreeId(educationEnrollViewModel.userEducationDegreeCodes()[i].DegreeId() + 1);
        }
        else {
            educationEnrollViewModel.majorCodes()[educationEnrollViewModel.userEducationDegreeCodes()[i].MajorId() - 1].Status("H"); //hidden
        }
    }
    for (var i = 0; i < educationEnrollViewModel.majorCodes().length; i++) {

        var id = educationEnrollViewModel.majorCodes()[i].DegreeId();
        var cost = educationEnrollViewModel.degreeCodes()[id].DegreeId() *
                 educationEnrollViewModel.majorCodes()[i].Cost() + educationEnrollViewModel.majorCodes()[i].Cost();

        educationEnrollViewModel.majorCodes()[i].Cost(cost);
        educationEnrollViewModel.majorCodes()[i].DegreeName(
               educationEnrollViewModel.degreeCodes()[id].DegreeName());
        educationEnrollViewModel.majorCodes()[i].DegreeImageFont(
                educationEnrollViewModel.degreeCodes()[id].DegreeImageFont());
        educationEnrollViewModel.majorCodes()[i].DegreeRank(
educationEnrollViewModel.degreeCodes()[id].DegreeRank());
    }
    loadEnrolledDegreeInfo();
}
function applyKoEducationEnroll() {
    ko.applyBindings(educationEnrollViewModel, $("#educationEnrollcontent-wrapper").get(0));
    ko.applyBindings(educationEnrollViewModel, $("#myeducationcontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#myeducationcontent-box .panel-heading").get(0));
    ko.applyBindings(educationEnrollViewModel, $("#saveeducation").get(0));
}
function EnrollingDegree(majorId, degreeId) {
    this.MajorId = majorId;
    this.DegreeId = degreeId;
}

function initializeEducationCompleted() {
    $("#completeddegreecontainer").html(completedEducationView);
    applyKoEducationCompleted();
    $('#completeddegreerefresh').click(function () {
        refreshEducationCompleted();
    });
    $("#completedDegreeItemcontent-box").perfectScrollbar(
    {
        suppressScrollX: true,
        wheelPropagation: true, alwaysVisibleY: true
    });
}
function applyKoEducationCompleted() {
    ko.applyBindings(educationEnrollViewModel, $("#completedDegreecontent-wrapper").get(0));
    ko.applyBindings(educationEnrollViewModel, $("#completedDegreecontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#completedDegreecontent-box .panel-heading").get(0));
}
function refreshEducationCompleted() {
    getallEducationcodes(true, function () {
        addeducationKnob();
    });
}