var peopleInfo = {
    UserId: ko.observable(0),
    FullName: ko.observable(''),
    Picture: ko.observable(''),
    CountryId: ko.observable(''),
    UserLevel: ko.observable(0)
};
var tour;
var searchViewModel = {
    webuserList: ko.observableArray([])
};
var accessdenied = '<span class="fontsize90">Access Denied</span>';
var userprofile = "/UserProfile/GetUserProfile";
var manageprofileurl = "/UserProfile/GetManageUserProfile";
var sasurl = "/api//WebUserService/GetBlobSasUrl";
var maxFileSize = "4000000";
var imageacceptFileTypes = /^image\/(gif|jpe?g|png)$/i;
var countryprofile = "/CountryProfile/GetCountryProfile";
var defaultprofilepic = "0.png";
var partypicContainer = "https://thinkaraimages.blob.core.windows.net/partycontainer/";
var profilepicContainer = "https://thinkaraimages.blob.core.windows.net/profilecontainer/";
var adspicContainer = "https://thinkaraimages.blob.core.windows.net/adscontainer/";
var codeTables;
var userBankAccountViewModel;
var voteCarosuelId = 13;
var taxCarosuelId = 15;
var robberyViewCarosuelId = 9;
var partyinfoViewCarosuelId = 20;
var electionticketViewCarosuelId = 21;
var manageprofileCarosuelId = 24;
var userprofileCarosuelId = 22;
var inviteCarosuelId = 23;
var stockForecastViewCarosuelId = 25;
var reportSuspectViewCarosuelId = 26;
var countryprofileCarosuelId = 14;
var userloanCarosuelId = 8;
var taskCarosuelId = 9;
var budgetCarosuelId = 10;
var cacheIndustryCodes = [];
var cacheAgendaCodes = [];
var cacheAdsType = "";
var industryCodeViewModel = {
    industryCodes: ko.observableArray()
};
var majorCodeViewModel = {
    majorCodes: ko.observableArray()
};
var cachemajorCodes = [];
var cacheSlotMachine3List = "";
var bankId = 10001;
var taxViewModel;
var taxViewModelCache;
var taxincomeCode = 6;
var taxadsCode = 8;
var taxstockCode = 5;
var taxpropertyCode = 3;
var taxeducationCode = 4;
var agendaCodeViewModel = {
    agendaCodes: ko.observableArray()
};
var countryCodeViewModel = {
    countries: ko.observableArray(),
    clickgiftnation: function (data, event) {
        var self = $(event.currentTarget);
        var inputcheck = self.children('input');
        var imgcheck = self.find('.img-check');

        if (inputcheck.prop('checked') == true) {
            inputcheck.prop('checked', false);
            imgcheck.css('opacity', '0.2');
            gitfCartViewModel.NationId.pop(inputcheck.val());

        } else {
            inputcheck.prop('checked', true);
            imgcheck.css('opacity', '1');
            gitfCartViewModel.NationId.push(inputcheck.val());
        }
        if (gitfCartViewModel.NationId().length !=
            countryCodeViewModel.countries().length) {
            var $checkbox = $("#giftcountryall");
            $checkbox.prop('checked', false);
            $button = $checkbox.siblings('button');
            updateDisplay($checkbox, $button);
        }
        updategitfCartViewModel();
    }

};
var countryCodewithoutownViewModel = ko.observableArray();
// First, checks if it isn't implemented yet.
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}
var Page = (function () {
    init = function () {
        initEvents();
        validationDefaults();
    },
    initEvents = function () {

        var config = {
            $carousel: $('#myCarousel'),
            $tileandicon: $('.tileSuccess, .tileSuccess a i, .tileWarning, .tileWarning a i '),
            $backbutton: $('.backbtn')
        };
        config.$tileandicon.bind("click", function () {
            $(this).attr("disabled", true);
            if ($(this).is("a"))
                var elemtile = $(this).parent("div");
            else
                var elemtile = $(this);

            var carosuelId = elemtile.data('tile');
            var myUrl = elemtile.data("url");
            if (peopleInfo.UserId() == 0) {
                return;
            }
            if (carosuelId == 1) {
                accountViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 2) {
                buyViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 3) {
                stockViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 4) {
                educationViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 5) {
                jobViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 6) {
                casinoViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 7) {
                electionViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 8) {
                userloanViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 9) {
                robberyViewClick(myUrl, carosuelId)
            }
            else if (carosuelId == 10) {
                budgetViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 11) {
                adsViewClick(myUrl, carosuelId);
            } else if (carosuelId == 12) {
                if (peopleInfo.IsLeader() == 'true') {
                    securityViewClick(myUrl, carosuelId);
                }
                else {
                    return;
                }
            }
            else if (carosuelId == userprofileCarosuelId) {
                profileViewClick(myUrl, carosuelId, peopleInfo.UserId());
            }
            else if (carosuelId == countryprofileCarosuelId) {
                countryProfileViewClick(myUrl, carosuelId, peopleInfo.CountryId());
            }
            else if (carosuelId == 15) {
                taxViewClick(myUrl, carosuelId);
            } else if (carosuelId == 16) {
                giftViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 18) {
                partyViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == 19) {
                lotteryViewClick(myUrl, carosuelId);
            }
            else if (carosuelId == manageprofileCarosuelId) {
                manageprofileViewClick(myUrl, carosuelId);
            }
            config.$carousel.carousel(carosuelId);
            ga('send', 'pageview', myUrl);
            $(this).attr("disabled", false);

        });
        $(document).on('click', '.backbtn', function () {
            $('#myCarousel').carousel(0);
            $("#main").animate({ scrollTop: $("#main").offset().top }, "slow");

        });
        $(document).click(function () {
            // all dropdowns
            $('.wrapper-dropdown-3').removeClass('active');
        });
        setupSearch();
        $("#demo, #tour").on("click", function () {
            startTour();
        });
    };
    return { init: init };
})();
function setupSearch() {
    ko.applyBindings(searchViewModel, $("#ddsearchresult ul.dropdown")[0]);
    $('#autosearch').on('click', function () {
        autoSearch();
    });
    $('#srch-term').typing({
        start: function (e) {
        },
        stop: function (e) {
            autoSearch();
        },
        delay: 1000
    });

}
function autoSearch() {
    if ($('#srch-term').val().trim().length > 2) {
        $.ajax({
            url: "/api/webuserservice/autocompleteusersearch",
            type: "get",
            contentType: "application/text",
            data: { term: $('#srch-term').val() },
            cache: true,
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                searchViewModel.webuserList.removeAll();
                for (i = 0; i < result.length; i++) {
                    searchViewModel.webuserList.push(result[i]);
                }
                $('#ddsearchresult').addClass('active');
            }
        });
    }
}
function updateDisplay(checkbox, button) {
    var settings = {
        on: {
            icon: 'glyphicon glyphicon-check'
        },
        off: {
            icon: 'glyphicon glyphicon-unchecked'
        }
    };

    var color = button.data('color');
    var isChecked = checkbox.is(':checked');
    // Set the button's state
    button.data('state', (isChecked) ? "on" : "off");
    // Set the button's icon
    button.find('.state-icon')
        .removeClass()
        .addClass('state-icon ' + settings[button.data('state')].icon);

    // Update the button's color
    if (isChecked) {
        button
            .removeClass('btn-default')
            .addClass('btn-' + color + ' active');
    }
    else {
        button
            .removeClass('btn-' + color + ' active')
            .addClass('btn-default');
    }
}
function updateSelection(updateviewmodel, checkbox) {
    var isChecked = checkbox.is(':checked');
    var inputcheck;
    var imgcheck;
    var parentdiv;
    parentdiv = checkbox.closest("div.panel").find("div.panel-body");
    inputcheck = parentdiv.find('.recipient').children('input');
    imgcheck = parentdiv.find('.recipient').children('.img-check');
    updateviewmodel.removeAll();

    if (!isChecked) {
        inputcheck.prop('checked', false);
        imgcheck.css('opacity', '0.2');

    } else {
        inputcheck.prop('checked', true);
        imgcheck.css('opacity', '1');
        inputcheck.each(function () {
            updateviewmodel.push($(this).val());

        });
    }
}
function initilaizeUpdatedCheckbox(checkbox, button) {
    var settings = {
        on: {
            icon: 'glyphicon glyphicon-check'
        },
        off: {
            icon: 'glyphicon glyphicon-unchecked'
        }
    };
    button.on('click', function () {
        checkbox.prop('checked', !checkbox.is(':checked'));
        checkbox.triggerHandler('change');
        updateDisplay(checkbox, button);
    });
    updateDisplay(checkbox, button);
    // Inject the icon if applicable
    if (button.find('.state-icon').length == 0) {
        button.prepend('<i class="state-icon ' + settings[button.data('state')].icon + '"></i> ');
    }
}
function budgetViewClick(myUrl, carosuelId, callback) {
    if (budgetView == "" || budgetvm == false) {
        $.when($.get(myUrl), getbudgetvm())
            .done(function (a1, a2) {
                budgetView = a1[0];
                $('#carousel' + carosuelId).html(budgetView);
                budgetvm = true;
                BudgetReInitialize();
                if (typeof (callback) != "function") {
                    BudgetInitialize();
                }
                else {
                    callback();
                }
            });
    } else {
        $('#carousel' + carosuelId).html(budgetView);
        if (typeof (callback) == "function") {
            BudgetReInitialize();
            callback();

        } else {
            BudgetReInitialize();
            BudgetInitialize();

        }
    }
}
function getbudgetvm() {
    if (budgetvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/budgetvm-1.0.0.js");
        budgetvm = true;
    }
}
function taxViewClick(myUrl, carosuelId, callback) {
    if (taxView == "" || taxvm == false) {
        $.when($.get(myUrl), gettaxvm())
            .done(function (a1, a2) {
                taxView = a1[0];
                $('#carousel' + carosuelId).html(taxView);
                taxvm = true;
                TaxReInitialize();
                if (typeof (callback) != "function") {
                    TaxInitialize();
                }
                else {
                    callback();
                }
            });
    } else {
        $('#carousel' + carosuelId).html(taxView);
        if (typeof (callback) == "function") {
            TaxReInitialize();
            callback();

        } else {
            TaxReInitialize();
            TaxInitialize();

        }
    }
}
function gettaxvm() {
    if (taxvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/taxvm-1.0.0.js");
        taxvm = true;
    }
}
function getTodaysForeCast() {
    stockForecastViewClick("/Stock/GetStockForecastView", stockForecastViewCarosuelId, function () {
        $('#myCarousel').carousel(stockForecastViewCarosuelId);
        getStockForecast();
        $("#main").animate({ scrollTop: 0 }, "slow");
    });
}

