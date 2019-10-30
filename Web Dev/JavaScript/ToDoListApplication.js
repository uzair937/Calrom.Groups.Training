ToDoListApplication.prototype.editList = function (event) {
    var list = document.querySelector('ul');
    if (event.target.tagName === 'LI') {
        event.target.classList.toggle('striked');
    }
}

ToDoListApplication.prototype.strikeText = function (event) {
    if (event.target.tagName === 'LI') {
        event.target.classList.toggle('striked');
    }
}

function ToDoListApplication(elements) {
    this.elements = elements;
    this.form = new ToDoListForm(this);
}

ToDoListApplication.prototype.itemAdded = function () {
    var toDoListItem = new ToDoListItem(this);
    var item = toDoListItem.render();
    toDoListItem.setupEvents(item);

    var container = document.getElementById('container');
    container.className = this.elements.container;
    container.appendChild(item);
};

ToDoListApplication.prototype.itemRemoved = function () {
    var container = document.getElementById('container');
    var boxes = document.getElementsByName('checkbox');
    var div = document.getElementsByClassName(elements.mainDiv);

    for (var i = 0; i < div.length; i++) {
        if (div[i].contains(boxes[i])) {
            if (boxes[i].checked) {
                container.removeChild(div[i]);
            }
        }
    }
};

ToDoListApplication.prototype.itemEdited = function (item) {
    this.editForm.dispose();
    this.editForm = null;
};

function ToDoListForm(app) {
    this.elements = app.elements;
    this.app = app;
    this.setupEvents();
}

ToDoListForm.prototype.valid = function () {
    var list = document.getElementsByClassName(this.elements.toDoListItem);
    var div = document.getElementsByClassName(this.elements.mainDiv);
    var input = document.getElementById('textInput').value;

    if (div.length === 0) {
        return true;
    }

    if (div.length > 0) {
        for (var i = 0; i < div.length; i++) {
            if (div[i].contains(list[i])) {
                if (list[i].textContent === input) {
                    return false;
                } else {
                    return true;
                }
            }
        }
    }
};

ToDoListForm.prototype.onSubmit = function () {
    if (this.valid()) {
        this.app.itemAdded();
    }
};

ToDoListForm.prototype.onDelete = function () {
    this.app.itemRemoved();
};

ToDoListForm.prototype.setupEvents = function () {
    var submit = document.getElementById('submitButton');
    submit.addEventListener('click', this.onSubmit.bind(this), false);
    var del = document.getElementById('deleteButton');
    del.addEventListener('click', function () {
        this.onDelete()
    }, false);
};

function ToDoListItem(app) {
    this.elements = app.elements;
    this.editForm = new ToDoListEditItemForm(app);
}

ToDoListItem.prototype.setupEvents = function (item) {
    item.addEventListener('click', function () {
        this.editForm = new ToDoListEditItemForm(this);
        this.editForm.render(item);
    }, false);
};

ToDoListItem.prototype.render = function () {
    var div = document.createElement('div');
    div.className = this.elements.mainDiv;
    var toDoList = document.createElement('ul');
    toDoList.className = this.elements.toDoList;
    var toDoListItem = document.createElement('li');
    toDoListItem.className = this.elements.toDoListItem;
    var input = document.getElementById('textInput').value;
    var node = document.createTextNode(input);
    var checkbox = document.createElement('input');
    checkbox.type = 'checkbox';
    checkbox.name = this.elements.checkbox;
    toDoListItem.appendChild(node);
    toDoListItem.appendChild(checkbox);
    toDoList.appendChild(toDoListItem);
    div.appendChild(toDoList);
    return div;
};

function ToDoListEditItemForm(app) {
    this.app = app;
}

ToDoListEditItemForm.prototype.onDiscard = function () {
    debugger;
    var editBox = document.getElementsByName('editBox');
    var items = document.getElementsByClassName(pageElements.toDoListItem),
        tab = [],
        liIndex;

    for (var i = 0; i < items.length; i++) {
        tab.push(items[i].innerHTML);
    }

    if (editBox.length > 0) {
        for (var i = 0; i < editBox.length; i++) {
            items[i].removeChild(editBox[i]);
        }
    }
};

ToDoListEditItemForm.prototype.render = function (item) {
    var editBox = document.getElementsByName('editBox');

    if (editBox.length === 0) {
        var edit = document.createElement('input');
        edit.type = 'text';
        edit.name = 'editBox';
        item.appendChild(edit);
    }
    window.addEventListener('keypress', function (event) {
        if (event.keyCode === 13) {
            var self = new ToDoListEditItemForm(app);
            self.onSave();
        }
        if (event.keyCode === 27) {
            var self = new ToDoListEditItemForm(app);
            self.onDiscard();
        }
    });
}

ToDoListEditItemForm.prototype.onSave = function () {
    var items = document.getElementsByClassName(pageElements.toDoListItem),
        tab = [],
        liIndex;

    for (var i = 0; i < items.length; i++) {
        tab.push(items[i].innerHTML);
    }

    var editBox = document.getElementsByName('editBox');
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
};

var pageElements = {
    container: '.container',
    mainDiv: '.main-div',
    toDoList: '.to-do-list',
    toDoListItem: '.to-do-list-item',
    checkbox: 'checkbox'
};

var app = new ToDoListApplication(pageElements);