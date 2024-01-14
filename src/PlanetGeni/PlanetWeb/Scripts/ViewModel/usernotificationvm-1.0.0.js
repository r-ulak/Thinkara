var pollFequency = 4000;
var longPollFrequency = 600000;
var pollFactor = 1;
var userNotificationViewModel;
var notificationCarousel = 17;
function initializeuserNotification(callsource) {
    userNotificationViewModel = {
        userNotificationDTO: ko.observableArray(),
        lastNotificationId: ko.observable(''),
        recentNotificationId: ko.observable(''),
        lastUpdatedAt: ko.observable(''),
        recentUpdatedAt: ko.observable(''),
        contentLoadTriggered: false
    };
    getDataOldUserNotification(callsource);
}

function applyUserNotificationBinding() {
    ko.applyBindings(userNotificationViewModel, $("#myuserNotificationcontent-box .panel-body")[0]);
    ko.applyBindings(userNotificationViewModel, $("#myuserNotificationcontent-box .panel-footer")[0]);
    ko.applyBindings(peopleInfo, $("#myuserNotificationcontent-box .panel-heading")[0]);
    setupNotificationView();
}
function applyUserNotificationNavBar() {
    ko.applyBindings(userNotificationViewModel, $("#notifbadge")[0]);
}

function setupNotificationView() {
    $("#myuserNotificationcontent-box .panel-body").perfectScrollbar(
{
    suppressScrollX: true
});

    $("#notificationshowmore").on("click", function () {
        getDataOldUserNotification('');
    });

    $("#notifcationdeleteall").on("click", function () {
        clearAllNotification();
    });
    $("#notificationsrefresh").on("click", function () {
        initializeuserNotification("initial");
        $('#carousel' + notificationCarousel).html(userNotificationView);
        applyUserNotificationBinding();
        ko.cleanNode($("#notifbadge")[0]);
        applyUserNotificationNavBar();
    });
}
function getDataNewUserNotification(callsource) {
    if (userNotificationViewModel.contentLoadTriggered == true) {
        return;
    }
    userNotificationViewModel.contentLoadTriggered = true;

    $.ajax({
        url: "/api/usernotificationservice/getnewnotificationlist",
        type: "get",
        contentType: "application/json",
        data: {
            recentNotificationId: userNotificationViewModel.recentNotificationId(),
            recentUpdatedAt: userNotificationViewModel.recentUpdatedAt()
        },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    }).done(function (data) {
        if (data.length > 0) {
            userNotificationViewModel.recentUpdatedAt(data[0].UpdatedAt);
            userNotificationViewModel.recentNotificationId(data[0].NotificationId);

            for (i = 0; i < data.length; i++) {
                processNotificationData(data[i]);
                userNotificationViewModel.userNotificationDTO.unshift(data[i]);
                if (callsource == 'poll' || callsource == 'once') {
                    notifcationGrowl(data[i].Description,
                        data[i].ShortDescription,
                        data[i].ImageFont, data[i].Url, data[i].Priority);
                }
            }
            if (callsource == 'once') {
                pollFactor = 1;
            }
        }
        else {
            if (callsource == 'once' && pollFactor < 3) {
                pollFactor++;
                userNotificationPollOnce();
            }
            else {
                pollFactor = 1; //Waited too much(3 factor ). no data comming back
            }
        }
        userNotificationViewModel.contentLoadTriggered = false;
    }).always(function () {
        if (callsource == 'poll') {
            userNotificationPoll();
        }
    });
}

function getDataOldUserNotification(callsource) {
    if (userNotificationViewModel.contentLoadTriggered == true) {
        return;
    }
    userNotificationViewModel.contentLoadTriggered = true;
    $.ajax({
        url: "/api/usernotificationservice/getoldnotificationlist",
        type: "get",
        contentType: "application/json",
        data: {
            lastNotificationId: userNotificationViewModel.lastNotificationId(),
            lastUpdatedAt: userNotificationViewModel.lastUpdatedAt()
        },
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    }).done(function (data) {
        if (data.length > 0) {
            userNotificationViewModel.lastNotificationId(data[data.length - 1].NotificationId);
            userNotificationViewModel.lastUpdatedAt(data[data.length - 1].UpdatedAt);
            if (callsource == 'initial') {
                userNotificationViewModel.recentUpdatedAt(data[0].UpdatedAt);
                userNotificationViewModel.recentNotificationId(data[0].NotificationId);
            }

            for (i = 0; i < data.length; i++) {
                processNotificationData(data[i]);
                userNotificationViewModel.userNotificationDTO.push(data[i]);
            }
        }
        userNotificationViewModel.contentLoadTriggered = false;
        userNotificationPoll();
    });
}
function processNotificationData(item) {
    item.Parms = processDateParm(item.Parms);
    processOnClickParm(item);
    item.DisplayDate = ParseDate(item.UpdatedAt);
    item.Description =
        item.Description.format.apply(item.Description,
        item.Parms.split("|"));

    item.Description = replaceAll(item.Description, ":::p-pic", profilepicContainer);
}
function userNotificationPoll() {
    setTimeout(function () {
        getDataNewUserNotification('poll');
    }, longPollFrequency);
}

function userNotificationPollOnce() {
    setTimeout(function () {
        getDataNewUserNotification('once');
    }, pollFequency * pollFactor);
}

function clearAllNotification() {
    $.ajax({
        url: "/api/usernotificationservice/clearallnotification",
        type: "post",
        contentType: "application/json",
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            userNotificationViewModel.userNotificationDTO.removeAll();
        }
    });
}
function markNotificationRead(notificationId) {
    $.ajax({
        url: "/api/usernotificationservice/markreadnotification",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(notificationId),
        dataType: "json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        },
        success: function (result) {
            var match = ko.utils.arrayFirst(userNotificationViewModel.userNotificationDTO(), function (item) {
                return notificationId === item.NotificationId;
            });
            if (match) {
                userNotificationViewModel.userNotificationDTO.remove(match);
            }
        }
    });
}

function notifcationGrowl(msg, title, imagefont, notifcationurl, priority) {
    var growlType = "notification";
    if (priority > 5) {
        growlType = "needaction";
    }
    $.growl({
        icon: imagefont,
        title: " <strong>" + title + ": </strong>",
        message: msg,
        url: notifcationurl
    }, {
        type: growlType,
        offset: 55,
        delay: 8000,
        animate: {
            enter: 'animated fadeInDown',
            exit: 'animated fadeOutUp'
        }
    });

}

function GoToTask() {
    usertaskViewClick("/UserTask/GetUserTasks", taskCarosuelId, function () {
        $('#myCarousel').carousel(taskCarosuelId);
    });
}