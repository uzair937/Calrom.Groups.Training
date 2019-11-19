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
    var ItemId = window.document.getElementById("ItemID").value;
    
    // Response returns
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            window.location.reload();
        }
    };

    var element = this.value;
    xhttp.open("POST", "/api/HttpBid/BidItem?value=" + element + "&itemId=" + ItemId, true);
    xhttp.send();
}