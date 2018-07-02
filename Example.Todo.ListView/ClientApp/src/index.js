import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';

const rootElement = document.getElementById('todo-list');

ReactDOM.render(<App />, rootElement);
