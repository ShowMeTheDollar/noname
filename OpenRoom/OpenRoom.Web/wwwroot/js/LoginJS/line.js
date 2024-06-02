function Login() {
    var URL = 'https://access.line.me/oauth2/v2.1/authorize?';
    URL += 'response_type=code';
    URL += '&client_id=2003888052';   //TODO:這邊要換成你的client_id
    URL += '&redirect_uri=https://c722-150-117-180-91.ngrok-free.app/';   //TODO:要將此redirect url 填回你的 LineLogin後台設定
    URL += '&scope=openid%20profile%20email';
    URL += '&state=abcde';
    window.location.href = URL;
}
$(function () {
    $('#Login').click(function (e) {
        Login();
    });
});