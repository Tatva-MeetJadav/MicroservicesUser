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
