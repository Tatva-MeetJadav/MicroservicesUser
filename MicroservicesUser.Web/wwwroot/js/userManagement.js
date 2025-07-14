var currentRow;
var blockId;
var deleteId;
var columnNameForSorting = "";
var orderOfSorting = "asc";
function fetchUserList(page, pageSize) {
    var searchQuery = $('input[name="searchQueryForUsers"]').val();
    var paginationDTO =
    {
        searchQuery: searchQuery,
        currentPage: page,
        columnNameForSorting: columnNameForSorting,
        orderOfSorting: orderOfSorting,
        pageSize: pageSize,
    }
    $.ajax({
        url: "/User/GetUserList",
        type: "POST",
        contentType: "application/json",
        traditional: true,
        data: JSON.stringify(paginationDTO),
        success: function (response) {
            $(".user-list-container").html(response);
        },
    });
}


$(document).on("change", ".items-per-page", function () {
    fetchUserList(1, $(this).val());
});

$(document).on("click", ".prev-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchUserList(currentPage - 1, $(".items-per-page").val());
});

$(document).on("click", ".next-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchUserList(currentPage + 1, $(".items-per-page").val());
});

$(document).on('input', '.search-query', function () {
    var pageSize = $(".items-per-page").val();
    fetchUserList(1, pageSize);
})

$(document).on("click", ".page-index", function () {
    var page = parseInt($(this).data("page"));
    var pageSize = $(".items-per-page").val();
    fetchUserList(page, pageSize);
});

$(document).on('click', '.btn-view', function () {
    var button = $(this);
    var row = button.closest('tr');
    var userName = row.find('.user-details h6').text();
    var userEmail = row.find('td:nth-child(2)').text();
    var userPhone = row.find('td:nth-child(3)').text();
    var userStatus = row.find('td:nth-child(4) .status-badge').text().trim();
    var userAddress = row.find('.user-address').val();
    var userAvatar = row.find('.user-avatar').attr('src');
    var userJoiningDate = row.find('.user-joining-date').val();

    if (!userPhone) {
        userPhone = '—';
    }

    $('#modalUserName').text(userName);
    $('#modalUserEmail').text(userEmail);
    $('#modalUserPhone').text(userPhone);
    $('#modalUserStatus').text(userStatus).removeClass().addClass('user-profile-status ' + (userStatus === 'Active' ? 'online' : 'offline'));
    $('#modalUserAvatar').attr('src', userAvatar);
    $('#modalUserAddress').text(userAddress);
    $('#modalUserJoinDate').text(userJoiningDate);
});

$(document).on('click', '.btn-block-unblock', function () {
    var row = $(this).closest('tr');
    var id = $(this).data('id');
    currentRow = row;
    blockId = id;

});

$(document).on('click', '.btn-confirm-lock,.btn-confirm-unlock', function () {
    $.ajax({
        url: "/User/BlockUnblockUser",
        type: "POST",
        data: { id: blockId },
        success: function (response) {
            currentRow.html(response);
            $('#blockUserModal').modal('hide');
            $('#unblockUserModal').modal('hide');
        },
    })
});

$(document).on('click', '.btn-delete', function () {
    var row = $(this).closest('tr');
    var id = $(this).data('id');
    currentRow = row;
    deleteId = id;
});

$(document).on('click', '#confirmDeleteUserBtn', function () {
    $.ajax({
        url: "/User/DeleteUser",
        type: "POST",
        data: { id: deleteId },
        success: function (response) {
            currentRow.html(response);
            $('#deleteUserModal').modal('hide');
        },
    })
});


$(document).on("click", ".sort-filter", function () {
    columnNameForSorting = $(this).data("column");
    if (orderOfSorting == "asc") {
        orderOfSorting = "dsc";
    } else {
        orderOfSorting = "asc";
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    var pageSize = $(".items-per-page").val();
    fetchUserList(currentPage, pageSize);
});