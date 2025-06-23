function fetchEmailVerificationHistoryList(page, pageSize) {
    var searchQuery = $('input[name="searchQueryForEmailHistory"]').val();
    $.ajax({
        url: "/History/GetEmailVerificationHistoryList",
        type: "GET",
        data: {
            searchQuery: searchQuery,
            page: page,
            pageSize: pageSize,
        },
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

$(document).ready(function () {
    fetchEmailVerificationHistoryList(1, 5);
});
