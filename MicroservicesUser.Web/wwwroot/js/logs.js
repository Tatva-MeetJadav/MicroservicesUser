var columnNameForSorting = "";
var orderOfSorting = "desc";
var columnNameForFilter = "";
var filterValue = "";
function fetchLogs(page, pageSize) {
    var searchQuery = $('input[name="searchQueryForLogs"]').val();
    var paginationDTO =
    {
        searchQuery: searchQuery,
        currentPage: page,
        columnNameForSorting: columnNameForSorting,
        orderOfSorting: orderOfSorting,
        columnNameForFilter: columnNameForFilter,
        filterValue: filterValue,
        pageSize: pageSize,
    }
    $.ajax({
        url: "/Log/GetLogList",
        type: "POST",
        contentType: "application/json",
        traditional: true,
        data: JSON.stringify(paginationDTO),
        success: function (response) {
            $(".log-list-container").html(response);
        },
    });
}

$(document).on("change", ".items-per-page", function () {
    fetchLogs(1, $(this).val());
});

$(document).on("click", ".prev-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchLogs(currentPage - 1, $(".items-per-page").val());
});

$(document).on("click", ".next-page", function () {
    if ($(this).hasClass('disabled')) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }
    var currentPage = parseInt($(".pagination-info").data("current-page"));
    fetchLogs(currentPage + 1, $(".items-per-page").val());
});

$(document).on('click', '.search-query', function () {
    var pageSize = $(".items-per-page").val();
    fetchLogs(1, pageSize);
})

$(document).on("click", ".page-index", function () {
    var page = parseInt($(this).data("page"));
    var pageSize = $(".items-per-page").val();
    fetchLogs(page, pageSize);
});

$(document).on("click", ".sort-filter", function () {
    columnNameForSorting = $(this).data("column");
    if (orderOfSorting == "asc") {
        orderOfSorting = "dsc";
    } else {
        orderOfSorting = "asc";
    }
    var pageSize = $(".items-per-page").val();
    fetchLogs(1, pageSize);
});

$(document).on('click', '.btn-view', function () {
    var button = $(this);
    var exception = button.data('exception');
    var modal = $('#exceptionModal');
    modal.find('#exceptionText').text(exception || "No exception details available.");
});

$(document).on('change', '.column-filter', function () {
    var column = $(this).data('column');
    var value = $(this).val();
    columnNameForFilter = column;
    filterValue = value;
    fetchLogs(1, $(".items-per-page").val());
});

$(document).ready(function () {
    const $input = $('#searchQueryForLogs');
    const $suggestionList = $('#suggestionList');

    // Sample static suggestions array (replace with AJAX if needed)
    const suggestions = [
        "ServiceA - Error",
        "ServiceB - Warning",
        "Machine123",
        "ServiceC - Debug",
        "Error connecting to DB",
        "ServiceD - Information",
        "Machine456"
    ];

    function hideSuggestions() {
        $suggestionList.removeClass('show');
        $input.attr('aria-expanded', 'false');
    }

    function showSuggestions() {
        $suggestionList.addClass('show');
        $input.attr('aria-expanded', 'true');
    }

    $input.on('input', function () {
        const query = $(this).val().toLowerCase().trim();
        if (!query) {
            hideSuggestions();
            $suggestionList.empty();
            return;
        }

        const filtered = suggestions.filter(item => item.toLowerCase().includes(query));

        if (filtered.length === 0) {
            hideSuggestions();
            $suggestionList.empty();
            return;
        }

        const itemsHtml = filtered.map(item =>
            `<li><button type="button" class="dropdown-item">${item}</button></li>`
        ).join('');

        $suggestionList.html(itemsHtml);
        showSuggestions();
    });

    // When a suggestion is clicked
    $suggestionList.on('click', '.dropdown-item', function () {
        const selectedText = $(this).text();
        $input.val(selectedText);
        hideSuggestions();
    });

    // Hide suggestions on clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest($input).length && !$(e.target).closest($suggestionList).length) {
            hideSuggestions();
        }
    });
});
