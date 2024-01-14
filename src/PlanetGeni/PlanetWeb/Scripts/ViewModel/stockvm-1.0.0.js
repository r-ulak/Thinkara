var stockBuyViewModel;
var stockBuyViewModelresult;
var stockSellViewModel;
var stockSellViewModelresult;
var stockSummaryViewModel;
var buyStockView;
var sellStockView;
var orderStockView;
var summaryStockView;
function initializestock() {
    buyStockView = $("#buystockcontainer").html();
    sellStockView = $("#sellstockcontainer").html();
    orderStockView = $("#stockordercontainer").html();
    summaryStockView = $("#stockSummarycontainer").html();
    initializestockNavigation();
    initializeStockBuy();
    setupStockBuy();
    initializeStockSell();
    initializeStockOrder();
    initializeStockSummary();
}
function initializestockNavigation() {
    $('#stock-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#stock-summary") {
            if (stockSummaryViewModel.stockTopN().length == 0
                 && stockSummaryViewModel.contentLoadTriggered == false) {
                stockSummaryViewModel.contentLoadTriggered = true;
                setupStockSummary();
                showLoadingImage($("#mystocksummarycontent-box .panel-body"))
                $.when(getStockSummary(), getTopNStockOwner())
                     .done(function (a1, a2) {
                         if (a2[0].length > 0) {
                             for (var i = 0; i < a2[0].length; i++) {
                                 a2[0][i].Rank = ordinal_suffix_of(i + 1);
                                 stockSummaryViewModel.stockTopN.push(a2[0][i]);
                             }
                         }
                         if (a1[0].length > 0) {
                             for (var i = 0; i < a1[0].length; i++) {
                                 stockSummaryChangeCss(a1[0][i]);
                                 stockSummaryViewModel.stockCodes.push(a1[0][i]);
                             }
                         }
                         stockSummaryViewModel.contentLoadTriggered = false;
                         hideLoadingImage($("#mystocksummarycontent-box .panel-body"))
                     });
            }
        }
        else if (self.attr('href') == "#stock-sell") {
            if (stockSellViewModel.stockCodes().length == 0
                 && stockSellViewModel.contentLoadTriggered == false) {
                setupStockSell();
                getUserStockSell(0);
            }
        }
        else if (self.attr('href') == "#stock-order") {
            if (stockOrderViewModel.stockCodes().length == 0
                && stockOrderViewModel.contentLoadTriggered == false) {
                setupStockOrder();
                getStockOrder(0);
            }
        }
    })
}

