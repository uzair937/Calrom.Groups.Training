window.onload = function () {
    var buttons = window.document.getElementsByName("Amount");
    for (var i = 0; i < buttons.length; i++) {
        if (buttons[i] !== undefined && buttons[i] !== null) {
            buttons[i].addEventListener("click", onBidItem);
        }
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

    var element = ;
    xhttp.open("POST", "/api/HttpBid/BidItem" + element, true);
    xhttp.send();
}