var budgetDetailsViewModel;
var budgetDetailsViewModelresult;
function getCountryBudgetViewModel(taskId) {
    $.ajax({
        url: "/api/countrybudgetservice/getcountrybudget",
        type: "get",
        contentType: "application/json",
        data: { taskId: taskId },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {

            for (i = 0; i < result.BudgetType.length; i++) {
                result.BudgetType[i].BudgetRatio = ((result.BudgetType[i].Amount / result.TotalAmount)).toFixed(2);
                result.BudgetType[i].BudgetPercent = ((result.BudgetType[i].Amount / result.TotalAmount) * 100).toFixed(2);
                result.IsTotalValid = ko.observable(false);
            }
            budgetDetailsViewModel = ko.mapping.fromJS(result);
            budgetDetailsViewModel.AfterRenderAction = function (elem) {
                var self = $(elem).find(".budgetType");
                var index = self.data("index");
                spinInput(self, index)

            }
            if (peopleInfo.IsLeader() != 'true') {
                $("#savebudget").popover({
                    trigger: 'hover click foucs',
                    placement: 'top',
                    container: 'body'
                });
            }
            else if (taskId != 'null') {
                budgetDetailsViewModel.AllowEdit(false);
            }
            else {
                budgetDetailsViewModel.AllowEdit(true);
            }


            ko.applyBindings(budgetDetailsViewModel, $("#mycountryBudgetcontent-box").get(0));
            budgetDetailsViewModel.IsTotalValid(IsTotalValid());



            $(".budgetType").bind("change", function (obj) {
                calculateTotals(this);
            });
        }
    });
}
function calculateTotals(obj) {
    var index = $(obj).data("index");
    var changedPercentValue = parseFloat($(obj).val()).toFixed(2);
    var totalAmount = parseFloat(budgetDetailsViewModel.TotalAmount()).toFixed(2);
    var budgetType = budgetDetailsViewModel.BudgetType();
    var currentPercent = parseFloat(budgetType[index].BudgetPercent()).toFixed(2);
    var currentValue = parseFloat(budgetType[index].Amount()).toFixed(2);
    var currentAmountLeft = parseFloat(budgetDetailsViewModel.AmountLeft()).toFixed(2);
    var deltaPercent = (changedPercentValue - currentPercent).toFixed(2);
    var totalPercent = 0;
    for (i = 0; i < budgetDetailsViewModel.BudgetType().length; i++) {
        if (i != index) {
            totalPercent = parseFloat(totalPercent) + parseFloat(budgetType[i].BudgetPercent());
            //console.log(i + " " + parseFloat(budgetType[i].BudgetPercent()) + "    "
            //+ parseFloat(budgetType[i].BudgetPercent()).toFixed(2));
        }
        else {
            totalPercent = parseFloat(totalPercent) + parseFloat(changedPercentValue);
            //console.log("changed value " + parseFloat(changedPercentValue));

        }
    }
    //console.log("total " + parseFloat(totalPercent));

    if (parseFloat(totalPercent) > 100 && deltaPercent > 0) {
        $(obj).val(currentPercent);

        return;
    }


    var newAmountLeft;

    $(obj).val(changedPercentValue);
    budgetType[index].Amount((totalAmount * changedPercentValue / 100).toFixed(2));

    budgetType[index].BudgetPercent(changedPercentValue);
    budgetDetailsViewModel.BudgetType(budgetType);
    newAmountLeft = totalAmount;
    for (i = 0; i < budgetDetailsViewModel.BudgetType().length; i++) {
        newAmountLeft = newAmountLeft - budgetDetailsViewModel.BudgetType()[i].Amount();
    }
    if (newAmountLeft < 0)
        newAmountLeft = 0;
    budgetDetailsViewModel.AmountLeft(newAmountLeft);
    budgetDetailsViewModel.IsTotalValid(IsTotalValid());

}
function spinInput(self, index) {
    self.TouchSpin({
        prefix: budgetDetailsViewModel.BudgetType()[index].Description(),
        initval: budgetDetailsViewModel.BudgetType()[index].BudgetPercent(),
        min: 0,
        max: 100,
        step: 0.01,
        decimals: 2,
        booster: false,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        prefix_extraclass: "textLeft",
        postfix: '%',
        mousewheel: true
    });
}
function IsTotalValid() {
    var totalPercent = 0;
    var budgetType = budgetDetailsViewModel.BudgetType();
    for (i = 0; i < budgetDetailsViewModel.BudgetType().length; i++) {
        totalPercent = parseFloat(totalPercent) + parseFloat(budgetType[i].BudgetPercent());
    }
    if (totalPercent > 100) {
        return false
    }
    return true;
}
function SaveBudget(btn) {
    if (budgetDetailsViewModel.IsTotalValid() == false || !taxDetailsViewModel.AllowEdit()) {
        btn.button('reset');
        return;
    }

    $.ajax({
        url: "/api/countrybudgetservice/savebudget",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(budgetDetailsViewModel),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            budgetDetailsViewModelresult = ko.mapping.fromJS(result);
            if (budgetDetailsViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mycountryBudgetcontent-box").html("");
                $("#mycountryBudgetcontent-box").addClass("hidden");
                $("#mycountryBudgetcontent-submit").removeClass("hidden");
                $("#mycountryBudgetcontent-submit").next(".backbtn").removeClass("hidden");

                ko.applyBindings(budgetDetailsViewModelresult, $("#mycountryBudgetcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#countryBudgetcontainer").offset().top }, "slow");

            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function BudgetReInitialize() {
    budgetDetailsViewModel = null;
    budgetDetailsViewModelresult = null;
    ko.cleanNode($("#mycountryBudgetcontent-box").get(0));

}
function BudgetInitialize() {
    getCountryBudgetViewModel("null");
    $('#savebudget').click(function () {
        if (IsTotalValid() == false) {
            return;
        }
        var btn = $(this)
        btn.button('loading')
        SaveBudget(btn);

    });
}