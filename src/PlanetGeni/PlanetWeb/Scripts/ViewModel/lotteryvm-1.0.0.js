var lotteryPickViewModel;
var myPick3ViewModel;
var lotteryPick3View;

function initializeLottery() {
    lotteryPickViewModel = {
        Amount: ko.observable(),
        AmountAvailable: ko.observable(userBankAccountViewModel.Cash()),
        NextDrawing: ko.observableArray(),
        pick3Orders: ko.observableArray(),
        myPick3: ko.observableArray(),
        myPick3lastDrawingId: ko.observable(0),
        pick5Orders: ko.observableArray(),
        myPick5: ko.observableArray(),
        myPick5lastDrawingId: ko.observable(0),
        contentLoadTriggered: false
    }
    lotteryPickViewModel.ShowValidation = ko.computed(function () {
        if (lotteryPickViewModel.AmountAvailable() < 0) {
            return true;
        }
        else {
            return false;
        }
    });
    initializelotteryPick3Navigation();
    setupPick3();
    setupPick5();
}
function initializelotteryPick3Navigation() {
    $('#lottery-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#lottery-pick5") {
            var self = $("#lotteryPick5content-wrapper input.dial");
            addlotteryKnob(self);
        }
        else if (self.attr('href') == "#lottery-pick3") {
            var self = $("#lotteryPick3content-wrapper input.dial");
            addlotteryKnob(self);
        } else if (self.attr('href') == "#lottery-pick3wins") {
            if (lotteryPickViewModel.myPick3().length == 0) {
                setupMyPick3();
            }
        }
        else if (self.attr('href') == "#lottery-pick5wins") {
            if (lotteryPickViewModel.myPick5().length == 0) {
                setupMyPick5();
            }
        }
        else if (self.attr('href') == "#lottery-howtoplay") {
        }
    })
}


function setupPick3() {
    var self = $("#lotteryPick3content-wrapper input.dial");
    addlotteryKnob(self);
    $('#randomPick3').click(function () {
        var btn = $(this);
        btn.button('loading');
        var self = $("#lotteryPick3content-wrapper input.dial");
        getRandomPick(self);
        btn.button('reset')
    });
    applyKoLotteryPick3pick3();
    getLotteryDrawingDate();
    $('#savelotteryPick3').click(function () {
        var btn = $(this);
        orderPick3(btn);
    });
}
function applyKoLotteryPick3pick3() {
    ko.applyBindings(lotteryPickViewModel, $("#mylotteryPick3content-box .panel-footer").get(0));
    ko.applyBindings(lotteryPickViewModel, $("#pick3Orders").get(0));
    ko.applyBindings(peopleInfo, $("#mylotteryPick3content-box .panel-heading").get(0));
}
function orderPick3(btn) {
    if (lotteryPickViewModel.contentLoadTriggered == true) {
        return;
    }
    lotteryPickViewModel.contentLoadTriggered = true;
    var self = $("#lotteryPick3content-wrapper input.dial");

    var pickOrder = {
        Number1: self.eq(0).val(),
        Number2: self.eq(1).val(),
        Number3: self.eq(2).val(),
    }

    $.ajax({
        url: "/api/LotteryService/SavePick3Lottery",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(pickOrder),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                lotteryPickViewModel.pick3Orders.push(pickOrder);
                var pick3Item = ko.utils.arrayFilter(lotteryPickViewModel.NextDrawing(), function (item) {
                    return item.LotteryType() == 'T';
                })
                userBankAccountViewModel.Cash(userBankAccountViewModel.Cash() - pick3Item[0].LotteryPrice());
                lotteryPickViewModel.AmountAvailable(userBankAccountViewModel.Cash());
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        lotteryPickViewModel.contentLoadTriggered = false;
        btn.button('reset')
    });
}

function setupPick5() {
    $('#randomPick5').click(function () {
        var btn = $(this);
        btn.button('loading');
        var self = $("#lotteryPick5content-wrapper input.dial");
        getRandomPick(self);
        btn.button('reset')
    });
    applyKoLotteryPick5pick5();
    $('#savelotteryPick5').click(function () {
        var btn = $(this);
        orderPick5(btn);
    });
}
function applyKoLotteryPick5pick5() {
    ko.applyBindings(lotteryPickViewModel, $("#mylotteryPick5content-box .panel-footer").get(0));
    ko.applyBindings(lotteryPickViewModel, $("#pick5Orders").get(0));
    ko.applyBindings(peopleInfo, $("#mylotteryPick5content-box .panel-heading").get(0));
}
function orderPick5(btn) {
    if (lotteryPickViewModel.contentLoadTriggered == true) {
        return;
    }
    lotteryPickViewModel.contentLoadTriggered = true;
    var self = $("#lotteryPick5content-wrapper input.dial");

    var pickOrder = {
        Number1: self.eq(0).val(),
        Number2: self.eq(1).val(),
        Number3: self.eq(2).val(),
        Number4: self.eq(2).val(),
        Number5: self.eq(2).val(),
    }

    $.ajax({
        url: "/api/LotteryService/SavePick5Lottery",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(pickOrder),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.StatusCode == 200) {
                userNotificationPollOnce();
                lotteryPickViewModel.pick5Orders.push(pickOrder);
                var pick5Item = ko.utils.arrayFilter(lotteryPickViewModel.NextDrawing(), function (item) {
                    return item.LotteryType() == 'F';
                })
                userBankAccountViewModel.Cash(userBankAccountViewModel.Cash() - pick5Item[0].LotteryPrice());
                lotteryPickViewModel.AmountAvailable(userBankAccountViewModel.Cash());
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        lotteryPickViewModel.contentLoadTriggered = false;
        btn.button('reset')
    });
}

