$('#showResultBtn').on('click', function () {
    let selectedUserIds = [];
    $('.dashboard-dropdown-user-id').each(function () {
        let checkbox = $(this).closest('li').find('input[type="checkbox"]');
        if (checkbox.is(':checked')) {
            selectedUserIds.push(parseInt($(this).val()));
        }
    });
    $.ajax({
        url: '/ProxyVpnDetection/GetDashboardData',
        type: 'POST',
        data: { userIds: selectedUserIds },
        success: function (response) {
            $('.proxy-vpn-dashboard-data').html(response);
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
