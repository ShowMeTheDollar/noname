
// swiper category
const swiper = new Swiper('.swiper-category', {
    // Optional parameters
    direction: 'horizontal',
    slidesPerView: 5,
    spaceBetween: 20,
    allowTouchMove: true,
    navigation: {
        nextEl: '.swiper-category .swiper-button-next',
        prevEl: '.swiper-category .swiper-button-prev',
    },
    scrollbar: {
        el: '.swiper-scrollbar',
    },
});
// card-section-set
const swiper2 = new Swiper('.swiper-card-item', {
    // Optional parameters
    direction: 'horizontal',
    allowTouchMove: true,

    pagination: {
        el: ".swiper-card-item .swiper-pagination",
        clickable: true,
    },
});

//flatpickr
flatpickr("input[type=datetime-local]", {
    mode: "range",
    showMonths: 2,
    dateFormat: "Y-m-d",
    minDate: "today",
    onClose: function (selectedDates, dateStr, instance) {
        if (selectedDates.length === 2) {
            // 獲取並調整開始日期
            let startDate = new Date(selectedDates[0]);
            startDate.setMinutes(startDate.getMinutes() - startDate.getTimezoneOffset());
            document.getElementById("startDatePicker").value = startDate.toISOString().substring(0, 10);

            // 獲取並調整結束日期
            let endDate = new Date(selectedDates[1]);
            endDate.setMinutes(endDate.getMinutes() - endDate.getTimezoneOffset());
            document.getElementById("endDatePicker").value = endDate.toISOString().substring(0, 10);
        }
    }
});
flatpickr(".phone-timepickr", {
    mode: "single",
    showMonths: 1,
    dateFormat: "Y-m-d",
    minDate: "today",
});

