var countries = {};
$(document).ready(function () {
    // Safely inject CSS3 and give the search results a shadow
    var cssObj = {
        'box-shadow': '#888 5px 10px 10px', // Added when CSS3 is standard
        '-webkit-box-shadow': '#888 5px 10px 10px', // Safari
        '-moz-box-shadow': '#888 5px 10px 10px'
    }; // Firefox 3.5+
    $("#suggestions").css(cssObj);

    // Fade out the suggestions box when not active
    $("#newBusinessCity").blur(function () {
        $('#suggestions').fadeOut();
        
    });
    $(window).scroll(function () {
        $('#mydiv').css('display', 'block');
        
    });

    $.getJSON('../Location/GetAllCountryList', function (data) {
        $.each(data, function (i, country) {
            countries[country.CountryId] = country.Code;
        });
    });


});


function cityCountryLookup(inputString) {
    if (inputString.length == 0) {
        $('#suggestions').fadeOut(); // Hide the suggestions box
    }
    else if (inputString.length <= 1) {
        $('#suggestions').fadeOut(); // Hide the suggestions box
    }
    else {
        $.getJSON('../Location/GetCityList?', { term: inputString }, function (data) {
            $('#suggestions').html('');
            if ($('#newBusinessCity').attr("data-cityid") != "-1")
            {
                resetTextBox();
            }
            var anchorTagElements = '';
            $.each(data, function (i, item) {
                anchorTagElements = anchorTagElements + '<a onclick="javascript:cityCountrySelcted(' + item.CityId + ','
                    + "'" + item.CountryId + "'" + ',' + "'" + item.City + "'" + ')" ><img alt="" src="/images/flags/flag_' +
                    countries[item.CountryId].replace(/ /g, "_") + '.png"/> <span class="searchheading">' +
                    item.City + ', ' + countries[item.CountryId] + '</span><span>This is short description</span></a>';
            });
            if (anchorTagElements.length > 1) {
                var searchresultTag = '<p id ="searchresults"><span class="category">City, Country Suggestions</span>'
                    + anchorTagElements + '</p>';
                $('#suggestions').html(searchresultTag);
                $('#suggestions').fadeIn(); // Show the suggestions box

            } else {
                $('#suggestions').fadeOut(); // Hide the suggestions box }
            }
        });
    }
}

function cityCountrySelcted(cityid, countryId, city) {
    
    var imgurl = '/images/flags/flag_' + countries[countryId].replace(/ /g, "_") + '.png';
    $('#suggestions').fadeOut();
    $('#newBusinessCity').css("background-image", "url(" + imgurl + ")");
    $('#newBusinessCity').css("padding-left", "35px");
    $('#newBusinessCity').addClass("textImage");
    $('#newBusinessCity').val(city);
    $('#newBusinessCity').attr("data-cityid", cityid);
    $('#Business_CityId').val(cityid);
}

function resetTextBox()
{
    $('#newBusinessCity').css("padding-left", "0px");
    $('#newBusinessCity').css("background-image", "");
    $('#newBusinessCity').removeClass("textImage");
    $('#newBusinessCity').attr("data-cityid", "-1");
    $('#Business_CityId').val("");
}
