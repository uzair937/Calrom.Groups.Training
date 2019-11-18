window.onload = function () {
    addListeners();
}

function onBork(e) {
    var url = $(".bork-timeline").attr("data-newborkurl");
    var borkText = document.getElementById("BorkText").value;
    $.ajax({
        type: "POST",
        url: url + "?borkText=" + borkText,
        success: function (data, status, xhr) {
            if (data) {
                $(".bork-timeline > .bork-container:last").remove();
                $(".bork-timeline > .bork-container:first").before(data);
                $(".all-bork-timeline > .bork-container:last").remove();
                $(".all-bork-timeline > .bork-container:first").before(data);
                $("#BorkText")[0].value = "";
            }
        }
    });
}

function changePage(e) {
    var url = $(".page-container").attr("data-newpageurl");
    var pageNum = this.attributes.pagevalue.value;

    $.ajax({
        type: "POST",
        url: url + "?pageNum=" + pageNum,
        success: function (data, status, xhr) {
            if (data) {
                var newTimeline = $(data).children(".bork-timeline");
                $(".bork-timeline").replaceWith(newTimeline);
                addListeners();
            }
        }
    });
}

function addListeners() {
    var borkButton = window.document.getElementById("bork-button");
    var prevButton = window.document.getElementById("prev-button");
    var nextButton = window.document.getElementById("next-button");

    if (borkButton !== undefined && borkButton !== null) {
        borkButton.addEventListener("click", onBork);
    }

    if (prevButton !== undefined && prevButton !== null) {
        prevButton.addEventListener("click", changePage);
    }

    if (nextButton !== undefined && nextButton !== null) {
        nextButton.addEventListener("click", changePage);
    }
}
