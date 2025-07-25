$(document).ready(function () {
    var chartDataDiv = $('.chart-data');
    var userLabels = JSON.parse(chartDataDiv.attr('data-user-labels'));
    var userCounts = JSON.parse(chartDataDiv.attr('data-user-counts'));
    var apiUsageRequestCount = Number(chartDataDiv.attr('data-service-requestcount'));
    var apiUsageDailyLimit = Number(chartDataDiv.attr('data-service-dailylimit'));
    var userChartCtx = document.getElementById('userChart').getContext('2d');
    var userChart = new Chart(userChartCtx, {
        type: 'bar',
        data: {
            labels: userLabels,
            datasets: [{
                label: 'User Registrations',
                data: userCounts,
                backgroundColor: 'rgba(0, 123, 255, 0.5)',
                borderColor: 'rgba(0, 123, 255, 1)',
                borderWidth: 1,
                fill: false,
                hoverBackgroundColor: 'rgba(0, 123, 255, 0.7)'
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: true, position: 'bottom' },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return `Count: ${context.raw}`;  // Tooltip displays the count
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: { stepSize: 5 }
                },
                x: {
                    type: 'category',  // Category scale for interval labels
                    title: { display: true, text: 'Time Intervals' }
                }
            }
        }
    });

    function updateServiceUsageChart(requestCount, dailyLimit) {
        var apiUsagePercent = dailyLimit > 0 ? Math.round((requestCount / dailyLimit) * 100) : 0;

        $('#circlePercent').text(apiUsagePercent + '%');
        apiUsageChart.data.datasets[0].data = [apiUsagePercent, 100 - apiUsagePercent];
        apiUsageChart.update();

        $('.api-bar').css('width', apiUsagePercent + '%');
        $('.bar-percentage').text(apiUsagePercent + '%');
        $('.usage-text').text(requestCount + ' / ' + dailyLimit + ' requests used');
    }

    $('.api-usage-card select').on('change', function () {
        var selectedRange = $(this).val();
        $.ajax({
            url: '/Dashboard/GetServiceUsageChartData',
            type: 'GET',
            data: { range: selectedRange },
            success: function (data) {
                var requestCount = data.requestCount;
                var dailyLimit = data.dailyLimit;
                updateServiceUsageChart(requestCount, dailyLimit);  // Update the chart and progress bar
            }
        });
    });

    $('.col-lg-7 .chart-time-filter').on('change', function () {
        var selectedRange = $(this).val();
        $.ajax({
            url: '/Dashboard/GetUserRegistrationsChartData',
            type: 'GET',
            data: { range: selectedRange },
            success: function (data) {
                userChart.data.labels = data.map(x => x.createdAt);
                userChart.data.datasets[0].data = data.map(x => x.userCount);
                userChart.update();
            }
        });
    });

    var apiUsageCtx = document.getElementById('apiUsageCircle').getContext('2d');
    var apiUsageChart = new Chart(apiUsageCtx, {
        type: 'doughnut',
        data: {
            datasets: [{
                data: [apiUsageRequestCount, 100 - apiUsageRequestCount], // Default usage
                backgroundColor: ['rgba(0, 123, 255, 0.7)', '#e9ecef'],
                borderWidth: 0
            }]
        },
        options: {
            cutout: '83%', 
            plugins: {
                tooltip: { enabled: false },
                legend: { display: false }
            }
        }
    });

    updateServiceUsageChart(apiUsageRequestCount, apiUsageDailyLimit);  // Initialize with data from server

});
