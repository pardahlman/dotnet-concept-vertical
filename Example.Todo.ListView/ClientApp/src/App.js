import React, { Component } from 'react';

export default class App extends Component {
  constructor(){
    super();
    this.state = { forecasts: [], loading: true };
    window.register('ListViewModel', list =>  this.setState({loading: false, ...list}));
    windows.publish()
  }

  render() {
    return (
      <div>
      <h1>Todo list</h1>
      {this.state.loading ? "Loading" : ''}
      </div>
    );
  }
}
