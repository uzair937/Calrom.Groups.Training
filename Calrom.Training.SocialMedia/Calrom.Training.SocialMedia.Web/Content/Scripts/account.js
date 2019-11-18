window.onload = function () {
    addListeners();
}

function onFollowUser(e) {
    var xhttp = new XMLHttpRequest();
    var userToFollowId = document.getElementById("userId").value;

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            xhttp.open("POST", "/Account/Account?userId=" + this.response, true);
            xhttp.send();
            window.location.reload();
        }
    };

    xhttp.open("POST", "/api/HttpAccount/FollowUser?userId=" + userToFollowId, true);
    xhttp.send();
}

function onUserClick(e) {
    var xhttp = new XMLHttpRequest();
    var userName = this.value;
    if (userName === "Select User") return;

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            window.location.href = ("/Account/Account?userId=" + this.response);
        }
    };

    xhttp.open("POST", "/api/HttpAccount/GetId?userName=" + userName, true);
    xhttp.send();
}

function searchBork(e) {
    var url = $(".search-container").attr("data-searchborkurl");
    var searchText = document.getElementById("SearchText").value;
    var userId = $(".background-shibe").attr("currentuserId");
    $.ajax({
        type: "POST",
        url: url + "?searchText=" + searchText + "&userId=" + userId,
        success: function (data, status, xhr) {
            if (data) {
                $(".search-container").replaceWith(data);
                addListeners();
            }
        }
    });
}

function addListeners() {
    var followButton = window.document.getElementsByClassName("follow-button")[0];
    var followedUserList = window.document.getElementsByClassName("followed-list")[0];
    var searchButton = window.document.getElementById("search-bork");

    if (followButton !== undefined && followButton !== null) {
        followButton.addEventListener("click", onFollowUser);
    }

    if (followedUserList !== undefined && followedUserList !== null) {
        followedUserList.addEventListener("change", onUserClick);
        var topItem = document.createElement('option');
        topItem.innerHTML += "Select User";
        topItem.setAttribute("selected", "selected");
        followedUserList.insertBefore(topItem, followedUserList.firstChild);
    }

    if (searchButton !== undefined && searchButton !== null) {
        searchButton.addEventListener("click", searchBork);
    }

}

