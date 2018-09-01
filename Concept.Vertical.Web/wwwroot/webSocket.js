const registrations = {};
const onConnectCallbacks = [];

window.onSocketConnect = (callback) => {
    onConnectCallbacks.push(callback);
}

const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl('/application')
    .build();



connection
    .start()
    .then(() => {

        connection.on('dataReceived', message => {
            var callbacks = registrations[message.type];
            if (!callbacks) {
                return;
            }
            callbacks.forEach(callback => callback(message.payload));
        });

        connection.on('connectionEstablished', (clientId) => {
            window.clientId = clientId;
            onConnectCallbacks.forEach(fn => fn());
        });
    });

window.register = (routingKey, callback) => {
    registrations[routingKey] = registrations[routingKey] || [];
    registrations[routingKey].push(callback);
};

window.publish = (msg, exchange, routingKey) => {
    connection.invoke('publishData', msg, exchange, routingKey);
}
