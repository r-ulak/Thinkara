var merchandiseBuyViewModel;
var merchandiseDetailsViewModelresult;
var merchandiseSellViewModel;
var merchandiseSellViewModelresult;
var merchandiseSummaryViewModel;
var buyMerchandiseView;
var sellMerchandiseView;
var summaryMerchandiseView;
function initializemerchandise() {
    sellMerchandiseView = $("#sellmerchandisecontainer").html();
    buyMerchandiseView = $("#buymerchandisecontainer").html();
    summaryMerchandiseView = $("#merchandiseSummarycontainer").html();

    initializemerchandiseNavigation();
    initializeMerchandiseBuy();
    setupMerchandiseBuy(false);
    initializeMerchandiseSell();
    initializeMerchandiseSummary();
}
function initializemerchandiseNavigation() {
    $('#mer-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#mer-summary") {
            if (merchandiseSummaryViewModel.merchandiseSummaryDTO().length == 0) {
                setupMerchandiseSummary();
                getMerchandiseSummary();
            }
            if (merchandiseSummaryViewModel.merchandiseTopN().length == 0) {
                getTopNPropertyOwner();
            }
        }
        else if (self.attr('href') == "#mer-inventory") {
            if (merchandiseSellViewModel.merchandiseCodes().length == 0) {
                setupMerchandiseSell();
                getMerchandiseInventory(0);
            }
            $("#merchandiseinvshowmore").on("click", function () {
                getMerchandiseInventory(merchandiseSellViewModel.lastMerchandiseTypeId());
            });
        }

    })
}


function initializeMerchandiseSummary() {
    $("#merchandiseSummarycontainer").html(summaryMerchandiseView);
    merchandiseSummaryViewModel = {
        merchandiseSummaryDTO: ko.observableArray(),
        merchandiseTopN: ko.observableArray()
    };
}
function setupMerchandiseSummary() {
    ko.applyBindings(peopleInfo, $("#mymerchandisesummarycontent-box .panel-heading").get(0));
    ko.applyBindings(merchandiseSummaryViewModel,
                  $("#merchandiseSummarycontent-box").get(0));
}
function addSummaryItemCss(item) {
    item.dialColor = ko.computed(function () {
        var result = "#9ABC32";
        if (item.AverageCondition < 50)
            result = "#D53F40";
        if (item.AverageCondition >= 50 && item.AverageCondition < 80)
            result = "#E8B110";
        if (item.AverageCondition >= 80)
            result = "#9ABC32";
        return result;
    });

    item.imageFont = ko.computed(function () {
        var result = "";

        if (item.MerchandiseTypeCode == 1)
            result = "fa icon-sportive21";
        else if (item.MerchandiseTypeCode == 2)
            result = "fa icon-skyscrapers";

        return result;
    });

    return item;
}
function getMerchandiseSummary() {
    $.ajax({
        url: "/api/merchandiseservice/getmerchandisesummary",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    addSummaryItemCss(result[i]);
                    merchandiseSummaryViewModel.merchandiseSummaryDTO.push(result[i]);
                }

                $("#merchandiseSummarycontent-wrapper .dial").knob(
                {
                    'readOnly': true, 'min': 0
                    , 'max': 100,
                    'height': 36,
                    'draw': function () {
                        $(this.i).val(this.cv + '%')
                    }
                });
            }
        }
    });
}
function getTopNPropertyOwner() {
    $.ajax({
        url: "/api/merchandiseservice/gettoptenpropertyowner",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#mymerchandisesummarycontent-box .panel-body")),
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    result[i].Rank = ordinal_suffix_of(i + 1);
                    merchandiseSummaryViewModel.merchandiseTopN.push(result[i]);
                }
                ko.applyBindings(merchandiseSummaryViewModel,
                    $("#merchandiseTopNSummary").get(0));
            }
        }
    }).always(function () {
        hideLoadingImage($("#mymerchandisesummarycontent-box .panel-body"));
    });
}

