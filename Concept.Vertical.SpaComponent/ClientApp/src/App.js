import React, { Component } from 'react';
import register from "./registerService";


export class App extends Component {

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
    register('WeatherForecast', forecasts =>  this.setState({forecasts: forecasts, loading: false}))
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table'>
        <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
        </thead>
        <tbody>
        {forecasts.map(forecast =>
          <tr key={forecast.dateFormatted}>
            <td>{forecast.dateFormatted}</td>
            <td>{forecast.temperatureC}</td>
            <td>{forecast.temperatureF}</td>
            <td>{forecast.summary}</td>
          </tr>
        )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : App.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1>Weather forecast!</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }
}
