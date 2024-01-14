var bankviewModel;
var bankbuyMetalviewModel;
var banksellMetalviewModel;
var buyMetalView = "";
var sellMetalView = "";
var bankstmtView = "";
var bankrichView = "";
var capitalTypes;
var fundTypes;
function initializeBank() {

    initializeBankView();
    setupBankView();
    initializeBankStmtView();
    initializeBankBuyMetalView();
    initializeBankSellMetalView();
    initializeRichView();

    initializeBankNavigation();

}
function initializeBankNavigation() {
    $('#bank-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#bank-buymetal") {
            if (bankbuyMetalviewModel.capitalTypecodes().length == 0
                && bankbuyMetalviewModel.contentLoadTriggered == false) {
                showLoadingImage($("#bank-buymetal"))
                bankbuyMetalviewModel.contentLoadTriggered = true;
                $.when(getbuymetalView(), getallCapitalType())
                       .done(function (a1, a2) {
                           buyMetalView = a1;
                           capitalTypes = a2;
                           $('#bank-buymetal').html(buyMetalView[0]);
                           bankbuyMetalviewModel.contentLoadTriggered = false;
                           setupBankBuyMetalView();
                           hideLoadingImage($("#bank-buymetal"))
                       });
            }
        }
        else if (self.attr('href') == "#bank-sellmetal") {
            if (banksellMetalviewModel.capitalTypecodes().length == 0
                && banksellMetalviewModel.contentLoadTriggered == false
                ) {
                showLoadingImage($("#bank-sellmetal"))
                banksellMetalviewModel.contentLoadTriggered = true;
                $.when(getsellmetalView(), getallCapitalType())
                       .done(function (a1, a2) {
                           sellMetalView = a1;
                           capitalTypes = a2;
                           $('#bank-sellmetal').html(sellMetalView[0]);
                           setupBankSellMetalView();
                           banksellMetalviewModel.contentLoadTriggered = false;
                           hideLoadingImage($("#bank-sellmetal"))
                       });
            }
        }
        else if (self.attr('href') == "#bank-stmt") {
            if (bankstmtviewModel.fundTypes().length == 0
                && bankstmtviewModel.contentLoadTriggered == false) {
                showLoadingImage($("#bank-stmt"))
                bankstmtviewModel.contentLoadTriggered = true;
                $.when(getbankstmtView(), getallFundType())
                       .done(function (a1, a2) {
                           bankstmtView = a1;
                           fundTypes = a2;
                           $('#bank-stmt').html(bankstmtView[0]);
                           hideLoadingImage($("#bank-stmt"))
                           bankstmtviewModel.contentLoadTriggered = false;
                           setupBankstmtView();
                       });
            }
        }
        else if (self.attr('href') == "#bank-rich") {
            if (bankrichviewModel.assetTopN().length == 0
               && bankrichviewModel.contentLoadTriggered == false) {
                showLoadingImage($("#bank-rich"))
                bankrichviewModel.contentLoadTriggered = true;
                $.when(getbankrichView(), gettopNRichest())
                       .done(function (a1, a2) {
                           bankrichView = a1;
                           $('#bank-rich').html(bankrichView[0]);
                           if (a2[0].length > 0) {
                               for (var i = 0; i < a2[0].length; i++) {
                                   a2[0][i].Rank = ordinal_suffix_of(i + 1);
                                   bankrichviewModel.assetTopN.push(a2[0][i]);
                               }
                           }
                           bankrichviewModel.contentLoadTriggered = false;
                           setupRichView();
                           hideLoadingImage($("#bank-rich"))
                       });
            }
        }
    });
}


