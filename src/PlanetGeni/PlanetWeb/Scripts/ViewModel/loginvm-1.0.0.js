var countryCodeViewModel;
var countryByaddress = "";
var countrycodedd ;

function getCountryCodes() {
    return $.ajax({
        url: "/api/countrycodeservice/getcountrycodes",
        type: "get",
        contentType: "application/json",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getHostAddressInfo() {
    return $.ajax({
        url: "/api/CountryCodeService/GetHostLocation",
        type: "get",
        headers: {
            RequestVerificationToken: Indexreqtoken
        }
    });
}
function getCountryByaddress() {
    getHostAddressInfo().done(function (result) {
        $.ajax({
            url: "https://geoip.nekudo.com/api/" + result,
            type: "get",
            contentType: "application/json",
            dataType: 'jsonp',   //you may use jsonp for cross origin request
            crossDomain: true,
            success: function (data) {
                countryByaddress = data.country.code;
                setdefaultCountyId(countrycodedd);

            }
        });
    });
}
function loginInitizalize() {
    countryCodeViewModel = {
        countries: ko.observableArray(),
        selectedCountryId: ko.observable()
    };
    getCountryByaddress();
    getCountryCodes().done(function (result) {
        for (i = 0; i < result.length; i++) {
            countryCodeViewModel.countries.push(result[i]);
        }
        setupcountrycodedd();
    });
}
function setupcountrycodedd() {
    ko.applyBindings(countryCodeViewModel, $("#ddCountrycdregister").get(0));
    countrycodedd = new DropDown($('#ddCountrycdregister'));

    countrycodedd.opts.on('click', function () {
        $("#countrycode").val(
        ($(this).data("id")));
        countryCodeViewModel.selectedCountryId($(this).data("id"));
    });


    $("#ddCountrycdregister ul").perfectScrollbar({
        suppressScrollX: true,
        wheelPropagation: true,
        wheelSpeed: 70,
        minScrollbarLength: 50

    });
    setdefaultCountyId(countrycodedd);

}
function setdefaultCountyId(countrycodedd) {
    if (countryByaddress == '' || countryCodeViewModel.countries().length == 0) {
        return;
    }
    console.log(countryByaddress);
    var countryItem = ko.utils.arrayFirst(countryCodeViewModel.countries(),
        function (item) {
            return item.CountryId.toLowerCase() === countryByaddress.toLowerCase();
        });
    var index = countryCodeViewModel.countries().indexOf(countryItem);
    countrycodedd.setValue(countryItem.Code, index, countryItem.CountryId.toLowerCase());
    $("#countrycode").val(countryItem.CountryId.toLowerCase());
    countryCodeViewModel.selectedCountryId(countryItem.CountryId.toLowerCase());
}
$(document).ready(function () {
    loginInitizalize();
});

$(document).click(function () {
    // all dropdowns
    $('.wrapper-dropdown-3').removeClass('active');
});