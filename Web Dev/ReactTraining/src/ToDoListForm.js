import React from 'react';

export class ToDoListForm extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            text: ''
        };
    }

    buttonClicked(e) {
        e.preventDefault();
        this.props.formSubmitted(this.state.text);
        this.setState({
            text: ''
        });
    }

    onChange(e) {
        let value = e.target.value;
        this.setState({
            text: value
        });
    }

    render() {
        return <form>
            <input type='text' onChange={(e) => this.onChange(e)} value={this.state.text} className='textInput' placeholder='Type in here' />
            <button onClick={(e) => this.buttonClicked(e)}>Click me</button>
        </form>
    }
}