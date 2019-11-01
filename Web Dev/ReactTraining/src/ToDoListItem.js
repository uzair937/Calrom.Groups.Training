import React from 'react';

export class ToDoListItem extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            text: '',
            editing: false
        };
    }

    onChange(e) {
        let value = e.target.value;
        this.setState({
            text: value
        });
    }

    render() {
        if (this.state.editing) {
            return <form onSubmit={(e) => this.handleEdit(e)}><input type='text' onChange={(e) => this.onChange(e)} value={this.state.text} className='textInput' placeholder={this.props.itemValue} /></form>
        } else {
            return <li onClick={(e) => this.handleEdit(e)}>
                    {this.props.itemValue}
                    <button onClick={(e) => this.handleDelete(e)}>X</button>
                </li>
        }
    }

    handleEdit(e) {
        e.preventDefault();
        if (this.state.editing === true) {
            this.setState({
                editing: false
            });
            this.props.formSubmitted(this.state.text, this.props.value);
        } else {
            this.setState({
                editing: true
            });
        }
    }

    handleDelete(e) {
        e.preventDefault();
        this.props.deleteValue(this.props.value);
    }
}