var logoutConnection = new signalR.HubConnectionBuilder()
    .withUrl("/logouthub")
    .build();


logoutConnection.start().catch(function (err) {
    return console.error(err.toString());
});
logoutConnection.on("ForceLogout", function () {
    window.location.href = "/Authentication/AdminLogin";
});

var notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();
notificationConnection.start().catch(console.error);
notificationConnection.on("ReceiveNotification", function () {
    GetUnreadNotifications();
});

$(document).on('submit', '.change-password-form', function (e) {
    e.preventDefault();
    var formData = $(this).serialize();
    $.ajax({
        url: "/Dashboard/AdminChangePassword",
        type: "POST",
        data: formData,
        success: function (response) {
            if (response == "success") {
                toastr.success("Password changed successfully.");
                $(".changePasswordModal").modal("hide");
            }
            else if (response == "wrongPassword") {
                toastr.error("Wrong password!");
            }
            else {
                toastr.error("Something went wrong!")
            }
        },
    });
});

$(document).ready(function () {
    $.ajax({
        url: '/Dashboard/GetProfilePhoto',
        method: 'GET',
        success: function (data) {
            let profilePhoto = data;
            let imagePath = "/images/profile/" + profilePhoto;
            $('.profile-image').attr('src', imagePath);
        },
    });
    if ($('.append-admin-notification').length > 0) {
        GetUnreadNotifications();
    }

});

function GetUnreadNotifications() {
    $.ajax({
        url: '/Dashboard/GetUnreadNotifications',
        method: 'GET',
        success: function (data) {
            $('.append-admin-notification').html(data);
        },
    });
}

$(document).on('click', '.mark-as-all-read-button', function () {
    $.ajax({
        url: '/Dashboard/ReadAllNotifications',
        method: 'POST',
        success: function (data) {
            $('.append-admin-notification').html(data);
        }
    });
})
