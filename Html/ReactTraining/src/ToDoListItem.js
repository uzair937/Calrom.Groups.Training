import React from 'react';

export class ToDoListItem extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            type: "li",
            text: this.props.itemValue,
        };
    }

    textClicked(e) {
        e.preventDefault();
        if (this.state.type === "input") {
            let updateObj = {
                itemId: this.props.itemId,
                newText: this.state.text
            }
            this.props.itemUpdated(updateObj);  //cant access the function to change it
            this.setState({
                type: "li",
            });
        }
        else {
            this.setState({
                type: "input",
            });
        }
    }

    onChange(e) {
        let value = e.target.value;
        this.setState({
            text: value
        });
    }   

    //updated value not being returned to the list

    render() {
        if (this.state.type === "li") {
           return <li onClick={(e) => this.textClicked(e)}> {this.props.itemValue} </li>
        }
        return <li> <form onSubmit={(e) => this.textClicked(e)}> <input onChange={(e) => this.onChange(e)} type="text" value= {this.state.text}></input></form></li>
    
    }
}