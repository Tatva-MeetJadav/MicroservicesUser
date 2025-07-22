var columnNameForFilter = "";
var filterValue = "";
var fromDate;
var toDate;
function fetchSupportList(page, pageSize) {
    var searchQuery = $('input[name="searchQueryForSupport"]').val();
    var paginationDTO =
    {
        searchQuery: searchQuery,
        currentPage: page,
        pageSize: pageSize,
    }
    var HelpAndSupportRequestDto =
    {
        paginationDTO: paginationDTO,
        categoryType: filterValue,
        fromDate: fromDate,
        toDate: toDate
    };
    $.ajax({
        url: "/HelpAndSupport/GetHelpAndSupportList",
        type: "POST",
        contentType: "application/json",
        traditional: true,
        data: JSON.stringify(HelpAndSupportRequestDto),
        success: function (response) {
            $(".load-admin-support-list").html(response);
        },
    });
}

$(document).on("change", ".items-per-page", function () {
    fetchSupportList(1, $(this).val());
});

$(document).on("click", ".prev-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchSupportList(currentPage - 1, $(".items-per-page").val());
});

$(document).on("click", ".next-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchSupportList(currentPage + 1, $(".items-per-page").val());
});

$(document).on('input', '.search-query', function () {
    var pageSize = $(".items-per-page").val();
    fetchSupportList(1, pageSize);
})

$(document).on("click", ".page-index", function () {
    var page = parseInt($(this).data("page"));
    var pageSize = $(".items-per-page").val();
    fetchSupportList(page, pageSize);
});
$(document).on('change', '.column-filter', function () {
    var column = $(this).data('column');
    var value = $(this).val();
    columnNameForFilter = column;
    filterValue = value;
    fetchSupportList(1, $(".items-per-page").val());
});

$(document).on('change', '.date-filter', function () {
    var currentFromDate = $('input[name="fromDate"]').val();
    var currentToDate = $('input[name="toDate"]').val();
    var today = getTodayDate();
    if (currentFromDate && currentFromDate > today) {
        toastr.error("From date cannot be in the future");
        $(this).val("");
    }
    if (currentToDate && currentToDate > today) {
        toastr.error("To date cannot be in the future");
        $(this).val("");
    }
    if (currentFromDate && currentToDate && currentFromDate > currentToDate) {
        toastr.error("From date cannot be greater than To date");
        $(this).val("");
    }
});

$(document).on("click", ".support-list-filter", function (e) {
    e.preventDefault();
    var tempFromDate = $('input[name="fromDate"]').val();
    var tempToDate = $('input[name="toDate"]').val();
    if (tempFromDate && tempToDate && tempFromDate > tempToDate) {
        toastr.error("From date cannot be greater than To date");
        return;
    }
    fromDate = tempFromDate;
    toDate = tempToDate;
    fetchSupportList(1, $(".items-per-page").val());
});

function getTodayDate() {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    return yyyy + '-' + mm + '-' + dd;
}

$(document).on("click", ".clear-all-filters", function () {
    $('input[name="searchQueryForSupport"]').val("");
    $('input[name="fromDate"]').val("");
    $('input[name="toDate"]').val("");
    $('select[name="category"]').val("");
    fromDate = "0001-01-01";
    toDate = "9999-12-31";
    filterValue = "";
    columnNameForFilter = "";
    fetchSupportList(1, $(".items-per-page").val());
});