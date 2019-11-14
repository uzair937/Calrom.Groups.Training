window.onload = function () {
    var followButton = window.document.getElementsByClassName("follow-button")[0];
    if (followButton !== undefined && followButton !== null) {
        followButton.addEventListener("click", onFollowUser);
    }
}

function onFollowUser(e) {
    var xhttp = new XMLHttpRequest();

    // Response returns
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            xhttp.open("POST", "/Account/Account?userId=" + this.response, true);
            xhttp.send();
            window.location.reload();
        }
    };

    var userToFollowId = document.getElementById("userId").value;
    xhttp.open("POST", "/api/HttpAccount/FollowUser?userId=" + userToFollowId, true);
    xhttp.send();
}

