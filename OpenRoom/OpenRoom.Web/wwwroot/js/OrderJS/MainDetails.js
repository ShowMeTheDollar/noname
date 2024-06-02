// Leaflet 地圖 ---------------------------------------------------------------------------------------
window.onload = function () {
    var map = L.map('map').setView([23.97565, 120.9738819], 8);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 20,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    var markers = L.markerClusterGroup({
        maxClusterRadius: 10,
        singleMarkerMode: true,
        chunkedLoading: true 
    });

    setMarker();

    function setMarker() {
        const itemContainers = document.querySelectorAll('.order-info-container');

        itemContainers.forEach(container => {
            const roomId = container.querySelector('.room-link').id;
            const infoDescription = container.querySelector('.check-in-p');
            const lat = parseFloat(infoDescription.getAttribute('data-lat'));
            const lng = parseFloat(infoDescription.getAttribute('data-lng'));
            const roomName = container.querySelector('.main-wish-title').textContent;
            const price = container.querySelector('.total-cost-content').textContent;
            // 房間名稱超過8個則將顯示 ...
            if (!isNaN(lat) && !isNaN(lng)) {
                let displayName;
                if (roomName.length > 8) {
                    displayName = roomName.substring(0, 10) + '...';
                } else {
                    displayName = roomName;
                }

                let popup = L.popup()
                    .setLatLng([lat, lng])
                    .setContent(`<a href="/Rooms/${roomId}" class="product-link">${displayName}<br/>${price}</a>`)
                    .openOn(map);

                markers.addLayer(popup);

                console.log("Latitude:", lat);
                console.log("Longitude:", lng);
                console.log("Room ID:", roomId);
                console.log("Room Name:", roomName);
                console.log("Price:", price);
            }
        });
        map.addLayer(markers);
    }
};


//Swiper ----------------------------------------------------
document.addEventListener('DOMContentLoaded', function () {
    function adjustSwiperSlideHeight() {
        const swiperContainer = document.querySelector('.swiper-container');
        if (!swiperContainer) return;

        const slides = swiperContainer.querySelectorAll('.swiper-slide');
        slides.forEach(function (slide) {
            const width = slide.offsetWidth;    // 取得寬度
            slide.style.height = width + 'px'; // 設置高度等於寬度，保持正方形
        });
    }

    // 調用函數來初次調整高度
    adjustSwiperSlideHeight();

    // 監聽窗口大小改變事件來重新調整高度
    window.addEventListener('resize', adjustSwiperSlideHeight);

    // 初始化 Swiper
    const swiper = new Swiper('.swiper', {
        pagination: {
            el: '.swiper-pagination',
        },
    });
});



// 複製地址 ---------------------------------------------------
document.querySelector('.copy-address-btn').addEventListener('click', function () {
    var addressContent = document.querySelector('.address-content').innerText;
    navigator.clipboard.writeText(addressContent).then(function () {
        Swal.fire({
            icon: "success",
            title: "地址已成功複製！",
            showConfirmButton: true
        });
    }, function (err) {
        console.error('無法複製地址: ', err);
    });
});
// 複製網址 ---------------------------------------------------
document.querySelector('.share-btn').addEventListener('click', function () {
    var currentUrl = window.location.href;
    navigator.clipboard.writeText(currentUrl).then(function () {
        Swal.fire({
            icon: "success",
            title: "頁面網址已成功複製！",
            showConfirmButton: true
        });
    }, function (err) {
        // 失敗處理
        console.error('無法複製頁面網址: ', err);
    });
});
// 取得收據頁面 ----------------------------------------------
document.querySelector('.receipt-btn').addEventListener('click', GoReceipt);
function GoReceipt() {
    const receiptId = document.querySelector('#receiptId').innerText;
    const origin = window.location.origin;
    const url = new URL(`${origin}/Order/Receipt/${receiptId}`);
    if (receiptId) {
        url.searchParams.set('receiptId', receiptId);
    }
     window.location.href = url.toString();  // 從當前頁面跳轉過去
    //window.open(url.toString(), '_blank');     // 開新頁面
}