function setupMyPick3() {
    getMy3Picks();
    applyKoMyPick3();
    $("#lotteryMyPick3WinItemcontent-box").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10, alwaysVisibleY: true
        });
    $('#mypick3winshowmore').click(function () {
        var btn = $(this);
        getMy3Picks(btn);
    });
}
function applyKoMyPick3() {
    ko.applyBindings(lotteryPickViewModel, $("#lotteryMyPick3Wincontent-wrapper").get(0));
    ko.applyBindings(peopleInfo, $("#mylotteryMyPick3Wincontent-box .panel-heading").get(0));
    ko.applyBindings(lotteryPickViewModel, $("#mylotteryMyPick3Wincontent-box .panel-footer").get(0));
}
function getMy3Picks() {
    if (lotteryPickViewModel.contentLoadTriggered == true) {
        return;
    }
    showLoadingImage($("#mylotteryMyPick3Wincontent-box .panel-heading"))
    $.when(getMyThreePicks(), GetPickThreeWinNumber())
    .done(function (a1, a2) {
        var winningNumbers = a2[0];
        var mypick = a1[0];
        var filter;
        if (winningNumbers.length > 0) {
            for (var i = 0; i < winningNumbers.length ; i++) {
                filter = $.grep(mypick, function (el, index) {
                    return (el.DrawingId == winningNumbers[i].DrawingId);
                });
                winningNumbers[i].MyPick3s = ko.observableArray([]);
                if (filter.length > 0) {
                    for (var j = 0; j < filter.length; j++) {
                        filter[j].Number1Css = "";
                        filter[j].Number2Css = "";
                        filter[j].Number3Css = "";
                        filter[j].Number4Css = "";
                        filter[j].Number5Css = "";
                        checkFormatchPick3(filter[j], winningNumbers[i]);
                        winningNumbers[i].MyPick3s.push(filter[j]);
                    }
                }
                lotteryPickViewModel.myPick3.push(winningNumbers[i]);
            }
            lotteryPickViewModel.myPick3lastDrawingId(winningNumbers[winningNumbers.length - 1].DrawingId);
        }
        lotteryPickViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#mylotteryMyPick3Wincontent-box .panel-heading"))
    });

}
function checkFormatchPick3(filterItem, winNumber) {
    if (filterItem.Number1 == winNumber.Number1 ||
             filterItem.Number1 == winNumber.Number2 ||
             filterItem.Number1 == winNumber.Number3
             ) {
        filterItem.Number1Css = "fa-1_5x  producttitle text-warning";
    }

    if (filterItem.Number2 == winNumber.Number1 ||
             filterItem.Number2 == winNumber.Number2 ||
             filterItem.Number2 == winNumber.Number3) {
        filterItem.Number2Css = "fa-1_5x  producttitle text-warning";
    }

    if (
             filterItem.Number3 == winNumber.Number1 ||
             filterItem.Number3 == winNumber.Number2 ||
             filterItem.Number3 == winNumber.Number3
        ) {
        filterItem.Number3Css = "fa-1_5x  producttitle text-warning";
    }

}
function getMyThreePicks() {
    lotteryPickViewModel.contentLoadTriggered = true;
    return $.ajax({
        url: "/api/LotteryService/GetMyThreePicks",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: {
            lastDrawingId: lotteryPickViewModel.myPick3lastDrawingId()
        }
    });
}
function GetPickThreeWinNumber() {
    lotteryPickViewModel.contentLoadTriggered = true;
    return $.ajax({
        url: "/api/LotteryService/getpickthreewinnumber",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: {
            lastDrawingId: lotteryPickViewModel.myPick3lastDrawingId()
        }
    });
}

