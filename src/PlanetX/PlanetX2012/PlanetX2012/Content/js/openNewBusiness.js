

function GetChart() {
    var iType = "1";
    var Type = "GET";
    var Url = "http://planetx.com/ChartService/ChartService.svc/GetChartUrl/" + iType;
    var ContentType = "application/json; charset=utf-8";
    var DataType = "json";
    var ProcessData = true;

    console.log("CallService");
    $.ajax({
        type: Type, //GET or POST or PUT or DELETE verb
        url: Url, // Location of the service        
        contentType: ContentType, // content type sent to server
        dataType: DataType, //Expected data format from server
        processdata: ProcessData, //True or False
        success: function (msg) {//On Successfull service call
            ServiceSucceeded(msg);
        },
        error: ServiceFailed// When Service call fails
    });
}

function ServiceSucceeded(result) {
    console.log("ServiceSucceeded");
    if (DataType == "json") {

        resultObject = result.GetUserResult;
        alert(resultObject);
        for (i = 0; i < resultObject.length; i++) {
            alert(resultObject[i]);
        }

    }

}

function ServiceFailed(xhr) {
    alert(xhr.responseText + "Failed");
    if (xhr.responseText) {
        var err = xhr.responseText;
        if (err)
            error(err);
        else
            error({ Message: "Unknown server error." })
    }
    return;
}

$.getJSON('../Finance/GetBusinessTypeList', function (data) {

    $('#businessCodes').ddslick({
        data: data,
        height: 200,
        selectText: "Select Business Type",
        onSelected: function (businessTypedata) {
            //callback function: show Subtypes                        
            $.getJSON("http://planetx.com/ChartService/ChartService.svc/GetChartUrl/1?callback=?",
                    function (data) {
                        $("#cityCountryAutoComplete").removeClass("hidden");
                        $("#chartdiv").removeClass("hidden");
                        $("#businessGraph").attr("src", '/images/charts/' + data.GetChartUrlResult);
                    });

            $("#Business_BusinessTypeId").val(businessTypedata.selectedData.value);
            $("#Business_BusinessSubtypeId").val("");
            $.getJSON('../Finance/GetBusinessSubTypeList?businessTypeId=' + businessTypedata.selectedData.value, function (data) {
                $('#businessSubCodes').ddslick('destroy');
                if (data[0].success) {
                    $('#businessSubCodes').ddslick({
                        data: data,
                        selectText: "Select Business SubType",
                        onSelected: function (businessSubTypedata) {
                            $.getJSON("http://planetx.com/ChartService/ChartService.svc/GetChartUrl/1?callback=?",
                                        function (data) {

                                            $("#businessGraph").attr("src", '/images/charts/' + data.GetChartUrlResult);
                                        });

                            $("#Business_BusinessSubtypeId").val(businessSubTypedata.selectedData.value);
                        }
                    });
                }
            });

        }

    });


});



