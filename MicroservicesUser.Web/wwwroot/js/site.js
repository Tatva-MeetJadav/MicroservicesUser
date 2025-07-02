const connection = new signalR.HubConnectionBuilder()
    .withUrl("/logouthub")
    .build();


connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("ForceLogout", function () {
    window.location.href = "/Authentication/Login";
});

$('#changePasswordModal').on('show.bs.modal', function () {
    var form = $('.change-password-form');
    form[0].reset();
    form.find('.text-danger').text('');
});

$(document).on('submit', '.change-password-form', function (e) {
    e.preventDefault();
    var formData = $(this).serialize();
    $.ajax({
        url: "/Dashboard/ChangePassword",
        type: "POST",
        data: formData,
        success: function (response) {
            if (response == "success") {
                toastr.success("Password changed successfully.");
                $("#changePasswordModal").modal("hide");
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

$('.sidebar-toggle').on('click', function () {
    var sidebar = $(this).closest('.main-container').find('.sidebar');
    var mainContent = $(this).closest('.main-container').find('.main-content');
    sidebar.toggleClass('sidebar-collapsed');
    mainContent.toggleClass('full-width');
});

$(function () {
    $('[data-bs-toggle="popover"]').popover();
});