function initializeMerchandiseSell() {
    $("#sellmerchandisecontainer").html(sellMerchandiseView);

    merchandiseSellViewModel = {
        merchandiseCodes: ko.observableArray(),
        lastMerchandiseTypeId: ko.observable(0),
        Amount: ko.observable(0),
        amountBeforeTax: ko.observable(0),
        tax: ko.observable(0),
        taxRate: ko.observable(taxViewModel.TaxType()[taxincomeCode - 1].TaxPercent()),
        contentLoadTriggered: false,
        AmountAvailable: ko.observable(0),
        AfterMerchandiseRenderAction: function (elem) {
            var self = $(elem).find(".merchandisetype");
            var index = self.data("index");
            if (index == null)
                return;
            slideMerchandiseInput(self, index,
               merchandiseSellViewModel.merchandiseCodes()[index].Quantity, 'S');
            $(elem).find('div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            });
        }
    };
    merchandiseSellViewModel.showSellValidation = ko.computed(function () {
        if (merchandiseSellViewModel.Amount() > 0) {
            return false
        } else {
            return true;
        }
    });

}
function setupMerchandiseSell() {
    if (!!ko.dataFor($("#merchandiseInventorycontent-box .panel-body")[0])) {
        return;
    }
    ko.applyBindings(merchandiseSellViewModel, $("#merchandiseInventorycontent-box .panel-body").get(0));
    ko.applyBindings(merchandiseSellViewModel, $("#merchandiseInventorycontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#merchandiseInventorycontent-box .panel-heading").get(0));
    ko.applyBindings(merchandiseSellViewModel, $("#sellmerchandise").get(0));
    $('#sellmerchandise').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveSellMerchandiseCart(btn);
    });
    $('#merchandiserefresh').click(function () {
        refreshMerchandiseInventory();
    });
    $('#sellmoreproperty').click(function () {
        showMerchandiseInventory();
    });
    $("#merchandiseInventoryItemcontent-box").perfectScrollbar(
 {
     suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
 });
}
function showMerchandiseInventory() {
    $("#sellmerchandisecontainer").html(sellMerchandiseView);
    initializeMerchandiseSell();
    setupMerchandiseSell();
    getMerchandiseInventory(0);
}
function addmerchandiseInvItemCss(item) {
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
function getMerchandiseInventory(lastMerchandiseTypeId) {
    if (merchandiseSellViewModel.contentLoadTriggered == true) {
        return;
    }
    $("#merchandiseinvshowmore").prop("disabled", true);
    $("#merchandiserefresh").prop("disabled", true);
    merchandiseSellViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/MerchandiseService/getmerchandiseinventory",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#merchandiseInventorycontent-box .panel-heading")),
        data: { lastMerchandiseTypeId: lastMerchandiseTypeId },
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    addmerchandiseInvItemCss(result[i]);
                    result[i].SellingUnit = ko.observable(0);
                    result[i].PurchasedAt = ParseDate(result[i].PurchasedAt);
                    result[i].ResaleAt = ko.observable(result[i].PurchasedPrice *
                        (result[i].ResaleRate / 100) * (result[i].MerchandiseCondition / 100));
                    merchandiseSellViewModel.merchandiseCodes.push(result[i]);
                    merchandiseSellViewModel.lastMerchandiseTypeId(result[i].MerchandiseTypeId);
                    addmerchandiseKnob(result[i].MerchandiseTypeId);
                    hideLoadingImage($("#merchandiseInventorycontent-box .panel-body"));
                }
            }
            $("#merchandiseinvshowmore").prop("disabled", false);
            $("#merchandiserefresh").prop("disabled", false);
        }
    }).always(function () {
        merchandiseSellViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#merchandiseInventorycontent-box .panel-heading"));
    });

}
function saveSellMerchandiseCart(btn) {
    var sellingItems = ko.utils.arrayFilter(merchandiseSellViewModel.merchandiseCodes(),
      function (item) {
          return item.SellingUnit() > 0;
      });
    $.ajax({
        url: "/api/MerchandiseService/savesellmerchandisecart",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(sellingItems),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            merchandiseSellViewModelresult = ko.mapping.fromJS(result);
            if (merchandiseSellViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#merchandiseInventorycontent-box").html("");
                $("#merchandiseInventorycontent-box").addClass("hidden");
                $("#merchandiseInventorycontent-submit").removeClass("hidden");
                $("#sellmoreproperty").removeClass("hidden");
                $("#merchandiseInventorycontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(merchandiseSellViewModelresult, $("#merchandiseInventorycontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#sellmerchandisecontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function refreshMerchandiseInventory() {
    merchandiseSellViewModel.merchandiseCodes.removeAll();
    merchandiseSellViewModel.lastMerchandiseTypeId(0);
    getMerchandiseInventory(0);
}


function initializeMerchandiseBuy() {
    $("#buymerchandisecontainer").html(buyMerchandiseView);
    merchandiseBuyViewModel = {
        lastmerchandisetypeIdauto: ko.observable(0),
        lastmerchandisetypeIdstate: ko.observable(0),
        lastmerchandisetypeIdstate: ko.observable(0),
        merchandiseCodes: ko.observableArray(),
        amountBeforeTax: ko.observable(0),
        tax: ko.observable(0),
        taxRate: ko.observable(taxViewModel.TaxType()[taxpropertyCode - 1].TaxPercent()),
        Amount: ko.observable(0),
        AmountAvailable: ko.observable(0),
        AfterMerchandiseRenderAction: function (elem) {
            var self = $(elem).find(".merchandisetype");
            var index = self.data("index");
            if (index == null)
                return;
            slideMerchandiseInput(self, index, 10, 'B');

            $(elem).find('div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            });
        }
    };
    merchandiseBuyViewModel.automerchandiseCount = ko.computed(function () {
        return ko.utils.arrayFilter(merchandiseBuyViewModel.merchandiseCodes(),
            function (item) {
                return item.MerchandiseTypeCode == 1;
            }).length;
    });
    merchandiseBuyViewModel.statemerchandiseCount = ko.computed(function () {
        return ko.utils.arrayFilter(merchandiseBuyViewModel.merchandiseCodes(),
            function (item) {
                return item.MerchandiseTypeCode == 2;
            }).length;
    });

}
function setupMerchandiseBuy(buymore) {
    if (!buymore) {
        getMerchandiseUserbankAccount();
        getMerchandiseViewModel(1);
        getMerchandiseViewModel(2);
    }
    $('#savemerchandise').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveMerchandiseCart(btn);
    });
    setmerchandiseScorllbar();
    $('#buymoremerchandise').click(function () {
        $("#buymerchandisecontainer").html(buyMerchandiseView);
        merchandiseBuyViewModel.tax(0);
        merchandiseBuyViewModel.Amount(0);
        for (var i = 0; i < merchandiseBuyViewModel.merchandiseCodes().length; i++) {
            merchandiseBuyViewModel.merchandiseCodes()[i].Quantity(0);
        }
        setupMerchandiseBuy(true);
    });
    $("#autoshowmore").on("click", function () {
        $("#autoshowmore").addClass('hidden');
        getMerchandiseViewModel(1);
    });
    $("#stateshowmore").on("click", function () {
        $("#stateshowmore").addClass('hidden');
        getMerchandiseViewModel(2);
    });

    merchandiseBuyViewModel.ShowValidation = ko.computed(function () {
        if (merchandiseBuyViewModel.AmountAvailable() < 0) {
            return true;
        }
        else {
            return false;
        }
    });
    merchandiseBuyViewModel.showBuyValidation = ko.computed(function () {
        if (merchandiseBuyViewModel.AmountAvailable() < 0 || merchandiseBuyViewModel.Amount() == 0) {
            return true;
        }
        else {
            return false;
        }
    });
    ko.applyBindings(merchandiseBuyViewModel, $("#mymerchandisecontent-box .panel-body").get(0));
    ko.applyBindings(merchandiseBuyViewModel, $("#mymerchandisecontent-box .panel-footer").get(2));
    ko.applyBindings(peopleInfo, $("#mymerchandisecontent-box .panel-heading").get(0));
    ko.applyBindings(merchandiseBuyViewModel, $("#savemerchandise").get(0));
}
function setmerchandiseScorllbar() {

    $("#realstate .panel-body, #automobile .panel-body").perfectScrollbar(
       {
           suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
       });
    $('.accordionmerchandise').on('shown.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').addClass('active');
        //$(e.target).animate({ height: "280" }, 600);
    });

    $('.accordionmerchandise').on('hidden.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').removeClass('active');
    });
}
function getMerchandiseUserbankAccount() {
    getUserbankAccount(merchandiseBuyViewModel);
}
function saveMerchandiseCart(btn) {
    var boughtItems = ko.utils.arrayFilter(merchandiseBuyViewModel.merchandiseCodes(),
         function (item) {
             return item.Quantity() > 0;
         });

    $.ajax({
        url: "/api/MerchandiseService/savemerchandisecart",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(boughtItems),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            merchandiseDetailsViewModelresult = ko.mapping.fromJS(result);
            if (merchandiseDetailsViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mymerchandisecontent-box").html("");
                $("#mymerchandisecontent-box").addClass("hidden");
                $("#buymoremerchandise").removeClass("hidden");
                $("#mymerchandisecontent-submit").removeClass("hidden");
                $("#mymerchandisecontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(merchandiseDetailsViewModelresult, $("#mymerchandisecontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#buymerchandisecontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function getMerchandiseViewModel(codetype) {
    var typeId = 0;
    if (codetype == 1) {
        typeId = merchandiseBuyViewModel.lastmerchandisetypeIdauto();
    }
    else if (codetype == 2) {
        typeId = merchandiseBuyViewModel.lastmerchandisetypeIdstate();
    }
    $.ajax({
        url: "/api/MerchandiseService/getmerchandisetypes",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: {
            merchandiseTypeId: typeId,
            merchandiseTypeCode: codetype
        },
        beforeSend: showLoadingImage($("#mer-buymerchandise .panel-heading").eq(0)),
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    result[i].Quantity = ko.observable(0);
                    merchandiseBuyViewModel.merchandiseCodes.push(result[i]);
                }
                if (codetype == 1) {
                    merchandiseBuyViewModel.lastmerchandisetypeIdauto(result[result.length - 1].MerchandiseTypeId);
                }
                else if (codetype == 2) {
                    typeId = merchandiseBuyViewModel.lastmerchandisetypeIdstate(result[result.length - 1].MerchandiseTypeId);
                }
                hideLoadingImage($("#mer-buymerchandise .panel-heading").eq(0));
                $("#stateshowmore").removeClass('hidden');
                $("#autoshowmore").removeClass('hidden');

            }
        }
    }).always(function () {
        hideLoadingImage($("#mer-buymerchandise .panel-heading").eq(0));
    });;
}

function addmerchandiseKnob(merchandiseTypeId) {
    $("#merchandiseInventorycontent-wrapper input[data-id=" + merchandiseTypeId + "]").knob(
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
function slideMerchandiseInput(self, index, qtyRange, tradetype) {
    self.simpleSlider();
    qtyMerchandiseBind(self, qtyRange, tradetype);
}

function qtyMerchandiseBind(self, qtyRange, tradetype) {
    self.bind("slider:changed", function (event, data) {
        var item = (event.currentTarget);
        var index = $(item).data("index");
        console.log(qtyRange);
        var qty = parseInt("0" + data.value * qtyRange, 10);
        if (tradetype == 'B') {
            merchandiseBuyViewModel.merchandiseCodes()[index].Quantity(qty);
            computemerchandiseTotal(tradetype, merchandiseBuyViewModel);
        }
        else if (tradetype == 'S') {
            merchandiseSellViewModel.merchandiseCodes()[index].SellingUnit(qty);
            computemerchandiseTotal(tradetype, merchandiseSellViewModel);
        }
    });
}
function computemerchandiseTotal(tradetype, vm) {

    var total = 0;
    var cost = 0;
    var quantity = 0;
    for (var p = 0; p < vm.merchandiseCodes().length; ++p) {
        if (tradetype == 'B') {
            cost = vm.merchandiseCodes()[p].Cost;
            quantity = vm.merchandiseCodes()[p].Quantity();
        } else if (tradetype == 'S') {
            cost = vm.merchandiseCodes()[p].ResaleAt();
            quantity = vm.merchandiseCodes()[p].SellingUnit();
        }
        total += cost * quantity;
    }
    vm.amountBeforeTax(total);
    if (tradetype == 'B') {
        vm.tax(total * vm.taxRate() / 100);
        total += vm.tax();
    }
    else if (tradetype == 'S') {
        vm.tax(total * vm.taxRate() / 100);
        total -= vm.tax();
    }

    var currentTotal = vm.Amount();
    var available = vm.AmountAvailable();
    vm.Amount(total);
    if (tradetype == 'B') {
        vm.AmountAvailable(available - (total - currentTotal));
    }
    else if (tradetype == 'S') {
        vm.AmountAvailable(total + (available - currentTotal));

    }
}
