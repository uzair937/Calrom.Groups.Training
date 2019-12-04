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
                    $(".content-container").html(data);
                    addSearchListeners();
                }
            }
        });
    }
} //searches for region group name matches

function onEdit(e) {
    var url = $(".table-header").attr("data-editurl");
    var regionId = this.parentNode.firstChild.innerHTML;
    if (regionId) {
        $.ajax({
            type: "POST",
            url: url + "?regionId=" + regionId,
            success: function (data, status, xhr) {
                if (data) {
                    $(".content-container").html(data);      //replaces all content/ search and edit
                    addEditListeners();
                }
            }
        });
    }
}   //runs when the user clicks on the edit button, opens the group, shows entries

function onRegionAdd(e) {
    var url = $(".add-button-container").attr("data-newregionurl");
    if (url) {
        $.ajax({
            type: "POST",
            url: url,
            success: function (data, status, xhr) {
                if (data) {
                    $(".content-container").html(data);      //replaces all content/ search and edit
                    addEditListeners();
                }
            }
        });
    }
} //adds a new region to the db and opens the edit tab

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
} //refreshes the edit tab using the in-html stored region ID

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
} //removes an entry from the group region

function addRegion(e) {
    var container = $(".airport-text-box");
    var url = $(".table-header").attr("data-addurl");
    var regionId = $(".table-header").attr("regionId");
    var value = "";
    var type = "";
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
        addRegionEntry(url, value, type, regionId);
    }
} //picks largest entry type, calls onSearch on finish to refresh list

function addRegionEntry(url, value, type) {
    $.ajax({
        type: "POST",
        url: url + "?entry=" + value + "&type=" + type + "&regionId=" + regionId,
        success: onSearch,
    });
} //runs the ajax to add an entry

function saveChanges() {
    var newName = window.document.getElementsByClassName("model-name")[0].value;
    var newDescription = window.document.getElementsByClassName("model-description")[0].value;
    var regionId = window.document.getElementsByClassName("model-id")[0].innerHTML;
    var url = $(".info-table-header").attr("data-savechangesurl");
    $.ajax({
        type: "POST",
        url: url + "?name=" + newName + "&description=" + newDescription + "&regionId=" + regionId,
        success: refreshEdit,
    });
}   //saves new name and description for region

function addMainListeners() {
    var searchButton = window.document.getElementsByClassName("search-button")[0];
    var addRegionButton = window.document.getElementsByClassName("add-region-button")[0];

    if (searchButton !== undefined && searchButton !== null) {
        searchButton.addEventListener("click", onSearch);
    }
    if (addRegionButton !== undefined && addRegionButton !== null) {
        addRegionButton.addEventListener("click", onRegionAdd);
    }
} //adds sidebar always-on listeners

function addSearchListeners() {
    var editButtons = window.document.getElementsByClassName("edit-button");
    var deleteButtons = window.document.getElementsByClassName("delete-button");

    for (var i = 0, len = editButtons.length; i < len; i++) {
        editButtonListeners(editButtons[i]);
    }
    for (var i = 0, len = deleteButtons.length; i < len; i++) {
        deleteButtonListeners(deleteButtons[i]);
    }
}   //adds listeners to the buttons in the search view

function addEditListeners() {
    var deleteButtons = window.document.getElementsByClassName("delete-button");
    var addButton = window.document.getElementsByClassName("add-button")[0];
    var saveButton = window.document.getElementsByClassName("save-changes-button")[0];

    for (var i = 0, len = deleteButtons.length; i < len; i++) {
        deleteEntryListeners(deleteButtons[i]);
    }
    if (addButton !== undefined && addButton !== null) {
        addButton.addEventListener("click", addRegion);
    }
    if (saveButton !== undefined && saveButton !== null) {
        saveButton.addEventListener("click", saveChanges);
    }
}   //adds listeners to the add/save/delete buttons in edit view

function deleteEntryListeners(item) {
    if (item !== undefined && item !== null) {
        item.addEventListener("click", entryDelete);
    }
} //adds listeners

function deleteButtonListeners(item) {
    if (item !== undefined && item !== null) {
        item.addEventListener("click", onDelete);
    }
} //adds listeners

function editButtonListeners(item) {
    if (item !== undefined && item !== null) {
        item.addEventListener("click", onEdit);
    }
} //adds listeners