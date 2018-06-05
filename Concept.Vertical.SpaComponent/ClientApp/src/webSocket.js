import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

const registrations = {};

export const register = (routingKey, callback) => {
  registrations[routingKey] = registrations[routingKey] || [];
  registrations[routingKey].push(callback);
};

const connection = new HubConnectionBuilder()
  .configureLogging(LogLevel.Trace)
  .withUrl('http://localhost:5001/application')
  .build();

connection
  .start()
  .then(() => {
    connection.on('dataReceived', message => {
      var callbacks = registrations[message.routingKey];
      if(!callbacks){
        return;
      }
      callbacks.forEach(callback => callback(message.payload));
    })
  });