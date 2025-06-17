$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);
    var token = urlParams.get('token');
    $('.password-reset-token').val(token);
});