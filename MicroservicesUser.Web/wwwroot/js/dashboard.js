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

$(document).ready(function () {
    var chartCanvas = $('.email-verification-bar-chart');
    var chartInstance = null;

    function renderChart(labels, scans) {
        if (chartInstance) {
            chartInstance.destroy();
        }
        var data = {
            labels: labels,
            datasets: [{
                label: 'Email Verifications',
                data: scans,
                backgroundColor: '0b6060',
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
                        title: { display: true, text: 'Scans' },
                        beginAtZero: true
                    }
                }
            }
        };
        chartInstance = new Chart(chartCanvas[0], config);
    }

    // Initial chart render
    var chartDataDiv = $('.chart-data');
    var labels = JSON.parse(chartDataDiv.attr('data-labels'));
    var scans = JSON.parse(chartDataDiv.attr('data-scans'));
    renderChart(labels, scans);

    // Handle dropdown change
    $('.chart-time-filter').on('change', function () {
        var range = $(this).val();
        $.ajax({
            url: '/Dashboard/GetChartData',
            data: { range: range },
            type: 'GET',
            success: function (data) {
                renderChart(data.labels, data.scans);
            }
        });
    });
});