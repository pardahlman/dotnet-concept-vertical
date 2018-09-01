import React, { Component } from 'react';

export default class App extends Component {
  constructor(){
    super();
    this.state = { Todos: [], loading: true };
    window.register('ListViewModel', list =>  this.setState({loading: false, ...list}));
    window.onSocketConnect(() =>
      window.publish({clientId: window.clientId}, 'example.todo.domain.queries', 'todolistquery')
    );
  }

  removeTodo(todo){
    console.log('remove', todo);
    window.publish({todoId: todo.Id}, 'example.todo.domain.commands', 'deletetodo');
  }

  render() {
    return (
      <div>
      <h1>Todo list</h1>
      {this.state.loading ? "Loading" : ''}
      <ul>
        {this.state.Todos.map((t, i) => <li key={i}>{t.Title} <a onClick={() => this.removeTodo(t)}>[x]</a></li>)}
      </ul>
      </div>
    );
  }
}
