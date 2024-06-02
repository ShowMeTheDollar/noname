flatpickr("#startDatePicker", {
    inline: true,
    mode: "multiple",
    minDate: "today",
    onChange: function (selectedDates, dateStr, instance) {
        // selectedDates(array),dateStr(yyyy-MM-dd),instance(object)
        Cookies.set('PickedDates', dateStr);
    },
    onMonthChange: function () {
        createAccommodationPrice();
    },
    onYearChange: function () {
        createAccommodationPrice();
    }
});
window.onload = () => {
    checkDisableButtons()
    Cookies.remove('PickedDates')
    Cookies.set('PickedDates', []);
};
const noRoom = document.querySelector('.no-room')
if (calendarModelJson.rooms.length > 0) {
    noRoom.style.display = 'none';
}
let fixedPriceInput = document.querySelector('.fixedPrice');
let weekendPriceInput = document.querySelector('.weekendPrice');
let separatePriceInput = document.querySelector('.separatePrice');
let roomId = document.getElementById('RoomId')
fixedPriceInput.value = calendarModelJson.rooms[0].fixedPrice
weekendPriceInput.value = calendarModelJson.rooms[0].weekendPrice ? calendarModelJson.rooms[0].weekendPrice : "";
roomId.value = calendarModelJson.rooms[0].roomId
const originalFixedPrice = fixedPriceInput
const originalweekendPrice = weekendPriceInput
let fixedInitialPrice = fixedPriceInput.value

let fixedPrice = "$" + document.querySelector('.fixedPrice').value
let weekendPricevalue = document.querySelector('.weekendPrice').value
let weekendPrice = weekendPricevalue ? "$" + weekendPricevalue : fixedPrice
const currentMonth = document.querySelector('.flatpickr-current-month')

currentMonth.addEventListener('change', () => {
    clearAllPrice()
    createAccommodationPrice();
});

let individualPricesArr = calendarModelJson.rooms[0].individualPrices;
let modifiedIndividualPricesArr = individualPricesArr.map(item => {
    // 將 date 屬性轉換為 Date 物件
    let date = new Date(item.date);
    // 將 date 屬性轉換為 UTC 時間
    date.setTime(date.getTime() - (date.getTimezoneOffset() * 60000));
    // 將 date 屬性轉換為毫秒數
    const formattedDate = date.toLocaleDateString('en-CA');
    // 將修改後的 date 屬性回傳
    return { ...item, date: formattedDate };
});


function clearAllPrice() {
    const allAccommodationFees = document.querySelectorAll('.flatpickr-day .accommodation-fee');
    allAccommodationFees.forEach(fee => fee.textContent = '');
}
function createAccommodationPrice() {
    const days = document.querySelectorAll('.flatpickr-rContainer .flatpickr-day');
    days.forEach(day => {
        const date = new Date(day.dateObj);
        //出來是這樣Mon Apr 01 2024 00:00:00 GMT+0800 (GMT+08:00)
        const formattedDate = date.toLocaleDateString('en-CA');
        //轉成加拿大的日期表示yyyy-MM-dd
        const roomPriceObj = modifiedIndividualPricesArr.find(roomPrice => roomPrice.date.split('T')[0] === formattedDate);
        //沒有找到的話會回傳undefined,Obj的日期樣式"2024-04-16T00:00:00",以T為中間值分成兩個部分取[0]
        if (roomPriceObj) {
            let price = "$" + roomPriceObj.separatePrice;
            let accommodationFee = document.createElement('div');
            accommodationFee.textContent = price;
            accommodationFee.classList.add('accommodation-fee');
            accommodationFee.style.pointerEvents = 'none';
            day.appendChild(accommodationFee);
        } else {
            let price = (date.getDay() === 5 || date.getDay() === 6) ? weekendPrice : fixedPrice;
            let accommodationFee = document.createElement('div');
            accommodationFee.textContent = price;
            accommodationFee.classList.add('accommodation-fee');
            accommodationFee.style.pointerEvents = 'none';
            day.appendChild(accommodationFee);
        }
    });
}

document.addEventListener("DOMContentLoaded", () => {
    createAccommodationPrice();
});

