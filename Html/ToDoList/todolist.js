window.addEventListener('load', run);
var checkNum;
var dragSrcEl = null;

function run() {
    checkNum = 0;
    var eventManager = new EventManager();
    eventManager.addSubmitEvents();
    var dragManager = new DragManager();
    dragManager.addDnDBottom();
}

function TaskBuilder() { }

TaskBuilder.prototype.createCheckbox = function () {
    var check = document.createElement('input');
    check.setAttribute("type", "checkbox");
    return check;
}

TaskBuilder.prototype.createContainer = function () {
    var li = document.createElement("div");
    li.setAttribute("draggable", "true");
    li.setAttribute("id", "box" + window.checkNum);
    return li;
}

TaskBuilder.prototype.createTextElement = function () {
    var textelement = document.createElement("p");
    var inputValue = document.getElementById("input-task").value;
    var t = document.createTextNode(inputValue);
    textelement.appendChild(t);
    textelement.setAttribute("class", "task-text");
    document.getElementById("input-task").value = '';
    return textelement;
}

TaskBuilder.prototype.buildTask = function (tEl, li, check) {
    li.appendChild(check);
    li.appendChild(tEl);
    return li;
}

function TaskManager() { }

TaskManager.prototype.deleteTasks = function () {
    event.preventDefault();
    var delList = document.getElementsByClassName("deletable");
    if (delList) {
        for (var i = delList.length - 1; i >= 0; i--) delList[i].remove();
    }
}

TaskManager.prototype.addTask = function () {
    event.preventDefault();
    var taskBuilder = new TaskBuilder();
    var taskManager = new TaskManager();
    var eventManager = new EventManager();
    var tEl = taskBuilder.createTextElement();
    var li = taskBuilder.createContainer();
    var check = taskBuilder.createCheckbox();
    li = taskBuilder.buildTask(tEl, li, check);
    eventManager.addListeners(li);
    taskManager.drawTask(li);
    window.checkNum++;
    return li;
}

TaskManager.prototype.drawTask = function (li) {
    var botel = document.getElementById("bottom-list");
    botel.insertAdjacentElement('beforebegin', li);
}

function EventManager() {
    this.el = document.getElementById("enter-task");
    if (!this.el) return;
    this.delel = document.getElementById("delete-task");
    this.botel = document.getElementById("bottom-list");
}

EventManager.prototype.pickDropMethod = function () {
    var dragOn, dropOn;
    for (var i = 0; i < event.currentTarget.parentNode.children.length; i++) {
        if (event.currentTarget === event.currentTarget.parentNode.children[i]) dropOn = i;
        if (window.dragSrcEl === event.currentTarget.parentNode.children[i]) dragOn = i;
    }
    return (dropOn - dragOn);
}

EventManager.prototype.addSubmitEvents = function () {
    var taskManager = new TaskManager();
    this.el.addEventListener("submit", taskManager.addTask);
    this.delel.addEventListener("submit", taskManager.deleteTasks);
}

EventManager.prototype.addListeners = function (li) {
    if (li) {
        var dragManager = new DragManager();
        var taskEditor = new TaskEditor();
        eC = li.firstChild;
        var eT = li.getElementsByClassName('task-text')[0];
        eC.addEventListener("click", this.checkBox);
        eT.addEventListener("click", taskEditor.textToInput);
        dragManager.addDnDHandlers(li);
    }
}

EventManager.prototype.escapeEdit = function () {
    if (event.key === "Escape") {
        eT = event.currentTarget.parentNode.getElementsByClassName('task-text')[0];
        eT.setAttribute("style", "display: block;");
        event.currentTarget.parentNode.getElementsByClassName("edit-form")[0].remove();
    }
}

EventManager.prototype.checkBox = function () {
    var li = event.currentTarget.parentNode;
    var textnode = li.getElementsByClassName("task-text")[0];
    var ts = document.createElement("s");
    var inner = document.createTextNode(textnode.innerText);
    li.setAttribute("class", "deletable");
    textnode.innerText = "";
    ts.appendChild(inner);
    textnode.appendChild(ts);
    eC = li.firstChild;
    eC.removeEventListener("click", this.checkBox);
    eC.addEventListener("click", this.unCheckBox);
}

EventManager.prototype.unCheckBox = function () {
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
    eC.removeEventListener("click", this.unCheckBox);
    eC.addEventListener("click", this.checkBox);
}

