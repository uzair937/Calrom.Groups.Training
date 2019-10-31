window.addEventListener("load", addingTask);

function addTask(event) {
    event.preventDefault();
    var inputtext = document.getElementById("task-text").value;

    if (inputtext === "") {
        alert("Please Enter Field");
        return false;
    }

    var creatingTaskLine = document.createElement("div");
    var inputelement = document.createElement("input");
    var inputelement2 = document.createElement("checkbox");
    var textelement = document.createElement("h2");
    inputelement2.setAttribute("class", "check");
    creatingTaskLine.setAttribute("class", "task-box");
    inputelement.setAttribute("type", "checkbox");
    var inputtext = document.createTextNode(inputtext);
    textelement.appendChild(inputtext);
    creatingTaskLine.appendChild(inputelement);
    creatingTaskLine.appendChild(textelement);
    document.getElementById("ListofTasks").appendChild(creatingTaskLine);

    var inputelement2 = creatingTaskLine.firstChild;
    inputelement.addEventListener("click", checkTask);
}

function addingTask() {
    var add = document.getElementById("add-task");
    add.addEventListener('click', addTask);

    var del = document.getElementById("delete-task");
    del.addEventListener('click', deleteTask);
}

function deleteTask(event) {
    event.preventDefault();
    var container = document.getElementsByClassName("task-box deletable");
    if (container) {
        for (var i = 0; i < container.length; i++) {
            container[i].remove();
            i--;
        }
    }
}

function unCheckTask(event) {
    var newelement = event.target.parentElement;
    newelement.setAttribute("class", "task-box");
    checkbox = newelement.firstChild;
    checkbox.removeEventListener("click", unCheckTask);
    checkbox.addEventListener("click", checkTask);
}

function checkTask(event) {
    var currentElement = event.target.parentElement;
    currentElement.setAttribute("class", "task-box deletable");
    checkbox = currentElement.firstChild;
    checkbox.removeEventListener("click", checkTask);
    checkbox.addEventListener("click", unCheckTask);
}

