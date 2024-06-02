//Initialize Swiper
var swiper1 = new Swiper(".swiper.reviews-swiper", {
    slidesPerView: 2,
    spaceBetween: 15,
    navigation: {
        nextEl: ".swiper.reviews-swiper .swiper-button-next",
        prevEl: ".swiper.reviews-swiper .swiper-button-prev",
    },
});

var swiper2 = new Swiper(".swiper.room-source-swiper", {
    slidesPerView: 3,
    spaceBetween: 15,
    navigation: {
        nextEl: ".swiper.room-source-swiper .swiper-button-next",
        prevEl: ".swiper.room-source-swiper .swiper-button-prev",
    },
});