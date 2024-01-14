var gitfCartViewModel;
var gitfCartViewModelresult;
var bankAccountViewModel;
var merchandiseGiftViewModel;
var merchandiseGiftViewModelresult;
var giftmerchandiseView;
function giftInitialize() {
    initializegiftNavigation();
    bankAccountViewModel = null;
    gitfCartViewModel = {
        ToId: ko.observableArray(),
        NationId: ko.observableArray(),
        Cash: ko.observable(0),
        Gold: ko.observable(0),
        Silver: ko.observable(0),
        TotalCash: ko.observable(0),
        TotalCashLeft: ko.observable(0),
        TotalGiftInCash: ko.observable(0),
        TotalSilver: ko.observable(0),
        TotalGold: ko.observable(0),
        TotalGoldCss: ko.observable('text-success'),
        TotalSilverCss: ko.observable('text-success'),
        TotalGiftInCashCss: ko.observable('text-success'),
        TotalTax: ko.observable(0),
        contentLoadTriggered: false
    };
    initializemerchandiseGift();
    giftmerchandiseView = $("#merchandisegiftcontainer").html();
    getGetGiftDetails();
}
function initializemerchandiseGift() {
    merchandiseGiftViewModel = {
        merchandiseCodes: ko.observableArray(),
        lastMerchandiseTypeId: ko.observable(0),
        ToId: ko.observableArray(),
        sendingItemsIndex: ko.observableArray(),
        TotalCashLeft: ko.observable(0),
        TotalTax: ko.observable(0),
        showView: ko.observable(false),
        TotalGiftLeftCashCss: ko.observable('text-success'),
        MerchandiseTypeId: ko.observableArray(),
        clickSendGift: function (data, event) {
            updatemerchandiseGiftViewModel();
            return true;
        }
    };
    setupSendMerchandiseGift();
}
function getGetGiftDetails() {
    if (gitfCartViewModel.contentLoadTriggered == true) {
        return;
    }
    gitfCartViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/giftservice/getgiftdetails",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#mysendGiftcontent-box .panel-body")),
        success: function (data) {
            bankAccountViewModel = ko.mapping.fromJS(data);
            setupCapitalView();
            merchandiseGiftViewModel.TotalCashLeft(bankAccountViewModel.Cash());
            gitfCartViewModel.TotalCashLeft(bankAccountViewModel.Cash());

            if (bankAccountViewModel.Cash() > 0
                || bankAccountViewModel.Gold() > 0
                || bankAccountViewModel.Silver() > 0) {
                $("#mysendGiftcontent-box .panel-body .well").removeClass("hidden");
            }

            setupgiftviewmodelCss();
        }
    }).always(function () {
        gitfCartViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#mysendGiftcontent-box .panel-body"));


    });
}
function setupCapitalView() {
    spinInputwtPrefix($("#giftGold"), "Gold", bankAccountViewModel.Gold());
    spinInputwtPrefix($("#giftSilver"), "Silver", bankAccountViewModel.Silver());
    spinInputwtPrefix($("#giftCash"), "Cash", bankAccountViewModel.Cash());
    applyKoSendGift();
    updateCheckBox($("#gift-capital"));
    defineCapitalGiftValidationRules();
    $('#sendGift').click(function () {
        var btn = $(this)
        sendCapitalGift(btn);
    });
    setupeventforViewModelUpdates();
    updategitfCartViewModel();
    setupPopOverGiftRate();
}
function applyKoSendGift() {

    ko.applyBindings(countryCodeViewModel, $("#giftcountryList")[0]);
    applyScroll();
    ko.applyBindings(peopleInfo, $("#capitalGiftFlagHeader").get(0));
    ko.applyBindings(friendViewModel, $("#giftfriendList")[0]);
    ko.applyBindings(gitfCartViewModel, $("#capitalgiftform")[0]);
    ko.applyBindings(bankAccountViewModel.GiftRate, $("#giftRate")[0]);
    ko.applyBindings(gitfCartViewModel, $("#countryListFooter")[0]);
    ko.applyBindings(gitfCartViewModel, $("#friendListFooter")[0]);
    ko.applyBindings(gitfCartViewModel, $("#sendGift")[0]);
}
function setupPopOverGiftRate() {
    $("#giftRate").find('[data-toggle="popover"]').popover({
        trigger: 'hover click foucs',
        placement: 'top',
        container: 'body'
    });
}
function applyKoSendMerchandiseGift() {
    ko.applyBindings(merchandiseGiftViewModel, $("#merchandiseGiftItemcontent-box").get(0));
    ko.applyBindings(friendViewModel, $("#giftfriendMerchandiseList")[0]);
    ko.applyBindings(peopleInfo, $("#merchandiseGiftcontent-box .panel-heading").get(0));
    ko.applyBindings(merchandiseGiftViewModel, $("#friendMerchandiseListFooter")[0]);
    ko.applyBindings(merchandiseGiftViewModel, $("#merchandiseGiftfooter").get(0));
    ko.applyBindings(merchandiseGiftViewModel, $("#merchandiseGiftTotalfooter").get(0));
    ko.applyBindings(merchandiseGiftViewModel, $("#sendmerchandise").get(0));
}
function applyScrollMerchandise() {
    $("#giftfriendMerchandiseList, #merchandiseGiftItemcontent-box").perfectScrollbar(
     {
         suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
     });
}
function applyScroll() {
    $("#giftfriendList, #giftcountryList").perfectScrollbar(
     {
         suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
     });
}
function updateCheckBox(parentId) {
    parentId.find('.button-checkbox').each(function () {
        // Settings
        var $widget = $(this),
            $button = $widget.find('button'),
            $checkbox = $widget.find('input:checkbox');
        $checkbox.on('change', function () {
            updateDisplay($checkbox, $button);
            if ($checkbox.attr('id') == 'giftfriendall') {
                updateSelection(gitfCartViewModel.ToId, $checkbox);
                updategitfCartViewModel();
            }
            else if ($checkbox.attr('id') == 'giftcountryall') {
                updateSelection(gitfCartViewModel.NationId, $checkbox);
                updategitfCartViewModel();
            } else if ($checkbox.attr('id') == 'giftfriendmerchandiseall') {
                updateSelection(merchandiseGiftViewModel.ToId, $checkbox);
                updatemerchandiseGiftViewModel();
            }
        });

        initilaizeUpdatedCheckbox($checkbox, $button);
    });

}
function defineCapitalGiftValidationRules() {
    $('#capitalgiftform').validate({
        ignore: "",
        rules: {
            totalgiftgold: {
                //required: '#checkboxGold:checked',
                min: 0,
                number: true
            },
            totalgiftsilver: {
                required: '#checkboxSilver:checked',
                min: 0,
                number: true
            },
            totalgiftCash: {
                required: '#checkboxCash:checked',
                min: 1,
                number: true
            },
        },
        messages: {
            totalgiftgold: {
                min: jQuery.format("Not enough Gold to gift")
            },
            totalgiftsilver: {
                min: jQuery.format("Not Enough Silver to gift ")
            }, totalgiftCash: {
                min: jQuery.format("Not Enough Cash to gift ")

            },
        },
        success: function (label) {
            // set &nbsp; as text for IE
            label.html("&nbsp;").addClass("checked");
        },
        errorPlacement: function (error, element) {
            console.log(element);
            if (element.parent('.bootstrap-touchspin').length) {
                error.insertAfter(element.parent('.bootstrap-touchspin'));
            } else if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            }
            else {
                error.insertAfter(element);
            }
        }
    });

}
function sendCapitalGift(btn) {
    if ($('#capitalgiftform').valid()) {
        collectData();
        btn.button('loading')
        $.ajax({
            url: "/api/giftservice/savesendgift",
            type: "post",
            contentType: "application/json",
            data: ko.toJSON(gitfCartViewModel),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                gitfCartViewModelresult = ko.mapping.fromJS(result);
                if (gitfCartViewModelresult.StatusCode() == 200) {
                    userNotificationPollOnce();
                    btn.addClass("hidden");
                    $("#mysendGiftcontent-box").html("");
                    $("#mysendGiftcontent-box").addClass("hidden");
                    $("#mysendGiftcontent-submit").removeClass("hidden");
                    $("#mysendGiftcontent-submit").next(".backbtn").removeClass("hidden");
                    console.log("Test7");
                    ko.applyBindings(gitfCartViewModelresult, $("#mysendGiftcontent-submit").get(0));
                    console.log("Test8");
                    $("#main").animate({ scrollTop: $("#sendGiftcontainer").offset().top }, "slow");
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
}
function collectData() {
    gitfCartViewModel.Cash($("#giftCash").val());
    gitfCartViewModel.Gold($("#giftGold").val());
    gitfCartViewModel.Silver($("#giftSilver").val());
}
function updategitfCartViewModel() {
    gitfCartViewModel.Cash(0);
    gitfCartViewModel.Gold(0);
    gitfCartViewModel.Silver(0);
    if ($("#checkboxCash").is(':checked')) {
        gitfCartViewModel.Cash($("#giftCash").val());
    }
    if ($("#checkboxGold").is(':checked')) {
        gitfCartViewModel.Gold($("#giftGold").val());
    }
    if ($("#checkboxSilver").is(':checked')) {
        gitfCartViewModel.Silver($("#giftSilver").val());
    }
    var totalrecipent = (gitfCartViewModel.ToId().length + gitfCartViewModel.NationId().length);
    var toatlgiftGold = gitfCartViewModel.Gold() * totalrecipent;
    var toatlgiftSilver = gitfCartViewModel.Silver() * totalrecipent;
    var totalCash = (parseFloat(gitfCartViewModel.Gold()) * bankAccountViewModel.GiftRate.CurrentGoldValue()
            + parseFloat(gitfCartViewModel.Silver()) * bankAccountViewModel.GiftRate.CurrentSilverValue()
            + parseFloat(gitfCartViewModel.Cash()))
            * totalrecipent;
    var totalgiftCash = totalCash * (1 + bankAccountViewModel.GiftRate.TaxRate() / 100);
    var totaltax = totalCash * (bankAccountViewModel.GiftRate.TaxRate() / 100);
    var totalCash = totaltax + gitfCartViewModel.Cash();

    gitfCartViewModel.TotalCashLeft(bankAccountViewModel.Cash() - totalgiftCash);
    gitfCartViewModel.TotalGiftInCash(totalgiftCash);
    gitfCartViewModel.TotalCash(totalCash);
    gitfCartViewModel.TotalSilver(bankAccountViewModel.Silver() - toatlgiftSilver);
    gitfCartViewModel.TotalGold(bankAccountViewModel.Gold() - toatlgiftGold);
    gitfCartViewModel.TotalTax(totaltax);

    setupgiftviewmodelCss();
}
function setupeventforViewModelUpdates() {
    $('#checkboxGold, #checkboxSilver, #checkboxCash').on('click', function () {
        updategitfCartViewModel();
    });
    $('#giftGold, #giftSilver, #giftCash').on('change', function () {
        updategitfCartViewModel();
    });
}
function initializegiftNavigation() {
    $('#gift-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#gift-treasure") {
            if (merchandiseGiftViewModel.merchandiseCodes().length == 0) {
                getMerchandiseGift(0);
            }
        }
    })
}
function setupgiftviewmodelCss() {
    if (gitfCartViewModel.TotalGiftInCash() >= 0) {
        gitfCartViewModel.TotalGiftInCashCss('text-success');
    } else {
        gitfCartViewModel.TotalGiftInCashCss('text-danger');

    }
    if (merchandiseGiftViewModel.TotalCashLeft() >= 0) {
        merchandiseGiftViewModel.TotalGiftLeftCashCss('text-success');
    } else {
        merchandiseGiftViewModel.TotalGiftLeftCashCss('text-danger');

    }
    if (gitfCartViewModel.TotalGold() >= 0) {
        gitfCartViewModel.TotalGoldCss('text-success');
    } else {
        gitfCartViewModel.TotalGoldCss('text-danger');

    }

    if (gitfCartViewModel.TotalSilver() >= 0) {
        gitfCartViewModel.TotalSilverCss('text-success');
    } else {
        gitfCartViewModel.TotalSilverCss('text-danger');

    }
}


function showGiftInventory() {
    $("#merchandisegiftcontainer").html(giftmerchandiseView);
    var cashLeft = merchandiseGiftViewModel.TotalCashLeft();
    merchandiseGiftViewModel = null;
    initializemerchandiseGift();
    setupSendMerchandiseGift();
    getMerchandiseGift(0);
    merchandiseGiftViewModel.TotalCashLeft(cashLeft);

}
function setupSendMerchandiseGift() {
    merchandiseGiftViewModel.showSendValidation = ko.computed(function () {
        var item;
        for (var i = 0; i < merchandiseGiftViewModel.sendingItemsIndex().length ; i++) {
            item = merchandiseGiftViewModel.merchandiseCodes()[merchandiseGiftViewModel.sendingItemsIndex()[i]];
            if (item.Quantity < merchandiseGiftViewModel.ToId().length) {
                return true;
            }
        }
        if (merchandiseGiftViewModel.sendingItemsIndex().length == 0 ||
            merchandiseGiftViewModel.ToId().length == 0 || merchandiseGiftViewModel.TotalCashLeft() < 0) {
            return true;
        }
        else {
            return false;
        }
    });
    applyKoSendMerchandiseGift();
    applyScrollMerchandise();
    updateCheckBox($("#gift-treasure"));
    $('#sendmerchandise').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveSendMerchandiseGift(btn);
    });


    $("#merchandisegiftshowmore").on("click", function () {
        getMerchandiseGift(merchandiseGiftViewModel.lastMerchandiseTypeId());
    });
}
function addgiftmerchandiseKnob(merchandiseTypeId) {
    $("#merchandiseGiftcontent-wrapper input[data-id=" + merchandiseTypeId + "]").knob(
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
function getMerchandiseGift(lastMerchandiseTypeId) {
    if (merchandiseGiftViewModel.contentLoadTriggered == true) {
        return;
    }
    merchandiseGiftViewModel.contentLoadTriggered = true;
    $("#merchandisegiftshowmore").prop("disabled", true);
    $.ajax({
        url: "/api/MerchandiseService/getmerchandiseinventory",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: { lastMerchandiseTypeId: lastMerchandiseTypeId },
        beforeSend: showLoadingImage($("#merchandiseGiftcontent-box .panel-heading")),
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    addmerchandiseGiftItemCss(result[i]);
                    result[i].PurchasedAt = ParseDate(result[i].PurchasedAt);
                    result[i].ResaleAt = (result[i].PurchasedPrice *
                        (result[i].ResaleRate / 100) * (result[i].MerchandiseCondition / 100)).toFixed(2);
                    merchandiseGiftViewModel.merchandiseCodes.push(result[i]);
                    merchandiseGiftViewModel.lastMerchandiseTypeId(result[i].MerchandiseTypeId);
                    addgiftmerchandiseKnob(result[i].MerchandiseTypeId);
                }
                merchandiseGiftViewModel.showView(true);
                $("#merchandiseGiftcontent-box fieldset").removeClass("hidden");
            }

            $("#merchandisegiftshowmore").prop("disabled", false);
        }
    }).always(function () {
        merchandiseGiftViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#merchandiseGiftcontent-box .panel-heading"));
    });;
}
function saveSendMerchandiseGift(btn) {
    console.log("Entering");
    console.log(merchandiseGiftViewModel.MerchandiseTypeId());
    $.ajax({
        url: "/api/giftservice/savesendgiftproperty",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(merchandiseGiftViewModel),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            merchandiseGiftViewModelresult = ko.mapping.fromJS(result);
            if (merchandiseGiftViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#merchandiseGiftcontent-box").html("");
                $("#merchandiseGiftcontent-box").addClass("hidden");
                $("#merchandiseGiftcontent-submit").removeClass("hidden");
                $("#merchandiseGiftcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(merchandiseGiftViewModelresult, $("#merchandiseGiftcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#merchandisegiftcontainer").offset().top }, "slow");
                bankAccountViewModel.Cash(merchandiseGiftViewModel.TotalCashLeft());
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function addmerchandiseGiftItemCss(item) {
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
function updatemerchandiseGiftViewModel() {
    var total = 0;
    var item;
    merchandiseGiftViewModel.MerchandiseTypeId.removeAll();
    for (var i = 0; i < merchandiseGiftViewModel.sendingItemsIndex().length ; i++) {
        item = merchandiseGiftViewModel.merchandiseCodes()[merchandiseGiftViewModel.sendingItemsIndex()[i]];
        total += parseFloat(item.PurchasedPrice) * merchandiseGiftViewModel.ToId().length;
        merchandiseGiftViewModel.MerchandiseTypeId.push(item.MerchandiseTypeId);
    }
    var tax = total * bankAccountViewModel.GiftRate.TaxRate() / 100;
    merchandiseGiftViewModel.TotalTax(tax);
    var leftCash = bankAccountViewModel.Cash() - tax;
    merchandiseGiftViewModel.TotalCashLeft(leftCash);
    setupgiftviewmodelCss();
}
