$('.profilePhotoInput').on('change', function () {
    var allowedTypes = ['image/jpeg', 'image/png', 'image/jpg'];
    var file = this.files[0];
    var errorSpan = $('.profilePhotoError');
    if (file && $.inArray(file.type, allowedTypes) === -1) {
        errorSpan.text('Only JPG and PNG files are allowed.');
        $(this).val('');
    } else {
        errorSpan.text('');
    }
});

$(document).ready(function () {
    var labels = [
        "08:00", "09:00", "10:00", "11:00", "12:00", "13:00"
    ];
    var data = {
        labels: labels,
        datasets: [{
            label: 'Email Verifications',
            data: [12, 18, 9, 15, 22, 17], // Replace with your real data
            backgroundColor: '#0b6060',
            borderColor: '#0b6060',
            borderWidth: 1
        }]
    };
    var config = {
        type: 'bar',
        data: data,
        options: {
            plugins: {
                legend: { display: false }
            },
            scales: {
                x: {
                    title: { display: true, text: 'Time' }
                },
                y: {
                    title: { display: true, text: 'Email Verifications' },
                    beginAtZero: true
                }
            }
        }
    };
    var chartCanvas = document.querySelector('.email-verification-bar-chart');
    new Chart(chartCanvas, config);
})
