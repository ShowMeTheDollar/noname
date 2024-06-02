//document.addEventListener('DOMContentLoaded', function () {
//    var redirectBtn = document.querySelectorAll('.next');
//    redirectBtn.forEach(function (button) {
//        button.addEventListener('click', function () {
//            // Use the global variable for redirection
//            window.location.href = nextUrl;
//        });
//    });
//});

//$(document).ready(function () {
//    $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
//        var currentTab = $(e.target).attr('href'); // 拿當前點及的標籤頁 href 的属性值
//        $('.tab-pane').hide(); // 隐藏所有的 tab-pane
//        $(currentTab).show(); // 顯示目前選的内容
//    });
//    //first tab active
//    $('.nav-tabs a:first').tab('show');
//});

document.addEventListener('DOMContentLoaded', function () {
    var redirectBtn = document.querySelectorAll('.next');
    redirectBtn.forEach(function (button) {
        button.addEventListener('click', function () {
            // Use the global variable for redirection
            window.location.href = nextUrl;
        });
    });
});

