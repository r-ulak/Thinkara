$(function () {
    var hideDelay = 500;
    var currentID;
    var hideTimer = null;

    // One instance that's reused to show info for the current person
    var container = $('#personPopupContainer');
    $('body').append(container);

    $('.personPopupTrigger').live('click', function () {

        if (hideTimer)
            clearTimeout(hideTimer);

        var pos = $(this).offset();
        var width = $(this).width();
        container.css({
            left: (pos.left + width) + 'px',
            top: pos.top - 5 + 'px'
        });

        //container.css('display', 'block');
        container.slideDown('fast');
    });

    $('.personPopupTrigger').live('mouseout', function () {
        if (hideTimer)
            clearTimeout(hideTimer);
        hideTimer = setTimeout(function () {
            container.slideUp('fast', function () { container.css('display', 'none'); });
        }, hideDelay);
    });

    // Allow mouse over of details without hiding details
    $('#personPopupContainer').mouseover(function () {
        if (hideTimer)
            clearTimeout(hideTimer);
    });

    // Hide after mouseout
    $('#personPopupContainer').mouseout(function () {
        if (hideTimer)
            clearTimeout(hideTimer);
        hideTimer = setTimeout(function () {
            container.slideUp('fast', function () { container.css('display', 'none'); });
        }, hideDelay);
    });
});