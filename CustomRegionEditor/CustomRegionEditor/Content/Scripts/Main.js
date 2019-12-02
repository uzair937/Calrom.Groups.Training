window.onload = function () {
    addMainListeners();
}

function onSearch(e) {
    var url = $(".search-container").attr("data-searchurl");
    var searchTerm = document.getElementsByClassName("search-text-box")[0].value;

    if (searchUser) {
        $.ajax({
            type: "POST",
            url: url + "?searchTerm=" + searchTerm,
            success: function (data, status, xhr) {
                if (data) {
                    $(".search-results-container").replaceWith(data);
                    addSearchListeners();
                }
            }
        });
    }
}

function onEdit(e) {
    var url = $(".search-container").attr("data-searchurl");
    var searchTerm = document.getElementsByClassName("search-text-box")[0].value;

    if (searchUser) {
        $.ajax({
            type: "POST",
            url: url + "?searchTerm=" + searchTerm,
            success: function (data, status, xhr) {
                if (data) {
                    $(".search-results-container").replaceWith(data);
                    addSearchListeners();
                }
            }
        });
    }
}

function onDelete(e) {
    var url = $(".search-container").attr("data-searchurl");
    var searchTerm = document.getElementsByClassName("search-text-box")[0].value;

    if (searchUser) {
        $.ajax({
            type: "POST",
            url: url + "?searchTerm=" + searchTerm,
            success: function (data, status, xhr) {
                if (data) {
                    $(".search-results-container").replaceWith(data);
                    addSearchListeners();
                }
            }
        });
    }
}

function addMainListeners() {
    var searchButton = window.document.getElementsByClassName("search-button")[0];

    if (searchButton !== undefined && searchButton !== null) {
        searchButton.addEventListener("click", onSearch);
    }
}

function addSearchListeners() {
    var editButton = window.document.getElementsByClassName("edit-button")[0];
    var deleteButton = window.document.getElementsByClassName("delete-button")[0];

    if (editButton !== undefined && editButton !== null) {
        editButton.addEventListener("click", onEdit);
    }
    if (deleteButton !== undefined && deleteButton !== null) {
        deleteButton.addEventListener("click", onDelete);
    }
}