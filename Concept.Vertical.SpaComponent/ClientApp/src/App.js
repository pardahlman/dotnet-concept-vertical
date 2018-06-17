import React, { Component } from 'react';
import { register, publish} from "./registerService";

const sendToggleCommand = (e) => {
  console.log('stop');
  publish({"stop": true}, 'ToggleWeatherUpdates');
}

export class App extends Component {

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
    register('WeatherUpdated', weatherUpdated =>  this.setState({forecasts: weatherUpdated.Forecasts, loading: false}))
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
          <tr key={forecast.DateFormatted}>
            <td>{forecast.DateFormatted}</td>
            <td>{forecast.TemperatureC}</td>
            <td>{forecast.TemperatureF}</td>
            <td>{forecast.Summary}</td>
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
        <button onClick={sendToggleCommand}>Toggle updates</button>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }
}
