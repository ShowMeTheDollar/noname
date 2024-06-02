//$(document).ready(function () {
//    $('.nav-link').click(function (e) {
//        e.preventDefault(); // 防止頁面跳轉

//        var targetId = $(this).data('target'); // 獲取被點擊的 Tab 對應的內容區域 ID
//        $('.tab-pane').hide(); // 隱藏所有內容區域
//        $(targetId).show(); // 只顯示對應的內容區域

//        $('.nav-link').removeClass('active'); // 移除所有 Tab 的 active 狀態
//        $(this).addClass('active'); // 為被點擊的 Tab 添加 active 狀態
//    });
//});


$(document).ready(function () {
    // 綁定點擊事件到 tab 上
    $('.nav-tabs .nav-item .nav-link').click(function (e) {
        e.preventDefault(); // 阻止連結的預設行為

        // 移除所有 tab 的 active 狀態並隱藏所有 tab-pane
        $('.nav-tabs .nav-item .nav-link').removeClass('active');
        $('.tab-pane').hide();

        // 添加 active 狀態到當前點擊的 tab 並顯示相對應的 tab-pane
        $(this).addClass('active');
        var currentTab = $(this).attr('aria-posinset');
        $('.tab-pane').eq(currentTab - 1).show(); // 顯示對應的 tab-pane，基於 0 索引
    });

    // 初始化時只顯示第一個 tab-pane
    $('.nav-tabs .nav-item .nav-link').first().click();
});