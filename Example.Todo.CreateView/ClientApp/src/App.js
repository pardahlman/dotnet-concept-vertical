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
    if(event.keyCode() !== 13) {
      return;
    }
    window.publish("createTodo", this.state.title);
  }

  render() {
    return (
      <div>
        <input onChange={e => this.onChange(e)} onKeyDown={e => this.handleChange(e)} value={this.state.title}/>
      </div>
    );
  }
}
