var connection = new signalR.HubConnectionBuilder()
    .withUrl("/logouthub")
    .build();


connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("ForceLogout", function () {
    window.location.href = "/Authentication/AdminLogin";
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
});
