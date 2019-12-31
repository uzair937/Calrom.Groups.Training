﻿window.onload = function () {
    addMainListeners();
    $('#helper-tooltip').tooltip();
}

function onSearch(e) {
    var url = $(".search-container").attr("data-searchurl");
    var searchTerm = document.getElementsByClassName("search-text-box")[0].value;
    var filter = "none";
    if ($(".search-filters-container").val() === "contains-airport") {
        filter = "airport";
    }
    if ($(".search-filters-container").val() === "contains-city") {
        filter = "city";
    }
    if ($(".search-filters-container").val() === "contains-state") {
        filter = "state";
    }
    if ($(".search-filters-container").val() === "contains-country") {
        filter = "country";
    }
    if ($(".search-filters-container").val() === "contains-region") {
        filter = "region";
    }
    if ($(".search-filters-container").val() === "for-region") {
        filter = "regionFilter";
    }
    if ($(".search-filters-container").val() === "for-country") {
        filter = "countryFilter";
    }
    if ($(".search-filters-container").val() === "for-state") {
        filter = "stateFilter";
    }
    if ($(".search-filters-container").val() === "for-city") {
        filter = "cityFilter";
    }
    if (searchTerm) {
        $.ajax({
            type: "POST",
            url: url + "?searchTerm=" + searchTerm + "&filter=" + filter,
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
    var regionId = $(this).parent().parent().attr("searchid");
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
    e.stopPropagation();
    $(this).css("color", "red");
    if (this !== undefined && this !== null) {
        this.addEventListener("click", onConfirmDelete);
        document.addEventListener("click", clearDelete);
    }
}

function onConfirmDelete(e) {
    e.stopPropagation();
    var url = $(".table-header").attr("data-deleteurl");
    var regionId = $(this).parent().parent().attr("searchid");

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
    var regionId = $(".model-id").attr("modelId");
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
    e.stopPropagation();
    $(this).css("color", "red");
    if (this !== undefined && this !== null) {
        this.addEventListener("click", entryConfirmDelete);
        document.addEventListener("click", clearDelete);
    }
}

function clearDelete() {
    var deleteButtons = window.document.getElementsByClassName("delete-button");
    for (var x = 0; x < deleteButtons.length; x++) {
        deleteButtons[x].removeEventListener("click", onConfirmDelete);
        deleteButtons[x].removeEventListener("click", entryConfirmDelete);
        $(deleteButtons[x]).css("color", "white");
    }
}

function entryConfirmDelete(e) {
    e.stopPropagation();
    var url = $(".table-header").attr("data-deleteentryurl");
    var entryId = $(this).parent().parent().attr("entryId");
    if (entryId) {
        $.ajax({
            type: "POST",
            url: url + "?entryId=" + entryId,
            success: refreshEdit,
        });
    }
} //removes an entry from the group region

function addEntry(e) {
    var container = $(".airport-text-box");
    var url = $(".table-header").attr("data-addurl");
    var regionId = $(".model-id").attr("modelId");
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
    if (value === "UK, IRELAND &amp; C.I" || value === "UK, IRELAND ") {
        value = "GBR";
    }
    if (value !== "" && value !== undefined) {
        addRegionEntry(url, value, type, regionId);
    }
} //picks largest entry type, calls onSearch on finish to refresh list

function addRegionEntry(url, value, type, regionId) {
    var newClass = new AddRegionViewModel(value, type, regionId);

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(newClass),

        success: function (data) {
            if (data) {
                $(".edit-info-container").html(data);      //replaces all content/ search and edit
                addEditListeners();
            }
        }
    });
} //runs the ajax to add an entry

function saveChanges2(e) {
    e.preventDefault();
    e.stopPropagation();
    var newName = $(".model-name").val();
    var newDescription = $(".model-description").val();
    var regionId = $(".model-id").attr("modelId");
    var url = $(".info-table-header").attr("data-savechangesurl");
    $.ajax({
        type: "POST",
        url: url + "?name=" + newName + "&description=" + newDescription + "&regionId=" + regionId ,
        success: function (data, status, xhr) {
            if (data) {
                $(".content-container").html(data);      //replaces all content/ search and edit
                addEditListeners();
            }
        }
    });
}   //saves new name and description for region

function AddRegionViewModel(value, type, regionId) {
    this.Entry = value;
    this.Type = type;
    this.RegionId = regionId;
}

function RegionViewModel(name, description, regionId) {
    this.Name = name;
    this.Description = description;
    this.RegionId = regionId;
}

function saveChanges(e) {
    debugger;
    e.preventDefault();
    e.stopPropagation();
    var newName = $(".model-name").val();
    var newDescription = $(".model-description").val();
    var regionId = $(".model-id").attr("modelId");
    var url = $(".info-table-header").attr("data-savechangesurl");
    var newClass = new RegionViewModel(newName, newDescription, regionId);
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(newClass),
        success: function (data, status, xhr) {
            if (data) {
                $(".content-container").html(data);      //replaces all content/ search and edit
                addEditListeners();
            }
        }
    });
}   //saves new name and description for region

