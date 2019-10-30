function insertData(event) {
    event.preventDefault();

    var input = document.getElementById('textInput').value;
    if (!input.match(/[a-z]/i)) {
        alert('Please enter an alphabetical or numerical value.');
    } else if (checkDupes()) {
        alert("Duplicate item.");
    } else {
        var div = document.createElement('div');
        div.className = 'mainDiv';
        var todo = document.createElement('ul');
        todo.className = 'todo';
        var list = document.createElement('li');
        list.className = 'list';
        var node = document.createTextNode(input);
        var checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.name = 'checkbox';
        list.appendChild(node);
        list.appendChild(checkbox);
        todo.appendChild(list);
        div.appendChild(todo);
        var container = document.getElementById('container');
        container.appendChild(div);
    }
}

function checkDupes() {
    debugger;
    var list = document.querySelectorAll('.list');
    var div = document.querySelectorAll('.mainDiv');
    var input = document.getElementById('textInput').value;

    for (var i = 0; i < div.length; i++) {
        if (div[i].contains(list[i])) {
            if (list[i].textContent === input) {
                return true;
            } else {
                return false
            }
        }
    }
}

function removeData() {
    var container = document.getElementById('container');
    var boxes = document.getElementsByName('checkbox');
    var div = document.querySelectorAll('.mainDiv');

    for (var i = 0; i < div.length; i++) {
        if (div[i].contains(boxes[i])) {
            if (boxes[i].checked) {
                container.removeChild(div[i]);
            }
        }
    }
}

function editList(event) {
    var items = document.querySelectorAll('.list'),
        tab = [],
        liIndex;

    for (var i = 0; i < items.length; i++) {
        tab.push(items[i].innerHTML);
    }

    for (var j = 0; j < items.length; j++) {
        var item = items[j];
        var editBox = document.querySelectorAll('.editBox');

        item.onclick = function () {
            if (editBox.length === 0) {
                var edit = document.createElement('input');
                edit.type = 'text';
                edit.className = 'editBox';
                this.appendChild(edit);
            }
        };
    }
}

function updateList() {
    var items = document.querySelectorAll('.list'),
        tab = [],
        liIndex;

    for (var i = 0; i < items.length; i++) {
        tab.push(items[i].innerHTML);
    }

    var editBox = document.querySelectorAll('.editBox');
    for (var i = 0; i < editBox.length; i++) {
        var edit = editBox[i];
    }
    for (var j = 0; j < items.length; j++) {
        var item = items[j];
        var checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.name = 'checkbox';
        item.innerHTML = edit.value;
        item.appendChild(checkbox);
    }
}

function cancelUpdate() {
    debugger;
    var editBox = document.querySelectorAll('.editBox');
    var items = document.querySelectorAll('.list'),
        tab = [],
        liIndex;

    if (editBox.length > 0) {
            for (var i = 0; i < editBox.length; i++) {
                var edit = editBox[i];
            }

            for (var i = 0; i < items.length; i++) {
                tab.push(items[i].innerHTML);
                items[i].removeChild(edit);
            }
        }
    }

    function clickEvents() {
        var submit = document.getElementById('submitButton');
        submit.addEventListener('click', insertData);
        var del = document.getElementById('deleteButton');
        del.addEventListener('click', removeData);
    }

    function keyEvents(event) {
        if (event.keyCode === 13) {
            updateList();
        }
        if (event.keyCode === 27) {
            cancelUpdate();
        }
    }

    function strikeText(event) {
        if (event.target.tagName === 'LI') {
            event.target.classList.toggle('striked');
        }
    }

    //window.addEventListener('click', strikeText);
    window.addEventListener('click', editList);
    window.addEventListener('keypress', keyEvents);
    window.addEventListener('load', clickEvents);

    // function ToDoListApplication(elements) {
    //     this.elements = elements;
    // }

    // ToDoListApplication.prototype.itemAdded = function(item) {

    // };

    // ToDoListApplication.prototype.itemEdited = function(item) {
    //     this.editForm.dispose();
    //     this.editForm = null;
    // };

    // ToDoListApplication.prototype.setupEvents = function() {

    // };

    // ToDoListApplication.prototype.render = function() {

    // };

    // function ToDoListForm(app) {
    //     this.app = app;
    // }

    // ToDoListForm.prototype.onSubmit = function() {
    //     if (valid) {
    //         this.app.itemAdded(item);
    //     }
    // };

    // ToDoListForm.setupEvents = function() {

    // };

    // ToDoListForm.prototype.render = function() {

    // };

    // function ToDoListEditItemForm(app) {
    //     this.app = app;
    // }

    // ToDoListEditItemForm.prototype.onDiscard = function() {

    // };

    // ToDoListEditItemForm.prototype.onSave = function() {
    //     if (valid) {
    //         this.app.itemEdited(item);
    //     }
    // };

    // ToDoListEditItemForm.setupEvents = function() {
    //     // addeventListener click
    // };

    // ToDoListEditItemForm.dispose = function() {
    //     // remove event listener click
    // };

    // ToDoListEditItemForm.prototype.render = function() {

    // };


    // var pageElements = {
    //     toDoListItem: ".to-do-list-item"
    // };

    // var app = new ToDoListApplication(pageElements);
    // app.render();