function initializeStockBuy() {
    $("#buystockcontainer").html(buyStockView);
    stockBuyViewModel = {
        contentLoadTriggered: false,
        stockCodes: ko.observableArray(),
        Amount: ko.observable(0),
        AmountAvailable: ko.observable(userBankAccountViewModel.Cash()),
        AfterStockRenderAction: function (elem) {
            var self = $(elem).find(".stockQuantity");
            var index = self.data("index");
            if (index == null)
                return;
            slideStockInput(self, index, 0, 1000, 'B');

            self = $(elem).find(".stockPriceOffer");
            index = self.data("index");
            if (index == null)
                return;
            slideStockInput(self, index, parseInt(self.data('currentvalue')), 0, 'B');
            $(elem).find('div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            });
        },
        onLimitValueCheck: function (data, event) {
            var self = $(event.currentTarget);
            self.closest('.ordertypegroup').siblings('.stockofferprice').removeClass('hidden')
            var index = self.data('index');
            stockBuyViewModel.stockCodes()[index].OrderType("L");
            computestockTotal('B', stockBuyViewModel);
            return true;
        },
        onMarketValueCheck: function (data, event) {
            var self = $(event.currentTarget);
            self.closest('.ordertypegroup').siblings('.stockofferprice').addClass('hidden')
            var index = self.data('index');
            stockBuyViewModel.stockCodes()[index].OrderType("M");
            computestockTotal('B', stockBuyViewModel);
            return true;
        }
    };
    stockBuyViewModel.ShowValidation = ko.computed(function () {
        if (stockBuyViewModel.AmountAvailable() >= 0) {
            return false;
        }
        else {
            return true;
        }
    });
}
function setupStockBuy() {
    ko.applyBindings(stockBuyViewModel, $("#mybuystockcontent-box .panel-body").get(0));
    ko.applyBindings(stockBuyViewModel, $("#mybuystockcontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#mybuystockcontent-box .panel-heading").get(0));
    ko.applyBindings(stockBuyViewModel, $("#buystock").get(0));
    getCurrentstockViewModel();
    $('#buystock').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveBuyStockCart(btn);
    });
    $("#stockbuypanelbody").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
        });
    $('#buymorestock').click(function () {
        initializeStockBuy();
        setupStockBuy();
    });
}
function getCurrentstockViewModel() {
    if (stockBuyViewModel.contentLoadTriggered == true) {
        return;
    }
    stockBuyViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/StockService/GetCurrentStock",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#mybuystockcontent-box .panel-body")),
        success: function (result) {
            if (result.length > 0) {
                stockBuyViewModel.stockCodes.removeAll();
                for (i = 0; i < result.length; i++) {
                    result[i].Quantity = ko.observable(0);
                    result[i].OfferPrice = ko.observable(0);
                    result[i].OrderType = ko.observable("M");
                    stockDayChangeCss(result[i]);
                    stockBuyViewModel.stockCodes.push(result[i]);
                }
            }
        }
    }).always(function () {
        stockBuyViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#mybuystockcontent-box .panel-body"));
    });
}
function saveBuyStockCart(btn) {
    var boughtItems = ko.utils.arrayFilter(stockBuyViewModel.stockCodes(),
         function (item) {
             return item.Quantity() > 0;
         });

    $.ajax({
        url: "/api/StockService/saveBuyStockCart",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(boughtItems),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            stockBuyViewModelresult = ko.mapping.fromJS(result);
            if (stockBuyViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mybuystockcontent-box").html("");
                $("#mybuystockcontent-box").addClass("hidden");
                $("#buymorestock").removeClass("hidden");
                $("#mybuystockcontent-submit").removeClass("hidden");
                $("#mybuystockcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(stockBuyViewModelresult, $("#mybuystockcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#buystockcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}

function initializeStockSell() {
    $("#sellstockcontainer").html(sellStockView);
    stockSellViewModel = {
        stockCodes: ko.observableArray(),
        amountBeforeTax: ko.observable(0),
        tax: ko.observable(0),
        taxRate: ko.observable(taxViewModel.TaxType()[taxstockCode - 1].TaxPercent()),
        Amount: ko.observable(0),
        AmountAvailable: ko.observable(0),
        lastDateTime: ko.observable(''),
        contentLoadTriggered: false,
        AfterStockRenderAction: function (elem) {
            var self = $(elem).find(".stockQuantity");
            var index = self.data("index");
            if (index == null)
                return;

            slideStockInput(self, index, 0,
                stockSellViewModel.stockCodes()[index].PurchasedUnit, 'S');

            self = $(elem).find(".stockPriceOffer");
            index = self.data("index");
            if (index == null)
                return;
            slideStockInput(self, index, parseInt(self.data('currentvalue')), 0, 'S');
            $(elem).find('div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            });
        },
        onLimitValueCheck: function (data, event) {
            var self = $(event.currentTarget);
            self.closest('.ordertypegroup').siblings('.stockofferprice').removeClass('hidden')
            var index = self.data('index');
            stockSellViewModel.stockCodes()[index].OrderType("L");
            computestockTotal('S', stockSellViewModel);
            return true;
        },
        onMarketValueCheck: function (data, event) {
            var self = $(event.currentTarget);
            self.closest('.ordertypegroup').siblings('.stockofferprice').addClass('hidden')
            var index = self.data('index');
            stockSellViewModel.stockCodes()[index].OrderType("M");
            computestockTotal('S', stockSellViewModel);
            console.log("onMarketValueCheck");
            return true;
        }
    };
    stockSellViewModel.showSellValidation = ko.computed(function () {
        if (stockSellViewModel.Amount() > 0) {
            return false
        } else {
            return true;
        }
    });
}
function setupStockSell() {
    ko.applyBindings(stockSellViewModel, $("#mysellstockcontent-box .panel-body").get(0));
    ko.applyBindings(stockSellViewModel, $("#mysellstockcontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#mysellstockcontent-box .panel-heading").get(0));
    ko.applyBindings(stockSellViewModel, $("#sellstock").get(0));

    $('#sellstock').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveSellStockCart(btn);
    });
    $('#stockrefresh').click(function () {
        refreshSell();
    });
    $('#sellmorestock').click(function () {
        initializeStockSell();
        setupStockSell();
        getUserStockSell(0);
    });
    $("#stocksellshowmore").on("click", function () {
        getUserStockSell(stockSellViewModel.lastDateTime());
    });

    $("#stocksellpanelbody").perfectScrollbar(
    {
        suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
    });
}
function showSell() {
    $("#stocksellcontainer").html(sellStockView);
    setupStockSell();
    getUserStockSell(0);
}
function getUserStockSell(lastDateTime) {
    if (stockSellViewModel.contentLoadTriggered == true) {
        return;
    }
    stockSellViewModel.contentLoadTriggered = true;
    $("#stocksellshowmore").prop("disabled", true);
    $("#stockrefresh").prop("disabled", true);
    $.ajax({
        url: "/api/StockService/GetStockByUser",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: { lastDateTime: lastDateTime },
        beforeSend: showLoadingImage($("#mysellstockcontent-box .panel-heading")),
        success: function (result) {
            if (result.length > 0) {
                stockSellViewModel.lastDateTime(result[result.length - 1].PurchasedAt);
                for (i = 0; i < result.length; i++) {
                    result[i].Quantity = ko.observable(0);
                    result[i].OfferPrice = ko.observable(0);
                    result[i].OrderType = ko.observable("M");
                    result[i].PurchasedAt = ParseDate(result[i].PurchasedAt);
                    stockProfitLoss(result[i]);
                    stockProfitLossCss(result[i]);
                    stockProfitLossPercent(result[i]);
                    stockDayChangeCss(result[i]);
                    stockSellViewModel.stockCodes.push(result[i]);
                }
            }

            $("#stocksellshowmore").prop("disabled", false);
            $("#stockrefresh").prop("disabled", false);

        }
    }).always(function () {
        stockSellViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#mysellstockcontent-box .panel-heading"));
    });
}
function refreshSell() {
    stockSellViewModel.stockCodes.removeAll();
    stockSellViewModel.lastDateTime(0);
    stockSellViewModel.Amount(0);
    stockSellViewModel.AmountAvailable(0);
    getUserStockSell(0);
}
function saveSellStockCart(btn) {
    var sellingItems = ko.utils.arrayFilter(stockSellViewModel.stockCodes(),
          function (item) {
              return item.Quantity() > 0;
          });
    $.ajax({
        url: "/api/StockService/savesellstockcart",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(sellingItems),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            stockSellViewModelresult = ko.mapping.fromJS(result);
            if (stockSellViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mysellstockcontent-box").html("");
                $("#mysellstockcontent-box").addClass("hidden");
                $("#mysellstockcontent-submit").removeClass("hidden");
                $("#sellmorestock").removeClass("hidden");
                $("#mysellstockcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(stockSellViewModelresult, $("#mysellstockcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#sellstockcontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}

function initializeStockOrder() {
    $("#stockordercontainer").html(orderStockView);
    stockOrderViewModel = {
        stockCodes: ko.observableArray(),
        lastDateTime: ko.observable(0),
        orderingItems: ko.observableArray(),
        contentLoadTriggered: false
    };
    stockOrderViewModel.showOrderValidation = ko.computed(function () {
        if (stockOrderViewModel.orderingItems().length > 0) {
            return false
        } else {
            return true;
        }
    });

}
function setupStockOrder() {
    ko.applyBindings(stockOrderViewModel, $("#stockOrdercontent-box .panel-body").get(0));
    ko.applyBindings(stockOrderViewModel, $("#stockOrdercontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#stockOrdercontent-box .panel-heading").get(0));
    ko.applyBindings(stockOrderViewModel, $("#orderstock").get(0));

    $('#orderstock').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveOrderStockCart(btn);
    });
    $('#stockorderrefresh').click(function () {
        refreshOrder();
    });
    $('#ordermorestock').click(function () {
        showOrder();
    });
    $("#stockordershowmore").on("click", function () {
        getStockOrder(stockOrderViewModel.lastDateTime());
    });
    $("#stockOrderItemcontent-box").perfectScrollbar(
 {
     suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
 });
}
function showOrder() {
    $("#stockordercontainer").html(orderStockView);
    setupStockOrder();
    getStockOrder(0);
}
function getStockOrder(lastDateTime) {
    if (stockOrderViewModel.contentLoadTriggered == true) {
        return;
    }
    $("#stockordershowmore").prop("disabled", true);
    $("#stockrefresh").prop("disabled", true);
    stockOrderViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/StockService/GetStockTradeByUser",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: { lastDateTime: lastDateTime },
        beforeSend: showLoadingImage($("#stockOrdercontent-box .panel-heading")),
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    result[i].PurchasedAt = ParseDate(result[i].PurchasedAt);
                    stockDayChangeCss(result[i]);
                    stockOrderViewModel.stockCodes.push(result[i]);
                    stockOrderViewModel.lastDateTime(result[i].UpdatedAt);
                }
            }
            $("#stockordershowmore").prop("disabled", false);
            $("#stockrefresh").prop("disabled", false);

        }
    }).always(function () {
        stockOrderViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#stockOrdercontent-box .panel-heading"));
    });

}
function refreshOrder() {
    stockOrderViewModel.stockCodes.removeAll();
    stockOrderViewModel.lastDateTime(0);
    stockOrderViewModel.orderingItems.removeAll();
    getStockOrder(0);
}
function saveOrderStockCart(btn) {
    $.ajax({
        url: "/api/StockService/saveorderstockcart",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(stockOrderViewModel.orderingItems()),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            stockOrderViewModelresult = ko.mapping.fromJS(result);
            if (stockOrderViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#stockOrdercontent-box").html("");
                $("#stockOrdercontent-box").addClass("hidden");
                $("#stockOrdercontent-submit").removeClass("hidden");
                $("#ordermorestock").removeClass("hidden");
                $("#stockOrdercontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(stockOrderViewModelresult, $("#stockOrdercontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#stockordercontainer").offset().top }, "slow");
                console.log("Ordering Item Remvoed");
                stockOrderViewModel.lastDateTime(0);
                stockOrderViewModel.stockCodes.removeAll();
                stockOrderViewModel.orderingItems.removeAll();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}

function initializeStockSummary() {
    $("#stockSummarycontainer").html(summaryStockView);
    stockSummaryViewModel = {
        contentLoadTriggered: false,
        stockCodes: ko.observableArray(),
        stockTopN: ko.observableArray()
    }
}
function setupStockSummary() {
    ko.applyBindings(peopleInfo, $("#mystocksummarycontent-box .panel-heading").get(0));
    ko.applyBindings(stockSummaryViewModel,
       $("#stockSummarycontent-box").get(0));
    ko.applyBindings(stockSummaryViewModel,
                      $("#stockTopNSummary").get(0));
    $("#stockSummarycontent-box").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
        });
}
function getStockSummary() {
    return $.ajax({
        url: "/api/stockservice/getstocksummary",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getTopNStockOwner() {
    return $.ajax({
        url: "/api/stockservice/gettoptenstockowner",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}

function slideStockInput(self, index, currentValue, qtyRange, tradetype) {
    self.simpleSlider();

    if (currentValue == 0) {
        qtyStockBind(self, qtyRange, tradetype);
    } else {
        offerPriceBind(self, currentValue * 3, tradetype);
    }

}
function qtyStockBind(self, qtyRange, tradetype) {
    self.bind("slider:changed", function (event, data) {
        var item = (event.currentTarget);
        var index = $(item).data("index");
        var qty = parseInt("0" + data.value * qtyRange, 10);
        if (tradetype == 'B') {
            stockBuyViewModel.stockCodes()[index].Quantity(qty);
            computestockTotal(tradetype, stockBuyViewModel);
        }
        else if (tradetype == 'S') {
            stockSellViewModel.stockCodes()[index].Quantity(qty);
            computestockTotal(tradetype, stockSellViewModel);
        }
    });
}
function offerPriceBind(self, maxRange, tradetype) {
    self.bind("slider:changed", function (event, data) {
        var item = (event.currentTarget);
        var index = $(item).data("index");
        var offerprice = (data.value * maxRange).toFixed(2);
        if (tradetype == 'B') {
            stockBuyViewModel.stockCodes()[index].OfferPrice(offerprice);
            computestockTotal(tradetype, stockBuyViewModel);

        }
        else if (tradetype == 'S') {
            stockSellViewModel.stockCodes()[index].OfferPrice(offerprice);
            computestockTotal(tradetype, stockSellViewModel);

        }
    });
}
function computestockTotal(tradetype, vm) {
    var total = 0;
    var cost = 0;
    var quantity = 0;
    for (var p = 0; p < vm.stockCodes().length; ++p) {
        if (vm.stockCodes()[p].OrderType() == "M") {
            cost = vm.stockCodes()[p].CurrentValue;
        } else {
            cost = vm.stockCodes()[p].OfferPrice();
        }

        quantity = vm.stockCodes()[p].Quantity();
        total += cost * quantity;
    }
    var currentTotal = vm.Amount();
    var available = vm.AmountAvailable();
    if (tradetype == 'B') {
        vm.AmountAvailable(available - (total - currentTotal));
    }
    else if (tradetype == 'S') {
        vm.amountBeforeTax(total);
        vm.tax(total * vm.taxRate() / 100);
        total -= vm.tax();
        vm.AmountAvailable(total + (available - currentTotal));
    }
    vm.Amount(total);


}
function stockProfitLoss(item) {
    item.ProfitLossPlusMinus = ko.computed(function () {
        var profit = (item.CurrentValue - item.PurchasedPrice) * item.PurchasedUnit;
        if (profit < 0)
            result = "-" + profit.toFixed(2);
        else {
            result = "+" + profit.toFixed(2);
        }
        return result;
    });
}
function stockProfitLossPercent(item) {
    item.ProfitLossPlusMinusPercent = ko.computed(function () {
        var profit = ((item.CurrentValue - item.PurchasedPrice) / item.PurchasedPrice) * 100;
        if (profit < 0)
            result = "-" + profit.toFixed(2) + "%";
        else {
            result = "+" + profit.toFixed(2) + "%";
        }
        return result;
    });
}
function stockProfitLossCss(item) {
    item.ProfitLossCss = ko.computed(function () {
        var profit = item.CurrentValue - item.PurchasedPrice;
        if (profit < 0)
            result = "text-danger";
        else {
            result = "text-success";
        }
        return result;
    });
}
function stockDayChangeCss(item) {
    item.DayChangeArrowCss = ko.computed(function () {
        if (item.DayChange < 0)
            result = "fa fa-arrow-circle-o-down text-danger";
        else {
            result = "fa fa-arrow-circle-o-up text-success";
        }
        return result;
    });
    item.DayChangeCss = ko.computed(function () {
        if (item.DayChange < 0)
            result = "text-danger";
        else {
            result = "text-success";
        }
        return result;
    });
    item.DayChangePlusMinus = ko.computed(function () {
        if (item.DayChange < 0)
            result = item.DayChange.toFixed(2);
        else {
            result = "+" + item.DayChange.toFixed(2);
        }
        return result;
    });
    item.DayChangePlusMinusPercent = ko.computed(function () {
        if (item.DayChangePercent < 0)
            result = item.DayChangePercent.toFixed(2) + "%";
        else {
            result = "+" + item.DayChangePercent.toFixed(2) + "%";
        }
        return result;
    });
}
function stockSummaryChangeCss(item) {
    item.SummaryChange = item.TotalCurrentValue - item.TotalPurchaseValue;
    item.SummaryChangePercent = (item.TotalCurrentValue - item.TotalPurchaseValue) / item.TotalCurrentValue * 100;
    item.SummaryChangeArrowCss = ko.computed(function () {
        if (item.SummaryChange < 0)
            result = "fa fa-arrow-circle-o-down text-danger";
        else {
            result = "fa fa-arrow-circle-o-up text-success";
        }
        return result;
    });
    item.SummaryChangeCss = ko.computed(function () {
        if (item.SummaryChange < 0)
            result = "text-danger";
        else {
            result = "text-success";
        }
        return result;
    });
    item.SummaryChangePlusMinus = ko.computed(function () {
        if (item.SummaryChange < 0)
            result = numeral(item.SummaryChange).format('0,0.00');
        else {
            result = "+" + numeral(item.SummaryChange).format('0,0.00');
        }
        return result;
    });
    item.SummaryChangePlusMinusPercent = ko.computed(function () {
        if (item.SummaryChangePercent < 0)
            result = item.SummaryChangePercent.toFixed(2) + "%";
        else {
            result = "+" + item.SummaryChangePercent.toFixed(2) + "%";
        }
        return result;
    });
}


function initializeStockForecastView() {
    stockForecastViewModel = {
        forecast: ko.observableArray(),
        contentLoadTriggered: false,
    };
    ko.applyBindings(stockForecastViewModel,
                 $("#mystockForecastcontent-box .panel-body").get(0));
}
function getStockForecast() {
    $.ajax({
        url: "/api/StockService/GetStockForeCast",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.length > 0) {
                stockForecastViewModel.forecast.removeAll();
                for (var i = 0; i < result.length; i++) {
                    stockForecastViewModel.forecast.push(result[i]);
                }
              
            }
        }
    });
}
