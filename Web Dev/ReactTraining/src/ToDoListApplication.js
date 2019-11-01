import React from 'react';
import { ToDoListForm } from '../src/ToDoListForm'
import { ToDoListItem } from '../src/ToDoListItem'

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

	render() {
		return <div>
			<ToDoListForm formSubmitted={(text) => this.onFormSubmit(text)} />

			{this.state.items.map((item, index) => {
				return <ToDoListItem formSubmitted={(text, key) => this.onFormEdit(text, key)} itemValue={item} key={index} value = {index}/>
			})}
		</div>
	}
}