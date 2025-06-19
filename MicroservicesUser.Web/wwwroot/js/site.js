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
    var form = $('.changePasswordForm');
    form[0].reset();
    form.find('.text-danger').text('');
});

$(document).on('submit', '.changePasswordForm', function (e) {
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