function validateEmail() {
    var email = document.getElementById('UserEmail').value;
    var pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // 簡單的電子郵件地址模式
    if (!pattern.test(email)) {
        Swal.fire({
            icon: 'warning',
            title: '無效的電子信箱',
            text: '請輸入有效的電子信箱地址。',
            confirmButtonText: '確定'
        });
        return false; // 阻止表單提交
    }

    //Swal.fire({
    //    icon: 'success',
    //    title: '查看電子信箱',
    //    text: '我們已將驗證信寄給您。',
    //});

    return true; // 允許表單提交
}