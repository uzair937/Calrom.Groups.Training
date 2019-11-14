window.onload = function () {
    var borkButton = window.document.getElementById("bork-button");
    if (borkButton !== undefined && borkButton !== null) {
        borkButton.addEventListener("click", onBork);
    }
}

function onBork(e) {
    var xhttp = new XMLHttpRequest();
    var borkText = document.getElementById("BorkText").value;

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            window.location.reload();
        }
    };

    xhttp.open("POST", "/api/HttpHome/AddBork?borkText=" + borkText, true);
    xhttp.send();
}