import * as SignalR from '@microsoft/signalr';

export const createSyncClient = (_userId: string, onNotify: (event: any) => void) => {
  const connection = new SignalR.HubConnectionBuilder()
    .withUrl('http://localhost:5000/hubs/sync')
    .withAutomaticReconnect()
    .build();

  connection.on('NotifyBookmarkChanged', onNotify);
  connection.on('NotifyCategoryChanged', onNotify);

  connection.start().catch(console.error);

  return connection;
};
