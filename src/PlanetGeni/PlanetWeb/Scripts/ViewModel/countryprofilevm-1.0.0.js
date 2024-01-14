var countryProfileviewModel;
function initializeCountryProfile(countryId) {
    initializeCountryProfileView(countryId);
    setupCountryProfileView();
}
function initializeCountryProfileView(countryId) {
    countryProfileviewModel = {
        budget: ko.observableArray([]),
        budgettriggred: false,
        revenue: ko.observableArray([]),
        revenuetriggred: false,
        ranks: ko.observableArray([]),
        rankstriggred: false,
        securityAsset: ko.observableArray([]),
        securityAssettriggred: false,
        leaders: ko.observableArray([]),
        leaderstriggred: false,
        countryProfileInfo: ko.observable(),
        countryProfileId: ko.observable(countryId),
    };
}
function setupCountryProfileView() {
    ko.applyBindings(peopleInfo, $("#countryProfileview-box .panel-heading").get(0));
    $("#countryProfileAccordion .panel-heading").each(function () {
        ko.applyBindings(countryProfileviewModel, this);
    });

    getCountryProfileStat();
    getProfilebudget();
    $("#budgetcountryProfile, #leaderscountryProfile,#revenuecountryProfile, #rankscountryProfile, #securityAssetcountryProfile").perfectScrollbar(
       {
           suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
       });

    ko.applyBindings(countryProfileviewModel, $("#budgetcountryProfile").get(0));
    ko.applyBindings(countryProfileviewModel, $("#revenuecountryProfile").get(0));
    ko.applyBindings(countryProfileviewModel, $("#rankscountryProfile").get(0));
    ko.applyBindings(countryProfileviewModel, $("#leaderscountryProfile").get(0));
    ko.applyBindings(countryProfileviewModel, $("#securityAssetcountryProfile").get(0));


    $('.accordioncountryProfile').on('shown.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').addClass('active');
        var depId = $(e.target).attr('id').replace('collapse', '');
        switch (depId) {
            case 'budgetcountryProfile':
                if (countryProfileviewModel.budgettriggred == false) {
                    getProfilebudget();
                }
                break;
            case 'revenuecountryProfile':
                if (countryProfileviewModel.revenuetriggred == false) {
                    getProfilerevenue();
                }
                break;
            case 'rankscountryProfile':
                if (countryProfileviewModel.rankstriggred == false) {
                    getProfileranks();
                }
                break;
            case 'securityAssetcountryProfile':
                if (countryProfileviewModel.securityAssettriggred == false) {
                    getProfilesecurityAsset();
                }
                break;
            case 'leaderscountryProfile':
                if (countryProfileviewModel.leaderstriggred == false) {
                    getProfileleaders();
                }
                break;
        }
    });
    $('.accordioncountryProfile').on('hidden.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').removeClass('active');
    });
}
function getCountryProfileStat() {
    $.ajax({
        url: "/api/CountryCodeService/GetCountryProfileDTO",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { countryId: countryProfileviewModel.countryProfileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#countryProfileview-box .panel-body").eq(0)),
        success: function (result) {
            $("#countryprofileitemHeader, #countryProfileAccordion").removeClass("hidden");
            countryProfileviewModel.countryProfileInfo = ko.mapping.fromJS(result);
            $("#countryProfileview-box .countryProfilestat").each(function () {
                ko.applyBindings(countryProfileviewModel.countryProfileInfo, this);
            });
        }
    }).always(function () {
        hideLoadingImage($("#countryProfileview-box .panel-body").eq(0));
    });
}
function getProfilebudget() {
    $.ajax({
        url: "/api/CountryBudgetService/GetCountBudgetPercenTile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { countryId: countryProfileviewModel.countryProfileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#budgetcountryProfile .panel-body").eq(0)),
        success: function (result) {
            console.log(result);
            for (i = 0; i < result.length; i++) {
                addKnobDialReverseCss(result[i], result[i].BudgetPercent);
                countryProfileviewModel.budget.push(result[i]);
                addpercentKnob(i, "budgetcountryProfile", 50);
            }
            countryProfileviewModel.budgettriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#budgetcountryProfile .panel-body").eq(0));
    });
}
function getProfilerevenue() {
    $.ajax({
        url: "/api/CountryTaxService/GetRevenueByCountry",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { countryId: countryProfileviewModel.countryProfileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#revenuecountryProfile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                addKnobDialReverseCss(result[i], result[i].TotalPercent);
                countryProfileviewModel.revenue.push(result[i]);
                addpercentKnob(i, "revenuecountryProfile", 50);
            }
            countryProfileviewModel.revenuetriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#revenuecountryProfile .panel-body").eq(0));
    });
}
function getProfileranks() {
    $.ajax({
        url: "/api/CountryCodeService/GetCountryRankingProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { countryId: countryProfileviewModel.countryProfileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#rankscountryProfile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                countryProfileviewModel.ranks.push(result[i]);
            }
            countryProfileviewModel.rankstriggred = true;

        }
    }).always(function () {
        hideLoadingImage($("#rankscountryProfile .panel-body").eq(0));
    });
}
function getProfilesecurityAsset() {
    $.ajax({
        url: "/api/NationalSecurityService/GetSecurityProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { countryId: countryProfileviewModel.countryProfileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#securityAssetcountryProfile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                addKnobDialCss(result[i]); countryProfileviewModel.securityAsset.push(result[i]);
                addpercentKnob(i, "securityAssetcountryProfile", 40);
            }
            countryProfileviewModel.securityAssettriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#securityAssetcountryProfile .panel-body").eq(0));
    });
}
function getProfileleaders() {
    $.ajax({
        url: "/api/CountryCodeService/GetActiveLeadersProfile",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        data: { countryId: countryProfileviewModel.countryProfileId() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#leaderscountryProfile .panel-body").eq(0)),
        success: function (result) {
            for (i = 0; i < result.length; i++) {
                countryProfileviewModel.leaders.push(result[i]);
            }
            countryProfileviewModel.leaderstriggred = true;
        }
    }).always(function () {
        hideLoadingImage($("#leaderscountryProfile .panel-body").eq(0));
    });
}
