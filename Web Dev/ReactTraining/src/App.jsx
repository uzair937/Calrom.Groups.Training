import React from 'react';
import ReactDOM from 'react-dom';
import { ToDoListApplication } from '../src/ToDoListApplication'

var anchor = document.querySelector('.app-anchor');
ReactDOM.render(<ToDoListApplication />, anchor);