function stockForecastViewClick(myUrl, carosuelId, callback) {
    if (stockForecastView == "") {
        $.when($.get(myUrl), getstockvm())
            .done(function (a1, a2) {
                stockForecastView = a1[0];
                $('#carousel' + carosuelId).html(stockForecastView);
                stockvm = true;
                initializeStockForecastView();
                if (typeof (callback) == "function") {
                    callback();
                }

            });

    } else {
        $('#carousel' + carosuelId).html(stockForecastView);
        initializeStockForecastView();
        if (typeof (callback) == "function") {
            callback();
        }
    }
}

function stockViewClick(myUrl, carosuelId) {
    if (stockView == "" || stockvm == false) {
        $.when($.get(myUrl), getstockvm())
            .done(function (a1, a2) {
                stockView = a1[0];
                $('#carousel' + carosuelId).html(stockView);
                stockvm = true;
                initializestock();

            });
    } else {
        $('#carousel' + carosuelId).html(stockView);
        initializestock();
    }
}
function getstockvm() {
    if (stockvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/stockvm-1.0.0.js");
        stockvm = true;
    }
}
function educationViewClick(myUrl, carosuelId) {
    if (educationView == "" || educationvm == false) {
        $.when($.get(myUrl), geteducationvm())
            .done(function (a1, a2) {
                educationView = a1[0];
                $('#carousel' + carosuelId).html(educationView);
                educationvm = true;
                initializeeducation();

            });
    } else {
        $('#carousel' + carosuelId).html(educationView);
        initializeeducation();
    }
}
function geteducationvm() {
    if (educationvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/educationvm-1.0.0.js");
        educationvm = true;
    }
}

