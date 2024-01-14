var mymanageprofileViewModel;
function initializeManageProfile() {
    mymanageprofileViewModel = {
        personpeopleInfo: peopleInfo,
        nameManageList: ko.observableArray(),
        profileUploadManageList: ko.observableArray(),
        uploadprofilePicObj: new uploadImageObj('profile')
    }
    setupmanageProfile();
}
function setupmanageProfile() {
    $("#managepicfileupload").change(function (evt) {
        if (this.disabled) return alert('File upload not supported!');
        handleFileSelect(evt,
            $("#profilepicprw"), mymanageprofileViewModel.uploadprofilePicObj, peopleInfo.Picture());
    });
    $("#managenamesave").on('click', function () {
        var btn = $(this);
        updateProfileChnages(btn);
    });
    $("#manageprofilepicupload").on('click', function () {
        var vm = mymanageprofileViewModel.uploadprofilePicObj;
        if (vm.validImage == false || vm.buffer == "") {
            return;
        }
        $(this).button('loading');
        uploadImage(vm,
            $("#profilepicprw"), updateProfilePic);
    });
    defineValidationRulesManageProfile($('#manageprofileform'));
    ko.applyBindings(mymanageprofileViewModel, $("#manageprofile").get(0));

}
function defineValidationRulesManageProfile(elem) {
    elem.validate({
        ignore: "",
        onkeyup: false,
        rules: {
            firstName: {
                required: true,
                maxlength: 45
            },
            lastName: {
                required: true,
                maxlength: 45
            },
        },
        messages: {
            firstName: {
                required: "Enter a First Name",
                maxlength: jQuery.format("Enter a max of {0} characters"),
            },
            lastName: {
                required: "Enter a Last Name",
                maxlength: jQuery.format("Enter a max of {0} characters")
            }
        },
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            }
            else {
                error.insertAfter(element);
            }
        }
    });
    console.log("applying validation");

}
function updateProfileChnages(btn) {
    if ($('#manageprofileform').valid()) {
        var profiledata = {
            NameFirst: mymanageprofileViewModel.personpeopleInfo.NameFirst(),
            NameLast: mymanageprofileViewModel.personpeopleInfo.NameLast()
        }
        btn.button('loading');
        $.ajax({
            url: "/api/WebUserService/SaveWebUserInfo",
            type: "post",
            contentType: "application/json",
            data: JSON.stringify(profiledata),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    userNotificationPollOnce();
                    mymanageprofileViewModel.nameManageList.push(1);
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
function updateProfilePic() {
    var btn = $("#manageprofilepicupload");
    var profiledata = {
        Picture: mymanageprofileViewModel.uploadprofilePicObj.imageName,
    }
    if (mymanageprofileViewModel.uploadprofilePicObj.imageName != peopleInfo.Picture()) {
        $.ajax({
            url: "/api/WebUserService/SaveWebUserPic",
            type: "post",
            contentType: "application/json",
            data: JSON.stringify(profiledata),
            dataType: "json",
            headers: {
                RequestVerificationToken: Indexreqtoken
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    userNotificationPollOnce();
                    mymanageprofileViewModel.profileUploadManageList.push(1);
                    peopleInfo.Picture(profiledata.Picture);
                }
            }
        }).always(function () {
            btn.button('reset')
        });
    }
    else {
        console.log("setting defaultprofilepic")
        mymanageprofileViewModel.profileUploadManageList.push(1);
        btn.button('reset');
    }
}

