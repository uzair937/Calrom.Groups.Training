import React from 'react';
import ReactDOM from 'react-dom';

class ExampleComponent extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            
        };
    }
    
    render() {
        let variable = 'another test';

        return <div>
            This is an example div.
            <br />
            {this.props.test};            
            <br />
            {variable}
            <br />
            <ExampleSubComponent bold={true}/>
            <ExampleSubComponent bold={false}/>
        </div>
    }
}

class ExampleSubComponent extends React.Component {
    render() {
        return <div style={{'fontWeight' : this.props.bold ? 'bold' : ''}}>
            This is a sub component.
        </div>
    }
}

var anchor = document.querySelector('.app-anchor');
ReactDOM.render(<ExampleComponent test = 'this is a test' />, anchor)