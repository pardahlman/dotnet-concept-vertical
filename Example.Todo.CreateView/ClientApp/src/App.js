import React, { Component } from 'react';

export default class App extends Component {
  constructor(){
    super();
    this.state = {title: ''}
  }

  onChange(event){
    this.setState({title: event.target.value});
  }

  handleChange(event) {
    if(event.keyCode !== 13) {
      return;
    }
    window.publish({title: this.state.title}, "example.todo.domain.commands", "createtodo");
  }

  render() {
    return (
      <div>
        <input onChange={e => this.onChange(e)} onKeyDown={e => this.handleChange(e)} value={this.state.title}/>
      </div>
    );
  }
}
