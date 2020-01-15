window.onload = function () {
    addMainListeners();
    $('#helper-tooltip').tooltip();
    loadAll();
}

function loadAll(e) {
    var url = $(".search-container").attr("data-searchurl");
    var searchTerm = "-all";
    var filter = "none";

    var searchForm = new SearchModel(filter, searchTerm);
    if (searchForm) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json",
            data: JSON.stringify(searchForm),
            success: function (data) {
                if (data) {
                    $(".content-container").html(data);
                    addSearchListeners();
                }
            },
        });
    }
} //searches for region group name matches

function onSearch(e) {
    var url = $(".search-container").attr("data-searchurl");
    var searchTerm = document.getElementsByClassName("search-text-box")[0].value;

    var searchForm = new SearchModel(searchTerm);
    if (searchTerm !== "") {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json",
            data: JSON.stringify(searchForm),
            success: function (data) {
                if (data) {
                    $(".content-container").html(data);
                    addSearchListeners();
                }
            },
        });
    }
    else {
        loadAll();
    }
} //searches for region group name matches

function SearchModel(searchText) {
    var self = this;
    self.Text = searchText;
}

function NewRegionEntry(value, type, regionId, name, description) {
    var self = this;
    self.Id = regionId;
    self.Entry = value;
    self.Type = type;
    self.Name = name;
    self.Description = description;
}

function IdForm(id, search) {
    var self = this;
    self.Id = id;
    if (search) {
        self.LastSearch = search;
    }
}

function SaveForm(name, description, id) {
    var self = this;
    self.Id = id;
    self.Name = name;
    self.Description = description;
}

function DeleteForm(id, name, description) {
    var self = this;
    self.EntryId = id;
    self.Name = name;
    self.Description = description;
}

function AutoCompleteForm(text, type) {
    var self = this;
    self.Type = type;
    self.Text = text;
}


function onEdit(e) {
    var url = $(".table-header").attr("data-editurl");
    var regionId = $(this).parent().parent().attr("regionId");

    var idForm = new IdForm(regionId);
    if (regionId) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json",
            data: JSON.stringify(idForm),
            success: function (data) {
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
    var regionId = $(this).parent().parent().attr("regionId");
    var regionName = $(this).parent().parent().attr("regionName");
    var lastSearch = document.getElementsByClassName("search-text-box")[0].value;
    if (lastSearch === undefined || lastSearch === "" ) {
        lastSearch = "-all";
    }
    var filter = "none";
    if ($(".search-filters-container").val() === "contains-airport") {
        filter = "airport";
    }
    else if ($(".search-filters-container").val() === "contains-city") {
        filter = "city";
    }
    else if ($(".search-filters-container").val() === "contains-state") {
        filter = "state";
    }
    else if ($(".search-filters-container").val() === "contains-country") {
        filter = "country";
    }
    else if ($(".search-filters-container").val() === "contains-region") {
        filter = "region";
    }
    else if ($(".search-filters-container").val() === "for-region") {
        filter = "regionFilter";
    }
    else if ($(".search-filters-container").val() === "for-country") {
        filter = "countryFilter";
    }
    else if ($(".search-filters-container").val() === "for-state") {
        filter = "stateFilter";
    }
    else if ($(".search-filters-container").val() === "for-city") {
        filter = "cityFilter";
    }
    var searchForm = new SearchModel(filter, lastSearch);
    var idForm = new IdForm(regionId, searchForm);

    if (idForm) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(idForm),
            contentType: "application/json",
            success: function (data) {
                if (data) {
                    $(".content-container").html(data);
                    addSearchListeners();
                }
                $(".alert-container").html('<th class="alert alert-warning" role="alert">Region '+ regionName +' Deleted</th>');
            }
        });
    }
} //Calls onSearch on finish to refresh list

function refreshEdit(e) {
    var url = $(".table-header").attr("data-editurl");
    var regionId = $(".model-id").attr("modelId");
    var idForm = new IdForm(regionId);

    if (idForm) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(idForm),
            contentType: "application/json",
            success: function (data) {
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
    var entry = $(this).parent().parent().attr("entryValue");
    var name = $(this).parent().parent().attr("entryName");
    name = name.charAt(0).toUpperCase() + name.toLowerCase().substring(1, name.length);

    var regionName = window.document.getElementsByClassName("model-name")[0].value;
    var description = window.document.getElementsByClassName("model-description")[0].value;
    var deleteForm = new DeleteForm(entry, regionName, description);

    if (deleteForm) {

        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(deleteForm),
            contentType: "application/json",
            success: function (data) {
                if (data) {
                    $(".content-container").html(data);      //replaces all content/ search and edit
                    addEditListeners();
                    $(".alert-container").html('<th class="alert alert-warning" role="alert">Entry ' + name +' Deleted</th>');
                }
            }
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
        var name = window.document.getElementsByClassName("model-name")[0].value;
        var description = window.document.getElementsByClassName("model-description")[0].value;
        var regionForm = new NewRegionEntry(value, type, regionId, name, description);

        value = value.charAt(0).toUpperCase() + value.toLowerCase().substring(1, value.length);
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json",
            data: JSON.stringify(regionForm),
            success: function (data) {
                if (data) {
                    $(".content-container").html(data);      //replaces all content/ search and edit
                    addEditListeners();
                    $(".alert-container").html('<th class="alert alert-success" role="alert">Entry ' + value + ' Added</th>');
                }
            }
        });
    }
} //picks largest entry type, calls onSearch on finish to refresh list

function saveChanges() {
    var name = window.document.getElementsByClassName("model-name")[0].value;
    var description = window.document.getElementsByClassName("model-description")[0].value;
    var regionId = $(".model-id").attr("modelId");
    var url = $(".info-table-header").attr("data-savechangesurl");
    var saveForm = new SaveForm(name, description, regionId);

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json",
        data: JSON.stringify(saveForm),
        success: function (data) {
            if (data) {
                $(".content-container").html(data);      //replaces all content/ search and edit
                addEditListeners();
                $(".alert-container").html('<th class="alert alert-success" role="alert">Saved Changes</th>');
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
        searchBox.addEventListener("input", onSearchChange);
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
    var autoCompleteForm = new AutoCompleteForm(text, type);

    if (type) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(autoCompleteForm),
            contentType: "application/json",
            success: function (data) {
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

function onSearchChange() {
    var url = $(".search-container").attr("data-autosearchurl");
    var text = $(this).val();
    var currentNode = this;
    var autoCompleteForm = new AutoCompleteForm(text);

    if (text) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(autoCompleteForm),
            contentType: "application/json",
            success: function (data) {
                if (data) {
                    $(".search-autocomplete").html(data);
                    addSearchCompleteListeners();
                }
            }
        });
    }
} //sets up autocomplete

function addSearchCompleteListeners() {
    var autoText = window.document.getElementsByClassName("single-suggestion");
    for (var x = 0; x < autoText.length; x++) {
        if (autoText[x] !== undefined && autoText[x] !== null) {
            autoText[x].addEventListener("click", onSearchSelect);
        }
    }
    document.addEventListener("click", removeAutoTab);
}

function onSearchSelect() {
    var textBox = $(".search-text-box");
    textBox.val(this.firstChild.innerHTML);
    removeAutoTab();
}