function addMainListeners() {
    var searchButton = window.document.getElementsByClassName("search-button")[0];
    var searchBox = document.getElementsByClassName("search-text-box")[0];
    var addRegionButton = window.document.getElementsByClassName("add-region-button")[0];

    if (searchButton !== undefined && searchButton !== null) {
        searchButton.addEventListener("click", onSearch);
    }
    if (searchBox !== undefined && searchBox !== null) {
        searchBox.addEventListener("keyup", onSearchKey);
    }
    if (addRegionButton !== undefined && addRegionButton !== null) {
        addRegionButton.addEventListener("click", onRegionAdd);
    }
} //adds sidebar always-on listeners

function onSearchKey(e) {
    if (e.keyCode === 13) {
        onSearch();
    }
}

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
    var regionBox = document.getElementsByName("region-box")[0];
    var countryBox = document.getElementsByName("country-box")[0];
    var stateBox = document.getElementsByName("state-box")[0];
    var cityBox = document.getElementsByName("city-box")[0];
    var airportBox = document.getElementsByName("airport-box")[0];

    for (var i = 0, len = deleteButtons.length; i < len; i++) {
        deleteEntryListeners(deleteButtons[i]);
    }
    if (addButton !== undefined && addButton !== null) {
        addButton.addEventListener("click", addEntry);
    }
    if (saveButton !== undefined && saveButton !== null) {
        saveButton.addEventListener("click", saveChanges);
    }

    if (regionBox !== undefined && regionBox !== null) {
        regionBox.addEventListener("input", onTextChange);
    }
    if (countryBox !== undefined && countryBox !== null) {
        countryBox.addEventListener("input", onTextChange);
    }
    if (stateBox !== undefined && stateBox !== null) {
        stateBox.addEventListener("input", onTextChange);
    }
    if (cityBox !== undefined && cityBox !== null) {
        cityBox.addEventListener("input", onTextChange);
    }
    if (airportBox !== undefined && airportBox !== null) {
        airportBox.addEventListener("input", onTextChange);
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

function onTextChange() {
    var url = $(".add-region-row").attr("data-autourl");
    var type = $(this).attr("autotype");
    var text = $(this).val();
    var currentNode = this;
    if (type) {
        $.ajax({
            type: "POST",
            url: url + "?type=" + type + "&text=" + text,
            success: function (data, status, xhr) {
                if (data) {
                    $(currentNode).next().html(data);
                    addAutoCompleteListeners();
                }
            }
        });
    }
} //sets up autocomplete

function addAutoCompleteListeners() {
    var autoText = window.document.getElementsByClassName("single-suggestion");
    for (var x = 0; x < autoText.length; x++) {
        if (autoText[x] !== undefined && autoText[x] !== null) {
            autoText[x].addEventListener("click", onTextSelect);
        }
    }
    document.addEventListener("click", removeAutoTab);
}

function onTextSelect() {
    var textBox = $(this).parent().parent().parent().prev();
    textBox.val(this.firstChild.innerHTML);
    removeAutoTab();
}

function removeAutoTab() {
    var autoTab = window.document.getElementsByClassName("autocomplete-container flex-auto");

    for (var x = 0; x < autoTab.length; x++) {
        autoTab[x].innerHTML = "";
    }
}