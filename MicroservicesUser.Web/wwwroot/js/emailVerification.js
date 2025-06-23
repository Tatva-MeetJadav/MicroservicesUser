$(document).on('submit', '.email-verify-form', function (e) {
    e.preventDefault();
    var formData = $(this).serialize();
    $.ajax({
        url: "/EmailVerification/Index",
        type: "POST",
        data: formData,
        success: function (response) {
            toastr.success("email verification successfully.");
            $('.append-email-verification-result').html(response);
        },
        error: function () {
            toastr.error("Something went wrong!");
        }
    });
});