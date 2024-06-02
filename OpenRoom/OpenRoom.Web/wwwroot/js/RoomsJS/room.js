window.addEventListener("load", function () {
    /*Initialize Swiper*/
    var swiper = new Swiper(".swiper", {
        scrollbar: {
            el: ".swiper-scrollbar",
            hide: true,
        },
    });

    //地圖初始化
    const mapDom = document.getElementById("main-map");
    const lat = parseFloat(mapDom.getAttribute("data-lat"));
    const lng = parseFloat(mapDom.getAttribute("data-lng"));
    const mainMap = L.map("main-map", {
        center: [lat, lng],
        zoom: 15
    });
    let osmForMain = new L.TileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
        minZoom: 8,
        maxZoom: 18,
        attribution: "&copy; <a href='http://www.openstreetmap.org/copyright'>OpenRoom</a>"
    });
    mainMap.addLayer(osmForMain);

    const mainCircle = L.circle([lat, lng], {
        color: "red",
        fillColor: "#f03",
        fillOpacity: 0.5,
        radius: 250
    });
    mainCircle.bindTooltip("<b>預定確認後會提供確切位置。</b>", {
        direction: "top"
    });
    mainMap.addLayer(mainCircle);

    document.getElementById("locationModal").addEventListener("show.bs.modal", function () {
        setTimeout(function () {
            // if (!modalMap) {
            const modalMap = L.map("modal-map", {
                center: [lat, lng],
                zoom: 15
            });
            let osmForModal = new L.TileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
                minZoom: 8,
                maxZoom: 18,
                attribution: "&copy; <a href='http://www.openstreetmap.org/copyright'>OpenRoom</a>"
            });
            modalMap.addLayer(osmForModal);

            const modalCircle = L.circle([lat, lng], {
                color: "red",
                fillColor: "#f03",
                fillOpacity: 0.5,
                radius: 250
            });
            modalCircle.bindTooltip("<b>預定確認後會提供確切位置。</b>", {
                direction: "top"
            });
            modalMap.addLayer(modalCircle);

            modalMap.invalidateSize();
            // }
        }, 500);
    });
});