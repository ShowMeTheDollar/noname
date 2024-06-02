
//flatpickr
flatpickr(".calendar-picker", {
    mode: "range",
    showMonths: 2,
    dateFormat: "Y-m-d",
    minDate: "today",
    onClose: function (selectedDates, dateStr, instance) {
        if (selectedDates.length === 2) {
            // 獲取並調整開始日期
            let startDate = new Date(selectedDates[0]);
            startDate.setMinutes(startDate.getMinutes() - startDate.getTimezoneOffset());
            document.getElementById("startDatePicker").innerText = startDate.toISOString().substring(0, 10);

            // 獲取並調整結束日期
            let endDate = new Date(selectedDates[1]);
            endDate.setMinutes(endDate.getMinutes() - endDate.getTimezoneOffset());
            document.getElementById("endDatePicker").innerText = endDate.toISOString().substring(0, 10);

            //將開始與結束日期顯示到畫面上
            document.querySelector('.trip-dates').innerText = `${startDate.toISOString().substring(0, 10)} 至 ${endDate.toISOString().substring(0, 10) }`;
        }
    }
});
