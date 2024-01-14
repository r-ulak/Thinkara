var adsViewModel;
var adsPostViewModel;

function initializeads() {
    adsInitialize();
    setupAds();

}
function adsInitialize() {
    adsPostViewModel = {
        adsTypeList: ko.observableArray(),
        adsFrequencyTypeId: -1,
        days: ko.observableArray(),
        adTime: "",
        startDate: "",
        endDate: "",
        message: "",
        previewMsg: "",
        imagesize: 0,
        cost: 0,
        totalCost: 0,
    };

    adsViewModel = ko.mapping.fromJS(cacheAdsType[0]);
    adsViewModel.uploadprofilePicObj =new uploadImageObj('ads');
    adsViewModel.posturlContent= [];
    adsViewModel.msgContent = ko.observable('');
    adsViewModel.previewContent = ko.observable('');
    adsViewModel.availableCash = ko.observable(userBankAccountViewModel.Cash());
    adsViewModel.totalCost = ko.observable(0);
    adsViewModel.cost = ko.observable(0);
    adsViewModel.tax = ko.observable(0);
    adsViewModel.taxRate = ko.observable(taxViewModel.TaxType()[taxadsCode - 1].TaxPercent());
    adsViewModel.picture = ko.observable(peopleInfo.Picture());
    adsViewModel.fullName = ko.observable(peopleInfo.FullName());
    adsViewModel.userId = ko.observable(peopleInfo.UserId());
    adsViewModel.adsTypeSelect = function (data, event) {
        var self = $(event.currentTarget);
        var id = self.data('id');
        if (self.is(':checked')) {
            adsPostViewModel.adsTypeList.push(id);
        } else {
            adsPostViewModel.adsTypeList.remove(id);
        }
        calcAdsTotal();
        return true;
    }
    adsViewModel.adsDaysSelect = function (data, event) {
        console.log("dayaysysys");
        var self = $(event.currentTarget);
        var id = self.data('id');
        if (adsPostViewModel.days.indexOf(id) < 0) {
            adsPostViewModel.days.push(id);
        }
        else {
            adsPostViewModel.days.remove(id);
        }
        calcAdsTotal();
        return true;
    }
}
function setupAds() {
    getAdsType();
    $("#adsupload").change(function (evt) {
        if (this.disabled) return alert('File upload not supported!');
        handleFileSelect(evt,
            $("#adsuploadprw"), adsViewModel.uploadprofilePicObj, '', spotpreview);
    });
    $('#adsContent').on('keydown blur', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (e.type == 'blur') {
            preview();
        }

        if (code == 32 || code == 13) {
            preview();
        }
        if (code == 8 || code == 46) {
            CleanUpUrls(adsViewModel.msgContent(), 'Post', adsViewModel);
            preview();
        }
    });
    $("#adsreset").on("click", function () {
        resetAds();
    });

    $('#saveads').click(function () {
        var btn = $(this)
        btn.button('loading')
        SaveAds(btn);
    });

    defineValidationRulesAds();
    ko.applyBindings(adsViewModel,
        $("#myadscontent-box .panel-body").get(0));
    ko.applyBindings(adsViewModel,
        $("#myadscontent-box .panel-footer").get(0));
    ko.applyBindings(peopleInfo,
        $("#myadscontent-box .panel-heading").get(0));
    ko.applyBindings(adsViewModel,
       $("#saveads").get(0));

    $('#adsStartDate').datetimepicker({
        pickTime: false,
        defaultDate: true,
        useStrict: true,
        minDate: moment()
    });
    $('#adsEndDate').datetimepicker({
        pickTime: false,
        defaultDate: true,
        useStrict: true
    });
    $("#adsStartDate").on("dp.change", function (e) {
        $('#adsEndDate').data("DateTimePicker").setMinDate(e.date);
        calcAdsTotal();
    });
    $("#adsEndDate").on("dp.change", function (e) {
        $('#adsStartDate').data("DateTimePicker").setMaxDate(e.date);
        calcAdsTotal();
    });
    $('#adsTime').datetimepicker({
        pickDate: false,
        useMinutes: false,
        useSeconds: false,
        useCurrent: false,
        useStrict: true
    });


    $("#adsTime input").attr('disabled', true);

    var ddtime = new DropDown($('#ddtime'));

    ddtime.opts.on('click', function () {
        adsPostViewModel.adTime = moment($(this).data("id"), 'HH').utc().format('HH');
    });
    var ddrecur = new DropDown($('#ddrecur'));
    ddrecur.opts.on('click', function () {
        recurrClicked($(this).data("id"));
        adsPostViewModel.adsFrequencyTypeId = $(this).data("id");
        calcAdsTotal();
    });
    $("#ddrecur ul, #ddtime ul").perfectScrollbar({
        suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
    });
}
function resetAds() {
    $('#adsContent').val('');
    $('#adsuploadprw').html('');
    adsViewModel.uploadprofilePicObj.blobSasUrl('');
    adsViewModel.uploadprofilePicObj.buffer = '';
    adsViewModel.uploadprofilePicObj.validImage = false;
    preview();
}
function recurrClicked(frequencyId) {
    if (frequencyId == 5) {
        $("#adsDaysInweek").removeClass("hidden");
    } else {
        $("#adsDaysInweek").addClass("hidden");
    }
}
function preview() {
    var content = htmlEscape($('#adsContent').val());
    var imageurl = '';
    buildWebContent(content, 'Post', true, adsViewModel, function () {
        for (var i = 0; i < adsViewModel.posturlContent.length; i++) {
            var item = adsViewModel.posturlContent[i];
            if (item.Content) {
                content = content.split(item.Uri).join(item.Content) //find and replace
            }
        }
        content = content.replace(/\r\n|\r|\n/g, "<br />");
        adsViewModel.previewContent(content);
        calcAdsTotal();
    });
}
function SaveAds(btn) {
    if ($('#adsform').valid()) {
        var vm = adsViewModel.uploadprofilePicObj;
        uploadImage(vm, $("#adsuploadprw"));

        collectDataAds();
        var adsDetailsViewModelresult;
        $.ajax({
            url: "/api/advertisementservice/saveads",
            type: "post",
            contentType: "application/json",
            data: ko.toJSON(adsPostViewModel),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                adsDetailsViewModelresult = ko.mapping.fromJS(result);
                if (adsDetailsViewModelresult.StatusCode() == 200) {
                    userNotificationPollOnce();
                    btn.addClass("hidden");
                    $("#myadscontent-box").html("");
                    $("#myadscontent-box").addClass("hidden");
                    $("#myadscontent-submit").removeClass("hidden");
                    $("#myadscontent-submit").next(".backbtn").removeClass("hidden");
                    ko.applyBindings(adsDetailsViewModelresult, $("#myadscontent-submit").get(0));
                    $("#main").animate({ scrollTop: $("#adscontainer").offset().top }, "slow");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                btn.button('reset');
            }
        }).always(function () {
            btn.button('reset');
        });
    } else {
        btn.button('reset');
    }
}
function collectDataAds() {
    preview();
    calcAdsTotal();
    adsPostViewModel.imagesize = adsViewModel.uploadprofilePicObj.buffer.byteLength;
    adsPostViewModel.message = adsViewModel.msgContent();
    adsPostViewModel.previewMsg = adsViewModel.previewContent();
    adsPostViewModel.startDate = moment($('#adsStartDate').data("date")).utc().format('L');
    adsPostViewModel.endDate = moment($('#adsEndDate').data("date")).utc().format('L')

    adsPostViewModel.cost = adsViewModel.cost();
    adsPostViewModel.totalCost = adsViewModel.totalCost();
}
function defineValidationRulesAds() {
    $('#adsform').validate({
        ignore: "",
        rules: {
            adsContent: {
                required: true,
                minlength: 5,
                maxlength: 1000
            },
            adsStartDate: {
                required: true,
                date: true
            },
            adsEndDate: {
                required: true,
                date: true
            },
            adsType: {
                required: true
            },
            adstime: {
                required: true
            },
            adsfrequency: {
                required: true
            },
            adsDays: {
                required: function (elem) {
                    if ($("#ddrecur >span").text().trim() == 'Custom') {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        },
        messages: {
            adsContent: {
                required: "Provide a Message",
                minlength: jQuery.format("Enter at least {0} characters"),
                maxlength: jQuery.format("Enter less than {0} characters")
            },
            adsStartDate: {
                required: "Provide a StartDate",
                rangelength: jQuery.format("Enter at least {0} characters")
            },
            adsEndDate: {
                required: "Provide a EndDate",
                rangelength: jQuery.format("Enter at least {0} characters")
            },
            adsType: {
                required: "You must check at least 1 box"
            },
            adstime: {
                required: "Provide a Time"
            },
            adsfrequency: {
                required: "Provide a Frequency"
            },
            adsDays: {
                required: "Select at least one day"
            }
        },
        // specifying a submitHandler prevents the default submit, good for the demo
        submitHandler: function () {
            alert("submitted!");
        },
        // set this class to error-labels to indicate valid fields
        success: function (label) {
            // set &nbsp; as text for IE
            label.html("&nbsp;").addClass("checked");
        },
        errorPlacement: function (error, element) {
            if (element.parent('.input-group-addon').parent().parent('li').parent('ul').length) {
                error.insertAfter(element.parent().parent().parent('li').parent('ul'));
            }
            else if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            }
            else if (element.parent().parent('li').parent('ul').length) {
                error.insertAfter(element.parent().parent('li').parent('ul'));
            } else if (element.parent('.wrapper-dropdown-3').length) {
                error.insertAfter(element.parent('.wrapper-dropdown-3'));
            } else if (element.parent().parent('#adsDaysInweek').length) {
                error.insertAfter(element.parent().parent('#adsDaysInweek'));
            }
            else if (element.parent('.status-upload').length) {
                error.insertAfter(element.parent('.status-upload'));
            }
            else {
                error.insertAfter(element);
            }
        }
    });

}
function calcAdsTotal() {
    var adsLength = adsViewModel.previewContent().length;
    var costTotal = 0;
    var taxTotal = 0;
    var total = 0;
    for (var i = 0; i < adsPostViewModel.adsTypeList().length; i++) {
        var adsType = ko.utils.arrayFirst(adsViewModel.AdsTypeList(),
            function (item) {
                return adsPostViewModel.adsTypeList()[i] === item.AdsTypeId();
            });
        if (typeof adsViewModel.uploadprofilePicObj.buffer.byteLength != 'undefined') {
            costTotal += adsViewModel.uploadprofilePicObj.buffer.byteLength * adsType.PricePerImageByte();
        }
        costTotal = costTotal + adsType.BaseCost();
        costTotal = costTotal + adsLength * adsType.PricePerChar();
        console.log(costTotal);

    }

    var startDate = moment($('#adsStartDate').data("date"));
    var endDate = moment($('#adsEndDate').data("date"));
    if (typeof endDate != 'undefined') {
        var totalDays = endDate.diff(startDate, 'days') + 1;
        if (adsPostViewModel.adsFrequencyTypeId !== -1) {

            var adsFrequency = ko.utils.arrayFirst(adsViewModel.FrequencyAds(),
          function (item) {
              return adsPostViewModel.adsFrequencyTypeId === item.AdsFrequencyTypeId();
          });
            var fqMultiple = adsFrequency.FrequencyMultiple();
            if (fqMultiple === 0) {
                fqMultiple = 1 + adsPostViewModel.days().length;
            }
            if (totalDays > 5) {
                costTotal = costTotal * totalDays * totalDays * totalDays * fqMultiple;
            }
            else {
                costTotal = costTotal * totalDays * fqMultiple;
            }
            console.log(costTotal);

        }

    }


    taxTotal = (adsViewModel.taxRate() / 100) * costTotal;
    total = costTotal + taxTotal;
    adsViewModel.cost(costTotal);
    adsViewModel.tax(taxTotal);
    adsViewModel.totalCost(total);

}