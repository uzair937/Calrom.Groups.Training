import React from 'react';
import {ToDoListForm} from '../src/ToDoListForm';
import {ToDoListItem} from '../src/ToDoListItem';

export class ToDoListApplication extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            items: ["does", "this", "work"]
        };
    }

    onFormSubmit(text) {
        this.setState({
            items: [...this.state.items, text]
        });
    }

    updateItem(updateObj) {
        let itemList = this.state.items;
        itemList[updateObj.itemId] = updateObj.newText;
        this.setState({
            item: itemList
        });
    }

    render() {
        return <div>
            <ToDoListForm formSubmitted={(text) => this.onFormSubmit(text)} />
            <ul>
                {this.state.items.map((item, index) => {
                    return <ToDoListItem key={index} itemValue={item} itemId={index} itemUpdated={(updateObj) => this.updateItem(updateObj)}/>
                })}
            </ul>
        </div>
    }
}