function setupMyPick5() {
    getMy5Picks();
    applyKoMyPick5();
    $("#lotteryMyPick5WinItemcontent-box").perfectScrollbar(
        {
            suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10, alwaysVisibleY: true
        });
    $('#mypick5winshowmore').click(function () {
        var btn = $(this);
        getMy5Picks(btn);
    });

}
function applyKoMyPick5() {
    ko.applyBindings(lotteryPickViewModel, $("#lotteryMyPick5Wincontent-wrapper").get(0));
    ko.applyBindings(peopleInfo, $("#mylotteryMyPick5Wincontent-box .panel-heading").get(0));
    ko.applyBindings(lotteryPickViewModel, $("#mylotteryMyPick5Wincontent-box .panel-footer").get(0));
}
function getMy5Picks() {
    if (lotteryPickViewModel.contentLoadTriggered == true) {
        return;
    }
    showLoadingImage($("#mylotteryMyPick5Wincontent-box .panel-heading"))
    $.when(getMyFivePicks(), GetPickFiveWinNumber())
    .done(function (a1, a2) {
        var winningNumbers = a2[0];
        var mypick = a1[0];
        var filter;
        if (winningNumbers.length > 0) {

            for (var i = 0; i < winningNumbers.length ; i++) {
                filter = $.grep(mypick, function (el, index) {
                    return (el.DrawingId == winningNumbers[i].DrawingId);
                });
                winningNumbers[i].MyPick5s = ko.observableArray([]);
                if (filter.length > 0) {
                    for (var j = 0; j < filter.length; j++) {
                        filter[j].Number1Css = "";
                        filter[j].Number2Css = "";
                        filter[j].Number3Css = "";
                        filter[j].Number4Css = "";
                        filter[j].Number5Css = "";
                        checkFormatchPick5(filter[j], winningNumbers[i]);
                        winningNumbers[i].MyPick5s.push(filter[j]);
                    }
                }
                lotteryPickViewModel.myPick5.push(winningNumbers[i]);
            }
            lotteryPickViewModel.myPick5lastDrawingId(winningNumbers[winningNumbers.length - 1].DrawingId);
        }
        lotteryPickViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#mylotteryMyPick5Wincontent-box .panel-heading"))
    });

}
function getMyFivePicks() {
    lotteryPickViewModel.contentLoadTriggered = true;
    return $.ajax({
        url: "/api/LotteryService/GetMyFivePicks",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: {
            lastDrawingId: lotteryPickViewModel.myPick5lastDrawingId()
        }
    });
}
function GetPickFiveWinNumber() {
    lotteryPickViewModel.contentLoadTriggered = true;
    return $.ajax({
        url: "/api/LotteryService/getpickfivewinnumber",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: {
            lastDrawingId: lotteryPickViewModel.myPick5lastDrawingId()
        }
    });
}
function checkFormatchPick5(filterItem, winNumber) {
    if (filterItem.Number1 == winNumber.Number1 ||
             filterItem.Number1 == winNumber.Number2 ||
             filterItem.Number1 == winNumber.Number3 ||
             filterItem.Number1 == winNumber.Number4 ||
             filterItem.Number1 == winNumber.Number5
             ) {
        filterItem.Number1Css = "fa-1_5x  producttitle text-warning";
    }

    if (filterItem.Number2 == winNumber.Number1 ||
            filterItem.Number2 == winNumber.Number2 ||
            filterItem.Number2 == winNumber.Number3 ||
            filterItem.Number2 == winNumber.Number4 ||
            filterItem.Number2 == winNumber.Number5
            ) {
        filterItem.Number2Css = "fa-1_5x  producttitle text-warning";
    }
    if (filterItem.Number3 == winNumber.Number1 ||
        filterItem.Number3 == winNumber.Number2 ||
        filterItem.Number3 == winNumber.Number3 ||
        filterItem.Number3 == winNumber.Number4 ||
        filterItem.Number3 == winNumber.Number5
        ) {
        filterItem.Number3Css = "fa-1_5x  producttitle text-warning";
    }

    if (filterItem.Number4 == winNumber.Number1 ||
        filterItem.Number4 == winNumber.Number2 ||
        filterItem.Number4 == winNumber.Number3 ||
        filterItem.Number4 == winNumber.Number4 ||
        filterItem.Number4 == winNumber.Number5
        ) {
        filterItem.Number4Css = "fa-1_5x  producttitle text-warning";
    }
    if (filterItem.Number5 == winNumber.Number1 ||
        filterItem.Number5 == winNumber.Number2 ||
        filterItem.Number5 == winNumber.Number3 ||
        filterItem.Number5 == winNumber.Number4 ||
        filterItem.Number5 == winNumber.Number5
        ) {
        filterItem.Number5Css = "fa-1_5x  producttitle text-warning";
    }


}


function getRandomPick(self) {
    var num = 1;
    for (var i = 0; i < self.length; i++) {
        num = getRandomNumber(1, 10);
        self.eq(i)
       .val(num)
       .trigger('change');
    }
}
function getLotteryDrawingDate() {
    $.ajax({
        url: "/api/LotteryService/GetNextLotteryDrawingDate",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.length > 0) {
                ko.mapping.fromJS(result, {}, lotteryPickViewModel.NextDrawing);
            }
        }
    });
}
function addlotteryKnob(self) {
    var width = "50%";
    if (window.matchMedia('(max-width: 320px)').matches) {
        width = "85%";
    }
    else if (window.matchMedia('(max-width: 480px)').matches) {
        width = "85%";
    }
    else if (window.matchMedia('(max-width: 720px)').matches) {
        width = "99%";
    }
    else {
        width = "99%";
    }
    console.log("Kboifiny");
    self.knob(
   {
       release: function (value) {
           this.$.attr('value', value);
       },
       'min': 1,
       'max': 10,
       'width': width,
       'height': width,
       'thickness': .3,
       'fgColor': "#eea236"
   });

}