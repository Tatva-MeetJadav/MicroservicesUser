var currentlySelectedUserIds = [];
var scannedStatus = "";
var valid = null;

function renderChart(passedLabels, passedDataPoints) {
    var chartCanvas = $('#verificationLineChart');
    var labels = passedLabels || JSON.parse(chartCanvas.attr('data-labels') || '[]');
    var dataPoints = passedDataPoints || JSON.parse(chartCanvas.attr('data-scans') || '[]');
    var ctx = chartCanvas[0].getContext('2d');
    window.verificationLineChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Verified Emails',
                data: dataPoints,
                borderColor: 'rgba(54, 162, 235, 1)',
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                fill: true,
                tension: 0.3,
                pointRadius: 5,
                pointHoverRadius: 7,
                borderWidth: 2,
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Time',
                        font: {
                            weight: 'bold'
                        }
                    }
                },
                y: {
                    beginAtZero: true,
                    ticks: { stepSize: 10 },
                    title: {
                        display: true,
                        text: 'Scans',
                        font: {
                            weight: 'bold'
                        }
                    }
                }
            },
            plugins: {
                legend: { display: true, position: 'top' },
                tooltip: { mode: 'index', intersect: false }
            },
            interaction: {
                mode: 'nearest',
                axis: 'x',
                intersect: false
            }
        }
    });
}


$(document).ready(function () {
    renderChart();
});

$(document).on('click', '#showResultBtn', function () {
    let selectedUserIds = [];
    $('.dashboard-dropdown-user-id').each(function () {
        let checkbox = $(this).closest('li').find('input[type="checkbox"]');
        if (checkbox.is(':checked')) {
            selectedUserIds.push(parseInt($(this).val()));
        }
    });
    currentlySelectedUserIds = selectedUserIds;
    $.ajax({
        url: '/EmailVerification/GetAdminDashboardData',
        type: 'POST',
        data: { userIds: selectedUserIds },
        success: function (response) {
            $('.email-verification-dashboard-data').html(response);
            if (window.verificationLineChart && typeof window.verificationLineChart.destroy === 'function') {
                window.verificationLineChart.destroy();
                window.verificationLineChart = null;
            }
            renderChart();
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            alert('Failed to fetch dashboard data.');
        }
    });
});

$('#dropdownSearch').on('keyup', function () {
    let searchTerm = $(this).val().toLowerCase();
    $('#dropdownMenu li').each(function () {
        let emailSpan = $(this).find('span').last();
        if (emailSpan.length) {
            let emailText = emailSpan.text().toLowerCase();
            if (emailText.includes(searchTerm)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        }
    });
});

function fetchEmailVerificationHistoryList(page, pageSize) {
    var searchQuery = $('input[name="searchQueryForHistory"]').val();
    var paginationDTO =
    {
        searchQuery: searchQuery,
        currentPage: page,
        pageSize: pageSize,
    }
    console.log(valid);
    var emailVerificationHistoryDTO =
    {
        userIds: currentlySelectedUserIds,
        paginationDTO: paginationDTO,
        scannedStatus: scannedStatus,
        valid: valid
    }
    console.log(emailVerificationHistoryDTO);
    $.ajax({
        url: "/EmailVerification/GetEmailVerificationHistory",
        type: "POST",
        contentType: "application/json",
        traditional: true,
        data: JSON.stringify(emailVerificationHistoryDTO),
        success: function (response) {
            $(".email-verification-history-list").html(response);
        },
    });
}


$(document).on("change", ".items-per-page", function () {
    fetchEmailVerificationHistoryList(1, parseInt($(this).val()));
});

$(document).on("click", ".prev-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchEmailVerificationHistoryList(currentPage - 1, parseInt($(".items-per-page").val()));
});

$(document).on("click", ".next-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchEmailVerificationHistoryList(currentPage + 1, parseInt($(".items-per-page").val()));
});

$(document).on('input', '.search-query', function () {
    var pageSize = $(".items-per-page").val();
    fetchEmailVerificationHistoryList(1, pageSize);
})

$(document).on("click", ".page-index", function () {
    var page = parseInt($(this).data("page"));
    var pageSize = parseInt($(".items-per-page").val());
    fetchEmailVerificationHistoryList(page, pageSize);
});

$(document).on('click', '.view-detail-eye', function () {
    var id = parseInt($(this).find('input').val());
    $.ajax({
        url: "/ProxyVpnDetection/GetProxyVpnViewDetail",
        type: "GET",
        data: { id: id },
        success: function (response) {
            $(".proxy-vpn-view-detail-body").html(response);
            $('.view-detail-email-verification').modal('show');
        },
    });
});

$('#selectAllActive').on('change', function () {
    var isChecked = $(this).is(':checked');
    $('.active-checkbox').prop('checked', isChecked);
    updateGlobalSelectAll();
});

$('#selectAllBlocked').on('change', function () {
    var isChecked = $(this).is(':checked');
    $('.blocked-checkbox').prop('checked', isChecked);
    updateGlobalSelectAll();
});

$('#selectAllInactive').on('change', function () {
    var isChecked = $(this).is(':checked');
    $('.inactive-checkbox').prop('checked', isChecked);
    updateGlobalSelectAll();
});

$('#selectAllUsers').on('change', function () {
    var isChecked = $(this).is(':checked');
    $('.user-checkbox').prop('checked', isChecked);
    $('#selectAllActive, #selectAllBlocked, #selectAllInactive').prop('checked', isChecked);
});


$('.user-checkbox').on('change', function () {
    updateGroupCheckbox('active');
    updateGroupCheckbox('blocked');
    updateGroupCheckbox('inactive');
    updateGlobalSelectAll();
});

function updateGroupCheckbox(group) {
    var all = $('.' + group + '-checkbox');
    var checked = all.filter(':checked');
    var groupSelectAllId = '#selectAll' + capitalize(group);
    $(groupSelectAllId).prop('checked', all.length === checked.length);
}

function updateGlobalSelectAll() {
    var all = $('.user-checkbox');
    var checked = all.filter(':checked');
    $('#selectAllUsers').prop('checked', all.length === checked.length);
}

function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

$(document).on('change', '.scanned-status-filter, .valid-filter', function () {
    var scannedStatusFilter = $('.scanned-status-filter').val();
    scannedStatus = scannedStatusFilter;
    var validValue = $('.valid-filter').val();
    if (validValue == "True") {
        valid = true;
    }
    else if (validValue == "False") {
        valid = false;
    }
    else {
        valid = null;
    }
    fetchEmailVerificationHistoryList(1, parseInt($(".items-per-page").val()));
});


$(document).on('change', '.chart-time-filter', function () {
    if (window.verificationLineChart && typeof window.verificationLineChart.destroy === 'function') {
        window.verificationLineChart.destroy();
        window.verificationLineChart = null;
    }
    var range = $(this).val();
    $.ajax({
        url: '/EmailVerification/GetChartData',
        traditional: true,
        data: { userIds: currentlySelectedUserIds, range: range },
        type: 'GET',
        success: function (data) {
            var scans = data.map(item => item.emailVerificationCount);
            var labels = data.map(item => item.createdAt);
            renderChart(labels, scans);
        }
    });
});