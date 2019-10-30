window.addEventListener('load', run);
var checknum = 0;
var dragSrcEl = null;
var dragOn;
var dropOn;

function checkPosition() {}

checkPosition.prototype.aboveOrBelow = function (dragSrcElt) {
    for (var i = 0; i < event.currentTarget.parentNode.children.length - 1; i++) {
        if (event.currentTarget === event.currentTarget.parentNode.children[i]) window.dropOn = i;
        if (dragSrcElt === event.currentTarget.parentNode.children[i]) window.dragOn = i;
    }
}

function run() {
    var el = document.getElementById("enter-task");
    if (!el) return;
    var delel = document.getElementById("delete-task");
    var botel = document.getElementById("bottom-list");
    addDnDHandlerBottom(botel);
    el.addEventListener("submit", addTask);
    delel.addEventListener("submit", delTask);
}

function delTask() {
    event.preventDefault();
    var delList = document.getElementsByClassName("deletable");
    if (delList) {
        for (var i = delList.length - 1; i >= 0; i--) delList[i].remove();
    }
}

function addTask() {
    event.preventDefault();
    var li = document.createElement("div");
    var textelement = document.createElement("p");
    var inputValue = document.getElementById("input-task").value;
    var check = document.createElement('input');
    var t = document.createTextNode(inputValue);
    li.setAttribute("draggable", "true");
    li.setAttribute("id", "box" + checknum);
    check.setAttribute("type", "checkbox");
    textelement.appendChild(t);
    textelement.setAttribute("class", "task-text");
    li.appendChild(check);
    li.appendChild(textelement);
    if (inputValue === '') alert("Enter a task");
    else {
        var botel = document.getElementById("bottom-list");
        botel.insertAdjacentElement('beforebegin', li);
    }
    document.getElementById("input-task").value = '';

    eC = li.firstChild;
    if (li) {
        var eT = li.getElementsByClassName('task-text')[0];
        eC.addEventListener("click", checkBox);
        eT.addEventListener("click", editText);
        addDnDHandlers(li);
    }
    checknum++;
}

function addDnDHandlers(li) {
    li.addEventListener("dragstart", drag);
    li.addEventListener("dragover", dragover);
    li.addEventListener("drop", drop);
}

function addDnDHandlerBottom(li) {
    li.addEventListener("dragover", dragover);
    li.addEventListener("drop", drop);
}

function dragover() {
    event.preventDefault();
    for (var i = 0; i < event.currentTarget.parentNode.children.length - 1; i++) {
        event.currentTarget.parentNode.children[i].setAttribute("style", "border: 0px")
        event.currentTarget.parentNode.children[i].setAttribute("style", "border-bottom: 3px solid black")
    }
    event.currentTarget.setAttribute("style", "border-bottom: 3px solid blue;")
    event.currentTarget.parentNode.children[event.currentTarget.parentNode.children.length-1].setAttribute("style", "border-bottom: 0;");
}

function drag() {
    dragSrcEl = event.currentTarget;
}

function drop() {
    event.preventDefault();
    if (event.stopPropagation) event.stopPropagation();
    var dropProto = new checkPosition();
    dropProto.aboveOrBelow(dragSrcEl);
    if (dragSrcEl != event.currentTarget) {
        if (window.dropOn - window.dragOn > 0) {
            event.currentTarget.parentNode.removeChild(dragSrcEl);
            var dropHTML = dragSrcEl;
            event.currentTarget.insertAdjacentElement('afterend', dropHTML);
            var dropElem = event.currentTarget.previousSibling;
            addDnDHandlers(dropElem);
            if (dropElem.className === "deletable") {
                dropElem.firstChild.checked = true;
            }
            for (var i = 0; i < event.currentTarget.parentNode.children.length - 1; i++) {
                event.currentTarget.parentNode.children[i].setAttribute("style", "border: 0px")
                event.currentTarget.parentNode.children[i].setAttribute("style", "border-bottom: 3px solid black")
            }
        }
        else {
            event.currentTarget.parentNode.removeChild(dragSrcEl);
            var dropHTML = dragSrcEl;
            event.currentTarget.insertAdjacentElement('beforebegin', dropHTML);
            var dropElem = event.currentTarget.previousSibling;
            addDnDHandlers(dropElem);
            if (dropElem.className === "deletable") {
                dropElem.firstChild.checked = true;
            }
            for (var i = 0; i < event.currentTarget.parentNode.children.length - 1; i++) {
                event.currentTarget.parentNode.children[i].setAttribute("style", "border: 0px")
                event.currentTarget.parentNode.children[i].setAttribute("style", "border-bottom: 3px solid black")
            }
        }
    }
}

function editText() {
    var li = event.currentTarget.parentNode;
    eT = li.getElementsByClassName('task-text')[0];
    var editInput = document.createElement("input");
    var formInput = document.createElement("form");
    var previousText = eT.innerText;
    editInput.setAttribute("class", "edit-form")
    editInput.setAttribute("type", "text");
    editInput.setAttribute("value", previousText);
    formInput.appendChild(editInput);
    eT.setAttribute("style", "display: none;");
    formInput.addEventListener("submit", addEditTask);
    formInput.addEventListener("keyup", escapeEdit);
    li.appendChild(formInput);
    editInput.focus();
    editInput.select();
}

function escapeEdit() {
    if (event.key === "Escape") {
        eT = event.currentTarget.parentNode.getElementsByClassName('task-text')[0];
        eT.setAttribute("style", "display: block;");
        event.currentTarget.parentNode.getElementsByClassName("edit-form")[0].remove();
    }
}

function addEditTask() {
    event.preventDefault();
    var li = event.currentTarget.parentNode;
    var newText = li.getElementsByClassName("edit-form")[0].value;
    var textelement = document.createElement("p");
    var t = document.createTextNode(newText);
    textelement.setAttribute("class", "task-text");
    textelement.appendChild(t);
    textelement.addEventListener("click", editText);
    debugger;
    li.appendChild(textelement);
    li.getElementsByClassName("edit-form")[0].remove();
    if (li.className === "deletable") checkBox();
}

function checkBox() {
    var li = event.currentTarget.parentNode;
    var textnode = li.getElementsByClassName("task-text")[0];
    var ts = document.createElement("s");
    var inner = document.createTextNode(textnode.innerText);
    li.setAttribute("class", "deletable");
    textnode.innerText = "";
    ts.appendChild(inner);
    textnode.appendChild(ts);
    eC = li.firstChild;
    eC.removeEventListener("click", checkBox);
    eC.addEventListener("click", unCheckBox);
}

function unCheckBox() {
    var li = event.currentTarget.parentNode;
    var check = document.createElement('input');
    var textnode = li.getElementsByClassName("task-text")[0];
    var inner = document.createTextNode(textnode.firstChild.innerText);
    li.setAttribute("class", "");
    textnode.innerText = "";
    textnode.appendChild(inner);
    li.firstChild.remove();
    check.setAttribute("type", "checkbox");
    li.insertBefore(check, textnode);
    eC = li.firstChild;
    eC.removeEventListener("click", unCheckBox);
    eC.addEventListener("click", checkBox);
}