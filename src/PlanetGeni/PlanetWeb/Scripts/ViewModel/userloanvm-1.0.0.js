var userLoanMyViewModel;
var userLoanRequestViewModel;
var userLoanRequestViewModelresult;
var userPayLoanRequestViewModelresult;
var requestLoanView;
var payloanCarosuelId = 14;
var sourcepayloanCarosuelId;
var paymentIndex = -1;
function userLoanInitialize() {
    userLoanMyViewModel = {
        contentLoadTriggered: false,
        userLoanMyDetailsDTO: ko.observableArray(),
        lastLendorUpdatedAt: ko.observable(''),
        lastBorrowerUpdatedAt: ko.observable(''),
        totalBorrowed: ko.observable(0),
        totalLended: ko.observable(0),
        totalBorrowedLeft: ko.observable(0),
        totalLendedLeft: ko.observable(0),
        totalBorrowedPaid: ko.observable(0),
        totalLendedPaid: ko.observable(0),
        lendorFullName: ko.observable('Bank'),
        lendorCountryCode: ko.observable(''),
        lendorOnlineStatus: ko.observable(0),
        lendorPicture: ko.observable('bank3.png'),
        loanCount: ko.observable(bankId)
    };
    userLoanRequestViewModel = null;
    userLoanRequestViewModelresult = null;
    requestLoanView = $("#loanRequestcontainer").html();
    setupMyloans();
    initializeNavigation();
}
function setupMyloans() {
    ko.applyBindings(userLoanMyViewModel, $("#loanviewcontent-box")[0]);
    ko.applyBindings(userLoanMyViewModel, $("#loanviewfooter")[0]);
    ko.applyBindings(peopleInfo, $("#myloanviewcontent-box .panel-heading").get(0));

    getDataMyUserLoan();

    $("#loanviewcontent-box").perfectScrollbar(
    {
        suppressScrollX: true
    });
    $("#loanviewshowmore").on("click", function () {
        getDataMyUserLoan();
    });
    $("#loanrefresh").on("click", function () {
        refreshLoanList();
    });
}
function refreshLoanList() {
    userLoanMyViewModel.userLoanMyDetailsDTO.removeAll();
    userLoanMyViewModel.lastBorrowerUpdatedAt('');
    userLoanMyViewModel.lastLendorUpdatedAt('');
    userLoanMyViewModel.totalBorrowed(0);
    userLoanMyViewModel.totalLended(0);
    userLoanMyViewModel.totalBorrowedLeft(0);
    userLoanMyViewModel.totalLendedLeft(0);
    userLoanMyViewModel.totalBorrowedPaid(0);
    userLoanMyViewModel.totalLendedPaid(0);
    userLoanMyViewModel.loanCount(0);
    getDataMyUserLoan();
}
function getDataMyUserLoan() {
    if (userLoanMyViewModel.contentLoadTriggered == true) {
        return;
    }
    userLoanMyViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/userloanservice/getloanlist",
        type: "get",
        contentType: "application/json",
        data: {
            lastLendorUpdatedAt: userLoanMyViewModel.lastLendorUpdatedAt(),
            lastBorrowerUpdatedAt: userLoanMyViewModel.lastBorrowerUpdatedAt()
        },
        beforeSend: showLoadingImage($("#myloanviewcontent-box .panel-heading")),
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (data) {
            if (data.length > 0) {
                userLoanMyViewModel.loanCount(userLoanMyViewModel.loanCount() + data.length);
                data.sort(function (l, r) {
                    return l.LoanSourceType === r.LoanSourceType
                    ? l.UpdatedAt < r.UpdatedAt ? 1 : -1
                    : l.LoanSourceType > r.LoanSourceType ? 1 : -1
                })
                var clickUrl = "";
                for (i = 0; i < data.length; i++) {
                    if (data[i].LoanSourceType == 'L' && data[i].Status == 'A') {
                        userLoanMyViewModel.lastLendorUpdatedAt(data[i].UpdatedAt);
                        userLoanMyViewModel.totalLended(userLoanMyViewModel.totalLended() + data[i].LoanAmount);
                        userLoanMyViewModel.totalLendedLeft(userLoanMyViewModel.totalLendedLeft() + data[i].LeftAmount);
                        userLoanMyViewModel.totalLendedPaid(userLoanMyViewModel.totalLendedPaid() + data[i].PaidAmount);
                    }
                    else if (data[i].LoanSourceType == 'B' && data[i].Status == 'A') {
                        userLoanMyViewModel.lastBorrowerUpdatedAt(data[i].UpdatedAt);
                        userLoanMyViewModel.totalBorrowed(userLoanMyViewModel.totalBorrowed() + data[i].LoanAmount);
                        userLoanMyViewModel.totalBorrowedLeft(userLoanMyViewModel.totalBorrowedLeft() + data[i].LeftAmount);
                        userLoanMyViewModel.totalBorrowedPaid(userLoanMyViewModel.totalBorrowedPaid() + data[i].LeftAmount);
                    }

                    data[i].OnClickUrl = "payLoan( '" + data[i].TaskId + "', " + i + ", this" + " )";
                    userLoanMyViewModel.userLoanMyDetailsDTO.push(data[i]);
                }
            }
        }
    }).always(function () {
        userLoanMyViewModel.contentLoadTriggered = false;
        hideLoadingImage($("#myloanviewcontent-box .panel-heading"));
    });
}
function disableRate() {
    $("#loanPercent").prop("disabled", true);
    $("#loanPercent").siblings('span.input-group-btn').children('button').prop("disabled", true);

}
function enableRate() {
    $("#loanPercent").prop("disabled", false);
    $("#loanPercent").siblings('span.input-group-btn').children('button').prop("disabled", false);

}
function setupRequestLoan() {
    ko.applyBindings(friendViewModel, $("#dduserloanFriends ul.dropdown")[0]);
    ko.applyBindings(userLoanMyViewModel, $("#selectedLendor")[0]);
    ko.applyBindings(peopleInfo, $("#myloanRequestcontent-box .panel-heading").get(0));
    spinloanInput();
    if (userLoanRequestViewModel === null) {
        getInterestRate();
    }
    else {
        ko.applyBindings(userLoanRequestViewModel, $("#askloanfooter").get(0));
    }
    selectBank();
    disableRate();
    $('#radiolendorBank, #lendorBank').on('click', function () {
        disableRate();
        $("#radiolendorBank").prop("checked", true);
        selectBank();
    });
    $('#radiolendorFriend').on('click', function () {
        enableRate();
    });
    var userloanlendordd = new DropDown($('#dduserloanFriends'));
    userloanlendordd.opts.on('click', function () {
        $("#radiolendorFriend").prop("checked", true)
        $("#loanPercent").prop("disabled", false);
        $("#loanPercent").siblings('span.input-group-btn').children('button').prop("disabled", false);
        userLoanMyViewModel.lendorCountryCode($(this).data("countrycode"));
        userLoanMyViewModel.lendorOnlineStatus($(this).data("onlinestatus"));
        userLoanMyViewModel.lendorFullName($(this).data("fullname"));
        userLoanMyViewModel.lendorPicture($(this).data("picture"));
        userLoanRequestViewModel.LendorId($(this).data("id"));
        $("input[name='loanLendorId']").val(userLoanRequestViewModel.LendorId());
    });
    userloanlendordd.dd.on('click', function () {
        $("#radiolendorFriend").prop("checked", true)
        $("#loanPercent").prop("disabled", false);
        $("#loanPercent").siblings('span.input-group-btn').children('button').prop("disabled", false);

    });
    $("#dduserloanFriends ul").perfectScrollbar({
        suppressScrollX: true, wheelPropagation: true, wheelSpeed: 5, minScrollbarLength: 10
    });
    $('#saveLoanRequest').click(function () {
        var btn = $(this)
        saveLoanResponse(btn);
    });
    $('#askmoreloan').click(function () {
        showRequestLoan();
    });
    defineRequestLoanValidationRules();
}
function spinloanInput() {
    var self = $("#loanAmount");
    self.TouchSpin({
        prefix: "Loan Me",
        initval: 0,
        min: 0,
        max: 1000000,
        step: 1,
        decimals: 2,
        booster: true,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        postfix: '<i class="fa icon-money28  fa-1x"></i>',
        mousewheel: true
    });
    self = $("#loanPercent");

    self.TouchSpin({
        prefix: "At",
        initval: 0,
        min: 0,
        max: 100,
        step: .1,
        decimals: 2,
        booster: false,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        postfix: '%',
        mousewheel: true
    });
}
function initializeNavigation() {
    $('#loan-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#loan-request") {
            showRequestLoan();
        }
    });
}
function getInterestRate() {
    $("#saveLoanRequest").prop("disabled", true);
    $.ajax({
        url: "/api/userloanservice/getquailfiedintrestedrate",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        beforeSend: showLoadingImage($("#myloanRequestcontent-box .panel-heading")),
        success: function (result) {
            userLoanRequestViewModel = ko.mapping.fromJS(result);
            ko.applyBindings(userLoanRequestViewModel, $("#askloanfooter").get(0));
            $("#saveLoanRequest").prop("disabled", false);
        }
    }).always(function () {
        hideLoadingImage($("#myloanRequestcontent-box .panel-heading"));
    });
}
function saveLoanResponse(btn) {
    if ($('#requestloanform').valid()) {
        collectData();
        btn.button('loading')
        $.ajax({
            url: "/api/userloanservice/RequestLoan",
            type: "post",
            contentType: "application/json",
            data: ko.toJSON(userLoanRequestViewModel),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                userLoanRequestViewModelresult = ko.mapping.fromJS(result);
                if (userLoanRequestViewModelresult.StatusCode() == 200) {
                    userNotificationPollOnce();
                    btn.addClass("hidden");
                    $("#myloanRequestcontent-box").html("");
                    $("#myloanRequestcontent-box").addClass("hidden");
                    $("#myloanRequestcontent-submit").removeClass("hidden");
                    $("#askmoreloan").removeClass("hidden");
                    $("#myloanRequestcontent-submit").next(".backbtn").removeClass("hidden");
                    ko.applyBindings(userLoanRequestViewModelresult, $("#myloanRequestcontent-submit").get(0));
                    $("#main").animate({ scrollTop: $("#loanRequestcontainer").offset().top }, "slow");
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
    userLoanRequestViewModel.LoanAmount($("#loanAmount").val());
    userLoanRequestViewModel.MonthlyIntrestRate($("#loanPercent").val());
    if ($('#radiolendorBank').is(':checked')) {
        userLoanRequestViewModel.LendorId(bankId);
    }
}
function defineRequestLoanValidationRules() {
    $('#requestloanform').validate({
        ignore: "",
        rules: {
            loanAmount: {
                required: true,
                min: 1,
                number: true
            },
            loanLendorId: {
                required: '#radiolendorFriend:checked',
                min: 1,
                number: true
            },
        },
        messages: {
            loanAmount: {
                min: jQuery.format("Enter Amount greater or equal to {0} ")
            },
            loanLendorId: {
                required: "Need to Select a Valid Lendor"
            }
        },// set this class to error-labels to indicate valid fields
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
function showRequestLoan() {
    $("#loanRequestcontainer").html(requestLoanView);
    setupRequestLoan();
}
function selectBank() {
    $("#radiolendorBank").prop("checked", true);
    userLoanMyViewModel.lendorCountryCode('');
    userLoanMyViewModel.lendorOnlineStatus(0);
    userLoanMyViewModel.lendorFullName('Bank');
    userLoanMyViewModel.lendorPicture('bank3.png');
}

function initializepayloan(maxAmount) {
    var self = $("#payloanAmount");
    self.TouchSpin({
        initval: 0,
        min: 0,
        max: maxAmount,
        step: 10,
        decimals: 2,
        booster: false,
        stepinterval: 1,
        mousewheel: true
    });
    $('#payloan').click(function () {
        var btn = $(this)
        savePayLoan(btn);
    });
    $('#payotherloan').click(function () {
        userloanViewClick("/UserLoan/GetUserLoans", userloanCarosuelId);
        $('#myCarousel').carousel(userloanCarosuelId);
    });
}
function payLoan(taskId, vmindex) {
    paymentIndex = vmindex;
    var self = $(this);
    if (self.closest('.panel').attr('id') != 'mypayLoancontent-box') {
        if (userLoanMyViewModel.userLoanMyDetailsDTO()[paymentIndex].LoanSourceType.indexOf("B") > -1) {
            payloanViewClick("/UserLoan/GetPayLoans", payloanCarosuelId, function () {
                ko.applyBindings(userLoanMyViewModel.userLoanMyDetailsDTO()[paymentIndex], $("#mypayLoancontent-box")[0]);
                initializepayloan(userLoanMyViewModel.userLoanMyDetailsDTO()[paymentIndex].LeftAmount);
                $('#myCarousel').carousel(payloanCarosuelId);
            });
        }
    }
}
function savePayLoan(btn) {

    collectPayLoanData();
    btn.button('loading')
    $.ajax({
        url: "/api/userloanservice/MakePayment",
        type: "post",
        contentType: "application/json",
        data: ko.toJSON(userLoanMyViewModel.userLoanMyDetailsDTO()[paymentIndex]),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            userPayLoanRequestViewModelresult = ko.mapping.fromJS(result);
            if (userPayLoanRequestViewModelresult.StatusCode() == 200) {
                userNotificationPollOnce();
                btn.addClass("hidden");
                $("#mypayLoancontent-box").html("");
                $("#mypayLoancontent-box").addClass("hidden");
                $("#mypayLoancontent-submit").removeClass("hidden");
                $("#payotherloan").removeClass("hidden");
                $("#mypayLoancontent-submit").next(".backbtn").removeClass("hidden");
                ko.applyBindings(userPayLoanRequestViewModelresult, $("#mypayLoancontent-submit").get(0));
                $("#main").animate({ scrollTop: $("#payLoancontainer").offset().top }, "slow");
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
function collectPayLoanData() {
    var payingAmount = $("#payloanAmount").val();

    if ($('#radiopayFull').is(':checked')) {
        payingAmount =
        userLoanMyViewModel.userLoanMyDetailsDTO()[paymentIndex].LeftAmount;
    }

    userLoanMyViewModel.userLoanMyDetailsDTO()[paymentIndex].PayingAmount = payingAmount;
}