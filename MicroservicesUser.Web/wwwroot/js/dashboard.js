$(document).ready(function () {
    var chartCanvas = $('.email-verification-bar-chart');
    var chartInstance = null;

    // Function to render or update the chart
    function renderChart(labels, scans) {
        if (chartInstance) {
            // Update the chart data
            chartInstance.data.labels = labels;
            chartInstance.data.datasets[0].data = scans;

            // Update the chart (this is the key change to avoid re-creating it)
            chartInstance.update();
        } else {
            // If the chart instance doesn't exist, create a new one
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

            // Create a new chart instance
            chartInstance = new Chart(chartCanvas[0], config);
        }
    }

    // Get initial data from the page
    var chartDataDiv = $('.chart-data');
    var labels = JSON.parse(chartDataDiv.attr('data-labels'));
    var scans = JSON.parse(chartDataDiv.attr('data-scans'));
    renderChart(labels, scans);

    // Update chart data based on time filter change
    $('.chart-time-filter').on('change', function () {
        var range = $(this).val();
        $.ajax({
            url: '/Dashboard/GetChartData',
            data: { range: range },
            type: 'GET',
            success: function (data) {
                renderChart(data.labels, data.scans); // Update chart with new data
            }
        });
    });
});
