var Page = (function () {


    init = function () {
        initEvents();
    },
    initEvents = function () {
        console.log("initializeed");
        var config = {
            $carousel: $('#myCarousel'),
            $suitcase: $('#nav-suitcase'),
            $heart: $('#nav-heart'),
            $backbutton: $('.backbtn')

        };

        config.$suitcase.on("click", function () {
            console.log("Clicked on suitcase");
            var myUrl = $("#MyFriends").val();
            $.post(myUrl, function (data) {
                $('#detailsDiv').replaceWith(data);
            });
            config.$carousel.carousel(1);
        });

        config.$backbutton.on("click", function () {

            config.$carousel.carousel(0);
        });

        config.$heart.on("click touchstart", function () {
            console.log("Clicked on heart");
            config.$carousel.carousel(2);
        });

    };

    return { init: init };

})();