document.querySelector('.flatpickr-days').addEventListener('click', function (event) {
    let target = event.target;
    if (!target.classList.contains('flatpickr-disabled') && target.classList.contains('flatpickr-day')) {
        createAccommodationPrice()
    }
});

/*數字輸入框調整*/
let priceInputs = document.querySelectorAll('.price-input');
priceInputs.forEach(input => {
    input.addEventListener('blur', function () {
        let price = parseInt(this.value);
        if (isNaN(price)) {
            if (this.classList.contains('fixedPrice')) {
                price = fixedInitialPrice;
            }
        }
        price = Math.max(100, price);
        price = Math.min(99999, price);
        price = Math.round(price);
        this.value = price;
    });
});

document.querySelectorAll('.price-input').forEach(input => {
    input.addEventListener('input', checkDisableButtons);
});

function checkDisableButtons() {
    const fixedPriceSaveButton = document.querySelector('.fixedPriceSave');
    const weekendPriceSaveButton = document.querySelector('.weekendPriceSave');
    const separatePriceSaveButton = document.querySelector('.separatePriceSave');
    fixedPriceSaveButton.disabled = fixedPriceInput.value === '';
    weekendPriceSaveButton.disabled = weekendPriceInput.value === '';
    separatePriceSaveButton.disabled = separatePriceInput.value === '';
}


window.onload = function () {
    updateRoomPriceInfo()
    checkDisableButtons()
};
document.getElementById('roomSelect').addEventListener('change', function () {
    // 獲取所選房源的roomId
    let selectedRoomId = this.value;

    // 從calendarModel中找到對應的房源資訊
    let selectedRoom = calendarModelJson.rooms.find(function (room) {
        return room.roomId == selectedRoomId;
    });

    // 更新表單中的相關欄位值
    document.querySelector('.fixedPrice').value = selectedRoom.fixedPrice
    document.querySelector('.weekendPrice').value = selectedRoom.weekendPrice ? selectedRoom.weekendPrice : '';
    roomId.value = selectedRoom.roomId
    fixedInitialPrice = selectedRoom.fixedPrice
    fixedPrice = "$" + document.querySelector('.fixedPrice').value
    weekendPricevalue = document.querySelector('.weekendPrice').value
    weekendPrice = weekendPricevalue ? "$" + weekendPricevalue : fixedPrice
    individualPricesArr = selectedRoom.individualPrices
    modifiedIndividualPricesArr = individualPricesArr.map(item => {
        let localDate = new Date(item.date);
        //加上去時差
        localDate.setTime(localDate.getTime() - (localDate.getTimezoneOffset() * 60000));
        const formattedLocalDate = localDate.toLocaleDateString('en-CA');
        return { ...item, date: formattedLocalDate };
    });
    clearAllPrice()
    createAccommodationPrice();
    checkDisableButtons()
});

function updateRoomPriceInfo() {
    if (beSelectedRoomId) {
        let roomIdToUse = beSelectedRoomId

        // 從calendarModel中找到對應的房源資訊
        let selectedRoom = calendarModelJson.rooms.find(function (room) {
            return room.roomId == roomIdToUse;
        });
        document.querySelector('.fixedPrice').value = selectedRoom.fixedPrice
        document.querySelector('.weekendPrice').value = selectedRoom.weekendPrice ? selectedRoom.weekendPrice : '';
        roomId.value = selectedRoom.roomId
        fixedInitialPrice = selectedRoom.fixedPrice
        fixedPrice = "$" + document.querySelector('.fixedPrice').value
        weekendPricevalue = document.querySelector('.weekendPrice').value
        weekendPrice = weekendPricevalue ? "$" + weekendPricevalue : fixedPrice
        individualPricesArr = selectedRoom.individualPrices
        modifiedIndividualPricesArr = individualPricesArr.map(item => {
            let localDate = new Date(item.date);
            //加上去時差
            localDate.setTime(localDate.getTime() - (localDate.getTimezoneOffset() * 60000));
            const formattedLocalDate = localDate.toLocaleDateString('en-CA');
            return { ...item, date: formattedLocalDate };
        });
        clearAllPrice()
        createAccommodationPrice();
        checkDisableButtons()
    }
}