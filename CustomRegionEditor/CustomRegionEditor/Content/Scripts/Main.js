window.onload = function () {
    addMainListeners();
}

function onSearch(e) {
    var url = $(".search-container").attr("data-searchurl");
    var searchTerm = document.getElementsByClassName("search-text-box")[0].value;

    if (searchTerm) {
        $.ajax({
            type: "POST",
            url: url + "?searchTerm=" + searchTerm,
            success: function (data, status, xhr) {
                if (data) {
                    $(".content-container").replaceWith(data);
                    addSearchListeners();
                }
            }
        });
    }
}

function onEdit(e) {
    var url = $(".table-header").attr("data-editurl");
    var regionId = this.parentNode.firstChild.innerHTML;

    if (regionId) {
        $.ajax({
            type: "POST",
            url: url + "?regionId=" + regionId,
            success: function (data, status, xhr) {
                if (data) {
                    $(".content-container").replaceWith(data);      //replaces all content/ search and edit
                    addEditListeners();                 //
                }
            }
        });
    }
}

function onDelete(e) {
    var url = $(".table-header").attr("data-deleteurl");
    var regionId = this.parentNode.firstChild.innerHTML;

    if (regionId) {
        $.ajax({
            type: "POST",
            url: url + "?regionId=" + regionId,
            success: onSearch,
        });
    }
} //Calls onSearch on finish to refresh list

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