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
            return <div>
                <li onClick={(e) => this.handleEdit(e)}>
                    {this.props.itemValue}
                </li>
                <button onClick={(e) => this.handleDelete(e)}>X</button>
            </div>
        }
    }

    handleEdit(e) {
        debugger;
        e.preventDefault();
        if (this.state.editing) {
            this.setState({
                editing: false
            });
            this.props.editSubmitted(this.state.text, this.props.value);
        } else {
            this.setState({
                editing: true
            });
        }
    }

    handleDelete(e) {
        e.preventDefault();
        if (!this.state.editing) {
            this.props.deleteValue(this.props.value);
        }
    }
}