function initializeBankView() {
    bankviewModel = {
        contentLoadTriggered: false,
        bankViewDetails: ko.observableArray([]),
        totalAsset: ko.observable(0),
        totalDebt: ko.observable(0)
    };
}
function setupBankView() {
    ko.applyBindings(bankviewModel, $("#bankview-box .panel-body").get(0));
    ko.applyBindings(bankviewModel, $("#bankview-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#bankview-box .panel-heading").get(0));
    getBankAccountViewInfo();
}
function getBankAccountViewInfo() {
    if (bankviewModel.contentLoadTriggered == true) {
        return;
    }
    bankviewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/UserBankAccountService/GetBankViewDetails",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#bankview-box .panel-body")),
        success: function (result) {
            if (result.length > 0) {
                bankviewModel.bankViewDetails.removeAll();
                var totalAsset = 0;
                var totalDebt = 0;
                for (var i = 0; i < result.length; i++) {
                    if (result[i].CapitalType != 'LoanLeftToPay') {
                        totalAsset += result[i].Total;
                    }
                    else {
                        totalDebt += result[i].Total;
                    }
                    bankviewModel.bankViewDetails.push(result[i]);
                    if (result[i].CapitalType == 'Cash') {
                        userBankAccountViewModel.Cash(result[i].Total);
                    }
                }
                bankviewModel.totalAsset(totalAsset);
                bankviewModel.totalDebt(totalDebt);
            }
        }
    }).always(function () {
        bankviewModel.contentLoadTriggered = false;
        hideLoadingImage($("#bankview-box .panel-body"));
    });

}

