var robberyViewModel;
var incidentReportViewModel;
function initializerobbery() {
    robberyViewModel = {
        contentLoadTriggered: false,
        totalAmountSnipped: ko.observable(0),
        robbedProperty: ko.observable(0),
        amountAllowedSnipped: ko.observable(0),
        victimCashFullName: ko.observable(''),
        victimCashCountryCode: ko.observable(''),
        victimCashOnlineStatus: ko.observable(0),
        victimCashPicture: ko.observable(''),
        victimCashId: ko.observable(0),

        victimPropFullName: ko.observable(''),
        victimPropCountryCode: ko.observable(''),
        victimPropOnlineStatus: ko.observable(0),
        victimPropPicture: ko.observable(''),
        victimPropId: ko.observable(0),
        victimProperty: ko.observableArray([]),
        crimeReport: ko.observable(0),
    }
    setupRobbery();
    initializeRobberyNavigation();
}
function initializeRobberyNavigation() {
    $('#robbery-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#robbery-snipe") {
        }
        else if (self.attr('href') == "#robbery-property") {

        }
        else if (self.attr('href') == "#robbery-report") {
            if (robberyViewModel.crimeReport() == 0) {
                getMyCrimeReport();
            }
        }
    })
}

function setupRobbery() {
    ko.applyBindings(peopleInfo, $("#mysnipeCashcontent-box .panel-heading").get(0));
    console.log("applying ko");
    ko.applyBindings(peopleInfo, $("#myrobberycontent-box .panel-heading").get(0));
    ko.applyBindings(friendViewModel, $("#ddsnipeFriends ul.dropdown")[0]);
    ko.applyBindings(robberyViewModel, $("#selectedVictim")[0]);
    ko.applyBindings(robberyViewModel, $("#snipeAmount")[0]);
    ko.applyBindings(friendViewModel, $("#ddrobproperty ul.dropdown")[0]);
    ko.applyBindings(robberyViewModel, $("#selectedPropertyVictim")[0]);
    ko.applyBindings(robberyViewModel, $("#propertyrobparent")[0]);

    var snipedd = new DropDown($('#ddsnipeFriends'));
    snipedd.opts.on('click', function () {
        robberyViewModel.victimCashId($(this).data("id"));
        getAllowedSnipeAmount();
        robberyViewModel.victimCashCountryCode($(this).data("countrycode"));
        robberyViewModel.victimCashOnlineStatus($(this).data("onlinestatus"));
        robberyViewModel.victimCashFullName($(this).data("fullname"));
        robberyViewModel.victimCashPicture($(this).data("picture"));
    });

    var robdd = new DropDown($('#ddrobproperty'));
    robdd.opts.on('click', function () {
        robberyViewModel.victimPropId($(this).data("id"));
        getVictimproperty();
        robberyViewModel.victimPropCountryCode($(this).data("countrycode"));
        robberyViewModel.victimPropOnlineStatus($(this).data("onlinestatus"));
        robberyViewModel.victimPropFullName($(this).data("fullname"));
        robberyViewModel.victimPropPicture($(this).data("picture"));
    });

    $("#ddsnipeFriends ul, #ddrobproperty ul, #propertyrob").perfectScrollbar({
        suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
    });
    spinrobberyInput();
    $('#saveCashSnipe').click(function () {
        var btn = $(this)
        saveCashSnipe(btn);
    });
    $('#saveRobbery').click(function () {
        var btn = $(this)
        saveRobProperty(btn);
    });
    $('#snipeMoreCash,#robMoreProperty').click(function () {
        showSnipeCash();
    });
}
function showSnipeCash() {
    $('#carousel' + robberyViewCarosuelId).html(robberyView);
    initializerobbery();
}
function getAllowedSnipeAmount() {
    $.ajax({
        url: "/api/robberyservice/allowedmaxcashsnipe",
        type: "get",
        contentType: "application/json",
        data: {
            friendId: robberyViewModel.victimCashId()
        },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            robberyViewModel.amountAllowedSnipped(result);
            $("#snipeAmount").trigger("touchspin.updatesettings", { max: result });
        }
    });
}
function spinrobberyInput() {
    var self = $("#snipeAmount");
    self.TouchSpin({
        prefix: "Snipe Cash",
        initval: 0,
        min: 0,
        max: 0,
        step: 1,
        decimals: 2,
        booster: true,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        postfix: '<i class="fa icon-money28  fa-1x"></i>',
        mousewheel: true
    });

}
function saveCashSnipe(btn) {
    btn.button('loading')
    $.ajax({
        url: "/api/RobberyService/SnipeCash",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify({
            VictimId: robberyViewModel.victimCashId(),
            Amount: robberyViewModel.totalAmountSnipped(),
        }),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var snipeViewModelresult = ko.mapping.fromJS(result);
            if (snipeViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mysnipeCashcontent-box").html("");
                $("#mysnipeCashcontent-box").addClass("hidden");
                $("#mysnipeCashcontent-submit").removeClass("hidden");
                $("#snipeMoreCash").removeClass("hidden");
                $("#mysnipeCashcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(snipeViewModelresult, $("#mysnipeCashcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#snipeCashcontainer").offset().top }, "slow");
                btn.button('reset');

            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function saveRobProperty(btn) {
    btn.button('loading')
    $.ajax({
        url: "/api/RobberyService/RobProperty",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify({
            VictimId: robberyViewModel.victimPropId(),
            MerchandiseTypeId: robberyViewModel.robbedProperty(),
        }),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var robViewModelresult = ko.mapping.fromJS(result);
            if (robViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#myrobberycontent-box").html("");
                $("#myrobberycontent-box").addClass("hidden");
                $("#myrobberycontent-submit").removeClass("hidden");
                $("#robMoreProperty").removeClass("hidden");
                $("#myrobberycontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(robViewModelresult, $("#myrobberycontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#robberycontainer").offset().top }, "slow");
                btn.button('reset');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function getVictimproperty() {
    $.ajax({
        url: "/api/MerchandiseService/GetMerchandiseProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: robberyViewModel.victimPropId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            robberyViewModel.victimProperty.removeAll();
            for (i = 0; i < result.length; i++) {
                addpropertyItemCss(result[i]);
                robberyViewModel.victimProperty.push(result[i]);
                addpropertycondtionKnob($("#propertyrob input[data-index=" + i + "]"));
            }
        }
    });
}

function initializeIncidentReport(taskId) {
    incidentReportViewModel = {
        contentLoadTriggered: false,
        crimeReport: ko.observable(''),
        suspectId: ko.observable(''),
        incidentId: ko.observable(taskId),
        suspectFullName: ko.observable(''),
        suspectCountryCode: ko.observable(''),
        suspectOnlineStatus: ko.observable(0),
        suspectPicture: ko.observable(''),
        suspectId: ko.observable(0),
        suspectNetWorth: ko.observable(0),
        property: ko.observableArray([]),
    }
    setupIncidentReport();
}
function setupIncidentReport() {
    ko.applyBindings(incidentReportViewModel, $("#selectedSuspect")[0]);
    ko.applyBindings(incidentReportViewModel, $("#propertysuspect")[0]);
    ko.applyBindings(friendViewModel, $("#ddincident ul.dropdown")[0]);
    var snipedd = new DropDown($('#ddincident'));
    snipedd.opts.on('click', function () {
        incidentReportViewModel.suspectId($(this).data("id"));
        getSuspectNetWorth();
        getSuspectProfileproperty();
        incidentReportViewModel.suspectCountryCode($(this).data("countrycode"));
        incidentReportViewModel.suspectOnlineStatus($(this).data("onlinestatus"));
        incidentReportViewModel.suspectFullName($(this).data("fullname"));
        incidentReportViewModel.suspectPicture($(this).data("picture"));
    });
    $("#ddincident ul, #propertysuspect .panel-body").perfectScrollbar({
        suppressScrollX: true, wheelPropagation: false, wheelSpeed: 5, minScrollbarLength: 10
    });
    $('#reportSuspect').click(function () {
        var btn = $(this)
        reportSuspect(btn);
    });
}
function getCrimeReport() {
    $.ajax({
        url: "/api/RobberyService/GetCrimeReportByIncident",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { incident: incidentReportViewModel.incidentId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            incidentReportViewModel.crimeReport = ko.mapping.fromJS(result);
            ko.applyBindings(incidentReportViewModel.crimeReport, $("#incidentInfo")[0]);
        }
    });
}
function getSuspectNetWorth() {
    $.ajax({
        url: "/api/UserBankAccountService/GetNetWorth",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: incidentReportViewModel.suspectId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            incidentReportViewModel.suspectNetWorth(result);
        }
    });
}
function getSuspectProfileproperty() {
    $.ajax({
        url: "/api/MerchandiseService/GetMerchandiseProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { profileId: incidentReportViewModel.suspectId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#propertysuspect .panel-body").eq(0)),
        success: function (result) {
            incidentReportViewModel.property.removeAll();
            for (i = 0; i < result.length; i++) {
                addpropertyItemCss(result[i]);
                incidentReportViewModel.property.push(result[i]);
                addpropertycondtionKnob($("#propertysuspect input[data-index=" + i + "]"));
            }
        }
    }).always(function () {
        hideLoadingImage($("#propertysuspect .panel-body").eq(0));
    });
}
function reportSuspect(btn) {
    btn.button('loading')
    $.ajax({
        url: "/api/RobberyService/ReportSuspect",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify({
            IncidentId: incidentReportViewModel.incidentId(),
            SuspectId: incidentReportViewModel.suspectId(),
        }),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var robViewModelresult = ko.mapping.fromJS(result);
            if (robViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#myreportSuspectcontent-box").html("");
                $("#myreportSuspectcontent-box").addClass("hidden");
                $("#myreportSuspectcontent-submit").removeClass("hidden");
                $("#myreportSuspectcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(robViewModelresult, $("#myreportSuspectcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#reportSuspectcontainer").offset().top }, "slow");
                btn.button('reset');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}


function getMyCrimeReport() {
    $.ajax({
        url: "/api/RobberyService/GetCrimeReportByUser",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            robberyViewModel.crimeReport = ko.mapping.fromJS(result);
            ko.applyBindings(robberyViewModel.crimeReport, $("#mycrimereportcontent-box .panel-body")[0]);
        }
    });
}