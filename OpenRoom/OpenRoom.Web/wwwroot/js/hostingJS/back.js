//document.getElementById('redirectToHostingSourceBtn').addEventListener('click', function () {
//    window.location.href = hostingSourceUrl; // Use the variable defined in the Razor view
//});

document.addEventListener('DOMContentLoaded', function () {
    var redirectBtn = document.querySelectorAll('.back');
    redirectBtn.forEach(function (button) {
        button.addEventListener('click', function () {
            // Use the global variable for redirection
            window.location.href = backUrl;
        });
    });
});