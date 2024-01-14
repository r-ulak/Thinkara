var achievementBusinessViewModel = {
    BusinessAchievement: ko.observableArray()
};
var achievementEducationViewModel = {
    EducationAchievement: ko.observableArray()
};


var awardSummaryViewModel;

$(document).ready(function () {
    getAwardSummaryViewModel();
    getAchievementViewModel();

});
function getAwardSummaryViewModel() {

    $.ajax({
        url: "/api/awardservice/getawardsummarydto",
        type: "get",
        contentType: "application/json",
        success: function (result) {            
            awardSummaryViewModel = ko.mapping.fromJS(result);            
            ko.applyBindings(awardSummaryViewModel, $("#awardSummary").get(0));
        },
        error: function (result) {
            //handle the error, left for brevity
        }
    });
}

function getAchievementViewModel() {

    $.ajax({
        url: "/api/awardservice/getachievementdto",
        type: "get",
        contentType: "application/json",
        data: { achievementType: "Business" }

    }).done(function (data) {
        if (data.length > 0) {
            for (i = 0; i < data.length; i++) {
                achievementBusinessViewModel.BusinessAchievement.push(data[i]);
            }
            ko.applyBindings(achievementBusinessViewModel, $("#businessAchievement")[0]);
        }
    });


    $.ajax({
        url: "/api/awardservice/getachievementdto",
        type: "get",
        contentType: "application/json",
        data: { achievementType: "Education" }

    }).done(function (data) {
        if (data.length > 0) {
            for (i = 0; i < data.length; i++) {
                achievementEducationViewModel.EducationAchievement.push(data[i]);
            }
            ko.applyBindings(achievementEducationViewModel, $("#educationAchievement")[0]);
        }
    });
}
