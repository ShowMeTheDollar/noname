// 顯示更多 ------------------------------------
window.onload = function () {
    var roomItems = document.querySelectorAll('.room-item');
    for (var i = 8; i < roomItems.length; i++) {
        roomItems[i].style.display = 'none';
    }

    var moreButton = document.querySelector('.more-btn');
    moreButton.addEventListener('click', function () {
        // 按一下顯示8個
        var hiddenItems = document.querySelectorAll('.room-item:not([style*="display: block"])');
        var itemsToShow = Math.min(hiddenItems.length, 8);
        for (var i = 0; i < itemsToShow; i++) {
            hiddenItems[i].style.display = 'block';
        }
        if (hiddenItems.length <= itemsToShow) {
            // 沒東西載入就隱藏按鈕
            moreButton.classList.add('d-none');
        }
    });

};