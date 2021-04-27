var activityTimeout = setTimeout(inActive, 10000);

function resetActive() {
    $(document.body).attr('class', 'active');
    clearTimeout(activityTimeout);
    activityTimeout = setTimeout(inActive, 30000);
}

// No activity do something.
function inActive() {
    window.location.replace("/Home/AdsVideo");
}

// Check for mousemove, could add other events here such as checking for key presses ect.
$(document).bind('mousemove', function () { resetActive() });