function initializeBankBuyMetalView() {
    bankbuyMetalviewModel = {
        contentLoadTriggered: false,
        capitalTypecodes: ko.observableArray([]),
        amount: ko.observable(0),
        amountAvailable: ko.observable(userBankAccountViewModel.Cash()),
        afterCapitalRender: function (elem) {
            var self = $(elem).find(".capitalType");
            var index = self.data("index");
            console.log("index {0}", index);
            if (index == null)
                return;
            qtyMetalBind(self, 100, 'B');
            self.simpleSlider();
            $(elem).find('div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            });
        }
    };
}
function getbuymetalView() {
    if (buyMetalView != "") {
        return buyMetalView;
    }
    return $.ajax("/BankAccount/BuyMetal");
}
function getallCapitalType() {
    if (typeof capitalTypes != 'undefined') {
        return capitalTypes;
    }
    return $.ajax({
        url: "/api/UserBankAccountService/GetMetalPrices",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function setupBankBuyMetalView() {
    if (bankbuyMetalviewModel.capitalTypecodes().length == 0) {
        var buycapitals = $.extend(true, [], capitalTypes[0]);
        for (var i = 0; i < buycapitals.length; i++) {
            buycapitals[i].Quantity = ko.observable(0);
            bankbuyMetalviewModel.capitalTypecodes.push(buycapitals[i]);
        }
    }
    ko.applyBindings(bankbuyMetalviewModel, $("#bankbuy-box .panel-body").get(0));
    ko.applyBindings(bankbuyMetalviewModel, $("#bankbuy-box .panel-footer").get(0));
    ko.applyBindings(bankbuyMetalviewModel, $("#buymetal").get(0));
    ko.applyBindings(peopleInfo, $("#bankbuy-box .panel-heading").get(0));
    $("#bankbuy-box .panel-body").perfectScrollbar(
       {
           suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
       });
    $('#buymetal').click(function () {
        var btn = $(this)
        btn.button('loading')
        buymetalCart(btn);
    });
    $('#buymoremetal').click(function () {
        $('#bank-buymetal').html(buyMetalView[0]);
        initializeBankBuyMetalView();
        setupBankBuyMetalView();
    });
}
function buymetalCart(btn) {
    var boughtItems = ko.utils.arrayFilter(bankbuyMetalviewModel.capitalTypecodes(),
        function (item) {
            return item.Quantity() > 0;
        });

    if (boughtItems.length == 0) {
        btn.button('reset')
        return;
    }
    var collectData = {
        GoldDelta: bankbuyMetalviewModel.capitalTypecodes()[0].Quantity(),
        SilverDelta: bankbuyMetalviewModel.capitalTypecodes()[1].Quantity()
    };

    $.ajax({
        url: "/api/UserBankAccountService/SaveBuyMetal",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(collectData),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var buymetalresult = ko.mapping.fromJS(result);
            if (buymetalresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#bankbuy-box").html("");
                $("#bankbuy-box").addClass("hidden");
                $("#buymoremetal").removeClass("hidden");
                $("#bankbuy-submit").removeClass("hidden");
                $("#bankbuy-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(buymetalresult, $("#bankbuy-submit").get(0));
                $("#main").animate({ scrollTop: $("#bankbuycontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}

function initializeBankSellMetalView() {
    banksellMetalviewModel = {
        contentLoadTriggered: false,
        capitalTypecodes: ko.observableArray([]),
        amount: ko.observable(0),
        afterCapitalRender: function (elem) {
            var self = $(elem).find(".capitalType");
            var index = self.data("index");
            console.log("index {0}", index);

            if (index == null)
                return;
            self.simpleSlider();
            qtyMetalBind(self, banksellMetalviewModel.capitalTypecodes()[index].Quantity(), 'S');

            $(elem).find('div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            });
        }
    };
}
function getsellmetalView() {
    if (sellMetalView != "") {
        return sellMetalView;
    }
    return $.ajax("/BankAccount/SellMetal");
}
function setupBankSellMetalView() {

    if (banksellMetalviewModel.capitalTypecodes().length == 0) {
        var sellcapitals = $.extend(true, [], capitalTypes[0]);
        for (var i = 0; i < sellcapitals.length; i++) {
            var qty = 0;
            if (i == 0) {
                qty = userBankAccountViewModel.Gold();
            }
            else if (i == 1) {
                qty = userBankAccountViewModel.Silver();
            }
            sellcapitals[i].Quantity = ko.observable(qty);
            sellcapitals[i].SellingUnit = ko.observable(0);
            banksellMetalviewModel.capitalTypecodes.push(sellcapitals[i]);
        }
    }
    ko.applyBindings(banksellMetalviewModel, $("#banksell-box .panel-body").get(0));
    ko.applyBindings(banksellMetalviewModel, $("#banksell-box .panel-footer").get(0));
    ko.applyBindings(banksellMetalviewModel, $("#sellmetal").get(0));
    ko.applyBindings(peopleInfo, $("#banksell-box .panel-heading").get(0));
    $("#banksell-box .panel-body").perfectScrollbar(
       {
           suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
       });
    $('#sellmetal').click(function () {
        var btn = $(this)
        btn.button('loading')
        sellmetalCart(btn);
    });
    $('#sellmoremetal').click(function () {
        $('#bank-sellmetal').html(sellMetalView[0]);
        initializeBankSellMetalView();
        setupBankSellMetalView();
    });
}
function sellmetalCart(btn) {

    if (banksellMetalviewModel.amount <= 0) {
        btn.button('reset')
        return;
    }
    var collectData = {
        GoldDelta: -banksellMetalviewModel.capitalTypecodes()[0].SellingUnit(),
        SilverDelta: -banksellMetalviewModel.capitalTypecodes()[1].SellingUnit()
    };

    $.ajax({
        url: "/api/UserBankAccountService/SaveSellMetal",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(collectData),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var sellmetalResult = ko.mapping.fromJS(result);
            if (sellmetalResult.StatusCode() == 200) {
                userNotificationPollOnce();
                setTimeout(function () {
                    getUserbankAccount();
                }, 5000);
                btn.addClass("hidden");
                $("#banksell-box").html("");
                $("#banksell-box").addClass("hidden");
                $("#sellmoremetal").removeClass("hidden");
                $("#banksell-submit").removeClass("hidden");
                $("#banksell-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(sellmetalResult, $("#banksell-submit").get(0));
                $("#main").animate({ scrollTop: $("#banksellcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}

function qtyMetalBind(self, qtyRange, tradetype) {
    console.log("self", self);
    self.bind("slider:changed", function (event, data) {
        var item = (event.currentTarget);
        var index = $(item).data("index");
        var qty = parseInt("0" + data.value * qtyRange, 10);
        console.log("Quantity {0}", qty);
        console.log("tradetype {0}", tradetype);

        if (tradetype == 'B') {
            bankbuyMetalviewModel.capitalTypecodes()[index].Quantity(qty);
            computemetalTotal(tradetype, bankbuyMetalviewModel);
        }
        else if (tradetype == 'S') {
            banksellMetalviewModel.capitalTypecodes()[index].SellingUnit(qty);
            computemetalTotal(tradetype, banksellMetalviewModel);
        }
    });
}
function computemetalTotal(tradetype, vm) {

    var total = 0;
    var cost = 0;
    var quantity = 0;
    for (var p = 0; p < vm.capitalTypecodes().length; ++p) {
        if (tradetype == 'B') {
            cost = vm.capitalTypecodes()[p].Cost;
            quantity = vm.capitalTypecodes()[p].Quantity();
        } else if (tradetype == 'S') {
            cost = vm.capitalTypecodes()[p].Cost;
            quantity = vm.capitalTypecodes()[p].SellingUnit();
        }
        console.log("cost  {0}", cost);
        console.log("tradetype {0}", tradetype);
        console.log("quantity {0}", quantity);
        total += cost * quantity;
    }
    console.log(total);
    vm.amount(total);

}


function initializeBankStmtView() {
    bankstmtviewModel = {
        contentLoadTriggered: false,
        fundTypes: ko.observableArray([]),
        bankstmtlist: ko.observableArray([]),
        lastDateTime: ko.observable(' '),
    };
}
function setupBankstmtView() {
    for (var i = 0; i < fundTypes[0].length; i++) {
        bankstmtviewModel.fundTypes.push(fundTypes[0][i]);
    }

    ko.applyBindings(bankstmtviewModel, $("#bankstmt-box .panel-body").get(0));
    ko.applyBindings(bankstmtviewModel, $("#bankstmt-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#bankstmt-box .panel-heading").get(0));
    $("#bankstmtshowmore").on("click", function () {
        getBankStmt();
    });
    getBankStmt();
    $("#bankstmt-box .panel-body").perfectScrollbar(
   {
       suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
   });

}
function getBankStmt() {
    if (bankstmtviewModel.contentLoadTriggered == true) {
        return;
    }
    bankstmtviewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/UserBankAccountService/GetBankStatement",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: { lastDateTime: bankstmtviewModel.lastDateTime() },
        beforeSend: showLoadingImage($("#bankstmt-box .panel-heading")),
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    result[i].FundCode =
                        bankstmtviewModel.fundTypes()[result[i].FundType - 1].Code;
                    result[i].ImageFont =
                        bankstmtviewModel.fundTypes()[result[i].FundType - 1].ImageFont;
                    if (result[i].SourceId == peopleInfo.UserId()) {
                        result[i].TrnCss = 'fa fa-minus-circle text-danger';
                    }
                    else {
                        result[i].TrnCss = 'fa fa-plus-circle text-success';
                    }
                    bankstmtviewModel.bankstmtlist.push(result[i]);
                }
                bankstmtviewModel.lastDateTime(result[result.length - 1].CreatedAT);
            }
        }
    }).always(function () {
        bankstmtviewModel.contentLoadTriggered = false;
        hideLoadingImage($("#bankstmt-box .panel-heading"))
    });

}
function getbankstmtView() {
    if (bankstmtView != "") {
        return bankstmtView;
    }
    return $.ajax("/BankAccount/BankStatement");
}
function getallFundType() {
    if (typeof fundTypes != 'undefined') {
        return fundTypes;
    }
    return $.ajax({
        url: "/api/UserBankAccountService/GetFundTypes",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}

function initializeRichView() {
    bankrichviewModel = {
        contentLoadTriggered: false,
        assetTopN: ko.observableArray([])
    };
}
function getbankrichView() {
    if (bankrichView != "") {
        return bankrichView;
    }
    return $.ajax("/BankAccount/Top10Richesht");
}
function gettopNRichest() {
    return $.ajax({
        url: "/api/UserBankAccountService/GetTopTenRichest",
        type: "get",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function setupRichView() {

    ko.applyBindings(bankrichviewModel, $("#bankrich-box .panel-body").get(0));
    ko.applyBindings(peopleInfo, $("#bankrich-box .panel-heading").get(0));
}
