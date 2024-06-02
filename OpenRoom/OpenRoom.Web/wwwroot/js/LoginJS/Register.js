document.addEventListener('DOMContentLoaded', function () {   
    if (registerResult == 'success') {
        Swal.fire({
            icon: 'success',
            title: '註冊成功',
            text: '請登入。',
            confirmButtonText: '確定'
        });
    } else if (registerResult == 'failure') {
        Swal.fire({
            icon: 'error',
            title: '您已經註冊過',
            text: '請確認您的輸入或連繫客服人員。',
            confirmButtonText: '確定'
        });
    }
});