//Leaflet
window.onload = function () {
    var map = L.map('map', {
        closePopupOnClick: false
    }).setView([23.97565, 120.9738819], 8);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
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

            if (!isNaN(lat) && !isNaN(lng)) {
                let displayName;
                if (roomName.length > 10) {
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

// Search field read query string
document.addEventListener('DOMContentLoaded', function () {
    // 從當前URL中取出查詢字串
    const urlParams = new URLSearchParams(window.location.search);

    // 從查詢字串中取出各個參數的值
    const location = urlParams.get('location');
    const checkInDate = urlParams.get('checkInDate');
    const checkOutDate = urlParams.get('checkOutDate');
    const numberOfGuests = urlParams.get('numberOfGuests');

    // 將取出的值填入相應的輸入欄位中
    if (location) {
        document.querySelector('.location input').value = location;
    }
    if (checkInDate) {
        document.getElementById('startDatePicker').value = checkInDate;
    }
    if (checkOutDate) {
        document.getElementById('endDatePicker').value = checkOutDate;
    }
    if (numberOfGuests) {
        document.querySelector('.guests input').value = numberOfGuests;
    }
});

// Pass search criteria to rooms
document.addEventListener('DOMContentLoaded', function () {
    const roomLinks = document.querySelectorAll('.room-link');
    roomLinks.forEach(function (link) {
        link.addEventListener('click', GoToRoomDetails);
    });
});
function GoToRoomDetails(event) {
    event.preventDefault();

    const roomId = event.currentTarget.id || 0;
    const location = document.querySelector('.location input').value;
    const checkInDate = document.getElementById('startDatePicker').value;
    const checkOutDate = document.getElementById('endDatePicker').value;
    const numberOfGuests = document.querySelector('.guests input').value;
    const origin = window.location.origin;
    const url = new URL(`${origin}/Rooms/${roomId}`);

    if (location) {
        url.searchParams.set('location', location);
    }
    if (checkInDate) {
        url.searchParams.set('checkInDate', checkInDate);
    }
    if (checkOutDate) {
        url.searchParams.set('checkOutDate', checkOutDate);
    }
    if (numberOfGuests) {
        url.searchParams.set('numberOfGuests', numberOfGuests);
    }

    window.location.href = url;
}

// numberofguests
const app1 = Vue.createApp({
    data() {
        return {
            guestCount: 1,
            maxGuests: 10
        };
    },
    methods: {
        increaseGuestCount() {
            if (this.guestCount < this.maxGuests) {
                this.guestCount++;
            }
        },
        decreaseGuestCount() {
            if (this.guestCount > 1) {
                this.guestCount--;
            }
        }
    }
});
app1.mount('#numberofguests');

//filter-button
const app2 = Vue.createApp({
    data() {
        return {
            minPrice: 1000,
            maxPrice: 100000,
            currentMin: 2000,
            currentMax: 10000,
            priceAvg: 2024,
            trackColor: 'background:linear-gradient(to right, #d5d5d5 0% , #000 0% , #000 100%, #d5d5d5 100%)',
            roomCategory: '',
            bedrooms: 0,
            beds: 0,
            bathrooms: 0,
            amenities: [],
            languages: [],
            priceGap: 1000,
        };
    },
    mounted() {
        this.updateRangeTrack();
    },
    methods: {
        clearCheck() {
            this.currentMin = this.minPrice;
            this.currentMax = this.maxPrice;
            this.roomCategory = '';
            this.bedrooms = 0;
            this.beds = 0;
            this.bathrooms = 0;
            this.amenities = [];
            this.languages = [];
            this.fillLine();
        },
        passData() {
            const origin = window.location.origin;
            const url = new URL(`${origin}/Search/DetailedSearch`);

            url.searchParams.append('CurrentMax', this.currentMax);
            url.searchParams.append('CurrentMin', this.currentMin);
            url.searchParams.append('RoomCategory', this.roomCategory);
            url.searchParams.append('Bedrooms', this.bedrooms);
            url.searchParams.append('Beds', this.beds);
            url.searchParams.append('Bathrooms', this.bathrooms);

            this.amenities.forEach(amenity => {
                url.searchParams.append('Amenities', amenity);
            });

            this.languages.forEach(language => {
                url.searchParams.append('Languages', language);
            });

            window.location = url;
        },
        updateRangeTrack() {
            let p1 = Math.round(((this.currentMin - this.minPrice) / (this.maxPrice - this.minPrice)) * 100);
            let p2 = Math.round(((this.currentMax - this.minPrice) / (this.maxPrice - this.minPrice)) * 100);
            this.trackColor = `background:linear-gradient(to right, #d5d5d5 ${p1}% , #000 ${p1}% , #000 ${p2}%, #d5d5d5 ${p2}%)`;
        },
        handlePriceInput(e) {
            let minPrice = parseInt(this.currentMin);
            let maxPrice = parseInt(this.currentMax);

            if (maxPrice - minPrice >= this.priceGap && maxPrice <= this.maxPrice) {
                if (e.target.className === "min-price") {
                    this.currentMin = minPrice;
                } else {
                    this.currentMax = maxPrice;
                }
                this.updateRangeTrack();
            }
        },
        handleRangeInput(e) {
            let minVal = parseInt(this.currentMin);
            let maxVal = parseInt(this.currentMax);

            if (maxVal - minVal < this.priceGap) {
                if (e.target.id === "slider-1") {
                    this.currentMin = maxVal - this.priceGap;
                } else {
                    this.currentMax = minVal + this.priceGap;
                }
            } else {
                this.currentMin = minVal;
                this.currentMax = maxVal;
            }
            this.updateRangeTrack();
        },
        fillLine() {
            this.updateRangeTrack();
        },
        setMax() {
            if (this.currentMax > this.maxPrice) {
                this.currentMax = this.maxPrice;
            }
        },
        setMin() {
            if (this.currentMin < this.minPrice) {
                this.currentMin = this.minPrice;
            }
        },
    },
    computed: {
        fillLineOnchange() {
            let p1 = Math.round(((this.currentMin - this.minPrice) / (this.maxPrice - this.minPrice)) * 100);
            let p2 = Math.round(((this.currentMax - this.minPrice) / (this.maxPrice - this.minPrice)) * 100);
            return `background-image:linear-gradient(to right, #d5d5d5 ${p1}% , #000 ${p1}% , #000 ${p2}%, #d5d5d5 ${p2}%)`;
        }
    }
});
app2.mount('#filter-button');