function DragManager() { }

DragManager.prototype.addDnDHandlers = function (li) {
    var dragEvents = new DragEvents();
    li.addEventListener("dragstart", dragEvents.drag);
    li.addEventListener("dragover", dragEvents.dragover);
    li.addEventListener("drop", dragEvents.drop);
}

DragManager.prototype.addDnDBottom = function () {
    var dragEvents = new DragEvents();
    var botel = document.getElementById("bottom-list");
    botel.addEventListener("dragover", dragEvents.dragover);
    botel.addEventListener("drop", dragEvents.drop);
}

function DragEvents() { }

DragEvents.prototype.dragover = function () {
    event.preventDefault();
    for (var i = 0; i < event.currentTarget.parentNode.children.length - 1; i++) {
        event.currentTarget.parentNode.children[i].setAttribute("style", "border: 0px")
        event.currentTarget.parentNode.children[i].setAttribute("style", "border-bottom: 3px solid black")
    }
    event.currentTarget.setAttribute("style", "border-bottom: 3px solid blue;")
    event.currentTarget.parentNode.children[event.currentTarget.parentNode.children.length - 1].setAttribute("style", "border-bottom: 0;");
}

DragEvents.prototype.drag = function () {
    window.dragSrcEl = event.currentTarget;
}

DragEvents.prototype.drop = function () {
    event.preventDefault();
    var eventManager = new EventManager();
    var dragManager = new DragManager();
    if (event.stopPropagation) event.stopPropagation();
    if (window.dragSrcEl != event.currentTarget) {
        if (eventManager.pickDropMethod() > 0) {
            event.currentTarget.parentNode.removeChild(dragSrcEl);
            var dropHTML = dragSrcEl;
            event.currentTarget.insertAdjacentElement('afterend', dropHTML);
        }
        else {
            event.currentTarget.parentNode.removeChild(dragSrcEl);
            var dropHTML = dragSrcEl;
            event.currentTarget.insertAdjacentElement('beforebegin', dropHTML);
        }
        var dropElem = event.currentTarget.previousSibling;
        dragManager.addDnDHandlers(dropElem);
        if (dropElem.className === "deletable") {
            dropElem.firstChild.checked = true;
        }
        for (var i = 0; i < event.currentTarget.parentNode.children.length - 1; i++) {
            event.currentTarget.parentNode.children[i].setAttribute("style", "border: 0px")
            event.currentTarget.parentNode.children[i].setAttribute("style", "border-bottom: 3px solid black")
        }
    }
}

function TaskEditor() { }

TaskEditor.prototype.textToInput = function () {
    event.preventDefault();
    var editManager = new EditManager();
    var formInput = editManager.createEditInput()
    editManager.addInputListeners(formInput);
    editManager.drawTask(formInput);
    editManager.focusOnInput(formInput);
}

TaskEditor.prototype.inputToText = function () {
    event.preventDefault();
    var eventManager = new EventManager();
    var editManager = new EditManager();
    var li = editManager.createNewTask()
    eventManager.addListeners(li);
    editManager.removeOldInput(li);
}

function EditManager() { }

EditManager.prototype.removeOldInput = function (li) {
    var eventManager = new EventManager();
    li.getElementsByClassName("edit-form")[0].remove();
    if (li.className === "deletable") eventManager.checkBox();
}

EditManager.prototype.createNewTask = function () {
    var li = event.currentTarget.parentNode;
    var eT = li.getElementsByClassName('task-text')[0];
    var newText = li.getElementsByClassName("edit-form")[0].value;
    eT.innerText = newText;
    eT.setAttribute("style", "display: block;");
    return li;
}

EditManager.prototype.focusOnInput = function (formInput) {
    var editInput = formInput.getElementsByClassName("edit-form")[0];
    editInput.focus();
    editInput.select();
}

EditManager.prototype.drawTask = function (newDiv) {
    event.currentTarget.parentNode.appendChild(newDiv);
}

EditManager.prototype.addInputListeners = function (formInput) {
    var eventManager = new EventManager();
    var taskEditor = new TaskEditor();
    formInput.addEventListener("submit", taskEditor.inputToText);
    formInput.addEventListener("keyup", eventManager.escapeEdit);
}

EditManager.prototype.createEditInput = function () {
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
    return formInput;
}
