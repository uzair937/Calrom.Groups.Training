window.onload = function () {
    var followButton = window.document.getElementsByClassName("follow-button")[0];
    if (followButton !== undefined && followButton !== null) {
        followButton.addEventListener("click", onFollowUser);
    }
}

function onBidItem(e) {
    var xhttp = new XMLHttpRequest();

    // Response returns
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var response = JSON.parse(this.response);
        }
    };

    var userToFollowId = document.getElementById("userId").value;
    xhttp.open("POST", "/api/HttpBidController/BidItem?userId=" + userToFollowId, true);
    xhttp.send();
}