function jobViewClick(myUrl, carosuelId) {
    if (jobView == "" && jobvm == false) {
        $.when($.get(myUrl), $.loadScriptNoCallBack("Scripts/ViewModel/jobvm-1.0.0.js"), getIndustryCodes(), getMajorData())
            .done(function (a1, a2, a3, a4) {
                cacheIndustryCodes = a3;
                jobView = a1[0];
                ko.mapping.fromJS(a3[0], {}, industryCodeViewModel.industryCodes);
                ko.mapping.fromJS(a4[0], {}, majorCodeViewModel.majorCodes);
                $('#carousel' + carosuelId).html(jobView);
                jobvm = true;
                initializeJob();
            });

    } else {
        $('#carousel' + carosuelId).html(jobView);
        initializeJob();
    }
}
function getMajorData() {
    if (cachemajorCodes.length > 0) {
        return cachemajorCodes;
    }
    return $.ajax({
        url: "/api/educationservice/getmajorcodes",
        type: "get",
        contentType: "application/text",
        cache: true,
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function partyViewClick(myUrl, carosuelId) {
    if (partyView == "" || partyvm == false) {
        $.when($.get(myUrl), getpoliticalpartyvm(), getAgendaCodes())
            .done(function (a1, a2, a3) {
                cacheAgendaCodes = a3;
                for (var i = 0; i < cacheAgendaCodes[0].length; i++) {
                    cacheAgendaCodes[0][i].selected = false;
                }
                partyView = a1[0];
                ko.mapping.fromJS(cacheAgendaCodes[0], {}, agendaCodeViewModel.agendaCodes);
                $('#carousel' + carosuelId).html(partyView);
                partyvm = true;
                initializeParty();
            });

    } else {
        $('#carousel' + carosuelId).html(partyView);
        initializeParty();
    }
}
function getPartyInfo(taskId) {
    partyInfoViewClick("/PoliticalParty/GetPartyInfoView", partyinfoViewCarosuelId, function () {
        getpartyinfoById(taskId);
        $('#myCarousel').carousel(partyinfoViewCarosuelId);
        applyKoPartyInfo();
    });
}

function partyInfoViewClick(myUrl, carosuelId, callback) {
    if (partyinfoView == "") {
        $.when($.get(myUrl), getpoliticalpartyvm(), getAgendaCodes())
            .done(function (a1, a2, a3) {
                cacheAgendaCodes = a3;
                partyinfoView = a1[0];
                ko.mapping.fromJS(a3[0], {}, agendaCodeViewModel.agendaCodes);
                $('#carousel' + carosuelId).html(partyinfoView);
                partyvm = true;
                initializePartyInfoView();
                if (typeof (callback) == "function") {
                    callback();
                }

            });

    } else {
        $('#carousel' + carosuelId).html(partyinfoView);
        initializePartyInfoView();
        if (typeof (callback) == "function") {
            callback();
        }
    }
}
function getpoliticalpartyvm() {
    if (partyvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/politicalpartyvm-1.0.0.js")
        partyvm = true;
    }
}
function getbankaccountvm() {
    if (accountvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/bankvm-1.0.0.js");
        accountvm = true;
    }
}
function getprofileprofilevm() {
    if (profilevm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/profilevm-1.0.0.js")
        profilevm = true;
    }
}
function getcountryProfilevm() {
    if (countryProfilevm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/countryprofilevm-1.0.0.js")
        countryProfilevm = true;
    }
}
function electionViewClick(myUrl, carosuelId) {
    if (electionView == "" || electionvm == false) {
        $.when($.get(myUrl), getpoliticalelectionvm(), getAgendaCodes())
            .done(function (a1, a2, a3) {
                cacheAgendaCodes = a3;
                for (var i = 0; i < cacheAgendaCodes[0].length; i++) {
                    cacheAgendaCodes[0][i].selected = false;
                }
                electionView = a1[0];
                ko.mapping.fromJS(cacheAgendaCodes[0], {}, agendaCodeViewModel.agendaCodes);
                $('#carousel' + carosuelId).html(electionView);
                electionvm = true;
                initializeElection();
            });

    } else {
        $('#carousel' + carosuelId).html(electionView);
        initializeElection();
    }
}
function runForOfficeViewClick(myUrl, carosuelId, callback) {
    if (electionticketView == "" || electionvm == false) {
        $.when($.get(myUrl), getpoliticalelectionvm(), getAgendaCodes())
            .done(function (a1, a2, a3) {
                cacheAgendaCodes = a3;
                for (var i = 0; i < cacheAgendaCodes[0].length; i++) {
                    cacheAgendaCodes[0][i].selected = false;
                }
                electionticketView = a1[0];
                ko.mapping.fromJS(cacheAgendaCodes[0], {}, agendaCodeViewModel.agendaCodes);
                $('#carousel' + carosuelId).html(electionticketView);
                electionvm = true;
                initializeRunforOfficeTicket();
                if (typeof (callback) == "function") {
                    callback();
                }
            });

    } else {
        $('#carousel' + carosuelId).html(electionticketView);
        initializeRunforOfficeTicket();
        if (typeof (callback) == "function") {
            callback();
        }
    }
}
function getRunforOffice(taskId) {
    runForOfficeViewClick("/Election/GetElectionTicket", electionticketViewCarosuelId, function () {
        getRunforOfficeById(taskId);
        $('#myCarousel').carousel(electionticketViewCarosuelId);
    });
}

function getpoliticalelectionvm() {
    if (electionvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/electionvm-1.0.0.js")
        electionvm = true;
    }
}
function lotteryViewClick(myUrl, carosuelId) {
    if (lotteryView == "" && lotteryvm == false) {
        $.when($.get(myUrl), $.loadScriptNoCallBack("Scripts/ViewModel/lotteryvm-1.0.0.js"))
            .done(function (a1, a2) {
                lotteryView = a1[0];
                $('#carousel' + carosuelId).html(lotteryView);
                lotteryvm = true;
                initializeLottery();
            });

    } else {
        //$('#carousel' + carosuelId).html(lotteryView);
        //initializeLottery();
    }
}
function casinoViewClick(myUrl, carosuelId) {
    if (casinoView == "" && casinovm == false) {
        $.when($.get(myUrl), $.loadScriptNoCallBack("Scripts/ViewModel/casinovm-1.0.0.js")
        , $.loadScriptNoCallBack("Scripts/jquery.slotmachine.js"), getSlotMachineThreeList(),
         $.loadScriptNoCallBack("Scripts/winwheel-1.2.js"))
            .done(function (a1, a2, a3, a4, a5) {
                casinoView = a1[0];
                cacheSlotMachine3List = a4;
                $('#carousel' + carosuelId).html(casinoView);
                casinovm = true;
                initializeCasino();
            });
    }
}
function getSlotMachineThreeList() {
    if (cacheSlotMachine3List.length > 0) {
        return cacheSlotMachine3List;
    }
    return $.ajax({
        url: "/api/casinoservice/getslotmachinethreelist",
        type: "get",
        contentType: "application/text",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });

}

function awardViewClick(myUrl, carosuelId) {
    if (awardView == "") {
        $.get(myUrl, function (data) {
            awardView = data;
            $('#carousel' + carosuelId).html(awardView);
            if (awardvm == false) {
                $("head").append("<link href='Content/awards.css' rel='stylesheet'>");
                $.loadScript("Scripts/ViewModel/awardvm-1.0.0.js", function (data, textStatus, jqxhr) {
                    awardvm = true;
                });
            }
        });
    } else {

    }

}

function payloanViewClick(myUrl, carosuelId, callback) {
    if (payloanView == "") {
        $.get(myUrl, function (data) {
            payloanView = data;
            $('#carousel' + carosuelId).html(payloanView);
            callback();
        });
    } else {
        $('#carousel' + carosuelId).html(payloanView);
        if (typeof (callback) == "function") {
            callback();
        }
    }
}

function voteViewClick(myUrl, carosuelId, callback) {
    if (voteView == "" || votevm == false) {
        $.when($.get(myUrl), getvotevm())
            .done(function (a1, a2) {
                voteView = a1[0];
                $('#carousel' + carosuelId).html(voteView);
                votevm = true;
                initializeVote();
                if (typeof (callback) == "function") {
                    callback();
                }
            });
    } else {
        $('#carousel' + carosuelId).html(voteView);
        initializeVote();
        if (typeof (callback) == "function") {
            callback();
        }
    }
}
function getvotevm() {
    if (votevm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/votevm-1.0.0.js");
        votevm = true;
    }
}
function userloanViewClick(myUrl, carosuelId) {
    if (userloanView == "" || userloanvm == false) {
        $.when($.get(myUrl), getuserloanvm())
            .done(function (a1, a2) {
                userloanView = a1[0];
                $('#carousel' + carosuelId).html(userloanView);
                userloanvm = true;
                userLoanInitialize();
            });
    } else {
        $('#carousel' + carosuelId).html(userloanView);
        userLoanInitialize();
    }
}
function getuserloanvm() {
    if (userloanvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/userloanvm-1.0.0.js");
        userloanvm = true;
    }
}
function giftViewClick(myUrl, carosuelId) {
    if (giftView == "" || giftvm == false) {
        $.when($.get(myUrl), getgiftvm())
            .done(function (a1, a2) {
                giftView = a1[0];
                $('#carousel' + carosuelId).html(giftView);
                giftvm = true;
                giftInitialize();
            });
    } else {
        $('#carousel' + carosuelId).html(giftView);
        giftInitialize();
    }
}
function getgiftvm() {
    if (giftvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/giftvm-1.0.0.js");
        giftvm = true;
    }
}

function usertaskViewClick(myUrl, carosuelId, callback) {
    if (usertaskView == "") {
        $.get(myUrl, function (data) {
            usertaskView = data;
            $('#carousel' + carosuelId).html(usertaskView);
            initializeusertask();
            $('#myCarousel').carousel(carosuelId);
            if (typeof (callback) == "function") {
                callback();
            }
        });
    }
    else {
        $('#carousel' + carosuelId).html(usertaskView);
        initializeusertask();
        if (typeof (callback) == "function") {
            callback();
        }
        $('#myCarousel').carousel(carosuelId);
    }
    $("#main").animate({ scrollTop: 0 }, "slow");
}
function accountViewClick(myUrl, carosuelId) {
    if (accountView == "" || accountvm == false) {
        $.when($.get(myUrl), getbankaccountvm())
            .done(function (a1, a2) {
                accountView = a1[0];
                $('#carousel' + carosuelId).html(accountView);
                accountvm = true;
                $('#myCarousel').carousel(carosuelId);
                initializeBank();

            });

    } else {
        $('#carousel' + carosuelId).html(accountView);
        $('#myCarousel').carousel(carosuelId);
        initializeBank();
    }
    $("#main").animate({ scrollTop: 0 }, "slow");

}
function buyViewClick(myUrl, carosuelId) {
    if (buyView == "" || buyvm == false) {
        $.when($.get(myUrl), getbuyvm())
            .done(function (a1, a2) {
                buyView = a1[0];
                $('#carousel' + carosuelId).html(buyView);
                buyvm = true;
                initializemerchandise();
            });
    } else {
        $('#carousel' + carosuelId).html(buyView);
        initializemerchandise();
    }
}
function getbuyvm() {
    if (buyvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/merchandisevm-1.0.0.js");
        buyvm = true;
    }
}
function userNotificationViewClick(myUrl, carosuelId) {
    if (userNotificationvm == true) {
        if (userNotificationView == "") {
            $.get(myUrl, function (data) {
                userNotificationView = data;
                $('#carousel' + carosuelId).html(userNotificationView);
                applyUserNotificationBinding();
                $('#myCarousel').carousel(carosuelId);
            });
        } else {
            $('#carousel' + carosuelId).html(userNotificationView);
            applyUserNotificationBinding();
            $('#myCarousel').carousel(carosuelId);
        }
    }
    $("#main").animate({ scrollTop: 0 }, "slow");
}
function inviteViewClick(myUrl, carosuelId, callback) {
    $("#sendemailinvite").addClass('hidden');
    if (inviteView == "") {
        $.when($.get(myUrl), getEmailInvitationList(friendViewModel.lastEmailInviteeId()))
            .done(function (a1, a2) {
                inviteView = a1[0];
                populatepartyemailInviteListvm(a2[0], friendViewModel);
                $('#carousel' + carosuelId).html(inviteView);
                $('#myCarousel').carousel(carosuelId);
                initializeInvite();
                console.log(typeof (callback));
                if (typeof (callback) == "function") {
                    callback();
                }
                $("#sendemailinvite").removeClass('hidden');
            });

    } else {
        $('#carousel' + carosuelId).html(inviteView);
        $('#myCarousel').carousel(carosuelId);
        initializeInvite();
        $("#sendemailinvite").removeClass('hidden');
        if (typeof (callback) == "function") {
            callback();
        }
    }
    $("#main").animate({ scrollTop: 0 }, "slow");
}
function manageProfile() {
    manageprofileViewClick(manageprofileurl,
        manageprofileCarosuelId);
    $("#main").animate({ scrollTop: 0 }, "slow");
}
function getmanageprofilevm() {
    if (manageProfilevm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/manageprofilevm-1.0.0.js")
        manageProfilevm = true;
    }
}
function manageprofileViewClick(myUrl, carosuelId) {
    if (manageProfileView == "" || manageProfilevm == false) {
        $.when($.get(myUrl), getmanageprofilevm())
            .done(function (a1, a2) {

                manageProfileView = a1[0];
                $('#carousel' + carosuelId).html(manageProfileView);
                manageProfilevm = true;
                $('#myCarousel').carousel(carosuelId);
                initializeManageProfile();
            });

    } else {
        $('#carousel' + carosuelId).html(manageProfileView);
        $('#myCarousel').carousel(carosuelId);
        initializeManageProfile();
    }
}
function getCountryName(countryId) {
    if (countryId.length == 0) {
        return;
    }
    var match = ko.utils.arrayFirst(countryCodeViewModel.countries(), function (item) {
        return countryId.toLowerCase() === item.CountryId.toLowerCase();
    });
    if (match == null) {
        console.log(countryId);
    }
    return match.Code;
}
function getAdsType() {
    if (cacheAdsType.length > 0) {
        return cacheAdsType;
    }
    return $.ajax({
        url: "/api/advertisementservice/getadstype",
        type: "get",
        contentType: "application/text",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });

}
function getadsvm() {
    if (adsvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/adsvm-1.0.0.js")
        adsvm = true;
    }
}
function getrobberyvm() {
    if (robberyvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/robberyvm-1.0.0.js")
        robberyvm = true;
    }
}
function robberyViewClick(myUrl, carosuelId) {
    if (robberyView == "" || robberyvm == false) {
        $.when($.get(myUrl), getrobberyvm())
            .done(function (a1, a2) {
                robberyView = a1[0];
                $('#carousel' + carosuelId).html(robberyView);
                robberyvm = true;
                initializerobbery();
            });
    } else {
        $('#carousel' + carosuelId).html(robberyView);
        initializerobbery();
    }
}
function reportCrime(incidentId) {
    reportCrimeViewClick("/Robbery/ReportIncidentView", reportSuspectViewCarosuelId, incidentId, function () {
        $('#myCarousel').carousel(reportSuspectViewCarosuelId);
        getCrimeReport();
        $("#main").animate({ scrollTop: 0 }, "slow");
    });
}

function reportCrimeViewClick(myUrl, carosuelId, taskId, callback) {
    if (reportCrimeView == "") {
        $.when($.get(myUrl), getrobberyvm())
            .done(function (a1, a2) {
                reportCrimeView = a1[0];
                $('#carousel' + carosuelId).html(reportCrimeView);
                robberyvm = true;
                initializeIncidentReport(taskId);
                if (typeof (callback) == "function") {
                    callback();
                }
            });

    } else {
        $('#carousel' + carosuelId).html(reportCrimeView);
        initializeIncidentReport(taskId);
        if (typeof (callback) == "function") {
            callback();
        }
    }
}
function adsViewClick(myUrl, carosuelId) {
    if (adsView == "" || adsvm == false) {
        $.when($.get(myUrl), getadsvm(), getAdsType())
            .done(function (a1, a2, a3) {
                cacheAdsType = a3;
                adsView = a1[0];
                $('#carousel' + carosuelId).html(adsView);
                adsvm = true;
                initializeads();
            });

    } else {
        $('#carousel' + carosuelId).html(adsView);
        initializeads();
    }
}
function profileViewClick(myUrl, carosuelId, profileid) {
    if (profileView == "" || profilevm == false) {
        $.when($.get(myUrl), getprofileprofilevm())
            .done(function (a1, a2) {
                profileView = a1[0];
                $('#carousel' + carosuelId).html(profileView);
                profilevm = true;
                $('#myCarousel').carousel(carosuelId);
                initializeProfile(profileid);

            });

    } else {
        $('#carousel' + carosuelId).html(profileView);
        $('#myCarousel').carousel(carosuelId);
        initializeProfile(profileid);
    }
}
function countryProfileViewClick(myUrl, carosuelId, countryProfileid) {
    if (countryProfileView == "" || countryProfilevm == false) {
        $.when($.get(myUrl), getcountryProfilevm())
            .done(function (a1, a2) {
                countryProfileView = a1[0];
                $('#carousel' + carosuelId).html(countryProfileView);
                countryProfilevm = true;
                $('#myCarousel').carousel(carosuelId);
                initializeCountryProfile(countryProfileid);

            });

    } else {
        $('#carousel' + carosuelId).html(countryProfileView);
        $('#myCarousel').carousel(carosuelId);
        initializeCountryProfile(countryProfileid);
    }
}
function securityViewClick(myUrl, carosuelId) {
    if (securityView == "" || securityvm == false) {
        $.when($.get(myUrl), getsecurityvm())
            .done(function (a1, a2) {
                securityView = a1[0];
                $('#carousel' + carosuelId).html(securityView);
                securityvm = true;
                initializesecurity();

            });
    } else {
        $('#carousel' + carosuelId).html(securityView);
        initializesecurity();
    }
}
function getsecurityvm() {
    if (securityvm == false) {
        return $.loadScriptNoCallBack("Scripts/ViewModel/securityvm-1.0.0.js");
        securityvm = true;
    }
}

function validationDefaults() {
    $.validator.setDefaults({
        highlight: function (element) {
            if ($(element).closest('.validation-group').length > 0) {
                $(element).closest('.validation-group').addClass('has-error');
                $(element).closest('.validation-group').find('span.help-block').first().removeClass('hidden');
            } else {
                $(element).closest('.form-group').addClass('has-error');
            }
        },
        unhighlight: function (element) {
            if ($(element).closest('.validation-group').length > 0) {
                $(element).closest('.validation-group').removeClass('has-error');
                $(element).closest('.validation-group').find('span.help-block').first().addClass('hidden');
            } else {
                $(element).closest('.form-group').removeClass('has-error');
            }
        },
        errorElement: 'span',
        errorClass: 'help-block fontsize90'
    });
    $.validator.addMethod('minStrict', function (value, el, param) {
        return value > param;
    }, "Amount must be more than {0}");
    $.validator.addMethod(
    "uniquePartyName",
    function (value, element) {
        if (elem.attr('id') == 'startpartyform') {
            return peopleInfo.CountryId;
        }
        else {
            if (myPartyViewModel.partyCodes()[0].PartyName == elem.find('input[name^=partyName]').val().trim()) {
                return true;
            }
        }

        $.ajax({
            type: "get",
            url: "/api/PartyService/IsUniquePartyName",
            data: JSON.stringify({
                PartyName: function () {
                    return elem.find('input[name^=partyName]').val();
                },
                CountryId: function () {
                    if (elem.attr('id') == 'startpartyform') {
                        return peopleInfo.CountryId;
                    }
                    else {
                        return myPartyViewModel.partyCodes()[0].CountryId;
                    }
                },
                OriginalName: function () {

                    if (elem.attr('id') == 'startpartyform') {
                        return "";
                    }
                    else {
                        return myPartyViewModel.partyCodes()[0].PartyName;
                    }
                }
            }),
            contentType: "application/json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                return result
            }
        });
    },
    "PartyName is Already Taken"
);

}
function ParseDate(datetoParse) {
    var result = "";
    var momenttoParse = moment.utc(datetoParse);
    var now = moment().utc();
    if (now.diff(momenttoParse, 'seconds') < 0) {
        result = momenttoParse.local().format('MM/DD/YYYY');
    }
    else if (now.diff(momenttoParse, 'minutes') < 1) {
        result = now.diff(momenttoParse, 'seconds') + " seconds ago";
    } else if (now.diff(momenttoParse, 'minutes') < 60) {
        result = now.diff(momenttoParse, 'minutes') + " minutes ago";
    } else if (now.diff(momenttoParse, 'minutes') < 1440) {
        result = now.diff(momenttoParse, 'hours') + " hours ago";
    }
    else if (now.diff(momenttoParse, 'days') < 7) {
        result = now.diff(momenttoParse, 'days') + " days ago";
    }
    else {
        result = momenttoParse.local().format('MM/DD/YYYY');
    }
    return result;
}
function replaceRange(s, start, end, substitute) {
    return s.substring(0, start) + substitute + s.substring(end);
}
function getCountryCodes() {
    return $.ajax({
        url: "/api/countrycodeservice/getcountrycodes",
        type: "get",
        contentType: "application/json",
        cache: true,
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getIndustryCodes() {
    if (cacheIndustryCodes.length > 0) {
        return cacheIndustryCodes;
    }
    return $.ajax({
        url: "/api/industrycodeservice/getindustrycodes",
        type: "get",
        contentType: "application/text",
        cache: true,
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function getAgendaCodes() {
    if (cacheAgendaCodes.length > 0) {
        return cacheAgendaCodes;
    }
    return $.ajax({
        url: "/api/PartyService/GetAllPoliticalAgendaJson",
        type: "get",
        contentType: "application/text",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    })
}
function processDateParm(dataparms) {
    var dateindex = dataparms.indexOf("Date:");
    if (dateindex > -1) {
        var datalastindex = dataparms.indexOf("<", dateindex);
        var date = dataparms.substring(dateindex + 5, datalastindex);
        var momentDate;
        if (moment.utc(date, "MM/DD/YYYY HH:mm:ss").isValid()) {
            momentDate = moment.utc(date, "MM/DD/YYYY HH:mm:ss a").local().format('llll');
        }
        else {
            momentDate = moment.utc(date, "DD/MM/YYYY HH:mm:ss a").local().format('llll');
        }
        dataparms = replaceRange(dataparms, dateindex,
            datalastindex,
            momentDate);
    }
    return dataparms;
}
function processOnClickParm(dataparms) {
    var onclickindex = dataparms.Parms.indexOf("|onclick:");
    if (onclickindex > -1) {
        var datalastindex = dataparms.Parms.indexOf(")", onclickindex) + 1;
        var onClickUrl = dataparms.Parms.substring(onclickindex + 9, datalastindex);

        dataparms.OnClickUrl = onClickUrl;
        dataparms.Parms = replaceRange(dataparms.Parms, onclickindex,
            datalastindex,
            '');
        processOnClickParm(dataparms);
    }
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}
function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function ordinal_suffix_of(i) {
    var j = i % 10,
        k = i % 100;
    if (j == 1 && k != 11) {
        return i + "st";
    }
    if (j == 2 && k != 12) {
        return i + "nd";
    }
    if (j == 3 && k != 13) {
        return i + "rd";
    }
    return i + "th";
}
$(document).ready(function () {
    $(document).ajaxError(function (event, request, settings) {
        console.log("exception" + settings.url);
        console.log("request.status " + request.status);
        console.log("peopleInfo " + peopleInfo);
        if (request.status == 403 || typeof (peopleInfo) == 'undefined' || peopleInfo.UserId() == 0) {
            console.log("request.status");
            $.ajax({
                type: "get",
                url: '/Account/LogOff',
                success: function (data) {
                    window.location = "/Account/Login";
                }
            });

        }
        ga(
          'send',
          'exception', {
              'exDescription': 'jQuery Ajax Error',
              'url': settings.url,
              'msg': JSON.stringify({
                  result: event.result,
                  status: request.status,
                  statusText: request.statusText,
                  crossDomain: settings.crossDomain,
                  dataType: settings.dataType
              }),
              'userAgent': navigator.userAgent,
              'exFatal': false,
          }
        );



    });
    jQuery.error = function (message) {
        console.log("exception message" + message)

        ga('send', 'exception', {
            'exDescription': 'jQuery Error',
            'exFatal': false,
            'msg': message,
            'userAgent': navigator.userAgent
        });
    }

    postcommentvm = true;
    friendvm = true;
    userNotificationvm = true;
    getAllFriendsPostBank();
    initializeuserNotification("initial");
    applyUserNotificationNavBar();
    $("[data-toggle=tooltip]").tooltip();
    initialzieTour();
    initialzieFirst();
});
function initializeFb() {
    $.loadScript("https://connect.facebook.net/en_US/all.js", function (data, textStatus, jqxhr) {
        FB.init({
            appId: '1627068334180861',
            frictionlessRequests: true
        });
    });
}
function sendFbmsg() {
    FB.ui({
        method: 'send',
        link: 'https://thinkara.com',
    });
}
function getAllFriendsPostBank() {
    $.when(getDataFriend(), getJsonCodeData(), getCountryTaxInfo(), getUserbankAccountasync(), getWebUserInfo(), getCountryCodes())
    .done(function (a4, a5, a6, a7, a8, a9) {
        $.when(getDataPostComment(false), getTimeLineView(), getpostview())
         .done(function (a1, a2, a3) {
             postData = a1[0];
             commentsViewTemplate = a2[0];
             commentsTreeViewTemplate = a2[0];
             commentsTreeViewTemplate = commentsTreeViewTemplate.replace("feedcomment", "feedcomment reply");
             postViewTemplate = a3[0];

             ProcessPost(true);
             initializebuyspot();
             initializeFb();
         });
        setupPeopleInfo(a8[0], a9[0]);
        taxViewModelCache = a6;
        taxViewModel = ko.observable();
        taxViewModel = ko.mapping.fromJS(a6[0]);

        if (a4[0].length > 0) {
            for (i = 0; i < a4[0].length; i++) {
                friendViewModel.friendDetailsDTO.push(a4[0][i]);
            }
        }
        if (friendViewModel.friendDetailsDTO().length < 2) {
            isThisFirstLogin();
        }
        ko.mapping.fromJS(a7[0], {}, userBankAccountViewModel = ko.mapping);
        ko.cleanNode($("#accountnav")[0]);
        ko.applyBindings(userBankAccountViewModel, $("#accountnav")[0]);
    });
}
function setupPeopleInfo(result, data) {
    peopleInfo = ko.mapping.fromJS(result);
    peopleInfo.ProfileId = ko.observable(peopleInfo.UserId());
    peopleInfo.RelationDirection = ko.observable('Self');

    friendViewModel.fullName(peopleInfo.FullName());
    friendViewModel.picture(peopleInfo.Picture());
    $("#sidebar").find('.guide').removeClass('hidden');
    for (i = 0; i < data.length; i++) {
        countryCodeViewModel.countries.push(data[i]);
    }
    ko.applyBindings(peopleInfo, $("#webinfonav")[0]);
    ko.applyBindings(peopleInfo, $("#nav-profile")[0]);
    ko.applyBindings(peopleInfo, $("#nav-country")[0]);
    ko.applyBindings(peopleInfo, $("#sendemailinvite")[0]);

    countryCodewithoutownViewModel(countryCodeViewModel.countries().slice(0));
    countryCodewithoutownViewModel.remove(function (item) {
        return item.CountryId == peopleInfo.CountryId();
    });

    if (peopleInfo.IsLeader() == 'true') {
        $("#nav-shield").find('.icon-base1').removeClass('hidden');
        $("#nav-shield").find('.fa-lock').addClass('hidden');
    }
    else {
        $("#nav-shield").popover({
            trigger: 'hover click foucs',
            placement: 'top',
            container: 'body'
        });
    }
}
function sendemailInvites() {
    inviteViewClick('/Friend/SendInvite', inviteCarosuelId);
}
function clickfindfriendtab() {
    inviteViewClick('/Friend/SendInvite', inviteCarosuelId, function () {
        console.log("triggering clikc");
        $("#findfriendtab").trigger("click");
    });
}
function getJsonCodeData() {
    $.getJSON("/Content/cdn/codetables.js", function (data) {
        codeTables = ko.mapping.fromJS(data);
    });
}
function spinInputwtPrefix(self, prefixTitle, maxAmount) {
    self.TouchSpin({
        prefix: prefixTitle,
        initval: 0,
        min: 0,
        max: maxAmount,
        step: 1,
        decimals: 2,
        booster: false,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        mousewheel: true
    });
}
function getUserbankAccount(vm) {
    getUserbankAccountasync().done(function (result) {
        if (typeof vm != 'undefined') {
            vm.AmountAvailable(result.Cash);
        }
        ko.mapping.fromJS(result, {}, userBankAccountViewModel = ko.mapping);
        ko.cleanNode($("#accountnav")[0]);
        ko.applyBindings(userBankAccountViewModel, $("#accountnav")[0]);
    });
}
function getUserbankAccountasync() {
    return $.ajax({
        url: "/api/UserBankAccountService/GetUserBankDetails",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getRandomNumber(min, max) {
    //Returns a random integer between min (inclusive) and max (inclusive)
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
function getWebUserInfo() {
    return $.ajax({
        url: "/api/WebUserService/GetWebUserInfo",
        type: "get",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function isThisFirstLogin() {
    $.ajax({
        url: "/api/WebUserService/IsThisFirstLogin",
        type: "get",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (data) {
            console.log(data);
            if (data == true) {
                $("#tourmodal").modal("show");
            }
        }

    });
}
function getCountryTaxInfo() {
    return $.ajax({
        url: "/api/countrytaxservice/getcountrytax",
        type: "get",
        contentType: "application/json",
        data: {
            taskId: "null"
        },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getUserLevelCss(userlevel, profileid) {
    if (profileid == peopleInfo.UserId()) {
        userLevelScore = peopleInfo.UserLevel();
    }
    else {
        userLevelScore = profileviewModel.peopleInfo.UserLevel();
    }
    if (userlevel <= userLevelScore) {
        return 'btn-shadow '
    }
}
function getUserLevelAvatar(index) {
    var avatar = 'fa-thumbs-up';
    switch (index) {
        case 0:
            avatar = "fa icon-fencing2";
            break;
        case 1:
            avatar = "fa icon-fencing1";
            break;
        case 2:
            avatar = "fa icon-fighter";
            break;
        case 3:
            avatar = "fa icon-tai1";
            break;
        case 4:
            avatar = "fa icon-ninja8";
            break;
        case 5:
            avatar = "fa icon-spy";
            break;
        case 6:
            avatar = "fa icon-ninja7";
            break;
    }
    return avatar;
}
function createGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function arrayFirstIndexOf(array, predicate, predicateOwner) {
    for (var i = 0, j = array.length; i < j; i++) {
        if (predicate.call(predicateOwner, array[i])) {
            return i;
        }
    }
    return -1;
}
function htmlEscape(str) {
    return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
}
function certificateFonttext(degreeId) {
    var result = "offer-red";
    if (degreeId == 1)
        result = "offer-black";
    else if (degreeId == 2)
        result = "offer-success";
    else if (degreeId == 3)
        result = "offer-darkblue";
    return result;
}
function degreeFonttext(degreeId) {
    var result = "text-red";
    if (degreeId == 1)
        result = "text-black";
    else if (degreeId == 2)
        result = "text-success";
    else if (degreeId == 3)
        result = "text-darkblue";
    return result;
}
function viewMyProfile() {
    profileViewClick(userprofile, userprofileCarosuelId, peopleInfo.UserId());
    $("#main").animate({ scrollTop: 0 }, "slow");
}
function viewUserProfile(userProfileId) {
    if (userProfileId >= bankId && userProfileId <= bankId + 243) {
        return false;
    }
    profileViewClick(userprofile, userprofileCarosuelId, userProfileId);
    $("#main").animate({ scrollTop: 0 }, "slow");
}
function viewCountryProfile(countryid) {
    countryProfileViewClick(countryprofile, countryprofileCarosuelId, countryid);
    $("#main").animate({ scrollTop: 0 }, "slow");
}
function addKnobDialCss(item, percent) {
    item.dialColor = ko.computed(function () {
        var result = "#9ABC32";
        if (percent < 50)
            result = "#D53F40";
        if (percent >= 50 && percent < 80)
            result = "#E8B110";
        if (percent >= 80)
            result = "#9ABC32";
        return result;
    });
}
function addKnobDialReverseCss(item, percent) {
    item.dialColor = ko.computed(function () {
        var result = "#D53F40";
        if (percent < 50)
            result = "#9ABC32";
        if (percent >= 50 && percent < 80)
            result = "#E8B110";
        if (percent >= 80)
            result = "#D53F40";
        return result;
    });
}
function addpercentKnob(index, knobid, size) {
    $("#" + knobid + " input[data-index=" + index + "]").knob(
    {
        'readOnly': true, 'min': 0
       , 'max': 100,
        'height': size,
        'width': size,
        'draw': function () {
            $(this.i).val(this.cv.toFixed(1) + '%')
        }
    });
}
function cashTextCss(amount) {
    if (amount > 0) {
        return 'text-success';
    }
    else {
        return 'text-danger';
    }
}
function showLoadingImage(parentId) {
    if (parentId.find('.loading-image').length) {
        return;
    }
    if (parentId.hasClass('panel-heading')) {
        parentId.append('<div class="loading-image marginleft20c"><img height="28px" title="loading..." src="Content/Images/cube.gif" alt="loading..." /></div>');
    }
    else {
        parentId.append('<div class="loading-image pull-right"><img height="28px" title="loading..." src="Content/Images/cube.gif" alt="loading..." /></div>');
    }
}

function hideLoadingImage(parentId) {
    parentId.find('.loading-image').remove();
}
function print_call_stack() {
    var stack = new Error().stack;
    console.log("PRINTING CALL STACK");
    console.log(stack);
}
function validateEmail(email) {
    var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    return re.test(email);
}
function uploadImageObj(source) {
    this.fileName = ko.observable('');
    this.source = source;
    this.blobSasUrl = ko.observable('');
    this.buffer = '';
    this.validImage = false;
    this.imageName = '';
    this.contentLoading = false;
}
function renderImage(ajaxRequest, vm, previewelem, callback) {
    if (ajaxRequest.readyState == 4) { // xmlhttp.status==200 == successful request
        var $image = $('<img src="' + vm.blobSasUrl() + '" alt="" />');
        if (previewelem != null) {
            previewelem.html($image);
        }
        if (typeof (callback) == "function") {
            callback();
        }
        vm.buffer = "";
        vm.validImage = false;
        vm.contentLoading = false;
    }
}
function uploadImage(vm, previewelem, callback) {
    if (vm.validImage == false || vm.buffer == "" || vm.blobSasUrl() == '' || vm.contentLoading == true) {
        return;
    }
    vm.contentLoading = true;
    var ajaxRequest = new XMLHttpRequest();
    ajaxRequest.onreadystatechange = function () {
        return renderImage(ajaxRequest, vm, previewelem, callback)
    };

    try {
        ajaxRequest.open('PUT', vm.blobSasUrl(), true);
        ajaxRequest.setRequestHeader('Content-Type', 'image/jpeg');
        ajaxRequest.setRequestHeader('x-ms-blob-type', 'BlockBlob');
        ajaxRequest.send(vm.buffer);
    }
    catch (e) {
        alert("can't upload the image to server.\n" + e.toString());
    }
}
function handleFileSelect(evt, previewelem, vm, blobName, callback) {
    var files = evt.target.files; // FileList object

    // Loop through the FileList, save them and render image files as thumbnails.
    for (var i = 0, file; file = files[i]; i++) {

        // Create a reader that would read the entire file data as ArrayBuffer
        var reader = new FileReader();
        readImage(file, previewelem, vm);
        reader.onloadend = (function (theFile) {
            return function (e) {
                // Once the reader is done reading the file bytes
                // We will issue an AJAX call against our service to get the SAS URL
                var sasData = {
                    SourceType: vm.source,
                    FileType: getfileNameExt(theFile.name),
                    BlobName: blobName
                };
                $.ajax({
                    type: 'POST',
                    url: sasurl,
                    contentType: "application/json",
                    data: JSON.stringify(sasData),
                    headers: {
                        RequestVerificationToken: Indexreqtoken
                    },
                    success: function (data) {
                        vm.blobSasUrl(data.Url);
                        vm.imageName = data.FileName;
                        vm.buffer = e.target.result;
                        if (typeof (callback) == "function") {
                            callback();
                        }
                    },
                    error: function (res, status, xhr) {
                        alert("can't get sas from the server");
                    }
                });
            };
        })(file);

        // Read in the image file as a data URL, once done the reader.onloadend event is raised
        reader.readAsArrayBuffer(file);
    }
}
function getfileNameExt(filename) {
    var a = filename.split(".");
    if (a.length === 1 || (a[0] === "" && a.length === 2)) {
        return "";
    }
    return a.pop();
}
function readImage(file, previewelem, vm) {
    vm.validImage = true;

    if (file.size > maxFileSize) {
        alert('Filesize is too big');
        vm.validImage = false;
        return;
    }
    if (file.type.length && !imageacceptFileTypes.test(file.type)) {
        alert('Not an accepted file type');
        vm.validImage = false;
        vm.buffer = "";
        vm.fileName('');
        previewelem.html('');
        return;
    }
    var image = new Image();
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function (_file) {
        image.src = _file.target.result;              // url.createObjectURL(file);
        image.onload = function () {
            vm.fileName(file.name);
            previewelem.html('<img src="' + this.src + '"> ' + '<br>');
        };
        image.onerror = function () {
            vm.validImage = false;
            vm.buffer = "";
            vm.fileName('');
            previewelem.html('');
            alert('Invalid file type: ' + file.type);
        };
    };
}
jQuery.loadScript = function (url, callback) {
    var load = true;
    //check all existing script tags in the page for the url
    jQuery('script[type="text/javascript"]')
      .each(function () {
          return load = (url != $(this).attr('src'));
      });
    console.log('load is ' + load);
    if (load) {
        //didn't find it in the page, so load it
        jQuery.ajax({
            type: 'GET',
            url: url,
            dataType: 'script',
            cache: true,
            ifModified: true,
            success: callback

        });
    } else {
        //already loaded so just call the callback
        if (jQuery.isFunction(callback)) {
            callback.call(this);
        };
    };

};
jQuery.loadScriptNoCallBack = function (url) {
    return $.ajax({
        type: 'GET',
        url: url,
        dataType: 'script',
        cache: true,
        ifModified: true
    });
};
jQuery.loadViewNoCallBack = function (url) {
    return $.ajax({
        type: 'GET',
        url: url,
        cache: true,
        ifModified: true
    });
};
function findUrls(text) {
    var source = (text || '').toString();
    var urlArray = [];
    var url;
    var matchArray;

    // Regular expression to find FTP, HTTP(S) and email URLs.
    var regexToken = /(((https?):\/\/)[\-\w@:%_\+.~#?,&\/\/=]+)/gi;

    // Iterate through any URLs in the text.
    while ((matchArray = regexToken.exec(source)) !== null) {
        var token = matchArray[0];
        urlArray.push(encodeURI(token)); //.replace("http://", "h");
    }

    return urlArray;
}
function getWebContent(content, vm, callback) {
    if (content.length < 1) {
        if (typeof (callback) == "function") {
            callback();
        }
        return;
    }
    var Type = "GET";
    var postdata = content.join("[stop]");
    var Url = "/FetchWebContentService/FetchWebContent.svc/GetUrlContent?urls=" + encodeURIComponent(postdata);
    var ContentType = "application/json; charset=utf-8";
    var DataType = "json";


    $.ajax({
        type: Type, //GET or POST or PUT or DELETE verb
        url: Url, // Location of the service        
        contentType: ContentType, // content type sent to server
        dataType: DataType, //Expected data format from server        
        success: function (msg) {//On Successfull service call
            result = msg.GetUrlContentResult;
            for (var i = 0; i < vm.posturlContent.length; i++) {
                var detail = vm.posturlContent[i];
                for (var j = 0; j < result.length; j++) {
                    var item = result[j];
                    if (item.Uri.toLowerCase() == detail.Uri.toLowerCase()) {
                        {

                            vm.posturlContent[i].Title = item.Title;
                            vm.posturlContent[i].Content = item.Content;
                            vm.posturlContent[i].Processed = true;
                        }
                    }
                }

                for (var k = 0; k < content.length; k++) {
                    var item = content[k];
                    if (item.toLowerCase() == detail.Uri.toLowerCase()) {
                        {
                            vm.posturlContent[i].Processed = true;

                        }
                    }
                }
            }
            if (typeof (callback) == "function") {
                callback();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });


}
function buildWebContent(textWithUrl, textSource, isPostSubmit, vm, callback) {

    if ($.grep(vm.posturlContent, function (item) {
        return item.Title != ''
            && item.Processed == true
    }).length >= 3) {
        callback();
        return;
    }

    var uris = findUrls(textWithUrl);
    if (uris.length < 0)
        return;
    CleanUpUrls(textWithUrl, textSource, vm);
    var contentLength = 0;
    if (textSource == 'Post')
        contentLength = vm.posturlContent.length;
    else if (textSource == 'Chat')
        contentLength = 0;
    var sendurls = [];

    if (uris.length > 0) {
        for (var j = 0; j < uris.length; j++) {
            if ($.grep(vm.posturlContent, function (item) { return item.Uri == uris[j] }).length == 0) {
                if (textSource == 'Post')
                    vm.posturlContent.push({ "Title": "", "Content": "", "Uri": uris[j], "Processed": false });
                else if (textSource == 'Chat')
                    vm.posturlContent.push({ "Title": "", "Content": "", "Uri": uris[j], "Processed": false });
            }

        }
        for (var i = 0; i < vm.posturlContent.length; i++) {
            if (vm.posturlContent[i].Processed == false)
                sendurls.push(vm.posturlContent[i].Uri);
        }


        if (isPostSubmit == false) {
            getWebContent(sendurls, vm);
        }
        else {
            getWebContent(sendurls, vm, function () {
                callback();
            });
        }

    } else {

        if (isPostSubmit == true)
            callback();
    }

}
function CleanUpUrls(textWithUrl, textSource, vm) {

    var uris = findUrls(textWithUrl);
    if (uris.length <= 0) {
        var contentLength = 0;
        if (textSource == 'Post')
            vm.posturlContent = [];
        else if (textSource == 'Chat')
            chaturlContent = [];

    }

    $.each(uris, function () {
        var siteUrl = this.valueOf();
        if (textSource == 'Post') {
            console.log("vm.posturlContent");
            console.log(vm.posturlContent);
            vm.posturlContent = $.grep(vm.posturlContent, function (item) {
                if (uris.indexOf(item.Uri) < 0)
                    $('#posturlconetnt [data-url="' + item.Uri + '"]').remove();

                return uris.indexOf(item.Uri) > -1;
            });
        }
        else if (textSource == 'Chat') {
            chaturlContent = $.grep(chaturlContent, function (item) {
                return uris.indexOf(item.Uri) > -1;
            });
        }
    });
}


function updateFriendRelation(btn, profileData) {
    btn.button('loading');
    $.ajax({
        url: "/api/FriendService/UpdateFriendRelation",
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
                if (typeof profileviewModel != 'undefined') {
                    profileviewModel.friendRelationResult.push(1);
                }
                btn.addClass('hidden');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')

        }
    }).always(function () {

        btn.button('reset')
    });
}
function updateFriends() {
    getDataFriend().success(function (data) {
        if (data.length > 0) {
            friendViewModel.friendDetailsDTO.removeAll();
            for (i = 0; i < data.length; i++) {
                friendViewModel.friendDetailsDTO.push(data[i]);
            }
        }
    });
}
function deleteCookie(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
function initialzieTour() {
    // Instance the tour
    tour = new Tour({
        steps: [
        {
            element: "#sendemailinvite",
            title: "&#xf0c0; Social Asset",
            content: "Let's Go earn some Socail Assets ",
            placement: "bottom",
            onNext: function () {
                clickfindfriendtab();
                $('#findfriendtab').waitUntilExists(sendpromise);

            }
        },
        {
            element: "#findfriendtab",
            title: "&#xf0e7; Import Friends",
            content: "Teleport your friends from the other side, or discover those who inhabit here already.",
            placement: "bottom",
            orphan: true,
            onNext: function () {
                $('a[href^="#invite-friends"]').trigger("click");

            }
        },
        {
            element: "#inviteview-box",
            title: "&#xf0e0; Invite Friends",
            content: "Send Invitation to your freinds ",
            orphan: true,
            placement: "top",
            onPrev: function () {
                clickfindfriendtab();
            },
            onNext: function () {
                $('#backbtn').trigger("click");
            }
        },
        {
            element: "#backbtn",
            title: "&#xf015; Hub",
            content: "Back to tile hub  ",
            orphan: false,
            placement: "bottom",
            onPrev: function () {
                sendemailInvites();
            }
        },
        {
            element: "#nav-education",
            title: "&#xf29b; Go To School",
            content: " Want to make more loot? go back to school!",
            orphan: false,
            placement: "bottom"
        },
        {
            element: "#nav-job",
            title: "&#xf2df; Make some money",
            content: " Search for job, apply and start making some serious money",
            orphan: false,
            placement: "bottom"
        },
        {
            element: "#nav-party",
            title: "&#xf25e; Party",
            content: " Start or join a party, attain or maintain your party in power",
            orphan: false,
            placement: "top"
        },
        {
            element: "#nav-election",
            title: "&#xf2c3; Election",
            content: " Run for Office, Make a diffrence!",
            orphan: false,
            placement: "top"
        },
        {
            element: "#nav-lottery",
            title: "&#xf1b2; Got Bored?",
            content: " if you get bored try your luck, make some more money",
            orphan: false,
            placement: "top"
        },
        {
            element: "#nav-shopping-cart",
            title: "&#xf309; Shopping",
            content: "Buy some luxury items to show off, or rental property to kick back and collect rents until the end of time. Buy real state to avoid paying monthly rent for Real state property.",
            orphan: false,
            placement: "bottom"
        },
        {
            element: "#nav-stock",
            title: "&#xf2c1; Trading",
            content: "Trade stocks smartly, qucikly become rich or broke",
            orphan: false,
            placement: "bottom"
        },
        {
            element: "#nav-bankac",
            title: "&#xf2e5; Banking",
            content: "Check your assets, sell/buy gold or silver. Gold is always the golden currency.",
            orphan: false,
            placement: "bottom"
        }
        ,
        {
            element: "#spot-im-root",
            title: "Community",
            content: "Get connected with community.",
            orphan: false,
            placement: "top"
        },
        {
            element: "#nav-bank",
            title: "&#xf25c; Cash-Strapped?",
            content: "Ask for some and you shall recieve.",
            orphan: false,
            placement: "top"
        },
        {
            element: "#nav-profile",
            title: "Profile",
            content: "Check out your virtual portfolio.",
            orphan: false,
            placement: "top"
        }
        ,
          {
              element: "#nav-question-circle",
              title: "Country Budget",
              content: "Leaders can allocate budget for the country.",
              orphan: false,
              placement: "top"
          },
          {
              element: "#nav-tax",
              title: "&#xf2c0; Tax",
              content: "'In this world nothing can be said to be certain, except death and taxes.' : Benjamin Franklin",
              orphan: false,
              placement: "top"
          },
          {
              element: "#nav-gift",
              title: "&#xf2ee; Gift",
              content: "Feeling generous? send some gift spread some +ve enegry.",
              orphan: false,
              placement: "top"
          },
         {
             element: "#nav-country",
             title: "Country's Profile",
             content: "Check out your country's portfolio and ranking.",
             orphan: false,
             placement: "top"
         }
        ]
    });

    tour.init();
}
function startTour() {
    tour.restart();
}

function sendpromise() {
    return (new jQuery.Deferred()).promise();
}

(function ($) {

    $.fn.waitUntilExists = function (handler, shouldRunHandlerOnce, isChild) {
        var found = 'found';
        var $this = $(this.selector);
        var $elements = $this.not(function () { return $(this).data(found); }).each(handler).data(found, true);

        if (!isChild) {
            (window.waitUntilExists_Intervals = window.waitUntilExists_Intervals || {})[this.selector] =
                window.setInterval(function () { $this.waitUntilExists(handler, shouldRunHandlerOnce, true); }, 500)
            ;
        }
        else if (shouldRunHandlerOnce && $elements.length) {
            window.clearInterval(window.waitUntilExists_Intervals[this.selector]);
        }

        return $this;
    }

}(jQuery));
function addpropertyItemCss(item) {
    item.dialColor = ko.computed(function () {
        var result = "#9ABC32";
        if (item.MerchandiseCondition < 50)
            result = "#D53F40";
        if (item.MerchandiseCondition >= 50 && item.MerchandiseCondition < 80)
            result = "#E8B110";
        if (item.MerchandiseCondition >= 80)
            result = "#9ABC32";
        return result;
    });
}
function addpropertycondtionKnob(self) {
    self.knob(
        {
            'readOnly': true, 'min': 0
            , 'max': 100,
            'height': 40,
            'width': 40,
            'draw': function () {
                $(this.i).val(this.cv + '%')
            }
        });
}
function striphtml(myString) {
    return myString.replace(/<(?:.|\n)*?>/gm, '');
}