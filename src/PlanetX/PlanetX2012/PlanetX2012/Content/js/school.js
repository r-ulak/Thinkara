function universityLookup(inputString) {
    if (inputString.length == 0) {
        $('#suggestions').fadeOut(); // Hide the suggestions box
    }
    else if (inputString.length <= 2) {
        $('#suggestions').fadeOut(); // Hide the suggestions box
    }
    else {
        $.getJSON('../Education/GetUniversityList?', { term: inputString }, function (data) {
            $('#suggestions').html('');
            if ($('#university').attr("data-univeristyid") != "-1") {
                resetTextBox();
            }
            var anchorTagElements = '';
            $.each(data, function (i, item) {
                anchorTagElements = anchorTagElements + '<a onclick="javascript:univeristySelected(' + item.UniversityId + ','
                    + "'" + item.University + "'" + ')" ><img alt="" src="/images/university.png"/> <span class="searchheading">' +
                    item.University + '</span><span>This is short description</span></a>';
            });
            if (anchorTagElements.length > 1) {
                var searchresultTag = '<p id ="searchresults"><span class="category">University Suggestions</span>'
                    + anchorTagElements + '</p>';
                $('#suggestions').html(searchresultTag);
                $('#suggestions').fadeIn(); // Show the suggestions box

            } else {
                $('#suggestions').fadeOut(); // Hide the suggestions box }
            }
        });
    }
}
function resetTextBox() {
    $('#university').css("padding-left", "0px");
    $('#university').css("background-image", "");
    $('#university').removeClass("textImage");
    $('#university').attr("data-univeristyid", "-1");
    $('#UniversityId').val("");
}
function univeristySelected(univeristyid, university) {

    var imgurl = '/images/university.png';
    $('#suggestions').fadeOut();
    $('#university').css("background-image", "url(" + imgurl + ")");
    $('#university').css("padding-left", "35px");
    $('#university').addClass("textImage");
    $('#university').val(university);
    $('#university').attr("data-univeristyid", univeristyid);
    $('#UniversityId').val(univeristyid);
}
function joinedSchool(joinSchoolData, status, xhr) {
    console.log("OnSucess called");    
    if (joinSchoolData.success == "1")
        $("#education-modal-dialog").dialog("close");
    else {
        $("#errSummaryJoinSchool").html(joinSchoolData.ex);
        $("#errSummaryJoinSchool").show();
    }


}
$.getJSON('../Education/GetDegreeTypeList', function (data) {
    $('#degreecodes').ddslick({
        data: data,
        selectText: "Select Degree",
        onSelected: function (degreeTypedata) {
            //callback function: show Subtypes            


            $("#DegreeType").val(degreeTypedata.selectedData.value);




        }
    });

});
$.getJSON('../Education/GetMajorTypeList', function (data) {
    $('#majorcodes').ddslick({
        data: data,
        height: 175,
        selectText: "Select Major",
        onSelected: function (majorTypedata) {


            $("#MajorType").val(majorTypedata.selectedData.value);
        }
    });

});


//$(document).ready(function () {

//    var watermark = 'School Name';

//    //init, set watermark text and class
//    $('#university').val(watermark).addClass('watermark');

//    //if blur and no value inside, set watermark text and class again.
//    $('#university').blur(function () {
//        if ($(this).val().length == 0) {
//            $(this).val(watermark).addClass('watermark');
//        }
//    });

//    //if focus and text is watermrk, set it to empty and remove the watermark class
//    $('#university').focus(function () {
//        if ($(this).val() == watermark) {
//            $(this).val('').removeClass('watermark');
//        }
//    });
//});