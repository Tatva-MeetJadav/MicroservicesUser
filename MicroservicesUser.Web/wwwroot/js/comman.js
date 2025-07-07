$('.sidebar-toggle').on('click', function () {
    var sidebar = $(this).closest('.main-container').find('.sidebar');
    var mainContent = $(this).closest('.main-container').find('.main-content');
    sidebar.toggleClass('sidebar-collapsed');
    mainContent.toggleClass('full-width');
});

$(function () {
    $('[data-bs-toggle="popover"]').popover();
});

$('.changePasswordModal').on('show.bs.modal', function () {
    var form = $('.change-password-form');
    form[0].reset();
    form.find('.text-danger').text('');
});

$('.profilePhotoInput').on('change', function () {
    var allowedTypes = ['image/jpeg', 'image/png', 'image/jpg'];
    var file = this.files[0];
    var errorSpan = $('.profilePhotoError');
    if (file && $.inArray(file.type, allowedTypes) === -1) {
        errorSpan.text('Only JPG/JPEG and PNG files are allowed.');
        $(this).val('');
    } else {
        errorSpan.text('');
    }
});
