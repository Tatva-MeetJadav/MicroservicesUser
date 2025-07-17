function renderChart() {
    var chartCanvas = $('#verificationLineChart');
    var labels = JSON.parse(chartCanvas.attr('data-labels') || '[]');
    var dataPoints = JSON.parse(chartCanvas.attr('data-scans') || '[]');
    var ctx = chartCanvas[0].getContext('2d');
    var verificationLineChart = new Chart(ctx, {
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

    return verificationLineChart;
}

$(document).ready(function () {
    renderChart();
});

var currentlySelectedUserIds = [];
var connectionType = "";
var riskLevel = "";

$('#showResultBtn').on('click', function () {
    let selectedUserIds = [];
    $('.dashboard-dropdown-user-id').each(function () {
        let checkbox = $(this).closest('li').find('input[type="checkbox"]');
        if (checkbox.is(':checked')) {
            selectedUserIds.push(parseInt($(this).val()));
        }
    });

    currentlySelectedUserIds = selectedUserIds;
    $.ajax({
        url: '/ProxyVpnDetection/GetDashboardData',
        type: 'POST',
        data: { userIds: selectedUserIds },
        success: function (response) {
            $('.proxy-vpn-dashboard-data').html(response);
            renderPieChart();
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
    var emailVerificationHistoryDTO =
    {
        userIds: currentlySelectedUserIds,
        paginationDTO: paginationDTO,
        connectionType: connectionType,
        riskStatus: riskLevel
    }
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
    fetchEmailVerificationHistoryList(1, $(this).val());
});

$(document).on("click", ".prev-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchEmailVerificationHistoryList(currentPage - 1, $(".items-per-page").val());
});

$(document).on("click", ".next-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchEmailVerificationHistoryList(currentPage + 1, $(".items-per-page").val());
});

$(document).on('input', '.search-query', function () {
    var pageSize = $(".items-per-page").val();
    fetchEmailVerificationHistoryList(1, pageSize);
})

$(document).on("click", ".page-index", function () {
    var page = parseInt($(this).data("page"));
    var pageSize = $(".items-per-page").val();
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

$(document).ready(function () {
    renderPieChart();
})

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