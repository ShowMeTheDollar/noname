
flatpickr("input[type=datetime-local]", {
    inline: true,

    dateFormat: "Y-m-d",
    mode: "multiple",
    minDate: "today",
});

const prevMonthNav = document.querySelector('.flatpickr-prev-month');
const nextMonthNav = document.querySelector('.flatpickr-next-month');

prevMonthNav.addEventListener('click', () => {
    setTimeout(() => {
        createAccommodationPrice();
    }, 50);
})
nextMonthNav.addEventListener('click', () => {
    setTimeout(() => {
        createAccommodationPrice();
    }, 50);
})

window.onload = () => {
    createAccommodationPrice();
};


function createAccommodationPrice() {
    const dayElements = document.querySelectorAll('.flatpickr-rContainer  .flatpickr-day');
    dayElements.forEach(dayElement => {
        const spanElement1 = document.createElement('div');
        const spanElement2 = document.createElement('span');
        spanElement1.textContent = '$10,000';
        spanElement1.style.pointerEvents = 'none'
        //spanElement2.textContent = 'TWD';
        //spanElement2.classList.add('ps-1');
        //spanElement2.style.pointerEvents = 'none'
        //spanElement1.appendChild(spanElement2);
        dayElement.appendChild(spanElement1);
    });
}

document.querySelector('.flatpickr-days').addEventListener('click', function (event) {
    const target = event.target;
    if (!target.classList.contains('flatpickr-disabled') && target.classList.contains('flatpickr-day')) {
        createAccommodationPrice();
    }


});
