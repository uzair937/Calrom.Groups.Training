import React from 'react';
import { ToDoListForm } from './ToDoListForm'
import { ToDoListItem } from './ToDoListItem'

export class ToDoListApplication extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            items: []
        };
    }

    onFormSubmit(text) {
        this.setState({
            items: [...this.state.items, text]
        });
    }

    onFormEdit(text, key) {
        var array = this.state.items;
        array[key] = text;

        this.setState({
            items: array
        });
    }

    onValueDeleted(key) {
        var array = this.state.items;
        array.splice(key, 1);

        this.setState({
            items: array
        });
    }

    render() {
        var i = 0;
        return <div>
            <ToDoListForm formSubmitted={(text) => this.onFormSubmit(text)} />

            {this.state.items.map((item, index) => {
                return <ToDoListItem editSubmitted={(text, key) => this.onFormEdit(text, key)} itemValue={item} key={index} value={i++} deleteValue={(key) => this.onValueDeleted(key)}/>
            })}
        </div>
    }
}