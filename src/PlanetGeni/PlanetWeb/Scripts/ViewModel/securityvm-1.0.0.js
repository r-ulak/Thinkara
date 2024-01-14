var securityDetailsViewModel;
var securityDetailsViewModelresult;
var weaponInvViewModel;
var weaponSummaryViewModel;
var requestWarKeyView;
var warrequestKeyViewModelresult;
var warrequestTargetCountryId;
function initializesecurity() {
    securityDetailsViewModelresult = null;
    securityDetailsViewModel = null;
    weaponInvViewModel = null;

    securityDetailsViewModel = {
        weaponCodes: ko.observableArray(),
        Amount: ko.observable(0),
        AmountAvailable: ko.observable(0),

        AfterWeaponRenderAction: function (elem) {
            var self = $(elem).find(".weapontype");
            var index = self.data("index");
            if (index == null)
                return;
            slidewepaonInput(self, index);
            $('.productbox div [data-toggle="popover"]').popover({
                trigger: 'hover click foucs',
                placement: 'top',
                container: 'body'
            })
        }
    };
    weaponInvViewModel = {
        weaponDetailsDTO: ko.observableArray(),
        lastWeaponId: ko.observable(0),
    };
    warrequestTargetCountryId =
        {
            targetCountryId: ko.observable(''),
            targetCountryName: ko.observable('')
        };
    weaponSummaryViewModel = {
        weaponSummaryDTO: ko.observableArray(),
        weaponTopN: ko.observableArray()
    }

    ko.applyBindings(weaponInvViewModel, $("#securityInventorycontent-box .panel-body").get(0));
    ko.applyBindings(weaponInvViewModel, $("#securityInventorycontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo, $("#securityInventorycontent-box .panel-heading").get(0));
    ko.applyBindings(peopleInfo, $("#mysecuritysummarycontent-box .panel-heading").get(0));

    getCountrysecurityViewModel();
    getCountryDefenseBudget();

    setsecurityScorllbar();

    $('#savesecurity').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveWepaonCart(btn);

    });
    initializeSecurityNavigation();
    TryGetWarRequest();
}
function setsecurityScorllbar() {
    $("#weaponInventorycontent-box, #sea, #air, #land  ").perfectScrollbar(
     {
         suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
     });

    $('.accordionsecurity').on('shown.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').addClass('active');
        $(e.target).animate({ height: "235" }, 600);
    });

    $('.accordionsecurity').on('hidden.bs.collapse', function (e) {
        $(e.target).prev('.panel-heading').removeClass('active');
    });
}
function initializeSecurityNavigation() {

    $('#nat-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#nat-summary") {
            if (weaponSummaryViewModel.weaponSummaryDTO().length == 0) {
                getWeaponSummary();
            }
            if (weaponSummaryViewModel.weaponTopN().length == 0) {
                getTopNWeaponStack();
            }
        }
        else if (self.attr('href') == "#nat-inventory") {
            if (weaponInvViewModel.weaponDetailsDTO().length == 0) {
                getWeaponInventory(0);
            }
            $("#weaponinvshowmore").on("click", function () {
                getWeaponInventory(weaponInvViewModel.lastWeaponId());
            });
        }
        else if (self.attr('href') == "#nat-warkey") {
            if (typeof requestWarKeyView != 'undefined') {
                ko.applyBindings(countryCodewithoutownViewModel, $("#ddCountrycd").get(0));
                ko.applyBindings(warrequestTargetCountryId, $("#targetCountryIdSprite").get(0));
                var countrycodedd = new DropDown($('#ddCountrycd'));
                countrycodedd.opts.on('click', function () {
                    warrequestTargetCountryId.targetCountryId($(this).data("id"));
                    warrequestTargetCountryId.targetCountryName($(this).data("name"));
                });

                $("#ddCountrycd ul").perfectScrollbar({
                    suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
                });


            }
        }
    })
}
function getCountryDefenseBudget() {
    $.ajax({
        url: "/api/countrybudgetservice/getcountrybudgetfornationaldefense",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            securityDetailsViewModel.AmountAvailable(result);
        }
    });
}
function addSecurityInvItemCss(item) {
    item.dialColor = ko.computed(function () {
        var result = "#9ABC32";
        if (item.WeaponCondition < 50)
            result = "#D53F40";
        if (item.WeaponCondition >= 50 && item.WeaponCondition < 80)
            result = "#E8B110";
        if (item.WeaponCondition >= 80)
            result = "#9ABC32";
        return result;
    });
}
function addSecuritySummaryItemCss(item) {
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

        if (item.WeaponTypeCode.indexOf("A") > -1)
            result = "fa icon-airplane27";
        else if (item.WeaponTypeCode.indexOf("L") > -1)
            result = "fa icon-war6";
        else if (item.WeaponTypeCode.indexOf("S") > -1)
            result = "fa fa-life-ring";
        return result;
    });

    return item;
}
function getWeaponInventory(lastWeaponId) {
    $.ajax({
        url: "/api/NationalSecurityService/getweaponinventory",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        data: { lastWeaponId: lastWeaponId },
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    addSecurityInvItemCss(result[i]);
                    result[i].PurchasedAt = ParseDate(result[i].PurchasedAt);
                    weaponInvViewModel.weaponDetailsDTO.push(result[i]);
                    weaponInvViewModel.lastWeaponId(result[i].CountryWeaponId);
                    addSecurityKnob(result[i].CountryWeaponId);
                }

            }
        }
    });
}
function addSecurityKnob(weaponId) {
    $("#weaponInventorycontent-wrapper input[data-id=" + weaponId + "]").knob(
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
function getTopNWeaponStack() {
    $.ajax({
        url: "/api/nationalsecurityservice/gettoptenweaponstackcountry",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    result[i].Rank = ordinal_suffix_of(i + 1);
                    weaponSummaryViewModel.weaponTopN.push(result[i]);
                }
                ko.applyBindings(weaponSummaryViewModel,
                    $("#weaponTopNSummary").get(0));
            }
        }
    });
}
function getWeaponSummary() {
    $.ajax({
        url: "/api/nationalsecurityservice/getweaponsummary",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    addSecuritySummaryItemCss(result[i]);
                    weaponSummaryViewModel.weaponSummaryDTO.push(result[i]);
                }
                ko.applyBindings(weaponSummaryViewModel, $("#weaponSummarycontent-box").get(0));

                $("#weaponSummarycontent-wrapper .dial").knob(
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
function getCountrysecurityViewModel() {
    $.ajax({
        url: "/api/NationalSecurityService/getweapontypes",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    result[i].Quantity = 0;
                }
                securityDetailsViewModel.weaponCodes = ko.mapping.fromJS(result);
                securityDetailsViewModel.ShowValidation = ko.computed(function () {
                    if (securityDetailsViewModel.AmountAvailable() >= 0)
                    { return false; }
                    else {
                        return true;
                    }
                });
                ko.applyBindings(securityDetailsViewModel, $("#mysecuritycontent-box .panel-body").get(0));
                ko.applyBindings(securityDetailsViewModel, $("#mysecuritycontent-box .panel-footer").get(0));
                ko.applyBindings(peopleInfo, $("#mysecuritycontent-box .panel-heading").get(0));
                ko.applyBindings(securityDetailsViewModel, $("#savesecurity").get(0));

            }
        }
    });
}
function slidewepaonInput(self, index) {
    self.simpleSlider();
    self.bind("slider:changed", function (event, data) {
        var item = (event.currentTarget);
        var index = $(item).data("index");
        var qty = parseInt("0" + data.value * 100, 10);
        var available = securityDetailsViewModel.AmountAvailable();
        securityDetailsViewModel.weaponCodes()[index].Quantity(qty);
        var newTotal = computeSecurityTotal();
        var currentTotal = securityDetailsViewModel.Amount();
        securityDetailsViewModel.Amount(newTotal);
        securityDetailsViewModel.AmountAvailable(available - (newTotal - currentTotal));
    });
}
function computeSecurityTotal() {

    var total = 0;
    var cost = 0;
    var quantity = 0;
    for (var p = 0; p < securityDetailsViewModel.weaponCodes().length; ++p) {
        cost = securityDetailsViewModel.weaponCodes()[p].Cost();
        quantity = securityDetailsViewModel.weaponCodes()[p].Quantity();
        total += cost * quantity;
    }
    return total;
}
function saveWepaonCart(btn) {
    var boughtItems = ko.utils.arrayFilter(securityDetailsViewModel.weaponCodes(),
         function (item) {
             return item.Quantity() > 0;
         });

    $.ajax({
        url: "/api/NationalSecurityService/savewepaoncart",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(boughtItems),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            securityDetailsViewModelresult = ko.mapping.fromJS(result);
            if (securityDetailsViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mysecuritycontent-box").html("");
                $("#mysecuritycontent-box").addClass("hidden");
                $("#mysecuritycontent-submit").removeClass("hidden");
                $("#mysecuritycontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(securityDetailsViewModelresult, $("#mysecuritycontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#securitycontainer").offset().top }, "slow");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function TryGetWarRequest() {
    if (typeof requestWarKeyView == 'undefined') {
        var myUrl = "/NationalSecurity/RequestWarKey"
        $.get(myUrl, function (data, textStatus, xhr) {
            if (xhr.status = 200) {
                requestWarKeyView = data;
                buildWarRequestView();
            }
        });
    } else {
        buildWarRequestView();
    }

}
function buildWarRequestView() {
    $("#nat-warkey").append(requestWarKeyView);
    $("#nat-container a[href='#nat-warkey']").removeClass("hidden");
    $('#requestWarKey').click(function () {
        var btn = $(this)
        btn.button('loading')
        saveRequestWarKey(btn);

    });
    ko.applyBindings(peopleInfo, $("#mywarRequestcontent-box .panel-heading").get(0));
}
function saveRequestWarKey(btn) {
    $.ajax({
        url: "/api/nationalsecurityservice/savewarrequest",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify({
            TargetCountryId: warrequestTargetCountryId.targetCountryId(),
            TargetCountryName: warrequestTargetCountryId.targetCountryName()
        }),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            warrequestKeyViewModelresult = ko.mapping.fromJS(result);
            if (warrequestKeyViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mywarRequestcontent-box").html("");
                $("#mywarRequestcontent-box").addClass("hidden");
                $("#mywarRequestcontent-submit").removeClass("hidden");
                $("#mywarRequestcontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(warrequestKeyViewModelresult, $("#mywarRequestcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#warRequestcontainer").offset().top }, "slow");
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset');
        }
    }).always(function () {
        btn.button('reset');
    });
}