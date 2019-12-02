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
                    addEditListeners();                 
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

function refreshEdit(e) {
    var url = $(".table-header").attr("data-editurl");
    var regionId = $(".table-header").attr("regionId");
    if (regionId) {
        $.ajax({
            type: "POST",
            url: url + "?regionId=" + regionId,
            success: function (data, status, xhr) {
                if (data) {
                    $(".content-container").replaceWith(data);      //replaces all content/ search and edit
                    addEditListeners();
                }
            }
        });
    }
}

function entryDelete(e) {
    var url = $(".table-header").attr("data-deleteentryurl");
    var regionId = $(".table-header").attr("regionId");
    var entryId = this.parentNode.firstChild.innerHTML;
    if (entryId) {
        $.ajax({
            type: "POST",
            url: url + "?entryId=" + entryId + "&regionId=" + regionId,
            success: refreshEdit,
        });
    }
}


function onAdd(e) {
    var container = $(".airport-text-box");
    var url = $(".table-header").attr("data-addurl");
    var value, type = "";
    if (container.value !== "") {
        type = "airport";
        value = container.value;
    }
    container = container.prev();
    if (container.value !== "") {
        type = "city";
        value = container.value;
    }
    container = container.prev();
    if (container.value !== "") {
        type = "state";
        value = container.value;
    }
    container = container.prev();
    if (container.value !== "") {
        type = "country";
        value = container.value;
    }
    container = container.prev();
    if (container.value !== "") {
        type = "region";
        value = container.value;
    }
    if (value !== "") {
        addRegion(url, value, type);
    }
} //Calls onSearch on finish to refresh list

function addRegion(url, value, type) {
    $.ajax({
        type: "POST",
        url: url + "?entry=" + value + "&type=" + type,
        success: onSearch,
    });
}

function addMainListeners() {
    var searchButton = window.document.getElementsByClassName("search-button")[0];

    if (searchButton !== undefined && searchButton !== null) {
        searchButton.addEventListener("click", onSearch);
    }
}

function addSearchListeners() {
    var editButtons = window.document.getElementsByClassName("edit-button");
    var deleteButtons = window.document.getElementsByClassName("delete-button");

    editButtons.forEach(editButtonListeners);
    deleteButtons.forEach(deleteButtonListeners);
}

function addEditListeners() {
    var deleteButtons = window.document.getElementsByClassName("delete-button");
    var addButton = window.document.getElementsByClassName("add-button")[0];

    deleteButtons.forEach(deleteEntryListeners);
    if (addButton !== undefined && addButton !== null) {
        addButton.addEventListener("click", addRegion);
    }
}

function deleteEntryListeners(item) {
    if (item !== undefined && item !== null) {
        item.addEventListener("click", entryDelete);
    }
}

function deleteButtonListeners(item) {
    if (item !== undefined && item !== null) {
        item.addEventListener("click", onDelete);
    }
}

function editButtonListeners(item) {
    if (item !== undefined && item !== null) {
        item.addEventListener("click", onEdit);
    }
}