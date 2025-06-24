var columnNameForSorting = "";
var orderOfSorting = "asc";
var columnNameForFilter = ""
var filterValue = false;

$(document).ready(function () {
    fetchEmailVerificationHistoryList(1, 5);
});

function fetchEmailVerificationHistoryList(page, pageSize) {
    var searchQuery = $('input[name="searchQueryForHistory"]').val();
    var paginationVM =
    {
        searchQuery: searchQuery,
        currentPage: page,
        pageSize: pageSize,
        columnNameForSorting: columnNameForSorting,
        orderOfSorting: orderOfSorting,
        columnNameForFilter: columnNameForFilter,
        filterValue: filterValue
    }
    $.ajax({
        url: "/History/GetEmailVerificationHistoryList",
        type: "POST",
        contentType: "application/json",
        traditional: true,
        data: JSON.stringify(paginationVM),
        success: function (response) {
            $(".load-history-table").html(response);
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

$(document).on("click", ".sort-filter", function () {
    columnNameForSorting = $(this).data("column");
    if (orderOfSorting == "asc") {
        orderOfSorting = "dsc";
    } else {
        orderOfSorting = "asc";
    }

    var currentPage = parseInt($(".pagination-info").data("current-page"));
    var pageSize = $(".items-per-page").val();
    fetchEmailVerificationHistoryList(currentPage, pageSize);
});

$(document).on('click', '.valid-email-history-switch', function () {
    columnNameForFilter = "Valid";
    filterValue = $(this).prop("checked");
    var pageSize = $(".items-per-page").val();
    fetchEmailVerificationHistoryList(1, pageSize);
});