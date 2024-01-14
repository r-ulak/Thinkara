var taxDetailsViewModel;
var taxDetailsViewModelresult;
function getCountryTaxViewModel(taskId) {
    if (taskId == "null") {
        taxresultBinding(taxViewModelCache[0]);
        return;
    }
    $.ajax({
        url: "/api/countrytaxservice/getcountrytax",
        type: "get",
        contentType: "application/json",
        data: { taskId: taskId },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#mycountryTaxcontent-box .panel-body").eq(0)),
        success: function (result) {
            taxresultBinding(result);
            taxDetailsViewModel.AllowEdit(false);
        }
    }).always(function () {
        hideLoadingImage($("#mycountryTaxcontent-box .panel-body").eq(0));
    });
}
function SlideInput(self, index) {
    self.simpleSlider();
    self.bind("slider:changed", function (event, data) {
        //var item = (event.currentTarget);
        var percent = parseFloat("0" + data.value * 100, 10);
        taxDetailsViewModel.TaxType()[index].TaxPercent(percent.toFixed(2));

        var total = 0;
        for (var p = 0; p < taxDetailsViewModel.TaxType().length; ++p) {
            total += parseFloat(taxDetailsViewModel.TaxType()[p].TaxPercent());
        }

        taxDetailsViewModel.Total(total.toFixed(2));
    });
}
function SaveTax(btn) {
    if (!taxDetailsViewModel.AllowEdit()) {
        btn.button('reset')
        return;
    }
    $.ajax({
        url: "/api/countrytaxservice/savetax",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(taxDetailsViewModel),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            taxDetailsViewModelresult = ko.mapping.fromJS(result);
            if (taxDetailsViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mycountryTaxcontent-box").html("");
                $("#mycountryTaxcontent-box").addClass("hidden");
                $("#mycountryTaxcontent-submit").removeClass("hidden");
                $("#mycountryTaxcontent-submit").next(".backbtn").removeClass("hidden");

                ko.applyBindings(taxDetailsViewModelresult, $("#mycountryTaxcontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#countryTaxcontainer").offset().top }, "slow");

            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            btn.button('reset')
        }
    }).always(function () {
        btn.button('reset')
    });
}
function TaxReInitialize() {
    taxDetailsViewModel = null;
    taxDetailsViewModelresult = null;

}
function TaxInitialize() {
    getCountryTaxViewModel("null");
    $('#savetax').click(function () {
        var btn = $(this)

        btn.button('loading')
        SaveTax(btn);

    });
}
function taxresultBinding(result) {
    $("#mycountryTaxcontent-box .panel-body div").removeClass('hidden');
    taxDetailsViewModel = ko.mapping.fromJS(result);
    if (peopleInfo.IsLeader() != 'true') {
        $("#savetax").popover({
            trigger: 'hover click foucs',
            placement: 'top',
            container: 'body'
        });
    } else {
        taxDetailsViewModel.AllowEdit(true);
    }
    taxDetailsViewModel.AfterRenderAction = function (elem) {
        var self = $(elem).find(".taxType");
        var index = self.data("index");
        SlideInput(self, index)
    }
    ko.applyBindings(taxDetailsViewModel, $("#mycountryTaxcontent-box").get(0));



}