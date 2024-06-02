// 顯示房間 ----------------------------------------------------------------------------------
document.addEventListener('DOMContentLoaded', function () {
    const closeBtn = document.querySelector('#my-close-btn');
    closeBtn.addEventListener('click', GoWishCards);
});

function GoWishCards() {

    const wishlistId = document.querySelector('#wishId').innerText;
    const myCount = document.querySelector('#myCount').innerText;
    const origin = window.location.origin;
    const url = new URL(`${origin}/Wish/WishlistDetail/${wishlistId}`); // 將 wishlistId 加到 URL 的查詢參數中，傳遞到下一個頁面，以便在該頁面中使用。

    if (wishlistId) {
        url.searchParams.set('wishlistId', wishlistId);
    }

    if (myCount) {
        url.searchParams.set('myCount', myCount);
    }

    window.location.href = url.toString(); // 將物件變成字串
}
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
        const itemContainers = document.querySelectorAll('.item-container');

        itemContainers.forEach(container => {
            const roomId = container.querySelector('.room-link').id;
            const infoDescription = container.querySelector('.info-description');
            const lat = parseFloat(infoDescription.getAttribute('data-lat'));
            const lng = parseFloat(infoDescription.getAttribute('data-lng'));
            const roomName = container.querySelector('.info-name').textContent;
            const price = container.querySelector('.info-price').textContent;
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
            }
        });
        map.addLayer(markers);
    }
};
// 複製分享網址 --------------------------------------------------------------------------------
document.querySelector('.send-link-btn').addEventListener('click', function () {
    var currentUrl = window.location.href;
    navigator.clipboard.writeText(currentUrl).then(function () {
        Swal.fire({
            icon: "success",
            text: "頁面網址已成功複製！"
        });
    }, function (err) {
        Swal.fire({
            icon: "error",
            text: "無法複製頁面網址！ " + error
        });
    });
});


// 重新命名 -------------------------------------------------------------------------------------
document.addEventListener('DOMContentLoaded', function () {
    var inputElement = document.querySelector('.to-rename-sub-input');
    var charCountElement = document.getElementById('charCount');
    var wishlistId = document.getElementById('wishId').textContent;

    inputElement.addEventListener('input', function () {
        var currentLength = inputElement.value.length;
        charCountElement.textContent = currentLength;

        if (currentLength >= 20) {
            inputElement.value = inputElement.value.slice(0, 19); // 防止超過20個字
            charCountElement.style.color = 'red'; // 顯示已達到最大字數
        } else {
            charCountElement.style.color = ''; // 顯示剩餘字數
        }
    });

    document.querySelector('.save-btn').addEventListener('click', function (event) {
        event.preventDefault(); // 阻止表單提交後重新整理頁面

        var newName = inputElement.value.trim();

        // 防呆
        if (newName.length < 1) {
            Swal.fire({
                icon: "warning",
                text: "內容不得為空白！",
            });
            return;
        }

        fetch('/Wish/UpdateWishlistNameApi', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ wishlistId: wishlistId, wishlistName: newName }) 
        })
            .then(function (response) {
                console.log('Response status:', response.status);
                return response.json();
            })
            .then(function (data) {
                console.log('Response data:', data);
                if (data.success) {
                    Swal.fire({
                        icon: "success",
                        title: "心願名單已更新！",
                        showConfirmButton: true,
                        timer: 6500
                    }).then((result) => {
                        if (result.isConfirmed) { // SweetAlert 內建屬性
                            window.location.reload();
                        }
                    });

                    inputElement.value = '';
                    $('#to-rename-sub').modal('hide');
                    $('.modal-backdrop').remove();
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: data.message || "更新心願名單時出錯！"
                    });
                }
            })
            .catch(function (error) {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: "更新名單時出錯！ " + error
                });
            });
    });
});
// 刪除收藏房間 ------------------------------------------------------------------------------------
document.querySelectorAll('.like-btn').forEach(btn => {
    btn.addEventListener('click', function (event) {
        event.preventDefault();
        const id = this.getAttribute('data-wishlist-detail-id');
        console.log(id);

        fetch(`/Wish/DeleteRoomFromWishDetail?Id=${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('回應時發生錯誤');
                }
                return response.json();
            })
            .then(data => {
                // 删除成功
                if (data.success) {
                    Swal.fire({
                        icon: "success",
                        title: "刪除成功！",
                        showConfirmButton: true
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload();
                        }
                    });
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "刪除失敗！ " + data.message
                    });
                }
            })
            .catch(error => {
                Swal.fire({
                    icon: "warning",
                    text: "操作出現問題！ " + data.message
                });
            });
    });
    // ----------------------------------
});



//Swiper
document.addEventListener('DOMContentLoaded', function () {
    function adjustSwiperSlideHeight() {
        const swiperContainer = document.querySelector('.swiper-container');
        if (!swiperContainer) return;

        const slides = swiperContainer.querySelectorAll('.swiper-slide');
        slides.forEach(function (slide) {
            const width = slide.offsetWidth;   // 取得寬度
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
