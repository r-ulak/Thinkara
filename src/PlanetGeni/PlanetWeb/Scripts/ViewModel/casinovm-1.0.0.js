var casinoViewModel;

function initializeCasino() {
    casinoViewModel = {
        Amount: ko.observable(),
        AmountAvailable: userBankAccountViewModel.Cash,
        contentLoadTriggered: false,
        machine1: null,
        machine2: null,
        machine3: null,
        slot3List: ko.mapping.fromJS(cacheSlotMachine3List[0]),
        betAmount: ko.observable(1),
        rouletNumber: ko.observable(1),
        rouletBet: ko.observable(1),
        rouletPower: ko.observable(1),
    }
    casinoViewModel.ShowValidation = ko.computed(function () {
        if (casinoViewModel.AmountAvailable() < 0) {
            return true;
        }
        else {
            return false;
        }
    });
    initializecasinoSlotNavigation();
    setupSlot();
    setupRoulet();
}
function initializecasinoSlotNavigation() {
    $('#casino-container a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var self = $(e.target);
        if (self.attr('href') == "#casino-slot") {
        }
        else if (self.attr('href') == "#casino-spin") {
            initializeRoulet();
        }
    })
}


function setupSlot() {
    setupbetInput();
    $('#spinSlot').click(function () {
        if (userBankAccountViewModel.Cash() >= casinoViewModel.betAmount()) {
            $(this).addClass("hidden");
            getSpinNumbers();
            userBankAccountViewModel.Cash(userBankAccountViewModel.Cash() - casinoViewModel.betAmount());
        }

    });
    ko.applyBindings(casinoViewModel, $(".machineContainer")[0]);
    ko.applyBindings(casinoViewModel, $("#casinoSlotcontainer .panel-footer")[0]);
    casinoViewModel.machine1 = $("#machine1").slotMachine({
        active: 0,
        delay: 500
    });
    casinoViewModel.machine2 = $("#machine2").slotMachine({
        active: 1,
        delay: 500
    });
    casinoViewModel.machine3 = $("#machine3").slotMachine({
        active: 2,
        delay: 500
    });
}
function getSpinNumbers() {
    if (casinoViewModel.contentLoadTriggered) {
        return;
    }
    casinoViewModel.contentLoadTriggered = true;

    $.ajax({
        url: "/api/casinoservice/getspinslotnumber",
        type: "get",
        contentType: "application/json",
        data: { betAmount: $("#slotbet").val() },
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            if (result) {

                casinoViewModel.machine1.setRandomize(function () {
                    return result.Number1;
                });
                casinoViewModel.machine2.setRandomize(function () {
                    return result.Number2;
                });
                casinoViewModel.machine3.setRandomize(function () {
                    return result.Number3;
                });

                if (result.Number1 == result.Number2 &&
                            result.Number1 == result.Number3) {
                    setTimeout(function () {
                        userNotificationPollOnce();
                    }, 2000);
                    setTimeout(function () {
                        getUserbankAccount(casinoViewModel);
                    }, 6000);
                }
                else if (result.Number1 == result.Number2 ||
                        result.Number2 == result.Number3 ||
                        result.Number1 == result.Number3) {
                    setTimeout(function () {
                        userNotificationPollOnce();
                    }, 2000);
                    setTimeout(function () {
                        getUserbankAccount(casinoViewModel);
                    }, 6000);
                }


                casinoViewModel.machine1.shuffle(7, onSpinComplete);
                setTimeout(function () {
                    casinoViewModel.machine2.shuffle(7, onSpinComplete);
                }, 500);

                setTimeout(function () {
                    casinoViewModel.machine3.shuffle(7, onSpinComplete);
                }, 1000);


            }
        }
    }).always(function () {
        casinoViewModel.contentLoadTriggered = false;

    });
}
function onSpinComplete(active) {
    switch (this.element[0].id) {
        case 'machine1':
            console.log('machine1: ' + this.active);
            break;
        case 'machine2':
            console.log('machine2: ' + this.active);
            break;
        case 'machine3':
            console.log('machine3: ' + this.active);
            $('#spinSlot').removeClass("hidden");
            break;
    }
}
function setupbetInput() {
    var self = $("#slotbet");
    self.TouchSpin({
        prefix: "Your Bet",
        initval: 0,
        min: 0,
        max: casinoViewModel.AmountAvailable(),
        step: 1,
        decimals: 2,
        booster: false,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        postfix: '<i class="fa icon-money28  fa-1x"></i>',
        mousewheel: true
    });
}


function setupRoulet() {
    $('#spinRoulet').click(function () {
        if (userBankAccountViewModel.Cash() >= casinoViewModel.rouletBet()) {
            getRouletNumber();
            userBankAccountViewModel.Cash(userBankAccountViewModel.Cash() - casinoViewModel.rouletBet());
        }
    });
    $('#playRoulet').click(function () {
        playAgain();
    });
    setuprouletInput();
    ko.applyBindings(casinoViewModel, $("#spinRoultcontainer .panel-body")[0]);
    ko.applyBindings(casinoViewModel, $("#spinRoultcontainer .panel-footer")[0]);

}
function getRouletNumber() {
    if (casinoViewModel.contentLoadTriggered || $("#rouletbet").val() < 1) {
        return;
    }
    casinoViewModel.contentLoadTriggered = true;

    $.ajax({
        url: "/api/casinoservice/getrouletnumber",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            SelectedNumber: $("#rouletnum").val(),
            BetAmount: $("#rouletbet").val()
        }),
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            console.log(result);
            startSpin(result, $("#rouletpower").val());
            $('#spinRoulet').addClass("hidden");
            $('#rouletnum').prop('disabled', true);
            $('#rouletbet').prop('disabled', true);
            $('#rouletpower').prop('disabled', true);
        }
    }).always(function () {
        casinoViewModel.contentLoadTriggered = false;
    });
}
function setuprouletInput() {
    var self = $("#rouletnum");
    console.log(self);
    self.TouchSpin({
        prefix: "Your Pick",
        initval: 0,
        min: 0,
        max: 7,
        step: 1,
        decimals: 0,
        booster: false,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        postfix: '<i class="fa icon-slot-machine  fa-1x"></i>',
        mousewheel: true
    });
    self = $("#rouletpower");
    self.TouchSpin({
        prefix: "Power",
        initval: 1,
        min: 1,
        max: 3,
        step: 1,
        decimals: 0,
        booster: false,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        postfix: '<i class="fa icon-slot-machine  fa-1x"></i>',
        mousewheel: true
    });
    self = $("#rouletbet");
    self.TouchSpin({
        prefix: "Your Bet",
        initval: 0,
        min: 0,
        max: casinoViewModel.AmountAvailable(),
        step: 1,
        decimals: 2,
        booster: false,
        stepinterval: 1,
        postfix_extraclass: "btn btn-default",
        postfix: '<i class="fa icon-money28  fa-1x"></i>',
        mousewheel: true
    });
}
function rouleteComlete(winprize) {
    console.log("You won " + winprize + "!\nClick 'Play Again' to have another go.");
    $('#playRoulet').removeClass("hidden");
    userNotificationPollOnce();
    setTimeout(function () {
        getUserbankAccount(casinoViewModel);
    }, 2000);

}
function playAgain() {
    resetWheel();
    $('#spinRoulet').removeClass("hidden");
    $('#playRoulet').addClass("hidden");
    $('#rouletnum').prop('disabled', false);
    $('#rouletbet').prop('disabled', false);
    $('#rouletpower').prop('disabled', false);

}