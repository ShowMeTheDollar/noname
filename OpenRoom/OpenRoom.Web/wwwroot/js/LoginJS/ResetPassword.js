document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('form');
    const newPassword = document.getElementById('newPassword');
    const confirmPassword = document.getElementById('confirmPassword');
    const tokenInput = document.querySelector('input[name="token"]');
    const timeInput = document.querySelector('input[name="time"]');

    // 從網址中獲取 token 和 time 參數的值
    const urlParams = new URLSearchParams(window.location.search);
    const token = urlParams.get('token');
    const time = urlParams.get('time');

    // 將 token 和 time 的值設置到隱藏的 input 欄位中
    tokenInput.value = token;
    timeInput.value = time;

    form.addEventListener('submit', function (event) {
        event.preventDefault();

        if (newPassword.value.trim() === '') {
            Swal.fire({
                icon: 'warning',
                title: '無效的密碼',
                text: '請輸入新密碼。',
                confirmButtonText: '確定'
            });
            newPassword.focus();
            return;
        }

        if (!isPasswordValid(newPassword.value)) {
            Swal.fire({
                icon: 'warning',
                title: '無效的密碼',
                text: '密碼必須包含至少一個大寫字母、一個小寫字母和一個數字，且最少8個字符。',
                confirmButtonText: '確定'
            });           
            newPassword.focus();
            return;
        }

        if (confirmPassword.value.trim() === '') {
            Swal.fire({
                icon: 'warning',
                title: '無效的密碼',
                text: '請輸入確認密碼。',
                confirmButtonText: '確定'
            });
            confirmPassword.focus();
            return;
        }

        if (newPassword.value !== confirmPassword.value) {
            Swal.fire({
                icon: 'warning',
                title: '無效的密碼',
                text: '新密碼與確認密碼不一致。',
                confirmButtonText: '確定'
            });
            confirmPassword.focus();
            return;
        }

        form.submit();
    });

    function isPasswordValid(password) {
        const passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$/;
        return passwordPattern.test(password);
    }
});