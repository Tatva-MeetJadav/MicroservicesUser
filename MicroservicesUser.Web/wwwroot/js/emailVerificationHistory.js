var columnNameForSorting = "CreatedAt";
var orderOfSorting = "desc";
var columnNameForFilter = ""
var filterValue = "False";

function fetchEmailVerificationHistoryList(page, pageSize) {
    var searchQuery = $('input[name="searchQueryForHistory"]').val();
    var paginationDTO =
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
        data: JSON.stringify(paginationDTO),
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
    var checkboxValue = $(this).prop("checked");
    if (checkboxValue) {
        filterValue = "True"
    }
    else {
        filterValue = "False"
    }
    var pageSize = $(".items-per-page").val();
    fetchEmailVerificationHistoryList(1, pageSize);
});

$(document).on("click", ".export-history-list", function () {
    var searchQuery = $('input[name="searchQueryForHistory"]').val();
    var totalItems = parseInt($(".pagination-info").data("total-items"));
    var paginationDTO = {
        totalItems: totalItems,
        searchQuery: searchQuery,
        columnNameForSorting: columnNameForSorting,
        orderOfSorting: orderOfSorting,
        columnNameForFilter: columnNameForFilter,
        filterValue: filterValue
    };

    $.ajax({
        url: "/History/ExportEmailVerificationHistoryList",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(paginationDTO),
        success: function (response) {
            var byteArray = new Uint8Array(atob(response.fileContents).split("").map(function (c) { return c.charCodeAt(0); }));
            var blob = new Blob([byteArray], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            var link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = response.fileName;
            link.click(); 
        },
        error: function (error) {
            alert("Error exporting data: " + error.responseText);
        }
    });

});