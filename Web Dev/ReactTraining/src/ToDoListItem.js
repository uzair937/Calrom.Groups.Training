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
            return <form onSubmit={(e) => this.handleEdit(e)}><input type='text' onChange={(e) => this.onChange(e)} value={this.props.itemValue} className='textInput' /></form>
        } else {
            return <li onClick={(e) => this.handleEdit(e)}>
                <checkbox className='checkbox'></checkbox>
                {this.props.itemValue}
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
}