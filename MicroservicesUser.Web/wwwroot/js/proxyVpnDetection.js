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
        let emailSpan = $(this).find('span').last(); // Assuming email is in the last span
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


$('#selectAllUsers').on('change', function () {
    let isChecked = $(this).is(':checked');
    $('#dropdownMenu input[type="checkbox"]').prop('checked', isChecked);
});


$('#dropdownMenu input[type="checkbox"]').on('change', function () {
    if (!$(this).is('#selectAllUsers')) {
        let allChecked = $('#dropdownMenu input[type="checkbox"]').not('#selectAllUsers').length ===
            $('#dropdownMenu input[type="checkbox"]:checked').not('#selectAllUsers').length;
        $('#selectAllUsers').prop('checked', allChecked);
    }
});


function fetchProxyVpnDetectionHistoryList(page, pageSize) {
    var searchQuery = $('input[name="searchQueryForHistory"]').val();
    var paginationDTO =
    {
        searchQuery: searchQuery,
        currentPage: page,
        pageSize: pageSize,
    }
    var proxyVpnDetectionHistoryDTO =
    {
        userIds: currentlySelectedUserIds,
        paginationDTO: paginationDTO,
        connectionType: connectionType,
        riskStatus: riskLevel
    }
    $.ajax({
        url: "/ProxyVpnDetection/GetProxyVpnDetectionHistory",
        type: "POST",
        contentType: "application/json",
        traditional: true,
        data: JSON.stringify(proxyVpnDetectionHistoryDTO),
        success: function (response) {
            $(".proxy-vpn-detection-history-list").html(response);
        },
    });
}


$(document).on("change", ".items-per-page", function () {
    fetchProxyVpnDetectionHistoryList(1, $(this).val());
});

$(document).on("click", ".prev-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchProxyVpnDetectionHistoryList(currentPage - 1, $(".items-per-page").val());
});

$(document).on("click", ".next-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchProxyVpnDetectionHistoryList(currentPage + 1, $(".items-per-page").val());
});

$(document).on('input', '.search-query', function () {
    var pageSize = $(".items-per-page").val();
    fetchProxyVpnDetectionHistoryList(1, pageSize);
})

$(document).on("click", ".page-index", function () {
    var page = parseInt($(this).data("page"));
    var pageSize = $(".items-per-page").val();
    fetchProxyVpnDetectionHistoryList(page, pageSize);
});

$(document).on('change', '.risk-filter, .connection-filter', function () {
    var risk = $('.risk-filter').val();
    var connection = $('.connection-filter').val();
    connectionType = connection;
    riskLevel = risk;
    fetchProxyVpnDetectionHistoryList(1, $(".items-per-page").val());
});

$(document).on('click', '.view-detail-eye', function () {
    var id = parseInt($(this).next().val());
    $.ajax({
        url: "/ProxyVpnDetection/GetProxyVpnViewDetail",
        type: "GET",
        data: { id: id },
        success: function (response) {
            $(".proxy-vpn-view-detail-body").html(response);
            $('.view-detail-proxy-vpn-detection').modal('show');
        },
    });
});

$(document).ready(function () {
    renderPieChart();
})