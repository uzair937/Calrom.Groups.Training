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
    var regionId = $(this).parent().parent().attr("searchId");
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
    var regionId = $(this).parent().parent().attr("searchId");

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
                    $(".content-container").html(data);      //replaces all content/ search and edit
                    addEditListeners();
                }
            }
        });
    }
} //refreshes the edit tab using the in-html stored region ID

function entryDelete(e) {
    var url = $(".table-header").attr("data-deleteentryurl");
    var regionId = $(".table-header").attr("regionId");
    var entryId = $(".single-region-entry").attr("entryId");
    if (entryId) {
        $.ajax({
            type: "POST",
            url: url + "?entryId=" + entryId + "&regionId=" + regionId,
            success: refreshEdit,
        });
    }
} //removes an entry from the group region

function addEntry(e) {
    var container = $(".airport-text-box");
    var url = $(".table-header").attr("data-addurl");
    var regionId = $(".table-header").attr("regionId");
    var value = "";
    var type = "";
    if (container.val() !== "" && container.val() !== undefined) {
        type = "airport";
        value = container.val();
    }
    container = $(".city-text-box");
    if (container.val() !== "" && container.val() !== undefined) {
        type = "city";
        value = container.val();
    }
    container = $(".state-text-box");
    if (container.val() !== "" && container.val() !== undefined) {
        type = "state";
        value = container.val();
    }
    container = $(".country-text-box");
    if (container.val() !== "" && container.val() !== undefined) {
        type = "country";
        value = container.val();
    }
    container = $(".region-text-box");
    if (container.val() !== "" && container.val() !== undefined) {
        type = "region";
        value = container.val();
    }
    if (value !== "" && value !== undefined) {
        addRegionEntry(url, value, type, regionId);
    }
} //picks largest entry type, calls onSearch on finish to refresh list

function addRegionEntry(url, value, type, regionId) {
    $.ajax({
        type: "POST",
        url: url + "?entry=" + value + "&type=" + type + "&regionId=" + regionId,
        success: refreshEdit,
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
        addButton.addEventListener("click", addEntry);
    }
    if (saveButton !== undefined && saveButton !== null) {
        saveButton.addEventListener("click", saveChanges);
    }

    addAutoComplete();
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

function addAutoComplete() {
    $('.region-text-box').autocomplete({
        source: '/home/GetRegions'
    });
    $('.country-text-box').autocomplete({
        source: '/home/GetCountries'
    });
    $('.state-text-box').autocomplete({
        source: '/home/GetStates'
    });
    $('.city-text-box').autocomplete({
        source: '/home/GetCities'
    });
    $('.airport-text-box').autocomplete({
        source: '/home/GetAirports'
    });
} //sets up autocomplete